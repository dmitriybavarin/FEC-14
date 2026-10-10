using Robust.Shared.Maths;

namespace Content.Client._RMC14.TacticalMap;

public sealed partial class TacticalMapWrapper
{
    private void FECSetupCanvasTools()
    {
        FECRedoButton.OnPressed += _ => Canvas.FECRedo();
        FECEraserButton.OnPressed += _ =>
        {
            Canvas.FECEraseMode = !Canvas.FECEraseMode;
            FECUpdateCanvasTools();
        };
        UpdateCanvasButton.OnPressed += _ => Canvas.FECMarkSent();
        Canvas.FECChanged += FECUpdateCanvasTools;
        FECUpdateCanvasTools();
    }

    private void FECUpdateCanvasTools()
    {
        UndoButton.Disabled = !Canvas.FECCanUndo;
        FECRedoButton.Disabled = !Canvas.FECCanRedo;
        FECEraserButton.Modulate = Canvas.FECEraseMode ? Color.Green : Color.White;

        var limit = Canvas.LineLimit;
        FECCanvasStatus.Text = Loc.GetString("fec-tacmap-canvas-status",
            ("lines", Canvas.Lines.Count),
            ("limit", limit > 0 ? limit : 0),
            ("dirty", Canvas.FECDirty ? 1 : 0));
        FECCanvasStatus.FontColorOverride = limit > 0 && Canvas.Lines.Count >= limit * 9 / 10
            ? Color.FromHex("#E0A040")
            : null;
    }
}
