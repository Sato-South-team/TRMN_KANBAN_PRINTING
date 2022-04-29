using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY_LAYER.Login
{
   public class Login
    {
        #region Variables
        static string _UserID, _UserName, _Password, _Type, _ConfirmPassword, _Rights;
        #endregion

        #region Properties
        public static string Rights
        {
            get { return Login._Rights; }
            set { Login._Rights = value; }
        }

        public static string ConfirmPassword
        {
            get { return Login._ConfirmPassword; }
            set { Login._ConfirmPassword = value; }
        }

        public static string Type
        {
            get { return Login._Type; }
            set { Login._Type = value; }
        }

        public static string Password
        {
            get { return Login._Password; }
            set { Login._Password = value; }
        }

        public static string UserName
        {
            get { return Login._UserName; }
            set { Login._UserName = value; }
        }

        public static string UserID
        {
            get { return Login._UserID; }
            set { Login._UserID = value; }
        }

        #endregion
    }
}
