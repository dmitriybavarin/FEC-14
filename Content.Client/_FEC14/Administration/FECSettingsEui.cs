using System.Globalization;
using System.Linq;
using Content.Client.Eui;
using Content.Shared._FEC14.Administration;
using Content.Shared.Eui;
using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Client.UserInterface.Controls;
using static Robust.Client.UserInterface.Controls.BoxContainer;

namespace Content.Client._FEC14.Administration;

[UsedImplicitly]
public sealed class FECSettingsEui : BaseEui
{
    private FECSettingsWindow _window = default!;

    public override void Opened()
    {
        _window = new FECSettingsWindow();
        _window.OnClose += () => SendMessage(new CloseEuiMessage());
        _window.OpenCentered();
    }

    public override void Closed()
    {
        _window.Close();
    }

    public override void HandleState(EuiStateBase state)
    {
        if (state is not FECSettingsEuiState s)
            return;

        _window.StatusLabel.Text = s.Status ?? string.Empty;
        _window.StatusLabel.Visible = s.Status != null;
        _window.Entries.DisposeAllChildren();

        foreach (var group in s.Entries.GroupBy(e => e.Category))
        {
            _window.Entries.AddChild(new PanelContainer
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
        });

        if (entry.Type == FECSettingType.Bool)
        {
            var check = new CheckBox { Pressed = entry.Value == "true", ToolTip = tooltip };
            check.OnToggled += args => SendMessage(new FECSettingsSetMsg(entry.Key, args.Pressed ? "true" : "false"));
            row.AddChild(check);
        }
        else
        {
            var edit = new LineEdit { Text = entry.Value, MinWidth = entry.Type == FECSettingType.String ? 200 : 90, ToolTip = tooltip };
            edit.OnTextEntered += args => SendMessage(new FECSettingsSetMsg(entry.Key, args.Text));
            row.AddChild(edit);

            var apply = new Button { Text = Loc.GetString("fec-settings-apply") };
            apply.OnPressed += _ => SendMessage(new FECSettingsSetMsg(entry.Key, edit.Text));
            row.AddChild(apply);
        }

        var reset = new Button { Text = Loc.GetString("fec-settings-reset"), Disabled = entry.Value == entry.DefaultValue };
        reset.OnPressed += _ => SendMessage(new FECSettingsResetMsg(entry.Key));
        row.AddChild(reset);

        _window.Entries.AddChild(row);
    }
}
