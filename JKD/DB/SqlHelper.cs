/*
 * Created by SharpDevelop.
 * User: Lenovo
 * Date: 2019/6/29
 * Time: 11:52
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
namespace JKD
{
    /// <summary>
    /// Description of SqlHelp.
    /// </summary>
    public static class SqlHelper
    {
        public static string MysqlConnStr = ConfigurationManager.ConnectionStrings["mysqlConnectionStr"].ConnectionString;
        public static Object ExecuteScaler(String sqltext, params MySqlParameter[] ps)
        {
            using (MySqlConnection conn = new MySqlConnection(MysqlConnStr))
            {
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = sqltext;
                    cmd.Parameters.AddRange(ps);
                    return cmd.ExecuteScalar();
                }
            }
        }
        public static DataTable ExecuteTable(String sqltext, params MySqlParameter[] ps)
        {
            using (MySqlDataAdapter da = new MySqlDataAdapter(sqltext, MysqlConnStr))
            {
                da.SelectCommand.Parameters.AddRange(ps);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        public static int ExecuteNoQuery(string sqltext, params MySqlParameter[] ps)
        {
            using (MySqlConnection conn = new MySqlConnection(MysqlConnStr))
            {
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = sqltext;
                    cmd.CommandType = CommandType.Text;
                    if (ps.Length > 0)
                    {
                        cmd.Parameters.AddRange(ps);
                    }

                    return cmd.ExecuteNonQuery();
                }
            }

        }

    }
}
