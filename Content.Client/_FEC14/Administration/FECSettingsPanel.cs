using System.Globalization;
using System.Linq;
using Content.Shared._FEC14.Administration;
using Robust.Client.Graphics;
using Robust.Client.UserInterface.Controls;
using static Robust.Client.UserInterface.Controls.BoxContainer;

namespace Content.Client._FEC14.Administration;

public sealed class FECSettingsPanel : BoxContainer
{
    public event Action<string, string>? OnSet;
    public event Action<string>? OnReset;

    private readonly BoxContainer _entries;
    private readonly Label _status;

    public FECSettingsPanel()
    {
        Orientation = LayoutOrientation.Vertical;
        SeparationOverride = 4;
        _entries = new BoxContainer { Orientation = LayoutOrientation.Vertical, SeparationOverride = 4 };
        _status = new Label { FontColorOverride = Color.FromHex("#C8C864"), Visible = false };
        AddChild(_entries);
        AddChild(_status);
    }

    public void Update(List<FECSettingEntry> entries, string? status)
    {
        _status.Text = status ?? string.Empty;
        _status.Visible = status != null;
        _entries.DisposeAllChildren();

        foreach (var group in entries.GroupBy(e => e.Category))
        {
            _entries.AddChild(new PanelContainer
            {
                PanelOverride = new StyleBoxFlat { BackgroundColor = Color.FromHex("#464966") },
                Margin = new Thickness(0, 6, 0, 0),
                Children =
                {
                    new Label { Text = Loc.GetString($"fec-settings-category-{group.Key}"), Margin = new Thickness(5, 0, 0, 0) },
                },
            });

            foreach (var entry in group)
            {
                AddRow(entry);
            }
        }
    }

    private void AddRow(FECSettingEntry entry)
    {
        var row = new BoxContainer { Orientation = LayoutOrientation.Horizontal, SeparationOverride = 6 };
        var nameKey = $"fec-settings-{entry.Key.Replace('.', '-').Replace('_', '-')}";
        var name = Loc.TryGetString(nameKey, out var text) ? text : entry.Key;
        var tooltip = Loc.GetString("fec-settings-tooltip", ("key", entry.Key), ("default", entry.DefaultValue),
            ("range", entry.Type is FECSettingType.Int or FECSettingType.Float ? 1 : 0),
            ("min", entry.Min.ToString(CultureInfo.InvariantCulture)), ("max", entry.Max.ToString(CultureInfo.InvariantCulture)));

        row.AddChild(new Label
        {
            Text = name,
            ToolTip = tooltip,
            HorizontalExpand = true,
            ClipText = true,
            Margin = new Thickness(12, 0, 0, 0),
            FontColorOverride = entry.Value == entry.DefaultValue ? null : Color.FromHex("#E0B060"),
        });

        if (entry.Type == FECSettingType.Bool)
        {
            var check = new CheckBox { Pressed = entry.Value == "true", ToolTip = tooltip };
            check.OnToggled += args => OnSet?.Invoke(entry.Key, args.Pressed ? "true" : "false");
            row.AddChild(check);
        }
        else
        {
            var edit = new LineEdit { Text = entry.Value, MinWidth = entry.Type == FECSettingType.String ? 200 : 90, ToolTip = tooltip };
            edit.OnTextEntered += args => OnSet?.Invoke(entry.Key, args.Text);
            row.AddChild(edit);

            var apply = new Button { Text = Loc.GetString("fec-settings-apply") };
            apply.OnPressed += _ => OnSet?.Invoke(entry.Key, edit.Text);
            row.AddChild(apply);
        }

        var reset = new Button { Text = Loc.GetString("fec-settings-reset"), Disabled = entry.Value == entry.DefaultValue };
        reset.OnPressed += _ => OnReset?.Invoke(entry.Key);
        row.AddChild(reset);

        _entries.AddChild(row);
    }
}
