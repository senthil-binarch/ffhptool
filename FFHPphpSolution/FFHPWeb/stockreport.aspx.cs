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
namespace FFHPWeb
{
    public partial class stockreport : System.Web.UI.Page
    {
        string conn = System.Configuration.ConfigurationManager.AppSettings["SqlConnectionString"].ToString();
        string queryString = "";
        MySqlDataAdapter DA;
        string s = "";
        bool t = false;
        string errormsg = "Try again";
        protected void Page_Load(object sender, EventArgs e)
        {
            lblerror.Text = "";
            lblerror.ForeColor = System.Drawing.Color.Black;
        }
        protected void btnsubmit_OnClick(object sender, EventArgs e)
        {
            DataSet stocksale = new DataSet("stocksale");
            try
            {
                if (TbxFromDate.Text != "")
                {
                    stocksale = getstocksaledata();
                    ViewState["stocksale"] = stocksale;
                    if (stocksale.Tables.Count > 0)
                    {
                        rptstocksale_notbuyed.DataSource = stocksale.Tables[0];
                        rptstocksale_notbuyed.DataBind();

                        rptstocksale_missing.DataSource = stocksale.Tables[1];
                        rptstocksale_missing.DataBind();

                        rptstocksale_unwanted.DataSource = stocksale.Tables[2];
                        rptstocksale_unwanted.DataBind();

                        btnsendemail.Visible = true;
                    }
                    else
                    {
                        btnsendemail.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public DataSet getstocksaledata()
        {

            DateTime stockdate = DateTime.ParseExact(TbxFromDate.Text.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            DataSet stocksale = new DataSet("stocksale");


            SqlConnection sqlConnection = new SqlConnection(conn);
            SqlCommand command = new SqlCommand("sp_missing", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@stockdate", SqlDbType.DateTime).Value = stockdate;//DateTime.Now.Date.ToString();
            sqlConnection.Open();
            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill(stocksale);
            sqlConnection.Close();

            return stocksale;
        }
        protected void btnsendemail_OnClick(object sender, EventArgs e)
        {
            try
            {
            StringBuilder mailstring = new StringBuilder();
            DataSet stocksale = new DataSet();
            stocksale = (DataSet)ViewState["stocksale"];
            
            if (stocksale.Tables.Count > 0)
            {
                mailstring.Append("<b>" + TbxFromDate.Text.ToString() + " Product List:</b><br /><br />");

                if (stocksale.Tables[3].Rows.Count > 0)
                {
                    DataTable dt=new DataTable();
                    dt=stocksale.Tables[3];
                    mailstring.Append("<b>Morning not received products </b><br />");
                    mailstring.Append(ConvertDataTableToHTML(dt));
                    mailstring.Append("<br /><br />");
                }

                if (stocksale.Tables[4].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = stocksale.Tables[4];
                    mailstring.Append("<b>Morning received products with weight difference</b><br />");
                    mailstring.Append(ConvertDataTableToHTML(dt));
                    mailstring.Append("<br /><br />");
                }

                if (stocksale.Tables[5].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = stocksale.Tables[5];
                    mailstring.Append("<b>Morning received not required products</b><br />");
                    mailstring.Append(ConvertDataTableToHTML(dt));
                    mailstring.Append("<br /><br />");
                }
            }

            if (mailstring.ToString() != "")
            {

                string subject = TbxFromDate.Text.ToString() + " Product List:";
                string body = mailstring.ToString();
                string mailto = System.Configuration.ConfigurationSettings.AppSettings["Mail_To"].ToString();
                string mailcc = System.Configuration.ConfigurationSettings.AppSettings["Mail_Cc"].ToString();
                string mailcredential = System.Configuration.ConfigurationSettings.AppSettings["Email"].ToString();
                string mailpassword = System.Configuration.ConfigurationSettings.AppSettings["EmailPassword"].ToString();
                
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(System.Configuration.ConfigurationSettings.AppSettings["Smtp"].ToString());//SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress(mailcredential, "FFHP.IN");
                mail.To.Add(mailto);
                mail.CC.Add(mailcc);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                SmtpServer.Port = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["Port"].ToString());//587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(mailcredential, mailpassword);
                SmtpServer.EnableSsl = false;

                SmtpServer.Send(mail);

                lblerror.Text = "Mail sent successfully.";
            }
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString() + ex.ToString();
            }
        }
        public static string ConvertDataTableToHTML(DataTable dt)
        {
            string html = "<table cellspacing='0' rules='all'  border = '1'>";
            //add header row
            html += "<tr>";
            for (int i = 0; i < dt.Columns.Count; i++)
                html += "<td>" + dt.Columns[i].ColumnName + "</td>";
            html += "</tr>";
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                html += "</tr>";
            }
            html += "</table>";
            return html;
        }
    }
}
