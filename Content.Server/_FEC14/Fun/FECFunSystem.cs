using Content.Server.Chat.Managers;
using Content.Server.Chat.Systems;
using Content.Server.GameTicking;
using Content.Server.Popups;
using Content.Shared._FEC14.CCVar;
using Content.Shared._RMC14.Marines;
using Content.Shared._RMC14.Marines.Announce;
using Content.Shared.Chat;
using Content.Shared.GameTicking;
using Content.Shared.Mobs;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Configuration;
using Robust.Shared.Player;
using Robust.Shared.Random;

namespace Content.Server._FEC14.Fun;

public sealed class FECFunSystem : EntitySystem
{
    [Dependency] private readonly SharedMarineAnnounceSystem _announce = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly IChatManager _chatManager = default!;
    [Dependency] private readonly ChatSystem _chat = default!;
    [Dependency] private readonly PopupSystem _popup = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly GameTicker _ticker = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    private const int DeathQuotes = 20;
    private const int AresTips = 15;
    private const int PrankLines = 15;

    private static readonly TimeSpan AresTipDelay = TimeSpan.FromMinutes(3);
    private static readonly SoundSpecifier HonkSound = new SoundPathSpecifier("/Audio/Items/bikehorn.ogg");

    private readonly List<(EntityUid Uid, TimeSpan End)> _spinning = new();
    private bool _tipSent;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<MobStateChangedEvent>(OnMobStateChanged);
        SubscribeLocalEvent<RoundRestartCleanupEvent>(OnCleanup);
    }

    private void OnCleanup(RoundRestartCleanupEvent ev)
    {
        _tipSent = false;
        _spinning.Clear();
    }

    private void OnMobStateChanged(MobStateChangedEvent ev)
    {
        if (ev.NewMobState != MobState.Dead ||
            !_cfg.GetCVar(FECCVars.FunEnabled) ||
            !HasComp<MarineComponent>(ev.Target) ||
            !TryComp(ev.Target, out ActorComponent? actor) ||
            !_random.Prob(Math.Clamp(_cfg.GetCVar(FECCVars.FunDeathQuoteChance), 0, 1)))
        {
            return;
        }

        var quote = Loc.GetString($"fec-fun-epitaph-{_random.Next(1, DeathQuotes + 1)}");
        var message = Loc.GetString("fec-fun-epitaph", ("text", quote));
        _chatManager.ChatMessageToOne(ChatChannel.Server, message, message, default, false, actor.PlayerSession.Channel, Color.FromHex("#B0A0D0"));
    }

    public override void Update(float frameTime)
    {
        if (_spinning.Count > 0)
            UpdateSpinning(frameTime);

        if (_tipSent || _ticker.RunLevel != GameRunLevel.InRound || _ticker.RoundDuration() < AresTipDelay)
            return;

        _tipSent = true;
        if (!_cfg.GetCVar(FECCVars.FunEnabled) || !_cfg.GetCVar(FECCVars.FunRoundStartQuote))
            return;

        var tip = Loc.GetString($"fec-fun-ares-tip-{_random.Next(1, AresTips + 1)}");
        _announce.AnnounceARES(null, Loc.GetString("fec-fun-ares-tip", ("text", tip)));
    }

    private void UpdateSpinning(float frameTime)
    {
        var now = _ticker.RoundDuration();
        for (var i = _spinning.Count - 1; i >= 0; i--)
        {
            var (uid, end) = _spinning[i];
            if (TerminatingOrDeleted(uid) || now >= end)
            {
                _spinning.RemoveAt(i);
                continue;
            }

            var xform = Transform(uid);
            _transform.SetLocalRotation(uid, xform.LocalRotation + Angle.FromDegrees(720 * frameTime), xform);
        }
    }

    public bool TryPrank(EntityUid target, string prank)
    {
        switch (prank)
        {
            case "say":
                _chat.TrySendInGameICMessage(target, Loc.GetString($"fec-prank-line-{_random.Next(1, PrankLines + 1)}"), InGameICChatType.Speak, false, ignoreActionBlocker: true);
                return true;
            case "honk":
                _audio.PlayPvs(HonkSound, target);
                Spawn("BikeHorn", _transform.GetMapCoordinates(target));
                _popup.PopupEntity(Loc.GetString("fec-prank-honk-popup"), target, target);
                return true;
            case "balloon":
                Spawn("Balloon", _transform.GetMapCoordinates(target));
                _popup.PopupEntity(Loc.GetString("fec-prank-balloon-popup"), target, target);
                return true;
            case "spin":
                _spinning.Add((target, _ticker.RoundDuration() + TimeSpan.FromSeconds(3)));
                _popup.PopupEntity(Loc.GetString("fec-prank-spin-popup"), target, target);
                return true;
            default:
                return false;
        }
    }
}
