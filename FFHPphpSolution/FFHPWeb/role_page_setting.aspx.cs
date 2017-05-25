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
    public partial class role_page_setting : System.Web.UI.Page
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
                    ddlrole.DataSource = dt;
                    ddlrole.Items.Clear();
                    ddlrole.Items.Add("--Select--");
                    ddlrole.DataBind();
                }
                catch (Exception ex)
                {
                    //sdf.Text = errormsg.ToString();
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void bindpages()
        {
            try
            {
                DataTable dt = new DataTable();
                try
                {
                    SqlConnection con = new SqlConnection(conn);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("Sp_role_page_ref", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("role_page_ref_id", 0);
                    cmd.Parameters.AddWithValue("roleid", Convert.ToInt32(ddlrole.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("pageid", 0);
                    cmd.Parameters.AddWithValue("operation", "select");
                    SqlParameter outputparam = new SqlParameter();
                    outputparam.ParameterName = "@output";
                    outputparam.DbType = DbType.Int32;
                    outputparam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outputparam);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                    con.Close();
                    gvpages.DataSource = dt;
                    gvpages.DataBind();
                }
                catch (Exception ex)
                {
                    //lblerror.Text = errormsg.ToString();
                }
            }
            catch (Exception ex)
            {
            }
        }
        protected void ddlrole_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlrole.SelectedIndex != -1)
            {
                bindpages();
            }
        }

        protected void gvpages_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            if (e.Row.RowType == DataControlRowType.DataRow && gvpages.EditIndex == e.Row.RowIndex)
            {
                CheckBox cbeditstatus = (CheckBox)e.Row.FindControl("cbeditstatus");
                HiddenField hfpageid = (HiddenField)e.Row.FindControl("hfpageid");
                //if (Convert.ToInt32((e.Row.FindControl("hfrefpageid") as HiddenField).Value) >0)
                if ((e.Row.FindControl("hfrefpageid") as HiddenField).Value!="")
                {
                    cbeditstatus.Checked = true;
                }
                else
                {
                    cbeditstatus.Checked = false;
                }
                

                //string s = (e.Row.FindControl("lblemail") as Label).Text.ToString();
                //string oripwd = Decrypt((e.Row.FindControl("hfeditpassword") as HiddenField).Value.ToString());
                //TextBox tbxeditpassword = (TextBox)e.Row.FindControl("tbxeditpassword");
                //tbxeditpassword.Text = oripwd;
            }
        }
        protected void gvpages_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvpages.EditIndex = e.NewEditIndex;
                bindpages();
            }
            catch (Exception ex)
            {
            }
        }
        protected void gvpages_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                CheckBox cbstatus = (CheckBox)gvpages.Rows[e.RowIndex].FindControl("cbeditstatus");
                HiddenField hfeditroleid = (HiddenField)gvpages.Rows[e.RowIndex].FindControl("hfeditroleid");
                HiddenField hfeditpageid = (HiddenField)gvpages.Rows[e.RowIndex].FindControl("hfpageid");
                HiddenField hfrole_page_ref_id = (HiddenField)gvpages.Rows[e.RowIndex].FindControl("hfrole_page_ref_id");

                SqlConnection con = new SqlConnection(conn);
                con.Open();
                SqlCommand cmd = new SqlCommand("Sp_role_page_ref", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (hfrole_page_ref_id.Value == "")
                {
                    cmd.Parameters.AddWithValue("role_page_ref_id", 0);
                    cmd.Parameters.AddWithValue("roleid", Convert.ToInt32(ddlrole.SelectedValue.ToString()));
                    if (cbstatus.Checked)
                    {
                        cmd.Parameters.AddWithValue("pageid", Convert.ToInt32(hfeditpageid.Value.ToString()));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("pageid", 0);
                    }
                    cmd.Parameters.AddWithValue("operation", "insert");
                }
                else
                {
                    cmd.Parameters.AddWithValue("role_page_ref_id", Convert.ToInt32(hfrole_page_ref_id.Value.ToString()));
                    cmd.Parameters.AddWithValue("roleid", Convert.ToInt32(hfeditroleid.Value.ToString()));
                    if (cbstatus.Checked)
                    {
                        cmd.Parameters.AddWithValue("pageid", Convert.ToInt32(hfeditpageid.Value.ToString()));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("pageid", 0);
                    }
                    cmd.Parameters.AddWithValue("operation", "update");
                }
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
                    //lblerror.Text = "Updated Successfully";
                    gvpages.EditIndex = -1;
                    bindpages();
                }
                else
                {
                    //lblerror.Text = "Not Updated Successfully";
                }

                //lblerror.ForeColor = System.Drawing.Color.Green;
                //lblerror.Text = "Updated successfully";
            }
            catch (Exception ex)
            {
            }
        }
        protected void gvpages_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvpages.EditIndex = -1;
            bindpages();
        }
    }
}
