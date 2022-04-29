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
    /// Interaction logic for GroupMaster.xaml
    /// </summary>
    public partial class GroupMaster : Window
    {
        public GroupMaster()
        {
            InitializeComponent();
        }

        #region Variable and Objects
        BUSINESS_LAYER.LogCreation.LogCreation obj_Log = new BUSINESS_LAYER.LogCreation.LogCreation();
        BUSINESS_LAYER.Masters.Masters obj_Mast = new BUSINESS_LAYER.Masters.Masters();
        #endregion

        #region Methods
        private void ShowDateTime()
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void Transaction(string Type)
        {
            if (Type == "Save" || Type == "Delete")
            {
                string Rights = "";
                ENTITY_LAYER.Masters.Masters.GroupID = cmbgroupid.Text;
                ENTITY_LAYER.Masters.Masters.Type = Type;
                for (int i = 0; i < dvgModules.Items.Count; i++)
                {
                    DataRowView Row = (DataRowView)dvgModules.Items[i];
                    string Flag = Row["Flag"].ToString();
                    if (Flag == "True")
                    {
                        Rights = Rights + Row["ModuleName"].ToString() + ",";
                    }

                }

                if (Rights == "")
                {
                    CommonClasses.CommonMethods.MessageBoxShow("PLEASE SELECT MODULES FROM LIST VIEW", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    return;
                }

                ENTITY_LAYER.Masters.Masters.Rights = Rights;
                CommonClasses.CommonVariable.Result = obj_Mast.BL_GroupMasterTransaction();
                if (CommonClasses.CommonVariable.Result == "Saved")
                {
                    CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.DataSaved, CustomMessageBox.CustomStriing.Successfull.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    Clear();
                }
                else if (CommonClasses.CommonVariable.Result == "Deleted")
                {
                    CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.DataDeleted, CustomMessageBox.CustomStriing.Successfull.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    Clear();
                }
                else
                    CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.Result, CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
            if (Type == "LoadFormDetails")
            {
                ENTITY_LAYER.Masters.Masters.Type = Type;
                DataTable dt = obj_Mast.BL_GroupMasterDetails().Tables[0];
                dt.Columns["Flag"].ReadOnly = false;
                dvgModules.ItemsSource = dt.DefaultView;
            }
            if (Type == "getRightsDetails")
            {
                GridItemUnChecked();
                DataTable dtGoupDetails = new DataTable();
                ENTITY_LAYER.Masters.Masters.Type = Type;
                ENTITY_LAYER.Masters.Masters.GroupID = Convert.ToString(cmbgroupid.SelectedValue);
                dtGoupDetails = obj_Mast.BL_GroupMasterDetails().Tables[0];
                if (cmbgroupid.SelectedIndex == -1)
                    CommonClasses.CommonMethods.FillComboBox(cmbgroupid, dtGoupDetails, "GroupID", "GroupID");
                else
                {

                    if (dtGoupDetails.Rows.Count > 0)
                    {
                        string[] ModuleName = dtGoupDetails.Rows[0]["Rights"].ToString().Split(',');
                        for (int i = 0; i < dvgModules.Items.Count; i++)
                        {
                            for (int j = 0; j < ModuleName.Length; j++)
                            {
                                DataRowView row = (DataRowView)dvgModules.Items[i];
                                if (row["ModuleName"].ToString() == ModuleName[j])
                                {
                                    row["Flag"] = "True";
                                }
                            }
                        }
                    }
                }
            }
        }
        private void Clear()
        {
            cmbgroupid.SelectedIndex = -1;
            cmbgroupid.Text = "";
            Transaction("getRightsDetails");
            GridItemUnChecked();
            cmbgroupid.Focus();
        }

        private void GridItemUnChecked()
        {
            PCDetails.IsChecked = false;
            for (int i = 0; i < dvgModules.Items.Count; i++)
            {
                DataRowView row = (DataRowView)dvgModules.Items[i];
                row["Flag"] = "false";
            }

        }
        private void GridItemChecked()
        {
            PCDetails.IsChecked = true;
            for (int i = 0; i < dvgModules.Items.Count; i++)
            {
                DataRowView row = (DataRowView)dvgModules.Items[i];
                row["Flag"] = "True";
            }

        }

        #endregion

        #region Events

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
                Transaction("LoadFormDetails");
                Transaction("getRightsDetails");
                ShowDateTime();
                cmbgroupid.Focus();
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "GROUP_MASTER", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbgroupid.Text != "")
                {
                    Transaction("Save");
                }
                else
                {
                    CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER GROUP ID", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    cmbgroupid.Focus();
                }
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "GROUP_MASTER", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbgroupid.SelectedIndex > -1)
                {
                    CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.DeleteConfirm, CustomMessageBox.CustomStriing.Question.ToString(), CustomMessageBox.CustomStriing.YESNO.ToString());
                    if (CommonClasses.CommonVariable.Result == "YES")
                    {
                        Transaction("Delete");
                    }
                }
                else
                {
                    CommonClasses.CommonMethods.MessageBoxShow("PLEASE SELECT GROUP ID", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    cmbgroupid.Focus();
                }
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "GROUP_MASTER", CommonClasses.CommonVariable.UserID);
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
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "GROUP_MASTER", CommonClasses.CommonVariable.UserID);
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
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "GROUP_MASTER", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void PCDetails_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                GridItemChecked();
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "GROUP_MASTER", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void PCDetails_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                GridItemUnChecked();
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "GROUP_MASTER", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }
        private void cmbgroupid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbgroupid.SelectedIndex > -1)
                {
                    Transaction("getRightsDetails");
                }
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "GROUP_MASTER", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.S) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.S))
            {
                btnSave_Click(sender, e);
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

        }
        #endregion
    }
}
