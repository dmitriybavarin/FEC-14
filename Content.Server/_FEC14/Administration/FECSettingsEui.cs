using Content.Server.Administration;
using Content.Server.Administration.Managers;
using Content.Server.EUI;
using Content.Shared._FEC14.Administration;
using Content.Shared.Eui;

namespace Content.Server._FEC14.Administration;

public sealed class FECSettingsEui : BaseEui
{
    [Dependency] private readonly IAdminManager _admin = default!;
    [Dependency] private readonly IEntityManager _entities = default!;

    private readonly FECSettingsSystem _settings;
    private string? _status;

    public FECSettingsEui()
    {
        IoCManager.InjectDependencies(this);
        _settings = _entities.System<FECSettingsSystem>();
    }

    public override void Opened()
    {
        _admin.OnPermsChanged += OnPermsChanged;
        StateDirty();
    }

    public override void Closed()
    {
        _admin.OnPermsChanged -= OnPermsChanged;
    }

    private void OnPermsChanged(AdminPermsChangedEventArgs args)
    {
        if (args.Player == Player && !_settings.CanUse(Player))
            Close();
    }

    public override EuiStateBase GetNewState()
    {
        return new FECSettingsEuiState(_settings.GetEntries(), _status);
    }

    public override void HandleMessage(EuiMessageBase msg)
    {
        base.HandleMessage(msg);

        if (!_settings.CanUse(Player))
        {
            Close();
            return;
        }

        switch (msg)
        {
            case FECSettingsSetMsg set:
                _settings.TrySet(Player, set.Key, set.Value, out _status);
                break;
            case FECSettingsResetMsg reset:
                _settings.Reset(Player, reset.Key, out _status);
                break;
        }

        StateDirty();
    }
}
