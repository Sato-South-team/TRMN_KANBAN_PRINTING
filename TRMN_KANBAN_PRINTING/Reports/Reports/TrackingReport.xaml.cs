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
using System.Windows.Threading;
using TRMN_KANBAN_PRINTING.CommonClasses;
using TRMN_KANBAN_PRINTING.StartUp;

namespace TRMN_KANBAN_PRINTING.Reports.Reports
{
    /// <summary>
    /// Interaction logic for TrackingReport.xaml
    /// </summary>
    public partial class TrackingReport : Window
    {
        public TrackingReport()
        {
            InitializeComponent();
        }
        #region Variables and Objects
        BUSINESS_LAYER.LogCreation.LogCreation obj_Log = new BUSINESS_LAYER.LogCreation.LogCreation();
        BUSINESS_LAYER.Reports.Reports obj_Report = new BUSINESS_LAYER.Reports.Reports();
        DataTable dt = new DataTable();
        #endregion

        private void ShowDateTime()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(this.dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e) => this.txtDatetime.Text = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss");

        private void Report(string Type)
        {
            if (Type == "ColumnName")
            {
                ENTITY_LAYER.Reports.Reports.Type = Type;
                ENTITY_LAYER.Reports.Reports.ColumnName = this.cbName.Text;
                this.dt = this.obj_Report.BL_ReportDetails().Tables[0];
                CommonMethods.FillComboBox(this.cbValue, this.dt, this.cbName.Text);
            }
            if (!(Type == "KanbanTracking"))
                return;
            ENTITY_LAYER.Reports.Reports.Type = Type;
            if (this.cbValue.SelectedIndex > -1)
            {
                if (this.cbValue.Text != "ALL")
                {
                    ENTITY_LAYER.Reports.Reports.ColumnValue = "= '" + this.cbValue.Text + "' and convert(date, CreatedOn, 103) between convert(date,'" + this.dtpFrom.SelectedDate.Value.ToString("dd MMM yyyy") + "',103) and convert(date,'" + this.dtpTo.SelectedDate.Value.ToString("dd MMM yyyy") + "',103)";
                }
                else
                {
                    string[] strArray = new string[5]
                    {
            "<> '' and convert(date, CreatedOn, 103) between convert(date, '",
            null,
            null,
            null,
            null
                    };
                    DateTime dateTime = this.dtpFrom.SelectedDate.Value;
                    strArray[1] = dateTime.ToString("dd MMM yyyy");
                    strArray[2] = "',103) and convert(date,'";
                    dateTime = this.dtpTo.SelectedDate.Value;
                    strArray[3] = dateTime.ToString("dd MMM yyyy");
                    strArray[4] = "',103)";
                    ENTITY_LAYER.Reports.Reports.ColumnValue = string.Concat(strArray);
                }
                ENTITY_LAYER.Reports.Reports.ColumnName = this.cbName.Text;
            }
            else
            {
                ENTITY_LAYER.Reports.Reports.ColumnValue = "between convert(date, '" + this.dtpFrom.SelectedDate.Value.ToString("dd MMM yyyy") + "',103) and convert(date, '" + this.dtpTo.SelectedDate.Value.ToString("dd MMM yyyy") + "',103)";
                ENTITY_LAYER.Reports.Reports.ColumnName = "convert(date, CreatedOn, 103)";
            }
            DataTable table = this.obj_Report.BL_ReportDetails().Tables[0];
            ReportViewer reportViewer = new ReportViewer();
            ReportViewer.dtReport = table;
            ReportViewer.ReportName = "Kanban-Tracking";
            reportViewer.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.WindowState = WindowState.Maximized;
                this.Width = SystemParameters.PrimaryScreenWidth;
                this.Height = SystemParameters.PrimaryScreenHeight;
                this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
                this.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
                Application.Current.MainWindow = (Window)this;
                this.SizeToContent = SizeToContent.Manual;
                this.ShowDateTime();
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_TRACKING", CommonVariable.UserID);
                CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow mainWindow = new MainWindow();
                this.Close();
                mainWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_TRACKING", CommonVariable.UserID);
                string Description = ex.Message.ToString();
                CustomMessageBox.CustomStriing customStriing = CustomMessageBox.CustomStriing.Error;
                string ErrorType = customStriing.ToString();
                customStriing = CustomMessageBox.CustomStriing.OK;
                string Result = customStriing.ToString();
                CommonMethods.MessageBoxShow(Description, ErrorType, Result);
            }
        }

        private void Clear()
        {
            this.dtpFrom.Text = "";
            this.dtpTo.Text = "";
            this.cbValue.SelectedIndex = -1;
            this.cbName.SelectedIndex = -1;
            this.cbName.Focus();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Clear();
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_TRACKING", CommonVariable.UserID);
                string Description = ex.Message.ToString();
                CustomMessageBox.CustomStriing customStriing = CustomMessageBox.CustomStriing.Error;
                string ErrorType = customStriing.ToString();
                customStriing = CustomMessageBox.CustomStriing.OK;
                string Result = customStriing.ToString();
                CommonMethods.MessageBoxShow(Description, ErrorType, Result);
            }
        }

        private bool ControlValidation()
        {
            if (this.dtpFrom.Text == "")
            {
                CommonMethods.MessageBoxShow("PLEASE SELECT FROM DATE.", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                this.dtpFrom.Focus();
                return false;
            }
            if (!(this.dtpTo.Text == ""))
                return true;
            CommonMethods.MessageBoxShow("PLEASE SELECT TO DATE.", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            this.dtpTo.Focus();
            return false;
        }

        private void BtnShow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!this.ControlValidation())
                    return;
                this.Report("KanbanTracking");
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_TRACKING", CommonVariable.UserID);
                string Description = ex.Message.ToString();
                CustomMessageBox.CustomStriing customStriing = CustomMessageBox.CustomStriing.Error;
                string ErrorType = customStriing.ToString();
                customStriing = CustomMessageBox.CustomStriing.OK;
                string Result = customStriing.ToString();
                CommonMethods.MessageBoxShow(Description, ErrorType, Result);
            }
        }

        private void CbName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.cbName.SelectedIndex <= -1)
                    return;
                this.cbName.Text = ((ContentControl)this.cbName.SelectedItem).Content.ToString();
                this.Report("ColumnName");
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_TRACKING", CommonVariable.UserID);
                CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

    }
}
