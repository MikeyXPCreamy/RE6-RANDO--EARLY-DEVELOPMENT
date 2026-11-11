using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MikeyXPCreamy.BioRand6
{
    internal sealed class ShareSlider : Control
    {
        private const int MinimumLabelWidth = 26;
        private const int KnobRadius = 6;
        private const int FocusRadius = 8;

        private int value;

        public Color Swatch = Theme.Ink;
        public int Minimum;
        public int Maximum = SeedSetup.MaxShare;
        public Func<int, string> Format = number => number.ToString();

        public event EventHandler ValueChanged;

        public ShareSlider()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.Selectable, true);
            TabStop = true;
            Cursor = Cursors.Hand;
            BackColor = Theme.Card;
            AccessibleRole = AccessibleRole.Slider;
        }

        public int Value
        {
            get { return value; }
            set
            {
                int limited = Math.Max(Minimum, Math.Min(Maximum, value));
                if (limited == this.value)
                    return;
                this.value = limited;
                AccessibleDescription = limited.ToString();
                Invalidate();
                if (ValueChanged != null)
                    ValueChanged(this, EventArgs.Empty);
            }
        }

        private Rectangle Track
        {
            get
            {
                int label = Theme.Scale(this, MinimumLabelWidth);
                for (int number = Minimum; number <= Maximum; number++)
                    label = Math.Max(label, TextRenderer.MeasureText(Format(number), Font).Width + 2);
                return new Rectangle(6, Height / 2 - 3, Math.Max(10, Width - label - 12), 6);
            }
        }

        private void SetFrom(int x)
        {
            Rectangle track = Track;
            Value = Minimum + (int)Math.Round((x - track.Left) * (double)(Maximum - Minimum) / track.Width);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Focus();
            if (Enabled)
                SetFrom(e.X);
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (Enabled && e.Button == MouseButtons.Left)
                SetFrom(e.X);
            base.OnMouseMove(e);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            return keyData == Keys.Left || keyData == Keys.Right || base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (Enabled)
            {
                if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Down)
                    Value--;
                if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Up)
                    Value++;
            }
            base.OnKeyDown(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            Invalidate();
            base.OnEnabledChanged(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            Invalidate();
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            Invalidate();
            base.OnLostFocus(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle track = Track;
            using (var back = new SolidBrush(Theme.Background))
                g.FillRectangle(back, track);
            int filled = (int)Math.Round(track.Width * (value - Minimum) / (double)Math.Max(1, Maximum - Minimum));
            using (var fill = new SolidBrush(Enabled ? Swatch : Theme.Line))
                g.FillRectangle(fill, track.Left, track.Top, filled, track.Height);
            using (var pen = new Pen(Theme.Line))
                g.DrawRectangle(pen, track);

            int knob = track.Left + filled;
            using (var brush = new SolidBrush(Enabled ? Color.White : Theme.Muted))
                g.FillEllipse(brush, knob - KnobRadius, Height / 2 - KnobRadius, KnobRadius * 2, KnobRadius * 2);
            if (Focused)
            {
                using (var pen = new Pen(Theme.Accent))
                    g.DrawEllipse(pen, knob - FocusRadius, Height / 2 - FocusRadius, FocusRadius * 2, FocusRadius * 2);
            }
            var valueArea = new Rectangle(track.Right + 4, 0, Width - track.Right - 4, Height);
            TextRenderer.DrawText(g, Format(value), Font, valueArea, Enabled ? Theme.Ink : Theme.Muted, TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
        }
    }
}
