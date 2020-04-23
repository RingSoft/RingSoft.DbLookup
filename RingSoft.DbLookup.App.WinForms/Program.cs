using RingSoft.DbLookup.App.WinForms.Forms;
using System;
using System.Windows.Forms;

namespace RingSoft.DbLookup.App.WinForms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var winFormsAppStart = new WinFormsAppStart();
            winFormsAppStart.StartApp("WinForms", args);
        }
    }
}
