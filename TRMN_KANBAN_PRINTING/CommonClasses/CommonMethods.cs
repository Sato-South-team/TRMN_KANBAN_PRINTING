using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TRMN_KANBAN_PRINTING.CommonClasses
{
    class CommonMethods
    {
        #region Common_Methods
        public static string DataTableToString(DataTable dt1)
        {
            try
            {
                StringBuilder str = new StringBuilder();
                for (int j = 0; j < dt1.Rows.Count; j++)
                {
                    for (int k = 0; k < dt1.Columns.Count; k++)
                    {
                        str.AppendFormat("{0}:{1}$", dt1.Columns[k].ColumnName, dt1.Rows[j][k]);
                    }
                }
                return str.ToString();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static void DigitsOnly(TextCompositionEventArgs e)
        {
            try
            {
                if (e.Text != "")
                {
                    char c = Convert.ToChar(e.Text);
                    if (Char.IsNumber(c))
                        e.Handled = false;
                    else
                        e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void MessageBoxShow(string Description, string ErrorType, string Result)
        {
            try
            {
                StartUp.CustomMessageBox objCustomMsg = new StartUp.CustomMessageBox(Description, ErrorType, Result);
                objCustomMsg.ShowDialog(); ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void FillComboBox(System.Windows.Controls.ComboBox ComboBox, DataTable dt, string DisPlayMember)
        {
            try
            {
                ComboBox.ItemsSource = null;
                ComboBox.DisplayMemberPath = "";
                ComboBox.ItemsSource = dt.DefaultView;
                ComboBox.DisplayMemberPath = DisPlayMember;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UNFill_ComboBox(System.Windows.Controls.ComboBox ComboBox)
        {
            try
            {
                ComboBox.ItemsSource = null;
                ComboBox.DisplayMemberPath = "";
                ComboBox.SelectedValuePath = "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void NumericValue(TextCompositionEventArgs e)
        {
            try
            {
                if (e.Text != "")
                {
                    char c = Convert.ToChar(e.Text);
                    if (Char.IsNumber(c))
                        e.Handled = false;
                    else
                        e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void FillComboBox(System.Windows.Controls.ComboBox ComboBox, DataTable dt, string DisPlayMember, string ValueMember)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                    ComboBox.ItemsSource = null;
                    ComboBox.DisplayMemberPath = "";
                    ComboBox.ItemsSource = dt.DefaultView;
                    ComboBox.DisplayMemberPath = DisPlayMember;
                    ComboBox.SelectedValuePath = ValueMember;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CreatLogDetails(string ErrorDescrription, string methodName, string ModuleName, string CreatedBy)
        {
            try
            {
                StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "Log\\" + ModuleName + "-" + System.DateTime.Now.ToString("dd-MM-yyyy") + ".txt", true);
                sw.WriteLine(ErrorDescrription + " , " + methodName + " , " + ModuleName + " , " + CreatedBy + " , " + System.DateTime.Now.ToString());
                sw.Dispose();
                sw.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void CreatDataBaseLogDetails(string DBServerName, string DBSarverID, string DBServerPassword, string DataBaseName)
        {
            try
            {
                StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "DataBaseSetting.txt");
                sw.WriteLine("DBServerName" + "," + "DBSarverID" + "," + "DBServerPassword" + "," + "DataBaseName");
                sw.WriteLine(DBServerName + "," + DBSarverID + "," + DBServerPassword + "," + DataBaseName);
                sw.Dispose();
                sw.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static string ReadFile(string Path)
        {
            try
            {
                string Result = "";
                if (File.Exists(Path))
                {
                    StreamReader SR = new StreamReader(Path);
                    Result = SR.ReadToEnd();
                    SR.Dispose();
                    SR.Close();
                }
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static string Encrypt_data(string str)
        {
            try
            {
                char[] arr = str.ToCharArray();
                Array.Reverse(arr);
                str = new string(arr);
                return Convert.ToBase64String(Encoding.Unicode.GetBytes(str));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static string Decrypt_data(string str)
        {
            try
            {
                char[] arr = Encoding.Unicode.GetString(Convert.FromBase64String(str)).ToCharArray();
                Array.Reverse(arr);
                return new string(arr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool CreateExcellFile(DataTable dt, string FilePath, string SheetName)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application excel;
                Microsoft.Office.Interop.Excel.Workbook worKbooK;
                Microsoft.Office.Interop.Excel.Worksheet worKsheeT;
                Microsoft.Office.Interop.Excel.Range celLrangE;
                excel = new Microsoft.Office.Interop.Excel.Application();

                excel.Visible = false;
                excel.DisplayAlerts = false;
                worKbooK = excel.Workbooks.Add(Type.Missing);

                worKsheeT = (Microsoft.Office.Interop.Excel.Worksheet)worKbooK.ActiveSheet;
                worKsheeT.Name = SheetName;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    worKsheeT.Cells[1, i + 1] = dt.Columns[i].ColumnName.ToString();
                    worKsheeT.Cells[1, i + 1].Font.Bold = true;
                    worKsheeT.Cells.Font.Size = 11;
                }
                int cell = 1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        worKsheeT.Cells[cell + 1, j + 1] = dt.Rows[i][j].ToString(); ;
                    }
                    cell++;
                }

                celLrangE = worKsheeT.Range[worKsheeT.Cells[1, 1], worKsheeT.Cells[dt.Rows.Count + 1, dt.Columns.Count]];
                celLrangE.EntireColumn.AutoFit();
                Microsoft.Office.Interop.Excel.Borders border = celLrangE.Borders;
                border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                worKbooK.SaveAs(FilePath);
                worKbooK.Close();
                excel.Quit();
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }
            return true;
        }
        public static DataTable ReadExcelData(string fileName, string SheetName)
        {
            try
            {
                string conn = string.Empty;
                DataTable dtexcel = new DataTable();

                if (fileName.EndsWith(".xls"))
                    conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
                else
                    conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1';"; //for above excel 2007  
                using (OleDbConnection con = new OleDbConnection(conn))
                {
                    OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from " + SheetName, con); //here we read data from sheet1  
                    oleAdpt.Fill(dtexcel); //fill excel data into dataTable  
                }
                return dtexcel;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        #endregion
    }
}
