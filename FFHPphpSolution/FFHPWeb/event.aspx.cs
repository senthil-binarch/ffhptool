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

namespace FFHPWeb
{
    public partial class _event : System.Web.UI.Page
    {
        string conn = System.Configuration.ConfigurationManager.AppSettings["SqlConnectionString"].ToString();
        string queryString = "";
        MySqlDataAdapter DA;
        string s = "";
        bool t = false;
        string errormsg = "Try again";
        protected void Page_Load(object sender, EventArgs e)
        {
            lblerror.Text = "";
        }
        protected void btnsubmit_OnClick(object sender, EventArgs e)
        {
            save();
            clear();
        }
        protected void btnclear_OnClick(object sender, EventArgs e)
        {
            clear();
        }
        void clear()
        {
            tbxname.Text = "";
            tbxemail.Text = "";
            tbxphone.Text = "";
            tbxcomment.Text = "";
        }
        void save()
        {
            try
            {
                SqlConnection con = new SqlConnection(conn);
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_event_users", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("eventid", 0);
                cmd.Parameters.AddWithValue("eventname", "");
                cmd.Parameters.AddWithValue("name", tbxname.Text.ToString().Trim());
                cmd.Parameters.AddWithValue("email", tbxemail.Text.ToString().Trim());
                cmd.Parameters.AddWithValue("phone", tbxphone.Text.ToString().Trim());
                cmd.Parameters.AddWithValue("comments", tbxcomment.Text.ToString().Trim());
                cmd.Parameters.AddWithValue("operation", "insert");

                SqlParameter outputparam = new SqlParameter();
                outputparam.ParameterName = "@output";
                outputparam.DbType = DbType.Int32;
                outputparam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputparam);
                cmd.ExecuteNonQuery();
                int i = Convert.ToInt32(cmd.Parameters["@output"].Value.ToString());
                con.Close();
                if (i > 0)
                {
                    lblerror.Text = "Successfully Saved";
                    //bindeventusers();
                    clear();
                }
                else
                {
                    lblerror.Text = "Not Successfully Saved";
                }

            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
        }
        public void bindeventusers()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(conn);
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_event_users", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("eventid", 0);
                cmd.Parameters.AddWithValue("eventname", "");
                cmd.Parameters.AddWithValue("name", "");
                cmd.Parameters.AddWithValue("email", "");
                cmd.Parameters.AddWithValue("phone", "");
                cmd.Parameters.AddWithValue("comments", "");
                cmd.Parameters.AddWithValue("operation", "select");
                SqlParameter outputparam = new SqlParameter();
                outputparam.ParameterName = "@output";
                outputparam.DbType = DbType.Int32;
                outputparam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputparam);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
                gveventusers.DataSource = dt;
                gveventusers.DataBind();
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
        }
        protected void btneventusers_OnClick(object sender, EventArgs e)
        {
            if (gveventusers.Visible == true)
            {
                gveventusers.Visible = false;
            }
            else
            {
                bindeventusers();
                gveventusers.Visible = true;
            }
        }
    }
}
