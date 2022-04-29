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
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        public CustomMessageBox()
        {
            InitializeComponent();
        }

        #region Type
        public enum CustomStriing
        {
            YESNO,
            OKCancel,
            OK,
            Error,
            Successfull,
            Information,
            Question,
            Exclamatory,
        }

        #endregion

        #region Methods
        public CustomMessageBox(string Description, string ErrorType, string Result)
        {
            //   rtbMessage = null;
            InitializeComponent();
            rtbMessage.Document.Blocks.Clear();
            rtbMessage.AppendText(Description);
            if (ErrorType == "Successfull")
            {
                BitmapImage ImgTest = new BitmapImage(new Uri(System.Windows.Navigation.BaseUriHelper.GetBaseUri(this), @"/TRMN_KANBAN_PRINTING;component/Images/Successfull.png"));
                imgIcon.Source = ImgTest;
                if (Result == "YESNO")
                {
                    btnYes.Content = "YES";
                    btnNo.Content = "NO";
                    btnOK.Visibility = Visibility.Hidden;
                    btnYes.Focus();
                }
                if (Result == "OKCANCEL")
                {
                    btnYes.Content = "OK";
                    btnNo.Content = "CANCEL";
                    btnOK.Visibility = Visibility.Hidden;
                    btnYes.Focus();
                }
                if (Result == "OK")
                {
                    btnYes.Visibility = Visibility.Hidden;
                    btnNo.Visibility = Visibility.Hidden;
                    btnOK.Focus();
                }
            }
            if (ErrorType == "Error")
            {
                BitmapImage ImgTest = new BitmapImage(new Uri(System.Windows.Navigation.BaseUriHelper.GetBaseUri(this), @"/TRMN_KANBAN_PRINTING;component/Images/error.png"));
                imgIcon.Source = ImgTest;
                if (Result == "YESNO")
                {
                    btnYes.Content = "YES";
                    btnNo.Content = "NO";
                    btnOK.Visibility = Visibility.Hidden;
                    btnYes.Focus();
                }
                if (Result == "OKCANCEL")
                {
                    btnYes.Content = "OK";
                    btnNo.Content = "CANCEL";
                    btnOK.Visibility = Visibility.Hidden;
                    btnYes.Focus();
                }
                if (Result == "OK")
                {
                    btnYes.Visibility = Visibility.Hidden;
                    btnNo.Visibility = Visibility.Hidden;
                    btnOK.Focus();
                }
            }
            if (ErrorType == "Information")
            {
                BitmapImage ImgTest = new BitmapImage(new Uri(System.Windows.Navigation.BaseUriHelper.GetBaseUri(this), @"/TRMN_KANBAN_PRINTING;component/Images/Information.png"));
                imgIcon.Source = ImgTest;
                if (Result == "YESNO")
                {
                    btnYes.Content = "YES";
                    btnNo.Content = "NO";
                    btnOK.Visibility = Visibility.Hidden;
                    btnYes.Focus();
                }
                if (Result == "OKCANCEL")
                {
                    btnYes.Content = "OK";
                    btnNo.Content = "CANCEL";
                    btnOK.Visibility = Visibility.Hidden;
                    btnYes.Focus();
                }
                if (Result == "OK")
                {
                    btnYes.Visibility = Visibility.Hidden;
                    btnNo.Visibility = Visibility.Hidden;
                    btnOK.Focus();
                }
            }
            if (ErrorType == "Question")
            {
                BitmapImage ImgTest = new BitmapImage(new Uri(System.Windows.Navigation.BaseUriHelper.GetBaseUri(this), @"/TRMN_KANBAN_PRINTING;component/Images/Question.png"));
                imgIcon.Source = ImgTest;
                if (Result == "YESNO")
                {
                    btnYes.Content = "YES";
                    btnNo.Content = "NO";
                    btnOK.Visibility = Visibility.Hidden;
                    btnYes.Focus();
                }
                if (Result == "OKCANCEL")
                {
                    btnYes.Content = "OK";
                    btnNo.Content = "CANCEL";
                    btnOK.Visibility = Visibility.Hidden;
                    btnYes.Focus();
                }
                if (Result == "OK")
                {
                    btnYes.Visibility = Visibility.Hidden;
                    btnNo.Visibility = Visibility.Hidden;
                    btnOK.Focus();
                }
            }
            if (ErrorType == "Exclamatory")
            {
                BitmapImage ImgTest = new BitmapImage(new Uri(System.Windows.Navigation.BaseUriHelper.GetBaseUri(this), @"/TRMN_KANBAN_PRINTING;component/Images/Exclamation.png"));
                imgIcon.Source = ImgTest;
                if (Result == "YESNO")
                {
                    btnYes.Content = "YES";
                    btnNo.Content = "NO";
                    btnOK.Visibility = Visibility.Hidden;
                    btnYes.Focus();
                }
                if (Result == "OKCancel")
                {
                    btnYes.Content = "OK";
                    btnNo.Content = "CANCEL";
                    btnOK.Visibility = Visibility.Hidden;
                    btnYes.Focus();
                }
                if (Result == "OK")
                {
                    btnYes.Visibility = Visibility.Hidden;
                    btnNo.Visibility = Visibility.Hidden;
                    btnOK.Focus();
                }
            }
        }

        #endregion

        #region Events
        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CommonClasses.CommonVariable.Result = "YES";
                this.Close();
            }
            catch (Exception ex)
            {
                CommonClasses.CommonMethods.CreatLogDetails(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "CUSTOMMESSAGEBOX", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CommonClasses.CommonVariable.Result = "NO";
                this.Close();
            }
            catch (Exception ex)
            {
                CommonClasses.CommonMethods.CreatLogDetails(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "CustomMessageBox", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.N) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.N))
                {
                    btnNo_Click(sender, e);
                }
                if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.Y) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.Y))
                {
                    btnYes_Click(sender, e);
                }
                if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.O) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.O))
                {
                    btnOK_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                CommonClasses.CommonMethods.CreatLogDetails(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "CUSTOMMESSAGEBOX", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CommonClasses.CommonVariable.Result = "OK";
                this.Close();
            }
            catch (Exception ex)
            {
                CommonClasses.CommonMethods.CreatLogDetails(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "CUSTOMMESSAGEBOX", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        #endregion
      


      
    }
}
