using System;
using System.IO;
using System.Windows.Forms;

namespace MikeyXPCreamy.BioRand6
{
    internal static class Program
    {
        [STAThread]
        private static int Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                if (AppDomain.CurrentDomain.BaseDirectory.StartsWith(Path.GetTempPath(), StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Extract the BioRand 6 zip first, then open biorand6.exe from the extracted folder.", "BioRand 6", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return 0;
                }
                using (var form = new MainForm())
                    Application.Run(form);
                return 0;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "BioRand 6", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 1;
            }
        }
    }
}
