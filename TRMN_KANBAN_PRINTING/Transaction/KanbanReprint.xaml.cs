using PQScan.BarcodeCreator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
using TRMN_KANBAN_PRINTING.Reports.Reports;
using TRMN_KANBAN_PRINTING.StartUp;

namespace TRMN_KANBAN_PRINTING.Transaction
{
    /// <summary>
    /// Interaction logic for KanbanReprint.xaml
    /// </summary>
    public partial class KanbanReprint : Window
    {
        public KanbanReprint()
        {
            InitializeComponent();
        }
        #region Variables and Objects
        BUSINESS_LAYER.LogCreation.LogCreation obj_Log = new BUSINESS_LAYER.LogCreation.LogCreation();
        BUSINESS_LAYER.Transaction.Transaction obj_Transaction = new BUSINESS_LAYER.Transaction.Transaction();
        DataTable dt = new DataTable();
        DataTable dtKanabn = new DataTable();
        DataTable dtReprint = new DataTable();
        string BarcodeValue,KanbanType;

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
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_RE-PRINT", CommonVariable.UserID);
                CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void Clear()
        {
            CommonMethods.UNFill_ComboBox(this.cbPDSNO);
            CommonMethods.UNFill_ComboBox(this.cboKanban);
            CommonMethods.UNFill_ComboBox(this.cboTRPartNo);
            this.cboKanban.SelectedIndex = -1;
            this.cbPDSNO.SelectedIndex = -1;
            this.cbDockNo.Text = "";
            this.txtBarcode.Text = "";
            this.cbName.SelectedIndex = -1;
            this.txtRemarks.Text = "";
            this.txtTRBarcode.Text = "";
            this.txtTRRemarks.Text = "";
            this.cbMonth.SelectedIndex = -1;
            this.cboTRPartNo.SelectedIndex = -1;
            this.cboReprintType.Text = "";
            this.cboTRPartNo.ItemsSource = null;
            this.cbMonth.ItemsSource = null;
            this.cbPDSNO.ItemsSource = null;
            this.cboKanban.ItemsSource = null;
            this.dvgDeatils.ItemsSource = null;
            this.dvgIntrDeatils.ItemsSource = null;
            this.gbCustomer.IsEnabled = false;
            this.gbInterKanban.IsEnabled = false;
            this.rdCustomer.IsChecked = new bool?(false);
            this.rdInternal.IsChecked = new bool?(false);
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Clear();
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_RE-PRINT", CommonVariable.UserID);
                string Description = ex.Message.ToString();
                CustomMessageBox.CustomStriing customStriing = CustomMessageBox.CustomStriing.Error;
                string ErrorType = customStriing.ToString();
                customStriing = CustomMessageBox.CustomStriing.OK;
                string Result = customStriing.ToString();
                CommonMethods.MessageBoxShow(Description, ErrorType, Result);
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
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_RE-PRINT", CommonVariable.UserID);
                string Description = ex.Message.ToString();
                CustomMessageBox.CustomStriing customStriing = CustomMessageBox.CustomStriing.Error;
                string ErrorType = customStriing.ToString();
                customStriing = CustomMessageBox.CustomStriing.OK;
                string Result = customStriing.ToString();
                CommonMethods.MessageBoxShow(Description, ErrorType, Result);
            }
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!this.Controlvalidation())
                    return;
                this.dtReprint = new DataTable();
                this.dtReprint.Columns.Add("SL_NO");
                this.dtReprint.Columns.Add("BARCODEVALUE");
                if ((this.dvgDeatils.SelectedItems.Count == 1 || this.dvgDeatils.Items.Count == 0) && (this.dvgIntrDeatils.SelectedItems.Count == 1 || this.dvgIntrDeatils.Items.Count == 0))
                {
                    if (this.KanbanType == "TKM Kanban")
                    {
                        ENTITY_LAYER.Transaction.Transaction.Remarks = this.txtRemarks.Text;
                        if (this.cboReprintType.Text == "KANBAN")
                        {
                            this.BarcodeValue = this.txtBarcode.Text;
                            this.dtReprint.Rows.Add((object)1, (object)this.BarcodeValue);
                            this.Transaction("Save");
                        }
                    }
                    else if (this.KanbanType == "TRMN Kanban")
                    {
                        ENTITY_LAYER.Transaction.Transaction.Remarks = this.txtTRRemarks.Text;
                        this.BarcodeValue = this.txtTRBarcode.Text;
                        this.dtReprint.Rows.Add((object)1, (object)this.BarcodeValue);
                        this.Transaction("Save");
                    }
                }
                else
                {
                    if (this.KanbanType == "TKM Kanban")
                    {
                        ENTITY_LAYER.Transaction.Transaction.Remarks = this.txtRemarks.Text;
                        if (this.cboReprintType.Text == "KANBAN")
                        {
                            for (int index = 0; index < this.dvgDeatils.SelectedItems.Count; ++index)
                            {
                                this.BarcodeValue = ((DataRowView)this.dvgDeatils.SelectedItems[index])["BARCODEVALUE"].ToString();
                                this.dtReprint.Rows.Add((object)(this.dt.Rows.Count + 1), (object)this.BarcodeValue);
                            }
                            this.Transaction("Save");
                        }
                    }
                   
                    if (this.KanbanType == "TRMN Kanban")
                    {
                        ENTITY_LAYER.Transaction.Transaction.Remarks = this.txtTRRemarks.Text;
                        for (int index = 0; index < this.dvgIntrDeatils.SelectedItems.Count; ++index)
                        {
                            this.BarcodeValue = ((DataRowView)this.dvgIntrDeatils.SelectedItems[index])["BARCODEVALUE"].ToString();
                            this.dtReprint.Rows.Add((object)(this.dt.Rows.Count + 1), (object)this.BarcodeValue);
                        }
                        this.Transaction("Save");
                    }
                }
                if (this.cboReprintType.Text == "SKID")
                    this.Transaction("SkidNo");
                this.Clear();
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_RE-PRINT", CommonVariable.UserID);
                string Description = ex.Message.ToString();
                CustomMessageBox.CustomStriing customStriing = CustomMessageBox.CustomStriing.Error;
                string ErrorType = customStriing.ToString();
                customStriing = CustomMessageBox.CustomStriing.OK;
                string Result = customStriing.ToString();
                CommonMethods.MessageBoxShow(Description, ErrorType, Result);
            }
        }

        private bool Controlvalidation()
        {
            if (this.KanbanType == "TRMN Kanban")
            {
                if ((this.dvgIntrDeatils.SelectedItems.Count == 1 || this.dvgIntrDeatils.SelectedItems.Count == 0) && this.txtTRBarcode.Text == "")
                {
                    CommonMethods.MessageBoxShow("PLEASE SCAN OR ENTER BARCODE VALUE", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    this.txtTRBarcode.Focus();
                    return false;
                }
                if (this.txtTRRemarks.Text == "")
                {
                    CommonMethods.MessageBoxShow("PLEASE ENTER REMARKS", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    this.txtTRRemarks.Focus();
                    return false;
                }
            }
            if (this.KanbanType == "TKM Kanban")
            {
                if (this.cboReprintType.SelectedIndex == -1)
                {
                    CommonMethods.MessageBoxShow("PLEASE SELECT REPRINT TYPE", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    this.cboReprintType.Focus();
                    return false;
                }
                if ((this.dvgDeatils.SelectedItems.Count == 1 || this.dvgDeatils.SelectedItems.Count == 0) && this.cboReprintType.Text == "KANBAN" && this.txtBarcode.Text == "")
                {
                    CommonMethods.MessageBoxShow("PLEASE SCAN OR ENTER BARCODE VALUE", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    this.txtBarcode.Focus();
                    return false;
                }
                if (this.txtRemarks.Text == "")
                {
                    CommonMethods.MessageBoxShow("PLEASE ENTER REMARKS", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    this.txtRemarks.Focus();
                    return false;
                }
            }
            return true;
        }

        private void Transaction(string Type)
        {
            if (Type == "ColumnName")
            {
                ENTITY_LAYER.Transaction.Transaction.Type = Type;
                ENTITY_LAYER.Transaction.Transaction.KanbanType = this.KanbanType;
                ENTITY_LAYER.Transaction.Transaction.ColumnName = Convert.ToString(this.cbName.Text);
                ENTITY_LAYER.Transaction.Transaction.Dt = (DataTable)null;
                this.dt = this.obj_Transaction.BL_RePrintDetails().Tables[0];
                CommonMethods.FillComboBox(this.cbPDSNO, this.dt, this.cbName.Text);
            }
            if (Type == "GetkanbanNo")
            {
                ENTITY_LAYER.Transaction.Transaction.Type = Type;
                ENTITY_LAYER.Transaction.Transaction.ColumnName = Convert.ToString(this.cbName.Text);
                ENTITY_LAYER.Transaction.Transaction.ColumnValue = Convert.ToString(this.cbPDSNO.Text);
                ENTITY_LAYER.Transaction.Transaction.Dt = (DataTable)null;
                if (this.KanbanType == "TKM Kanban")
                {
                    ENTITY_LAYER.Transaction.Transaction.KanbanType = this.KanbanType;
                    this.dtKanabn = this.obj_Transaction.BL_RePrintDetails().Tables[0];
                    CommonMethods.FillComboBox(this.cboKanban, this.dtKanabn, "EkanbanNo");
                }
                if (this.KanbanType == "TRMN Kanban")
                {
                    ENTITY_LAYER.Transaction.Transaction.KanbanType = this.KanbanType;
                    this.dtKanabn = this.obj_Transaction.BL_RePrintDetails().Tables[0];
                    CommonMethods.FillComboBox(this.cbMonth, this.dtKanabn.DefaultView.ToTable(true, "Kan_Month"), "Kan_Month");
                }
            }
            if (Type == "GetSerialNo")
            {
                ENTITY_LAYER.Transaction.Transaction.Type = Type;
                ENTITY_LAYER.Transaction.Transaction.EkanbanNo = this.cboKanban.Text;
                ENTITY_LAYER.Transaction.Transaction.ColumnName = Convert.ToString(this.cbName.Text);
                ENTITY_LAYER.Transaction.Transaction.ColumnValue = Convert.ToString(this.cbPDSNO.Text);
                ENTITY_LAYER.Transaction.Transaction.Dt = (DataTable)null;
                if (this.KanbanType == "TKM Kanban")
                {
                    ENTITY_LAYER.Transaction.Transaction.KanbanType = this.KanbanType;
                    ENTITY_LAYER.Transaction.Transaction.ColumnName = Convert.ToString(this.cbName.Text);
                    ENTITY_LAYER.Transaction.Transaction.ColumnValue = Convert.ToString(this.cbPDSNO.Text);
                    this.dt = this.obj_Transaction.BL_RePrintDetails().Tables[0];
                    this.dvgDeatils.ItemsSource = this.dt.DefaultView;
                }
                if (this.KanbanType == "TRMN Kanban")
                {
                    ENTITY_LAYER.Transaction.Transaction.KanbanType = this.KanbanType;
                    ENTITY_LAYER.Transaction.Transaction.ColumnName = Convert.ToString(this.cbMonth.Text);
                    ENTITY_LAYER.Transaction.Transaction.ColumnValue = Convert.ToString(this.cboTRPartNo.Text);
                    this.dt = this.obj_Transaction.BL_RePrintDetails().Tables[0];
                    this.dvgIntrDeatils.ItemsSource = this.dt.DefaultView;
                }
            }
            if (Type == "Save")
            {
                ENTITY_LAYER.Transaction.Transaction.Type = Type;
                ENTITY_LAYER.Transaction.Transaction.ColumnName = Convert.ToString(this.cbName.Text);
                ENTITY_LAYER.Transaction.Transaction.ColumnValue = Convert.ToString(this.cbPDSNO.Text);
                ENTITY_LAYER.Transaction.Transaction.EkanbanNo = this.cboKanban.Text;
                ENTITY_LAYER.Transaction.Transaction.Barcodevalue = this.BarcodeValue;
                ENTITY_LAYER.Transaction.Transaction.KanbanType = this.KanbanType;
                ENTITY_LAYER.Transaction.Transaction.Dt = this.dtReprint;
                this.dt = this.obj_Transaction.BL_RePrintDetails().Tables[0];
                if (this.KanbanType == "TKM Kanban")
                {
                    for (int index = 0; index < this.dt.Rows.Count; ++index)
                        this.dt.Rows[index]["PARTNO"] = (object)this.dt.Rows[index]["PARTNO"].ToString().Insert(5, "-").ToString().Insert(11, "-");

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["BARCODEVALUE"].ToString() != "")
                        {
                            Barcode barcode1 = new Barcode();
                            barcode1.Data = dt.Rows[i]["BARCODEVALUE"].ToString();
                            barcode1.BarType = BarCodeType.QRCode;
                            barcode1.QRCodeECL = ErrorCorrectionLevelMode.L;
                            barcode1.Width = 1000;
                            barcode1.Height = 1000;
                            barcode1.BackgroundColor = System.Drawing.Color.White;
                            Bitmap barcode3 = barcode1.CreateBarcode();
                            MemoryStream memoryStream1 = new MemoryStream();
                            barcode3.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            barcode3.Save((Stream)memoryStream1, ImageFormat.Jpeg);
                            byte[] array1 = memoryStream1.ToArray();
                            memoryStream1.Close();
                            memoryStream1.Dispose();
                            dt.Rows[i]["BARCODEIMAGE"] = array1;
                        }

                    }
                    ReportViewer reportViewer = new ReportViewer();
                    ReportViewer.dtReport = this.dt;
                    ReportViewer.ReportName = "TKMBARCODE";
                    reportViewer.ShowDialog();
                }
                else if (this.KanbanType == "TRMN Kanban")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Barcode barcode1 = new Barcode();
                        Barcode barcode2 = new Barcode();
                        barcode1.Data = dt.Rows[i]["BarcodeValue"].ToString();
                        barcode1.BarType = BarCodeType.QRCode;
                        barcode1.QRCodeECL = ErrorCorrectionLevelMode.L;
                        barcode1.Width = 1000;
                        barcode1.Height = 1000;
                        barcode1.BackgroundColor = System.Drawing.Color.White;
                        Bitmap barcode3 = barcode1.CreateBarcode();
                        MemoryStream memoryStream1 = new MemoryStream();
                        barcode3.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        barcode3.Save((Stream)memoryStream1, ImageFormat.Jpeg);
                        byte[] array1 = memoryStream1.ToArray();
                        memoryStream1.Close();
                        memoryStream1.Dispose();

                        barcode2.Data = dt.Rows[i]["OneDBarcode"].ToString();
                        barcode2.BarType = BarCodeType.Code39;
                        barcode2.QRCodeECL = ErrorCorrectionLevelMode.L;
                        barcode2.Width = 2000;
                        barcode2.Height = 1000;
                        barcode2.BackgroundColor = System.Drawing.Color.White;
                        Bitmap barcode4 = barcode2.CreateBarcode();
                        MemoryStream memoryStream2 = new MemoryStream();
                        barcode4.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        barcode4.Save((Stream)memoryStream2, ImageFormat.Jpeg);
                        byte[] array2 = memoryStream2.ToArray();
                        memoryStream2.Close();
                        memoryStream2.Dispose();

                        dt.Rows[i]["BARCODEIMAGE"] = array1;
                        dt.Rows[i]["ONEDBARCODEIMAGE"] = array2;

                    }
                    ReportViewer reportViewer = new ReportViewer();
                    ReportViewer.dtReport = this.dt;
                    ReportViewer.ReportName = "TRMNBARCODE";
                    reportViewer.ShowDialog();
                }
                CommonMethods.MessageBoxShow("KANBAN RE-PRINTED SUCCESSFULLY", CustomMessageBox.CustomStriing.Successfull.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
            if (!(Type == "SkidNo"))
                return;
            ENTITY_LAYER.Transaction.Transaction.Type = Type;
            ENTITY_LAYER.Transaction.Transaction.ColumnName = Convert.ToString(this.cbName.Text);
            ENTITY_LAYER.Transaction.Transaction.ColumnValue = Convert.ToString(this.cbPDSNO.Text);
            ENTITY_LAYER.Transaction.Transaction.DockCode = Convert.ToString(this.cbDockNo.Text);
            ENTITY_LAYER.Transaction.Transaction.Remarks = this.txtRemarks.Text;
            this.dt = this.obj_Transaction.BL_RePrintDetails().Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Barcode barcode1 = new Barcode();
                Barcode barcode2 = new Barcode();
                if (dt.Rows[i]["SkidBarcodeValue"].ToString() != "")
                {

                    barcode1.Data = dt.Rows[i]["SkidBarcodeValue"].ToString();
                    barcode1.BarType = BarCodeType.QRCode;
                    barcode1.QRCodeECL = ErrorCorrectionLevelMode.L;
                    barcode1.Width = 1000;
                    barcode1.Height = 1000;
                    barcode1.BackgroundColor = System.Drawing.Color.White;
                    Bitmap barcode3 = barcode1.CreateBarcode();
                    MemoryStream memoryStream1 = new MemoryStream();
                    barcode3.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    barcode3.Save((Stream)memoryStream1, ImageFormat.Jpeg);
                    byte[] array1 = memoryStream1.ToArray();
                    memoryStream1.Close();
                    memoryStream1.Dispose();
                    dt.Rows[i]["SkidBarcode"] = array1;
                }


                if (dt.Rows[i]["SkidBarcodeValue1"].ToString() != "")
                {
                    barcode2.Data = dt.Rows[i]["SkidBarcodeValue1"].ToString();
                    barcode2.BarType = BarCodeType.QRCode;
                    barcode2.QRCodeECL = ErrorCorrectionLevelMode.L;
                    barcode2.Width = 1000;
                    barcode2.Height = 1000;
                    barcode2.BackgroundColor = System.Drawing.Color.White;
                    Bitmap barcode4 = barcode2.CreateBarcode();
                    MemoryStream memoryStream2 = new MemoryStream();
                    barcode4.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    barcode4.Save((Stream)memoryStream2, ImageFormat.Jpeg);
                    byte[] array2 = memoryStream2.ToArray();
                    memoryStream2.Close();
                    memoryStream2.Dispose();
                    dt.Rows[i]["SkidBarcode1"] = array2;

                }


            }
            ReportViewer reportViewer1 = new ReportViewer();
            ReportViewer.dtReport = this.dt;
            ReportViewer.ReportName = "Skid No";
            reportViewer1.ShowDialog();
            CustomMessageBox.CustomStriing customStriing = CustomMessageBox.CustomStriing.Successfull;
            string ErrorType = customStriing.ToString();
            customStriing = CustomMessageBox.CustomStriing.OK;
            string Result = customStriing.ToString();
            CommonMethods.MessageBoxShow("SKID RE-PRINTED SUCCESSFULLY", ErrorType, Result);
        }

        private void CbName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.cbName.SelectedIndex <= -1)
                    return;
                this.cbName.Text = ((ContentControl)this.cbName.SelectedItem).Content.ToString();
                this.Transaction("ColumnName");
                this.dvgDeatils.ItemsSource = null;
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_RE-PRINT", CommonVariable.UserID);
                CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void CbPDSNO_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.cbPDSNO.SelectedIndex <= -1)
                    return;
                this.cbPDSNO.Text = ((DataRowView)this.cbPDSNO.SelectedItem)[0].ToString();
                this.dvgDeatils.ItemsSource = null;
                this.Transaction("GetkanbanNo");
                this.Transaction("GetSerialNo");
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_RE-PRINT", CommonVariable.UserID);
                string Description = ex.Message.ToString();
                CustomMessageBox.CustomStriing customStriing = CustomMessageBox.CustomStriing.Error;
                string ErrorType = customStriing.ToString();
                customStriing = CustomMessageBox.CustomStriing.OK;
                string Result = customStriing.ToString();
                CommonMethods.MessageBoxShow(Description, ErrorType, Result);
            }
        }

        private void CboKanban_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.cboKanban.SelectedIndex > -1)
                {
                    this.cboKanban.Text = ((DataRowView)this.cboKanban.SelectedItem)[0].ToString();
                    DataView dataView = new DataView(this.dt);
                    this.dt.DefaultView.RowFilter = "KANBANNO='" + this.cboKanban.Text + "'";
                }
                else
                    this.dt.DefaultView.RowFilter = "KANBANNO<>''";
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_RE-PRINT", CommonVariable.UserID);
                string Description = ex.Message.ToString();
                CustomMessageBox.CustomStriing customStriing = CustomMessageBox.CustomStriing.Error;
                string ErrorType = customStriing.ToString();
                customStriing = CustomMessageBox.CustomStriing.OK;
                string Result = customStriing.ToString();
                CommonMethods.MessageBoxShow(Description, ErrorType, Result);
            }
        }

        private void DvgDeatils_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.dvgDeatils.SelectedItems.Count == 1)
                    this.txtBarcode.Text = ((DataRowView)this.dvgDeatils.SelectedItems[0])["BARCODEVALUE"].ToString();
                else
                    this.txtBarcode.Text = "";
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_RE-PRINT", CommonVariable.UserID);
                string Description = ex.Message.ToString();
                CustomMessageBox.CustomStriing customStriing = CustomMessageBox.CustomStriing.Error;
                string ErrorType = customStriing.ToString();
                customStriing = CustomMessageBox.CustomStriing.OK;
                string Result = customStriing.ToString();
                CommonMethods.MessageBoxShow(Description, ErrorType, Result);
            }
        }

        private void RdInternal_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                bool? isChecked = this.rdInternal.IsChecked;
                bool flag = true;
                if (!(isChecked.GetValueOrDefault() == flag & isChecked.HasValue))
                    return;
                this.KanbanType = "TRMN Kanban";
                CommonMethods.UNFill_ComboBox(this.cbPDSNO);
                CommonMethods.UNFill_ComboBox(this.cboKanban);
                CommonMethods.UNFill_ComboBox(this.cboTRPartNo);
                CommonMethods.UNFill_ComboBox(this.cbMonth);
                this.gbInterKanban.IsEnabled = true;
                this.gbCustomer.IsEnabled = false;
                this.cboKanban.SelectedIndex = -1;
                this.cbPDSNO.SelectedIndex = -1;
                this.cbDockNo.Text = "";
                this.txtBarcode.Text = "";
                this.cbName.SelectedIndex = -1;
                this.txtRemarks.Text = "";
                this.txtTRBarcode.Text = "";
                this.txtTRRemarks.Text = "";
                this.cboTRPartNo.Text = "";
                this.cbMonth.Text = "";
                this.dvgDeatils.ItemsSource = null;
                this.dvgIntrDeatils.ItemsSource =null;
                this.dvgDeatils.Visibility = Visibility.Hidden;
                this.dvgIntrDeatils.Visibility = Visibility.Visible;
                this.Transaction("GetkanbanNo");
                this.cbMonth.Focus();
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_RE-PRINT", CommonVariable.UserID);
                string Description = ex.Message.ToString();
                CustomMessageBox.CustomStriing customStriing = CustomMessageBox.CustomStriing.Error;
                string ErrorType = customStriing.ToString();
                customStriing = CustomMessageBox.CustomStriing.OK;
                string Result = customStriing.ToString();
                CommonMethods.MessageBoxShow(Description, ErrorType, Result);
            }
        }

        private void CbMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.cbMonth.SelectedIndex <= -1)
                    return;
                DataView dataView = new DataView(this.dtKanabn);
                DataRowView selectedItem = (DataRowView)this.cbMonth.SelectedItem;
                this.dvgIntrDeatils.ItemsSource = null;
                this.cbMonth.Text = selectedItem[0].ToString();
                dataView.RowFilter = "Kan_Month='" + this.cbMonth.Text + "'";
                this.cboTRPartNo.SelectionChanged -= new SelectionChangedEventHandler(this.CboTRPartNo_SelectionChanged);
                CommonMethods.FillComboBox(this.cboTRPartNo, dataView.ToTable(true, "PartNo"), "PartNo");
                this.cboTRPartNo.SelectionChanged += new SelectionChangedEventHandler(this.CboTRPartNo_SelectionChanged);
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_RE-PRINT", CommonVariable.UserID);
                CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void CboTRPartNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.cboTRPartNo.SelectedIndex <= -1)
                    return;
                this.cboTRPartNo.Text = ((DataRowView)this.cboTRPartNo.SelectedItem)[0].ToString();
                this.Transaction("GetSerialNo");
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_RE-PRINT", CommonVariable.UserID);
                string Description = ex.Message.ToString();
                CustomMessageBox.CustomStriing customStriing = CustomMessageBox.CustomStriing.Error;
                string ErrorType = customStriing.ToString();
                customStriing = CustomMessageBox.CustomStriing.OK;
                string Result = customStriing.ToString();
                CommonMethods.MessageBoxShow(Description, ErrorType, Result);
            }
        }

        private void DvgIntrDeatils_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.dvgIntrDeatils.SelectedItems.Count == 1)
                    this.txtTRBarcode.Text = ((DataRowView)this.dvgIntrDeatils.SelectedItems[0])["BARCODEVALUE"].ToString();
                else
                    this.txtBarcode.Text = "";
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_RE-PRINT", CommonVariable.UserID);
                string Description = ex.Message.ToString();
                CustomMessageBox.CustomStriing customStriing = CustomMessageBox.CustomStriing.Error;
                string ErrorType = customStriing.ToString();
                customStriing = CustomMessageBox.CustomStriing.OK;
                string Result = customStriing.ToString();
                CommonMethods.MessageBoxShow(Description, ErrorType, Result);
            }
        }

        private void CboReprintType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cboReprintType.SelectedIndex <= -1)
                return;
            this.cboReprintType.Text = ((ContentControl)this.cboReprintType.SelectedItem).Content.ToString();
            this.dvgDeatils.ItemsSource = null;
        }

        private void CbDockNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.cbDockNo.SelectedIndex > -1)
                {
                    this.cbDockNo.Text = ((ContentControl)this.cbDockNo.SelectedItem).Content.ToString();
                    DataView dataView = new DataView(this.dt);
                    this.dt.DefaultView.RowFilter = "DOCKCODE='" + this.cbDockNo.Text + "'";
                }
                else
                    this.dt.DefaultView.RowFilter = "DOCKCODE<>''";
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_RE-PRINT", CommonVariable.UserID);
                CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void RdCustomer_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                bool? isChecked = this.rdCustomer.IsChecked;
                bool flag = true;
                if (!(isChecked.GetValueOrDefault() == flag & isChecked.HasValue))
                    return;
                this.KanbanType = "TKM Kanban";
                this.gbInterKanban.IsEnabled = false;
                this.gbCustomer.IsEnabled = true;
                CommonMethods.UNFill_ComboBox(this.cbPDSNO);
                CommonMethods.UNFill_ComboBox(this.cboKanban);
                CommonMethods.UNFill_ComboBox(this.cboTRPartNo);
                CommonMethods.UNFill_ComboBox(this.cbMonth);
                this.cbDockNo.Text = "";
                this.cboKanban.SelectedIndex = -1;
                this.cbPDSNO.SelectedIndex = -1;
                this.txtBarcode.Text = "";
                this.cbName.SelectedIndex = -1;
                this.txtRemarks.Text = "";
                this.txtTRBarcode.Text = "";
                this.txtTRRemarks.Text = "";
                this.cboTRPartNo.Text = "";
                this.cbMonth.Text = "";
                this.dvgDeatils.ItemsSource = null;
                this.dvgIntrDeatils.ItemsSource = null;
                this.dvgDeatils.Visibility = Visibility.Visible;
                this.dvgIntrDeatils.Visibility = Visibility.Hidden;
                this.cboReprintType.Focus();
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_RE-PRINT", CommonVariable.UserID);
                string Description = ex.Message.ToString();
                CustomMessageBox.CustomStriing customStriing = CustomMessageBox.CustomStriing.Error;
                string ErrorType = customStriing.ToString();
                customStriing = CustomMessageBox.CustomStriing.OK;
                string Result = customStriing.ToString();
                CommonMethods.MessageBoxShow(Description, ErrorType, Result);
            }
        }
    }
}
