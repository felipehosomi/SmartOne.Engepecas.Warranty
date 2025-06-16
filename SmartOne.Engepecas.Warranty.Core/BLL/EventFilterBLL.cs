using SBO.Hub.Helpers;

namespace SmartOne.Engepecas.Warranty.Core.BLL
{
    internal class EventFilterBLL
    {
        public static void CreateEvents()
        {
            EventFilterHelper.SetFormEvent("FrmWarrantyFilter", SAPbouiCOM.BoEventTypes.et_CLICK);
            EventFilterHelper.SetFormEvent("FrmWarrantyFilter", SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST);

            EventFilterHelper.SetFormEvent("FrmWarranty", SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED);
            EventFilterHelper.SetFormEvent("FrmWarranty", SAPbouiCOM.BoEventTypes.et_VALIDATE);

            EventFilterHelper.SetFormEvent("FrmWarrantyNCQuery", SAPbouiCOM.BoEventTypes.et_CLICK);

            EventFilterHelper.EnableEvents();
        }
    }
}
