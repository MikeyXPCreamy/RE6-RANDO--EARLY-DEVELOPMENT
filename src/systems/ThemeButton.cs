using System.Drawing;
using System.Windows.Forms;

namespace MikeyXPCreamy.BioRand6
{
    internal sealed class ThemeButton : Button
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            if (Enabled)
            {
                base.OnPaint(e);
                return;
            }
            e.Graphics.Clear(BackColor);
            using (var border = new Pen(FlatAppearance.BorderColor))
                e.Graphics.DrawRectangle(border, 0, 0, Width - 1, Height - 1);
            TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, Theme.Muted, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
        }
    }
}
