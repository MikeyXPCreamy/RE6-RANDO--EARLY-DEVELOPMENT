using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace MikeyXPCreamy.BioRand6
{
    internal sealed class MixPie : Control
    {
        private const int MaximumPieSize = 70;
        private const int MaximumRowHeight = 20;
        private const int MinimumRowHeight = 12;
        private const int SwatchSize = 10;

        public sealed class Slice
        {
            public string Name;
            public int Weight;
            public Color Color;
        }

        public List<Slice> Slices = new List<Slice>();
        public bool Dim;

        public MixPie()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            BackColor = Theme.Card;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int size = Math.Min(Height - 4, Theme.Scale(this, MaximumPieSize));
            var circle = new Rectangle(2, (Height - size) / 2, size, size);
            int total = Slices.Sum(slice => slice.Weight);
            DrawPie(g, circle, total);

            int row = Math.Min(Theme.Scale(this, MaximumRowHeight), Math.Max(MinimumRowHeight, (Height - 2) / Math.Max(1, Slices.Count)));
            int x = circle.Right + 8;
            int y = Math.Max(0, (Height - row * Slices.Count) / 2);
            foreach (Slice slice in Slices)
            {
                bool off = slice.Weight == 0;
                using (var brush = new SolidBrush(Dim || off ? Theme.Line : slice.Color))
                    g.FillRectangle(brush, x, y + 4, SwatchSize, SwatchSize);
                string text = slice.Name + " " + (off ? "off" : (int)Math.Round(100.0 * slice.Weight / total) + "%");
                TextRenderer.DrawText(g, text, Font, new Rectangle(x + 14, y, Width - x - 14, row), Dim || off ? Theme.Muted : Theme.Ink, TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
                y += row;
            }
        }

        private void DrawPie(Graphics g, Rectangle circle, int total)
        {
            if (total == 0)
            {
                using (var pen = new Pen(Theme.Line, 2))
                    g.DrawEllipse(pen, circle);
            }
            float start = -90F;
            foreach (Slice slice in Slices.Where(slice => slice.Weight > 0))
            {
                float sweep = 360F * slice.Weight / total;
                using (var brush = new SolidBrush(Dim ? Theme.Line : slice.Color))
                    g.FillPie(brush, circle, start, sweep);
                if (sweep < 360F)
                {
                    using (var pen = new Pen(Theme.Card, 2))
                        g.DrawPie(pen, circle, start, sweep);
                }
                start += sweep;
            }
        }
    }
}
