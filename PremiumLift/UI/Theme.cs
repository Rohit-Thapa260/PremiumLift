using System.Drawing.Drawing2D;

namespace PremiumLift.UI
{
    /// <summary>
    /// "Aurum" palette taken from a luxury lift cabin: linen fabric walls,
    /// walnut panels, brushed-brass trim, black-glass control panels with
    /// white keys (green when lit) and a marble floor.
    /// </summary>
    internal static class Theme
    {
        // Fabric walls
        public static readonly Color FabricDark  = Color.FromArgb(205, 192, 171);
        public static readonly Color Fabric      = Color.FromArgb(222, 211, 193);
        public static readonly Color FabricLight = Color.FromArgb(238, 230, 216);

        // Walnut
        public static readonly Color WoodDark  = Color.FromArgb(88, 56, 30);
        public static readonly Color Wood      = Color.FromArgb(128, 84, 46);
        public static readonly Color WoodLight = Color.FromArgb(168, 118, 68);

        // Brushed brass
        public static readonly Color Brass      = Color.FromArgb(184, 147, 90);
        public static readonly Color BrassLight = Color.FromArgb(230, 204, 150);
        public static readonly Color BrassDark  = Color.FromArgb(118, 90, 50);

        // Black glass (control panels)
        public static readonly Color Glass      = Color.FromArgb(12, 12, 14);
        public static readonly Color GlassEdge  = Color.FromArgb(46, 44, 42);
        public static readonly Color GlassText  = Color.FromArgb(236, 232, 224);
        public static readonly Color GlassMuted = Color.FromArgb(142, 137, 130);
        public static readonly Color KeyGreen   = Color.FromArgb(76, 224, 138);

        // Text on fabric
        public static readonly Color Espresso = Color.FromArgb(43, 33, 25);
        public static readonly Color Taupe    = Color.FromArgb(118, 102, 84);

        // Marble / shaft
        public static readonly Color Marble     = Color.FromArgb(160, 153, 145);
        public static readonly Color MarbleDark = Color.FromArgb(112, 106, 99);
        public static readonly Color Shaft      = Color.FromArgb(24, 19, 15);

        public static readonly Color WarmLight = Color.FromArgb(255, 226, 168);
        public static readonly Color Ready     = Color.FromArgb(56, 160, 98);

        public static GraphicsPath RoundRect(RectangleF r, float radius)
        {
            var path = new GraphicsPath();
            float d = Math.Max(0.1f, Math.Min(radius * 2f, Math.Min(r.Width, r.Height)));
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        public static Color ParentBack(Control c) =>
            c.Parent != null ? c.Parent.BackColor : Fabric;

        public static Color WithAlpha(Color c, int alpha) =>
            Color.FromArgb(Math.Clamp(alpha, 0, 255), c);

        /// <summary>Linen-like woven texture (deterministic, so it never flickers).</summary>
        public static void PaintFabric(Graphics g, Rectangle r, Color baseColor)
        {
            using (var b = new SolidBrush(baseColor)) g.FillRectangle(b, r);
            var rnd = new Random(7);
            using (var light = new Pen(WithAlpha(Color.White, 26)))
            using (var dark = new Pen(WithAlpha(Espresso, 10)))
            {
                for (int x = r.Left; x < r.Right; x += 2)
                    g.DrawLine(rnd.Next(3) == 0 ? light : dark, x, r.Top, x, r.Bottom);
                for (int y = r.Top; y < r.Bottom; y += 3)
                    g.DrawLine(rnd.Next(2) == 0 ? light : dark, r.Left, y, r.Right, y);
            }
        }

        /// <summary>Walnut veneer: vertical grain over a warm gradient.</summary>
        public static void PaintWood(Graphics g, Rectangle r)
        {
            if (r.Width < 2 || r.Height < 2) return;
            using (var b = new LinearGradientBrush(r, WoodLight, WoodDark, 0f))
            {
                b.InterpolationColors = new ColorBlend
                {
                    Colors = new[] { WoodDark, WoodLight, Wood, WoodLight, WoodDark },
                    Positions = new[] { 0f, 0.25f, 0.5f, 0.78f, 1f }
                };
                g.FillRectangle(b, r);
            }
            var rnd = new Random(11);
            for (int x = r.Left; x < r.Right; x += 3)
            {
                int a = rnd.Next(6, 26);
                using (var p = new Pen(Color.FromArgb(a, rnd.Next(2) == 0 ? Color.Black : Color.FromArgb(255, 220, 170))))
                    g.DrawLine(p, x + (float)rnd.NextDouble(), r.Top, x + (float)rnd.NextDouble() * 2, r.Bottom);
            }
        }

        /// <summary>Black glass with a soft diagonal reflection.</summary>
        public static void PaintGlass(Graphics g, RectangleF r, GraphicsPath clip)
        {
            using (var b = new LinearGradientBrush(r, Color.FromArgb(26, 26, 29), Glass, 90f))
                g.FillPath(b, clip);
            var state = g.Save();
            g.SetClip(clip, CombineMode.Intersect);
            using (var refl = new GraphicsPath())
            {
                refl.AddPolygon(new[]
                {
                    new PointF(r.X, r.Y), new PointF(r.X + r.Width * 0.55f, r.Y),
                    new PointF(r.X, r.Y + r.Height * 0.35f)
                });
                using (var rb = new SolidBrush(Color.FromArgb(10, 255, 255, 255)))
                    g.FillPath(rb, refl);
            }
            g.Restore(state);
        }
    }
}
