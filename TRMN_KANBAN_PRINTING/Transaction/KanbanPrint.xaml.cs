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
using System.IO;
using TRMN_KANBAN_PRINTING.StartUp;
using Microsoft.Win32;
using System.Data.OleDb;
using System.Data;
using QRCoder;
using System.Drawing;
using ThoughtWorks.QRCode.Codec;
using System.Drawing.Imaging;
using System.Globalization;
using PQScan.BarcodeCreator;
using TRMN_KANBAN_PRINTING.CommonClasses;
using TRMN_KANBAN_PRINTING.Reports.Reports;
using System.Windows.Threading;

namespace TRMN_KANBAN_PRINTING.Transaction
{
    /// <summary>
    /// Interaction logic for KanbanPrint.xaml
    /// </summary>
    public partial class KanbanPrint : Window
    {
        public KanbanPrint()
        {
            InitializeComponent();
        }
        #region Variables and Objects
        BUSINESS_LAYER.LogCreation.LogCreation obj_Log = new BUSINESS_LAYER.LogCreation.LogCreation();
        BUSINESS_LAYER.Transaction.Transaction obj_Transaction = new BUSINESS_LAYER.Transaction.Transaction();
        BUSINESS_LAYER.Reports.Reports obj_Report = new BUSINESS_LAYER.Reports.Reports();
        DataTable dt = new DataTable();
        string OrderNo = "", OldOrder = "", Location = "", OldLocation = "", SeqenceNo = "";
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
                this.gbMultiple.IsEnabled = false;
                this.gbSingle.IsEnabled = false;
                this.txtTemplate.Visibility = Visibility.Hidden;
                this.SizeToContent = SizeToContent.Manual;
                this.Transaction("LoadDropDown");
                this.ShowDateTime();
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_PRINT", CommonVariable.UserID);
                CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void Transaction(string Type)
        {
            CustomMessageBox.CustomStriing customStriing;
            if (Type == "Save")
            {
                if (this.cmbkanbanType.Text == "TRMN Kanban")
                    ENTITY_LAYER.Transaction.Transaction.Dt = this.dt;
                else if (this.cmbkanbanType.Text == "TKM Kanban")
                    ENTITY_LAYER.Transaction.Transaction.TKMdt = this.dt;
                ENTITY_LAYER.Transaction.Transaction.Type = Type;
                ENTITY_LAYER.Transaction.Transaction.KanbanType = this.cmbkanbanType.Text;
                CommonVariable.Result = this.obj_Transaction.BL_KanbanPrintTransaction();
                if (CommonVariable.Result.Contains("Saved"))
                {
                    this.txtBatch.Text = "Last Batch No=" + CommonVariable.Result.Split('~')[1];
                    customStriing = CustomMessageBox.CustomStriing.Question;
                    string ErrorType = customStriing.ToString();
                    customStriing = CustomMessageBox.CustomStriing.YESNO;
                    string Result = customStriing.ToString();
                    CommonMethods.MessageBoxShow("DO YOU WANT TO PRINT ALL KANBANS?", ErrorType, Result);
                    if (CommonVariable.Result == "YES")
                    {
                        this.Transaction("GetprintedDetails");
                        this.Transaction("Print");
                    }
                }
                else if (CommonVariable.Result == "Duplicate")
                {
                    string duplicate = CommonVariable.Duplicate;
                    customStriing = CustomMessageBox.CustomStriing.Information;
                    string ErrorType = customStriing.ToString();
                    customStriing = CustomMessageBox.CustomStriing.OK;
                    string Result = customStriing.ToString();
                    CommonMethods.MessageBoxShow(duplicate, ErrorType, Result);
                }
                else
                {
                    string result = CommonVariable.Result;
                    string ErrorType = CustomMessageBox.CustomStriing.Information.ToString();
                    customStriing = CustomMessageBox.CustomStriing.OK;
                    string Result = customStriing.ToString();
                    CommonMethods.MessageBoxShow(result, ErrorType, Result);
                }
            }
            if (Type == "Print")
            {
                bool flag = false;
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("SLNO");
                dataTable.Columns.Add("PARTNO");
                dataTable.Columns.Add("KANBANNO");
                dataTable.Columns.Add("BARCODEVALUE");
                dataTable.Columns.Add("SERIALNO");
                dataTable.Columns.Add("QTY");
                dataTable.Columns.Add("KAN_MONTH");
                dataTable.Columns.Add("PDSNO");
                //dataTable.Columns.Add("BARCODEIMAGE");
                //dataTable.Columns["BARCODEIMAGE"].DataType = typeof(byte[]);
                //dataTable.Columns.Add("ONEDBARCODEIMAGE");
                //dataTable.Columns["ONEDBARCODEIMAGE"].DataType = typeof(byte[]);
                dataTable.Columns.Add("ONEDBARCODE");
                if (this.cmbkanbanType.Text == "TRMN Kanban")
                {
                    ENTITY_LAYER.Transaction.Transaction.KanbanType = this.cmbkanbanType.Text;
                    ENTITY_LAYER.Transaction.Transaction.Type = Type;
                    ENTITY_LAYER.Transaction.Transaction.BatchNo = this.txtBatch.Text.Split('=')[1];
                    for (int index1 = 0; index1 < this.dvgDeatilsForTRMN.Items.Count; ++index1)
                    {
                        DataRowView dataRowView = (DataRowView)this.dvgDeatilsForTRMN.Items[index1];
                        if (dataRowView["Label_Type"].ToString().StartsWith("T"))
                            ENTITY_LAYER.Transaction.Transaction.Barcodevalue = dataRowView["KANBAN_NO"].ToString().Replace(" ", "") + "|" + dataRowView["TR_PART_NUMBER"] + "|" + dataRowView["PART_NAME"] + "|" + dataRowView["LINE_NO"] + "|" + dataRowView["LOCATION_NAME"].ToString().Replace(" ", "") + "|" + dataRowView["ID_CODE"] + "|" + dataRowView["BIN_NO"] + "|" + dataRowView["BOX_TYPE"] + "|" + dataRowView["KANBAN_LOC_PROCESS_LINE_NO"].ToString().Replace(" ", "") + "|" + dataRowView["SUPPLIER_CODE_NAME"].ToString().Replace(" ", "") + "|TEMP";
                        else
                            ENTITY_LAYER.Transaction.Transaction.Barcodevalue = dataRowView["KANBAN_NO"].ToString().Replace(" ", "") + "|" + dataRowView["TR_PART_NUMBER"] + "|" + dataRowView["PART_NAME"] + "|" + dataRowView["LINE_NO"] + "|" + dataRowView["LOCATION_NAME"].ToString().Replace(" ", "") + "|" + dataRowView["ID_CODE"] + "|" + dataRowView["BIN_NO"] + "|" + dataRowView["BOX_TYPE"] + "|" + dataRowView["KANBAN_LOC_PROCESS_LINE_NO"].ToString().Replace(" ", "") + "|" + dataRowView["SUPPLIER_CODE_NAME"].ToString().Replace(" ", "") + "|PERM";
                        int int32_1 = Convert.ToInt32(dataRowView["TOTALPRINTEDQTY"]);
                        int num1 = 0;
                        int int32_2 = Convert.ToInt32(dataRowView["TOTAL_QTY"]);
                        for (int index2 = int32_1; index2 < int32_1 + Convert.ToInt32(dataRowView["REMAINING_QTY"]); ++index2)
                        {
                            DateTime dateTime = DateTime.Today;
                            int num2 = dateTime.Year;
                            string str1 = num2.ToString();
                            dateTime = DateTime.ParseExact(this.cmbMonth.Text, "MMMM", (IFormatProvider)CultureInfo.InvariantCulture);
                            num2 = dateTime.Month;
                            string str2 = num2.ToString().PadLeft(2, '0');
                            num2 = index2 + 1;
                            string str3 = num2.ToString().PadLeft(4, '0');
                            string str4 = str1 + str2 + str3;
                            num1 = (int)Convert.ToInt16(dataRowView["BOX_QTY"]) >= int32_2 ? int32_2 : (int)Convert.ToInt16(dataRowView["BOX_QTY"]);
                            string str5 = ENTITY_LAYER.Transaction.Transaction.Barcodevalue + "|" + (object)num1 + "|" + str4;
                            //Barcode barcode1 = new Barcode();
                            //Barcode barcode2 = new Barcode();
                            //barcode1.Data = str5;
                            //barcode1.BarType = BarCodeType.QRCode;
                            //barcode1.QRCodeECL = ErrorCorrectionLevelMode.L;
                            //barcode1.Width = 1000;
                            //barcode1.Height = 1000;
                            //barcode1.BackgroundColor = System.Drawing.Color.White;
                            //Bitmap barcode3 = barcode1.CreateBarcode();
                            //MemoryStream memoryStream1 = new MemoryStream();
                            //barcode3.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            //barcode3.Save((Stream)memoryStream1, ImageFormat.Jpeg);
                            //byte[] array1 = memoryStream1.ToArray();
                            //memoryStream1.Close();
                            //memoryStream1.Dispose();
                            string str6 = num1.ToString().Length != 1 ? num1.ToString() : "0" + (object)num1;
                            str6 = dataRowView["TR_PART_NUMBER"].ToString().Replace("-", "").PadRight(10, '0') + str6;

                            //barcode2.Data = dataRowView["TR_PART_NUMBER"].ToString().Replace("-", "").PadRight(10, '0') + str6;
                            //barcode2.BarType = BarCodeType.Code39;
                            //barcode2.QRCodeECL = ErrorCorrectionLevelMode.L;
                            //barcode2.Width = 2000;
                            //barcode2.Height = 1000;
                            //barcode2.BackgroundColor = System.Drawing.Color.White;
                            //Bitmap barcode4 = barcode2.CreateBarcode();
                            //MemoryStream memoryStream2 = new MemoryStream();
                            //barcode4.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            //barcode4.Save((Stream)memoryStream2, ImageFormat.Jpeg);
                            //byte[] array2 = memoryStream2.ToArray();
                            //memoryStream2.Close();
                            //memoryStream2.Dispose();
                            DataRowCollection rows = dataTable.Rows;
                            object[] objArray = new object[9]
                            {
                                (object) (dataTable.Rows.Count + 1),
                                dataRowView["TR_PART_NUMBER"],
                                dataRowView["KANBAN_NO"],
                                (object) (ENTITY_LAYER.Transaction.Transaction.Barcodevalue + "|" + (object) num1 + "|" + str4),
                                (object) str4,
                                (object) num1,
                                null,
                                null,
                                null,
                                //null,
                                //null
                            };
                            dateTime = DateTime.Today;
                            objArray[6] = (object)(dateTime.Year.ToString() + "-" + this.cmbMonth.Text);
                            objArray[7] = (object)"";
                           // objArray[8] = (object)array1;
                            //objArray[9] = (object)array2
                            objArray[8] = (object)str6;
                            rows.Add(objArray);
                            int32_2 -= (int)Convert.ToInt16(dataRowView["BOX_QTY"]);
                        }
                    }
                    ENTITY_LAYER.Transaction.Transaction.TRMNSerialdt = dataTable;
                    DataTable table = this.obj_Transaction.BL_KanbanDPrintDetails().Tables[0];
                    if (table.Rows.Count > 0)
                    {
                        if (table.Columns.Count == 1)
                        {
                            string Description = table.Rows[0][0].ToString();
                            customStriing = CustomMessageBox.CustomStriing.Error;
                            string ErrorType = customStriing.ToString();
                            customStriing = CustomMessageBox.CustomStriing.OK;
                            string Result = customStriing.ToString();
                            CommonMethods.MessageBoxShow(Description, ErrorType, Result);
                            this.Clear();
                            return;
                        }

                        //table.Columns.Add("BARCODEIMAGE");
                        //table.Columns["BARCODEIMAGE"].DataType = typeof(byte[]);
                        //table.Columns.Add("ONEDBARCODEIMAGE");
                        //table.Columns["ONEDBARCODEIMAGE"].DataType = typeof(byte[]);

                       // DataRow rows = table.Rows;

                        for (int i=0;i<table.Rows.Count;i++)
                        {
                            Barcode barcode1 = new Barcode();
                            Barcode barcode2 = new Barcode();
                            barcode1.Data = table.Rows[i]["BarcodeValue"].ToString();
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

                            barcode2.Data = table.Rows[i]["OneDBarcode"].ToString();
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

                            table.Rows[i]["BARCODEIMAGE"] = array1;
                            table.Rows[i]["ONEDBARCODEIMAGE"] = array2;

                        }
                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewer.dtReport = table;
                        ReportViewer.ReportName = "TRMNBARCODE";
                        reportViewer.ShowDialog();
                        DataGrid dvgDeatilsForTrmn = this.dvgDeatilsForTRMN;
                        dvgDeatilsForTrmn.ItemsSource = table.DefaultView;
                        for (int index = 0; index < table.Rows.Count; ++index)
                        {
                            this.PrintKanban((DataRowView)dvgDeatilsForTrmn.Items[index], table.Rows[index]["BarcodeValue"].ToString(), table.Rows[index]["Qty"].ToString());
                            flag = true;
                        }
                        customStriing = CustomMessageBox.CustomStriing.Successfull;
                        string ErrorType1 = customStriing.ToString();
                        customStriing = CustomMessageBox.CustomStriing.OK;
                        string Result1 = customStriing.ToString();
                        CommonMethods.MessageBoxShow("KANBAN PRINTED SUCCESSFULLY", ErrorType1, Result1);
                        this.Clear();
                    }
                    else
                    {
                        customStriing = CustomMessageBox.CustomStriing.Information;
                        string ErrorType = customStriing.ToString();
                        customStriing = CustomMessageBox.CustomStriing.OK;
                        string Result = customStriing.ToString();
                        CommonMethods.MessageBoxShow("DATA NOT FOUND TO PRINT", ErrorType, Result);
                    }
                }
                else if (this.cmbkanbanType.Text == "TKM Kanban")
                {
                    string str7 = "";
                    int num = 0;
                    for (int index = 0; index < this.dvgDeatilsforTKM.Items.Count; ++index)
                    {
                        DataRowView dataRowView = (DataRowView)this.dvgDeatilsforTKM.Items[index];
                        ENTITY_LAYER.Transaction.Transaction.EkanbanNo = dataRowView["KANBANNO"].ToString();
                        ENTITY_LAYER.Transaction.Transaction.PartNo = dataRowView["PARTNO"].ToString();
                        ENTITY_LAYER.Transaction.Transaction.KanbanType = this.cmbkanbanType.Text;
                        ENTITY_LAYER.Transaction.Transaction.Type = Type;
                        ENTITY_LAYER.Transaction.Transaction.BatchNo = this.txtBatch.Text.Split('=')[1];
                        ENTITY_LAYER.Transaction.Transaction.Nooflabels = dataRowView["TOTALABEL"].ToString();
                        for (int int32 = Convert.ToInt32(dataRowView["PRINTEDQTY"]); int32 < Convert.ToInt32(dataRowView["REMAININGQTY"]); ++int32)
                        {
                            string str8 = "";
                            if (str7 == "")
                            {
                                str7 = dataRowView["KANBANNO"].ToString();
                                ++num;
                                this.SeqenceNo = num.ToString() + "/" + ENTITY_LAYER.Transaction.Transaction.Nooflabels;
                                str8 = num.ToString().PadLeft(5, '0') + "/" + ENTITY_LAYER.Transaction.Transaction.Nooflabels.PadLeft(5, '0');
                            }
                            else if (str7.ToString() == dataRowView["KANBANNO"].ToString())
                            {
                                ++num;
                                str7 = dataRowView["KANBANNO"].ToString();
                                this.SeqenceNo = num.ToString() + "/" + ENTITY_LAYER.Transaction.Transaction.Nooflabels;
                                str8 = num.ToString().PadLeft(5, '0') + "/" + ENTITY_LAYER.Transaction.Transaction.Nooflabels.PadLeft(5, '0');
                            }
                            else if (str7.ToString() != dataRowView["KANBANNO"].ToString())
                            {
                                str7 = dataRowView["KANBANNO"].ToString();
                                num = 1;
                                this.SeqenceNo = num.ToString() + "/" + ENTITY_LAYER.Transaction.Transaction.Nooflabels;
                                str8 = num.ToString().PadLeft(5, '0') + "/" + ENTITY_LAYER.Transaction.Transaction.Nooflabels.PadLeft(5, '0');
                            }
                            ENTITY_LAYER.Transaction.Transaction.Barcodevalue = dataRowView["PDSNO"].ToString() + " " + dataRowView["PARTNO"].ToString().Insert(5, "-").ToString().Insert(11, "-") + " " + str8 + "," + dataRowView["SUPPLIERCODE"].ToString().Replace("-", "") + "," + dataRowView["SUPPLIERCODE"].ToString().Split('-')[1] + "," + dataRowView["LOCATION_NAME"].ToString().Replace(" ", "") + "," + dataRowView["ORDERNO"] + "," + dataRowView["KANBANNO"] + "," + dataRowView["BINQTY"] + "," + dataRowView["DOCKCODE"] + "," + dataRowView["PLANENO"] + "," + dataRowView["CONVEYANCENO"] + "," + dataRowView["STOREADDRESS"];
                            string barcodevalue = ENTITY_LAYER.Transaction.Transaction.Barcodevalue;
                            //Bitmap barcode = new Barcode()
                            //{
                            //    Data = barcodevalue,
                            //    BarType = BarCodeType.QRCode,
                            //    QRCodeECL = ErrorCorrectionLevelMode.L,
                            //    Width = 1000,
                            //    Height = 1000,
                            //    BackgroundColor = System.Drawing.Color.White
                            //}.CreateBarcode();
                            //MemoryStream memoryStream = new MemoryStream();
                            //barcode.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            //barcode.Save((Stream)memoryStream, ImageFormat.Jpeg);
                            //byte[] array = memoryStream.ToArray();
                            //memoryStream.Close();
                            //memoryStream.Dispose();
                            dataTable.Rows.Add((object)(dataTable.Rows.Count + 1), dataRowView["PARTNO"], dataRowView["KANBANNO"], (object)ENTITY_LAYER.Transaction.Transaction.Barcodevalue, (object)this.SeqenceNo, dataRowView["BINQTY"], (object)"", dataRowView["PDSNO"],"");
                        }
                    }
                    ENTITY_LAYER.Transaction.Transaction.TRMNSerialdt = dataTable;
                    DataTable table = this.obj_Transaction.BL_KanbanDPrintDetails().Tables[0];
                    if (table.Rows.Count > 0)
                    {
                        if (table.Columns.Count == 1)
                        {
                            string Description = table.Rows[0][0].ToString();
                            customStriing = CustomMessageBox.CustomStriing.Error;
                            string ErrorType = customStriing.ToString();
                            customStriing = CustomMessageBox.CustomStriing.OK;
                            string Result = customStriing.ToString();
                            CommonMethods.MessageBoxShow(Description, ErrorType, Result);
                            this.Clear();
                            return;
                        }
                        this.dvgDeatilsForTRMN.ItemsSource = table.DefaultView;
                        for (int index = 0; index < table.Rows.Count; ++index)
                        {
                            table.Rows[index]["PARTNO"] = (object)table.Rows[index]["PARTNO"].ToString().Insert(5, "-").ToString().Insert(11, "-");
                            if (this.Location != "" && this.Location != table.Rows[index]["LOCATION_TYPE"].ToString())
                            {
                                DataRow row = table.NewRow();
                                row["LOCATION_TYPE"] = !(this.Location == "INHOUSE") ? (object)(this.Location + " IS") : (object)"ORDER IS";
                                row["ORDERNO"] = (object)(this.OrderNo + " " + table.Rows[index]["DOCKCODE"]);
                                table.Rows.InsertAt(row, index);
                                table.AcceptChanges();
                                ++index;
                            }
                            this.OrderNo = table.Rows[index]["ORDERNO"].ToString();
                            this.Location = table.Rows[index]["LOCATION_TYPE"].ToString();
                        }

                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            if (table.Rows[i]["BARCODEVALUE"].ToString() != "")
                            {
                                Barcode barcode1 = new Barcode();
                                barcode1.Data = table.Rows[i]["BARCODEVALUE"].ToString();
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
                                table.Rows[i]["BARCODEIMAGE"] = array1;
                            }

                        }

                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewer.dtReport = table;
                        ReportViewer.ReportName = "TKMBARCODE";
                        reportViewer.ShowDialog();
                        customStriing = CustomMessageBox.CustomStriing.Successfull;
                        string ErrorType2 = customStriing.ToString();
                        customStriing = CustomMessageBox.CustomStriing.OK;
                        string Result2 = customStriing.ToString();
                        CommonMethods.MessageBoxShow("KANBAN PRINTED SUCCESSFULLY", ErrorType2, Result2);
                        this.Transaction("SkidNo");
                        this.Clear();
                    }
                    else
                    {
                        customStriing = CustomMessageBox.CustomStriing.Information;
                        string ErrorType = customStriing.ToString();
                        customStriing = CustomMessageBox.CustomStriing.OK;
                        string Result = customStriing.ToString();
                        CommonMethods.MessageBoxShow("NO LABELS TO PRINT", ErrorType, Result);
                    }
                }
            }
            if (Type == "GetPartDetails")
            {
                ENTITY_LAYER.Transaction.Transaction.Type = Type;
                ENTITY_LAYER.Transaction.Transaction.KanbanType = this.cmbkanbanType.Text;
                if (this.cmbkanbanType.Text == "TRMN Kanban")
                {
                    ENTITY_LAYER.Transaction.Transaction.Dt = this.dt;
                    DataTable table = this.obj_Transaction.BL_KanbanDPrintDetails().Tables[0];
                    if (table.Columns.Count > 1)
                    {
                        this.dvgDeatilsForTRMN.ItemsSource = table.DefaultView;
                        int num = 0;
                        for (int index = 0; index < table.Rows.Count; ++index)
                            num += Convert.ToInt32(table.Rows[index]["NO_OF_LABLES"]);
                        this.lblCount.Content = (object)num.ToString();
                    }
                    else
                    {
                        string Description = table.Rows[0][0].ToString();
                        customStriing = CustomMessageBox.CustomStriing.Information;
                        string ErrorType = customStriing.ToString();
                        customStriing = CustomMessageBox.CustomStriing.OK;
                        string Result = customStriing.ToString();
                        CommonMethods.MessageBoxShow(Description, ErrorType, Result);
                    }
                }
                else if (this.cmbkanbanType.Text == "TKM Kanban")
                {
                    ENTITY_LAYER.Transaction.Transaction.TKMdt = this.dt;
                    DataTable table = this.obj_Transaction.BL_KanbanDPrintDetails().Tables[0];
                    if (table.Columns.Count > 1)
                    {
                        this.dvgDeatilsforTKM.ItemsSource = table.DefaultView;
                        int num = 0;
                        for (int index = 0; index < table.Rows.Count; ++index)
                            num += Convert.ToInt32(table.Rows[index]["NOOFLABELS"]);
                        this.lblCount.Content = (object)num.ToString();
                    }
                    else
                    {
                        string Description = table.Rows[0][0].ToString();
                        customStriing = CustomMessageBox.CustomStriing.Information;
                        string ErrorType = customStriing.ToString();
                        customStriing = CustomMessageBox.CustomStriing.OK;
                        string Result = customStriing.ToString();
                        CommonMethods.MessageBoxShow(Description, ErrorType, Result);
                    }
                }
            }
            if (Type == "LoadDropDown")
            {
                ENTITY_LAYER.Transaction.Transaction.Type = Type;
                ENTITY_LAYER.Transaction.Transaction.Dt = (DataTable)null;
                ENTITY_LAYER.Transaction.Transaction.TKMdt = (DataTable)null;
                ENTITY_LAYER.Transaction.Transaction.TRMNSerialdt = (DataTable)null;
                DataSet dataSet = this.obj_Transaction.BL_KanbanDPrintDetails();
                CommonMethods.FillComboBox(this.cmbPartNo, dataSet.Tables[0], "TR_PART_NUMBER");
                if (dataSet.Tables[1].Rows.Count > 0)
                    this.txtBatch.Text = "Last Batch No=" + dataSet.Tables[1].Rows[0]["BatchNo"].ToString();
            }
            if (Type == "SkidNo")
            {
                ENTITY_LAYER.Reports.Reports.Type = Type;
                ENTITY_LAYER.Reports.Reports.BatchNo = this.txtBatch.Text.Split('=')[1];
                DataTable table = this.obj_Report.BL_ReportDetails().Tables[0];

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    Barcode barcode1 = new Barcode();
                    Barcode barcode2 = new Barcode();
                    if (table.Rows[i]["SequenceNo"].ToString()!="")
                    {
                       
                        barcode1.Data = table.Rows[i]["SequenceNo"].ToString();
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
                        table.Rows[i]["SkidBarcode"] = array1;
                    }


                if (table.Rows[i]["SequenceNo1"].ToString() != "")
                    {
                        barcode2.Data = table.Rows[i]["SequenceNo1"].ToString();
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
                        table.Rows[i]["SkidBarcode1"] = array2;

                    }


                }
                ReportViewer reportViewer = new ReportViewer();
                ReportViewer.dtReport = table;
                ReportViewer.ReportName = "Skid No";
                reportViewer.ShowDialog();
            }
            if (!(Type == "GetprintedDetails"))
                return;
            ENTITY_LAYER.Transaction.Transaction.KanbanType = this.cmbkanbanType.Text;
            ENTITY_LAYER.Transaction.Transaction.Type = Type;
            ENTITY_LAYER.Transaction.Transaction.BatchNo = this.txtBatch.Text.Split('=')[1];
            if (this.cmbkanbanType.Text == "TRMN Kanban")
                this.dvgDeatilsForTRMN.ItemsSource = this.obj_Transaction.BL_KanbanDPrintDetails().Tables[0].DefaultView;
            if (this.cmbkanbanType.Text == "TKM Kanban")
                this.dvgDeatilsforTKM.ItemsSource = this.obj_Transaction.BL_KanbanDPrintDetails().Tables[0].DefaultView;
        }

        private void Clear()
        {
            this.dvgDeatilsforTKM.ItemsSource = null;
            this.dvgDeatilsForTRMN.ItemsSource = null;
            this.txtBrowseFile.Text = "";
            this.cmbkanbanType.Text = "";
            this.gbSingle.IsEnabled = false;
            this.gbMultiple.IsEnabled = false;
            this.rdMultiple.IsChecked = new bool?(false);
            this.rdSingle.IsChecked = new bool?(false);
            this.cmbMonth.Text = "";
            this.txtTemplate.Visibility = Visibility.Hidden;
            this.cmbPartNo.Text = "";
            this.lblCount.Content = (object)"0";
            this.txtTotalQTy.Text = "";
            this.cmbLabelType.Text = "";
            this.cmbkanbanType.Text = "";
            this.OldLocation = "";
            this.OldOrder = "";
            this.OrderNo = "";
            this.Location = "";
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.cmbkanbanType.SelectedIndex == -1)
                {
                    CommonMethods.MessageBoxShow("PLEASE SELECT KANBAN TYPE", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    this.cmbkanbanType.Focus();
                }
                else if (this.cmbkanbanType.Text == "TRMN Kanban")
                {
                    if (this.cmbMonth.SelectedIndex == -1)
                    {
                        CommonMethods.MessageBoxShow("PLEASE SELECT MONTH", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                        this.cmbMonth.Focus();
                    }
                    else
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|Excel files(*.xls)|*.xls";
                        openFileDialog.ShowDialog();
                        if (!(openFileDialog.FileName != ""))
                            return;
                        this.txtBrowseFile.Text = openFileDialog.FileName;
                        string empty = string.Empty;
                        this.dt = new DataTable();
                        using (OleDbConnection selectConnection = new OleDbConnection(!this.txtBrowseFile.Text.EndsWith(".xls") ? "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + this.txtBrowseFile.Text + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1';" : "provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + this.txtBrowseFile.Text + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"))
                            new OleDbDataAdapter("select * from  [PrintData$]", selectConnection).Fill(this.dt);
                        this.dt.Columns.Add("MONTH");
                        int count = this.dt.Rows.Count;
                        for (int index = 0; index < count; ++index)
                        {
                            if (this.dt.Rows[index]["TR_PART_NUMBER"].ToString() != "")
                                this.dt.Rows[index]["Month"] = (object)(DateTime.Now.Year.ToString() + "-" + this.cmbMonth.Text);
                        }
                        this.Transaction("GetPartDetails");
                    }
                }
                else if (this.cmbkanbanType.Text == "TKM Kanban")
                {
                    this.dt = new DataTable();
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Text files (*.txt)|*.txt";
                    openFileDialog.ShowDialog();
                    if (!(openFileDialog.FileName != ""))
                        return;
                    this.txtBrowseFile.Text = openFileDialog.FileName;
                    string empty = string.Empty;
                    this.dt.Columns.Add("SLNO");
                    this.dt.Columns.Add("SUPPLIER");
                    this.dt.Columns.Add("SUPPLIERCODE");
                    this.dt.Columns.Add("ARRIVALDATE");
                    this.dt.Columns.Add("ARRIVALTIME");
                    this.dt.Columns.Add("SUPPLIERSTOREADDRESS");
                    this.dt.Columns.Add("PACKINGCODE");
                    this.dt.Columns.Add("PARTNO");
                    this.dt.Columns.Add("PARTNAME");
                    this.dt.Columns.Add("KANBANNO");
                    this.dt.Columns.Add("BINQTY");
                    this.dt.Columns.Add("ORDERNO");
                    this.dt.Columns.Add("DOCKCODE");
                    this.dt.Columns.Add("PLANENO");
                    this.dt.Columns.Add("CONVEYANCENO");
                    this.dt.Columns.Add("KANBANORIENTATION");
                    this.dt.Columns.Add("STOREADDRESS");
                    this.dt.Columns.Add("NOOFLABELS");
                    this.dt.Columns.Add("SUPPLIERPICKUPROUT");
                    this.dt.Columns.Add("DEPATURDATE");
                    this.dt.Columns.Add("DEPATURDATE1");
                    this.dt.Columns.Add("DEPATURTIME");
                    this.dt.Columns.Add("DEPATURTIME1");
                    this.dt.Columns.Add("ARRIVALDATE1");
                    this.dt.Columns.Add("ARRIVALTIME1");
                    this.dt.Columns.Add("ROUTE1");
                    this.dt.Columns.Add("MAINPLANE");
                    this.dt.Columns.Add("SKIDNOBARCODE");
                    this.dt.Columns["SKIDNOBARCODE"].DataType = typeof(byte[]);
                    this.dt.Columns.Add("PLANT");
                    this.dt.Columns.Add("PDSNO");
                    this.dt.Columns.Add("BILLEDOUT");
                    this.dt.Columns.Add("DEPATURDATE2");
                    this.dt.Columns.Add("DEPATURTIME2");
                    string[] strArray1 = new StreamReader(this.txtBrowseFile.Text).ReadToEnd().Split('\n');
                    for (int index = 0; index < strArray1.Length; ++index)
                    {
                        if (strArray1[index] != "")
                        {
                            string[] strArray2 = strArray1[index].Split('|');
                            string str = strArray2[8] + "|" + strArray2[46] + "|" + strArray2[0];
                            byte[] barcodeInBytes = new Barcode()
                            {
                                Data = str,
                                BarType = BarCodeType.QRCode,
                                QRCodeECL = ErrorCorrectionLevelMode.L,
                                Width = 300,
                                Height = 300,
                                BackgroundColor = System.Drawing.Color.White
                            }.CreateBarcodeInBytes();
                            this.dt.Rows.Add((object)strArray2[0], (object)strArray2[3], (object)(strArray2[1] + "-" + strArray2[2]), (object)strArray2[12], (object)strArray2[13], (object)strArray2[41], (object)strArray2[63], (object)strArray2[38], (object)strArray2[39], (object)strArray2[40], (object)strArray2[42], (object)strArray2[9], (object)strArray2[6], strArray2[15] == "" ? (object)" " : (strArray2[15] == "-" ? (object)"-" : (object)strArray2[15].PadLeft(2, '0')), (object)strArray2[46], (object)strArray2[60], (object)strArray2[41], (object)strArray2[44], strArray2[16] == "" ? (object)"" : (strArray2[16] == "-" ? (object)"-" : (strArray2[16].Substring(0, 4) + "-" + strArray2[17] == "" ? (object)"" : (strArray2[17] == "-" ? (object)"-" : (object)strArray2[17].PadLeft(2, '0')))), (object)strArray2[10], (object)strArray2[22], (object)strArray2[11], (object)strArray2[23], (object)strArray2[20], (object)strArray2[21], strArray2[14] + "-" + strArray2[15] == "" ? (object)"" : (strArray2[15] == "-" ? (object)"-" : (object)strArray2[15].PadLeft(2, '0')), strArray2[14] + "-" + strArray2[15] == "" ? (object)"" : (strArray2[15] == "-" ? (object)"-" : (object)strArray2[15].PadLeft(2, '0')), (object)barcodeInBytes, (object)strArray2[4], (object)strArray2[8], (object)strArray2[57], (object)strArray2[34], (object)strArray2[35]);
                        }
                    }
                    this.Transaction("GetPartDetails");
                }
                else
                    CommonMethods.MessageBoxShow("PRINTING OPTION COMING SOON.........", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_PRINT", CommonVariable.UserID);
                string Description = ex.Message.ToString();
                CustomMessageBox.CustomStriing customStriing = CustomMessageBox.CustomStriing.Error;
                string ErrorType = customStriing.ToString();
                customStriing = CustomMessageBox.CustomStriing.OK;
                string Result = customStriing.ToString();
                CommonMethods.MessageBoxShow(Description, ErrorType, Result);
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool? isChecked = this.rdMultiple.IsChecked;
                bool flag1 = false;
                int num;
                if (isChecked.GetValueOrDefault() == flag1 & isChecked.HasValue)
                {
                    isChecked = this.rdSingle.IsChecked;
                    bool flag2 = false;
                    num = isChecked.GetValueOrDefault() == flag2 & isChecked.HasValue ? 1 : 0;
                }
                else
                    num = 0;
                if (num != 0)
                {
                    CommonMethods.MessageBoxShow("PLEASE SELECT EIGHTER SINGLE OR MULTIPALE", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                }
                else
                {
                    isChecked = this.rdMultiple.IsChecked;
                    bool flag3 = true;
                    if (isChecked.GetValueOrDefault() == flag3 & isChecked.HasValue)
                    {
                        if (this.txtBrowseFile.Text == "")
                        {
                            CommonMethods.MessageBoxShow("PLEASE BROWSE THE ANY KANBAN FILE FOR YOUR TRANSACTION", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                            this.cmbkanbanType.Focus();
                        }
                        else
                            this.Transaction("Save");
                    }
                    isChecked = this.rdSingle.IsChecked;
                    bool flag4 = true;
                    if (!(isChecked.GetValueOrDefault() == flag4 & isChecked.HasValue))
                        return;
                    if (this.dvgDeatilsForTRMN.Items.Count == 0)
                    {
                        CommonMethods.MessageBoxShow("NO DATA FOUND FOR SAVING", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                        this.cmbPartNo.Focus();
                    }
                    else
                        this.Transaction("Save");
                }
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_PRINT", CommonVariable.UserID);
                CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void CmbkanbanType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.cmbkanbanType.SelectedIndex <= -1)
                    return;
                this.cmbkanbanType.Text = ((ContentControl)this.cmbkanbanType.SelectedItem).Content.ToString();
                if (this.cmbkanbanType.Text == "TRMN Kanban")
                {
                    this.dvgDeatilsForTRMN.Visibility = Visibility.Visible;
                    this.dvgDeatilsforTKM.Visibility = Visibility.Collapsed;
                    this.txtBrowseFile.Text = "";
                    this.txtTemplate.Visibility = Visibility.Visible;
                }
                if (this.cmbkanbanType.Text == "TKM Kanban")
                {
                    this.dvgDeatilsForTRMN.Visibility = Visibility.Collapsed;
                    this.dvgDeatilsforTKM.Visibility = Visibility.Visible;
                    this.txtBrowseFile.Text = "";
                    this.txtTemplate.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_PRINT", CommonVariable.UserID);
                CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Clear();
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_PRINT", CommonVariable.UserID);
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
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_PRINT", CommonVariable.UserID);
                string Description = ex.Message.ToString();
                CustomMessageBox.CustomStriing customStriing = CustomMessageBox.CustomStriing.Error;
                string ErrorType = customStriing.ToString();
                customStriing = CustomMessageBox.CustomStriing.OK;
                string Result = customStriing.ToString();
                CommonMethods.MessageBoxShow(Description, ErrorType, Result);
            }
        }

        private void PrintKanban(DataRowView obj_DR, string obj_Barcodevalue, string Qty)
        {
        }

        private void RdSingle_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                bool? isChecked = this.rdSingle.IsChecked;
                bool flag = true;
                if (!(isChecked.GetValueOrDefault() == flag & isChecked.HasValue))
                    return;
                this.cmbkanbanType.Text = "TRMN Kanban";
                this.gbSingle.IsEnabled = true;
                this.gbMultiple.IsEnabled = false;
                this.txtBrowseFile.Text = "";
                this.cmbPartNo.Text = "";
                this.txtTotalQTy.Text = "";
                this.cmbLabelType.Text = "";
                this.cmbMonth.Focus();
                this.lblCount.Content = (object)"0";
                this.dvgDeatilsforTKM.ItemsSource = null;
                this.dvgDeatilsForTRMN.ItemsSource = null;
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_PRINT", CommonVariable.UserID);
                string Description = ex.Message.ToString();
                CustomMessageBox.CustomStriing customStriing = CustomMessageBox.CustomStriing.Error;
                string ErrorType = customStriing.ToString();
                customStriing = CustomMessageBox.CustomStriing.OK;
                string Result = customStriing.ToString();
                CommonMethods.MessageBoxShow(Description, ErrorType, Result);
            }
        }

        private void TxtTotalQTy_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                CommonMethods.NumericValue(e);
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_PRINT", CommonVariable.UserID);
                string Description = ex.Message.ToString();
                CustomMessageBox.CustomStriing customStriing = CustomMessageBox.CustomStriing.Error;
                string ErrorType = customStriing.ToString();
                customStriing = CustomMessageBox.CustomStriing.OK;
                string Result = customStriing.ToString();
                CommonMethods.MessageBoxShow(Description, ErrorType, Result);
            }
        }

        private void BtnBarcodeTemp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|Excel files(*.xls)|*.xls";
                saveFileDialog.ShowDialog();
                if (!(saveFileDialog.FileName != ""))
                    return;
                dt.Columns.Add("KANBAN_NO");
                dt.Columns.Add("TR_PART_NUMBER");
                dt.Columns.Add("PART_NAME");
                dt.Columns.Add("LINE_NO");
                dt.Columns.Add("LOCATION_NAME");
                dt.Columns.Add("ID_CODE");
                dt.Columns.Add("BIN_NO");
                dt.Columns.Add("BOX_TYPE");
                dt.Columns.Add("KANBAN_LOC_PROCESS_LINE_NO");
                dt.Columns.Add("SUPPLIER_CODE_NAME");
                dt.Columns.Add("KANBAN_TYPE");
                dt.Columns.Add("QTY");
                dt.Columns.Add("SERIAL_NO");
                CommonMethods.CreateExcellFile(dt, saveFileDialog.FileName, "PrintData");
                CustomMessageBox.CustomStriing customStriing = CustomMessageBox.CustomStriing.Successfull;
                string ErrorType = customStriing.ToString();
                customStriing = CustomMessageBox.CustomStriing.OK;
                string Result = customStriing.ToString();
                CommonMethods.MessageBoxShow("TEMPLATE CREATED SUCCESSFULLY", ErrorType, Result);
                this.Clear();
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_PRINT", CommonVariable.UserID);
                CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.C) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.C))
                this.BtnClear_Click(sender, (RoutedEventArgs)e);
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.E) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.E) || Keyboard.IsKeyDown(Key.Escape) && Keyboard.IsKeyDown(Key.Escape))
                this.BtnExit_Click(sender, (RoutedEventArgs)e);
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.B) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.B))
                this.BtnBrowse_Click(sender, (RoutedEventArgs)e);
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.P) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.P))
                this.BtnSave_Click(sender, (RoutedEventArgs)e);
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.G) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.G))
                this.BtnGetDetails_Click(sender, (RoutedEventArgs)e);
            if ((!Keyboard.IsKeyDown(Key.LeftAlt) || !Keyboard.IsKeyDown(Key.T)) && (!Keyboard.IsKeyDown(Key.RightAlt) || !Keyboard.IsKeyDown(Key.T)))
                return;
            this.BtnBarcodeTemp_Click(sender, (RoutedEventArgs)e);
        }

        private void RdMultiple_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                bool? isChecked = this.rdMultiple.IsChecked;
                bool flag = true;
                if (!(isChecked.GetValueOrDefault() == flag & isChecked.HasValue))
                    return;
                this.gbSingle.IsEnabled = false;
                this.gbMultiple.IsEnabled = true;
                this.txtBrowseFile.Text = "";
                this.cmbPartNo.Text = "";
                this.txtTotalQTy.Text = "";
                this.cmbLabelType.Text = "";
                this.cmbkanbanType.Text = "";
                this.cmbkanbanType.Text = "";
                this.dvgDeatilsforTKM.ItemsSource = null;
                this.dvgDeatilsForTRMN.ItemsSource = null;
                this.lblCount.Content = (object)"0";
                this.cmbkanbanType.Focus();
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_PRINT", CommonVariable.UserID);
                string Description = ex.Message.ToString();
                CustomMessageBox.CustomStriing customStriing = CustomMessageBox.CustomStriing.Error;
                string ErrorType = customStriing.ToString();
                customStriing = CustomMessageBox.CustomStriing.OK;
                string Result = customStriing.ToString();
                CommonMethods.MessageBoxShow(Description, ErrorType, Result);
            }
        }

        private void LinkTemplate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|Excel files(*.xls)|*.xls";
                saveFileDialog.ShowDialog();
                if (!(saveFileDialog.FileName != ""))
                    return;
                dt.Columns.Add("SL_NO");
                dt.Columns.Add("TR_PART_NUMBER");
                dt.Columns.Add("TOTAL_QTY");
                dt.Columns.Add("LABEL_TYPE");
                CommonMethods.CreateExcellFile(dt, saveFileDialog.FileName, "PrintData");
                CustomMessageBox.CustomStriing customStriing = CustomMessageBox.CustomStriing.Successfull;
                string ErrorType = customStriing.ToString();
                customStriing = CustomMessageBox.CustomStriing.OK;
                string Result = customStriing.ToString();
                CommonMethods.MessageBoxShow("TEMPLATE CREATED SUCCESSFULLY", ErrorType, Result);
                this.Clear();
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_PRINT", CommonVariable.UserID);
                CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private bool ControlValidationforSingle()
        {
            if (this.cmbPartNo.SelectedIndex == -1)
            {
                CommonMethods.MessageBoxShow("PLEASE SELECT PART NO", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                this.cmbPartNo.Focus();
                return false;
            }
            if (this.txtTotalQTy.Text == "")
            {
                CommonMethods.MessageBoxShow("PLEASE ENTER TOTAL QTY", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                this.txtTotalQTy.Focus();
                return false;
            }
            if (this.txtTotalQTy.Text == "0")
            {
                CommonMethods.MessageBoxShow("0 IS SNOT ACCEPTABLE FOR TOTAL QTY", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                this.txtTotalQTy.Focus();
                return false;
            }
            if (this.cmbLabelType.SelectedIndex == -1)
            {
                CommonMethods.MessageBoxShow("PLEASE SELECT LABEL TYPE", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                this.cmbLabelType.Focus();
                return false;
            }
            if (this.cmbMonth.SelectedIndex != -1)
                return true;
            CommonMethods.MessageBoxShow("PLEASE SELECT MONTH", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            this.cmbMonth.Focus();
            return false;
        }

        private void BtnGetDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!this.ControlValidationforSingle())
                    return;
                this.dt = new DataTable();
                this.dt.Columns.Add("SL_NO");
                this.dt.Columns.Add("TR_PART_NUMBER");
                this.dt.Columns.Add("TOTAL_QTY");
                this.dt.Columns.Add("LABEL_TYPE");
                this.dt.Columns.Add("MONTH");
                this.dt.Rows.Add((object)"1", (object)this.cmbPartNo.Text, (object)this.txtTotalQTy.Text, (object)this.cmbLabelType.Text, (object)(DateTime.Now.Year.ToString() + "-" + this.cmbMonth.Text));
                this.Transaction("GetPartDetails");
            }
            catch (Exception ex)
            {
                this.obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "KANBAN_PRINT", CommonVariable.UserID);
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
