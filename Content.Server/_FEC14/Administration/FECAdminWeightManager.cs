using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Content.Server.Administration.Managers;
using Content.Server.Database;
using Content.Shared._FEC14.CCVar;
using Content.Shared.Administration;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Network;
using Robust.Shared.Player;

namespace Content.Server._FEC14.Administration;

public sealed class FECAdminWeightManager
{
    public const int NotAdmin = -1;
    public const int HostWeight = int.MaxValue;

    [Dependency] private readonly IAdminManager _admin = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly IServerDbManager _db = default!;
    [Dependency] private readonly ILogManager _log = default!;
    [Dependency] private readonly IPlayerManager _players = default!;

    private readonly Dictionary<int, FECRankWeight> _weights = new();
    private string? _loadedPath;
    private DateTime _loadedTime;

    public int GetRankWeight(int? rankId)
    {
        if (rankId == null)
            return 0;

        EnsureLoaded();
        return _weights.TryGetValue(rankId.Value, out var entry) ? entry.Weight : 0;
    }

    public IReadOnlyDictionary<int, int> GetRankWeights()
    {
        EnsureLoaded();
        return _weights.ToDictionary(p => p.Key, p => p.Value.Weight);
    }

    public void SetRankWeight(int rankId, string name, int weight)
    {
        EnsureLoaded();
        _weights[rankId] = new FECRankWeight { Id = rankId, Name = name, Weight = weight };
        Save();
    }

    public void RemoveRank(int rankId)
    {
        EnsureLoaded();
        if (_weights.Remove(rankId))
            Save();
    }

    public int GetWeight(ICommonSession session)
    {
        var data = _admin.GetAdminData(session, includeDeAdmin: true);
        if (data == null)
            return NotAdmin;

        if (data.HasFlag(AdminFlags.Host, includeDeAdmin: true))
            return HostWeight;

        return GetRankWeight((_admin as AdminManager)?.FECGetRankId(session));
    }

    public async Task<int> GetWeightAsync(NetUserId userId)
    {
        if (_players.TryGetSessionById(userId, out var session) && _admin.GetAdminData(session, includeDeAdmin: true) != null)
            return GetWeight(session);

        var admin = await _db.GetAdminDataForAsync(userId);
        if (admin == null)
            return NotAdmin;

        var flags = AdminFlagsHelper.NamesToFlags(admin.Flags.Where(f => !f.Negative).Select(f => f.Flag));
        if (admin.AdminRank != null)
            flags |= AdminFlagsHelper.NamesToFlags(admin.AdminRank.Flags.Select(f => f.Flag));

        if ((flags & AdminFlags.Host) != 0)
            return HostWeight;

        return GetRankWeight(admin.AdminRankId);
    }

    public bool CanTarget(ICommonSession actor, ICommonSession target)
    {
        var targetWeight = GetWeight(target);
        return targetWeight == NotAdmin || actor == target || GetWeight(actor) >= targetWeight;
    }

    public async Task<bool> CanTargetAsync(NetUserId actor, NetUserId target)
    {
        if (actor == target)
            return true;

        var targetWeight = await GetWeightAsync(target);
        return targetWeight == NotAdmin || await GetWeightAsync(actor) >= targetWeight;
    }

    private void EnsureLoaded()
    {
        var path = _cfg.GetCVar(FECCVars.AdminWeightsFile);
        var time = File.Exists(path) ? File.GetLastWriteTimeUtc(path) : DateTime.MinValue;
        if (path == _loadedPath && time == _loadedTime)
            return;

        _loadedPath = path;
        _loadedTime = time;
        _weights.Clear();

        if (time == DateTime.MinValue)
            return;

        try
        {
            var list = JsonSerializer.Deserialize<List<FECRankWeight>>(File.ReadAllText(path)) ?? new();
            foreach (var entry in list)
            {
                _weights[entry.Id] = entry;
            }
        }
        catch (Exception e)
        {
            _log.GetSawmill("fec.admin.weight").Error($"Failed to read {path}: {e.Message}");
        }
    }

    private void Save()
    {
        var path = _cfg.GetCVar(FECCVars.AdminWeightsFile);
        var dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(dir))
            Directory.CreateDirectory(dir);

        var list = _weights.Values.OrderByDescending(w => w.Weight).ThenBy(w => w.Id).ToList();
        File.WriteAllText(path, JsonSerializer.Serialize(list, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        }));

        _loadedPath = path;
        _loadedTime = File.GetLastWriteTimeUtc(path);
    }

    private sealed class FECRankWeight
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Weight { get; set; }
    }
}
