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

namespace TRMN_KANBAN_PRINTING.Transaction
{
    /// <summary>
    /// Interaction logic for KanbanElimination.xaml
    /// </summary>
    public partial class KanbanElimination : Window
    {
        public KanbanElimination()
        {
            InitializeComponent();
        }
        #region Variables and Objects
        BUSINESS_LAYER.LogCreation.LogCreation obj_Log = new BUSINESS_LAYER.LogCreation.LogCreation();
        BUSINESS_LAYER.Transaction.Transaction obj_Transaction = new BUSINESS_LAYER.Transaction.Transaction();
        DataTable dt = new DataTable();
        string BarcodeValue;
        #endregion

        private void ShowDateTime()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(this.dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e) => this.txtDatetime.Text = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss");

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
                this.Transaction("GetDetails");
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_ELIMINATION", CommonVariable.UserID);
                CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void Transaction(string Type)
        {
            if (Type == "GetDetails")
            {
                ENTITY_LAYER.Transaction.Transaction.TransactionType = nameof(KanbanElimination);
                ENTITY_LAYER.Transaction.Transaction.Type = Type;
                this.dt = this.obj_Transaction.BL_TransactionDetails().Tables[0];
                CommonMethods.FillComboBox(this.cmbPDS, new DataView(this.dt).ToTable(true, "PDSNo"), "PDSNo");
            }
            if (!(Type == "Save") || !this.Controlvalidation())
                return;
            ENTITY_LAYER.Transaction.Transaction.TransactionType = nameof(KanbanElimination);
            ENTITY_LAYER.Transaction.Transaction.Type = Type;
            ENTITY_LAYER.Transaction.Transaction.EkanbanNo = this.cmbKanban.Text;
            ENTITY_LAYER.Transaction.Transaction.PDSBarcode = this.cmbPDS.Text;
            this.dt = this.obj_Transaction.BL_TransactionDetails().Tables[0];
            this.dvgDeatils.ItemsSource = this.dt.DefaultView;
            CommonMethods.MessageBoxShow(CommonVariable.DataDeleted, CustomMessageBox.CustomStriing.Successfull.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            this.cmbKanban.SelectedIndex = -1;
        }

        private void Clear()
        {
            this.cmbKanban.SelectedIndex = -1;
            this.cmbPDS.SelectedIndex = -1;
            this.cmbPDS.Focus();
            this.dvgDeatils.ItemsSource = null;
        }

        private bool Controlvalidation()
        {
            if (this.cmbPDS.SelectedIndex == -1)
            {
                CommonMethods.MessageBoxShow("PLEASE SELECT PDS NO", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                this.cmbPDS.Focus();
                return false;
            }
            if (this.cmbKanban.SelectedIndex != -1)
                return true;
            CommonMethods.MessageBoxShow("PLEASE SELECT KANBAN NO ", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            this.cmbKanban.Focus();
            return false;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Transaction("Save");
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_ELIMINATION", CommonVariable.UserID);
                CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Clear();
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_ELIMINATION", CommonVariable.UserID);
                string Description = ex.Message.ToString();
                CustomMessageBox.CustomStriing customStriing = CustomMessageBox.CustomStriing.Error;
                string ErrorType = customStriing.ToString();
                customStriing = CustomMessageBox.CustomStriing.OK;
                string Result = customStriing.ToString();
                CommonMethods.MessageBoxShow(Description, ErrorType, Result);
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow mainWindow = new MainWindow();
                this.Close();
                mainWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_ELIMINATION", CommonVariable.UserID);
                string Description = ex.Message.ToString();
                CustomMessageBox.CustomStriing customStriing = CustomMessageBox.CustomStriing.Error;
                string ErrorType = customStriing.ToString();
                customStriing = CustomMessageBox.CustomStriing.OK;
                string Result = customStriing.ToString();
                CommonMethods.MessageBoxShow(Description, ErrorType, Result);
            }
        }

        private void CmbPDS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.cmbPDS.SelectedIndex <= -1)
                    return;
                this.cmbPDS.Text = ((DataRowView)this.cmbPDS.SelectedItem)[0].ToString();
                DataView dataView = new DataView(this.dt);
                dataView.RowFilter = "PDSNo='" + this.cmbPDS.Text + "'";
                CommonMethods.FillComboBox(this.cmbKanban, dataView.ToTable(true, "EkanbanNo"), "EkanbanNo");
                this.dvgDeatils.ItemsSource = dataView.ToTable().DefaultView;
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_ELIMINATION", CommonVariable.UserID);
                string Description = ex.Message.ToString();
                CustomMessageBox.CustomStriing customStriing = CustomMessageBox.CustomStriing.Error;
                string ErrorType = customStriing.ToString();
                customStriing = CustomMessageBox.CustomStriing.OK;
                string Result = customStriing.ToString();
                CommonMethods.MessageBoxShow(Description, ErrorType, Result);
            }
        }

        private void CmbKanban_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.cmbKanban.SelectedIndex <= -1)
                    return;
                this.cmbKanban.Text = ((DataRowView)this.cmbKanban.SelectedItem)[0].ToString();
                this.dvgDeatils.ItemsSource = new DataView(this.dt)
                {
                    RowFilter = ("PDSNo='" + this.cmbPDS.Text + "' and EkanbanNo='" + this.cmbKanban.Text + "' ")
                }.ToTable().DefaultView;
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_ELIMINATION", CommonVariable.UserID);
                string Description = ex.Message.ToString();
                CustomMessageBox.CustomStriing customStriing = CustomMessageBox.CustomStriing.Error;
                string ErrorType = customStriing.ToString();
                customStriing = CustomMessageBox.CustomStriing.OK;
                string Result = customStriing.ToString();
                CommonMethods.MessageBoxShow(Description, ErrorType, Result);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.C) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.C))
                this.btnClear_Click(sender, (RoutedEventArgs)e);
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.E) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.E) || Keyboard.IsKeyDown(Key.Escape) && Keyboard.IsKeyDown(Key.Escape))
                this.btnExit_Click(sender, (RoutedEventArgs)e);
            if ((!Keyboard.IsKeyDown(Key.LeftAlt) || !Keyboard.IsKeyDown(Key.D)) && (!Keyboard.IsKeyDown(Key.RightAlt) || !Keyboard.IsKeyDown(Key.D)))
                return;
            this.btnDelete_Click(sender, (RoutedEventArgs)e);
        }

    }
}
