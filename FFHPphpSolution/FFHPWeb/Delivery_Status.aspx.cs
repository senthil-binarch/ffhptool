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
//using Renci.SshNet;

namespace FFHPWeb
{
    public partial class Delivery_Status : System.Web.UI.Page
    {
        string smsconn = System.Configuration.ConfigurationManager.AppSettings["SmsDBConnection"].ToString();//"server=68.178.143.39;userid=ffhplog;password=FFHPl0g!;database=ffhplog;Convert Zero Datetime=True";  //sms connection
        string queryString = "";
        MySqlDataAdapter DA;
        string s = "";
        bool t = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Binddropdownroute();
            }
        }
        protected void btnsubmit_OnClick(object sender, EventArgs e)
        {
            try
            {
                DateTime dtf = DateTime.ParseExact(TbxFromDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                Get_Delivery_Status(dtf, dtf);
            }
            catch
            {
            }
        }
        public void Get_Delivery_Status(DateTime fromdate, DateTime todate)
        {
            try
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                DataTable dt = new DataTable();
                DataTable dtfilter = new DataTable();
                
                APIMethods objdeliverystatus = new APIMethods();
                dt = objdeliverystatus.Get_Delivery_Status(fromdate, todate).Tables[0];
                //sshobj.SshDisconnect(client);
                if (ddlroutelist.SelectedIndex != 0)
                {
                    var result = (from r in dt.AsEnumerable()
                                  where r["route_number"].ToString() == ddlroutelist.SelectedItem.Text.ToString()
                                  select r).ToList();

                    if (result.Count > 0)
                    {
                        dtfilter = result.CopyToDataTable();
                    }
                }
                else
                {
                    dtfilter = dt;
                }
                decimal _orderamount = 0;
                decimal _billedamount = 0;
                decimal _paidamount = 0;
                foreach (DataRow row in dtfilter.Rows)
                {
                    if (_orderamount == 0)
                    {
                        _orderamount = Convert.ToDecimal(row["order_amount"].ToString());
                    }
                    else
                    {
                        _orderamount = _orderamount + Convert.ToDecimal(row["order_amount"].ToString());
                    }

                    if (_billedamount == 0)
                    {
                        _billedamount = Convert.ToDecimal(row["billed_amount"].ToString());
                    }
                    else
                    {
                        _billedamount = _billedamount + Convert.ToDecimal(row["billed_amount"].ToString());
                    }

                    if (_paidamount == 0)
                    {
                        _paidamount = Convert.ToDecimal(row["paid_amount"].ToString());
                    }
                    else
                    {
                        _paidamount = _paidamount + Convert.ToDecimal(row["paid_amount"].ToString());
                    }
                }

                grddeliverystatus.DataSource = dtfilter;
                grddeliverystatus.DataBind();

                if (_orderamount != 0 && _billedamount != 0)
                {
                    ((Label)grddeliverystatus.FooterRow.FindControl("lblordertotalamount")).Text = _orderamount.ToString();
                    ((Label)grddeliverystatus.FooterRow.FindControl("lblbilledtotalamount")).Text = _billedamount.ToString();
                    ((Label)grddeliverystatus.FooterRow.FindControl("lblpaidtotalamount")).Text = _paidamount.ToString();
                }
                //if (dt.Rows.Count > 0)
                //{
                //    grddeliverystatus.DataSource = dt;
                //    grddeliverystatus.DataBind();
                //}
                //else
                //{
                    
                //}
                
            }
            catch (Exception ex)
            {
                
            }
            
        }
        public void Binddropdownroute()
        {
            ddlroutelist.Items.Clear();
            ddlroutelist.Items.Add("-All-");
            ddlroutelist.DataSource = getroutelist();
            ddlroutelist.DataTextField = "route_name";
            ddlroutelist.DataValueField = "route_id";
            ddlroutelist.DataBind();
        }
        public DataSet getroutelist()
        {
            string queryString = "";
            DataSet routelist = new DataSet();
            queryString = @"SELECT * FROM `ffhp_route_orders`";
            if (queryString != "")
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();

                MySqlConnection con = new MySqlConnection(smsconn);
                con.Open();

                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, smsconn);

                adapteradminmail.Fill(routelist, "RouteName");
                con.Close();
                //sshobj.SshDisconnect(client);
            }
            return routelist;
        }
    }
}
