using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.html;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Globalization;
namespace FFHPWeb
{
    public partial class partner_payment_transaction : System.Web.UI.Page
    {
        string sqlcon = System.Configuration.ConfigurationManager.AppSettings["PartnerSqlConnectionString"].ToString();
        string conn = System.Configuration.ConfigurationManager.AppSettings["Connection"].ToString();
        string queryString = "";
        MySqlDataAdapter DA;
        string s = "";
        bool t = false;
        string errormsg = "Try again";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindpartner();
            }
            lblerror.Text = "";
        }
        private void bindpartner()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(sqlcon);
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_ffhp_partner", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("ffhp_partner_id", 0);
                cmd.Parameters.AddWithValue("name", "");
                cmd.Parameters.AddWithValue("phone", "");
                cmd.Parameters.AddWithValue("email", "");
                cmd.Parameters.AddWithValue("password", "");
                cmd.Parameters.AddWithValue("address", "");
                cmd.Parameters.AddWithValue("operation", "select");
                SqlParameter outputparam = new SqlParameter();
                outputparam.ParameterName = "@output";
                outputparam.DbType = DbType.Int32;
                outputparam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputparam);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
                ViewState["partnerdetails"] = dt;
                var distinctValues = dt.AsEnumerable()
                        .Select(row => new
                        {
                            ffhp_partner_id = row.Field<int>("ffhp_partner_id"),
                            name = row.Field<string>("name"),
                            phone = row.Field<string>("phone"),
                            email = row.Field<string>("email"),
                            address = row.Field<string>("address")
                        })
                        .Distinct();
                ddlpartner.Items.Clear();
                ddlpartner.DataSource = distinctValues;
                ddlpartner.Items.Add("--Select--");
                ddlpartner.DataBind();

            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
        }
        protected void ddlpartner_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlpartner.SelectedIndex > 0)
            {
                bindtransaction();
            }
        }
        public void bindtransaction()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(sqlcon);
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_partner_payment_transaction", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("partner_payment_transaction_id", 0);
                cmd.Parameters.AddWithValue("ffhp_partner_id", Convert.ToInt32(ddlpartner.SelectedValue.ToString()));
                cmd.Parameters.AddWithValue("payment_amount", 0);
                cmd.Parameters.AddWithValue("reference_no", 0);
                cmd.Parameters.AddWithValue("payment_date", DateTime.Now);
                cmd.Parameters.AddWithValue("operation", "selectwithpartnerid");
                SqlParameter outputparam = new SqlParameter();
                outputparam.ParameterName = "@output";
                outputparam.DbType = DbType.Int32;
                outputparam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputparam);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
                gvpaymenttransaction.DataSource = dt;
                gvpaymenttransaction.DataBind();
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
        }
        protected void btnsubmit_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (validate())
                {
                    SqlConnection con = new SqlConnection(sqlcon);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_partner_payment_transaction", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("partner_payment_transaction_id", 0);
                    cmd.Parameters.AddWithValue("ffhp_partner_id", Convert.ToInt32(ddlpartner.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("payment_amount", Convert.ToDecimal(tbxamount.Text.ToString().Trim()));
                    cmd.Parameters.AddWithValue("reference_no", tbxrefnumber.Text.ToString().Trim());

                    DateTime dtf = DateTime.ParseExact(TbxFromDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                    cmd.Parameters.AddWithValue("payment_date", dtf);
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
                        bindtransaction();
                        clear();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void clear()
        {
            ddlpartner.SelectedIndex = 0;
            tbxamount.Text = "";
            tbxrefnumber.Text = "";
            TbxFromDate.Text = "";
        }
        private bool validate()
        {
            bool t = false;
            if (ddlpartner.SelectedIndex > 0)
            {
                if (tbxamount.Text != "")
                {
                    if (tbxrefnumber.Text != "")
                    {
                        if (TbxFromDate.Text != "")
                        {
                            t = true;
                        }
                    }
                }
            }
            return t;
        }
    }
}
