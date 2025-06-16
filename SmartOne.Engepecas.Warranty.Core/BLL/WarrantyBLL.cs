using SAPbobsCOM;
using SBO.Hub;
using SBO.Hub.DAO;
using SmartOne.Engepecas.Warranty.Core.DAO;
using SmartOne.Engepecas.Warranty.Core.Model;
using System;
using System.Collections.Generic;

namespace SmartOne.Engepecas.Warranty.Core.BLL
{
    public class WarrantyBLL
    {
        public int GenerateNC(List<WarrantyModel> list)
        {
            try
            {
                object maxCode = CrudDAO.ExecuteScalar(Hana.Warranty_GetMaxCode);
                if (maxCode == null)
                {
                    maxCode = 1;
                }

                maxCode = Convert.ToInt32(maxCode) + 1;
                string date = $"'{DateTime.Now.ToString("yyyyMMdd")}'";

                foreach (WarrantyModel model in list)
                {
                    string update = String.Format(Hana.Warranty_UpdateLine, model.NC, "Y", maxCode, date, 0, model.Id, model.Line);
                    CrudDAO.ExecuteNonQuery(update);
                }
                return Convert.ToInt32(maxCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GenerateJE(DateTime dueDate, double docTotal, string memo, List<WarrantyModel> list)
        {
            int transId = 0;
            try
            {
                WarrantyConfigModel configModel = new CrudDAO().FillModelFromSql<WarrantyConfigModel>(Hana.WarrantyConfig_Get);

                JournalEntries je = (JournalEntries)SBOApp.Company.GetBusinessObject(BoObjectTypes.oJournalEntries);
                je.Memo = memo;
                je.DueDate = dueDate;

                je.Lines.BPLID = configModel.BPLId;
                je.Lines.AccountCode = configModel.DebitAccount;
                je.Lines.Debit = docTotal;
                je.Lines.LineMemo = memo;

                je.Lines.Add();

                je.Lines.BPLID = configModel.BPLId;
                je.Lines.AccountCode = configModel.CreditAccount;
                je.Lines.Credit = docTotal;
                je.Lines.LineMemo = memo;

                if (je.Add() != 0)
                {
                    throw new Exception("Erro ao gerar LCM: " + SBOApp.Company.GetLastErrorDescription());
                }
                else
                {
                    transId = Convert.ToInt32(SBOApp.Company.GetNewObjectKey());

                    foreach (WarrantyModel model in list)
                    {
                        string update = String.Format(Hana.Warranty_UpdateJE, transId, model.Id, model.Line);
                        CrudDAO.ExecuteNonQuery(update);
                    }
                }
                return transId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CancelNC(List<WarrantyModel> list)
        {
            try
            {
                object cancelled = CrudDAO.ExecuteScalar(String.Format(Hana.JournalEntry_IsCancelled, list[0].TransId));
                if (cancelled == null)
                {
                    throw new Exception("Não é possível cancelar, verificar com financeiro para estornar o lançamento");
                }

                foreach (WarrantyModel model in list)
                {
                    string update = String.Format(Hana.Warranty_UpdateLine, "", "N", 0, "NULL", 0, model.Id, model.Line);
                    CrudDAO.ExecuteNonQuery(update);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
