using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

namespace FFHPWeb
{
    public partial class OrderList : System.Web.UI.Page
    {
        string conn = "server=68.178.143.107;userid=stagingffhpin;password=Stag1ngffHP!n;database=stagingffhpin;Convert Zero Datetime=True";  //second
        string queryString = "";
        MySqlDataAdapter DA;
        string s = "";
        bool t = false;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnsubmit_OnClick(object sender, EventArgs e)
        {
            //queryString = "SELECT * FROM bb_email_config WHERE is_deleted='true' AND email_type_id='24'";
            queryString = "SELECT * FROM sales_flat_order_item WHERE order_id='287'";
            if (queryString != "")
            {
                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                DataSet orderlist = new DataSet();
                adapteradminmail.Fill(orderlist, "sales_flat_order_item");
                GridView1.DataSource = orderlist;
                GVOrder.DataBind();
                
            }
        }
    }
}
