using SAPbouiCOM;
using SBO.Hub;
using SBO.Hub.DAO;
using SBO.Hub.Forms;
using SBO.Hub.UI;
using SBO.Hub.Util;
using SmartOne.Engepecas.Warranty.Core.BLL;
using SmartOne.Engepecas.Warranty.Core.DAO;
using SmartOne.Engepecas.Warranty.Core.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmartOne.Engepecas.Warranty.Core.Forms
{
    public class FrmWarranty : BaseForm
    {
        Form Form;
        public static WarrantyFilterModel WarrantyFilterModel { get; set; }

        public FrmWarranty()
        {

        }

        public FrmWarranty(MenuEvent menuEvent)
        {
            MenuEventInfo = menuEvent;
        }

        public FrmWarranty(ItemEvent itemEvent)
        {
            ItemEventInfo = itemEvent;
        }

        public override object Show()
        {
            Form = (Form)base.Show();
            FormCount++;
            return Form;
        }

        public object Show(WarrantyFilterModel warrantyFilterModel)
        {
            Form = (Form)base.Show();
            Form.Freeze(true);
            string sql = String.Format(Hana.Warranty_Get,
                                       warrantyFilterModel.DateFrom.ToString("yyyyMMdd"),
                                       warrantyFilterModel.DateTo.ToString("yyyyMMdd"),
                                       warrantyFilterModel.WarrantyCode,
                                       warrantyFilterModel.CardCode,
                                       warrantyFilterModel.SerialNum);

            List<WarrantyModel> list = new CrudDAO().FillModelListFromSql<WarrantyModel>(sql);
            LoadScreen(list);

            Form.Items.Item("bt_Gen").Enabled = true;
            Form.Items.Item("bt_JE").Enabled = false;
            Form.Items.Item("bt_Cancel").Enabled = false;

            Form.Freeze(false);
            return Form;
        }

        public override bool ItemEvent()
        {
            Form = SBOApp.Application.Forms.GetForm(ItemEventInfo.FormTypeEx, ItemEventInfo.FormTypeCount);
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_ITEM_PRESSED)
                {
                    if (ItemEventInfo.ItemUID == "bt_Gen")
                    {
                        GenerateNC();
                    }
                    else if (ItemEventInfo.ItemUID == "bt_JE")
                    {
                        GenerateJE();
                    }
                    else if (ItemEventInfo.ItemUID == "bt_Search")
                    {
                        Search();
                    }
                    else if (ItemEventInfo.ItemUID == "bt_Cancel")
                    {
                        Cancel();
                    }
                    else if (ItemEventInfo.ItemUID.StartsWith("fl_"))
                    {
                        Form.PaneLevel = Convert.ToInt32(ItemEventInfo.ItemUID.Split('_')[1]);
                    }
                    else if (ItemEventInfo.ItemUID.StartsWith("gr_") && ItemEventInfo.ColUID == "Baixar")
                    {
                        CalculateTotal();
                    }
                }
                else if (ItemEventInfo.EventType == BoEventTypes.et_VALIDATE && ItemEventInfo.ItemChanged)
                {
                    if (ItemEventInfo.ItemUID.StartsWith("gr_") && ItemEventInfo.ColUID == "A Receber")
                    {
                        CalculateTotal();
                    }
                }
            }
            return true;
        }

        private void Search()
        {
            Form.Freeze(true);

            int index = 0;
            while (true)
            {
                try
                {
                    Form.DataSources.DataTables.Add($"dt_{index}").Clear();
                    Form.Items.Item($"gr_{index}").Visible = false;
                    Form.Items.Item($"st_{index}").Visible = false;
                    index++;
                }
                catch
                {
                    break;
                }
            }

            int warrantyCode;
            int transId;
            DateTime docDate;

            Int32.TryParse(Form.DataSources.UserDataSources.Item("ud_WarCode").Value, out warrantyCode);
            Int32.TryParse(Form.DataSources.UserDataSources.Item("ud_JE").Value, out transId);

            string dateStr;
            if (DateTime.TryParseExact(Form.DataSources.UserDataSources.Item("ud_Date").Value, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out docDate))
            {
                dateStr = $"'{docDate.ToString("yyyyMMdd")}'";
            }
            else
            {
                dateStr = "NULL";
            }

            string sql = String.Format(Hana.WarrantyNC_Get,
                                      warrantyCode,
                                      dateStr,
                                      transId);

            List<WarrantyModel> list = new CrudDAO().FillModelListFromSql<WarrantyModel>(sql);
            if (list.Count > 0)
            {
                LoadScreen(list, true);

                Form.DataSources.UserDataSources.Item("ud_Total").Value = list.Sum(x => x.Balance).ToString();
                Form.DataSources.UserDataSources.Item("ud_Date").Value = list[0].WarDate.ToString("dd/MM/yyyy");
                Form.DataSources.UserDataSources.Item("ud_JE").Value = list[0].TransId.ToString();

                if (list[0].TransId != 0)
                {
                    Form.Items.Item("bt_JE").Enabled = false;
                }
                else
                {
                    Form.Items.Item("bt_JE").Enabled = true;
                }
                Form.Items.Item("bt_Gen").Enabled = false;
                Form.Items.Item("bt_Cancel").Enabled = true;
            }
            else
            {
                SBOApp.Application.SetStatusBarMessage("Nenhum registro encontrado");
            }

            Form.Freeze(false);
        }

        private void LoadScreen(List<WarrantyModel> list, bool search = false)
        {
            var listByCallId = list.GroupBy(x => x.Id);

            Item itemBase = Form.Items.Item("st_WarCode");

            int topBase = itemBase.Top;
            int pane = 1;
            int gridCount = 0;
            int index = 0;

            string formXml = "";
            if (search)
            {
                formXml = Form.GetAsXML();
            }
            foreach (var itemByCallId in listByCallId)
            {
                WarrantyModel firstModel = itemByCallId.First();
                if (gridCount == 3)
                {
                    topBase = itemBase.Top;
                    pane++;
                    //Form.DataSources.UserDataSources.Add($"ud_{firstModel.Id}", BoDataType.dt_SHORT_TEXT, 1);

                    Folder fl_Base = Form.Items.Item($"fl_1").Specific as Folder;

                    Item it_Folder;
                    if (!formXml.Contains($"fl_{pane}"))
                    {
                        it_Folder = Form.Items.Add($"fl_{pane}", BoFormItemTypes.it_FOLDER);
                    }
                    else
                    {
                        it_Folder = Form.Items.Item($"fl_{pane}");
                    }
                    it_Folder.Visible = true;
                    it_Folder.Height = fl_Base.Item.Height;
                    it_Folder.Width = fl_Base.Item.Width;
                    it_Folder.Top = fl_Base.Item.Top;
                    it_Folder.Left = fl_Base.Item.Left + 10;

                    ((Folder)it_Folder.Specific).DataBind.SetBound(true, "", $"ud_1");
                    ((Folder)it_Folder.Specific).Caption = $"{pane}";
                    ((Folder)it_Folder.Specific).Pane = pane;
                    //((Folder)it_Folder.Specific).ValOn = pane.ToString();
                    //((Folder)it_Folder.Specific).ValOff = "0";
                    ((Folder)it_Folder.Specific).GroupWith("fl_1");
                    it_Folder.Visible = true;
                    gridCount = 0;
                }

                Item it_Title;
                if (!formXml.Contains($"st_{index}"))
                {
                    it_Title = Form.Items.Add($"st_{index}", BoFormItemTypes.it_STATIC);
                }
                else
                {
                    it_Title = Form.Items.Item($"st_{index}");
                }
                it_Title.Left = itemBase.Left + 10;
                it_Title.Top = topBase + 60;
                it_Title.Width = 600;
                it_Title.FromPane = pane;
                it_Title.ToPane = pane;
                it_Title.Visible = true;

                ((StaticText)it_Title.Specific).Caption = $"{firstModel.Serial} - {firstModel.CardName} - {firstModel.WC} - {firstModel.CreateDate.ToString("dd/MM/yyyy")} {firstModel.CreateTimeStr}";

                Item it_Grid;
                if (!formXml.Contains($"gr_{index}"))
                {
                    it_Grid = Form.Items.Add($"gr_{index}", BoFormItemTypes.it_GRID);
                }
                else
                {
                    it_Grid = Form.Items.Item($"gr_{index}");
                }

                it_Grid.Left = itemBase.Left + 10;
                it_Grid.Top = topBase + 75;
                it_Grid.Width = 900;
                it_Grid.Height = 150;
                it_Grid.FromPane = pane;
                it_Grid.ToPane = pane;
                it_Grid.Visible = true;

                DataTable dt_Warranty;
                if (!formXml.Contains($"dt_{index}"))
                {
                    dt_Warranty = Form.DataSources.DataTables.Add($"dt_{index}");
                }
                else
                {
                    dt_Warranty = Form.DataSources.DataTables.Item($"dt_{index}");
                    dt_Warranty.Clear();
                }

                dt_Warranty.CreateColumns(typeof(WarrantyModel));
                dt_Warranty.FillTable(itemByCallId.ToList());

                Grid gr_Warranty = (Grid)it_Grid.Specific;
                gr_Warranty.DataTable = dt_Warranty;

                gr_Warranty.Columns.Item("Id").Editable = false;
                gr_Warranty.Columns.Item("Atendimento").Editable = false;
                gr_Warranty.Columns.Item("Descrição").Editable = false;
                gr_Warranty.Columns.Item("Multiplicador").Editable = false;
                gr_Warranty.Columns.Item("Valor Total").Editable = false;
                gr_Warranty.Columns.Item("A Receber").Editable = false;
                gr_Warranty.Columns.Item("Descrição").Editable = false;
                gr_Warranty.Columns.Item("WC").Editable = false;
                gr_Warranty.Columns.Item("Nome Cliente").Editable = false;
                gr_Warranty.Columns.Item("Data Abertura").Editable = false;
                gr_Warranty.Columns.Item("Hora").Editable = false;
                gr_Warranty.Columns.Item("Série").Editable = false;

                gr_Warranty.Columns.Item("NC").Width = 120;
                gr_Warranty.Columns.Item("Baixar").Type = BoGridColumnType.gct_CheckBox;

                gr_Warranty.Columns.Item("Line").Visible = false;
                gr_Warranty.Columns.Item("WarDate").Visible = false;
                gr_Warranty.Columns.Item("TransId").Visible = false;

                if (search)
                {
                    it_Grid.Enabled = false;
                }

                ((EditTextColumn)gr_Warranty.Columns.Item("Valor Total")).ColumnSetting.SumType = BoColumnSumType.bst_Auto;
                ((EditTextColumn)gr_Warranty.Columns.Item("A Receber")).ColumnSetting.SumType = BoColumnSumType.bst_Auto;


                topBase += it_Title.Height + it_Grid.Height;
                gridCount++;
                index++;
            }

            Form.Items.Item($"fl_1").Click();
        }

        private void CalculateTotal()
        {
            List<WarrantyModel> list = new List<WarrantyModel>();
            int index = 0;

            while (true)
            {
                try
                {
                    DataTable dt_Warranty = Form.DataSources.DataTables.Item($"dt_{index}");
                    list.AddRange(dt_Warranty.FillModelByColumnValue<WarrantyModel>(false, "Baixar"));
                    index++;
                }
                catch
                {
                    break;
                }
            }

            Form.DataSources.UserDataSources.Item("ud_Total").Value = list.Sum(x => x.Balance).ToString();
        }

        private void GenerateNC()
        {
            try
            {
                WarrantyBLL warrantyBLL = new WarrantyBLL();
                int index = 0;

                List<WarrantyModel> list = new List<WarrantyModel>();
                while (true)
                {
                    try
                    {
                        DataTable dt_Warranty = Form.DataSources.DataTables.Item($"dt_{index}");
                        list.AddRange(dt_Warranty.FillModelByColumnValue<WarrantyModel>(true, "Baixar"));
                        index++;
                    }
                    catch
                    {
                        break;
                    }
                }
                if (list.Count == 0)
                {
                    throw new Exception("Nenhum item selecionado para baixa");
                }
                int warCode = warrantyBLL.GenerateNC(list);
                SBOApp.Application.MessageBox("Baixa efetuada com sucesso!");

                Form.DataSources.UserDataSources.Item("ud_WarCode").Value = warCode.ToString();

                Form.Items.Item("bt_Search").Click();
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage(ex.Message);
            }
        }

        private void GenerateJE()
        {
            try
            {
                WarrantyBLL warrantyBLL = new WarrantyBLL();
                int index = 0;
                DateTime dueDate;
                double docTotal;

                if (!DateTime.TryParseExact(Form.DataSources.UserDataSources.Item("ud_DueDate").Value, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out dueDate))
                {
                    throw new Exception("Data vencimento deve ser informada");
                }

                double.TryParse(Form.DataSources.UserDataSources.Item("ud_Total").Value, out docTotal);
                if (docTotal == 0)
                {
                    throw new Exception("Valor total deve ser maior do que zero");
                }

                List<WarrantyModel> list = new List<WarrantyModel>();
                while (true)
                {
                    try
                    {
                        DataTable dt_Warranty = Form.DataSources.DataTables.Item($"dt_{index}");
                        list.AddRange(dt_Warranty.FillModelByColumnValue<WarrantyModel>(true, "Baixar"));
                        index++;
                    }
                    catch
                    {
                        break;
                    }
                }

                int transId = warrantyBLL.GenerateJE(dueDate, docTotal, Form.DataSources.UserDataSources.Item("ud_Memo").Value, list);
                SBOApp.Application.MessageBox("Lançamento efetuado com sucesso!");
                Form.Items.Item("bt_JE").Enabled = false;
                Form.DataSources.UserDataSources.Item("ud_JE").Value = transId.ToString();
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage(ex.Message);
            }
        }

        private void Cancel()
        {
            try
            {
                WarrantyBLL warrantyBLL = new WarrantyBLL();
                int index = 0;
               
                List<WarrantyModel> list = new List<WarrantyModel>();
                while (true)
                {
                    try
                    {
                        DataTable dt_Warranty = Form.DataSources.DataTables.Item($"dt_{index}");
                        list.AddRange(dt_Warranty.FillModelByColumnValue<WarrantyModel>(true, "Baixar"));
                        index++;
                    }
                    catch
                    {
                        break;
                    }
                }

                warrantyBLL.CancelNC(list);
                SBOApp.Application.MessageBox("Cancelamento efetuado com sucesso!");
                Form.Items.Item("bt_Gen").Enabled = false;
                Form.Items.Item("bt_JE").Enabled = false;
                Form.Items.Item("bt_Cancel").Enabled = false;
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage(ex.Message);
            }
        }
    }
}
