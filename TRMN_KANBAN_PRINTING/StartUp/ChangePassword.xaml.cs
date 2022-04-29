using System;
using System.Collections.Generic;
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
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {
        public ChangePassword()
        {
            InitializeComponent();
        }
        #region Variables and Objects
        BUSINESS_LAYER.LogCreation.LogCreation obj_Log = new BUSINESS_LAYER.LogCreation.LogCreation();
        BUSINESS_LAYER.Login.Login obj_Login = new BUSINESS_LAYER.Login.Login();
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        #endregion

        #region methods
        private void ShowCapslock()
        {
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }
        private void Clear()
        {
            txtUserID.Text = "";
            txtOldPassowrd.Password = "";
            txtNewPassword.Password = "";
            txtConfirmedPassword.Password = "";
            txtUserID.Focus();
        }
        private void Transaction()
        {
            ENTITY_LAYER.Login.Login.UserID = txtUserID.Text;
            ENTITY_LAYER.Login.Login.Password = txtOldPassowrd.Password;
            ENTITY_LAYER.Login.Login.ConfirmPassword = txtConfirmedPassword.Password;
            ENTITY_LAYER.Login.Login.Type = "ChangePassword";
            CommonClasses.CommonVariable.Result = obj_Login.BL_Login();
            if (CommonClasses.CommonVariable.Result == "Updated")
            {
                CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.DataUpdated, CustomMessageBox.CustomStriing.Successfull.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                Clear();
            }
            else if (CommonClasses.CommonVariable.Result == "INVALID USER ID")
            {
                CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.Result, CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtUserID.Focus();
            }

            else if (CommonClasses.CommonVariable.Result == "INVALID PASSWORD")
            {
                CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.Result, CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtOldPassowrd.Focus();
            }
            else
            {
                CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.Result, CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtUserID.Focus();
            }

        }

        #endregion

        #region Events
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Boolean Capslock = Console.CapsLock;
            if (txtConfirmedPassword.IsFocused == true || txtOldPassowrd.IsFocused == true || txtNewPassword.IsFocused == true)
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


        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "CHANGE_PASSWORD", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }


        private void btnsave_Click(object sender, RoutedEventArgs e)
        {
            if (txtUserID.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER USER ID", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtUserID.Focus();
            }
            else if (txtOldPassowrd.Password == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER OLD PASSWORD", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtOldPassowrd.Focus();
            }
            else if (txtNewPassword.Password == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER NEW PASSWORD", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtNewPassword.Focus();
            }
            else if (txtConfirmedPassword.Password == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER CONFIRMED PASSWORD", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtConfirmedPassword.Focus();
            }
            else if (txtNewPassword.Password != txtConfirmedPassword.Password)
            {
                CommonClasses.CommonMethods.MessageBoxShow("NEW AND CONFIRMED PASWWORD IS NOT MATCHING", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtNewPassword.Focus();
            }
            else
                Transaction();
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StartUp.Login obj_Login = new Login();
                this.Close();
                obj_Login.ShowDialog();
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "CHANGE_PASSWORD", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                txtUserID.Focus();
                ShowCapslock();
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "CHANGE_PASSWORD", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }
        #endregion
    }
}
