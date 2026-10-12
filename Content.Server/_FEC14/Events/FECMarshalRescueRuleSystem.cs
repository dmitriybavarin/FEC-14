using System.Linq;
using Content.Server._FEC14.GameTicking;
using Content.Server._RMC14.Rules.DistressSignal;
using Content.Server.Chat.Managers;
using Content.Server.Explosion.EntitySystems;
using Content.Server.GameTicking;
using Content.Server.GameTicking.Rules;
using Content.Server.Humanoid.Systems;
using Content.Server.Mind;
using Content.Server.Players.PlayTimeTracking;
using Content.Server.Preferences.Managers;
using Content.Server.Station.Systems;
using Content.Shared._RMC14.Marines;
using Content.Shared._RMC14.Marines.Announce;
using Content.Shared._RMC14.Rules;
using Content.Shared._RMC14.Weapons.Ranged.IFF;
using Content.Shared._RMC14.Xenonids;
using Content.Shared._RMC14.Xenonids.Construction.Nest;
using Content.Shared.Cuffs.Components;
using Content.Shared.GameTicking;
using Content.Shared.GameTicking.Components;
using Content.Shared.Ghost;
using Content.Shared.Inventory;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Preferences;
using Content.Shared.Roles;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.Player;
using Robust.Shared.Random;

namespace Content.Server._FEC14.Events;

public sealed class FECMarshalRescueRuleSystem : GameRuleSystem<FECMarshalRescueRuleComponent>
{
    [Dependency] private readonly IChatManager _chat = default!;
    [Dependency] private readonly CMDistressSignalRuleSystem _distress = default!;
    [Dependency] private readonly FECEventsSystem _events = default!;
    [Dependency] private readonly ExplosionSystem _explosion = default!;
    [Dependency] private readonly GunIFFSystem _iff = default!;
    [Dependency] private readonly InventorySystem _inventory = default!;
    [Dependency] private readonly SharedMarineAnnounceSystem _announce = default!;
    [Dependency] private readonly MindSystem _mind = default!;
    [Dependency] private readonly MobStateSystem _mobState = default!;
    [Dependency] private readonly IPlayerManager _players = default!;
    [Dependency] private readonly PlayTimeTrackingSystem _playTime = default!;
    [Dependency] private readonly IServerPreferencesManager _prefs = default!;
    [Dependency] private readonly RandomHumanoidSystem _randomHumanoid = default!;
    [Dependency] private readonly SharedRoleSystem _roles = default!;
    [Dependency] private readonly StationSpawningSystem _stationSpawning = default!;
    [Dependency] private readonly FECRoundSummarySystem _summary = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    protected override void Started(EntityUid uid, FECMarshalRescueRuleComponent comp, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.Started(uid, comp, gameRule, args);

        if (!TrySpawnMarshal(comp))
        {
            Log.Info("FEC marshal rescue event could not find a volunteer or a planet spawn point");
            _events.ReportOutcome(uid, "failed");
            comp.Stage = FECMarshalStage.Finished;
            GameTicker.EndGameRule(uid, gameRule);
            return;
        }

        comp.Stage = FECMarshalStage.Monitoring;
        comp.NextCheck = Timing.CurTime + comp.CheckInterval;
        _announce.AnnounceARES(null, Loc.GetString("fec-marshal-ares-beacon", ("name", comp.MarshalName)));
    }

    protected override void ActiveTick(EntityUid uid, FECMarshalRescueRuleComponent comp, GameRuleComponent gameRule, float frameTime)
    {
        base.ActiveTick(uid, comp, gameRule, frameTime);

        var now = Timing.CurTime;
        switch (comp.Stage)
        {
            case FECMarshalStage.Monitoring when now >= comp.NextCheck:
                CheckMarshal(uid, comp, gameRule);
                break;
            case FECMarshalStage.ReportSent when now >= comp.NextStage:
                _announce.AnnounceHighCommand(Loc.GetString("fec-marshal-hc-reply"));
                comp.Stage = FECMarshalStage.Inbound;
                comp.NextStage = now + RandomTime(comp.InboundMin, comp.InboundMax);
                break;
            case FECMarshalStage.Inbound when now >= comp.NextStage:
                _announce.AnnounceARES(null, Loc.GetString("fec-marshal-ares-unknown-vessel"));
                comp.Stage = FECMarshalStage.AirDefense;
                comp.NextStage = now + comp.AirDefenseDelay;
                break;
            case FECMarshalStage.AirDefense when now >= comp.NextStage:
                Crash(uid, comp, gameRule);
                break;
            case FECMarshalStage.Assault when now >= comp.NextAssaultCheck:
                comp.NextAssaultCheck = now + TimeSpan.FromSeconds(2);
                CheckAssault(uid, comp, gameRule);
                break;
        }
    }

    private bool TrySpawnMarshal(FECMarshalRescueRuleComponent comp)
    {
        var candidates = new List<ICommonSession>();
        foreach (var session in _players.Sessions)
        {
            if (session.Status != SessionStatus.InGame ||
                _prefs.GetPreferencesOrNull(session.UserId)?.SelectedCharacter is not HumanoidCharacterProfile profile ||
                !profile.AntagPreferences.Contains(comp.Preference))
            {
                continue;
            }

            if (session.AttachedEntity is { } attached && !HasComp<GhostComponent>(attached))
                continue;

            candidates.Add(session);
        }

        if (candidates.Count == 0 || !_events.TryGetPlanetSpawn(out var coordinates))
            return false;

        var fresh = candidates.Where(c => !_events.IsOnRoleCooldown(c.UserId, comp.Preference)).ToList();
        var player = RobustRandom.Pick(fresh.Count > 0 ? fresh : candidates);
        _events.RecordRole(player.UserId, comp.Preference);
        if (GameTicker.PlayerGameStatuses.GetValueOrDefault(player.UserId) != PlayerGameStatus.JoinedGame)
            GameTicker.PlayerJoinGame(player, true);

        var profileToUse = GameTicker.GetPlayerProfile(player);
        var mob = _stationSpawning.SpawnPlayerMob(coordinates, comp.Job, profileToUse, null);
        var mind = _mind.TryGetMind(player.UserId, out var existing) ? existing.Value.Owner : _mind.CreateMind(player.UserId).Owner;
        _mind.TransferTo(mind, mob);
        _roles.MindAddJobRole(mind, jobPrototype: comp.Job);
        _playTime.PlayerRolesChanged(player);
        RaiseLocalEvent(mob, new PlayerSpawnCompleteEvent(mob, player, comp.Job, false, true, 0, default, profileToUse), true);

        comp.Marshal = mob;
        comp.MarshalName = Name(mob);
        _chat.DispatchServerMessage(player, Loc.GetString("fec-marshal-brief"));
        return true;
    }

    private void CheckMarshal(EntityUid uid, FECMarshalRescueRuleComponent comp, GameRuleComponent gameRule)
    {
        var status = GetMarshalStatus(comp);
        switch (status)
        {
            case "evacuated":
                _announce.AnnounceARES(null, Loc.GetString("fec-marshal-ares-evacuated", ("name", comp.MarshalName)));
                _announce.AnnounceHighCommand(Loc.GetString("fec-marshal-hc-thanks"));
                _summary.AddLine(Loc.GetString("fec-marshal-summary-saved", ("name", comp.MarshalName)));
                _events.ReportOutcome(uid, "saved");
                comp.Stage = FECMarshalStage.Finished;
                GameTicker.EndGameRule(uid, gameRule);
                return;
            case "alive":
                _announce.AnnounceARES(null, Loc.GetString("fec-marshal-ares-alive", ("name", comp.MarshalName), ("minutes", (int) comp.CheckInterval.TotalMinutes)));
                comp.NextCheck = Timing.CurTime + comp.CheckInterval;
                return;
            default:
                _announce.AnnounceARES(null, Loc.GetString("fec-marshal-ares-report", ("name", comp.MarshalName), ("status", Loc.GetString($"fec-marshal-status-{status}"))));
                _summary.AddLine(Loc.GetString("fec-marshal-summary-lost", ("name", comp.MarshalName), ("status", Loc.GetString($"fec-marshal-status-{status}"))));
                _events.ReportOutcome(uid, "lost");
                comp.Stage = FECMarshalStage.ReportSent;
                comp.NextStage = Timing.CurTime + RandomTime(comp.HighCommandDelayMin, comp.HighCommandDelayMax);
                return;
        }
    }

    private string GetMarshalStatus(FECMarshalRescueRuleComponent comp)
    {
        if (comp.Marshal is not { } marshal || TerminatingOrDeleted(marshal))
            return "missing";

        if (!TryComp(marshal, out MobStateComponent? mobState) || _mobState.IsDead(marshal, mobState))
            return "dead";

        if (HasComp<XenoNestedComponent>(marshal) ||
            TryComp(marshal, out CuffableComponent? cuffable) && cuffable.CuffedHandCount > 0)
        {
            return "detained";
        }

        return IsOnShip(marshal) ? "evacuated" : "alive";
    }

    private bool IsOnShip(EntityUid uid)
    {
        var xform = Transform(uid);
        return xform.GridUid is { } grid && HasComp<AlmayerComponent>(grid) ||
               xform.MapUid is { } map && HasComp<AlmayerComponent>(map);
    }

    private void Crash(EntityUid uid, FECMarshalRescueRuleComponent comp, GameRuleComponent gameRule)
    {
        if (!_events.TryGetShipSpawn(out var coordinates))
        {
            _announce.AnnounceARES(null, Loc.GetString("fec-marshal-ares-vessel-lost"));
            _events.ReportOutcome(uid, "no-squad");
            comp.Stage = FECMarshalStage.Finished;
            GameTicker.EndGameRule(uid, gameRule);
            return;
        }

        _announce.AnnounceARES(null, Loc.GetString("fec-marshal-ares-air-defense-failed"));
        _explosion.QueueExplosion(_transform.ToMapCoordinates(coordinates), comp.ExplosionType, comp.ExplosionIntensity, 5f, 40f, null);

        var size = Math.Clamp(_players.PlayerCount / Math.Max(1, comp.PlayersPerMember), comp.MinSquad, comp.MaxSquad);
        for (var i = 0; i < size; i++)
        {
            var offset = RobustRandom.NextVector2(2.5f);
            var spawnAt = coordinates.Offset(offset);
            var member = _randomHumanoid.SpawnRandomHumanoid(i == 0 ? comp.LeaderSettings : comp.MemberSettings, spawnAt, string.Empty);
            comp.Squad.Add(member);
        }

        _announce.AnnounceARES(null, Loc.GetString("fec-marshal-ares-boarding"));
        comp.Stage = FECMarshalStage.Assault;
        comp.ClaimDeadline = Timing.CurTime + comp.UnclaimedTimeout;
    }

    private void CheckAssault(EntityUid uid, FECMarshalRescueRuleComponent comp, GameRuleComponent gameRule)
    {
        comp.Squad.RemoveWhere(member => TerminatingOrDeleted(member));

        var alive = 0;
        var aliveOnShip = 0;
        var claimed = 0;
        foreach (var member in comp.Squad)
        {
            FixFaction(member, comp);

            var hasPlayer = TryComp(member, out ActorComponent? actor);
            if (hasPlayer)
            {
                claimed++;
                comp.AnyClaimed = true;
                if (comp.Briefed.Add(member))
                    _chat.DispatchServerMessage(actor!.PlayerSession, Loc.GetString("fec-marshal-punitive-brief"));
            }

            if (!TryComp(member, out MobStateComponent? mobState) || _mobState.IsDead(member, mobState))
                continue;

            alive++;
            if (IsOnShip(member))
                aliveOnShip++;
        }

        if (!comp.AnyClaimed && Timing.CurTime > comp.ClaimDeadline)
        {
            foreach (var member in comp.Squad)
            {
                QueueDel(member);
            }

            _announce.AnnounceARES(null, Loc.GetString("fec-marshal-ares-vessel-lost"));
            _summary.AddLine(Loc.GetString("fec-marshal-summary-no-squad"));
            _events.ReportOutcome(uid, "no-squad");
            Finish(uid, comp, gameRule);
            return;
        }

        if (comp.AnyClaimed && alive == 0)
        {
            _announce.AnnounceARES(null, Loc.GetString("fec-marshal-ares-squad-destroyed"));
            _announce.AnnounceHighCommand(Loc.GetString("fec-marshal-hc-squad-destroyed"));
            _summary.AddLine(Loc.GetString("fec-marshal-summary-repelled"));
            _events.ReportOutcome(uid, "repelled");
            Finish(uid, comp, gameRule);
            return;
        }

        if (claimed == 0 || aliveOnShip == 0 || CountCrewOnShip(comp) > 0)
            return;

        var key = XenosAlive() ? "fec-marshal-end-punished-xenos" : "fec-marshal-end-punished";
        _summary.AddLine(Loc.GetString("fec-marshal-summary-punished"));
        _events.ReportOutcome(uid, "punished");
        _distress.FECEndRound(DistressSignalRuleResult.MinorMarineVictory, key);
        Finish(uid, comp, gameRule);
    }

    private void FixFaction(EntityUid member, FECMarshalRescueRuleComponent comp)
    {
        _iff.SetUserFaction(member, comp.Faction);
        if (_inventory.TryGetSlotEntity(member, "id", out var card) && TryComp(card, out ItemIFFComponent? itemIff))
            _iff.SetIdFaction((card.Value, itemIff), comp.Faction);
    }

    private int CountCrewOnShip(FECMarshalRescueRuleComponent comp)
    {
        var count = 0;
        var query = EntityQueryEnumerator<ActorComponent, MobStateComponent>();
        while (query.MoveNext(out var uid, out _, out var mobState))
        {
            if (comp.Squad.Contains(uid) || _mobState.IsDead(uid, mobState) || HasComp<GhostComponent>(uid) || !IsOnShip(uid))
                continue;

            count++;
        }

        return count;
    }

    private bool XenosAlive()
    {
        var query = EntityQueryEnumerator<XenoComponent, MobStateComponent>();
        while (query.MoveNext(out var uid, out _, out var mobState))
        {
            if (!_mobState.IsDead(uid, mobState))
                return true;
        }

        return false;
    }

    private void Finish(EntityUid uid, FECMarshalRescueRuleComponent comp, GameRuleComponent gameRule)
    {
        comp.Stage = FECMarshalStage.Finished;
        GameTicker.EndGameRule(uid, gameRule);
    }

    private TimeSpan RandomTime(TimeSpan min, TimeSpan max)
    {
        var spread = Math.Max(0, (max - min).TotalSeconds);
        return min + TimeSpan.FromSeconds(RobustRandom.NextDouble() * spread);
    }
}
