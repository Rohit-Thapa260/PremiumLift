using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace PremiumLift.UI
{
    /// <summary>
    /// Lift car with champagne-bronze sliding doors. When the doors open the
    /// cabin is drawn in perspective: linen side walls with brass trims,
    /// walnut back wall with a backlit mirror, cove-lit ceiling with
    /// downlights, bronze handrails and a marble floor with LED edge light.
    /// Animate with <see cref="DoorOpenRatio"/> (0 closed, 1 open) and Top.
    /// </summary>
    public class ElevatorCar : Control
    {
        private float _doorOpenRatio;
        private string _carLabel = "LIFT A";

        public ElevatorCar()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);
            Size = new Size(150, 250);
            BackColor = Theme.Shaft;
        }

        [Category("Lift"), DefaultValue(0f)]
        public float DoorOpenRatio
        {
            get => _doorOpenRatio;
            set
            {
                float v = Math.Clamp(value, 0f, 1f);
                if (Math.Abs(v - _doorOpenRatio) > 0.0001f) { _doorOpenRatio = v; Invalidate(); }
            }
        }

        [Category("Lift"), DefaultValue("LIFT A")]
        public string CarLabel
        {
            get => _carLabel;
            set { _carLabel = value ?? string.Empty; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Theme.ParentBack(this));
            if (Width < 50 || Height < 80) return;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            // Brass portal frame
            var frame = new RectangleF(0.5f, 0.5f, Width - 1.5f, Height - 1.5f);
            using (var p = Theme.RoundRect(frame, 4))
            using (var fb = new LinearGradientBrush(frame, Theme.BrassLight, Theme.BrassDark, 0f))
            {
                fb.InterpolationColors = new ColorBlend
                {
                    Colors = new[] { Theme.BrassDark, Theme.BrassLight, Theme.Brass, Theme.BrassLight, Theme.BrassDark },
                    Positions = new[] { 0f, 0.2f, 0.5f, 0.8f, 1f }
                };
                g.FillPath(fb, p);
            }

            // Black-glass lintel with the car name
            const float lintelH = 22f;
            var lintel = new RectangleF(6, 5, Width - 12, lintelH);
            using (var lp = Theme.RoundRect(lintel, 3))
            using (var lb = new SolidBrush(Theme.Glass))
                g.FillPath(lb, lp);
            using (var hf = new Font("Segoe UI Semibold", 7.5f, FontStyle.Bold))
            using (var hb = new SolidBrush(Theme.GlassText))
            using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                g.DrawString(_carLabel.ToUpperInvariant(), hf, hb, lintel, sf);

            var opening = new RectangleF(9, lintelH + 10, Width - 18, Height - lintelH - 18);
            if (opening.Width < 20 || opening.Height < 20) return;

            if (_doorOpenRatio > 0.001f)
                DrawCabin(g, opening);

            float leafW = opening.Width / 2f * (1f - _doorOpenRatio);
            if (leafW > 0.5f)
            {
                DrawDoorLeaf(g, new RectangleF(opening.X, opening.Y, leafW, opening.Height));
                DrawDoorLeaf(g, new RectangleF(opening.Right - leafW, opening.Y, leafW, opening.Height));
            }

            using (var edge = new Pen(Theme.BrassDark, 1f))
                g.DrawRectangle(edge, opening.X, opening.Y, opening.Width, opening.Height);
        }

        private static float Lerp(float a, float b, float t) => a + (b - a) * t;

        /// <summary>One-point-perspective drawing of the luxury cabin.</summary>
        private static void DrawCabin(Graphics g, RectangleF o)
        {
            var state = g.Save();
            g.SetClip(o);

            // Back wall rectangle
            float bx = o.X + o.Width * 0.2f, bw = o.Width * 0.6f;
            float by = o.Y + o.Height * 0.11f, bh = o.Height * 0.69f;
            var back = new RectangleF(bx, by, bw, bh);

            // Ceiling with cove light and downlights
            var ceiling = new[] { new PointF(o.X, o.Y), new PointF(o.Right, o.Y), new PointF(back.Right, by), new PointF(bx, by) };
            using (var cb = new LinearGradientBrush(o, Color.FromArgb(214, 196, 166), Color.FromArgb(240, 228, 206), 90f))
                g.FillPolygon(cb, ceiling);
            using (var cove = new Pen(Color.FromArgb(200, 255, 236, 196), 2f))
                g.DrawLine(cove, bx, by, back.Right, by);
            foreach (float t in new[] { 0.3f, 0.7f })
            {
                float y = Lerp(o.Y, by, 0.5f);
                float x = Lerp(Lerp(o.X, bx, 0.5f), Lerp(o.Right, back.Right, 0.5f), t);
                using (var glow = new SolidBrush(Color.FromArgb(70, 255, 240, 210)))
                    g.FillEllipse(glow, x - 4, y - 2.5f, 8, 5);
                using (var dot = new SolidBrush(Color.White))
                    g.FillEllipse(dot, x - 1.8f, y - 1.1f, 3.6f, 2.2f);
            }

            // Side walls: linen with brass trim lines
            var left = new[] { new PointF(o.X, o.Y), new PointF(bx, by), new PointF(bx, back.Bottom), new PointF(o.X, o.Bottom) };
            var right = new[] { new PointF(o.Right, o.Y), new PointF(back.Right, by), new PointF(back.Right, back.Bottom), new PointF(o.Right, o.Bottom) };
            using (var lw = new LinearGradientBrush(new RectangleF(o.X, o.Y, bx - o.X + 1, o.Height), Color.FromArgb(196, 182, 160), Theme.Fabric, 0f))
                g.FillPolygon(lw, left);
            using (var rw = new LinearGradientBrush(new RectangleF(back.Right - 1, o.Y, o.Right - back.Right + 2, o.Height), Theme.Fabric, Color.FromArgb(196, 182, 160), 0f))
                g.FillPolygon(rw, right);

            using (var trim = new Pen(Theme.WithAlpha(Theme.Brass, 210), 1.2f))
            {
                foreach (float t in new[] { 0.33f, 0.72f })
                {
                    g.DrawLine(trim, Lerp(o.X, bx, t), Lerp(o.Y, by, t), Lerp(o.X, bx, t), Lerp(o.Bottom, back.Bottom, t));
                    g.DrawLine(trim, Lerp(o.Right, back.Right, t), Lerp(o.Y, by, t), Lerp(o.Right, back.Right, t), Lerp(o.Bottom, back.Bottom, t));
                }
            }

            // Black-glass car operating panel on the right wall
            float c0 = 0.42f, c1 = 0.62f;
            var cop = new[]
            {
                new PointF(Lerp(o.Right, back.Right, c0), Lerp(o.Y + o.Height * 0.2f, by + bh * 0.2f, c0)),
                new PointF(Lerp(o.Right, back.Right, c1), Lerp(o.Y + o.Height * 0.2f, by + bh * 0.2f, c1)),
                new PointF(Lerp(o.Right, back.Right, c1), Lerp(o.Bottom - o.Height * 0.12f, back.Bottom - bh * 0.1f, c1)),
                new PointF(Lerp(o.Right, back.Right, c0), Lerp(o.Bottom - o.Height * 0.12f, back.Bottom - bh * 0.1f, c0)),
            };
            using (var copB = new SolidBrush(Theme.Glass)) g.FillPolygon(copB, cop);
            float copX = (cop[0].X + cop[1].X) / 2f;
            for (int i = 0; i < 4; i++)
            {
                float y = Lerp(cop[0].Y, cop[3].Y, 0.25f + i * 0.13f);
                using (var key = new Pen(i == 3 ? Theme.KeyGreen : Color.FromArgb(200, 255, 255, 255), 0.8f))
                    g.DrawEllipse(key, copX - 1.6f, y - 1.6f, 3.2f, 3.2f);
            }

            // Back wall: walnut panels around a backlit mirror
            Theme.PaintWood(g, Rectangle.Round(back));
            var mirror = new RectangleF(bx + bw * 0.24f, by + bh * 0.05f, bw * 0.52f, bh * 0.92f);
            for (int i = 4; i >= 1; i--)
            {
                using (var halo = new Pen(Color.FromArgb(28 + (4 - i) * 26, 255, 228, 176), i * 2f))
                    g.DrawRectangle(halo, mirror.X, mirror.Y, mirror.Width, mirror.Height);
            }
            using (var mb = new LinearGradientBrush(mirror, Color.FromArgb(150, 140, 126), Color.FromArgb(104, 97, 88), 90f))
                g.FillRectangle(mb, mirror);
            // reflected doorway inside the mirror
            var refl = new RectangleF(mirror.X + mirror.Width * 0.38f, mirror.Y + mirror.Height * 0.32f,
                                      mirror.Width * 0.3f, mirror.Height * 0.48f);
            using (var rb = new SolidBrush(Color.FromArgb(150, 128, 92, 58))) g.FillRectangle(rb, refl);
            using (var mf = new Pen(Color.FromArgb(240, 255, 240, 210), 1.2f))
                g.DrawRectangle(mf, mirror.X, mirror.Y, mirror.Width, mirror.Height);

            // Marble floor with LED edge light
            var floor = new[] { new PointF(o.X, o.Bottom), new PointF(bx, back.Bottom), new PointF(back.Right, back.Bottom), new PointF(o.Right, o.Bottom) };
            using (var fl = new LinearGradientBrush(new RectangleF(o.X, back.Bottom - 1, o.Width, o.Bottom - back.Bottom + 2), Theme.MarbleDark, Theme.Marble, 90f))
                g.FillPolygon(fl, floor);
            using (var vein = new Pen(Color.FromArgb(40, 255, 255, 255), 0.8f))
            {
                g.DrawLine(vein, Lerp(bx, o.X, 0.2f), Lerp(back.Bottom, o.Bottom, 0.3f), Lerp(back.Right, o.Right, 0.4f), Lerp(back.Bottom, o.Bottom, 0.7f));
                g.DrawLine(vein, Lerp(bx, o.X, 0.6f), Lerp(back.Bottom, o.Bottom, 0.8f), Lerp(back.Right, o.Right, 0.1f), Lerp(back.Bottom, o.Bottom, 0.35f));
            }
            using (var led = new Pen(Color.FromArgb(220, 255, 226, 168), 1.6f))
            {
                g.DrawLine(led, o.X, o.Bottom, bx, back.Bottom);
                g.DrawLine(led, bx, back.Bottom, back.Right, back.Bottom);
                g.DrawLine(led, back.Right, back.Bottom, o.Right, o.Bottom);
            }

            // Bronze handrails (back + both sides)
            float railFront = o.Y + o.Height * 0.6f;
            float railBack = by + bh * 0.58f;
            using (var rail = new Pen(Color.FromArgb(48, 40, 34), 2.6f) { StartCap = LineCap.Round, EndCap = LineCap.Round })
            {
                g.DrawLine(rail, bx + 4, railBack, back.Right - 4, railBack);
                g.DrawLine(rail, Lerp(o.X, bx, 0.15f), Lerp(railFront, railBack, 0.15f), Lerp(o.X, bx, 0.85f), Lerp(railFront, railBack, 0.85f));
                g.DrawLine(rail, Lerp(o.Right, back.Right, 0.15f), Lerp(railFront, railBack, 0.15f), Lerp(o.Right, back.Right, 0.32f), Lerp(railFront, railBack, 0.32f));
            }

            g.Restore(state);
        }

        private static void DrawDoorLeaf(Graphics g, RectangleF r)
        {
            if (r.Width < 1f) return;
            using (var b = new LinearGradientBrush(r, Theme.BrassDark, Theme.Brass, 0f))
            {
                b.InterpolationColors = new ColorBlend
                {
                    Colors = new[] { Color.FromArgb(150, 122, 82), Color.FromArgb(226, 205, 164), Color.FromArgb(186, 158, 112), Color.FromArgb(140, 112, 74) },
                    Positions = new[] { 0f, 0.35f, 0.7f, 1f }
                };
                g.FillRectangle(b, r);
            }
            using (var brush = new Pen(Color.FromArgb(26, 255, 255, 255), 1f))
                for (float x = r.X + 2; x < r.Right; x += 3f)
                    g.DrawLine(brush, x, r.Y, x, r.Bottom);
            using (var edge = new Pen(Color.FromArgb(140, 70, 52, 30), 1f))
                g.DrawRectangle(edge, r.X, r.Y, r.Width, r.Height);
        }
    }
}
