using System;
using System.Collections.Generic;
//using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;

namespace Presentation
{
    static class Program
    {
        private static bool FirstInstance
        {
            get
            {
                bool created;
                string name = Assembly.GetEntryAssembly().FullName;
                // created will be True if the current thread creates and owns the mutex.
                // Otherwise created will be False if a previous instance already exists.
                Mutex mutex = new Mutex(true, name, out created);
                return created;
            }
        }
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (FirstInstance)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormLogin.frmLogin());
                //Application.Run(new Presentation.InfGeográfica.frmCto());
            }
            else
            {
                MessageBox.Show("Ya existe una instancia en ejecución", "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Application.Exit();
            }
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FormLogin.frmLogin());
            ////Application.Run(new MDIParentPrincipal());


        }
    }
}
