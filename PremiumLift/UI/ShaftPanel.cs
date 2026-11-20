using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace PremiumLift.UI
{
    /// <summary>
    /// Lift shaft for a two-floor building (Floor 1 on top, Ground floor G below):
    /// warm dark shaft, bronze guide rails, marble floor slabs with a brass
    /// nosing and the hoist cables down to the car.
    /// </summary>
    public class ShaftPanel : Panel
    {
        public const int FloorCount = 2;
        public const int SlabThickness = 8;

        public ShaftPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            BackColor = Theme.Shaft;
            Size = new Size(180, 634);
        }

        [Browsable(false)]
        public int ZoneHeight => Height / FloorCount;

        /// <summary>Car Top that rests level with a floor (0 = ground, 1 = top).</summary>
        public int GetCarTopForFloor(int floor, int carHeight)
        {
            int zoneIndexFromTop = (FloorCount - 1) - floor;
            int zoneBottom = ZoneHeight * (zoneIndexFromTop + 1);
            return zoneBottom - SlabThickness - carHeight;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            var g = e.Graphics;
            if (Width < 10 || Height < 10) { g.Clear(BackColor); return; }

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            using (var bg = new LinearGradientBrush(ClientRectangle, Color.FromArgb(18, 14, 11), Color.FromArgb(38, 31, 25), 0f))
            {
                bg.InterpolationColors = new ColorBlend
                {
                    Colors = new[] { Color.FromArgb(18, 14, 11), Color.FromArgb(38, 31, 25), Color.FromArgb(18, 14, 11) },
                    Positions = new[] { 0f, 0.5f, 1f }
                };
                g.FillRectangle(bg, ClientRectangle);
            }

            using (var rail = new Pen(Theme.WithAlpha(Theme.Brass, 110), 3f))
            using (var bracket = new Pen(Theme.WithAlpha(Theme.Brass, 50), 1f))
            {
                foreach (int x in new[] { 6, Width - 7 })
                {
                    g.DrawLine(rail, x, 0, x, Height);
                    for (int y = 20; y < Height; y += 46)
                        g.DrawLine(bracket, x - 5, y, x + 5, y);
                }
            }

            var car = Controls.OfType<ElevatorCar>().FirstOrDefault();
            if (car != null)
            {
                using (var cable = new Pen(Theme.WithAlpha(Theme.BrassLight, 90), 1.2f))
                {
                    int cx = car.Left + car.Width / 2;
                    foreach (int dx in new[] { -8, 0, 8 })
                        g.DrawLine(cable, cx + dx, 0, cx + dx, car.Top);
                }
            }

            using (var lf = new Font("Segoe UI Semibold", 7f, FontStyle.Bold))
            using (var lb = new SolidBrush(Theme.WithAlpha(Theme.BrassLight, 170)))
            {
                for (int zone = 0; zone < FloorCount; zone++)
                {
                    int floorNumber = (FloorCount - 1) - zone;
                    int slabY = ZoneHeight * (zone + 1) - SlabThickness;
                    var slab = new Rectangle(0, slabY, Width, SlabThickness);
                    using (var sb = new LinearGradientBrush(slab, Theme.Marble, Theme.MarbleDark, 90f))
                        g.FillRectangle(sb, slab);
                    using (var gl = new Pen(Theme.Brass, 1.5f))
                        g.DrawLine(gl, 0, slabY, Width, slabY);

                    g.DrawString(floorNumber == 0 ? "GROUND  ·  G" : $"LEVEL {floorNumber}", lf, lb, 14, ZoneHeight * zone + 8);
                }
            }

            using (var border = new Pen(Theme.BrassDark, 1f))
                g.DrawRectangle(border, 0, 0, Width - 1, Height - 1);
        }
    }
}
