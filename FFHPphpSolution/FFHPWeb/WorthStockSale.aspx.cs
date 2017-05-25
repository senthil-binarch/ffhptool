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
//using Excel = Microsoft.Office.Interop.Excel;
using System.Globalization;
namespace FFHPWeb
{
    public partial class WorthStockSale : System.Web.UI.Page
    {
        //Excel.Application oXL = null;
        //Excel._Workbook oWB = null;

        //string conn = System.Configuration.ConfigurationManager.AppSettings["SqlConnectionString"].ToString();

        string sqlconn = System.Configuration.ConfigurationManager.AppSettings["SqlConnectionString"].ToString();
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
                //getcurrentstocksaledata();
            }
            lblerror.Text = "";
            lblerror.ForeColor = System.Drawing.Color.Black;
        }
        public int databasetohistory(string fromtablename, string totablename)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(sqlconn);
                SqlCommand command = new SqlCommand("sp_loadhistory_stockproducts", sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@fromtablename", SqlDbType.VarChar).Value = fromtablename;
                command.Parameters.Add("@totablename", SqlDbType.VarChar).Value = totablename;
                command.Parameters.Add("@outputvar", SqlDbType.VarChar, 30);
                command.Parameters["@outputvar"].Direction = ParameterDirection.Output;
                sqlConnection.Open();
                int i = command.ExecuteNonQuery();
                sqlConnection.Close();
                i = Convert.ToInt32(command.Parameters["@outputvar"].Value.ToString());
                return i;
            }
            catch (SqlException ex)
            {
                //Console.WriteLine("SQL Error" + ex.Message.ToString());
                lblerror.Text = errormsg + ex.ToString();
                return 0;
            }
        }
        public void getcurrentstocksaledata()
        {
            
            DateTime stockdate = DateTime.ParseExact(TbxFromDate.Text.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture).AddDays(-1);
            DataTable stocksale = new DataTable("stocksale");
            //queryString = "SELECT '100001' as ordernumber,'FFHP' as customername,entity_id as product_id,name, price, weight,CAST((price/weight) AS DECIMAL(12,3))as per1kg_pc,IF( weight <1, 'kg', 'pc' ) AS unit FROM `catalog_product_flat_1` ORDER BY entity_id";
            queryString = @"select productid,name,
(select (balancescannedweight-balancetrayweight) from stockproducts_history where stockdate=stockproduct.stockdate and productid=stockproducts.productid) as Openingweight,(select balancepiececount from stockproducts_history where stockdate=stockproduct.stockdate and productid=stockproducts.productid) as Openingpiece,(morningscannedweight-morningtrayweight) as morningscannedweight,morningpiececount,(localpurchasescannedweight-localpurchasetrayweight) as localpurchasescannedweight,localpurchasepiececount,onlinescannedweight,onlinescannedpiece,(balancescannedweight-balancetrayweight) as balancescannedweight,balancepiececount,(localsalescannedweight-localsaletrayweight) as localsalescannedweight,localsalepiececount,(aftersalescannedweight-aftersaletrayweight) as aftersalescannedweight,aftersalepiececount,((select (balancescannedweight-balancetrayweight) from stockproducts_history where stockdate=stockproduct.stockdate and productid=stockproducts.productid)+(morningscannedweight-morningtrayweight)+(localpurchasescannedweight-localpurchasetrayweight))-(onlinescannedweight+(balancescannedweight-balancetrayweight)+(localsalescannedweight-localsaletrayweight))as missedweight,(select balancepiececount from stockproducts_history where stockdate=stockproduct.stockdate and productid=stockproducts.productid)+morningpiececount+localpurchasepiececount-(onlinescannedpiece+balancepiececount+localsalepiececount)as missedpiece from stockproducts";
            if (queryString != "")
            {
                SqlConnection sqlConnection = new SqlConnection(sqlconn);
                SqlDataAdapter adapteradminmail = new SqlDataAdapter(queryString, sqlConnection);
                adapteradminmail.Fill(stocksale);
            }
            //gvstocksale.DataSource = stocksale;
            //gvstocksale.DataBind();

            rptstocksale.DataSource = stocksale;
            rptstocksale.DataBind();
        }
        public void getstocksaledata()
        {

            DateTime stockdate = DateTime.ParseExact(TbxFromDate.Text.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            DataTable stocksale = new DataTable("stocksale");
            SqlConnection con = new SqlConnection(sqlconn);
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_worth_various_qty", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("fromdate", stockdate);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(stocksale);
            con.Close();

            if (stocksale.Rows.Count > 0)
            {
                ddlworthtype.Visible = true;
            }
            else
            {
                ddlworthtype.Visible = false;
            }

            ViewState["stocksale"] = Process(stocksale);
            loaddata();
            //rptstocksale.DataSource = (DataTable)ViewState["stocksale"];
            //rptstocksale.DataBind();

            //rptstocksale_incoming.DataSource = (DataTable)ViewState["stocksale"];
            //rptstocksale_incoming.DataBind();
        }
        public DataTable getffhpproducts()
        {
            DataTable ffhpproducts = new DataTable();
            queryString = "SELECT * FROM  1_ffhp_products";
            if (queryString != "")
            {
                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);
                adapteradminmail.Fill(ffhpproducts);
            }
            return ffhpproducts;
        }
        public DataTable Process(DataTable dt)
        {
            DataTable dt3 = new DataTable();
            DataTable resultdt = new DataTable();

            dt3 = getffhpproducts();

            


            DataTable dtResult = new DataTable();
            //dtResult.Columns.Add("pid", typeof(int));
            //dtResult.Columns.Add("pname", typeof(string));
            //dtResult.Columns.Add("purchase_weight", typeof(decimal));
            //dtResult.Columns.Add("purchase_price", typeof(decimal));
            //dtResult.Columns.Add("unit", typeof(string));
            //dtResult.Columns.Add("price_per_kgpc", typeof(decimal));
            //dtResult.Columns.Add("purchase_date", typeof(DateTime));
            //dtResult.Columns.Add("%", typeof(decimal));
            //dtResult.Columns.Add("group", typeof(string));
            dtResult.Columns.Add("productid", typeof(int));
            dtResult.Columns.Add("name", typeof(string));
            dtResult.Columns.Add("purchaseunit", typeof(string));
            dtResult.Columns.Add("onlinesalesperunit", typeof(string));

            dtResult.Columns.Add("Stockweight", typeof(decimal));
            dtResult.Columns.Add("Stockpurchaseprice", typeof(decimal));
            dtResult.Columns.Add("Stocktotal", typeof(decimal));

            dtResult.Columns.Add("Instockweight", typeof(decimal));
            dtResult.Columns.Add("Instockpurchaseprice", typeof(decimal));
            dtResult.Columns.Add("Instocktotal", typeof(decimal));

            dtResult.Columns.Add("Extendedweight", typeof(decimal));
            dtResult.Columns.Add("Extendedpurchaseprice", typeof(decimal));
            dtResult.Columns.Add("Extendedtotal", typeof(decimal));

            dtResult.Columns.Add("MarketPurchase", typeof(decimal));
            dtResult.Columns.Add("Marketpurchaseprice", typeof(decimal));
            dtResult.Columns.Add("Marketpurchasetotal", typeof(decimal));

            dtResult.Columns.Add("localpurchaseweight", typeof(decimal));
            dtResult.Columns.Add("Localpurchaseprice", typeof(decimal));
            dtResult.Columns.Add("localpurchasetotal", typeof(decimal));

            dtResult.Columns.Add("Onlinescannedweight", typeof(decimal));
            dtResult.Columns.Add("OnlinescannedPurchaseprice", typeof(decimal));
            dtResult.Columns.Add("Onlinescannedtotal", typeof(decimal));

            dtResult.Columns.Add("Onlinescannedsellingweight", typeof(decimal));
            dtResult.Columns.Add("Onlinescannedsellingprice", typeof(decimal));
            dtResult.Columns.Add("Onlinescannedsellingtotal", typeof(decimal));

            dtResult.Columns.Add("balanceweight", typeof(decimal));
            dtResult.Columns.Add("Balancepurchaseprice", typeof(decimal));
            dtResult.Columns.Add("balancetotal", typeof(decimal));

            dtResult.Columns.Add("localsaleweight", typeof(decimal));
            dtResult.Columns.Add("Localsalepurchaseprice", typeof(decimal));
            dtResult.Columns.Add("localsaletotal", typeof(decimal));

            dtResult.Columns.Add("Online_Scanned_Market_purchase_qty", typeof(decimal));
            dtResult.Columns.Add("Online_Scanned_Market_purchase_qty_purchaseprice", typeof(decimal));
            dtResult.Columns.Add("Online_Scanned_Market_purchase_qty_price", typeof(decimal));

            dtResult.Columns.Add("aftersaleweight", typeof(decimal));
            dtResult.Columns.Add("Aftersalepurchaseprice", typeof(decimal));
            dtResult.Columns.Add("aftersaletotal", typeof(decimal));

            dtResult.Columns.Add("missedweight", typeof(decimal));
            dtResult.Columns.Add("Missedpurchaseprice", typeof(decimal));
            dtResult.Columns.Add("missedtotal", typeof(decimal));

            dtResult.Columns.Add("group", typeof(string));

            var result = from dataRows1 in dt.AsEnumerable()
                         join dataRows2 in dt3.AsEnumerable()
                         on dataRows1.Field<int>("productid") equals dataRows2.Field<int>("productid")

                         select dtResult.LoadDataRow(new object[]
             {
                dataRows1.Field<int>("productid"),
                dataRows1.Field<string>("name"),
                dataRows1.Field<string>("purchaseunit"),
                dataRows1.Field<string>("onlinesalesperunit"),
                
                dataRows1.Field<decimal?>("Stockweight"),
                dataRows1.Field<decimal?>("Stockpurchaseprice"),
                dataRows1.Field<decimal?>("Stocktotal"),

                dataRows1.Field<decimal?>("Instockweight"),
                dataRows1.Field<decimal?>("Instockpurchaseprice"),
                dataRows1.Field<decimal?>("Instocktotal"),
                
                dataRows1.Field<decimal?>("Extendedweight"),
                dataRows1.Field<decimal?>("Extendedpurchaseprice"),
                dataRows1.Field<decimal?>("Extendedtotal"),
                
                dataRows1.Field<decimal?>("MarketPurchase"),
                dataRows1.Field<decimal?>("Marketpurchaseprice"),
                dataRows1.Field<decimal?>("Marketpurchasetotal"),
                
                dataRows1.Field<decimal?>("localpurchaseweight"),
                dataRows1.Field<decimal?>("Localpurchaseprice"),
                dataRows1.Field<decimal?>("localpurchasetotal"),

                dataRows1.Field<decimal?>("Onlinescannedweight"),
                dataRows1.Field<decimal?>("OnlinescannedPurchaseprice"),
                dataRows1.Field<decimal?>("Onlinescannedtotal"),
                
                dataRows1.Field<decimal?>("Onlinescannedsellingweight"),
                dataRows1.Field<decimal?>("Onlinescannedsellingprice"),
                dataRows1.Field<decimal?>("Onlinescannedsellingtotal"),
                
                dataRows1.Field<decimal?>("balanceweight"),
                dataRows1.Field<decimal?>("Balancepurchaseprice"),
                dataRows1.Field<decimal?>("balancetotal"),
                
                dataRows1.Field<decimal?>("localsaleweight"),
                dataRows1.Field<decimal?>("Localsalepurchaseprice"),
                dataRows1.Field<decimal?>("localsaletotal"),
                
                dataRows1.Field<decimal?>("Online_Scanned_Market_purchase_qty"),
                dataRows1.Field<decimal?>("Online_Scanned_Market_purchase_qty_purchaseprice"),
                dataRows1.Field<decimal?>("Online_Scanned_Market_purchase_qty_price"),
                
                dataRows1.Field<decimal?>("aftersaleweight"),
                dataRows1.Field<decimal?>("Aftersalepurchaseprice"),
                dataRows1.Field<decimal?>("aftersaletotal"),
                
                dataRows1.Field<decimal?>("missedweight"),
                dataRows1.Field<decimal?>("Missedpurchaseprice"),
                dataRows1.Field<decimal?>("missedtotal"),

                dataRows2.Field<string>("group"),
              }, false);

            resultdt = result.CopyToDataTable();
            resultdt = resort(resultdt, "group, productid", "ASC");

            return resultdt;
        }
        public static DataTable resort(DataTable dt, string colName, string direction)
        {
            dt.DefaultView.Sort = colName + " " + direction;
            dt = dt.DefaultView.ToTable();
            return dt;
        }
        protected void btnsubmit_OnClick(object sender, EventArgs e)
        {
            if (TbxFromDate.Text != "")
            {
                getstocksaledata();
                btnsendemail.Visible = true;
            }
        }
        protected void ddlworthtype_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewState["stocksale"] != null)
            {
                loaddata();
            }
        }
        public void loaddata()
        {
            if (ddlworthtype.SelectedValue.ToString() == "0")
            {
                rptstocksale_incoming.Visible = false;
                rptstocksale_local.Visible = false;
                rptstocksale_balance.Visible = false;

                rptstocksale.DataSource = (DataTable)ViewState["stocksale"];
                rptstocksale.DataBind();
                rptstocksale.Visible = true;
            }
            else if (ddlworthtype.SelectedValue.ToString() == "1")
            {
                rptstocksale.Visible = false;
                rptstocksale_local.Visible = false;
                rptstocksale_balance.Visible = false;

                rptstocksale_incoming.DataSource = filterdata("Instockweight"); //(DataTable)ViewState["stocksale"];
                rptstocksale_incoming.DataBind();

                rptstocksale_incoming.Visible = true;
            }
            else if (ddlworthtype.SelectedValue.ToString() == "2")
            {
                rptstocksale.Visible = false;
                rptstocksale_incoming.Visible = false;
                rptstocksale_balance.Visible = false;

                rptstocksale_local.DataSource = filterdata("localsaleweight");//(DataTable)ViewState["stocksale"];
                rptstocksale_local.DataBind();

                rptstocksale_local.Visible = true;
            }
            else if (ddlworthtype.SelectedValue.ToString() == "3")
            {
                rptstocksale.Visible = false;
                rptstocksale_incoming.Visible = false;
                rptstocksale_local.Visible = false;

                rptstocksale_balance.DataSource = filterdata("balanceweight");//(DataTable)ViewState["stocksale"];
                rptstocksale_balance.DataBind();

                rptstocksale_balance.Visible = true;
            }
        }
        public DataTable filterdata(string weightname)
        {
            DataTable dtreturn = new DataTable();
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["stocksale"];
            if (dt.Rows.Count > 0)
            {
                var dValue = from row in dt.AsEnumerable()
                             where Convert.ToDouble(row.Field<decimal>(weightname).ToString()) > 0
                             select row;

                if (dValue.Any())
                {
                    dtreturn = dValue.CopyToDataTable();
                }
            }
            return dtreturn;
        }
        public DataTable sendmail_filterdata(string weightname)
        {
            int serialNumber = 0;
            DataTable dtreturn = new DataTable();
            dtreturn.Columns.Add("Sno", typeof(int));
            dtreturn.Columns.Add("Pid", typeof(int));
            dtreturn.Columns.Add("Name", typeof(string));
            dtreturn.Columns.Add("Unit", typeof(string));
            dtreturn.Columns.Add("Weight", typeof(decimal));
            dtreturn.Columns.Add("Price_per_kg_pc", typeof(decimal));
            dtreturn.Columns.Add("Total", typeof(decimal));
            dtreturn.Columns.Add("Group", typeof(string));

            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["stocksale"];
            DataRow nrow = null;
            if (weightname == "localsaleweight")
            {
                if (dt.Rows.Count > 0)
                {
                    var dValue = from row in dt.AsEnumerable()
                                 where Convert.ToDouble(row.Field<decimal>(weightname).ToString()) > 0
                                 select new
                                 {
                                     Sno=serialNumber++,
                                     Pid = row.Field<int>("productid"),
                                     Name = row.Field<string>("name"),
                                     Unit = row.Field<string>("purchaseunit"),
                                     Weight = row.Field<decimal>("localsaleweight"),
                                     Price_per_kg_pc = row.Field<decimal>("Localsalepurchaseprice"),
                                     Total = row.Field<decimal>("localsaletotal"),
                                     Group=row.Field<string>("group")
                                 };

                    if (dValue.Any())
                    {
                        foreach (var rowObj in dValue)
                        {
                            nrow = dtreturn.NewRow();
                            dtreturn.Rows.Add(rowObj.Sno, rowObj.Pid, rowObj.Name, rowObj.Unit, rowObj.Weight, rowObj.Price_per_kg_pc, rowObj.Total, rowObj.Group);
                        }
                    }

                }
            }
            else if(weightname=="balanceweight")
            {
                if (dt.Rows.Count > 0)
                {
                    var dValue = from row in dt.AsEnumerable()
                                 where Convert.ToDouble(row.Field<decimal>(weightname).ToString()) > 0
                                 select new
                                 {
                                     Sno = serialNumber++,
                                     Pid = row.Field<int>("productid"),
                                     Name = row.Field<string>("name"),
                                     Unit = row.Field<string>("purchaseunit"),
                                     Weight = row.Field<decimal>("balanceweight"),
                                     Price_per_kg_pc = row.Field<decimal>("balancepurchaseprice"),
                                     Total = row.Field<decimal>("balancetotal"),
                                     Group=row.Field<string>("group")
                                 };

                    if (dValue.Any())
                    {
                        foreach (var rowObj in dValue)
                        {
                            nrow = dtreturn.NewRow();
                            dtreturn.Rows.Add(rowObj.Sno, rowObj.Pid, rowObj.Name, rowObj.Unit, rowObj.Weight, rowObj.Price_per_kg_pc, rowObj.Total,rowObj.Group);
                        }
                    }

                }
            }

            return dtreturn;
        }
        protected void btnsendemail_OnClick(object sender, EventArgs e)
        {
            try
            {
                StringBuilder mailstring = new StringBuilder();
                DataTable stocksale = new DataTable();
                stocksale = (DataTable)ViewState["stocksale"];
                DataTable dt = new DataTable();
                
                    if (stocksale.Rows.Count > 0)
                    {
                        mailstring.Append("<b>" + TbxFromDate.Text.ToString() + " Stock Worth Report:</b><br /><br />");


                        mailstring.Append("<b>Local Sale Worth:</b><br />");
                        dt = sendmail_filterdata("localsaleweight");
                        mailstring.Append(ConvertDataTableToHTML(dt));
                        mailstring.Append("<br /><br />");
                        var localsaletotalworth = dt.AsEnumerable().Sum(x => x.Field<decimal>("Total"));
                        var localsalesumofkgs = dt.AsEnumerable().Where(x => x.Field<string>("Unit").ToLower() == "kg").Sum(x => x.Field<decimal>("Weight"));

                        mailstring.Append("<b>Total Worth:</b>"+localsaletotalworth.ToString()+" Rs.<br />");
                        mailstring.Append("<b>Sum of Kgs:</b>" + localsalesumofkgs.ToString() + " kgs<br />");

                        mailstring.Append("<br /><br />");

                        mailstring.Append("<b>Balance Worth:</b><br />");
                        dt = sendmail_filterdata("balanceweight");
                        mailstring.Append(ConvertDataTableToHTML(dt));
                        mailstring.Append("<br /><br />");
                        var balancetotalworth = dt.AsEnumerable().Sum(x => x.Field<decimal>("Total"));
                        var balancesumofkgs = dt.AsEnumerable().Where(x => x.Field<string>("Unit").ToLower() == "kg").Sum(x => x.Field<decimal>("Weight"));

                        mailstring.Append("<b>Total Worth:</b>" + balancetotalworth.ToString() + " Rs.<br />");
                        mailstring.Append("<b>Sum of Kgs:</b>" + balancesumofkgs.ToString() + " kgs<br />");

                        mailstring.Append("<br /><br />");


                        if (mailstring.ToString() != "")
                        {
                            string subject = TbxFromDate.Text.ToString() + " Stock Worth Report:";
                            string body = mailstring.ToString();
                            
                            string mailto = System.Configuration.ConfigurationSettings.AppSettings["Mail_To"].ToString();
                            string mailcc = System.Configuration.ConfigurationSettings.AppSettings["Mail_Cc"].ToString();
                            string mailcredential = System.Configuration.ConfigurationSettings.AppSettings["Email"].ToString();
                            string mailpassword = System.Configuration.ConfigurationSettings.AppSettings["EmailPassword"].ToString();

                            MailMessage mail = new MailMessage();
                            //mail.Priority = MailPriority.High;
                            //mail.Headers.Add("Message-Id", String.Concat("<", DateTime.Now.ToString("yyMMdd"), ".", DateTime.Now.ToString("HHmmss"), "@binarch.com>"));
                            SmtpClient SmtpServer = new SmtpClient(System.Configuration.ConfigurationSettings.AppSettings["Smtp"].ToString());
                            mail.From = new MailAddress(mailcredential, "FFHP.IN");
                            mail.To.Add(mailto);
                            mail.CC.Add(mailcc);
                            mail.Subject = subject;
                            //mail.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");

                            //System.Net.Mail.AlternateView plainView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(System.Text.RegularExpressions.Regex.Replace(body, @"<(.|\n)*?>", string.Empty), null, "text/plain");
                            //System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(body, null, "text/html");

                            //mail.AlternateViews.Add(plainView);
                            //mail.AlternateViews.Add(htmlView);

                            mail.Body = body;
                            mail.IsBodyHtml = true;

                            SmtpServer.Port = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["Port"].ToString());//587;
                            SmtpServer.Credentials = new System.Net.NetworkCredential(mailcredential, mailpassword);
                            SmtpServer.EnableSsl = false;

                            SmtpServer.Send(mail);

                            lblerror.Text = "Mail sent successfully.";

                            

                            //string subject = TbxFromDate.Text.ToString() + " Stock Worth Report:";
                            //string body = mailstring.ToString();
                            //string mailto = "senthil@binarch.in";//email;//System.Configuration.ConfigurationSettings.AppSettings["Mail_To"].ToString();
                            ////string mailcc = System.Configuration.ConfigurationSettings.AppSettings["Mail_Cc"].ToString();
                            //string mailcredential = System.Configuration.ConfigurationSettings.AppSettings["Mail_Credential"].ToString();
                            //string mailpassword = System.Configuration.ConfigurationSettings.AppSettings["Mail_Password"].ToString();
                            ////using (FileStream fileStream = File.OpenRead(filepath))
                            ////{
                            //MailMessage mail = new MailMessage();
                            //SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                            ////SmtpClient SmtpServer = new SmtpClient("localhost");
                            //mail.From = new MailAddress(mailcredential, "FFHP.IN");
                            //mail.To.Add(mailto);
                            ////mail.CC.Add(mailcc);
                            //mail.Subject = subject;
                            //mail.Body = body;
                            //mail.IsBodyHtml = true;

                            //SmtpServer.Port = 587;
                            //SmtpServer.Credentials = new System.Net.NetworkCredential(mailcredential, mailpassword);
                            //SmtpServer.EnableSsl = true;

                            //SmtpServer.Send(mail);

                            //lblerror.Text = "Mail sent successfully.";
                        }
                        else
                        {
                            lblerror.Text = "Empty mail. Please try again.";
                        }
                    }
                    else
                    {
                        lblerror.Text = "No data found. Please try again.";
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
                {
                    if (j < 4)
                    {
                        html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                    }
                    else
                    {
                        html += "<td align='right'>" + dt.Rows[i][j].ToString() + "</td>";
                    }
                }
                html += "</tr>";
            }
            html += "</table>";
            return html;
        }
    }
}
