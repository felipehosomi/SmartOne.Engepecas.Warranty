using SBO.Hub.Attributes;
using System;

namespace SmartOne.Engepecas.Warranty.Core.Model
{
    public class WarrantyModel
    {
        [HubModel(UIFieldName = "Id", Index = 10)]
        public int Id { get; set; }
        [HubModel(UIFieldName = "Atendimento", Index = 20)]
        public string Service { get; set; }
        [HubModel(UIFieldName = "Descrição", Index = 30)]
        public string Description { get; set; }
        [HubModel(UIFieldName = "Multiplicador", Index = 40)]
        public double Multiplier { get; set; }
        [HubModel(UIFieldName = "Valor Total", Index = 50)]
        public double Total { get; set; }
        [HubModel(UIFieldName = "A Receber", Index = 60)]
        public double Balance { get; set; }
        [HubModel(UIFieldName = "NC", Index = 70)]
        public string NC { get; set; }
        [HubModel(UIFieldName = "Baixar", Index = 80)]
        public string Generate { get; set; }
        [HubModel(UIFieldName = "WC", Index = 90)]
        public string WC { get; set; }
        [HubModel(UIFieldName = "Nome Cliente", Index = 100)]
        public string CardName { get; set; }

        [HubModel(UIFieldName = "Data Abertura", Index = 110)]
        public DateTime CreateDate { get; set; }
        [HubModel(UIIgnore = true)]
        public int CreateTime { get; set; }
        [HubModel(UIFieldName = "Hora", Index = 120)]
        public string CreateTimeStr
        {
            get
            {
                return $"{CreateTime.ToString().Substring(0, CreateTime.ToString().Length - 2)}:{CreateTime.ToString().Substring(CreateTime.ToString().Length - 2)}";
            }
        }
        [HubModel(UIFieldName = "Série", Index = 130)]
        public string Serial { get; set; }

        [HubModel(UIFieldName = "Line")]
        public int Line { get; set; }
        [HubModel(UIFieldName = "WarDate")]
        public DateTime WarDate { get; set; }
        [HubModel(UIFieldName = "TransId")]
        public int TransId { get; set; }
    }
}
