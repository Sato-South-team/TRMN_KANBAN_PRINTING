using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace BUSINESS_LAYER.Transaction
{
    public class Transaction
    {
        DATA_LAYER.DatabaseConnectivity.DatabaseConnections obj_DB = new DATA_LAYER.DatabaseConnectivity.DatabaseConnections();

        public DataSet  BL_GetPrnDetails()
        {
            try
            {
                return obj_DB.ExecuteDataSetParam("proc_GetPrnDetails", ENTITY_LAYER.Transaction.Transaction.PrnType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string  BL_KanbanPrintTransaction()
        {
            try
            {
                return obj_DB.ExecuteProcedureParam("proc_KanbanPrint", ENTITY_LAYER.Transaction.Transaction.PartNo, ENTITY_LAYER.Transaction.Transaction.EkanbanNo,ENTITY_LAYER.Transaction.Transaction.KanbanType, ENTITY_LAYER.Transaction.Transaction.Dt, ENTITY_LAYER.Transaction.Transaction.TKMdt, ENTITY_LAYER.Login.Login.UserID,ENTITY_LAYER.Transaction.Transaction.Type,ENTITY_LAYER.Transaction.Transaction.Nooflabels,ENTITY_LAYER.Transaction.Transaction.BatchNo, ENTITY_LAYER.Transaction.Transaction.RefNo, ENTITY_LAYER.Transaction.Transaction.Barcodevalue, ENTITY_LAYER.Transaction.Transaction.TRMNSerialdt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet BL_KanbanDPrintDetails()
        {
            try
            {
                return obj_DB.ExecuteDataSetParam("proc_KanbanPrint", ENTITY_LAYER.Transaction.Transaction.PartNo, ENTITY_LAYER.Transaction.Transaction.EkanbanNo, ENTITY_LAYER.Transaction.Transaction.KanbanType, ENTITY_LAYER.Transaction.Transaction.Dt, ENTITY_LAYER.Transaction.Transaction.TKMdt, ENTITY_LAYER.Login.Login.UserID, ENTITY_LAYER.Transaction.Transaction.Type, ENTITY_LAYER.Transaction.Transaction.Nooflabels, ENTITY_LAYER.Transaction.Transaction.BatchNo, ENTITY_LAYER.Transaction.Transaction.RefNo, ENTITY_LAYER.Transaction.Transaction.Barcodevalue,ENTITY_LAYER.Transaction.Transaction.TRMNSerialdt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet BL_RePrintDetails()
        {
            try
            {
                return obj_DB.ExecuteDataSetParam("proc_Reprint", ENTITY_LAYER.Transaction.Transaction.Type, ENTITY_LAYER.Transaction.Transaction.Barcodevalue, ENTITY_LAYER.Transaction.Transaction.ColumnName, ENTITY_LAYER.Transaction.Transaction.ColumnValue, ENTITY_LAYER.Transaction.Transaction.EkanbanNo, ENTITY_LAYER.Transaction.Transaction.Remarks, ENTITY_LAYER.Transaction.Transaction.KanbanType, ENTITY_LAYER.Login.Login.UserID, ENTITY_LAYER.Transaction.Transaction.Dt, ENTITY_LAYER.Transaction.Transaction.DockCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet BL_TransactionDetails()
        {
            try
            {
                return obj_DB.ExecuteDataSetParam("proc_Transaction", ENTITY_LAYER.Transaction.Transaction.TRMNBarcode, ENTITY_LAYER.Transaction.Transaction.EkanbanNo, ENTITY_LAYER.Transaction.Transaction.PDSBarcode, ENTITY_LAYER.Transaction.Transaction.InvoiceBarcode, ENTITY_LAYER.Transaction.Transaction.SkidBarcode, ENTITY_LAYER.Transaction.Transaction.TransactionType, ENTITY_LAYER.Login.Login.UserID, ENTITY_LAYER.Transaction.Transaction.Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
