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
//using Renci.SshNet;
namespace FFHPWeb
{
    public partial class partner_dashboard : System.Web.UI.Page
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
        private void bindpartner_couponcode(int partnerid)
        {
            ddlcoupon.Items.Clear();
            ddlcoupon.DataSource = get_assigned_couponcode_with_partner(partnerid);
            ddlcoupon.Items.Add("--Select--");
            ddlcoupon.DataBind();
        }
        private DataTable get_assigned_couponcode_with_partner(int partnerid)
        {
            DataTable dt = new DataTable();
            try
            {

                SqlConnection con = new SqlConnection(sqlcon);
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_ffhp_partner_coupon", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("ffhp_partner_coupon_id", 0);
                cmd.Parameters.AddWithValue("ffhp_partner_id", partnerid);
                cmd.Parameters.AddWithValue("coupon_code", "");
                cmd.Parameters.AddWithValue("operation", "select_assigned_coupon_with_partner");
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
            return dt;
        }
        public void get_customer_with_coupon(string coupon_code)
        {
            queryString = @"SELECT a.customer_id, a.customer_firstname, a.customer_lastname, a.customer_email,a.coupon_code, z.base_grand_total, z.increment_id, z.created_at
FROM `sales_flat_order_grid` AS z
INNER JOIN `sales_flat_order` AS a ON z.entity_id = a.entity_id
WHERE a.coupon_code = '" + coupon_code + "'";
            if (queryString != "")
            {
                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                DataSet customerorderlist = new DataSet();
                adapteradminmail.Fill(customerorderlist, "customerdetails");
                gvpartnercoupon.DataSource = customerorderlist;
                gvpartnercoupon.DataBind();
            }
        }
        protected void ddlpartner_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlpartner.SelectedIndex != 0)
            {
                bindpartner_couponcode(Convert.ToInt32(ddlpartner.SelectedValue.ToString()));
            }
            else
            {
                lblerror.Text = "Please select a partner";
            }
            
        }
        protected void ddlcoupon_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlcoupon.SelectedIndex != 0)
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                get_customer_with_coupon(ddlcoupon.SelectedItem.Text.ToString());
                //sshobj.SshDisconnect(client);
            }
        }
    }
}
