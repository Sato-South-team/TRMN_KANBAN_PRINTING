using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUSINESS_LAYER.Login
{
    public class Login
    {
        #region Objects
        DATA_LAYER.DatabaseConnectivity.DatabaseConnections obj_DB = new DATA_LAYER.DatabaseConnectivity.DatabaseConnections();
        #endregion

        #region Login
        public string BL_Login()
        {
            try
            {
                return obj_DB.ExecuteProcedureParam("Proc_Login", ENTITY_LAYER.Login.Login.UserID, ENTITY_LAYER.Login.Login.Password, ENTITY_LAYER.Login.Login.ConfirmPassword, ENTITY_LAYER.Login.Login.Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}