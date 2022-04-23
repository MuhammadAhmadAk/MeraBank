using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MeraBank.Models.DL
{
    public class Helper
    {
        static SqlConnection con = new SqlConnection("server=DESKTOP-5PA2S0V;database=MeraBank;integrated security=true");
        static public bool ExecuteQuery(string s)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(s, con);
                con.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                con.Close();
                return false;

            }
            finally
            {
                con.Close();

            }

        }
        static public DataTable ExecuteTable(string s)
        {
            SqlDataAdapter da = new SqlDataAdapter(s, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

    }
}