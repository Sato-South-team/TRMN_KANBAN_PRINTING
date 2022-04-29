using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUSINESS_LAYER.Reports
{
   public class Reports
    {
        #region Objects
        DATA_LAYER.DatabaseConnectivity.DatabaseConnections obj_DB = new DATA_LAYER.DatabaseConnectivity.DatabaseConnections();
        #endregion

        #region GroupMaster
        public DataSet BL_ReportDetails()
        {
            try
            {
                return obj_DB.ExecuteDataSetParam("proc_Report", ENTITY_LAYER.Reports.Reports.Type, ENTITY_LAYER.Reports.Reports.BatchNo, ENTITY_LAYER.Reports.Reports.ColumnName, ENTITY_LAYER.Reports.Reports.ColumnValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
