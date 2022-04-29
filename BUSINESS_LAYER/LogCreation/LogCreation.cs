using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUSINESS_LAYER.LogCreation
{
    public class LogCreation
    {
        #region Objects
        DATA_LAYER.DatabaseConnectivity.DatabaseConnections objDB = new DATA_LAYER.DatabaseConnectivity.DatabaseConnections();
        #endregion

        #region LogCreation
        public void CreateLog(string ErrorDescription, string MethodName, string ModulName, string UserId)
        {
            try
            {
                objDB.ExecuteProcedureParam("Proc_LogDetails", ErrorDescription, MethodName, ModulName, UserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}