using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TRMN_KANBAN_PRINTING.StartUp;

namespace TRMN_KANBAN_PRINTING
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        BUSINESS_LAYER.LogCreation.LogCreation obj_Log = new BUSINESS_LAYER.LogCreation.LogCreation();
       
        private void StartUP(object sender, StartupEventArgs e)
        {
            try
            {
                bool Running;

                using (Mutex mutex = new Mutex(true, "TRMN_KANBAN_SYSTEM", out Running))
                {
                    if (Running)
                    {

                        string data = CommonClasses.CommonMethods.ReadFile(AppDomain.CurrentDomain.BaseDirectory + "\\DataBaseSetting.txt");

                        if (data != "")
                        {
                            data = data.Replace("\r\n", ",");
                            string[] DataSplit = data.Split(',');
                            if (DataSplit.Length > 0)
                            {
                                ENTITY_LAYER.DatabaseSettings.DatabaseSettings.SqldbServer = DataSplit[4].ToString();
                                ENTITY_LAYER.DatabaseSettings.DatabaseSettings.SqlDBUserID = DataSplit[5].ToString();
                                ENTITY_LAYER.DatabaseSettings.DatabaseSettings.SqlDBPassword = DataSplit[6].ToString();
                                ENTITY_LAYER.DatabaseSettings.DatabaseSettings.SqlDBName = DataSplit[7].ToString();
                                StartUp.Login obj_Login = new Login();
                                obj_Login.ShowDialog();
                            }
                            else
                            {
                                CommonClasses.CommonMethods.MessageBoxShow("INCORRECT DATABASE SETTING!!", CustomMessageBox.CustomStriing.Exclamatory.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

                            }
                        }
                        else
                        {
                            StartUp.DatabaseSetting obj_DBS = new StartUp.DatabaseSetting();
                            obj_DBS.ShowDialog();
                        }

                    }
                    else
                    {
                        CommonClasses.CommonMethods.MessageBoxShow("APPLICATION IS ALREADY RUNNING!!!", CustomMessageBox.CustomStriing.Exclamatory.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    }
                }
              
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAINWINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void Grid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
            }
            catch (Exception ex)
            {
               obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAINWINDOW", CommonClasses.CommonVariable.UserID);
               CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void Grid_MouseLeftButtonUp_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                
            }
            catch (Exception ex)
            {
               obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAINWINDOW", CommonClasses.CommonVariable.UserID);
               CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void ContentPresenter_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            
        }
    }
}
