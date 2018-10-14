using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NSites.ApplicationObjects.UserInterfaces;

namespace FrameWork
{
    static class ApplicationStarter
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        
        
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LogInUI());
            //Application.Run(new SoftwareLicenseUI(""));
        }
    }
}
