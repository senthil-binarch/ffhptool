using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using iTextSharp.tool.xml;
using Excel = Microsoft.Office.Interop.Excel;
using System.Globalization;
//using Renci.SshNet;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections;

namespace FFHPWeb
{
    public partial class collectionsheet : System.Web.UI.Page
    {//string conn = "server=68.178.143.107;userid=stagingffhpin;password=Stag1ngffHP!n;database=stagingffhpin;Convert Zero Datetime=True";  //staging
        //string conn = "server=68.178.143.11;userid=ffhpmagento;password=D6QpT!KDd0dKHI;database=ffhpmagento;Convert Zero Datetime=True";  //Live
        //string conn = "server=192.168.1.2;userid=;password=;database=ffhp;Convert Zero Datetime=True";  //Local
        string smsconn = System.Configuration.ConfigurationManager.AppSettings["SmsDBConnection"].ToString();
        string queryString = "";
        MySqlDataAdapter DA;
        string s = "";
        bool t = false;
        string errormsg = "Try again";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind_deliveryboy();
            }
        }
        public void Bind_deliveryboy()
        {
            DataSet ds = new DataSet();
            deliveryboy_order obj = new deliveryboy_order();
            ds = obj.getdriver_deliveryboylist();

            DataTable DTdeliveryboy = new DataTable();
            var resultdeliveryboy = from r in ds.Tables[0].AsEnumerable()
                                    where r["designation"].ToString() == "Delivery Boy"
                                    select r;
            DTdeliveryboy = resultdeliveryboy.CopyToDataTable();

            ddldeliveryboy.Items.Clear();
            ddldeliveryboy.Items.Add("-Select-");
            ddldeliveryboy.DataSource = DTdeliveryboy;
            ddldeliveryboy.DataTextField = "name";
            ddldeliveryboy.DataValueField = "ddid";
            ddldeliveryboy.DataBind();
        }
        protected void btnsubmit_OnClick(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = get_collection_sheet();
                gvcollectionsheet.DataSource = ds;
                gvcollectionsheet.DataBind();

                decimal sumofB2B = ds.Tables[0].AsEnumerable()
            .Where(r => (String)r["payment_mode"] == "B2B" && (decimal)r["order_amount"] > 0 && (decimal)r["paid_amount"] == 0 && (string)r["payment_status"] == "close" && (string)r["delivery_status"] == "close")
            .Sum(r => (decimal)r["billed_amount"]);
                gvcollectionsheet.FooterRow.Cells[3].Text = sumofB2B.ToString();

                decimal sumofpp = ds.Tables[0].AsEnumerable()
            .Where(r => (String)r["payment_mode"] == "PP")
            .Sum(r => (decimal)r["billed_amount"]);
                gvcollectionsheet.FooterRow.Cells[5].Text = sumofpp.ToString();

                decimal sumofcod = ds.Tables[0].AsEnumerable()
            .Where(r => (String)r["payment_mode"] == "COD")
            .Sum(r => (decimal)r["billed_amount"]);
                gvcollectionsheet.FooterRow.Cells[7].Text = sumofcod.ToString();

                decimal sumofebs = ds.Tables[0].AsEnumerable()
            .Where(r => (String)r["payment_mode"] == "EBS")
            .Sum(r => (decimal)r["billed_amount"]);
                gvcollectionsheet.FooterRow.Cells[9].Text = sumofebs.ToString();

                decimal sumofrefund = ds.Tables[0].AsEnumerable()
                    .Where(r => (String)r["payment_mode"] == "EBS" && (decimal)r["ebsrefund"] < 0)
            .Sum(r => (decimal)r["ebsrefund"]);
                gvcollectionsheet.FooterRow.Cells[11].Text = sumofrefund.ToString();

                decimal sumofebsextra = ds.Tables[0].AsEnumerable()
                    .Where(r => (String)r["payment_mode"] == "EBS" && (decimal)r["ebsrefund"] >=50)
            .Sum(r => (decimal)r["ebsrefund"]);
                gvcollectionsheet.FooterRow.Cells[13].Text = sumofebsextra.ToString();
            }
            catch (Exception ex)
            {

            }
        }
        public DataSet get_collection_sheet()
        {

            DataSet ffhp_delivery_status = new DataSet();
            //DateTime dtf = DateTime.ParseExact(TbxFromDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                ArrayList ordernumber;
                if (ddldeliveryboy.SelectedIndex>0)
                {
                    //ConnectSsh sshobj = new ConnectSsh();
                    //SshClient client = sshobj.SshConnect();
                    try
                    {
                    ordernumber = get_deliveryboy_ordernumber(ddldeliveryboy.SelectedItem.Text.ToString());
                    queryString = @"SELECT customer_id,
customer_address_id,
customer_name,
order_number,
route_number,
DATE_FORMAT( datescanned, '%m/%d/%Y' )as datescanned,
TIME_TO_SEC(timescanned)as timescanned,
order_amount,
CASE
WHEN order_amount=0 THEN 0 ELSE
round(billed_amount) END as billed_amount,
CASE 
WHEN payment_mode='COD' and order_amount>0 and paid_amount=0 and payment_status='close' and delivery_status='close' THEN 'B2B' 
WHEN order_amount=0 THEN 'Replacement' else payment_mode END as payment_mode,
no_of_bags,
latitude,
longitude,
area,
payment_status,
CASE WHEN paid_amount is null THEN 0 else paid_amount END as paid_amount,
IF(payment_mode='EBS',
CASE WHEN refund is null THEN billed_amount-order_amount ELSE refund END,
refund)as refund,
IF(payment_mode='EBS',billed_amount-order_amount,0)as ebsrefund,
TIME_TO_SEC(start_time) as start_time,
TIME_TO_SEC(delivery_time) as delivery_time,
duration,
kilometer,
deliveryboy_name,
deliveryboy_ph,
delivery_status,
discription,
DATE_FORMAT( date, '%m/%d/%Y' )as date 
FROM `ffhp_delivery_status` 
where order_number in (" + ordernumber[0].ToString() + ")" + @"

UNION ALL

SELECT customer_id,
customer_address_id,
customer_name,
order_number,
route_number,
DATE_FORMAT( datescanned, '%m/%d/%Y' )as datescanned,
TIME_TO_SEC(timescanned)as timescanned,
order_amount,
round(billed_amount)as billed_amount,
'PP' as payment_mode,
no_of_bags,
latitude,
longitude,
area,
payment_status,
CASE WHEN paid_amount is null THEN 0 else paid_amount END as paid_amount,
refund,
0 as ebsrefund,
TIME_TO_SEC(start_time) as start_time,
TIME_TO_SEC(delivery_time) as delivery_time,
duration,
kilometer,
deliveryboy_name,
deliveryboy_ph,
delivery_status,
discription,
DATE_FORMAT( date, '%m/%d/%Y' )as date 
FROM `ffhp_delivery_status` 
where order_number in (" + ordernumber[1].ToString() + ")";

                    //where date ='" + String.Format("{0:yyyy/MM/dd}", dtf) + "' ORDER BY route_number, payment_mode DESC";


                    if (queryString != "")
                    {

                        MySqlConnection con = new MySqlConnection(smsconn);
                        con.Open();

                        MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, smsconn);

                        adapteradminmail.Fill(ffhp_delivery_status, "ffhp_delivery_status");
                        con.Close();


                    }
                    //sshobj.SshDisconnect(client);
                    }
                    catch (Exception ex)
                    {
                        //sshobj.SshDisconnect(client);
                    }
                }
            
            return ffhp_delivery_status;
        }
        public DataSet get_deliveryboy_routelistone(string _deliveryboy_name)
        {
            string queryString = "";
            DataSet ordenumberlist = new DataSet();
            queryString = @"SELECT route_id,route_name,deliveryboy_ordernumber,payment_pending_ordernumber,driver_name,driver_phone,deliveryboy_name,deliveryboy_phone FROM `ffhp_route_orders` where deliveryboy_name='" + _deliveryboy_name+"'";
            if (queryString != "")
            {
                MySqlConnection con = new MySqlConnection(smsconn);
                con.Open();

                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, smsconn);

                adapteradminmail.Fill(ordenumberlist, "RouteName");
                con.Close();
            }
            return ordenumberlist;
        }
        public ArrayList get_deliveryboy_ordernumber(string deliveryboyname)
        {
            ArrayList al = new ArrayList();
           
            string deliveryboy_ordernumber = "0";
            string deliveryboy_paymentpending = "0";
            DataSet ds=new DataSet();

            ds = get_deliveryboy_routelistone(deliveryboyname);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["deliveryboy_ordernumber"].ToString().Trim() != "")
                    {
                        deliveryboy_ordernumber = ds.Tables[0].Rows[0]["deliveryboy_ordernumber"].ToString();
                        
                    }
                    if (ds.Tables[0].Rows[0]["payment_pending_ordernumber"].ToString().Trim() != "")
                    {
                        //if (deliveryboy_ordernumber != "")
                        //{
                        //    deliveryboy_ordernumber = "," + ds.Tables[0].Rows[0]["payment_pending_ordernumber"].ToString().Trim();
                        //}
                        //else
                        //{
                        //    deliveryboy_ordernumber = ds.Tables[0].Rows[0]["payment_pending_ordernumber"].ToString().Trim();
                        //}
                        deliveryboy_paymentpending = ds.Tables[0].Rows[0]["payment_pending_ordernumber"].ToString().Trim();
                        
                    }
                }
            }
            al.Add(deliveryboy_ordernumber);
            al.Add(deliveryboy_paymentpending);
            //return deliveryboy_ordernumber;
            return al;
        }
    }
}
