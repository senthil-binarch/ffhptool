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
    public partial class vendor_details : System.Web.UI.Page
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
                //test();
                bindvendordetails();
            }
            lblerror.Text = "";
        }
        public void test()
        {
            test obj = new test();
 
            DataTable products = obj.ProductsTable;
            DataTable details = obj.DetailsTable;

            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("ID", typeof(int));
            dtResult.Columns.Add("Product", typeof(string));

            var items= (from p in products.AsEnumerable()
            join t in details.AsEnumerable()
            on p.Field<int>("Id") equals t.Field<int>("Id")
            //where p.Field<int>("Id")==1
            select dtResult.LoadDataRow(new object[]
            {
                p.Field<int>("ID"),
                p.Field<string>("Product"),
            },false));
            DataTable dt = items.CopyToDataTable();
            // var dataRow=DataTable.AsEnumerable().Where(x => x.Field<int>("Id") == 2).FirstOrDefault();
             
            
            //foreach(var item in items)
            //{
            //var name=item.ProductName;
            //}
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
                gvvendordetails.DataSource = dt;
                gvvendordetails.DataBind();
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
        }
        public bool validation()
        {
            bool t = false;
            if (tbxvendorname.Text != "")
            {
                if (tbxtelephone.Text != "")
                {
                    t = true;
                }
                else
                {
                    lblerror.Text = "Please enter Telephone";
                }
            }
            else
            {
                lblerror.Text = "Please enter Vendor Name";
            }
            return t;
        }
        protected void btnsave_OnClick(object sender, EventArgs e)
        {
            if (validation())
            {
                saveupdate();
            }
        }
        void saveupdate()
        {
            try
            {
                SqlConnection con = new SqlConnection(conn);
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_vendordetails", con);
                cmd.CommandType = CommandType.StoredProcedure;


                if (btnsave.Text == "Save")
                {
                    cmd.Parameters.AddWithValue("vendorid", 0);
                    cmd.Parameters.AddWithValue("vendorname", tbxvendorname.Text.ToString().Trim());
                    cmd.Parameters.AddWithValue("telephone", tbxtelephone.Text.ToString().Trim());
                    cmd.Parameters.AddWithValue("operation", "insert");
                }
                else if (btnsave.Text == "Update")
                {
                    cmd.Parameters.AddWithValue("vendorid", Convert.ToInt32(hfvendorid.Value.ToString()));
                    cmd.Parameters.AddWithValue("vendorname", tbxvendorname.Text.ToString().Trim());
                    cmd.Parameters.AddWithValue("telephone", tbxtelephone.Text.ToString().Trim());
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
                if (btnsave.Text == "Save")
                {
                    if (i > 0)
                    {
                        lblerror.Text = "Successfully Saved";
                        bindvendordetails();
                        clear();
                    }
                    else
                    {
                        lblerror.Text = "Not Successfully Saved, already exist.";
                    }
                }
                else if (btnsave.Text == "Update")
                {
                    if (i > 0)
                    {
                        lblerror.Text = "Successfully Update";
                        bindvendordetails();
                        clear();
                    }
                    else
                    {
                        lblerror.Text = "Not Successfully Update";
                    }
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
        }
        protected void btnedit_OnClick(object sender, EventArgs e)
        {
            GridViewRow row = (GridViewRow)(((Control)sender).NamingContainer);
            int vendorid = Convert.ToInt32(gvvendordetails.Rows[row.RowIndex].Cells[1].Text.ToString());
            string vendorname = gvvendordetails.Rows[row.RowIndex].Cells[2].Text.ToString();
            string telephone = gvvendordetails.Rows[row.RowIndex].Cells[3].Text.ToString();

            tbxvendorname.Text = vendorname.ToString();
            tbxtelephone.Text = telephone.ToString();
            hfvendorid.Value = vendorid.ToString();

            btnsave.Text = "Update";

        }
        void clear()
        {
            tbxvendorname.Text = "";
            tbxtelephone.Text = "";
            hfvendorid.Value = "";
            btnsave.Text = "Save";
        }
        protected void btnback_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("vendor_products.aspx", false);
        }
        protected void btnclear_OnClick(object sender, EventArgs e)
        {
            clear();
        }
    }
}
