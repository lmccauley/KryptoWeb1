using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;


namespace KryptoWeb
{
    public class DataLayer
    {
        public static DataTable GetSummary()
        {
            //string sql = "INSERT INTO summary(coin, poolbalance) VALUES (@coin, @poolbalance) ON DUPLICATE KEY UPDATE poolbalance = @poolbalance;";
            using (MySqlConnection dbConn = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                MySqlCommand cmd = new MySqlCommand("SELECT coin, poolbalance FROM summary;", dbConn);
                dbConn.Open();
                DataTable dataTable = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                da.Fill(dataTable);

                return dataTable;
            }            
            
        }

        public static void InsertPoolInfo(Coin c)
        {
            string sql = "INSERT INTO summary(coin, poolbalance) VALUES (@coin, @poolbalance) ON DUPLICATE KEY UPDATE poolbalance = @poolbalance;";            
            using (MySqlConnection dbConn = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                try
                {
                    dbConn.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, dbConn);
                    cmd.Prepare();

                    //cmd.Parameters.AddWithValue("@coin", c.coinName);
                    //cmd.Parameters.AddWithValue("@poolbalance", c.balance);
                    cmd.Parameters.Add("@coin", MySqlDbType.VarChar, 4).Value = c.coinName;
                    cmd.Parameters.Add("@poolbalance", MySqlDbType.Decimal).Value = c.balance;

                    cmd.ExecuteNonQuery();

                    dbConn.Close();
                }
                catch (MySqlException e)
                {
                    Console.WriteLine("error: " + e.ToString());
                }
            }    
        }
    }
}