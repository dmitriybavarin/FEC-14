using System.IO;
using System.Linq;
using System.Text.Json;
using Content.Server.Administration.Logs;
using Content.Server.Chat.Managers;
using Content.Server.Players.JobWhitelist;
using Content.Shared._FEC14.CCVar;
using Content.Shared.Database;
using Robust.Shared.Configuration;
using Robust.Shared.Network;
using Robust.Shared.Timing;

namespace Content.Server._FEC14.JobWhitelist;

public sealed class FECTempJobWhitelistSystem : EntitySystem
{
    [Dependency] private readonly IAdminLogManager _adminLog = default!;
    [Dependency] private readonly IChatManager _chat = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly JobWhitelistManager _jobWhitelist = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    private static readonly TimeSpan CheckInterval = TimeSpan.FromMinutes(1);

    private readonly List<FECTempJobWhitelist> _entries = new();
    private string? _loadedPath;
    private TimeSpan _nextCheck;

    public override void Update(float frameTime)
    {
        if (_timing.RealTime < _nextCheck)
            return;

        _nextCheck = _timing.RealTime + CheckInterval;
        ExpireDue();
    }

    public DateTime? GetExpiry(Guid userId, string jobId)
    {
        EnsureLoaded();
        return _entries.FirstOrDefault(e => e.UserId == userId && e.JobId == jobId)?.ExpiresAt;
    }

    public void Add(NetUserId userId, string username, string jobId, TimeSpan duration, string admin)
    {
        EnsureLoaded();
        _entries.RemoveAll(e => e.UserId == userId.UserId && e.JobId == jobId);
        _entries.Add(new FECTempJobWhitelist
        {
            UserId = userId.UserId,
            Username = username,
            JobId = jobId,
            ExpiresAt = DateTime.UtcNow + duration,
            AddedBy = admin,
        });
        Save();
        _jobWhitelist.AddWhitelist(userId, jobId);
    }

    public bool Forget(Guid userId, string jobId)
    {
        EnsureLoaded();
        if (_entries.RemoveAll(e => e.UserId == userId && e.JobId == jobId) == 0)
            return false;

        Save();
        return true;
    }

    public static bool TryParseDuration(string text, out TimeSpan duration)
    {
        duration = TimeSpan.Zero;
        text = text.Trim().ToLowerInvariant();
        if (text.Length == 0)
            return false;

        var unit = text[^1];
        var number = char.IsDigit(unit) ? text : text[..^1];
        if (!double.TryParse(number, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var value) || value <= 0)
            return false;

        duration = unit switch
        {
            'm' or 'м' => TimeSpan.FromMinutes(value),
            'h' or 'ч' => TimeSpan.FromHours(value),
            'd' or 'д' => TimeSpan.FromDays(value),
            'w' or 'н' => TimeSpan.FromDays(value * 7),
            _ when char.IsDigit(unit) => TimeSpan.FromMinutes(value),
            _ => TimeSpan.Zero,
        };

        return duration > TimeSpan.Zero && duration < TimeSpan.FromDays(3650);
    }

    private void ExpireDue()
    {
        EnsureLoaded();
        var now = DateTime.UtcNow;
        var expired = _entries.Where(e => e.ExpiresAt <= now).ToList();
        if (expired.Count == 0)
            return;

        foreach (var entry in expired)
        {
            _jobWhitelist.RemoveWhitelist(new NetUserId(entry.UserId), entry.JobId);
            _entries.Remove(entry);
            _adminLog.Add(LogType.Action, LogImpact.Medium, $"Temporary job whitelist {entry.JobId} of {entry.Username} expired");
            _chat.SendAdminAlert(Loc.GetString("fec-temp-whitelist-expired", ("player", entry.Username), ("job", entry.JobId)));
        }

        Save();
    }

    private void EnsureLoaded()
    {
        var path = _cfg.GetCVar(FECCVars.TempJobWhitelistsFile);
        if (path == _loadedPath)
            return;

        _loadedPath = path;
        _entries.Clear();
        if (!File.Exists(path))
            return;

        try
        {
            _entries.AddRange(JsonSerializer.Deserialize<List<FECTempJobWhitelist>>(File.ReadAllText(path)) ?? new());
        }
        catch (Exception e)
        {
            Log.Error($"Failed to read {path}: {e.Message}");
        }
    }

    private void Save()
    {
        var path = _cfg.GetCVar(FECCVars.TempJobWhitelistsFile);
        var dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(dir))
            Directory.CreateDirectory(dir);

        File.WriteAllText(path, JsonSerializer.Serialize(_entries, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        }));
        _loadedPath = path;
    }

    private sealed class FECTempJobWhitelist
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string JobId { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public string AddedBy { get; set; } = string.Empty;
    }
}
