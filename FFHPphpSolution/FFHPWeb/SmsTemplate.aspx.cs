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
using System.Net;
//using Renci.SshNet;
namespace FFHPWeb
{
    public partial class SmsTemplate : System.Web.UI.Page
    {
        string conn = System.Configuration.ConfigurationManager.AppSettings["Connection"].ToString();
        string smsconn = System.Configuration.ConfigurationManager.AppSettings["SmsDBConnection"].ToString();//"server=68.178.143.39;userid=ffhplog;password=FFHPl0g!;database=ffhplog;Convert Zero Datetime=True";  //sms connection
        string queryString = "";
        MySqlDataAdapter DA;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();

                Bindsmstype();
                //sshobj.SshDisconnect(client);
            }
            lblerror.Text = "";
        }
        public void Bindsmstype()
        {
            try
            {
                DataTable smstemplate = new DataTable("smstemplate");
                queryString = "SELECT * FROM `1_ffhp_smsformat`";
                if (queryString != "")
                {
                    MySqlConnection con = new MySqlConnection(smsconn);
                    con.Open();

                    MySqlDataAdapter adaptersmstemplate = new MySqlDataAdapter(queryString, smsconn);


                    adaptersmstemplate.Fill(smstemplate);
                    GVsmstemplate.DataSource = smstemplate;
                    GVsmstemplate.DataBind();
                    //ddlsmstype.DataSource = smstemplate;
                    //ddlsmstype.AppendDataBoundItems = true;
                    //ddlsmstype.Items.Add("-Select-");
                    //ddlsmstype.DataTextField = "sms_type";
                    //ddlsmstype.DataValueField = "sms_id";
                    //ddlsmstype.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }
        protected void btnaddnew_OnClick(object sender, EventArgs e)
        {
            tblentry.Visible = true;
            //txtsmstype.Enabled = true;
            txtsmstype.Text = "";
            txttemplate.Text = "";
            BtnSave.Text = "Save";
        }
        protected void BtnSave_OnClick(object sender, EventArgs e)
        {
            try
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                
                    if (BtnSave.Text == "Save")
                    {
                        if (SmsTypeValidate() == 0)
                        {
                            queryString = @"INSERT INTO 1_ffhp_smsformat(sms_type,sms_content,order_based)VALUES('" + txtsmstype.Text.ToString() + "','" + txttemplate.Text.ToString() + "','" + ddlorderbased.SelectedValue.ToString() + "')";
                        }
                        else
                        {
                            queryString = "";
                        }

                    }
                    else
                    {
                        if (HFsmstype.Value.ToString().Trim() != txtsmstype.Text.ToString().Trim())
                        {
                            if (SmsTypeValidate() == 0)
                            {
                                queryString = @"UPDATE 1_ffhp_smsformat set sms_type='" + txtsmstype.Text.ToString() + "',sms_content='" + txttemplate.Text.ToString() + "',order_based='" + ddlorderbased.SelectedValue.ToString() + "' where sms_id=" + HFsmsid.Value.ToString();
                            }
                            else
                            {
                                queryString = "";
                            }
                        }
                        else
                        {
                            queryString = @"UPDATE 1_ffhp_smsformat set sms_content='" + txttemplate.Text.ToString() + "',order_based='" + ddlorderbased.SelectedValue.ToString() + "' where sms_id=" + HFsmsid.Value.ToString();
                        }
                        
                        //queryString = @"UPDATE 1_ffhp_smsformat set sms_type='" + txtsmstype.Text.ToString() + "',sms_content='" + txttemplate.Text.ToString() + "',order_based='" + ddlorderbased.SelectedValue.ToString() + "' where sms_id=" + HFsmsid.Value.ToString();
                        
                    }
                    if (queryString != "")
                    {
                        MySqlConnection con = new MySqlConnection(smsconn);
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand(queryString, con);
                        cmd.ExecuteNonQuery();
                        tblentry.Visible = false;
                        //txtsmstype.Enabled = true;
                        txtsmstype.Text = "";
                        txttemplate.Text = "";
                        lblerror.Text = BtnSave.Text.ToString() + " Successfuly";
                        BtnSave.Text = "Save";
                        Bindsmstype();
                    }
                    else
                    {
                        lblerror.Text = "Sms Type already exist.";
                    }

                    //sshobj.SshDisconnect(client);
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        public int SmsTypeValidate()
        {
            int t = 0;
            try
            {
                
                DataTable smstemplate = new DataTable("smstemplate");
                queryString = "SELECT * FROM `1_ffhp_smsformat` where sms_type='" + txtsmstype.Text.ToString() + "'";
                if (queryString != "")
                {
                    MySqlConnection con = new MySqlConnection(smsconn);
                    con.Open();

                    MySqlDataAdapter adaptersmstemplate = new MySqlDataAdapter(queryString, smsconn);


                    adaptersmstemplate.Fill(smstemplate);
                    t = smstemplate.Rows.Count;
                    
                }
            }
            catch (Exception ex)
            {
            }
            return t;
        }
        protected void BtnCancel_OnClick(object sender, EventArgs e)
        {
            tblentry.Visible = false;
            //txtsmstype.Enabled = true;
            txtsmstype.Text = "";
            txttemplate.Text = "";
            BtnSave.Text = "Save";
        }
        protected void btnedit_OnClick(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
                txtsmstype.Text = gvr.Cells[0].Text.ToString();
                //txtsmstype.Enabled = false;
                txttemplate.Text = gvr.Cells[1].Text.ToString();
                HFsmsid.Value = ((HiddenField)gvr.FindControl("HFsmsid")).Value.ToString();
                HForderbased.Value = ((HiddenField)gvr.FindControl("HForderbased")).Value.ToString();
                ddlorderbased.SelectedValue = ((HiddenField)gvr.FindControl("HForderbased")).Value.ToString();
                HFsmstype.Value = gvr.Cells[0].Text.ToString();
                BtnSave.Text = "Update";
                tblentry.Visible = true;
            }
            catch (Exception ex)
            {
            }
        }
        protected void ddlsmstype_OnSelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
