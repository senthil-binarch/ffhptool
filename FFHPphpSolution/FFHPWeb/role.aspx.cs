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
    public partial class role : System.Web.UI.Page
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
                bindrole();
            }
            lblerror.Text = "";
        }
        protected void btnadd_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (validation())
                {
                    SqlConnection con = new SqlConnection(conn);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_ffhprole", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("roleid", 0);
                    cmd.Parameters.AddWithValue("rolename", tbxrolename.Text.ToString().Trim());
                    cmd.Parameters.AddWithValue("rolestatus", "1");
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
                        lblerror.Text = "Save Successfully";
                        bindrole();
                    }
                    else
                    {
                        lblerror.Text = "The Role already exist.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
        }
        public bool validation()
        {
            bool t = false;

            if (tbxrolename.Text != "")
            {
                t = true;
            }
            else
            {
                lblerror.Text = "Please enter role name";
            }

            return t;
        }
        public void bindrole()
        {
            try
            {
                DataTable dt = new DataTable();
                try
                {
                    SqlConnection con = new SqlConnection(conn);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_ffhprole", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("roleid", 0);
                    cmd.Parameters.AddWithValue("rolename", "");
                    cmd.Parameters.AddWithValue("rolestatus", "1");
                    cmd.Parameters.AddWithValue("operation", "select");
                    SqlParameter outputparam = new SqlParameter();
                    outputparam.ParameterName = "@output";
                    outputparam.DbType = DbType.Int32;
                    outputparam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outputparam);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                    con.Close();
                    gvrole.DataSource = dt;
                    gvrole.DataBind();
                }
                catch (Exception ex)
                {
                    lblerror.Text = errormsg.ToString();
                }
            }
            catch (Exception ex)
            {
            }
        }
        protected void gvrole_OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            gvrole.EditIndex = e.NewEditIndex;
            bindrole();
        }
        protected void gvrole_OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvrole.EditIndex = -1;
            bindrole();
        }
        protected void gvrole_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = (GridViewRow)gvrole.Rows[e.RowIndex]; 
                TextBox editrolename = (TextBox)row.FindControl("tbxeditrolename");

                HiddenField hfroleid=(HiddenField)row.FindControl("hfroleid");
                SqlConnection con = new SqlConnection(conn);
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_ffhprole", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("roleid", Convert.ToInt32(hfroleid.Value.ToString()));
                cmd.Parameters.AddWithValue("rolename", editrolename.Text.ToString().Trim());
                cmd.Parameters.AddWithValue("rolestatus", "1");
                cmd.Parameters.AddWithValue("operation", "update");

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
                    lblerror.Text = "Updated Successfully";
                    gvrole.EditIndex = -1;
                    bindrole();
                }
                else
                {
                    lblerror.Text = "Not Updated Successfully";
                }
                
            }
            catch (Exception ex)
            {
            }
        }
    }
}
