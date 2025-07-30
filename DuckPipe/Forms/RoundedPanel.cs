using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

public class RoundedPanel : Panel
{
    public int BorderRadius { get; set; } = 10;
    public Color BorderColor { get; set; } = Color.Black;
    public int BorderThickness { get; set; } = 1;

    public RoundedPanel()
    {
        this.DoubleBuffered = true;
        this.BackColor = Color.White;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        var bounds = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
        using var path = GetRoundedRectPath(bounds, BorderRadius);

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        // Remplir le fond
        using var brush = new SolidBrush(this.BackColor);
        e.Graphics.FillPath(brush, path);

        // Dessiner la bordure
        using var pen = new Pen(BorderColor, BorderThickness);
        e.Graphics.DrawPath(pen, path);
        this.Region = new Region(path);  
    }

    private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
    {
        var path = new GraphicsPath();
        int r = radius * 2;

        path.StartFigure();
        path.AddArc(rect.X, rect.Y, r, r, 180, 90);
        path.AddArc(rect.Right - r, rect.Y, r, r, 270, 90);
        path.AddArc(rect.Right - r, rect.Bottom - r, r, r, 0, 90);
        path.AddArc(rect.X, rect.Bottom - r, r, r, 90, 90);
        path.CloseFigure();

        return path;
    }
}
