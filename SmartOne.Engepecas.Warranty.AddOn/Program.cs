using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SBO.Hub;
using SmartOne.Engepecas.Warranty.Core.BLL;

namespace SmartOne.Engepecas.Warranty.AddOn
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Application.Exit();
                return;
            }

            var sboApp = new SBOApp(args[0], $"{Application.StartupPath}\\SmartOne.Engepecas.Warranty.Core.dll");
            sboApp.InitializeApplication();

            SBOApp.AutoTranslateHana = true;

            InitializeBLL.Initialize();
            var oListener = new SBO.Hub.Services.Listener();
            var oThread = new Thread(new ThreadStart(oListener.startListener));
            oThread.IsBackground = true;
            oThread.Start();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run();
        }
    }
}
