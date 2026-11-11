using System.Drawing;
using System.Windows.Forms;

namespace MikeyXPCreamy.BioRand6
{
    internal sealed class ThemeCombo : ComboBox
    {
        private const int WmPaint = 0x000F;
        private const int WmPrintClient = 0x0318;

        public ThemeCombo(string accessibleName, int width)
        {
            Width = width;
            DropDownStyle = ComboBoxStyle.DropDownList;
            FlatStyle = FlatStyle.Flat;
            DrawMode = DrawMode.OwnerDrawFixed;
            BackColor = Theme.Background;
            ForeColor = Theme.Ink;
            Font = Theme.BodyFont();
            AccessibleName = accessibleName;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;
            bool selected = (e.State & DrawItemState.Selected) != 0 && (e.State & DrawItemState.ComboBoxEdit) == 0;
            using (var fill = new SolidBrush(selected ? Theme.Primary : Theme.Background))
                e.Graphics.FillRectangle(fill, e.Bounds);
            Color color = selected ? Color.White : Enabled ? Theme.Ink : Theme.Muted;
            TextRenderer.DrawText(e.Graphics, (string)Items[e.Index], Font, Rectangle.Inflate(e.Bounds, -3, 0), color, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (Enabled || (m.Msg != WmPaint && m.Msg != WmPrintClient))
                return;
            using (Graphics g = m.Msg == WmPrintClient ? Graphics.FromHdc(m.WParam) : CreateGraphics())
                PaintDisabledFrame(g);
        }

        private void PaintDisabledFrame(Graphics g)
        {
            int arrowWidth = SystemInformation.GetHorizontalScrollBarArrowWidthForDpi(DeviceDpi);
            int width = ClientSize.Width;
            int height = ClientSize.Height;
            var field = new Rectangle(3, 3, width - arrowWidth - 6, height - 6);
            using (var frame = new Region(ClientRectangle))
            using (var fill = new SolidBrush(Theme.Background))
            {
                frame.Exclude(field);
                g.FillRegion(fill, frame);
            }
            using (var border = new Pen(Theme.Line))
                g.DrawRectangle(border, 0, 0, width - 1, height - 1);

            int x = width - arrowWidth - 1 + arrowWidth / 2 + arrowWidth % 2;
            int y = 1 + (height - 2) / 2;
            int reach = LogicalToDeviceUnits(2);
            using (var ink = new SolidBrush(Theme.Muted))
                g.FillPolygon(ink, new[] { new Point(x - reach, y - 1), new Point(x + reach + 1, y - 1), new Point(x, y + reach) });
        }
    }
}
