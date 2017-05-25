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
    public partial class StockSale : System.Web.UI.Page
    {
        //Excel.Application oXL = null;
        //Excel._Workbook oWB = null;

        string conn = System.Configuration.ConfigurationManager.AppSettings["SqlConnectionString"].ToString();
        string queryString = "";
        MySqlDataAdapter DA;
        string s = "";
        bool t = false;
        string errormsg = "Try again";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //getcurrentstocksaledata();
                }
                lblerror.Text = "";
                lblerror.ForeColor = System.Drawing.Color.Black;
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        protected void Btnupload_OnClick(object sender, EventArgs e)
        {
            try
            {
                int i = 0;
                if (FUstocksale.HasFile)
                {
                    string file = FUstocksale.PostedFile.FileName.ToString().ToLower();
                    if (file.EndsWith(".xls"))
                    {
                        if (File.Exists(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["StocksaleFilePath"].ToString() + "stockproducts.xls")))
                        {
                            File.Delete(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["StocksaleFilePath"].ToString() + "stockproducts.xls"));
                            FUstocksale.SaveAs(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["StocksaleFilePath"].ToString() + "stockproducts.xls"));
                            Upload_stockproducts_Data();
                            //i = exefiletodatabase(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["StocksaleFilePath"].ToString() + "stockproducts.csv"), "stockproducts");
                            //if (i > 0)
                            //{
                            //    i = databasetohistory("stockproducts", "stockproducts_history");
                            //    if (i > 0)
                            //    {
                            //        lblerror.Text = "Upload Successfully.";
                            //        //ScriptManager.RegisterStartupScript(this, GetType(), "ConfirmShowreport", "ConfirmShowreport('PurchaseReport.aspx');", true);
                            //        //Response.Redirect("PurchaseReport.aspx", false);
                            //    }
                            //}


                            ////databasetohistory();
                        }
                        else
                        {
                            FUstocksale.SaveAs(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["StocksaleFilePath"].ToString() + "stockproducts.xls"));
                            Upload_stockproducts_Data();
                            //i = exefiletodatabase(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["StocksaleFilePath"].ToString() + "stockproducts.csv"), "stockproducts");
                            //if (i > 0)
                            //{
                            //    i = databasetohistory("stockproducts", "stockproducts_history");
                            //    if (i > 0)
                            //    {
                            //        lblerror.Text = "Upload Successfully.";
                            //        //ScriptManager.RegisterStartupScript(this, GetType(), "ConfirmShowreport", "ConfirmShowreport('PurchaseReport.aspx');", true);
                                    
                            //    }
                            //}
                            ////databasetohistory();
                        }
                    }
                    else
                    {
                        lblerror.Text = "upload only .csv file";
                    }
                }
                else
                {
                    lblerror.Text = "Plese select a file";
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg;
            }
        }
        public int Upload_stockproducts_Data()
        {
            DataTable dt = new DataTable();
            dt = Readdata_from_excel_to_datatable();
            int i = 0;
            if (dt.Rows.Count > 0)
            {
                Truncate_Table("stockproducts");

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                {
                    bulkCopy.DestinationTableName =
                        "dbo.stockproducts";

                    try
                    {
                        // Write from the source to the destination.
                        bulkCopy.WriteToServer(dt);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    i = databasetohistory("stockproducts", "stockproducts_history");
                    if (i > 0)
                    {
                        lblerror.Text = "Upload Successfully.";
                        //ScriptManager.RegisterStartupScript(this, GetType(), "ConfirmShowreport", "ConfirmShowreport('PurchaseReport.aspx');", true);
                        //Response.Redirect("PurchaseReport.aspx", false);
                    }
                }
            }
            return i;
        }
        public int Truncate_Table(string tablename)
        {
            int i = 0;
            queryString = "TRUNCATE TABLE " + tablename;
            SqlConnection con = new SqlConnection(conn);
            con.Open();
            SqlCommand cmd = new SqlCommand(queryString, con);
            i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }
        public DataTable Readdata_from_excel_to_datatable()
        {
            DataTable dt = new DataTable();
            System.Data.OleDb.OleDbConnection MyConnection;
            //System.Data.OleDb.OleDbCommand myCommand = new System.Data.OleDb.OleDbCommand();
            string filepath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["StocksaleFilePath"].ToString() + "stockproducts.xls");
            string sql = null;
            MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + filepath + "';Extended Properties=Excel 8.0;");
            MyConnection.Open();
            //myCommand.Connection = MyConnection;
            //sql = "select ordernumber,customername,productid,name,weight,units,scannedweight,imagename,weightproductid,weightcalculation from [purchase$] where ordernumber='" + ordernumber + "'";
            sql = "select *  from [Sheet1$]";// where purchase_date='" + DateTime.Now.Date.ToString("MM-dd-yyyy") + "'";
            //myCommand.CommandText = sql;
            System.Data.OleDb.OleDbDataAdapter DataAdapter = new System.Data.OleDb.OleDbDataAdapter(sql, MyConnection);
            //DataAdapter.SelectCommand.Connection = MyConnection;
            //DataAdapter.SelectCommand.CommandText = sql;
            DataAdapter.Fill(dt);
            MyConnection.Close();
            return dt;
        }
        public int exefiletodatabase(string filepath, string tablename)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(conn);
                SqlCommand command = new SqlCommand("sp_fileupload", sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@filepath", SqlDbType.VarChar).Value = filepath;
                command.Parameters.Add("@tablename", SqlDbType.VarChar).Value = tablename;
                sqlConnection.Open();
                int i = command.ExecuteNonQuery();
                sqlConnection.Close();
                return i;
            }
            catch (SqlException ex)
            {
                //Console.WriteLine("SQL Error" + ex.Message.ToString());
                lblerror.Text = errormsg + ", Data mismatch or Duplicate entries, Please check."; //+ex.ToString();
                lblerror.ForeColor = System.Drawing.Color.Red;
                return 0;
            }
        }
        public int databasetohistory(string fromtablename, string totablename)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(conn);
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
                SqlConnection sqlConnection = new SqlConnection(conn);
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
            //queryString = "SELECT '100001' as ordernumber,'FFHP' as customername,entity_id as product_id,name, price, weight,CAST((price/weight) AS DECIMAL(12,3))as per1kg_pc,IF( weight <1, 'kg', 'pc' ) AS unit FROM `catalog_product_flat_1` ORDER BY entity_id";
//            queryString = @"select productid,name,
//(select (balancescannedweight-balancetrayweight) from stockproducts_history where stockdate='" + stockdate.ToString("MM/dd/yyyy") + "' and productid=stockproducts.productid) as Openingweight,(select balancepiececount from stockproducts_history where stockdate='" + stockdate.ToString("MM/dd/yyyy") + "' and productid=stockproducts.productid) as Openingpiece,(morningscannedweight-morningtrayweight) as morningscannedweight,morningpiececount,(localpurchasescannedweight-localpurchasetrayweight) as localpurchasescannedweight,localpurchasepiececount,onlinescannedweight,onlinescannedpiece,(balancescannedweight-balancetrayweight) as balancescannedweight,balancepiececount,(localsalescannedweight-localsaletrayweight) as localsalescannedweight,localsalepiececount,(aftersalescannedweight-aftersaletrayweight) as aftersalescannedweight,aftersalepiececount,((select (balancescannedweight-balancetrayweight) from stockproducts_history where stockdate='" + stockdate.ToString("MM/dd/yyyy") + "' and productid=stockproducts.productid)+(morningscannedweight-morningtrayweight)+(localpurchasescannedweight-localpurchasetrayweight))-(onlinescannedweight+(balancescannedweight-balancetrayweight)+(localsalescannedweight-localsaletrayweight))as missedweight,(select balancepiececount from stockproducts_history where stockdate='" + stockdate.ToString("MM/dd/yyyy") + "' and productid=stockproducts.productid)+morningpiececount+localpurchasepiececount-(onlinescannedpiece+balancepiececount+localsalepiececount)as missedpiece from stockproducts_history where stockdate='"+stockdate.ToString("MM/dd/yyyy")+"'";
            queryString = @"select productid,name,
(select (balancescannedweight-balancetrayweight) from stockproducts_history as a where a.stockdate=stockproducts_history.stockdate-1 and a.productid=stockproducts_history.productid) as Openingweight,
(select balancepiececount from stockproducts_history as a where a.stockdate=stockproducts_history.stockdate-1 and a.productid=stockproducts_history.productid) as Openingpiece,
(morningscannedweight-morningtrayweight) as morningscannedweight,morningpiececount,
(localpurchasescannedweight-localpurchasetrayweight) as localpurchasescannedweight,
localpurchasepiececount,onlinescannedweight,onlinescannedpiece,
(balancescannedweight-balancetrayweight) as balancescannedweight,
balancepiececount,(localsalescannedweight-localsaletrayweight) as localsalescannedweight,
localsalepiececount,(aftersalescannedweight-aftersaletrayweight) as aftersalescannedweight,
aftersalepiececount,
ISNULL(((select (balancescannedweight-balancetrayweight) from stockproducts_history as a where a.stockdate=stockproducts_history.stockdate-1 and a.productid=stockproducts_history.productid)+(morningscannedweight-morningtrayweight)+(localpurchasescannedweight-localpurchasetrayweight))-(onlinescannedweight+(balancescannedweight-balancetrayweight)+(localsalescannedweight-localsaletrayweight)),0) as missedweight,
ISNULL((select balancepiececount from stockproducts_history as a where a.stockdate=stockproducts_history.stockdate-1 and a.productid=stockproducts_history.productid)+morningpiececount+localpurchasepiececount-(onlinescannedpiece+balancepiececount+localsalepiececount),0) as missedpiece 
from stockproducts_history where stockdate='" + stockdate.ToString("MM/dd/yyyy") + "'";
            if (queryString != "")
            {
                SqlConnection sqlConnection = new SqlConnection(conn);
                SqlDataAdapter adapteradminmail = new SqlDataAdapter(queryString, sqlConnection);
                adapteradminmail.Fill(stocksale);
            }
            //gvstocksale.DataSource = stocksale;
            //gvstocksale.DataBind();

            rptstocksale.DataSource = stocksale;
            rptstocksale.DataBind();
            ViewState["stocksale"] = stocksale;

            var result = from o in stocksale.AsEnumerable()
                         where o.Field<decimal>("missedweight") + o.Field<decimal>("missedpiece") != 0
                         select o;
            if (result.Any())
            {
                rptstocksale_mismatch.DataSource = result.CopyToDataTable();
                rptstocksale_mismatch.DataBind();

                btnsendemail.Visible = true;
            }
        }
        protected void btnsubmit_OnClick(object sender, EventArgs e)
        {
            if (TbxFromDate.Text != "")
            {
                getstocksaledata();
            }
        }
        protected void btnsendemail_OnClick(object sender, EventArgs e)
        {
            try
            {
                int incrementid=0;
                StringBuilder mailstring = new StringBuilder();
                DataTable stocksale = new DataTable();
                stocksale = (DataTable)ViewState["stocksale"];
                DataTable dt = new DataTable();

                DataTable dtresult = new DataTable();
                dtresult.Columns.Add("Sno", typeof(int));
                dtresult.Columns.Add("Pid", typeof(int));
                dtresult.Columns.Add("Name", typeof(string));
                dtresult.Columns.Add("Missed_wgt", typeof(decimal));
                dtresult.Columns.Add("Missed_pc", typeof(decimal));

                DataRow nrow = null;
                if (stocksale.Rows.Count > 0)
                {

                    var result = from o in stocksale.AsEnumerable()
                                 where o.Field<decimal>("missedweight") + o.Field<decimal>("missedpiece") != 0
                                 select new
                                     {
                                         Sno = incrementid++,
                                         Pid = o.Field<int>("productid"),
                                         Name = o.Field<string>("name"),
                                         Missed_wgt = o.Field<decimal>("missedweight"),
                                         Missed_pc = o.Field<decimal>("missedpiece")
                                     };
                    if (result.Any())
                    {
                        foreach (var rowObj in result)
                        {
                            nrow = dtresult.NewRow();
                            dtresult.Rows.Add(rowObj.Sno, rowObj.Pid, rowObj.Name, rowObj.Missed_wgt, rowObj.Missed_pc);
                        }
                    }

                    if (dtresult.Rows.Count > 0)
                    {
                        mailstring.Append("<b>" + TbxFromDate.Text.ToString() + " Stock Mismatch Report:</b><br /><br />");
                        mailstring.Append(ConvertDataTableToHTML(dtresult));
                        mailstring.Append("<br /><br />");
                    }
                    
                    if (mailstring.ToString() != "")
                    {
                        string subject = TbxFromDate.Text.ToString() + " Stock Mismatch Report:";
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
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                html += "</tr>";
            }
            html += "</table>";
            return html;
        }
    }
}
