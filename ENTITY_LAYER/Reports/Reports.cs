using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY_LAYER.Reports
{
   public class Reports
    {
        #region Variables
        static string _Type,_BatchNo,_ColumnName,_ColumnValue;
        #endregion
        #region Properties
        public static string Type { get => _Type; set => _Type = value; }
        public static string BatchNo { get => _BatchNo; set => _BatchNo = value; }
        public static string ColumnName { get => _ColumnName; set => _ColumnName = value; }
        public static string ColumnValue { get => _ColumnValue; set => _ColumnValue = value; }

        #endregion
    }
}
