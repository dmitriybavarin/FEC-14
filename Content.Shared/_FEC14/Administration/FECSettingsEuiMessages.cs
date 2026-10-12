using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared._FEC14.Administration;

[Serializable, NetSerializable]
public enum FECSettingType : byte
{
    Bool,
    Int,
    Float,
    String,
}

[Serializable, NetSerializable]
public sealed class FECSettingEntry(string category, string key, FECSettingType type, string value, string defaultValue, double min, double max)
{
    public readonly string Category = category;
    public readonly string Key = key;
    public readonly FECSettingType Type = type;
    public readonly string Value = value;
    public readonly string DefaultValue = defaultValue;
    public readonly double Min = min;
    public readonly double Max = max;
}

[Serializable, NetSerializable]
public sealed class FECSettingsEuiState(List<FECSettingEntry> entries, string? status) : EuiStateBase
{
    public readonly List<FECSettingEntry> Entries = entries;
    public readonly string? Status = status;
}

[Serializable, NetSerializable]
public sealed class FECSettingsSetMsg(string key, string value) : EuiMessageBase
{
    public readonly string Key = key;
    public readonly string Value = value;
}

[Serializable, NetSerializable]
public sealed class FECSettingsResetMsg(string key) : EuiMessageBase
{
    public readonly string Key = key;
}
