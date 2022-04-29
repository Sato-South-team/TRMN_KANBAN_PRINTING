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
using System.Printing;
using System.Drawing.Printing;
using System.Diagnostics;
using System.Data;

namespace TRMN_KANBAN_PRINTING.StartUp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Varialble and Objects
        BUSINESS_LAYER.LogCreation.LogCreation obj_Log = new BUSINESS_LAYER.LogCreation.LogCreation();
        BUSINESS_LAYER.Transaction.Transaction obj_Transaction = new BUSINESS_LAYER.Transaction.Transaction();
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
            ENTITY_LAYER.Transaction.Transaction.Type = Type;
            DataSet ds = obj_Transaction.BL_TransactionDetails();
            txtWornning.AppendText("");
            txtWornning.Background = Brushes.White;
            txtWorning.Text = "";
            if (ds.Tables[0].Rows.Count>0)
            {
                if(ds.Tables[0].Rows[0]["Value"].ToString()!="")
                {
                    txtWornning.AppendText("These kanbans are new kanbans, and these are printed at first time :- " + ds.Tables[0].Rows[0]["Value"].ToString().Remove(0,1) + '\r');
                    txtWornning.Background = Brushes.Red;
                    txtWorning.Text = "WARNINGS";
                }
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                if (ds.Tables[1].Rows[0]["Value"].ToString() != "")
                {
                    txtWornning.AppendText("These are BO kanbans and These kanbans are no more to print :- " + ds.Tables[1].Rows[0]["Value"].ToString().Remove(0,1));
                    txtWornning.Background = Brushes.Red;
                    txtWorning.Text = "WARNINGS";
                }
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
                txtuserID.Text ="Application is using by "+  CommonClasses.CommonVariable.UserName;
                SizeToContent = SizeToContent.Manual;
                Transaction("KanbanValidations");
                ShowDateTime();
              

            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void BtnMasters_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int ControlsCount = 0;
                GridSubMenu.Children.RemoveRange(0, 9);

                if (CommonClasses.CommonVariable.Rights == "" || CommonClasses.CommonVariable.Rights == null)
                {
                    CommonClasses.CommonMethods.MessageBoxShow("NO RIGHTS TO ACCESS THE MASTERS", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    return;
                }
                for (int J = 0; J < GridSubMenu.RowDefinitions.Count; J++)
                {
                    if (ControlsCount != 4)   //how many Controls in Grid
                    {
                        for (int i = 0; i < GridSubMenu.ColumnDefinitions.Count; i++)
                        {
                            if (i == 0 && J == 0)
                            {
                                Grid g = new Grid();
                                Button obj = new Button();
                                obj.Content = "USER MASTER";
                                obj.Height = 100;
                                obj.Width = 230;
                                //  obj.Style = (Style)FindResource("SubMenuButton");
                                Grid.SetColumn(obj, i);
                                Grid.SetRow(obj, J);
                                GridSubMenu.Children.Add(obj);
                                ControlsCount = ControlsCount + 1;

                                if (CommonClasses.CommonVariable.Rights.Contains("USER MASTER"))
                                {
                                    obj.Click += UserMaster_Click;
                                }
                                else
                                {
                                    obj.Click -= UserMaster_Click;
                                    obj.ToolTip = "Access Denied";
                                }
                            }
                            if (i == 1 && J == 0)
                            {
                                Grid g = new Grid();
                                Button obj = new Button();
                                obj.Content = "GROUP MASTER";
                                obj.Height = 100;
                                obj.Width = 230;
                                // obj.Style = (Style)FindResource("SubMenuButton");
                                Grid.SetColumn(obj, i);
                                Grid.SetRow(obj, J);
                                GridSubMenu.Children.Add(obj);
                                ControlsCount = ControlsCount + 1;
                               // obj.Click += GroupMaster_Click;
                                if (CommonClasses.CommonVariable.Rights.Contains("GROUP MASTER"))
                                    obj.Click += GroupMaster_Click;
                                else
                                {
                                    obj.Click -= GroupMaster_Click;
                                    obj.ToolTip = "Access Denied";
                                }
                            }
                            if (i == 2 && J == 0)
                            {
                                Grid g = new Grid();
                                Button obj = new Button();
                                obj.Content = "PART MASTER";
                                obj.Height = 100;
                                obj.Width = 230;
                                // obj.Style = (Style)FindResource("SubMenuButton");
                                Grid.SetColumn(obj, i);
                                Grid.SetRow(obj, J);
                                GridSubMenu.Children.Add(obj);
                                ControlsCount = ControlsCount + 1;
                              //  obj.Click += GroupMaster_Click;
                                if (CommonClasses.CommonVariable.Rights.Contains("PART MASTER"))
                                    obj.Click += PartMaster_Click;
                                else
                                {
                                    obj.Click -= PartMaster_Click;
                                    obj.ToolTip = "Access Denied";
                                }
                            }
                            if (i == 3 && J == 0)
                            {
                                Grid g = new Grid();
                                Button obj = new Button();
                                obj.Content = "LOCATION MASTER";
                                obj.Height = 100;
                                obj.Width = 230;
                                // obj.Style = (Style)FindResource("SubMenuButton");
                                Grid.SetColumn(obj, i);
                                Grid.SetRow(obj, J);
                                GridSubMenu.Children.Add(obj);
                                ControlsCount = ControlsCount + 1;
                               // obj.Click += GroupMaster_Click;
                                if (CommonClasses.CommonVariable.Rights.Contains("LOCATION MASTER"))
                                    obj.Click += SortLocation_Click;
                                else
                                {
                                    obj.Click -= SortLocation_Click;
                                    obj.ToolTip = "Access Denied";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void SortLocation_Click(object sender, RoutedEventArgs e)
        {
           try
            {
                Masters.SortingLocationMaster obj_SortingLocationMaster = new Masters.SortingLocationMaster();
                this.Close();
                obj_SortingLocationMaster.ShowDialog();
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void PartMaster_Click(object sender, RoutedEventArgs e)
        {
          try
            {
                Masters.PartMaster obj_PartMaster = new Masters.PartMaster();
                this.Close();
                obj_PartMaster.ShowDialog();
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void GroupMaster_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Masters.GroupMaster obj_GroupMaster = new Masters.GroupMaster();
                this.Close();
                obj_GroupMaster.ShowDialog();
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }

        }

        private void UserMaster_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Masters.UserMaster obj_UserMaster = new Masters.UserMaster();
                this.Close();
                obj_UserMaster.ShowDialog();
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }

        }

        private void BtnTransport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int ControlsCount = 0;
                GridSubMenu.Children.RemoveRange(0, 9);

                if (CommonClasses.CommonVariable.Rights == "" || CommonClasses.CommonVariable.Rights == null)
                {
                    CommonClasses.CommonMethods.MessageBoxShow("NO RIGHTS TO ACCESS THE MASTERS", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    return;
                }
                for (int J = 0; J < GridSubMenu.RowDefinitions.Count; J++)
                {
                    if (ControlsCount != 3)   //how many Controls in Grid
                    {
                        for (int i = 0; i < GridSubMenu.ColumnDefinitions.Count; i++)
                        {
                            if (i == 0 && J == 0)
                            {
                                Grid g = new Grid();
                                Button obj = new Button();
                                obj.Content = "KANBAN PRINT";
                                obj.Height = 100;
                                obj.Width = 230;
                                //  obj.Style = (Style)FindResource("SubMenuButton");
                                Grid.SetColumn(obj, i);
                                Grid.SetRow(obj, J);
                                GridSubMenu.Children.Add(obj);
                                ControlsCount = ControlsCount + 1;

                                if (CommonClasses.CommonVariable.Rights.Contains("KANBAN PRINT"))
                                {
                                    obj.Click += KanbanPrint_Click;
                                }
                                else
                                {
                                    obj.Click -= KanbanPrint_Click;
                                    obj.ToolTip = "Access Denied";
                                }
                            }
                            if (i == 1 && J == 0)
                            {
                                Grid g = new Grid();
                                Button obj = new Button();
                                obj.Content = "KANBAN REPRINT";
                                obj.Height = 100;
                                obj.Width = 230;
                                // obj.Style = (Style)FindResource("SubMenuButton");
                                Grid.SetColumn(obj, i);
                                Grid.SetRow(obj, J);
                                GridSubMenu.Children.Add(obj);
                                ControlsCount = ControlsCount + 1;

                                if (CommonClasses.CommonVariable.Rights.Contains("KANBAN REPRINT"))
                                    obj.Click += KanbanReprint_Click;
                                else
                                {
                                    obj.Click -= KanbanReprint_Click;
                                    obj.ToolTip = "Access Denied";
                                }
                            }
                            if (i == 2 && J == 0)
                            {
                                Grid g = new Grid();
                                Button obj = new Button();
                                obj.Content = "KANBAN ELIMINATION";
                                obj.Height = 100;
                                obj.Width = 230;
                                // obj.Style = (Style)FindResource("SubMenuButton");
                                Grid.SetColumn(obj, i);
                                Grid.SetRow(obj, J);
                                GridSubMenu.Children.Add(obj);
                                ControlsCount = ControlsCount + 1;

                                if (CommonClasses.CommonVariable.Rights.Contains("KANBAN ELIMINATION"))
                                    obj.Click += KanbanElimination_Click;
                                else
                                {
                                    obj.Click -= KanbanElimination_Click;
                                    obj.ToolTip = "Access Denied";
                                }
                            }
                            if (i == 3 && J == 0)
                            {
                                Grid g = new Grid();
                                Button obj = new Button();
                                obj.Content = "KANBAN DASHBOARD";
                                obj.Height = 100;
                                obj.Width = 230;
                                // obj.Style = (Style)FindResource("SubMenuButton");
                                Grid.SetColumn(obj, i);
                                Grid.SetRow(obj, J);
                                GridSubMenu.Children.Add(obj);
                                ControlsCount = ControlsCount + 1;

                                if (CommonClasses.CommonVariable.Rights.Contains("DASHBOARD"))
                                    obj.Click += KanbanDashhBoard_Click;
                                else
                                {
                                    obj.Click -= KanbanDashhBoard_Click;
                                    obj.ToolTip = "Access Denied";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
        }

        private void KanbanDashhBoard_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Transaction.DashBoard obj_DashBoard = new Transaction.DashBoard();
                this.Close();
                obj_DashBoard.ShowDialog();
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void KanbanElimination_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Transaction.KanbanElimination obj_KanbanElimination = new Transaction.KanbanElimination();
                this.Close();
                obj_KanbanElimination.ShowDialog();
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void KanbanPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Transaction.KanbanPrint obj_kanbanPrint = new Transaction.KanbanPrint();
                this.Close();
                obj_kanbanPrint.ShowDialog();
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }

        }

        private void KanbanReprint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Transaction.KanbanReprint obj_KanbanReprint = new Transaction.KanbanReprint();
                this.Close();
                obj_KanbanReprint.ShowDialog();
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }

        }

       
     
        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int ControlsCount = 0;
                GridSubMenu.Children.RemoveRange(0, 9);

                if (CommonClasses.CommonVariable.Rights == "" || CommonClasses.CommonVariable.Rights == null)
                {
                    CommonClasses.CommonMethods.MessageBoxShow("NO RIGHTS TO ACCESS THE MASTERS", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    return;
                }
                for (int J = 0; J < GridSubMenu.RowDefinitions.Count; J++)
                {
                    if (ControlsCount != 3)   //how many Controls in Grid
                    {
                        for (int i = 0; i < GridSubMenu.ColumnDefinitions.Count; i++)
                        {
                            if (i == 0 && J == 0)
                            {
                                Grid g = new Grid();
                                Button obj = new Button();
                                obj.Content = "TRACKING REPORT";
                                obj.Height = 100;
                                obj.Width = 230;
                                //  obj.Style = (Style)FindResource("SubMenuButton");
                                Grid.SetColumn(obj, i);
                                Grid.SetRow(obj, J);
                                GridSubMenu.Children.Add(obj);
                                ControlsCount = ControlsCount + 1;

                                if (CommonClasses.CommonVariable.Rights.Contains("TRACKING REPORT"))
                                {
                                    obj.Click += TrackingReport_Click;
                                }
                                else
                                {
                                    obj.Click -= TrackingReport_Click;
                                    obj.ToolTip = "Access Denied";
                                }
                            }
                            if (i == 1 && J == 0)
                            {
                                Grid g = new Grid();
                                Button obj = new Button();
                                obj.Content = "TRANSACTION LOG REPORT";
                                obj.Height = 100;
                                obj.Width = 230;
                                //  obj.Style = (Style)FindResource("SubMenuButton");
                                Grid.SetColumn(obj, i);
                                Grid.SetRow(obj, J);
                                GridSubMenu.Children.Add(obj);
                                ControlsCount = ControlsCount + 1;

                                if (CommonClasses.CommonVariable.Rights.Contains("TRANSACTION LOG REPORT"))
                                {
                                    obj.Click += TransactionLogReport_Click;
                                }
                                else
                                {
                                    obj.Click -= TransactionLogReport_Click;
                                    obj.ToolTip = "Access Denied";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
        }
        private void TrackingReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Reports.Reports.TrackingReport obj_TrackingReport = new Reports.Reports.TrackingReport();
                // Reports.Reports.ReportViewer.dtReport=
                this.Close();
                obj_TrackingReport.ShowDialog();
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }

        }
        private void TransactionLogReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Reports.Reports.LogReport obj_LogReportReport = new Reports.Reports.LogReport();
                // Reports.Reports.ReportViewer.dtReport=
                this.Close();
                obj_LogReportReport.ShowDialog();
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }
        #endregion
    }
}
