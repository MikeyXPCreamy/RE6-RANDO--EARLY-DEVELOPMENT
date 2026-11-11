using System;
using System.Drawing;
using System.Windows.Forms;

namespace MikeyXPCreamy.BioRand6
{
    internal static class Theme
    {
        public static readonly Color Background = Color.FromArgb(8, 9, 23);
        public static readonly Color Card = Color.FromArgb(16, 18, 36);
        public static readonly Color Ink = Color.FromArgb(136, 131, 182);
        public static readonly Color Muted = Color.FromArgb(101, 97, 140);
        public static readonly Color Accent = Ink;
        public static readonly Color Primary = Color.FromArgb(41, 35, 67);
        public static readonly Color Line = Color.FromArgb(52, 48, 84);
        public static readonly Color Rail = Color.FromArgb(3, 3, 11);
        public static readonly Color ButtonFill = Color.FromArgb(24, 25, 46);
        public static readonly Color ButtonBorder = Color.FromArgb(66, 61, 91);
        public static readonly Color RailHover = Color.FromArgb(12, 13, 30);

        private const string FontName = "Segoe UI";
        private const float BodySize = 9F;
        private const float DesignDpi = 96F;

        public static Font BodyFont()
        {
            return new Font(FontName, BodySize);
        }

        public static Font BoldFont()
        {
            return new Font(FontName, BodySize, FontStyle.Bold);
        }

        public static int Scale(Control control, int pixels)
        {
            return (int)Math.Round(pixels * control.DeviceDpi / DesignDpi);
        }

        public static Label HeaderLabel(string text)
        {
            return new Label
            {
                Text = text,
                UseMnemonic = false,
                AutoSize = true,
                Font = BodyFont(),
                ForeColor = Accent,
                BackColor = Color.Transparent,
                Padding = new Padding(2, 0, 2, 0)
            };
        }

        public static CheckBox HeaderCheck(string text)
        {
            var check = new CheckBox
            {
                Text = text,
                UseMnemonic = false,
                AutoSize = true,
                Checked = true,
                Font = BodyFont(),
                ForeColor = Accent,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            StyleCheck(check);
            return check;
        }

        public static CheckBox RowCheck(string text, int x, int y, int width)
        {
            var check = new CheckBox
            {
                Text = text,
                UseMnemonic = false,
                AutoSize = false,
                Size = new Size(width, 22),
                Location = new Point(x, y),
                Font = BodyFont(),
                ForeColor = Ink,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                AutoEllipsis = true
            };
            StyleCheck(check);
            return check;
        }

        private static void StyleCheck(CheckBox check)
        {
            check.FlatAppearance.BorderColor = Accent;
            check.FlatAppearance.CheckedBackColor = Primary;
        }

        public static Label LabelAt(string text, int x, int y, int width, int height, float size, Color color, FontStyle style)
        {
            return new Label
            {
                Text = text,
                UseMnemonic = false,
                Location = new Point(x, y),
                Size = new Size(width, height),
                Font = new Font(FontName, size, style),
                ForeColor = color,
                BackColor = Color.Transparent
            };
        }

        public static Label TextAt(string text, int x, int y, int width, int height)
        {
            return LabelAt(text, x, y, width, height, BodySize, Ink, FontStyle.Regular);
        }

        public static Label NoteAt(string text, int x, int y, int width, int height)
        {
            return LabelAt(text, x, y, width, height, BodySize, Muted, FontStyle.Regular);
        }

        public static Button ButtonAt(string text, int x, int y, int width, int height, bool primary)
        {
            var button = new ThemeButton
            {
                Text = text,
                UseMnemonic = false,
                Location = new Point(x, y),
                Size = new Size(width, height),
                FlatStyle = FlatStyle.Flat,
                BackColor = primary ? Primary : ButtonFill,
                ForeColor = Ink,
                Font = BodyFont(),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = primary ? Accent : ButtonBorder;
            return button;
        }

        public static LinkLabel Link(string text)
        {
            return new LinkLabel
            {
                Text = text,
                AutoSize = true,
                Font = BodyFont(),
                LinkColor = Accent,
                ActiveLinkColor = Color.White,
                VisitedLinkColor = Accent,
                DisabledLinkColor = Line,
                LinkBehavior = LinkBehavior.HoverUnderline,
                BackColor = Color.Transparent
            };
        }
    }
}
