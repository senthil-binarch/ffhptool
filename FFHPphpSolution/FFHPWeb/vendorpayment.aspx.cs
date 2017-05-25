using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Net.Mail;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using System.Web.UI.HtmlControls;

namespace FFHPWeb
{
    public partial class vendorpayment : System.Web.UI.Page
    {
        string conn = System.Configuration.ConfigurationManager.AppSettings["SqlConnectionString"].ToString();
        string queryString = "";
        MySqlDataAdapter DA;
        string s = "";
        bool t = false;
        string errormsg = "Try again";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindvendordetails();
            }
            lblerror.Text = "";
            lblerror.ForeColor = System.Drawing.Color.Black;
        }
        public void bindvendordetails()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(conn);
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_vendordetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("vendorid", 0);
                cmd.Parameters.AddWithValue("vendorname", "");
                cmd.Parameters.AddWithValue("telephone", "");
                cmd.Parameters.AddWithValue("operation", "select");
                SqlParameter outputparam = new SqlParameter();
                outputparam.ParameterName = "@output";
                outputparam.DbType = DbType.Int32;
                outputparam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputparam);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
                ddlvendor.DataSource = dt;
                ddlvendor.Items.Add("--Select--");
                ddlvendor.DataBind();
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
        }
        protected void ddlvendor_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(conn);
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_vendorpayment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("paymentid", 0);
                cmd.Parameters.AddWithValue("vendorid", Convert.ToInt32(ddlvendor.SelectedValue.ToString()));
                cmd.Parameters.AddWithValue("last_balance_amount", 0);
                cmd.Parameters.AddWithValue("current_purchase_amount", 0);
                cmd.Parameters.AddWithValue("paid_amount", 0);
                cmd.Parameters.AddWithValue("payment_mode", "");
                cmd.Parameters.AddWithValue("operation", "vendor_balance_currentprice");
                SqlParameter outputparam = new SqlParameter();
                outputparam.ParameterName = "@output";
                outputparam.DbType = DbType.Int32;
                outputparam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputparam);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
        }
    }
}
