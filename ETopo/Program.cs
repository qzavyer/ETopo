using System;
using System.Windows.Forms;

namespace ETopo
{
    static class Program
    {
        /// <summary>
        /// точка входа в приложение
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrMain());
        }
    }
}
