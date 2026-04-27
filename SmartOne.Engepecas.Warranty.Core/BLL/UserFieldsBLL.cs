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

            // Create the ENG_PAINEL table
            userObject.CreateUserTable("ENG_PAINEL", "Painel de acompanhamento", SAPbobsCOM.BoUTBTableType.bott_NoObjectAutoIncrement);

            // Add fields to the ENG_PAINEL table
            userObject.InsertUserField("@ENG_PAINEL", "ENG_ID", "ID Painel", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 10, false);
            userObject.InsertUserField("@ENG_PAINEL", "ENG_TipoLcto", "Tipo Lancamento", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, false);
            userObject.AddValidValueToUserField("@ENG_PAINEL", "U_ENG_TipoLcto", new Dictionary<string, string>
    {
        { "CPN", "Cadastro PN" },
        { "PV", "Pedido de Venda" }
    });

            userObject.InsertUserField("@ENG_PAINEL", "ENG_DtLcto", "Data Lancamento", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 11, false);
            userObject.InsertUserField("@ENG_PAINEL", "ENG_HrLancamento", "Horario Lancamento", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, false);

            userObject.InsertUserField("@ENG_PAINEL", "ENG_Status", "Status", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, false);
            userObject.AddValidValueToUserField("@ENG_PAINEL", "U_ENG_Status", new Dictionary<string, string>
    {
        { "1", "Aguardando" },
        { "2", "Cadastrado" },
        { "3", "Aprovado" },
        { "4", "Reprovado" },
        { "5", "Cancelado" },
        { "6", "NF Emitida" }
    });

            userObject.InsertUserField("@ENG_PAINEL", "ENG_Motivo", "Motivo", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, false);
            userObject.AddValidValueToUserField("@ENG_PAINEL", "U_ENG_Motivo", new Dictionary<string, string>
    {
        { "1", "Recebimento" },
        { "2", "Cadastrar" },
        { "3", "Analise de credito" },
        { "4", "Aumento de limite" },
        { "5", "Reanalise" },
        { "6", "Inativo" },
        { "7", "Inadimplente" }
    });

            userObject.InsertUserField("@ENG_PAINEL", "ENG_CardCode", "Codigo do PN", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 15, false);
            userObject.InsertUserField("@ENG_PAINEL", "ENG_CardName", "Nome do PN", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, false);
            userObject.InsertUserField("@ENG_PAINEL", "ENG_CondPag", "Condicao de Pagamento", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, false);
            userObject.InsertUserField("@ENG_PAINEL", "ENG_FormPag", "Forma de pagamento", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, false);
            userObject.InsertUserField("@ENG_PAINEL", "ENG_ValorPrazo", "Valor a prazo", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 21, false);
            userObject.InsertUserField("@ENG_PAINEL", "ENG_ValorAVista", "Valor a vista", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 21, false);
            userObject.InsertUserField("@ENG_PAINEL", "ENG_ValorTotal", "Valor Total do Pedido (Com impostos)", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 21, false);
            userObject.InsertUserField("@ENG_PAINEL", "ENG_LimiteDisp", "Limite disponivel", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 21, false);
            userObject.InsertUserField("@ENG_PAINEL", "ENG_NumYell", "N Pedido Yell", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, false);
            userObject.InsertUserField("@ENG_PAINEL", "ENG_VendComp", "VendedorComprador", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, false);
            userObject.InsertUserField("@ENG_PAINEL", "ENG_Filial", "Loja", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, false);
            userObject.InsertUserField("@ENG_PAINEL", "ENG_ObsYell", "Observacao Vendedor", SAPbobsCOM.BoFieldTypes.db_Memo, SAPbobsCOM.BoFldSubTypes.st_None, 254, false);
            userObject.InsertUserField("@ENG_PAINEL", "ENG_ObsSAP", "Observacao FinanceiroCadastroAprovador", SAPbobsCOM.BoFieldTypes.db_Memo, SAPbobsCOM.BoFldSubTypes.st_None, 254, false);
            userObject.InsertUserField("@ENG_PAINEL", "AprovUser", "Usuario decisao", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, false);
            userObject.InsertUserField("@ENG_PAINEL", "AprovDtHr", "Dthora decisao", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 11, false);
            userObject.InsertUserField("@ENG_PAINEL", "ENG_Adiantamento", "Fatura de Adiantamento", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, false);
            userObject.InsertUserField("@ENG_PAINEL", "ENG_ObsInterna", "Observacao Interna", SAPbobsCOM.BoFieldTypes.db_Memo, SAPbobsCOM.BoFldSubTypes.st_None, 254, false);
            userObject.InsertUserField("@ENG_PAINEL", "ENG_Tipo", "Tipo de Pedido", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, false);
            userObject.InsertUserField("@ENG_PAINEL", "ENG_Integrado", "Pedido Integrado", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, false);
            userObject.InsertUserField("@ENG_PAINEL", "ENG_LinkYelll", "Link Yelll", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 254, false);

            // Create the ENG_PAINEL_HIST table
            userObject.CreateUserTable("ENG_PAINEL_HIST", "Painel Acomp. Historico", SAPbobsCOM.BoUTBTableType.bott_NoObjectAutoIncrement);

            // Add fields to the ENG_PAINEL_HIST table
            userObject.InsertUserField("@ENG_PAINEL_HIST", "TipoObs", "Tipo", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, false);
            userObject.InsertUserField("@ENG_PAINEL_HIST", "ObsTexto", "Observações", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 253, false);
            userObject.InsertUserField("@ENG_PAINEL_HIST", "UserSAP", "Usuario SAP", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, false);
            userObject.InsertUserField("@ENG_PAINEL_HIST", "DataHora", "Data e hora", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 11, false);
            userObject.InsertUserField("@ENG_PAINEL_HIST", "NumYell", "Numero Pedido Yelll", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, false);

            userObject.InsertUserField("SCL6", "ENG_RecebidoNC", "Recebido NC", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 100, false);

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
