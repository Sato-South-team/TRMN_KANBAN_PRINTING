using SATOPrinterAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TRMN_KANBAN_PRINTING.CommonClasses
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DOCINFO
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pDocName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pOutputFile;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pDataType;
    }
    public  class SatoPrinter
    {
        #region Variables and Objects
        Printer printer = null;
        #endregion

        #region Methods
        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern long OpenPrinter(string pPrinterName, ref IntPtr phPrinter, int pDefault);
        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern long StartDocPrinter(IntPtr hPrinter, int Level, ref DOCINFO pDocInfo);
        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern long StartPagePrinter(IntPtr hPrinter);
        [DllImport("winspool.drv", CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern long WritePrinter(IntPtr hPrinter, string data, int buf, ref int pcWritten);
        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern long EndPagePrinter(IntPtr hPrinter);
        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern long EndDocPrinter(IntPtr hPrinter);
        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern long ClosePrinter(IntPtr hPrinter);


        public void PrintUSBCommand(string printData, string PrinterName)
        {
            System.IntPtr lhPrinter = new System.IntPtr();
            DOCINFO di = new DOCINFO();
            int pcWritten = 0;
            int iprinter = 0;
                SatoPrinter.OpenPrinter(PrinterName, ref lhPrinter, 0);
                if (lhPrinter == IntPtr.Zero)
                {
                    Console.WriteLine("Printer Not found");
                    //Console.ReadLine();
                    return;
                }
                //PrintDirect.OpenPrinter("LPT:", ref lhPrinter, 0); 
                di.pDocName = "Test";
                //di.pDataType = "RAW";

                SatoPrinter.StartDocPrinter(lhPrinter, 1, ref di);
                SatoPrinter.StartPagePrinter(lhPrinter);
                SatoPrinter.WritePrinter(lhPrinter, printData, printData.Length, ref pcWritten);
                SatoPrinter.EndPagePrinter(lhPrinter);
                SatoPrinter.EndDocPrinter(lhPrinter);
                SatoPrinter.ClosePrinter(lhPrinter);
            
        }
        public  SatoPrinter(Printer.InterfaceType PrinterInterface, string PrinterIp = "192.168.1.1", string PortNo = "9100")
        {
            printer = new Printer();
            if (PrinterInterface == Printer.InterfaceType.USB)
            {
                FillUSBPTR();
            }
            else if (PrinterInterface == Printer.InterfaceType.TCPIP)
            {
                FillTcpPTR(PrinterIp, PortNo);
            }


        }
        private void FillUSBPTR()
        {
            var USBPorts = printer.GetUSBList();
            if (USBPorts.Count > 0)
            {
                printer.Interface = Printer.InterfaceType.USB;
                printer.USBPortID = USBPorts[0].PortID;
            }
        }

        public string QueryStatus()
        {
            byte[] qry = SATOPrinterAPI.Utils.StringToByteArray("");
            byte[] result = printer.Query(qry);
            string status = SATOPrinterAPI.Utils.ByteArrayToString(result);
            return status;
        }
        public string GetprinterStatus()
        {
            string status = "Not Connected";
            printer.Connect();
            // Thread.Sleep(2000);
            SATOPrinterAPI.Printer.Status printerStatus = printer.GetPrinterStatus();
            
            status = printerStatus.Code;
            status = GetStatusMessages(status);
            if (status.StartsWith("ONLINE"))
            {
                return "ONLINE_" + status;
            }
            else
            {
                return "OFFLINE_" + status;
            }
            printer.Disconnect();
            return status;
        }

        private void FillTcpPTR(string printerIP, string portNo)
        {
            printer.Interface = Printer.InterfaceType.TCPIP;
            printer.TCPIPAddress = printerIP;
            printer.TCPIPPort = portNo;
        }
        private static string GetStatusMessages(string data)
        {
            switch (Convert.ToChar(data))//HexToInt(data)))
            {
                case '0':
                    return ("OFFLINE_STATE" + " : " + "STATUS_NO_ERROR");

                case '1':
                    return ("OFFLINE_STATE" + " : " + "STATUS_RIBBON_LABEL_NEAR_END");

                case '2':
                    return ("OFFLINE_STATE" + " : " + "STATUS_BUFFER_NEAR_FULL");

                case '3':
                    return ("OFFLINE_STATE" + " : " + "STATUS_RIBBON_LABEL_NEAR_END_BUFFER_NEAR_FULL");

                case '4':
                    return ("OFFLINE_STATE" + " : " + "STATUS_PRINTER_PAUSE");

                case 'A':
                    return ("ONLINE_STATE" + " : " + "STATUS_WAIT_TO_RECEIVE" + " : " + "STATUS_NO_ERROR");

                case 'B':
                    return ("ONLINE_STATE" + " : " + "STATUS_WAIT_TO_RECEIVE" + " : " + "STATUS_RIBBON_LABEL_NEAR_END");

                case 'C':
                    return ("ONLINE_STATE" + " : " + "STATUS_WAIT_TO_RECEIVE" + " : " + "STATUS_BUFFER_NEAR_FULL");

                case 'D':
                    return ("ONLINE_STATE" + " : " + "STATUS_WAIT_TO_RECEIVE" + " : " + "STATUS_RIBBON_LABEL_NEAR_END_BUFFER_NEAR_FULL");

                case 'E':
                    return ("ONLINE_STATE" + " : " + "STATUS_WAIT_TO_RECEIVE" + " : " + "STATUS_PRINTER_PAUSE");

                case 'G':
                    return ("ONLINE_STATE" + " : " + "STATUS_PRINTING");

                case 'H':
                    return ("ONLINE_STATE" + " : " + "STATUS_PRINTING" + " : " + "STATUS_RIBBON_LABEL_NEAR_END");

                case 'I':
                    return ("ONLINE_STATE" + " : " + "STATUS_PRINTING" + " : " + "STATUS_BUFFER_NEAR_FULL");

                case 'J':
                    return ("ONLINE_STATE" + " : " + "STATUS_PRINTING" + " : " + "STATUS_RIBBON_LABEL_NEAR_END_BUFFER_NEAR_FULL");

                case 'K':
                    return ("ONLINE_STATE" + " : " + "STATUS_PRINTING" + " : " + "STATUS_PRINTER_PAUSE");

                case 'M':
                    return ("ONLINE_STATE" + " : " + "STATUS_STANDBY");

                case 'N':
                    return ("ONLINE_STATE" + " : " + "STATUS_STANDBY" + " : " + "STATUS_RIBBON_LABEL_NEAR_END");

                case 'O':
                    return ("ONLINE_STATE" + " : " + "STATUS_STANDBY" + " : " + "STATUS_BUFFER_NEAR_FULL");

                case 'P':
                    return ("ONLINE_STATE" + " : " + "STATUS_STANDBY" + " : " + "STATUS_RIBBON_LABEL_NEAR_END_BUFFER_NEAR_FULL");

                case 'Q':
                    return ("ONLINE_STATE" + " : " + "STATUS_STANDBY" + " : " + "STATUS_PRINTER_PAUSE");

                case 'S':
                    return ("ONLINE_STATE" + " : " + "STATUS_ANALYZING");

                case 'T':
                    return ("ONLINE_STATE" + " : " + "STATUS_ANALYZING" + " : " + "STATUS_RIBBON_LABEL_NEAR_END");

                case 'U':
                    return ("ONLINE_STATE" + " : " + "STATUS_ANALYZING" + " : " + "STATUS_BUFFER_NEAR_FULL");

                case 'V':
                    return ("ONLINE_STATE" + " : " + "STATUS_ANALYZING" + " : " + "STATUS_RIBBON_LABEL_NEAR_END_BUFFER_NEAR_FULL");

                case 'W':
                    return ("ONLINE_STATE" + " : " + "STATUS_ANALYZING" + " : " + "STATUS_PRINTER_PAUSE");

                case 'b':
                    return ("ERROR_DETECTION" + " : " + "STATUS_HEAD_OPEN");

                case 'c':
                    return ("ERROR_DETECTION" + " : " + "STATUS_PAPER_END");

                case 'd':
                    return ("ERROR_DETECTION" + " : " + "STATUS_RIBBON_END");

                case 'e':
                    return ("ERROR_DETECTION" + " : " + "STATUS_MEDIA_ERROR");

                case 'f':
                    return ("ERROR_DETECTION" + " : " + "STATUS_SENSOR_ERROR");

                case 'g':
                    return ("ERROR_DETECTION" + " : " + "STATUS_HEAD_ERROR");

                case 'h':
                    return ("ERROR_DETECTION" + " : " + "STATUS_CUTTER_OPEN_ERROR");

                case 'i':
                    return ("ERROR_DETECTION" + " : " + "STATUS_CARD_ERROR");

                case 'j':
                    return ("ERROR_DETECTION" + " : " + "STATUS_CUTTER_ERROR");

                case 'k':
                    return ("ERROR_DETECTION" + " : " + "STATUS_OTHER_ERRORS");

                case 'o':
                    return ("ERROR_DETECTION" + " : " + "STATUS_OTHER_IC_TAG_ERROR");

                case 'q':
                    return ("ERROR_DETECTION" + " : " + "STATUS_BATTER_ERROR");
            }
            return "UNEXPECTED_VALUE";
        }
        public void PrintCommand_TCPIP(string printData)
        {
            byte[] sbplByte = SATOPrinterAPI.Utils.StringToByteArray(printData);
            printer.Send(sbplByte);
        }
        public void SendRawData(string PrinterName, string printData)
        {
            Driver drive = new Driver();
            drive.SendRawData(PrinterName,printData);
        }
        #endregion
    }
}
