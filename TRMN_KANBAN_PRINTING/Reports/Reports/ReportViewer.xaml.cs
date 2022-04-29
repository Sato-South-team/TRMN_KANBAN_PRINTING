using SAPBusinessObjects.WPF.Viewer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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
using CrystalDecisions.CrystalReports;
namespace TRMN_KANBAN_PRINTING.Reports.Reports
{
    /// <summary>
    /// Interaction logic for ReportViewer.xaml
    /// </summary>
    public partial class ReportViewer : Window
    {

        #region Variables and Objects
        BUSINESS_LAYER.LogCreation.LogCreation obj_Log = new BUSINESS_LAYER.LogCreation.LogCreation();
        BUSINESS_LAYER.Transaction.Transaction obj_Transaction = new BUSINESS_LAYER.Transaction.Transaction();
        public static DataTable dtReport = new DataTable();
        public static string ReportName = "";
        [DllImport("Winspool.drv")]
        private static extern bool SetDefaultPrinter(string printerName);
        #endregion
        public ReportViewer()
        {
            InitializeComponent();
        }



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
                ShowDateTime();
                switch (ReportName)
                {

                    case "Skid No":
                        CrystallReport.SkidNo ObjSkidNo = new CrystallReport.SkidNo();
                        ObjSkidNo.SetDataSource(dtReport);
                        crystalReportsViewer1.ViewerCore.ReportSource = ObjSkidNo;
                        crystalReportsViewer1.ToggleSidePanel = Constants.SidePanelKind.None;
                      //  ObjSkidNo.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, AppDomain.CurrentDomain.BaseDirectory+"\\S"+DateTime.Now.ToString("ddMMyyHHmmss.pdf"));

                        // crystalReportsViewer1.ViewerCore.Zoom(90);
                        break;
                    case "Kanban-Tracking":
                        CrystallReport.TrackingReport ObjTrackingReport = new CrystallReport.TrackingReport();
                        ObjTrackingReport.SetDataSource(dtReport);
                        crystalReportsViewer1.ViewerCore.ReportSource = ObjTrackingReport;
                        crystalReportsViewer1.ToggleSidePanel = Constants.SidePanelKind.None;
                        // crystalReportsViewer1.ViewerCore.Zoom(90);
                        break;
                    case "TKMBARCODE":
                        CrystallReport.TKMLabelDesign ObjTKMLabelDesign = new CrystallReport.TKMLabelDesign();
                        ObjTKMLabelDesign.SetDataSource(dtReport);
                        SetDefaultPrinter("TKM");
                        crystalReportsViewer1.ViewerCore.ReportSource = ObjTKMLabelDesign;
                        crystalReportsViewer1.ToggleSidePanel = Constants.SidePanelKind.None;
                        break;
                    case "TRMNBARCODE":
                        CrystallReport.TRMNlabelDesign ObjTRMNlabelDesign = new CrystallReport.TRMNlabelDesign();
                        ObjTRMNlabelDesign.SetDataSource(dtReport);
                        SetDefaultPrinter("TRMN");
                        crystalReportsViewer1.ViewerCore.ReportSource = ObjTRMNlabelDesign;
                        crystalReportsViewer1.ToggleSidePanel = Constants.SidePanelKind.None;
                        break;
                    case "ORDERS CREATION LOG":
                        CrystallReport.OrderCreationLog ObjOrderCreationLog = new CrystallReport.OrderCreationLog();
                        ObjOrderCreationLog.SetDataSource(dtReport);
                        crystalReportsViewer1.ViewerCore.ReportSource = ObjOrderCreationLog;
                        crystalReportsViewer1.ToggleSidePanel = Constants.SidePanelKind.None;
                       // crystalReportsViewer1.
                        break;
                    case "AUTHENDICATION LOG":
                        CrystallReport.AuthendicationLog ObjAuthendicationLog = new CrystallReport.AuthendicationLog();
                        ObjAuthendicationLog.SetDataSource(dtReport);
                        crystalReportsViewer1.ViewerCore.ReportSource = ObjAuthendicationLog;
                        crystalReportsViewer1.ToggleSidePanel = Constants.SidePanelKind.None;
                        // crystalReportsViewer1.
                        break;
                    case "OPERATION LOG":
                        CrystallReport.OperationLog ObjOperationLog = new CrystallReport.OperationLog();
                        ObjOperationLog.SetDataSource(dtReport);
                        crystalReportsViewer1.ViewerCore.ReportSource = ObjOperationLog;
                        crystalReportsViewer1.ToggleSidePanel = Constants.SidePanelKind.None;
                        // crystalReportsViewer1.
                        break;
                }
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "REPORT_VIEWER", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "REPORT_VIEWER", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
        }
    }
}
