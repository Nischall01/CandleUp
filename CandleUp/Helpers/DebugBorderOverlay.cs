using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.VisualTree;

namespace CandleUp.Helpers;

// DebugBorderOverlay.cs

public sealed class DebugBorderOverlay : Control
{
    private readonly Pen _pen = new(Brushes.Red, 1, new DashStyle(new double[] { 2, 2 }, 0));
    private readonly Visual _root;

    public DebugBorderOverlay(Visual root)
    {
        _root = root;
        IsHitTestVisible = false;
        ZIndex = int.MaxValue;
        ClipToBounds = false;
        HorizontalAlignment = HorizontalAlignment.Stretch;
        VerticalAlignment = VerticalAlignment.Stretch;
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        foreach (var v in _root.GetVisualDescendants().OfType<Visual>())
        {
            if (v == this) continue;

            var b = v.Bounds;
            if (b.Width <= 0 || b.Height <= 0) continue;

            var p = v.TranslatePoint(b.Position, _root);
            if (p is null) continue;

            var rect = new Rect(p.Value, b.Size);
            context.DrawRectangle(null, _pen, rect);
        }

        // Keep it live-updating while active (debug-only perf tradeoff)
        InvalidateVisual();
    }
}