using Content.Client.Eui;
using Content.Client.UserInterface.Controls;
using Content.Shared._FEC14.Events;
using Content.Shared.Eui;
using JetBrains.Annotations;
using Robust.Client.UserInterface.Controls;
using static Robust.Client.UserInterface.Controls.BoxContainer;

namespace Content.Client._FEC14.Events;

[UsedImplicitly]
public sealed class FECEventsEui : BaseEui
{
    private FECEventsWindow _window = default!;

    public override void Opened()
    {
        _window = new FECEventsWindow();
        _window.OnClose += () => SendMessage(new CloseEuiMessage());
        _window.ResetRotationButton.OnPressed += _ => SendMessage(new FECEventsResetRotationMsg());
        _window.OpenCentered();
    }

    public override void Closed()
    {
        _window.Close();
    }

    public override void HandleState(EuiStateBase state)
    {
        if (state is not FECEventsEuiState s)
            return;

        _window.InfoLabel.Text = s.Info;
        _window.StatusLabel.Text = s.Status ?? string.Empty;
        _window.StatusLabel.Visible = s.Status != null;

        _window.Entries.DisposeAllChildren();
        foreach (var entry in s.Entries)
        {
            AddRow(entry);
        }

        _window.LogBox.DisposeAllChildren();
        if (s.Log.Count == 0)
            _window.LogBox.AddChild(new Label { Text = Loc.GetString("fec-events-ui-log-empty"), FontColorOverride = Color.Gray });

        for (var i = s.Log.Count - 1; i >= 0; i--)
        {
            _window.LogBox.AddChild(new Label { Text = s.Log[i] });
        }
    }

    private void AddRow(FECEventsEuiEntry entry)
    {
        var row = new BoxContainer { Orientation = LayoutOrientation.Horizontal, SeparationOverride = 6 };
        row.AddChild(new Label
        {
            Text = Loc.GetString("fec-events-ui-row",
                ("name", entry.Name),
                ("id", entry.Id),
                ("players", entry.MinPlayers),
                ("weight", entry.Weight),
                ("status", entry.Status)),
            HorizontalExpand = true,
            ClipText = true,
            FontColorOverride = entry.Blocked ? Color.FromHex("#C86464") : null,
        });

        var start = new ConfirmButton
        {
            Text = Loc.GetString("fec-events-ui-start"),
            ConfirmationText = Loc.GetString("fec-events-ui-start-confirm"),
        };
        start.OnPressed += _ => SendMessage(new FECEventsStartMsg(entry.Id));
        row.AddChild(start);

        if (entry.Blocked)
        {
            var unblock = new Button { Text = Loc.GetString("fec-events-ui-unblock") };
            unblock.OnPressed += _ => SendMessage(new FECEventsUnblockMsg(entry.Id));
            row.AddChild(unblock);
        }
        else
        {
            var block = new Button { Text = Loc.GetString("fec-events-ui-block") };
            block.OnPressed += _ => SendMessage(new FECEventsBlockMsg(entry.Id, _window.DurationEdit.Text));
            row.AddChild(block);
        }

        _window.Entries.AddChild(row);
    }
}
