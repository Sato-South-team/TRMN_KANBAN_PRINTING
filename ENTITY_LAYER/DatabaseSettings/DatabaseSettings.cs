using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY_LAYER.DatabaseSettings
{
    public class DatabaseSettings
    {
        #region Variables
        static string _SqldbServer, _SqlDBName, _SqlDBUserID, _SqlDBPassword;
        #endregion

        #region Properties
        public static string SqlDBPassword
        {
            get { return _SqlDBPassword; }
            set { _SqlDBPassword = value; }
        }

        public static string SqlDBUserID
        {
            get { return _SqlDBUserID; }
            set { _SqlDBUserID = value; }
        }

        public static string SqlDBName
        {
            get { return _SqlDBName; }
            set { _SqlDBName = value; }
        }

        public static string SqldbServer
        {
            get { return _SqldbServer; }
            set { _SqldbServer = value; }
        }
        #endregion
    }
}
