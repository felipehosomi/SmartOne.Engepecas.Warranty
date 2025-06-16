using SAPbouiCOM;
using SBO.Hub.DAO;
using SBO.Hub.Forms;
using SmartOne.Engepecas.Warranty.Core.DAO;
using SmartOne.Engepecas.Warranty.Core.Model;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace SmartOne.Engepecas.Warranty.Core.Forms
{
    internal class FrmWarrantyNCQuery : BaseForm
    {
        public FrmWarrantyNCQuery()
        {

        }

        public FrmWarrantyNCQuery(MenuEvent menuEvent)
        {
            MenuEventInfo = menuEvent;
        }

        public FrmWarrantyNCQuery(ItemEvent itemEvent)
        {
            ItemEventInfo = itemEvent;
        }

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    if (ItemEventInfo.ItemUID == "bt_Search")
                    {
                        Form.Freeze(true);

                        string nc = Form.DataSources.UserDataSources.Item("ud_NC").Value;
                        string type = Form.DataSources.UserDataSources.Item("ud_Type").Value;

                        string sql = String.Format(Hana.WarrantyNC_Query, type, nc);
                        Form.DataSources.DataTables.Item("dt_NC").ExecuteQuery(sql);

                        Grid gr_NC = (Grid)Form.Items.Item("gr_NC").Specific;
                        ((EditTextColumn)gr_NC.Columns.Item("Total")).ColumnSetting.SumType = BoColumnSumType.bst_Auto;
                        ((EditTextColumn)gr_NC.Columns.Item("A Receber")).ColumnSetting.SumType = BoColumnSumType.bst_Auto;

                        Form.Freeze(false);
                    }
                }
            }
           
            return true;
        }
    }
}
