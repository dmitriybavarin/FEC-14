using System.Numerics;
using Content.Shared._RMC14.TacticalMap;

namespace Content.Client._RMC14.TacticalMap;

public sealed partial class TacticalMapControl
{
    private const int FECHistoryLimit = 100;
    private const float FECEraseTolerance = 9f;

    private readonly List<List<TacticalMapLine>> _fecUndo = new();
    private readonly List<List<TacticalMapLine>> _fecRedo = new();
    private List<TacticalMapLine>? _fecStrokeSnapshot;
    private bool _fecErasing;
    private bool _fecEraseChanged;

    public bool FECEraseMode { get; set; }
    public bool FECDirty { get; private set; }
    public bool FECCanUndo => _fecUndo.Count > 0;
    public bool FECCanRedo => _fecRedo.Count > 0;

    public event Action? FECChanged;

    public void FECLoadLines(IEnumerable<TacticalMapLine> lines, bool dirty)
    {
        Lines.Clear();
        Lines.AddRange(lines);
        FECSyncThicknesses();
        _fecUndo.Clear();
        _fecRedo.Clear();
        FECDirty = dirty;
        FECChanged?.Invoke();
    }

    public void FECMarkSent()
    {
        FECDirty = false;
        FECChanged?.Invoke();
    }

    public void FECClear()
    {
        if (Lines.Count == 0)
            return;

        FECPushUndo(FECSnapshot());
        Lines.Clear();
        LineThicknesses.Clear();
        FECModified();
    }

    public void FECUndo()
    {
        if (_fecUndo.Count == 0)
            return;

        _fecRedo.Add(FECSnapshot());
        FECRestore(FECPop(_fecUndo));
    }

    public void FECRedo()
    {
        if (_fecRedo.Count == 0)
            return;

        FECPushUndo(FECSnapshot(), false);
        FECRestore(FECPop(_fecRedo));
    }

    private bool FECStartPointer(Vector2 position)
    {
        if (FECEraseMode)
        {
            _fecErasing = true;
            _fecEraseChanged = false;
            _fecStrokeSnapshot = FECSnapshot();
            FECEraseAt(position);
            return true;
        }

        _fecStrokeSnapshot = FECSnapshot();
        return false;
    }

    private bool FECMovePointer(Vector2 position)
    {
        if (!_fecErasing)
            return false;

        FECEraseAt(position);
        return true;
    }

    private bool FECEndPointer()
    {
        var snapshot = _fecStrokeSnapshot;
        _fecStrokeSnapshot = null;

        if (_fecErasing)
        {
            _fecErasing = false;
            if (_fecEraseChanged && snapshot != null)
            {
                FECPushUndo(snapshot);
                FECModified();
            }

            return true;
        }

        if (snapshot != null && !FECSame(snapshot))
        {
            FECPushUndo(snapshot);
            FECModified();
        }

        return false;
    }

    private void FECEraseAt(Vector2 controlPosition)
    {
        if (Lines.Count == 0)
            return;

        var point = (Vector2) ConvertIndicesToLineCoordinates(PositionToIndices(controlPosition));
        var tolerance = FECEraseTolerance / Math.Max(_zoomFactor, 0.3f);

        for (var i = Lines.Count - 1; i >= 0; i--)
        {
            if (i >= Lines.Count)
                continue;

            var line = Lines[i];
            if (FECDistance(point, line.Start, line.End) > tolerance)
                continue;

            var (from, to) = FECChain(i);
            Lines.RemoveRange(from, to - from + 1);
            _fecEraseChanged = true;
            i = Math.Min(i, from);
        }

        FECSyncThicknesses();
    }

    private (int From, int To) FECChain(int index)
    {
        var from = index;
        var to = index;
        var color = Lines[index].Color;

        while (from > 0 && Lines[from - 1].Color == color && FECNear(Lines[from - 1].End, Lines[from].Start))
            from--;

        while (to < Lines.Count - 1 && Lines[to + 1].Color == color && FECNear(Lines[to].End, Lines[to + 1].Start))
            to++;

        return (from, to);
    }

    private static bool FECNear(Vector2i a, Vector2i b)
    {
        return Math.Abs(a.X - b.X) <= 3 && Math.Abs(a.Y - b.Y) <= 3;
    }

    private static float FECDistance(Vector2 point, Vector2i start, Vector2i end)
    {
        var a = (Vector2) start;
        var b = (Vector2) end;
        var ab = b - a;
        var lengthSquared = ab.LengthSquared();
        if (lengthSquared < 0.0001f)
            return Vector2.Distance(point, a);

        var t = Math.Clamp(Vector2.Dot(point - a, ab) / lengthSquared, 0f, 1f);
        return Vector2.Distance(point, a + ab * t);
    }

    private List<TacticalMapLine> FECSnapshot()
    {
        return new List<TacticalMapLine>(Lines);
    }

    private bool FECSame(List<TacticalMapLine> snapshot)
    {
        if (snapshot.Count != Lines.Count)
            return false;

        for (var i = 0; i < snapshot.Count; i++)
        {
            if (snapshot[i] != Lines[i])
                return false;
        }

        return true;
    }

    private void FECPushUndo(List<TacticalMapLine> snapshot, bool clearRedo = true)
    {
        _fecUndo.Add(snapshot);
        if (_fecUndo.Count > FECHistoryLimit)
            _fecUndo.RemoveAt(0);

        if (clearRedo)
            _fecRedo.Clear();
    }

    private static List<TacticalMapLine> FECPop(List<List<TacticalMapLine>> stack)
    {
        var last = stack[^1];
        stack.RemoveAt(stack.Count - 1);
        return last;
    }

    private void FECRestore(List<TacticalMapLine> snapshot)
    {
        Lines.Clear();
        Lines.AddRange(snapshot);
        FECSyncThicknesses();
        FECModified();
    }

    private void FECSyncThicknesses()
    {
        LineThicknesses.Clear();
        foreach (var line in Lines)
        {
            LineThicknesses.Add(line.Thickness);
        }
    }

    private void FECModified()
    {
        FECDirty = true;
        FECChanged?.Invoke();
    }
}
