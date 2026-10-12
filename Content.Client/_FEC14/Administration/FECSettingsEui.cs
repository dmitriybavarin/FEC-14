using Content.Client.Eui;
using Content.Shared._FEC14.Administration;
using Content.Shared.Eui;
using JetBrains.Annotations;

namespace Content.Client._FEC14.Administration;

[UsedImplicitly]
public sealed class FECSettingsEui : BaseEui
{
    private FECSettingsWindow _window = default!;

    public override void Opened()
    {
        _window = new FECSettingsWindow();
        _window.OnClose += () => SendMessage(new CloseEuiMessage());
        _window.Panel.OnSet += (key, value) => SendMessage(new FECSettingsSetMsg(key, value));
        _window.Panel.OnReset += key => SendMessage(new FECSettingsResetMsg(key));
        _window.OpenCentered();
    }

    public override void Closed()
    {
        _window.Close();
    }

    public override void HandleState(EuiStateBase state)
    {
        if (state is FECSettingsEuiState s)
            _window.Panel.Update(s.Entries, s.Status);
    }
}
