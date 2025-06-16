using System;
using SBO.Hub.Attributes;

namespace SmartOne.Engepecas.Warranty.Core.Model
{
    public class WarrantyFilterModel
    {
        [HubModel(UserDataSource = "ud_DtFrom")]
        public DateTime DateFrom { get; set; }
        [HubModel(UserDataSource = "ud_DtTo")]
        public DateTime DateTo { get; set; }
        [HubModel(UserDataSource = "ud_WC")]
        public string WarrantyCode { get; set; }
        [HubModel(UserDataSource = "ud_BP")]
        public string CardCode { get; set; }
        [HubModel(UserDataSource = "ud_Serial")]
        public string SerialNum { get; set; }
    }
}
