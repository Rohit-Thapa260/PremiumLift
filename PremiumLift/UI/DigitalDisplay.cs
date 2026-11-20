using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace PremiumLift.UI
{
    public enum TravelDirection { None, Up, Down }

    /// <summary>
    /// Floor indicator set into black glass: crisp white digit with a soft
    /// glow and up/down arrows that light green while travelling.
    /// </summary>
    public class DigitalDisplay : Control
    {
        private string _displayText = "0";
        private string _caption = "FLOOR";
        private TravelDirection _direction = TravelDirection.None;
        private bool _showArrows = true;
        private bool _compactRow;

        public DigitalDisplay()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);
            Size = new Size(200, 100);
            BackColor = Theme.Glass;
        }

        [Category("Lift"), DefaultValue("0")]
        public string DisplayText
        {
            get => _displayText;
            set { value = value ?? string.Empty; if (_displayText != value) { _displayText = value; Invalidate(); } }
        }

        [Category("Lift"), DefaultValue("FLOOR")]
        public string Caption
        {
            get => _caption;
            set { _caption = value ?? string.Empty; Invalidate(); }
        }

        [Category("Lift"), DefaultValue(TravelDirection.None)]
        public TravelDirection Direction
        {
            get => _direction;
            set { if (_direction != value) { _direction = value; Invalidate(); } }
        }

        [Category("Lift"), DefaultValue(true)]
        public bool ShowArrows
        {
            get => _showArrows;
            set { _showArrows = value; Invalidate(); }
        }

        /// <summary>Single-row layout: caption left, arrows, floor number right.</summary>
        [Category("Lift"), DefaultValue(false)]
        public bool CompactRow
        {
            get => _compactRow;
            set { _compactRow = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Theme.ParentBack(this));
            if (Width < 20 || Height < 20) return;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            var r = new RectangleF(0.5f, 0.5f, Width - 1.5f, Height - 1.5f);
            using (var p = Theme.RoundRect(r, 8))
            {
                using (var fill = new SolidBrush(Color.FromArgb(4, 4, 5)))
                    g.FillPath(fill, p);
                using (var edge = new Pen(Theme.GlassEdge, 1f))
                    g.DrawPath(edge, p);
            }

            if (_compactRow) { PaintRow(g, r); return; }

            using (var cf = new Font("Segoe UI Semibold", Math.Max(6.5f, Height * 0.075f), FontStyle.Bold))
            using (var cb = new SolidBrush(Theme.GlassMuted))
                g.DrawString(_caption.ToUpperInvariant(), cf, cb, r.X + 10, r.Y + 8);

            float arrowW = _showArrows ? r.Height * 0.36f : 0f;
            if (_showArrows)
            {
                float cx = r.X + 14 + arrowW / 2f;
                float midY = r.Y + r.Height * 0.58f;
                float s = arrowW * 0.34f;
                DrawArrow(g, cx, midY - s * 1.4f, s, true, _direction == TravelDirection.Up);
                DrawArrow(g, cx, midY + s * 1.4f, s, false, _direction == TravelDirection.Down);
            }

            var textRect = new RectangleF(r.X + arrowW + 12, r.Y + r.Height * 0.12f,
                                          r.Width - arrowW - 20, r.Height * 0.88f);
            DrawDigits(g, textRect, r.Height * 0.62f);
        }

        private void PaintRow(Graphics g, RectangleF r)
        {
            using (var sf = new StringFormat { LineAlignment = StringAlignment.Center })
            using (var cf = new Font("Segoe UI Semibold", 8f, FontStyle.Bold))
            using (var cb = new SolidBrush(Theme.GlassText))
                g.DrawString(_caption.ToUpperInvariant(), cf, cb,
                             new RectangleF(r.X + 10, r.Y, r.Width * 0.5f, r.Height), sf);

            float digitW = r.Height * 0.9f;
            float midY = r.Y + r.Height / 2f;
            float s = r.Height * 0.13f;
            if (_showArrows)
            {
                float ax = r.Right - digitW - s * 2.4f;
                DrawArrow(g, ax, midY - s * 1.4f, s, true, _direction == TravelDirection.Up);
                DrawArrow(g, ax, midY + s * 1.4f, s, false, _direction == TravelDirection.Down);
            }
            DrawDigits(g, new RectangleF(r.Right - digitW - 4, r.Y + 1, digitW, r.Height), r.Height * 0.66f);
        }

        private void DrawDigits(Graphics g, RectangleF rect, float px)
        {
            using (var df = new Font("Segoe UI Light", Math.Max(8f, px), FontStyle.Regular, GraphicsUnit.Pixel))
            using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                using (var glow = new SolidBrush(Color.FromArgb(40, 255, 255, 255)))
                {
                    var offsets = new[] { new PointF(-1.2f, 0f), new PointF(1.2f, 0f), new PointF(0f, -1.2f), new PointF(0f, 1.2f) };
                    foreach (var o in offsets)
                    {
                        var rr = rect; rr.Offset(o.X, o.Y);
                        g.DrawString(_displayText, df, glow, rr, sf);
                    }
                }
                using (var b = new SolidBrush(Color.White))
                    g.DrawString(_displayText, df, b, rect, sf);
            }
        }

        private static void DrawArrow(Graphics g, float cx, float cy, float s, bool up, bool active)
        {
            PointF[] pts = up
                ? new[] { new PointF(cx, cy - s), new PointF(cx + s, cy + s * 0.6f), new PointF(cx - s, cy + s * 0.6f) }
                : new[] { new PointF(cx, cy + s), new PointF(cx + s, cy - s * 0.6f), new PointF(cx - s, cy - s * 0.6f) };
            using (var b = new SolidBrush(active ? Theme.KeyGreen : Color.FromArgb(38, 255, 255, 255)))
                g.FillPolygon(b, pts);
        }
    }
}
