using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class SqlConnectionHelper
    {
        public static IDbConnection GetConnection()
        {
            string _connStr = SysConfig.ConnectionString;

            IDbConnection connSQL = null;
            try
            {
                connSQL = new SqlConnection(_connStr);
                connSQL.Open();
                return connSQL;
            }
            catch (Exception exp)
            {
                throw new Exception("Create SQLConn Failed:" + exp.Message);
            }
        }

        //public IDbConnection GetMySqlConnection()
        //{
        //    string _connStr = SysConfig.MysqlConnectionString;

        //    IDbConnection connSQL = null;
        //    try
        //    {
        //        connSQL = new MySqlConnection(_connStr);
        //        connSQL.Open();
        //        return connSQL;
        //    }
        //    catch (Exception exp)
        //    {
        //        throw new Exception("Create MySqlConn Failed:" + exp.Message);
        //    }
        //}

        public static IDbConnection GetSQLiteConnection()
        {
            string _connStr = SysConfig.SQLiteConnectionString;
            IDbConnection connSQL = null;
            try
            {
                connSQL = new SQLiteConnection(_connStr);
                connSQL.Open();
                return connSQL;
            }
            catch (Exception exp)
            {
                throw new Exception("Create SQLiteConn Failed:" + exp.Message);
            }
        }
    }
}
