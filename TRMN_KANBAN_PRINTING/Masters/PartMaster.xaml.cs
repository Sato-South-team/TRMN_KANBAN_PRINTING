using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TRMN_KANBAN_PRINTING.StartUp;

namespace TRMN_KANBAN_PRINTING.Masters
{
    /// <summary>
    /// Interaction logic for PartMaster.xaml
    /// </summary>
    public partial class PartMaster : Window
    {
        public PartMaster()
        {
            InitializeComponent();
        }
        #region Variables and Objects
        BUSINESS_LAYER.LogCreation.LogCreation obj_Log = new BUSINESS_LAYER.LogCreation.LogCreation();
        BUSINESS_LAYER.Masters.Masters obj_Mast = new BUSINESS_LAYER.Masters.Masters();
        DataTable dt = new DataTable();
        DataTable obj_Dt = new DataTable();
        #endregion

        private void ShowDateTime()
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            txtDatetime.Text = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss");
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.WindowState = WindowState.Maximized;
                this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
                this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
                MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
                MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
                Application.Current.MainWindow = this;
                SizeToContent = SizeToContent.Manual;
                dt.Columns.Add("SL_NO");
                dt.Columns.Add("TR_PART_NUMBER");
                dt.Columns.Add("PART_NAME");
                dt.Columns.Add("KANBAN_NO");
                dt.Columns.Add("ID_CODE");
                dt.Columns.Add("CUSTOMER_PART_NO");
                dt.Columns.Add("SUPPLIER_CODE_NAME");
                dt.Columns.Add("LINE_NO");
                dt.Columns.Add("LOCATION_NUMBER");
                dt.Columns.Add("KANBAN_LOC_PROCESS_LINE_NO");
                dt.Columns.Add("BOX_TYPE");
                dt.Columns.Add("BOX_QTY");
                dt.Columns.Add("BIN_NO");
                dt.Columns.Add("REMARKS");
                dt.Columns.Add("REFNO");
                Transaction("GetDeatils");
                ShowDateTime();
                txtPartNo.Focus();
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "PART_MASTER", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }

        }

        private void dvgMasterDeatils_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dvgMasterDeatils.SelectedItems.Count == 1)
                {
                    DataRowView dr = (DataRowView)dvgMasterDeatils.SelectedItems[0];
                   CommonClasses.CommonVariable.RefNo =Convert.ToInt32( dr["REFNO"]);
                    txtPartNo.Text = dr["TR_PART_NUMBER"].ToString();
                    txtPartName.Text = dr["PART_NAME"].ToString();
                    txtEkanban.Text = dr["KANBAN_NO"].ToString();
                    txtIDcode.Text = dr["ID_CODE"].ToString();
                    txtCustPartNo.Text = dr["CUSTOMER_PART_NO"].ToString();
                    txtLineNo.Text = dr["LINE_NO"].ToString();
                    txtLineSup.Text = dr["KANBAN_LOC_PROCESS_LINE_NO"].ToString();
                    txtLocName.Text = dr["LOCATION_NUMBER"].ToString();
                    txtRemarks.Text = dr["REMARKS"].ToString();
                    txtSupCode.Text = dr["SUPPLIER_CODE_NAME"].ToString();
                    txtBinSize.Text = dr["BOX_TYPE"].ToString();
                    txtBinQty.Text = dr["BOX_QTY"].ToString();
                    txtBinno.Text = dr["BIN_NO"].ToString();
                    txtPartNo.Focus();
                }
                else
                {
                    txtPartNo.Text = "";
                    txtPartName.Text = "";
                    txtEkanban.Text = "";
                    txtIDcode.Text = "";
                    txtLineNo.Text = "";
                    txtLineSup.Text = "";
                    txtLocName.Text = "";
                    txtRemarks.Text = "";
                    txtSupCode.Text = "";
                    txtBinSize.Text = "";
                    txtBinQty.Text = "";
                    txtBinno.Text = "";
                    txtCustPartNo.Text = "";
                    CommonClasses.CommonVariable.RefNo = 0;
                    txtPartNo.Focus();
                }
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "PART_MASTER", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dvgMasterDeatils.SelectedItems.Count == 0)
                {
                    if (ControlValidation())
                    {
                        dt.Rows.Clear();
                        Transaction("Save");
                    }
                }
                else
                    CommonClasses.CommonMethods.MessageBoxShow("YOU CAN NOT SAVE THE RECORDS PLEASE GO FOR DELETION OR UPDATION", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "PART_MASTER", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dvgMasterDeatils.SelectedItems.Count > 0)
                {
                    if (dvgMasterDeatils.SelectedItems.Count == 1)
                    {
                        if (ControlValidation())
                        {
                            dt.Rows.Clear();
                            Transaction("Update");
                        }
                    }
                    else
                        CommonClasses.CommonMethods.MessageBoxShow("MULTIPLE SELECTION WILL NOT SUPPORT FOR UPDATE", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                }
                else
                    CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.RowSelection, CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "PART_MASTER", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dvgMasterDeatils.SelectedItems.Count > 0)
                {
                    CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.DeleteConfirm, CustomMessageBox.CustomStriing.Question.ToString(), CustomMessageBox.CustomStriing.YESNO.ToString());
                    if (CommonClasses.CommonVariable.Result == "YES")
                    {
                        for (int i = 0; i < dvgMasterDeatils.SelectedItems.Count; i++)
                        {
                            DataRowView dr = (DataRowView)dvgMasterDeatils.SelectedItems[i];
                            CommonClasses.CommonVariable.RefNo  = Convert.ToInt32(dr["Refno"]);
                            Transaction("Delete");
                        }

                        if (CommonClasses.CommonVariable.Result == "Deleted")
                        {
                            CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.DataDeleted, CustomMessageBox.CustomStriing.Successfull.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                            Clear();
                        }
                        else
                        {
                            CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.Result, CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                        }
                    }
                }
                else
                    CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.RowSelection, CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "PART_MASTER", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "PART_MASTER", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.S) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.S))
            {
                btnSave_Click(sender, e);
            }
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.U) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.U))
            {
                btnUpdate_Click(sender, e);
            }
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.C) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.C))
            {
                btnClear_Click(sender, e);
            }
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.E) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.E) || Keyboard.IsKeyDown(Key.Escape) && Keyboard.IsKeyDown(Key.Escape))
            {
                btnExit_Click(sender, e);
            }
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.D) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.D))
            {
                btnDelete_Click(sender, e);
            }
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.I) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.I))
            {
                BtnImport_Click(sender, e);
            }
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.T) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.T))
            {
                BtnTemplat_Click(sender, e);
            }
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.X) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.X))
            {
                BtnExport_Click(sender, e);
            }
        }

        private void BtnTemplat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog obj_SD = new SaveFileDialog();
                obj_SD.Filter = "Excel files (*.xlsx)|*.xlsx|Excel files(*.xls)|*.xls";
                obj_SD.ShowDialog();
                if (obj_SD.FileName != "")
                {
                    dt.Rows.Clear();
                    dt.Columns.Remove("REFNO");
                    CommonClasses.CommonMethods.CreateExcellFile(dt, obj_SD.FileName, "MasterData");
                    CommonClasses.CommonMethods.MessageBoxShow("TEMPLATE CREATED SUCCESSFULLY", CustomMessageBox.CustomStriing.Successfull.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    Clear();
                }
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "PART_MASTER", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
           
        }

        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog obj_OP = new OpenFileDialog();
                obj_OP.Filter = "Excel files (*.xlsx)|*.xlsx|Excel files(*.xls)|*.xls";
                obj_OP.ShowDialog();
                if (obj_OP.FileName != "")
                {
                    dt = CommonClasses.CommonMethods.ReadExcelData(obj_OP.FileName, "[MasterData$]");
                    
                    int Count = dt.Columns.Count;
                    for (int i = 14; i < Count; i++)
                    {
                        dt.Columns.RemoveAt(13);
                        dt.AcceptChanges();
                    }
                    dt.Columns.Add("REFNO");
                    if (dt.Rows.Count>0)
                    Transaction("Save");
                    else
                        CommonClasses.CommonMethods.MessageBoxShow("DATA NOT FOUND TO IMPORT", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

                }
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "PART_MASTER", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
          
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog  obj_SD = new SaveFileDialog();
                obj_SD.Filter = "Excel files (*.xlsx)|*.xlsx|Excel files(*.xls)|*.xls";
                obj_SD.ShowDialog();
                if (obj_SD.FileName != "")
                {
                    obj_Dt.Columns.Remove("REFNO");
                    if (obj_Dt.Rows.Count > 0)
                    {
                        CommonClasses.CommonMethods.CreateExcellFile(obj_Dt, obj_SD.FileName, "MasterData");
                        CommonClasses.CommonMethods.MessageBoxShow("DATA EXPORTED SUCCESSFULLY", CustomMessageBox.CustomStriing.Successfull.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                        Clear();
                    }
                    else
                        CommonClasses.CommonMethods.MessageBoxShow("DATA NOT FOUND TO EXPORT", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

                    
                }

            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "PART_MASTER", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
           
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StartUp.MainWindow ObjHm = new StartUp.MainWindow();
                this.Close();
                ObjHm.ShowDialog();
              
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "PART_MASTER", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }
        private void Transaction(string Type)
        {
            if (Type == "Save" || Type == "Update" || Type == "Delete")
            {
                ENTITY_LAYER.Masters.Masters.PartNo = txtPartNo.Text;
                ENTITY_LAYER.Masters.Masters.Ekanban = txtEkanban.Text;

                if (dt.Rows.Count == 0)
                    dt.Rows.Add("1", txtPartNo.Text, txtPartName.Text, txtEkanban.Text, txtIDcode.Text,txtCustPartNo.Text, txtSupCode.Text, txtLineNo.Text, txtLocName.Text, txtLineSup.Text, txtBinSize.Text, txtBinQty.Text, txtBinno.Text, txtRemarks.Text, CommonClasses.CommonVariable.RefNo.ToString());

                ENTITY_LAYER.Masters.Masters.Dt = dt;
                ENTITY_LAYER.Masters.Masters.Type = Type;
                ENTITY_LAYER.Masters.Masters.RefNo = CommonClasses.CommonVariable.RefNo;
                CommonClasses.CommonVariable.Result = obj_Mast.BL_PartMasterTransaction();
                if (CommonClasses.CommonVariable.Result == "Saved")
                {
                    CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.DataSaved, CustomMessageBox.CustomStriing.Successfull.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    Clear();
                }
                else if (CommonClasses.CommonVariable.Result == "Updated")
                {
                    CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.DataUpdated, CustomMessageBox.CustomStriing.Successfull.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    Clear();
                }
                else if (CommonClasses.CommonVariable.Result == "Duplicate")
                    CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.Duplicate, CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                else if (CommonClasses.CommonVariable.Result != "Deleted")
                    CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.Result, CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
            if (Type == "GetDeatils")
            {
                ENTITY_LAYER.Masters.Masters.Type = Type;
                ENTITY_LAYER.Masters.Masters.Dt = null;
                obj_Dt = obj_Mast.BL_PartMasterDetails().Tables[0];
                dvgMasterDeatils.ItemsSource = obj_Dt.DefaultView;
            }

        }
        private void Clear()
        {
            txtPartNo.Text = "";
            txtPartName.Text = "";
            txtEkanban.Text = "";
            txtIDcode.Text = "";
            txtLineNo.Text = "";
            txtLineSup.Text = "";
            txtLocName.Text = "";
            txtRemarks.Text = "";
            txtSupCode.Text = "";
            txtBinSize.Text = "";
            txtBinQty.Text = "";
            txtCustPartNo.Text = "";
            txtBinno.Text = "";
            CommonClasses.CommonVariable.RefNo = 0;
            txtPartNo.Focus();
            Transaction("GetDeatils");
        }
        private bool ControlValidation()
        {
            if (txtPartNo.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER PART NO", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtPartNo.Focus();
                return false;
            }
            if (txtPartName.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER PART NAME", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtPartName.Focus();
                return false;
            }
            if (txtEkanban.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER E-KANBAN NO", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtEkanban.Focus();
                return false;
            }
            if (txtIDcode.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER ID CODE", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtIDcode.Focus();
                return false;
            }
            if (txtSupCode.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER SUPPLIER CODE/NAME", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtSupCode.Focus();
                return false;
            }
            if (txtLineNo.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER LINE NO", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtLineNo.Focus();
                return false;
            }
            if (txtLocName.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER LOCATION NO", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtLocName.Focus();
                return false;
            }
            if (txtLineSup.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER LINE/SUPPLIER CODE", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtLineSup.Focus();
                return false;
            }
            if (txtBinSize.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER BIN SIZE", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtBinSize.Focus();
                return false;
            }
            if (txtBinQty.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER BIN QTY", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtBinQty.Focus();
                return false;
            }
            if (txtBinno.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER BIN NO", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtBinno.Focus();
                return false;
            }
            return true;
        }

        private void TxtBinQty_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                CommonClasses.CommonMethods.NumericValue(e);
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "PART_MASTER", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
        }
    }
}
