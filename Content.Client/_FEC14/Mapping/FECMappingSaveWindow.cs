using System.Numerics;
using Robust.Client.Console;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.Utility;

namespace Content.Client._FEC14.Mapping;

public sealed class FECMappingSaveWindow : DefaultWindow
{
    public FECMappingSaveWindow(string mapId)
    {
        Title = Loc.GetString("fec-mapping-ui-save-title");
        MinSize = new Vector2(460, 120);

        var mapEdit = new LineEdit { Text = mapId, MinWidth = 70, PlaceHolder = Loc.GetString("fec-mapping-ui-save-map") };
        var pathEdit = new LineEdit { Text = "/Maps/_FEC14/mapping.yml", HorizontalExpand = true };
        var save = new Button { Text = Loc.GetString("fec-mapping-ui-save") };
        save.OnPressed += _ =>
        {
            var map = mapEdit.Text.Trim();
            var path = pathEdit.Text.Trim();
            if (map.Length == 0 || path.Length == 0)
                return;

            IoCManager.Resolve<IClientConsoleHost>().ExecuteCommand($"savemap {CommandParsing.Escape(map)} \"{CommandParsing.Escape(path)}\"");
            Close();
        };

        Contents.AddChild(new BoxContainer
        {
            Orientation = BoxContainer.LayoutOrientation.Vertical,
            SeparationOverride = 4,
            Margin = new Thickness(6),
            Children =
            {
                new BoxContainer
                {
                    Orientation = BoxContainer.LayoutOrientation.Horizontal,
                    SeparationOverride = 6,
                    Children = { mapEdit, pathEdit, save },
                },
                new Label { Text = Loc.GetString("fec-mapping-ui-save-hint"), FontColorOverride = Color.FromHex("#A0A0A0") },
            },
        });
    }
}
