using System.Linq;
using System.Numerics;
using Content.Client.Eui;
using Content.Shared._FEC14.Administration;
using Content.Shared.Eui;
using JetBrains.Annotations;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.Utility;

namespace Content.Client._FEC14.Administration;

[UsedImplicitly]
public sealed class FECPlayerHistoryEui : BaseEui
{
    private DefaultWindow _window = default!;
    private BoxContainer _summary = default!;
    private BoxContainer _entries = default!;
    private OptionButton _filter = default!;
    private FECPlayerHistoryEuiState? _state;
    private FECPlayerHistoryKind? _selected;

    public override void Opened()
    {
        _window = new DefaultWindow
        {
            MinSize = new Vector2(620, 520),
            SetSize = new Vector2(720, 640),
            Title = Loc.GetString("fec-history-title", ("player", string.Empty)),
        };

        var root = new BoxContainer { Orientation = BoxContainer.LayoutOrientation.Vertical, SeparationOverride = 6, Margin = new Thickness(6) };
        _summary = new BoxContainer { Orientation = BoxContainer.LayoutOrientation.Vertical };
        _filter = new OptionButton();
        _filter.AddItem(Loc.GetString("fec-history-filter-all"), -1);
        foreach (var kind in Enum.GetValues<FECPlayerHistoryKind>())
        {
            _filter.AddItem(KindName(kind), (int) kind);
        }

        _filter.OnItemSelected += args =>
        {
            _filter.SelectId(args.Id);
            _selected = args.Id < 0 ? null : (FECPlayerHistoryKind) args.Id;
            Refresh();
        };

        _entries = new BoxContainer { Orientation = BoxContainer.LayoutOrientation.Vertical, SeparationOverride = 4 };
        var scroll = new ScrollContainer { HorizontalExpand = true, VerticalExpand = true, HScrollEnabled = false };
        scroll.AddChild(_entries);

        root.AddChild(_summary);
        root.AddChild(_filter);
        root.AddChild(scroll);
        _window.Contents.AddChild(root);
        _window.OnClose += () => SendMessage(new CloseEuiMessage());
        _window.OpenCentered();
    }

    public override void Closed()
    {
        _window.Close();
    }

    public override void HandleState(EuiStateBase state)
    {
        if (state is not FECPlayerHistoryEuiState s)
            return;

        _state = s;
        _window.Title = Loc.GetString("fec-history-title", ("player", s.Player));
        Refresh();
    }

    private void Refresh()
    {
        _summary.DisposeAllChildren();
        _entries.DisposeAllChildren();
        if (_state == null)
            return;

        if (_state.Loading)
        {
            _summary.AddChild(new Label { Text = Loc.GetString("fec-history-loading") });
            return;
        }

        foreach (var line in _state.Summary)
        {
            _summary.AddChild(new Label { Text = line });
        }

        var entries = _state.Entries.Where(e => _selected == null || e.Kind == _selected).ToList();
        if (entries.Count == 0)
        {
            _entries.AddChild(new Label { Text = Loc.GetString("fec-history-empty") });
            return;
        }

        foreach (var entry in entries)
        {
            var label = new RichTextLabel { HorizontalExpand = true };
            var date = entry.Date is { } d ? $"{d:dd.MM.yyyy HH:mm}" : "—";
            var text = $"[color={KindColor(entry.Kind)}][bold]{KindName(entry.Kind)}[/bold][/color] [color=#a0a0a0]{date}[/color]\n{FormattedMessage.EscapeText(entry.Text)}";
            label.SetMessage(FormattedMessage.FromMarkupPermissive(text));
            _entries.AddChild(label);
        }
    }

    private static string KindName(FECPlayerHistoryKind kind)
    {
        return Loc.GetString($"fec-history-kind-{kind.ToString().ToLowerInvariant()}");
    }

    private static string KindColor(FECPlayerHistoryKind kind)
    {
        return kind switch
        {
            FECPlayerHistoryKind.Ban => "#ff5555",
            FECPlayerHistoryKind.RoleBan => "#ff9955",
            FECPlayerHistoryKind.ChatBan => "#ffcc55",
            FECPlayerHistoryKind.Watchlist => "#cc77ff",
            FECPlayerHistoryKind.Whitelist => "#77dd77",
            _ => "#77bbff",
        };
    }
}
