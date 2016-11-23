using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;
using System.Web.Script.Services;
public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    [ScriptMethod]
    public static string  SaveAddress(Address address)
    {
        //Check if values (city & street ) existence
        int city_id, street_id, status=0;
        object obj;
        string city = address.city, street = address.street, status_message;
        string con = ConfigurationManager.ConnectionStrings["EladSysConnectionString"].ConnectionString;
        SqlConnection sql_con = new SqlConnection(con);
        SqlCommand cmd01 = new SqlCommand("SELECT id FROM city WHERE name =  '" + city + "'");
        cmd01.CommandType = CommandType.Text;
        cmd01.Connection = sql_con;
        sql_con.Open();
        obj = cmd01.ExecuteScalar();
        if (obj != null)
            {
                if ( int.TryParse(obj.ToString(), out city_id))
                {
                    status = 1; 
                }
            }
        SqlCommand cmd02 = new SqlCommand("SELECT id FROM street WHERE name =  '" + street + "'");
        cmd02.CommandType = CommandType.Text;
        cmd02.Connection = sql_con;
        obj = cmd02.ExecuteScalar();
            if (obj != null)
            {
                if (int.TryParse(obj.ToString(), out street_id))
                {
                    status = 2;
                }

            }
        //Insert  values (city & street ) if true  
        if (status == 0) 
        {
            SqlCommand cmd03 = new SqlCommand("INSERT INTO city VALUES(@city)");
            cmd03.CommandType = CommandType.Text;
            cmd03.Parameters.AddWithValue("@city", address.city);
            cmd03.Connection = sql_con;
            cmd03.ExecuteNonQuery();
            SqlCommand cmd04 = new SqlCommand("SELECT id FROM city WHERE name =  '" + address.city + "'");
            cmd04.CommandType = CommandType.Text;
            cmd04.Connection = sql_con;
            obj = cmd04.ExecuteScalar();    
            if( int.TryParse(obj.ToString(), out city_id))
            {
                SqlCommand cmd05 = new SqlCommand("INSERT INTO street VALUES(@street, @city_id)");
                cmd05.CommandType = CommandType.Text;
                cmd05.Parameters.AddWithValue("@street", street);
                cmd05.Parameters.AddWithValue("@city_id", city_id);
                cmd05.Connection = sql_con;
                cmd05.ExecuteNonQuery();
            }
            
        }
        
        sql_con.Close();
        //Set return message  
        switch (status)
        {
            case 1:
                {
                    status_message = "ערך בשדה עיר קיים במערכת";
                    break;
                }
            case 2:
                {
                    status_message = "ערך בשדה רחוב קיים במערכת";
                    break;
                }
            default:
                {
                    status_message = "השדות נשמרו בהצלחה!";
                    break;
                }
        }
        return status_message;
    }
    
}