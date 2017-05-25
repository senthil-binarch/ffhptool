using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
//using System.Web.Mail;
using System.Net.Mail;
using iTextSharp.tool.xml;
using Excel = Microsoft.Office.Interop.Excel;
using System.Text.RegularExpressions;
using System.Globalization;
//using Renci.SshNet;
namespace FFHPWeb
{
    public partial class CouponCustomer : System.Web.UI.Page
    {
        string conn = System.Configuration.ConfigurationManager.AppSettings["Connection"].ToString();
        string queryString = "";
        MySqlDataAdapter DA;
        string errormsg = "Try again";
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void bindcouponcodecustomer()
        {
            try
            {
                if (txtcouponcode.Text != "")
                {
                    queryString = "SELECT increment_id,STATUS , coupon_code, customer_id, DATE_FORMAT( created_at, '%d/%m/%Y' ) AS ordered_date FROM `sales_flat_order`WHERE coupon_code ='" + txtcouponcode.Text.ToString().Trim() + "'";
                    if (queryString != "")
                    {
                        MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                        DataSet couponcustomer = new DataSet();
                        adapteradminmail.Fill(couponcustomer, "sales_flat_order_free_item");
                        gvcouponcodecustomers.DataSource = couponcustomer;
                        gvcouponcodecustomers.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnsubmit_OnClick(object sender, EventArgs e)
        {
            //ConnectSsh sshobj = new ConnectSsh();
            //SshClient client = sshobj.SshConnect();
            bindcouponcodecustomer();
            //sshobj.SshDisconnect(client);
        }
    }
}
