using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace ENTITY_LAYER.Transaction
{
   public static  class Transaction
    {
       static string _PrnType,_PartNo,_EkanbanNo,_Type,_kanbanType,_Nooflabels,_Barcodevalue,_BatchNo,_ColumnName,_ColumnValue,_Remarks,
            _CustomerBarcode,_TRMNBarcode,_SkidBarcode,_PDSBarcode,_InvoiceBarcode,_TransactionType,_DockCode;
        static DataTable dt,_TKMdt,_TRMNSerialdt;
        static int _RefNo;
        public static  string PrnType { get => _PrnType; set => _PrnType = value; }
        public static DataTable Dt { get => dt; set => dt = value; }
        public static string PartNo { get => _PartNo; set => _PartNo = value; }
        public static string EkanbanNo { get => _EkanbanNo; set => _EkanbanNo = value; }
        public static string Type { get => _Type; set => _Type = value; }
        public static string KanbanType { get => _kanbanType; set => _kanbanType = value; }
        public static string Nooflabels { get => _Nooflabels; set => _Nooflabels = value; }
        public static DataTable TKMdt { get => _TKMdt; set => _TKMdt = value; }
        public static string Barcodevalue { get => _Barcodevalue; set => _Barcodevalue = value; }
        public static string BatchNo { get => _BatchNo; set => _BatchNo = value; }
        public static int RefNo { get => _RefNo; set => _RefNo = value; }
        public static string ColumnName { get => _ColumnName; set => _ColumnName = value; }
        public static string ColumnValue { get => _ColumnValue; set => _ColumnValue = value; }
        public static string Remarks { get => _Remarks; set => _Remarks = value; }
        public static string CustomerBarcode { get => _CustomerBarcode; set => _CustomerBarcode = value; }
        public static string SkidBarcode { get => _SkidBarcode; set => _SkidBarcode = value; }
        public static string PDSBarcode { get => _PDSBarcode; set => _PDSBarcode = value; }
        public static string InvoiceBarcode { get => _InvoiceBarcode; set => _InvoiceBarcode = value; }
        public static string TransactionType { get => _TransactionType; set => _TransactionType = value; }
        public static string TRMNBarcode { get => _TRMNBarcode; set => _TRMNBarcode = value; }
        public static DataTable TRMNSerialdt { get => _TRMNSerialdt; set => _TRMNSerialdt = value; }
        public static string DockCode { get => _DockCode; set => _DockCode = value; }
    }
}
