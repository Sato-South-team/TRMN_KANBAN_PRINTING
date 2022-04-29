using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
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

namespace TRMN_KANBAN_PRINTING.StartUp
{
    /// <summary>
    /// Interaction logic for DatabaseSetting.xaml
    /// </summary>
    public partial class DatabaseSetting : Window
    {
        public DatabaseSetting()
        {
            InitializeComponent();
        }

        #region Variabels and Objects
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        #endregion

        #region Methods
        private void ShowCapslock()
        {
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }
        private void GetDBServer()
        {
            DataTable dtDbServer = new DataTable();
            dtDbServer.Columns.Add("Display");
            dtDbServer.Columns.Add("Value");
            DataTable dtResults = SqlDataSourceEnumerator.Instance.GetDataSources();

            string strInstance;
            foreach (DataRow dr in dtResults.Rows)
            {
                if (dr["InstanceName"].ToString() != string.Empty)
                {
                    strInstance = "\\" + dr["InstanceName"].ToString();
                }
                else
                {
                    strInstance = string.Empty;
                }

                DataRow drRow = dtDbServer.NewRow();
                drRow["Display"] = dr["ServerName"].ToString() + strInstance;
                drRow["Value"] = dr["ServerName"].ToString() + strInstance;
                dtDbServer.Rows.Add(drRow);
                CommonClasses.CommonMethods.FillComboBox(cbServerName, dtDbServer, "Display");
            }

        }


        private bool ControlsValidation()
        {
            if (cbServerName.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE SELECT OR ENTER DATABASE SERVER NAME", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                cbServerName.Focus();
                return false;
            }
            if (txtServerID.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER DATABASE SERVER ID", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtServerID.Focus();
                return false;
            }
            if (txtServerPassword.Password == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER DATABASE SERVER PASSWORD", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtServerPassword.Focus();
                return false;
            }
            return true;
        }

        private bool ControlsValidationForSaving()
        {
            if (cbServerName.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE SELECT OR ENTER DATABASE SERVER NAME", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                cbServerName.Focus();
                return false;
            }
            if (txtServerID.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER DATABASE SERVER ID", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtServerID.Focus();
                return false;
            }
            if (txtServerPassword.Password == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER DATABASE SERVER PASSWORD", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtServerPassword.Focus();
                return false;
            }
            if (cbDataBaseName.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER OR SELECT DATABASE NAME", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                cbDataBaseName.Focus();
                return false;
            }
            return true;
        }
        public void getDBSchema(string strSource, string strUser, string strPwd)
        {
            try
            {
                DataTable dtSchema = new DataTable();
                dtSchema.Columns.Add("Display");
                dtSchema.Columns.Add("Value");
                string strCon = "Data Source=" + strSource + ";" + " User ID=" + strUser + "; Password=" + strPwd + ";";

                SqlConnection oCon = new SqlConnection(strCon);
                oCon.Open();
                DataTable dtResults = oCon.GetSchema("Databases"); ;
                oCon.Close();
                foreach (DataRow dr in dtResults.Rows)
                {
                    DataRow drRow = dtSchema.NewRow();
                    drRow["Display"] = dr["database_name"].ToString();
                    drRow["Value"] = dr["database_name"].ToString();
                    dtSchema.Rows.Add(drRow);
                }
                CommonClasses.CommonMethods.FillComboBox(cbDataBaseName, dtSchema, "Display");
            }
            catch (Exception ex)
            {
                CommonClasses.CommonMethods.CreatLogDetails(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "DATABASE_SETTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void Clear()
        {
            cbDataBaseName.SelectedIndex = -1;
            cbDataBaseName.Text = "";
            cbServerName.SelectedIndex = -1;
            cbServerName.Text = "";
            txtServerID.Text = "";
            txtServerPassword.Password = "";
            cbServerName.Focus();
        }

        #endregion

        #region Events
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Boolean Capslock = Console.CapsLock;
            if (txtServerPassword.IsFocused == true)
            {
                if (Capslock == true)
                    txtPasswordPopup.IsOpen = true;
                else
                    txtPasswordPopup.IsOpen = false;
            }
            else
            {
                txtPasswordPopup.IsOpen = false;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                GetDBServer();
                cbServerName.Focus();
                ShowCapslock();
            }
            catch (Exception ex)
            {
                CommonClasses.CommonMethods.CreatLogDetails(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "DATABASE_SETTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

      
        private void btnsave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ControlsValidationForSaving())
                {
                    string DBResiult = "";
                    ENTITY_LAYER.DatabaseSettings.DatabaseSettings.SqldbServer = cbServerName.Text;
                    ENTITY_LAYER.DatabaseSettings.DatabaseSettings.SqlDBUserID = txtServerID.Text;
                    ENTITY_LAYER.DatabaseSettings.DatabaseSettings.SqlDBName = cbDataBaseName.Text;
                    ENTITY_LAYER.DatabaseSettings.DatabaseSettings.SqlDBPassword = txtServerPassword.Password;
                    CommonClasses.CommonMethods.CreatDataBaseLogDetails(ENTITY_LAYER.DatabaseSettings.DatabaseSettings.SqldbServer, ENTITY_LAYER.DatabaseSettings.DatabaseSettings.SqlDBUserID, ENTITY_LAYER.DatabaseSettings.DatabaseSettings.SqlDBPassword, ENTITY_LAYER.DatabaseSettings.DatabaseSettings.SqlDBName);
                    CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.DataSaved.ToString(), CustomMessageBox.CustomStriing.Successfull.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    Clear();
                    this.Hide();
                    Login objLogin = new Login();
                    objLogin.ShowDialog();
                }

            }
            catch (Exception ex)
            {
                CommonClasses.CommonMethods.CreatLogDetails(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "DATABASE_SETTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();

            }
            catch (Exception ex)
            {
                CommonClasses.CommonMethods.CreatLogDetails(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "DATABASE_SETTING", CommonClasses.CommonVariable.UserID);
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
                CommonClasses.CommonMethods.CreatLogDetails(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "DATABASE_SETTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void cbDataBaseName_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ControlsValidation())
                {
                    if (cbDataBaseName.SelectedIndex == -1)
                        getDBSchema(cbServerName.Text, txtServerID.Text, txtServerPassword.Password);
                }
            }
            catch (Exception ex)
            {
                CommonClasses.CommonMethods.CreatLogDetails(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "DATABASE_SETTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }



        private void cbDataBaseName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab)
                btnsave_Click(sender, e);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.S) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.S))
            {
                btnsave_Click(sender, e);
            }

            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.C) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.C))
            {
                btnClear_Click(sender, e);
            }
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.E) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.E))
            {
                btnExit_Click(sender, e);
            }

        }

        private void txtServerPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                Boolean Capslock = Console.CapsLock;
                if (Capslock == true)
                    txtPasswordPopup.IsOpen = true;
            }
            catch (Exception ex)
            {
                CommonClasses.CommonMethods.CreatLogDetails(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "DATABASE_SETTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }

        }

        private void txtServerPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                txtPasswordPopup.IsOpen = false;
            }
            catch (Exception ex)
            {
                CommonClasses.CommonMethods.CreatLogDetails(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "DATABASE_SETTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }
#endregion
    }
}
