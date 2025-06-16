using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SBO.Hub.Helpers;

namespace SmartOne.Engepecas.Warranty.Core.BLL
{
    public class UserFieldsBLL
    {
        public static void CreateUserFields()
        {
            UserObject userObject = new UserObject();
            userObject.CreateUserTable("ENG_SERV_MULT", "Multiplicador tipo serviço", SAPbobsCOM.BoUTBTableType.bott_NoObjectAutoIncrement);
            userObject.InsertUserField("@ENG_SERV_MULT", "Tipo", "Tipo", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, true);
            Dictionary<string, string> validValues = new Dictionary<string, string>();
            validValues.Add("Atendimento HR", "Atendimento Horas");
            validValues.Add("Atendimento KM", "Atendimento KM");
            validValues.Add("Peças", "Peças");
            validValues.Add("Nota Terceiro", "Nota Terceiro");
            userObject.AddValidValueToUserField("@ENG_SERV_MULT", "U_Tipo", validValues);
            userObject.InsertUserField("@ENG_SERV_MULT", "Mult", "Multiplicador", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Quantity, 50, true);

            userObject.InsertUserField("SCL6", "ENG_GAR_COD", "Cód Garantia", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10);
            userObject.InsertUserField("SCL6", "ENG_GAR_DATA", "Data Lanç Garantia", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 10);
            userObject.InsertUserField("SCL6", "ENG_GAR_LCM", "LCM Garantia", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10);

            userObject.CreateUserTable("ENG_SERV_NC", "Serv NC", SAPbobsCOM.BoUTBTableType.bott_NoObjectAutoIncrement);
            userObject.InsertUserField("@ENG_SERV_NC", "BPLId", "Filial", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, true);
            userObject.InsertUserField("@ENG_SERV_NC", "ContaDebito", "Conta/PN Débito", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, true);
            userObject.InsertUserField("@ENG_SERV_NC", "ContaCredito", "Conta/PN Crédito", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, true);
            
            //userObject.CreateUserTable("SOWARRANTY", "SO|Garantia", SAPbobsCOM.BoUTBTableType.bott_MasterData);
            //userObject.CreateUserTable("SOWARRANTY1", "SO|Garantia Linhas", SAPbobsCOM.BoUTBTableType.bott_MasterDataLines);
            //userObject.AddValidValueToUserField("OINV", "U_Checked", "N", "Não", true);
            //userObject.AddValidValueToUserField("OINV", "U_Checked", "Y", "Sim");


            //userObject.InsertUserField("INV1", "Checked", "Conferido", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1);
            //userObject.AddValidValueToUserField("INV1", "U_Checked", "N", "Não", true);
            //userObject.AddValidValueToUserField("INV1", "U_Checked", "Y", "Sim");
        }
    }
}
