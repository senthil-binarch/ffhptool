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
using System.Globalization;

namespace FFHPWeb
{
    public partial class purchase_entry_report : System.Web.UI.Page
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
                bindproduct();
                bindvendordetails();
            }
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
                ViewState["products"] = dt;
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
                DateTime dtf = DateTime.ParseExact(TbxFromDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                DateTime dtt = DateTime.ParseExact(TbxToDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                if (ddlvendor.SelectedIndex > 0)
                {
                    using (SqlConnection con = new SqlConnection(conn))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_purchase_entry_report", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@fromdate", SqlDbType.DateTime).Value = Convert.ToDateTime(dtf);
                            cmd.Parameters.Add("@todate", SqlDbType.DateTime).Value = Convert.ToDateTime(dtt);
                            cmd.Parameters.Add("@vendorid", SqlDbType.Int).Value = Convert.ToInt32(ddlvendor.SelectedValue.ToString());
                            cmd.Parameters.Add("@productid", SqlDbType.Int).Value = 0;
                            cmd.Parameters.Add("@operation", SqlDbType.VarChar).Value = "selectbyvendorporducts";
                            con.Open();
                            SqlDataAdapter sda = new SqlDataAdapter(cmd);
                            sda.Fill(dt);
                            con.Close();
                        }
                    }
                }
                else
                {
                    dt=(DataTable)ViewState["products"];
                }
                
                var result = (from DataRow dRow in dt.Rows
                              select new { productid = dRow["productid"].ToString(), name = dRow["name"].ToString() }).Distinct();
                var distinctValues = dt.AsEnumerable()
                        .Select(row => new
                        {
                            productid = row.Field<int>("productid"),
                            pidname = row.Field<string>("name")
                        })
                        .Distinct();
                ddlproduct.Items.Clear();
                ddlproduct.DataSource = dt;
                ddlproduct.Items.Add("--Select--");
                ddlproduct.DataBind();
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }

        }
        protected void btnsubmit_OnClick(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            try
            {
                DateTime dtf = DateTime.ParseExact(TbxFromDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                DateTime dtt = DateTime.ParseExact(TbxToDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                
                    using (SqlConnection con = new SqlConnection(conn))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_purchase_entry_report", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@fromdate", SqlDbType.DateTime).Value =dtf; //Convert.ToDateTime(dtf.ToString("yyyy-MM-dd"));
                            cmd.Parameters.Add("@todate", SqlDbType.DateTime).Value = dtt; //Convert.ToDateTime(dtt.ToString("yyyy-MM-dd"));
                            if (ddlvendor.SelectedIndex > 0)
                            {
                                cmd.Parameters.Add("@vendorid", SqlDbType.Int).Value = Convert.ToInt32(ddlvendor.SelectedValue.ToString());
                            }
                            else
                            {
                                cmd.Parameters.Add("@vendorid", SqlDbType.Int).Value = 0;
                            }
                            if (ddlproduct.SelectedIndex > 0)
                            {
                                cmd.Parameters.Add("@productid", SqlDbType.Int).Value = Convert.ToInt32(ddlproduct.SelectedValue.ToString());
                            }
                            else
                            {
                                cmd.Parameters.Add("@productid", SqlDbType.Int).Value = 0;
                            }
                            if (ddlvendor.SelectedIndex > 0 && ddlproduct.SelectedIndex > 0)
                            {
                                cmd.Parameters.Add("@operation", SqlDbType.VarChar).Value = "reportinputs4";
                            }
                            else if (ddlvendor.SelectedIndex > 0)
                            {
                                cmd.Parameters.Add("@operation", SqlDbType.VarChar).Value = "reportinputs2";
                            }
                            else if (ddlproduct.SelectedIndex > 0)
                            {
                                cmd.Parameters.Add("@operation", SqlDbType.VarChar).Value = "reportinputs3";
                            }
                            else
                            {
                                cmd.Parameters.Add("@operation", SqlDbType.VarChar).Value = "reportinputs1";
                            }
                            //cmd.Parameters.Add("@operation", SqlDbType.VarChar).Value = "selectbyvendorporducts";
                            con.Open();
                            SqlDataAdapter sda = new SqlDataAdapter(cmd);
                            sda.Fill(dt);
                            con.Close();
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        var sum = dt.AsEnumerable().Sum(dr => dr.Field<decimal>("price"));
                        gvpurchasereport.ShowFooter = true;
                        gvpurchasereport.DataSource = dt;
                        gvpurchasereport.DataBind();
                        gvpurchasereport.FooterRow.Cells[10].Text = sum.ToString("0.00");
                    }
                    else
                    {
                        gvpurchasereport.DataSource = dt;
                        gvpurchasereport.DataBind();
                    }
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
        }
        protected void gvpurchasereport_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Convert.ToDouble(e.Row.Cells[6].Text.ToString()) > 0)
                {
                    e.Row.Cells[6].Style.Add("Color", "Red");
                }
            }
        }
    }
}
