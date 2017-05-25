using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
using System.Web.UI.HtmlControls;
using System.Globalization;

namespace FFHPWeb
{
    public class missingitems
    {
        string conn = System.Configuration.ConfigurationManager.AppSettings["SqlConnectionString"].ToString();
        string queryString = "";
        MySqlDataAdapter DA;
        string s = "";
        bool t = false;
        string errormsg = "Try again";
        public void exportToExcel()
        {
            //create a folder
            string directorypath = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["FilePathMissingItems"].ToString() + DateTime.Now.ToString("dd-MM-yyyy"));
            if (Directory.Exists(directorypath) == false)
            {
                Directory.CreateDirectory(directorypath);
            }
            string filename = "MissingItems" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss") + ".xls";
            //string filename = "MissingItems" + DateTime.Now.ToString("dd-MM-yyyy") +".xls";
            string filepath = System.Configuration.ConfigurationManager.AppSettings["FilePathMissingItems"].ToString() + DateTime.Now.ToString("dd-MM-yyyy") + "/" + filename;

            //string delfilename = "MissingItems" + DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy") + ".xls";
            string deldirectorypath = System.Configuration.ConfigurationManager.AppSettings["FilePathMissingItems"].ToString() + DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");

            if (Directory.Exists(HttpContext.Current.Server.MapPath(deldirectorypath)))
            {
                DirectoryInfo di = new DirectoryInfo(HttpContext.Current.Server.MapPath(deldirectorypath));

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                Directory.Delete(HttpContext.Current.Server.MapPath(deldirectorypath));
            }

            //DataTable dt = getmissingitems();
            DataTable dt = getmissingitems_today();
            /*Set up work book, work sheets, and excel application*/
            //Microsoft.Office.Interop.Excel.Application oexcel = new Microsoft.Office.Interop.Excel.Application();
            Excel.Application xlApp;
            xlApp = new Excel.ApplicationClass();
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                object misValue = System.Reflection.Missing.Value;

                //Microsoft.Office.Interop.Excel.Workbook obook = oexcel.Workbooks.Add(misValue);
                //Microsoft.Office.Interop.Excel.Worksheet osheet = new Microsoft.Office.Interop.Excel.Worksheet();
                Excel.Workbook obook;
                Excel.Worksheet osheet;




                System.Globalization.CultureInfo oldCI = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");


                obook = xlApp.Workbooks.Add(misValue);


                //  obook.Worksheets.Add(misValue);

                osheet = (Microsoft.Office.Interop.Excel.Worksheet)obook.Sheets["Sheet1"];
                int colIndex = 0;
                int rowIndex = 1;

                foreach (DataColumn dc in dt.Columns)
                {
                    colIndex++;
                    osheet.Cells[1, colIndex] = dc.ColumnName;
                }
                foreach (DataRow dr in dt.Rows)
                {
                    rowIndex++;
                    colIndex = 0;

                    foreach (DataColumn dc in dt.Columns)
                    {
                        colIndex++;
                        osheet.Cells[rowIndex, colIndex] = dr[dc.ColumnName];
                    }
                }

                osheet.Columns.AutoFit();

                //Release and terminate excel
                xlApp.DisplayAlerts = false;
                obook.SaveAs(HttpContext.Current.Server.MapPath(filepath), Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, false, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                obook.Close(true, misValue, misValue);
                xlApp.Workbooks.Close();
                xlApp.Quit();

                if (obook != null)
                {
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(osheet);
                    osheet = null;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(obook);
                    obook = null;
                    xlApp.Workbooks.Close();
                    xlApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
                    xlApp = null;
                }

                //releaseObject(osheet);

                //releaseObject(obook);

                //releaseObject(oexcel);
                GC.Collect();

                string mailto = System.Configuration.ConfigurationManager.AppSettings["Mail_To"].ToString();
                string mailcredential = System.Configuration.ConfigurationManager.AppSettings["Mail_Credential"].ToString();
                string mailpassword = System.Configuration.ConfigurationManager.AppSettings["Mail_Password"].ToString();

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress(mailcredential);
                mail.To.Add(mailto);
                mail.Subject = "PFA - MissingItems(XLS)";
                mail.Body = "PFA - MissingItems(XLS) <br><br><br> Thanks, <br><br><img width='100px' height='100px' alt='FFHP' src='" + HttpContext.Current.Server.MapPath("Images/FFHPLogo1.png") + "'>";
                mail.IsBodyHtml = true;

                System.Net.Mail.Attachment attachment;
                //attachment = new System.Net.Mail.Attachment(Server.MapPath("MailFiles/CustomerInformation/" + Session.SessionID +s+ ".xls"));
                attachment = new System.Net.Mail.Attachment(HttpContext.Current.Server.MapPath(filepath));
                mail.Attachments.Add(attachment);

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(mailcredential, mailpassword);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                xlApp.Quit();
            }
        }
        public DataTable getmissingitems_today()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(conn);
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_missingitems", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        public DataTable getmissingitems()
        {
            DataTable dtmissingitems = new DataTable();
            DataTable dtreceivedqty = new DataTable();
            dtreceivedqty = getreceivedqty();
            if (dtreceivedqty.Rows.Count > 0)
            {
                weightcalculate obj = new weightcalculate();
                DataTable dtpurchaseqty = obj.calculate();

                DataTable dtResult = new DataTable();
                dtResult.Columns.Add("productid", typeof(int));
                dtResult.Columns.Add("productname", typeof(string));
                dtResult.Columns.Add("purchaseqty", typeof(double));
                dtResult.Columns.Add("receivedqty", typeof(double));
                dtResult.Columns.Add("missedqty", typeof(double));

                var items = (from owt in dtpurchaseqty.AsEnumerable()
                             join t in dtreceivedqty.AsEnumerable()
                             on Convert.ToInt32(owt.Field<string>("Product_Id")) equals t.Field<int>("productid")
                             //where Convert.ToInt32(owt.Field<string>("Product_Id")) == 246
                             select dtResult.LoadDataRow(new object[]
                                {
                                Convert.ToInt32(owt.Field<string>("Product_Id")),
                                t.Field<string>("name"),
                                Convert.ToDouble(owt.Field<string>("TotalWeight")),
                                Convert.ToDouble(t.Field<decimal>("morningscannedweight")),
                                Convert.ToDouble(owt.Field<string>("TotalWeight"))-Convert.ToDouble(t.Field<decimal>("morningscannedweight"))
                                }, false));
                if (items.Any())
                {
                    dtmissingitems = items.CopyToDataTable();
                }
            }
            return dtmissingitems;
        }
        public DataTable getreceivedqty()
        {
            DataTable dt = new DataTable();
            try
            {
                DateTime stockdate = DateTime.Now;
                //DateTime stockdate = DateTime.ParseExact(dtime.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                DataTable stocksale = new DataTable("stocksale");
                //queryString = "SELECT '100001' as ordernumber,'FFHP' as customername,entity_id as product_id,name, price, weight,CAST((price/weight) AS DECIMAL(12,3))as per1kg_pc,IF( weight <1, 'kg', 'pc' ) AS unit FROM `catalog_product_flat_1` ORDER BY entity_id";
                //            queryString = @"select productid,name,
                //(select (balancescannedweight-balancetrayweight) from stockproducts_history where stockdate='" + stockdate.ToString("MM/dd/yyyy") + "' and productid=stockproducts.productid) as Openingweight,(select balancepiececount from stockproducts_history where stockdate='" + stockdate.ToString("MM/dd/yyyy") + "' and productid=stockproducts.productid) as Openingpiece,(morningscannedweight-morningtrayweight) as morningscannedweight,morningpiececount,(localpurchasescannedweight-localpurchasetrayweight) as localpurchasescannedweight,localpurchasepiececount,onlinescannedweight,onlinescannedpiece,(balancescannedweight-balancetrayweight) as balancescannedweight,balancepiececount,(localsalescannedweight-localsaletrayweight) as localsalescannedweight,localsalepiececount,(aftersalescannedweight-aftersaletrayweight) as aftersalescannedweight,aftersalepiececount,((select (balancescannedweight-balancetrayweight) from stockproducts_history where stockdate='" + stockdate.ToString("MM/dd/yyyy") + "' and productid=stockproducts.productid)+(morningscannedweight-morningtrayweight)+(localpurchasescannedweight-localpurchasetrayweight))-(onlinescannedweight+(balancescannedweight-balancetrayweight)+(localsalescannedweight-localsaletrayweight))as missedweight,(select balancepiececount from stockproducts_history where stockdate='" + stockdate.ToString("MM/dd/yyyy") + "' and productid=stockproducts.productid)+morningpiececount+localpurchasepiececount-(onlinescannedpiece+balancepiececount+localsalepiececount)as missedpiece from stockproducts_history where stockdate='"+stockdate.ToString("MM/dd/yyyy")+"'";
                queryString = @"select productid,name,
(morningscannedweight-morningtrayweight) as morningscannedweight,morningpiececount
from stockproducts_history where stockdate='" + stockdate.ToString("MM/dd/yyyy") + "'";
                if (queryString != "")
                {
                    SqlConnection sqlConnection = new SqlConnection(conn);
                    SqlDataAdapter adapteradminmail = new SqlDataAdapter(queryString, sqlConnection);
                    adapteradminmail.Fill(dt);
                }
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
    }
}
