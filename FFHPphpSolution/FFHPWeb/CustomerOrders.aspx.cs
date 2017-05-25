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
    public partial class CustomerOrders : System.Web.UI.Page
    {
        string conn = System.Configuration.ConfigurationManager.AppSettings["Connection"].ToString();
        string queryString = "";
        MySqlDataAdapter DA;
        string errormsg = "Try again";
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void bindCustomerOrderlist()
        {
            try
            {
                if (txtcustomerid.Text != "")
                {
                    queryString = "SELECT a.billing_name, b.increment_id, DATE_FORMAT( b.created_at, '%d/%m/%Y' ) AS ordered_date, b.status FROM `sales_flat_order_grid` AS a JOIN sales_flat_order AS b ON a.entity_id = b.entity_id WHERE a.customer_id ='" + txtcustomerid.Text.ToString().Trim() + "'";
                    if (queryString != "")
                    {
                        MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                        DataSet couponcustomer = new DataSet();
                        adapteradminmail.Fill(couponcustomer, "sales_flat_order_free_item");
                        gvcustomerlist.DataSource = couponcustomer;
                        gvcustomerlist.DataBind();
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
            bindCustomerOrderlist();
            //sshobj.SshDisconnect(client);
        }
    }
}
