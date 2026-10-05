using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using Content.Shared._FEC14.CCVar;
using Robust.Shared.Configuration;

namespace Content.Server._FEC14.Chat;

public static class FECOocColors
{
    private static readonly TimeSpan RecheckInterval = TimeSpan.FromSeconds(5);

    private static string? _rawCVar;
    private static Dictionary<Guid, string> _cvarColors = new();

    private static string? _fileSetting;
    private static string? _filePath;
    private static DateTime _fileWriteTime;
    private static DateTime _nextCheck;
    private static Dictionary<Guid, string> _fileColors = new();

    public static bool TryGetColor(IConfigurationManager config, Guid userId, [NotNullWhen(true)] out string? color)
    {
        RefreshFile(config);
        if (_fileColors.TryGetValue(userId, out color))
            return true;

        var raw = config.GetCVar(FECCVars.OocColors);
        if (raw != _rawCVar)
        {
            _cvarColors = ParseList(raw);
            _rawCVar = raw;
        }

        return _cvarColors.TryGetValue(userId, out color);
    }

    public static Dictionary<Guid, string> ParseList(string raw)
    {
        var result = new Dictionary<Guid, string>();
        foreach (var entry in raw.Split(new[] { ',', ';', '\n' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var parts = entry.Split('=', 2, StringSplitOptions.TrimEntries);
            if (parts.Length == 2 && Guid.TryParse(parts[0], out var id) && Normalize(parts[1]) is { } hex)
                result[id] = hex;
        }

        return result;
    }

    private static void RefreshFile(IConfigurationManager config)
    {
        var now = DateTime.UtcNow;
        var setting = config.GetCVar(FECCVars.OocColorsFile);
        if (setting == _fileSetting && now < _nextCheck)
            return;

        _nextCheck = now + RecheckInterval;
        if (setting != _fileSetting)
        {
            _fileSetting = setting;
            _filePath = Resolve(setting);
            _fileWriteTime = default;
            _fileColors = new Dictionary<Guid, string>();
        }

        if (_filePath == null || !File.Exists(_filePath))
        {
            _filePath = Resolve(setting);
            if (_filePath == null)
                return;
        }

        try
        {
            var writeTime = File.GetLastWriteTimeUtc(_filePath);
            if (writeTime == _fileWriteTime)
                return;

            _fileWriteTime = writeTime;
            _fileColors = ParseFile(File.ReadAllText(_filePath));
        }
        catch (Exception e) when (e is IOException or JsonException or UnauthorizedAccessException)
        {
            IoCManager.Resolve<ILogManager>().GetSawmill("fec.ooc_colors").Warning($"Failed to read {_filePath}: {e.Message}");
        }
    }

    private static Dictionary<Guid, string> ParseFile(string json)
    {
        var result = new Dictionary<Guid, string>();
        using var doc = JsonDocument.Parse(json, new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip, AllowTrailingCommas = true });
        if (doc.RootElement.ValueKind != JsonValueKind.Object)
            return result;

        foreach (var prop in doc.RootElement.EnumerateObject())
        {
            if (!Guid.TryParse(prop.Name, out var id))
                continue;

            var value = prop.Value.ValueKind switch
            {
                JsonValueKind.String => prop.Value.GetString(),
                JsonValueKind.Object when prop.Value.TryGetProperty("color", out var c) && c.ValueKind == JsonValueKind.String => c.GetString(),
                _ => null,
            };

            if (value != null && Normalize(value) is { } hex)
                result[id] = hex;
        }

        return result;
    }

    private static string? Normalize(string value)
    {
        return Color.TryFromHex(value.Trim()) is { } color ? color.ToHex() : null;
    }

    private static string? Resolve(string setting)
    {
        if (string.IsNullOrWhiteSpace(setting))
            return null;

        if (Path.IsPathRooted(setting))
            return setting;

        foreach (var start in new[] { Directory.GetCurrentDirectory(), AppContext.BaseDirectory })
        {
            var dir = new DirectoryInfo(start);
            while (dir != null)
            {
                var candidate = Path.Combine(dir.FullName, setting);
                if (File.Exists(candidate))
                    return candidate;

                dir = dir.Parent;
            }
        }

        return null;
    }
}
