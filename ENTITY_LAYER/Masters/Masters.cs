using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace ENTITY_LAYER.Masters
{
    public static class Masters
    {
        #region Variables
        static int _RefNo;
        static string   _Type, _UserID, _UserName, _GroupID, _Password, _Rights,_LoginID,_PartNo,_Ekanban,_LocationName,_LocationType;
        static DataTable _Dt;
        #endregion
        #region Properties
        public static string Type { get => _Type; set => _Type = value; }
        public static string UserID { get => _UserID; set => _UserID = value; }
        public static string UserName { get => _UserName; set => _UserName = value; }
        public static string GroupID { get => _GroupID; set => _GroupID = value; }
        public static string Password { get => _Password; set => _Password = value; }
        public static string Rights { get => _Rights; set => _Rights = value; }
        public static int RefNo { get => _RefNo; set => _RefNo = value; }
        public static string LoginID { get => _LoginID; set => _LoginID = value; }
        public static string PartNo { get => _PartNo; set => _PartNo = value; }
        public static DataTable Dt { get => _Dt; set => _Dt = value; }
        public static string LocationName { get => _LocationName; set => _LocationName = value; }
        public static string Ekanban { get => _Ekanban; set => _Ekanban = value; }
        public static string LocationType { get => _LocationType; set => _LocationType = value; }


        #endregion
    }
}
