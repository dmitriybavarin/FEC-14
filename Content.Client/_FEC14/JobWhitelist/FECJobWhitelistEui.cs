using System.Linq;
using Content.Client.Eui;
using Content.Client.UserInterface.Controls;
using Content.Shared._FEC14.JobWhitelist;
using Content.Shared.Eui;
using Content.Shared.Roles;
using JetBrains.Annotations;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Prototypes;
using static Robust.Client.UserInterface.Controls.BoxContainer;

namespace Content.Client._FEC14.JobWhitelist;

[UsedImplicitly]
public sealed class FECJobWhitelistEui : BaseEui
{
    [Dependency] private readonly IPrototypeManager _prototypes = default!;

    private const int GroupByPlayer = 0;
    private const int GroupByJob = 1;

    private FECJobWhitelistWindow _window = default!;
    private FECJobWhitelistEuiState? _state;
    private readonly List<JobPrototype> _jobs = new();
    private HashSet<string> _visible = new();
    private int _group = GroupByPlayer;
    private string? _selectedJob;

    public override void Opened()
    {
        _window = new FECJobWhitelistWindow();
        _window.OnClose += () => SendMessage(new CloseEuiMessage());

        _window.GroupSelector.AddItem(Loc.GetString("fec-job-whitelist-ui-group-player"), GroupByPlayer);
        _window.GroupSelector.AddItem(Loc.GetString("fec-job-whitelist-ui-group-job"), GroupByJob);
        _window.GroupSelector.OnItemSelected += args =>
        {
            _window.GroupSelector.SelectId(args.Id);
            _group = args.Id;
            Refresh();
        };

        _window.SearchEdit.OnTextChanged += _ => Refresh();
        _window.RefreshButton.OnPressed += _ => SendMessage(new FECJobWhitelistRefreshMsg());

        _visible = FECLobbyJobs.Visible(_prototypes);
        _jobs.AddRange(_prototypes.EnumeratePrototypes<JobPrototype>()
            .OrderBy(j => _visible.Contains(j.ID) ? 0 : 1)
            .ThenByDescending(j => j.Whitelisted)
            .ThenBy(j => j.LocalizedName));

        for (var i = 0; i < _jobs.Count; i++)
        {
            _window.JobSelector.AddItem(JobLabel(_jobs[i]), i);
        }

        if (_jobs.Count > 0)
        {
            _window.JobSelector.SelectId(0);
            _selectedJob = _jobs[0].ID;
        }

        _window.JobSelector.OnItemSelected += args =>
        {
            _window.JobSelector.SelectId(args.Id);
            _selectedJob = _jobs.ElementAtOrDefault(args.Id)?.ID;
        };

        _window.AddButton.OnPressed += _ =>
        {
            if (_selectedJob != null)
                SendMessage(new FECJobWhitelistAddMsg(_window.PlayerEdit.Text, _selectedJob));
        };

        _window.OpenCentered();
    }

    public override void Closed()
    {
        _window.Close();
    }

    public override void HandleState(EuiStateBase state)
    {
        if (state is not FECJobWhitelistEuiState s)
            return;

        _state = s;
        _window.StatusLabel.Text = s.Status ?? string.Empty;
        _window.StatusLabel.Visible = s.Status != null;
        Refresh();
    }

    private void Refresh()
    {
        _window.Entries.DisposeAllChildren();
        if (_state == null)
            return;

        var search = _window.SearchEdit.Text.Trim();
        var entries = _state.Entries
            .Where(e => search.Length == 0 ||
                        e.PlayerName.Contains(search, StringComparison.CurrentCultureIgnoreCase) ||
                        e.JobId.Contains(search, StringComparison.CurrentCultureIgnoreCase) ||
                        JobName(e.JobId).Contains(search, StringComparison.CurrentCultureIgnoreCase))
            .ToList();

        var players = entries.Select(e => e.UserId).Distinct().Count();
        _window.SummaryLabel.Text = Loc.GetString("fec-job-whitelist-ui-summary",
            ("entries", entries.Count),
            ("players", players));

        if (entries.Count == 0)
        {
            _window.Entries.AddChild(new Label { Text = Loc.GetString("fec-job-whitelist-ui-empty") });
            return;
        }

        if (_group == GroupByJob)
        {
            foreach (var group in entries.GroupBy(e => e.JobId).OrderBy(g => JobName(g.Key)))
            {
                AddHeader(Loc.GetString("fec-job-whitelist-ui-job-header",
                    ("job", JobName(group.Key)),
                    ("id", group.Key),
                    ("count", group.Count()),
                    ("other", _visible.Contains(group.Key) ? 0 : 1)));

                foreach (var entry in group.OrderBy(e => e.PlayerName))
                {
                    AddRow(PlayerLabel(entry), entry);
                }
            }

            return;
        }

        foreach (var group in entries.GroupBy(e => e.UserId).OrderBy(g => g.First().PlayerName))
        {
            AddHeader(Loc.GetString("fec-job-whitelist-ui-player-header",
                ("player", PlayerLabel(group.First())),
                ("count", group.Count())));

            foreach (var entry in group.OrderBy(e => JobName(e.JobId)))
            {
                AddRow(Loc.GetString("fec-job-whitelist-ui-job-row",
                    ("job", JobName(entry.JobId)),
                    ("id", entry.JobId),
                    ("other", _visible.Contains(entry.JobId) ? 0 : 1)), entry);
            }
        }
    }

    private void AddHeader(string text)
    {
        _window.Entries.AddChild(new PanelContainer
        {
            PanelOverride = new Robust.Client.Graphics.StyleBoxFlat { BackgroundColor = Color.FromHex("#464966") },
            Margin = new Thickness(0, 6, 0, 0),
            Children =
            {
                new Label { Text = text, Margin = new Thickness(5, 0, 0, 0) },
            },
        });
    }

    private void AddRow(string text, FECJobWhitelistEntry entry)
    {
        var row = new BoxContainer { Orientation = LayoutOrientation.Horizontal, SeparationOverride = 6 };
        row.AddChild(new Label
        {
            Text = text,
            HorizontalExpand = true,
            ClipText = true,
            Margin = new Thickness(12, 0, 0, 0),
        });

        var remove = new ConfirmButton
        {
            Text = Loc.GetString("fec-job-whitelist-ui-remove"),
            ConfirmationText = Loc.GetString("fec-job-whitelist-ui-remove-confirm"),
        };
        remove.OnPressed += _ => SendMessage(new FECJobWhitelistRemoveMsg(entry.UserId, entry.JobId));
        row.AddChild(remove);

        _window.Entries.AddChild(row);
    }

    private string PlayerLabel(FECJobWhitelistEntry entry)
    {
        return Loc.GetString("fec-job-whitelist-ui-player-name",
            ("player", entry.PlayerName),
            ("online", entry.Online ? 1 : 0));
    }

    private string JobLabel(JobPrototype job)
    {
        return Loc.GetString("fec-job-whitelist-ui-job-option",
            ("job", job.LocalizedName),
            ("id", job.ID),
            ("other", _visible.Contains(job.ID) ? 0 : 1),
            ("whitelisted", job.Whitelisted ? 1 : 0));
    }

    private string JobName(string id)
    {
        return _prototypes.TryIndex<JobPrototype>(id, out var job) ? job.LocalizedName : id;
    }
}
