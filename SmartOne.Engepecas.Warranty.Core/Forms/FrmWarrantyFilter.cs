using SAPbouiCOM;
using SBO.Hub.Forms;
using SBO.Hub.UI;
using SmartOne.Engepecas.Warranty.Core.Model;

namespace SmartOne.Engepecas.Warranty.Core.Forms
{
    internal class FrmWarrantyFilter : BaseForm
    {
        public FrmWarrantyFilter()
        {

        }

        public FrmWarrantyFilter(MenuEvent menuEvent)
        {
            MenuEventInfo = menuEvent;
        }

        public FrmWarrantyFilter(ItemEvent itemEvent)
        {
            ItemEventInfo = itemEvent;
        }

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    if (ItemEventInfo.ItemUID == "bt_Gen")
                    {
                        WarrantyFilterModel warrantyFilter = new WarrantyFilterModel();
                        Form.ValidateAndFillModelByUserDataSource<WarrantyFilterModel>(warrantyFilter);
                        FrmWarranty.WarrantyFilterModel = warrantyFilter;
                        FrmWarranty frmWarranty = new FrmWarranty();
                        frmWarranty.Show(warrantyFilter);
                    }
                }
                else if (ItemEventInfo.EventType == BoEventTypes.et_CHOOSE_FROM_LIST)
                {
                    this.OnChooseFromList();
                }
            }

            return true;
        }

        private void OnChooseFromList()
        {
            IChooseFromListEvent oCFLEvent = ((IChooseFromListEvent)ItemEventInfo);
            ChooseFromList oCFL = Form.ChooseFromLists.Item(oCFLEvent.ChooseFromListUID);
            DataTable oDataTable = oCFLEvent.SelectedObjects;

            if (oDataTable != null && oDataTable.Rows.Count > 0)
            {
                try
                {
                    if (ItemEventInfo.ItemUID == "et_BP")
                    {
                        Form.DataSources.UserDataSources.Item("ud_BP").Value = oDataTable.GetValue("CardCode", 0).ToString();
                    }
                }
                catch { }
            }
        }
    }
}