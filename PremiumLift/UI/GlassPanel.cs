using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace PremiumLift.UI
{
    public enum PanelSurface { Fabric, BlackGlass }

    /// <summary>
    /// Rounded card with a brushed-brass frame. Fabric = linen wall,
    /// BlackGlass = lift control panel.
    /// </summary>
    public class GlassPanel : Panel
    {
        private string _title = string.Empty;
        private int _cornerRadius = 14;
        private PanelSurface _surface = PanelSurface.Fabric;

        public GlassPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            BackColor = Theme.FabricLight;
            ForeColor = Theme.Espresso;
        }

        [Category("Appearance"), DefaultValue("")]
        public string Title
        {
            get => _title;
            set { _title = value ?? string.Empty; Invalidate(); }
        }

        [Category("Appearance"), DefaultValue(14)]
        public int CornerRadius
        {
            get => _cornerRadius;
            set { _cornerRadius = Math.Max(0, value); Invalidate(); }
        }

        [Category("Appearance"), DefaultValue(PanelSurface.Fabric)]
        public PanelSurface Surface
        {
            get => _surface;
            set { _surface = value; Invalidate(); }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Theme.ParentBack(this));
            if (Width < 6 || Height < 6) return;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            var outer = new RectangleF(0.5f, 0.5f, Width - 1.5f, Height - 1.5f);

            using (var path = Theme.RoundRect(outer, _cornerRadius))
            {
                if (_surface == PanelSurface.Fabric)
                {
                    var state = g.Save();
                    g.SetClip(path);
                    Theme.PaintFabric(g, ClientRectangle, BackColor);
                    g.Restore(state);
                }
                else
                {
                    Theme.PaintGlass(g, outer, path);
                }

                // Brushed-brass frame
                using (var bb = new LinearGradientBrush(new RectangleF(0, 0, Width, Height),
                                                        Theme.BrassLight, Theme.BrassDark, 60f))
                using (var pen = new Pen(bb, _surface == PanelSurface.BlackGlass ? 3f : 1.6f))
                    g.DrawPath(pen, path);
            }

            if (_title.Length > 0)
            {
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                bool glass = _surface == PanelSurface.BlackGlass;

                using (var bar = new SolidBrush(Theme.Brass))
                    g.FillRectangle(bar, 18, 16, 3, 14);

                using (var f = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold))
                using (var b = new SolidBrush(glass ? Theme.BrassLight : Theme.Espresso))
                    g.DrawString(_title.ToUpperInvariant(), f, b, 26, 14);
            }
        }
    }
}
