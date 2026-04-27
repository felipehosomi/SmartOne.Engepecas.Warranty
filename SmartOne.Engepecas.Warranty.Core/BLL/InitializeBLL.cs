using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SBO.Hub;
using SBO.Hub.Helpers;
using SBO.Hub.SBOHelpers;
using SmartOne.Engepecas.Warranty.Core.DAO;

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

            FormattedSearch formattedSearch = new FormattedSearch();
            formattedSearch.AssignFormattedSearch("NC Lançados", Hana.WarrantyNC_FormattedSearch, "FrmWarranty", "et_WarCode");

            SBOApp.AutoTranslateHana = false;

        }
    }
}
