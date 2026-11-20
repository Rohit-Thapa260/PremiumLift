using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace PremiumLift.UI
{
    public enum LuxShape { Round, Pill }

    /// <summary>
    /// Round  = lift key on black glass: thin white ring, white glyph,
    ///          green ring + glow when <see cref="IsLit"/> (request registered).
    /// Pill   = app action button: solid brass when <see cref="IsPrimary"/>,
    ///          otherwise a brass outline.
    /// </summary>
    public class LuxButton : Button
    {
        private bool _isLit, _isPrimary, _hover, _pressed;
        private LuxShape _shape = LuxShape.Round;
        private string _caption = string.Empty;

        public LuxButton()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = Theme.Glass;
            ForeColor = Theme.GlassText;
            Cursor = Cursors.Hand;
            Size = new Size(80, 80);
            UseVisualStyleBackColor = false;
        }

        [Category("Lift"), DefaultValue(false)]
        public bool IsLit
        {
            get => _isLit;
            set { if (_isLit != value) { _isLit = value; Invalidate(); } }
        }

        [Category("Lift"), DefaultValue(false)]
        public bool IsPrimary
        {
            get => _isPrimary;
            set { _isPrimary = value; Invalidate(); }
        }

        [Category("Lift"), DefaultValue(LuxShape.Round)]
        public LuxShape Shape
        {
            get => _shape;
            set { _shape = value; UpdateRegion(); Invalidate(); }
        }

        [Category("Lift"), DefaultValue("")]
        public string Caption
        {
            get => _caption;
            set { _caption = value ?? string.Empty; Invalidate(); }
        }

        protected override bool ShowFocusCues => false;

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; _pressed = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnMouseDown(MouseEventArgs e) { _pressed = true; Invalidate(); base.OnMouseDown(e); }
        protected override void OnMouseUp(MouseEventArgs e) { _pressed = false; Invalidate(); base.OnMouseUp(e); }
        protected override void OnEnabledChanged(EventArgs e) { Invalidate(); base.OnEnabledChanged(e); }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateRegion();
        }

        private GraphicsPath ShapePath(RectangleF r)
        {
            if (_shape == LuxShape.Pill) return Theme.RoundRect(r, r.Height / 2f);
            var p = new GraphicsPath();
            p.AddEllipse(r);
            return p;
        }

        private void UpdateRegion()
        {
            if (Width <= 0 || Height <= 0) return;
            using (var p = ShapePath(new RectangleF(0, 0, Width, Height)))
            {
                var old = Region;
                Region = new Region(p);
                old?.Dispose();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Theme.ParentBack(this));
            if (Width < 6 || Height < 6) return;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            if (_shape == LuxShape.Round) PaintKey(g);
            else PaintPill(g);
        }

        private void PaintKey(Graphics g)
        {
            var outer = new RectangleF(2, 2, Width - 5, Height - 5);

            using (var path = ShapePath(outer))
            {
                // Glass face
                Color top = _pressed ? Color.FromArgb(40, 40, 44) : Color.FromArgb(24, 24, 27);
                using (var face = new LinearGradientBrush(outer, top, Theme.Glass, 90f))
                    g.FillPath(face, path);

                if (_isLit)
                {
                    using (var glow = new PathGradientBrush(path))
                    {
                        glow.CenterColor = Theme.WithAlpha(Theme.KeyGreen, 70);
                        glow.SurroundColors = new[] { Theme.WithAlpha(Theme.KeyGreen, 0) };
                        g.FillPath(glow, path);
                    }
                }

                Color ring = !Enabled ? Theme.GlassEdge
                           : _isLit ? Theme.KeyGreen
                           : _hover ? Color.White
                           : Theme.WithAlpha(Theme.GlassText, 190);
                using (var pen = new Pen(ring, _isLit ? 2.6f : 1.8f))
                    g.DrawPath(pen, path);
            }

            Color textColor = !Enabled ? Theme.GlassMuted : _isLit ? Theme.KeyGreen : Theme.GlassText;
            DrawLabel(g, outer, textColor, _isLit ? Theme.KeyGreen : Theme.GlassMuted);
        }

        private void PaintPill(Graphics g)
        {
            var r = new RectangleF(1, 1, Width - 3, Height - 3);
            using (var path = ShapePath(r))
            {
                if (_isPrimary)
                {
                    Color a = _pressed ? Theme.Brass : Theme.BrassLight;
                    Color b = _pressed ? Theme.BrassDark : Theme.Brass;
                    using (var fill = new LinearGradientBrush(r, a, b, 90f))
                        g.FillPath(fill, path);
                }
                else if (_hover || _pressed)
                {
                    using (var fill = new SolidBrush(Theme.WithAlpha(Theme.Brass, _pressed ? 80 : 40)))
                        g.FillPath(fill, path);
                }

                using (var pen = new Pen(_isPrimary ? Theme.BrassDark : Theme.Brass, 1.6f))
                    g.DrawPath(pen, path);
            }
            DrawLabel(g, r, Enabled ? Theme.Espresso : Theme.Taupe, Theme.Taupe);
        }

        private void DrawLabel(Graphics g, RectangleF r, Color textColor, Color captionColor)
        {
            using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            using (var tb = new SolidBrush(textColor))
            {
                if (_caption.Length == 0)
                {
                    g.DrawString(Text, Font, tb, r, sf);
                    return;
                }
                var main = new RectangleF(r.X, r.Y, r.Width, r.Height * 0.68f);
                var cap = new RectangleF(r.X, r.Y + r.Height * 0.58f, r.Width, r.Height * 0.3f);
                g.DrawString(Text, Font, tb, main, sf);
                using (var cf = new Font("Segoe UI Semibold", Math.Max(6f, Font.Size * 0.45f), FontStyle.Bold))
                using (var cb = new SolidBrush(captionColor))
                    g.DrawString(_caption.ToUpperInvariant(), cf, cb, cap, sf);
            }
        }
    }
}
