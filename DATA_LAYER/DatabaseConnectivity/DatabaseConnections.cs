using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
namespace DATA_LAYER.DatabaseConnectivity
{
    public class DatabaseConnections
    {
        #region "VARIABLE AND OBJECT DECLARATION"


        public SqlConnection con1 = new SqlConnection();
        public string strSqlConn;


        #endregion

        #region "METHODS"

        /// <summary>
        /// CHECK CONNECTION
        /// </summary>
        /// <returns>CONNECTION STATE</returns>

        public bool Connect()
        {
            try
            {
                strSqlConn = "Data Source=" + ENTITY_LAYER.DatabaseSettings.DatabaseSettings.SqldbServer + ";initial catalog=" + ENTITY_LAYER.DatabaseSettings.DatabaseSettings.SqlDBName + "; Integrated Security=false;User ID=" + ENTITY_LAYER.DatabaseSettings.DatabaseSettings.SqlDBUserID + ";PWD=" + ENTITY_LAYER.DatabaseSettings.DatabaseSettings.SqlDBPassword;
                if (con1.State == ConnectionState.Closed)
                {
                    con1.ConnectionString = strSqlConn;
                    con1.Open();

                }
                if (con1.State == ConnectionState.Open)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// CHECK DISCONNECTION
        /// </summary>
        /// <returns>CONNECTION STATE</returns>
        public void Disconnect()
        {
            try
            {
                if (con1.State == ConnectionState.Open)
                {
                    con1.Close();
                    con1.Dispose();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// EXECUTE NON QUERY WITH SQL STORE PROC AND PARAMETERS
        /// </summary>
        /// <param name="qry"></param>
        /// <returns></returns>

        public string ExecuteProcedureParam(string Proc, params object[] InpParam)
        {

            try
            {
                string Result = "";
                if (Connect() == true)
                {

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con1;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = Proc;
                    cmd.CommandTimeout = 0;
                    int inputParm = 0;
                    SqlCommandBuilder.DeriveParameters(cmd);
                    foreach (SqlParameter Parms in cmd.Parameters)
                    {
                        if (Parms.Direction == ParameterDirection.Input)
                        {
                            if (Parms.SqlDbType != SqlDbType.Structured)
                            {
                                Parms.Value = InpParam[inputParm];
                            }
                            else
                            {
                                string name = Parms.TypeName;
                                int index = name.IndexOf(".");
                                name = name.Substring(index + 1);
                                if (name.Contains("."))
                                {
                                    Parms.TypeName = name;
                                }
                                Parms.Value = InpParam[inputParm];

                            }
                            inputParm++;
                        }
                        else if (Parms.Direction == ParameterDirection.InputOutput)
                        {
                            Parms.Value = "0";
                            inputParm++;
                        }
                    }
                    cmd.ExecuteNonQuery();

                    foreach (SqlParameter parms in cmd.Parameters)
                    {
                        if (parms.Direction == ParameterDirection.InputOutput)
                        {
                            Result = parms.Value.ToString();
                            break;
                        }
                    }
                    cmd.Dispose();
                    Disconnect();
                    return Result;
                }
                else
                {
                    throw new Exception("database connection not found");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// EXECUTE DATATABLE WITH SQL STORE PROC AND PARAMETERS
        /// </summary>
        /// <param name="qry"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTableParam(string Proc, params object[] InpParam)
        {

            try
            {
                DataTable dt = new DataTable();
                if (Connect() == true)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con1;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = Proc;
                    cmd.CommandTimeout = 0;
                    int inputParm = 0;
                    SqlCommandBuilder.DeriveParameters(cmd);
                    foreach (SqlParameter Parms in cmd.Parameters)
                    {
                        if (Parms.Direction == ParameterDirection.Input)
                        {
                            if (Parms.SqlDbType != SqlDbType.Structured)
                            {
                                Parms.Value = InpParam[inputParm];
                            }
                            else
                            {
                                string name = Parms.TypeName;
                                int index = name.IndexOf(".");
                                name = name.Substring(index + 1);
                                if (name.Contains("."))
                                {
                                    Parms.TypeName = name;
                                }
                                Parms.Value = InpParam[inputParm];
                            }
                            inputParm++;
                        }
                    }
                    dt.Load(cmd.ExecuteReader());
                    cmd.Dispose();
                    Disconnect();
                    return dt;
                }
                else
                {
                    throw new Exception("database connection not found");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// EXECUTE DATASET WITH SQL STORE PROC AND PARAMETERS
        /// </summary>
        /// <param name="qry"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSetParam(string Proc, params object[] InpParam)
        {

            try
            {
                DataSet ds = new DataSet();
                if (Connect() == true)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con1;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = Proc;
                    cmd.CommandTimeout = 0;
                    int inputParm = 0;
                    SqlCommandBuilder.DeriveParameters(cmd);
                    foreach (SqlParameter Parms in cmd.Parameters)
                    {
                        if (Parms.Direction == ParameterDirection.Input)
                        {
                            if (Parms.SqlDbType != SqlDbType.Structured)
                            {
                                Parms.Value = InpParam[inputParm];
                            }
                            else
                            {
                                string name = Parms.TypeName;
                                int index = name.IndexOf(".");
                                name = name.Substring(index + 1);
                                if (name.Contains("."))
                                {
                                    Parms.TypeName = name;
                                }
                                Parms.Value = InpParam[inputParm];
                            }
                            inputParm++;
                        }
                    }
                    SqlDataAdapter SDP = new SqlDataAdapter(cmd);
                    SDP.Fill(ds);
                    SDP.Dispose();
                    cmd.Dispose();
                    Disconnect();
                    return ds;
                }
                else
                {
                    throw new Exception("database connection not found");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// EXECUTE SCALER WITH SQL STORE PROC AND PARAMETERS
        /// </summary>
        /// <param name="qry"></param>
        /// <returns></returns>

        public string ExecuteScalerParam(string Proc, params object[] InpParam)
        {

            try
            {
                string Result = "";
                if (Connect() == true)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con1;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = Proc;
                    cmd.CommandTimeout = 0;
                    int inputParm = 0;
                    SqlCommandBuilder.DeriveParameters(cmd);
                    foreach (SqlParameter Parms in cmd.Parameters)
                    {
                        if (Parms.Direction == ParameterDirection.Input)
                        {
                            Parms.Value = InpParam[inputParm];
                            inputParm++;
                        }
                    }
                    Result = cmd.ExecuteScalar().ToString();
                    cmd.Dispose();
                    Disconnect();
                    return Result;
                }
                else
                {
                    throw new Exception("database connection not found");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// EXECUTE SCALAR FUNCTION WITH SQL QUERY
        /// </summary>
        /// <param name="qry"></param>
        /// <returns></returns>
        public int ExecuteScalar(string qry)
        {
            try
            {
                if (Connect() == true)
                {
                    SqlCommand cmd = new SqlCommand(qry, con1);
                    cmd.CommandText = qry;
                    int i = (int)cmd.ExecuteScalar();
                    cmd.Dispose();
                    return i;
                }
                else
                {
                    throw new Exception("database connection not found");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// EXECUTE NONQUERY
        /// </summary>
        /// <param name="qry"></param>
        public void ExecuteNonQuery(string qry)
        {
            try
            {
                if (Connect() == true)
                {
                    SqlCommand cmd = new SqlCommand(qry, con1);
                    cmd.CommandText = qry;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    Disconnect();
                }
                else
                {
                    throw new Exception("database connection not found");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// EXECUTE DATASET WITH SQL QUERY
        /// </summary>
        /// <param name="qry"></param>
        /// <returns></returns>
        public DataSet ExecuteDataset(string qry)
        {
            try
            {
                if (Connect() == true)
                {

                    SqlDataAdapter Sqlda = new SqlDataAdapter(qry, con1);
                    DataSet ds = new DataSet();
                    Sqlda.Fill(ds);
                    Sqlda.Dispose();
                    Disconnect();
                    return ds;
                }
                else
                {
                    throw new Exception("database connection not found");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// EXECUTE DATATABLE WITH SQL QUERY
        /// </summary>
        /// <param name="qry"></param>
        /// <returns></returns>


        public DataTable ExecuteDataTable(string qry)
        {
            try
            {
                if (Connect() == true)
                {

                    SqlDataAdapter Sqlda = new SqlDataAdapter(qry, con1);
                    DataSet ds = new DataSet();
                    Sqlda.Fill(ds);
                    Sqlda.Dispose();
                    Disconnect();
                    return ds.Tables[0];
                }
                else
                {
                    throw new Exception("database connection not found");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion
}