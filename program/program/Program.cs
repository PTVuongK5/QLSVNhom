using System;
using System.Windows.Forms;
using program.View;

namespace program
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Start the application with the login form
            Application.Run(new frmLogin());

        }
    }
}
