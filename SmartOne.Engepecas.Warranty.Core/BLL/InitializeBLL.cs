using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SBO.Hub;
using SBO.Hub.Helpers;

namespace SmartOne.Engepecas.Warranty.Core.BLL
{
    public class InitializeBLL
    {
        public static void Initialize()
        {
            UserFieldsBLL.CreateUserFields();
            EventFilterBLL.CreateEvents();
            try
            {
                MenuHelper.LoadFromXML($"{Application.StartupPath}\\Menu\\Menu.xml");
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage($"Erro ao criar menu: {ex.Message}");
            }

        }
    }
}
