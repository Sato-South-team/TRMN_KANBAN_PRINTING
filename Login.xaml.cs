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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        BUSINESS_LAYER.LogCreation.LogCreation obj_Log = new BUSINESS_LAYER.LogCreation.LogCreation();
        BUSINESS_LAYER.Login.Login obj_Login = new BUSINESS_LAYER.Login.Login();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                txtUserID.Focus();
                Version Version = Assembly.GetExecutingAssembly().GetName().Version;
                txtVersion.Text = String.Format(this.txtVersion.Text, Version.Major, Version.Minor, Version.Build, Version.Revision);

            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "LOGIN", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void ValidateLogin()
        {
            ENTITY_LAYER.Login.Login.UserID = txtUserID.Text;
            ENTITY_LAYER.Login.Login.Password = txtPassword.Password;
            ENTITY_LAYER.Login.Login.Type = "Login";
            CommonClasses.CommonVariable.Result = obj_Login.BL_Login();
            if (CommonClasses.CommonVariable.Result.StartsWith("VALID CREDENTIAL"))
            {
                CommonClasses.CommonVariable.UserName = CommonClasses.CommonVariable.Result.Split('+')[1].ToString();
                CommonClasses.CommonVariable.Rights = CommonClasses.CommonVariable.Result.Split('+')[2].ToString();
                StartUp.MainWindow objMainWindow = new MainWindow();
                this.Close();
                objMainWindow.ShowDialog();
            }
            else if (CommonClasses.CommonVariable.Result == "INVALID USER ID")
            {
                CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.Result, CustomMessageBox.CustomStriing.Exclamatory.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtUserID.Text = "";
                txtUserID.Focus();
            }
            else if (CommonClasses.CommonVariable.Result == "INVALID PASSWORD")
            {
                CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.Result, CustomMessageBox.CustomStriing.Exclamatory.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtPassword.Password = "";
                txtPassword.Focus();
            }
            else if (CommonClasses.CommonVariable.Result == "FIRST TIME LOGIN")
            {
                if ((txtUserID.Text.ToUpper() == "SARBLR") && (txtPassword.Password.ToUpper() == "SARBLR"))
                {
                    ENTITY_LAYER.Login.Login.UserName = "SARBLR";
                    ENTITY_LAYER.Login.Login.Rights = "USER MASTER,GROUP MASTER";
                    StartUp.MainWindow objMainWindow = new MainWindow();
                    this.Close();
                    objMainWindow.ShowDialog();
                }
                else
                {
                    CommonClasses.CommonMethods.MessageBoxShow("FIRST TIME LOGIN. PLEASE ENTER VALID USER ID OR PASSWORD", CustomMessageBox.CustomStriing.Exclamatory.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    return;
                }
            }
            else
            {
                CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.Result, CustomMessageBox.CustomStriing.Exclamatory.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtUserID.Focus();
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtUserID.Text == "")
                {
                    CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER THE USER ID", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    txtUserID.Focus();
                    return;
                }
                if (txtPassword.Password == "")
                {
                    CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER THE PASSWORD", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    txtPassword.Focus();
                    return;
                }
                ValidateLogin();
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "LOGIN", CommonClasses.CommonVariable.UserID);
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
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "LOGIN", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void LinkForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Hide();
                ForgotPassword objForgotPassword = new ForgotPassword();
                objForgotPassword.ShowDialog();
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "LOGIN", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void LinkChangePassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Hide();
                ChangePassword objChangePassword = new ChangePassword();
                objChangePassword.ShowDialog();
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "LOGIN", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.L) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.L))
            {
                btnLogin_Click(sender, e);
            }
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.E) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.E))
            {
                btnExit_Click(sender, e);
            }
        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                btnLogin_Click(sender, e);
        }
    }
}
