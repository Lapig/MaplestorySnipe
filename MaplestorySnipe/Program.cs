using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace MaplestorySnipe
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new Form1());
            }
            catch (NullReferenceException) { MessageBox.Show("Please open the game client."); }
            catch (IndexOutOfRangeException) { MessageBox.Show("Please open the game client"); }
            catch (Exception e) { MessageBox.Show("Uknown Exception: " + e); };
        }
    }
}
