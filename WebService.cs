using System;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Services;
using System.Web.Services.Protocols;
using AjaxControlToolkit;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService {

    public WebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public CascadingDropDownNameValue[] GetCity(string knownCategoryValues, string category)
    {
        string con = ConfigurationManager.ConnectionStrings["EladSysConnectionString"].ToString();
        SqlConnection sql_con = new SqlConnection (con);
        sql_con.Open();
        SqlCommand comm = new SqlCommand("SELECT * FROM city",sql_con);
        SqlDataReader dr = comm.ExecuteReader();

        List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
        while (dr.Read())
        {
            values.Add(new CascadingDropDownNameValue
            {
                value = dr[0].ToString(),
                name = dr[1].ToString()

            });
        }
        dr.Close();
        sql_con.Close();
        return values.ToArray();
    }

    [WebMethod]
    public CascadingDropDownNameValue[] GetStreet(string knownCategoryValues, string category)
    {
        StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
        int city_id;
        if (!kv.ContainsKey("city") || !Int32.TryParse(kv["city"], out city_id))
        {
            return null;
        }
        string con = ConfigurationManager.ConnectionStrings["EladSysConnectionString"].ToString();
        SqlConnection sql_con = new SqlConnection(con);
        sql_con.Open();
        SqlCommand comm = new SqlCommand("SELECT * FROM street where city_id=" + city_id, sql_con);
        SqlDataReader dr = comm.ExecuteReader();

        List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
        while (dr.Read())
        {
            values.Add(new CascadingDropDownNameValue
            {
                value = dr[0].ToString(),
                name = dr[1].ToString()

            });
        }
        dr.Close();
        sql_con.Close();
        return values.ToArray();
    }
    
}
