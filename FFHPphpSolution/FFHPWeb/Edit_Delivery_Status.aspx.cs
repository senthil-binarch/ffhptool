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
    public partial class Edit_Delivery_Status : System.Web.UI.Page
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
                Binddropdownroute();
                //sshobj.SshDisconnect(client);
            }
            lblStatus.Text = "";
        }
        protected void btnsubmit_OnClick(object sender, EventArgs e)
        {
            try
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                if (TbxFromDate.Text != "")
                {
                    DateTime dtf = DateTime.ParseExact(TbxFromDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    Get_Delivery_Status(dtf, dtf);
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
        protected void btnsearch_OnClick(object sender, EventArgs e)
        {
            try
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                string wherecondition = "";
                if (Tbxcustomername.Text != "" || Tbxordernumber.Text != "")
                {
                    
                    if (Tbxcustomername.Text.ToString() != "")
                    {
                        wherecondition = "where customer_name like '" + Tbxcustomername.Text.ToString() + "%'";
                    }
                    else if (Tbxordernumber.Text.ToString() != "")
                    {
                        wherecondition = "where order_number in(" + Tbxordernumber.Text.ToString() + ")";
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
                    wherecondition = "";
                    if (ddlpstatus.SelectedIndex > 0)
                    {
                        wherecondition = " where payment_status='" + ddlpstatus.SelectedItem.Text.ToString() + "' ";
                    }
                    if (ddldstatus.SelectedIndex > 0)
                    {
                        if (wherecondition == "")
                        {
                            wherecondition = wherecondition + " where delivery_status='" + ddldstatus.SelectedItem.Text.ToString() + "' ";
                        }
                        else
                        {
                            wherecondition = wherecondition + " and delivery_status='" + ddldstatus.SelectedItem.Text.ToString() + "' ";
                        }
                    }
                }
                
                if (wherecondition != "")
                {
                    Get_Delivery_Status(wherecondition);
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
        protected void btnedit_OnClick(object sender, EventArgs e)
        {
            try
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                if (Tbxordernumberedit.Text != "")
                {
                    Get_Delivery_Status_ordernumber(Tbxordernumberedit.Text.ToString().Trim());
                    grddeliverystatus.Visible = true;
                }
                else
                {
                    grddeliverystatus.Visible = false;
                    StatusMessage("Error", "Select condition");
                }
                //sshobj.SshDisconnect(client);
            }
            catch (Exception ex)
            {
            }
        }
        protected void btnupdate_OnClick(object sender, EventArgs e)
        {
            try
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                if (ddlpaymentstatus.SelectedIndex != 0)
                {
                    if (ddldeliverystatus.SelectedIndex != 0)
                    {
                        if (Tbxdescription.Text != "")
                        {
                            int i = 0;
                            APIMethods objdeliverystatus = new APIMethods();
                            DataTable dt = new DataTable();
                            dt = (DataTable)ViewState["editdata"];
                            //DateTime dtf = DateTime.ParseExact(dt.Rows[0]["date"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                            i = objdeliverystatus.Set_Delivery_Status_ordernumber(dt.Rows[0]["order_number"].ToString(), dt.Rows[0]["datescannededit"].ToString(), ddlpaymentstatus.SelectedItem.Text.ToString(), ddldeliverystatus.SelectedItem.Text.ToString(), Tbxdescription.Text.ToString().Trim());
                            if (i > 0)
                            {
                                StatusMessage("Information", "Updated Successfully");
                                Get_Delivery_Status_ordernumber(Tbxordernumberedit.Text.ToString().Trim());
                                tbedit.Visible = false;
                            }
                            else
                            {
                                StatusMessage("Error", "Not Successfully Updated");
                                tbedit.Visible = true;
                            }
                        }
                        else
                        {
                            StatusMessage("Error", "Enter description");
                        }
                    }
                    else
                    {
                        StatusMessage("Error", "Select delivery status");
                    }
                }
                else
                {
                    StatusMessage("Error", "Select payment status");
                }
                //sshobj.SshDisconnect(client);
            }
            catch (Exception ex)
            {
            }
        }
        public void Get_Delivery_Status(DateTime fromdate, DateTime todate)
        {
            try
            {
                tbedit.Visible = false;
                DataTable dt = new DataTable();
                DataTable dtfilter = new DataTable();
                
                APIMethods objdeliverystatus = new APIMethods();
                dt = objdeliverystatus.Get_Delivery_Status(fromdate, todate).Tables[0];
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
        public void Get_Delivery_Status(string wherecodition)
        {
            try
            {
                tbedit.Visible = false;
                DataTable dt = new DataTable();
                DataTable dtfilter = new DataTable();

                APIMethods objdeliverystatus = new APIMethods();
                dt = objdeliverystatus.Get_Delivery_Status(wherecodition).Tables[0];
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
        public void Get_Delivery_Status_ordernumber(string ordernumber)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dtfilter = new DataTable();

                APIMethods objdeliverystatus = new APIMethods();
                dt = objdeliverystatus.Get_Delivery_Status_ordernumber(ordernumber).Tables[0];
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
                    tbedit.Visible = true;
                    ddlpaymentstatus.SelectedIndex = ddlpaymentstatus.Items.IndexOf(ddlpaymentstatus.Items.FindByText(dt.Rows[0]["payment_status"].ToString()));
                    ddldeliverystatus.SelectedIndex = ddldeliverystatus.Items.IndexOf(ddldeliverystatus.Items.FindByText(dt.Rows[0]["delivery_status"].ToString()));
                    Tbxdescription.Text = dt.Rows[0]["discription"].ToString();
                }
                else
                {
                    grddeliverystatus.DataSource = null;
                    grddeliverystatus.DataBind();
                    tbedit.Visible = false;
                    ddlpaymentstatus.SelectedIndex = 0;
                    ddldeliverystatus.SelectedIndex = 0;
                    Tbxdescription.Text = "";
                }
                ViewState["editdata"] = dt;
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
                MySqlConnection con = new MySqlConnection(smsconn);
                con.Open();

                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, smsconn);

                adapteradminmail.Fill(routelist, "RouteName");
                con.Close();
            }
            return routelist;
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
