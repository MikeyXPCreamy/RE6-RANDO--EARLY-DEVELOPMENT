using System.Drawing;
using System.Windows.Forms;

namespace MikeyXPCreamy.BioRand6
{
    internal sealed class ThemeMenuColors : ProfessionalColorTable
    {
        public override Color ToolStripDropDownBackground
        {
            get { return Theme.Card; }
        }

        public override Color MenuBorder
        {
            get { return Theme.Line; }
        }

        public override Color MenuItemBorder
        {
            get { return Theme.Accent; }
        }

        public override Color MenuItemSelected
        {
            get { return Theme.Primary; }
        }

        public override Color MenuItemSelectedGradientBegin
        {
            get { return Theme.Primary; }
        }

        public override Color MenuItemSelectedGradientEnd
        {
            get { return Theme.Primary; }
        }
    }
}
