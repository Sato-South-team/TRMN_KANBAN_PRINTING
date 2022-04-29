using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUSINESS_LAYER.Masters
{
   public class Masters
    {
        #region Objects
        DATA_LAYER.DatabaseConnectivity.DatabaseConnections obj_DB = new DATA_LAYER.DatabaseConnectivity.DatabaseConnections();
        #endregion

        #region GroupMaster
        public string BL_GroupMasterTransaction()
        {
            try
            {
                return obj_DB.ExecuteProcedureParam("Proc_GroupMaster", ENTITY_LAYER.Masters.Masters.GroupID, ENTITY_LAYER.Masters.Masters.Rights, ENTITY_LAYER.Masters.Masters.Type, ENTITY_LAYER.Login.Login.UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet BL_GroupMasterDetails()
        {
            try
            {
                return obj_DB.ExecuteDataSetParam("Proc_GroupMaster", ENTITY_LAYER.Masters.Masters.GroupID, ENTITY_LAYER.Masters.Masters.Rights, ENTITY_LAYER.Masters.Masters.Type, ENTITY_LAYER.Login.Login.UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region UserMaster
        public string BL_UserMasterTransaction()
        {
            try
            {
                return obj_DB.ExecuteProcedureParam("Proc_UserMaster", ENTITY_LAYER.Masters.Masters.RefNo, ENTITY_LAYER.Masters.Masters.UserID, ENTITY_LAYER.Masters.Masters.UserName, ENTITY_LAYER.Masters.Masters.Password, ENTITY_LAYER.Masters.Masters.GroupID, ENTITY_LAYER.Login.Login.UserID, ENTITY_LAYER.Masters.Masters.Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet BL_UserMasterDetails()
        {
            try
            {
                return obj_DB.ExecuteDataSetParam("Proc_UserMaster", ENTITY_LAYER.Masters.Masters.RefNo, ENTITY_LAYER.Masters.Masters.UserID, ENTITY_LAYER.Masters.Masters.UserName, ENTITY_LAYER.Masters.Masters.Password, ENTITY_LAYER.Masters.Masters.GroupID, ENTITY_LAYER.Login.Login.UserID, ENTITY_LAYER.Masters.Masters.Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region PartMaster
        public string BL_PartMasterTransaction()
        {
            try
            {
                return obj_DB.ExecuteProcedureParam("proc_PartMaster", ENTITY_LAYER.Masters.Masters.RefNo, ENTITY_LAYER.Masters.Masters.PartNo, ENTITY_LAYER.Masters.Masters.Ekanban, ENTITY_LAYER.Masters.Masters.Dt, ENTITY_LAYER.Login.Login.UserID, ENTITY_LAYER.Masters.Masters.Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet BL_PartMasterDetails()
        {
            try
            {
                return obj_DB.ExecuteDataSetParam("proc_PartMaster", ENTITY_LAYER.Masters.Masters.RefNo, ENTITY_LAYER.Masters.Masters.PartNo, ENTITY_LAYER.Masters.Masters.Ekanban, ENTITY_LAYER.Masters.Masters.Dt, ENTITY_LAYER.Login.Login.UserID, ENTITY_LAYER.Masters.Masters.Type);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region SortingLocationMaster
        public string BL_SortingLocationMasterTransaction()
        {
            try
            {
                return obj_DB.ExecuteProcedureParam("proc_SortingLocationMaster", ENTITY_LAYER.Masters.Masters.RefNo, ENTITY_LAYER.Masters.Masters.LocationName, ENTITY_LAYER.Masters.Masters.Dt, ENTITY_LAYER.Login.Login.UserID, ENTITY_LAYER.Masters.Masters.Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet BL_SortingLocationMasterDetails()
        {
            try
            {
                return obj_DB.ExecuteDataSetParam("proc_SortingLocationMaster", ENTITY_LAYER.Masters.Masters.RefNo, ENTITY_LAYER.Masters.Masters.LocationName, ENTITY_LAYER.Masters.Masters.Dt, ENTITY_LAYER.Login.Login.UserID, ENTITY_LAYER.Masters.Masters.Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}
