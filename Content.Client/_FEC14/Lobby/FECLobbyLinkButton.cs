using Content.Shared.CCVar;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Configuration;

namespace Content.Client._FEC14.Lobby;

public sealed class FECLobbyLinkButton : Button
{
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly IUriOpener _uri = default!;

    public FECLobbyLinkButton()
    {
        IoCManager.InjectDependencies(this);
        Text = Loc.GetString("fec-lobby-link-button");
        Visible = false;
        OnPressed += _ =>
        {
            var url = _cfg.GetCVar(CCVars.InfoLinksDiscord);
            if (!string.IsNullOrWhiteSpace(url))
                _uri.OpenUri(url);
        };
    }

    protected override void EnteredTree()
    {
        base.EnteredTree();
        _cfg.OnValueChanged(CCVars.InfoLinksDiscord, OnLinkChanged, true);
    }

    protected override void ExitedTree()
    {
        base.ExitedTree();
        _cfg.UnsubValueChanged(CCVars.InfoLinksDiscord, OnLinkChanged);
    }

    private void OnLinkChanged(string url)
    {
        Visible = !string.IsNullOrWhiteSpace(url);
    }
}
