using Content.Client._RMC14.Roadmap;
using Content.Client.Credits;
using Content.Client.Stylesheets;
using Content.Shared.CCVar;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controllers;
using Robust.Shared.Configuration;

namespace Content.Client._FEC14.Roadmap;

public sealed class FECRoadmapUIController : UIController
{
    [Dependency] private readonly IConfigurationManager _config = default!;
    [Dependency] private readonly IUriOpener _uriOpener = default!;

    private FECRoadmapChooserWindow? _chooser;
    private FECRoadmapWindow? _window;

    public void ToggleChooser()
    {
        if (_chooser != null || _window != null || UIManager.GetUIController<RoadmapUIController>().IsOpen)
        {
            CloseAll();
            return;
        }

        _chooser = new FECRoadmapChooserWindow();
        _chooser.OnClose += () => _chooser = null;
        _chooser.FecButton.OnPressed += _ =>
        {
            _chooser?.Close();
            OpenFec();
        };
        _chooser.RmcButton.OnPressed += _ =>
        {
            _chooser?.Close();
            UIManager.GetUIController<RoadmapUIController>().ToggleRmcRoadmap();
        };
        _chooser.OpenCentered();
    }

    public void OpenFec()
    {
        if (_window != null)
            return;

        _window = new FECRoadmapWindow();
        _window.OnClose += () => _window = null;

        if (_config.GetCVar(CCVars.InfoLinksDiscord) is { Length: > 0 } discordLink)
        {
            _window.DiscordButton.StyleClasses.Add(StyleBase.ButtonCaution);
            _window.DiscordButton.Visible = true;
            _window.DiscordButton.OnPressed += _ => _uriOpener.OpenUri(discordLink);
        }

        if (_config.GetCVar(CCVars.InfoLinksPatreon) is { Length: > 0 } patreonLink)
        {
            _window.PatreonButton.StyleClasses.Add(StyleBase.ButtonCaution);
            _window.PatreonButton.Visible = true;
            _window.PatreonButton.OnPressed += _ => _uriOpener.OpenUri(patreonLink);
        }

        _window.CreditsButton.StyleClasses.Add(StyleBase.ButtonCaution);
        _window.CreditsButton.OnPressed += _ => new CreditsWindow().OpenCentered();

        _window.OpenCentered();
    }

    private void CloseAll()
    {
        _chooser?.Close();
        _window?.Close();
        var rmc = UIManager.GetUIController<RoadmapUIController>();
        if (rmc.IsOpen)
            rmc.ToggleRmcRoadmap();
    }
}
