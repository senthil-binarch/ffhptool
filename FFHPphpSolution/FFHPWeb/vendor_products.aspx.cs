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
    public partial class vendor_products : System.Web.UI.Page
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
                bindproduct();
            }
            lblerror.Text = "";
            lblerror.ForeColor = System.Drawing.Color.Black;
        }
        public void bindproduct()
        {

            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(conn);
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_products", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("productid", 0);
                cmd.Parameters.AddWithValue("name", "");
                cmd.Parameters.AddWithValue("operation", "select");
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
                //APIMethods objmobileorders = new APIMethods();
                //dt = objmobileorders.getProduct_Weight_Price_for_Before_After_Sale();


                //dtpurchase.Columns.Add("purchase_date", typeof(DateTime), System.DateTime.Now.ToShortDateString());
                //gvpurchase.DataSource = dt;
                //gvpurchase.DataBind();
                ddlproduct.DataSource = dt;
                ddlproduct.Items.Add("--Select--");
                ddlproduct.DataBind();
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }

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
            Bindvendorproducts();
        }
        void Bindvendorproducts()
        {
            DataTable dt = new DataTable();
            try
            {
                if (ddlvendor.SelectedIndex > 0)
                {
                    SqlConnection con = new SqlConnection(conn);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_vendor_products", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("id", 0);
                    cmd.Parameters.AddWithValue("vendorid", Convert.ToInt32(ddlvendor.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("productid", 0);
                    cmd.Parameters.AddWithValue("status", 0);
                    cmd.Parameters.AddWithValue("operation", "select_vendorwise");
                    SqlParameter outputparam = new SqlParameter();
                    outputparam.ParameterName = "@output";
                    outputparam.DbType = DbType.Int32;
                    outputparam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outputparam);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                    con.Close();
                    gvvendorproducts.DataSource = dt;
                    gvvendorproducts.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
        }
        protected void btnadd_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (ddlvendor.SelectedIndex > 0)
                {
                    if (ddlproduct.SelectedIndex > 0)
                    {
                        SqlConnection con = new SqlConnection(conn);
                        con.Open();
                        SqlCommand cmd = new SqlCommand("sp_vendor_products", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("id", 0);
                        cmd.Parameters.AddWithValue("vendorid", Convert.ToInt32(ddlvendor.SelectedValue.ToString()));
                        cmd.Parameters.AddWithValue("productid", Convert.ToInt32(ddlproduct.SelectedValue.ToString()));
                        cmd.Parameters.AddWithValue("status", 0);
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
                            Bindvendorproducts();
                        }
                        else
                        {
                            lblerror.Text = ddlproduct.SelectedItem.Text.ToString() + " already exist.";
                        }
                        
                    }
                    else
                    {
                        lblerror.Text = "Please select a product";
                    }
                }
                else
                {
                    lblerror.Text = "Please select a vendor";
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
        }
        protected void gvvendorproducts_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //TextBox tbxbilledweight = (TextBox)gvvendorproducts.Rows[e.Row.RowIndex].FindControl("tbxweight");
                //TextBox tbxprice = (TextBox)gvvendorproducts.Rows[e.Row.RowIndex].FindControl("tbxprice");
                HiddenField hfid = (HiddenField)e.Row.FindControl("hfid");
                HiddenField hfstatus = (HiddenField)e.Row.FindControl("hfstatus");
                Button btnsave = (Button)e.Row.FindControl("btnsave");
                if (hfstatus.Value == "1")
                {
                    btnsave.Text = "Enabled";
                }
                else
                {
                    btnsave.Text = "Disabled";
                }
            }
        }
        protected void btnsave_OnClick(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = (GridViewRow)(((Control)sender).NamingContainer);
                HiddenField hfid = (HiddenField)gvvendorproducts.Rows[row.RowIndex].FindControl("hfid");
                Button btnsave = (Button)gvvendorproducts.Rows[row.RowIndex].FindControl("btnsave");
                SqlConnection con = new SqlConnection(conn);
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_vendor_products", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("id", Convert.ToInt32(hfid.Value.ToString()));
                cmd.Parameters.AddWithValue("vendorid", Convert.ToInt32(ddlvendor.SelectedValue.ToString()));
                cmd.Parameters.AddWithValue("productid", Convert.ToInt32(gvvendorproducts.Rows[row.RowIndex].Cells[1].Text.ToString()));
                if (btnsave.Text == "Enabled")
                {
                    cmd.Parameters.AddWithValue("status", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("status", 1);
                }
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
                    lblerror.Text = "Successfully Updated";
                }
                Bindvendorproducts();
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
        }
    }
}
