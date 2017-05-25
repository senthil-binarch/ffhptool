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
    public partial class Pending_Payment_Status : System.Web.UI.Page
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
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                
                //sshobj.SshDisconnect(client);
            }
            lblStatus.Text = "";
        }
        protected void btnsearch_OnClick(object sender, EventArgs e)
        {
            try
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                DateTime dtf = DateTime.ParseExact(TbxFromDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                DateTime dtt = DateTime.ParseExact(TbxToDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                string wherecondition = "";
                if (TbxFromDate.Text != "" && TbxToDate.Text != "")
                {
                    wherecondition = "date between '" + dtf.ToString("yyyy-MM-dd") + "' and '" + dtt.ToString("yyyy-MM-dd") + "' ";
                }

                if (Tbxcustomername.Text != "" || Tbxordernumber.Text != "")
                {

                    if (Tbxcustomername.Text.ToString() != "")
                    {
                        wherecondition = wherecondition + " and customer_name like '" + Tbxcustomername.Text.ToString() + "%'";
                    }
                    else if (Tbxordernumber.Text.ToString() != "")
                    {
                        wherecondition = wherecondition + " and order_number in(" + Tbxordernumber.Text.ToString() + ")";
                    }
                    if (ddlpstatus.SelectedIndex > 0)
                    {
                        wherecondition = wherecondition + " and payment_status='" + ddlpstatus.SelectedItem.Text.ToString() + "' ";
                    }
                    if (ddldstatus.SelectedIndex > 0)
                    {
                        wherecondition = wherecondition + " and delivery_status='" + ddldstatus.SelectedItem.Text.ToString() + "' ";
                    }

                }
                else
                {
                    //wherecondition = "";
                    if (ddlpstatus.SelectedIndex > 0)
                    {
                        wherecondition = wherecondition + " and payment_status='" + ddlpstatus.SelectedItem.Text.ToString() + "' ";
                    }
                    if (ddldstatus.SelectedIndex > 0)
                    {

                        wherecondition = wherecondition + " and delivery_status='" + ddldstatus.SelectedItem.Text.ToString() + "' ";
                    }
                }

                if (wherecondition != "")
                {
                    Get_Delivery_Status(" where "+wherecondition);
                    grddeliverystatus.Visible = true;
                }
                else
                {
                    grddeliverystatus.Visible = false;
                    StatusMessage("Error", "Select condition");
                }
                //sshobj.SshDisconnect(client);
            }
            catch
            {
            }
        }
        //protected void btnsearch_OnClick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //ConnectSsh sshobj = new ConnectSsh();
        //        //SshClient client = sshobj.SshConnect();
        //        string wherecondition = "";
        //        //if (TbxFromDate.Text != "" && TbxToDate.Text != "")
        //        //{
        //        //    wherecondition = "where date between '" + TbxFromDate.Text + "' and '" + TbxToDate.Text+"' ";
        //        //}

        //        if (Tbxcustomername.Text != "" || Tbxordernumber.Text != "")
        //        {
                    
        //            if (Tbxcustomername.Text.ToString() != "")
        //            {
        //                wherecondition = "where customer_name like '" + Tbxcustomername.Text.ToString() + "%'";
        //            }
        //            else if (Tbxordernumber.Text.ToString() != "")
        //            {
        //                wherecondition = "where order_number in(" + Tbxordernumber.Text.ToString() + ")";
        //            }
        //            if (ddlpstatus.SelectedIndex > 0)
        //            {
        //                wherecondition = wherecondition + " and payment_status='" + ddlpstatus.SelectedItem.Text.ToString() + "' ";
        //            }
        //            if (ddldstatus.SelectedIndex > 0)
        //            {
        //                wherecondition = wherecondition + " and delivery_status='" + ddldstatus.SelectedItem.Text.ToString() + "' ";
        //            }

        //        }
        //        else
        //        {
        //            wherecondition = "";
        //            if (ddlpstatus.SelectedIndex > 0)
        //            {
        //                wherecondition = " where payment_status='" + ddlpstatus.SelectedItem.Text.ToString() + "' ";
        //            }
        //            if (ddldstatus.SelectedIndex > 0)
        //            {
        //                if (wherecondition == "")
        //                {
        //                    wherecondition = wherecondition + " where delivery_status='" + ddldstatus.SelectedItem.Text.ToString() + "' ";
        //                }
        //                else
        //                {
        //                    wherecondition = wherecondition + " and delivery_status='" + ddldstatus.SelectedItem.Text.ToString() + "' ";
        //                }
        //            }
        //        }
                
        //        if (wherecondition != "")
        //        {
        //            Get_Delivery_Status(wherecondition);
        //            grddeliverystatus.Visible = true;
        //        }
        //        else
        //        {
        //            grddeliverystatus.Visible = false;
        //            StatusMessage("Error", "Select condition");
        //        }
        //        //sshobj.SshDisconnect(client);
        //    }
        //    catch
        //    {
        //    }
        //}
        
        public void Get_Delivery_Status(string wherecodition)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dtfilter = new DataTable();

                APIMethods objdeliverystatus = new APIMethods();
                dt = objdeliverystatus.Get_Delivery_Status(wherecodition).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    ddllist.Visible = true;
                }
                else
                {
                    ddllist.Visible = false;
                }
                ViewState["Delivery_Status"] = dt;

                loaddata();

                //if (ddlroutelist.SelectedIndex != 0)
                //{
                //    var result = (from r in dt.AsEnumerable()
                //                  where r["route_number"].ToString() == ddlroutelist.SelectedItem.Text.ToString()
                //                  select r).ToList();

                //    if (result.Count > 0)
                //    {
                //        dtfilter = result.CopyToDataTable();
                //    }
                //}
                //else
                //{
                //    dtfilter = dt;
                //}
                
                
                
            }
            catch (Exception ex)
            {

            }

        }
        public void binddata(DataTable dt)
        {
            DataTable dtfilter = new DataTable();
            dtfilter = dt;
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
            if (dt.Rows.Count > 0)
            {
                grddeliverystatus.DataSource = dtfilter;
                grddeliverystatus.DataBind();
                if (_orderamount != 0 && _billedamount != 0)
                {
                    ((Label)grddeliverystatus.FooterRow.FindControl("lblordertotalamount")).Text = _orderamount.ToString();
                    ((Label)grddeliverystatus.FooterRow.FindControl("lblbilledtotalamount")).Text = _billedamount.ToString();
                    ((Label)grddeliverystatus.FooterRow.FindControl("lblpaidtotalamount")).Text = _paidamount.ToString();
                }
            }
            else
            {
                grddeliverystatus.DataSource = null;
                grddeliverystatus.DataBind();
            }
        }
        protected void ddllist_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                loaddata();
            }
            catch (Exception ex)
            {

            }
        }
        public void loaddata()
        {
            if (ddllist.SelectedValue.ToString() == "0")
            {
                binddata((DataTable)ViewState["Delivery_Status"]);//(DataTable)ViewState["stocksale"];
            }
            else if (ddllist.SelectedValue.ToString() == "1")
            {
                binddata(filterdata("Payment Pending"));//(DataTable)ViewState["stocksale"];
            }
            else if (ddllist.SelectedValue.ToString() == "2")
            {
                binddata(filterdata("Cheque Collected"));//(DataTable)ViewState["stocksale"];
                
            }
            else if (ddllist.SelectedValue.ToString() == "3")
            {
                binddata(filterdata("Tomorrow Delivery"));//(DataTable)ViewState["stocksale"];
            }
            else if (ddllist.SelectedValue.ToString() == "3")
            {
                binddata(filterdata("Already Paid"));//(DataTable)ViewState["stocksale"];
            }
        }
        public DataTable filterdata(string filtervalue)
        {
            
            DataTable dtreturn = new DataTable();
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["Delivery_Status"];
            try
            {
                if (dt.Rows.Count > 0)
                {
                    //var dValue = from row in dt.AsEnumerable()
                    //             where Convert.ToString(row.Field<String>("discription").ToString()) == "0,Payment Pending"
                    //             select row;
                    //var dValue = from row in dt.AsEnumerable()
                                 //where row.Field<string>("discription").Contains(filtervalue)
                                 //select row;

                    var query = dt.AsEnumerable()
    .Where(r => r.Field<string>("discription")!=null && r.Field<string>("discription").Contains(filtervalue));

                    if (query.Any())
                    {
                        dtreturn = query.CopyToDataTable();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return dtreturn;
        }
        private void StatusMessage(string MassageType, string Msg)
        {
            if (MassageType == "Information")
            {
                lblStatus.ForeColor = System.Drawing.Color.Blue;
                lblStatus.Text = Msg;
            }
            else if (MassageType == "Error")
            {
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Text = Msg;
            }
        }
    }
}
