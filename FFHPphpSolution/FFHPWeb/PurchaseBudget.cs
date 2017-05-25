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

namespace FFHPWeb
{
    public class PurchaseBudget
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
            string directorypath = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["FilePathBudget"].ToString() + DateTime.Now.ToString("dd-MM-yyyy"));
            if (Directory.Exists(directorypath) == false)
            {
                Directory.CreateDirectory(directorypath);
            }
            string filename = "PurchaseBudget" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss") + ".xls";
            //string filename = "PurchaseBudget" + DateTime.Now.ToString("dd-MM-yyyy") +".xls";
            string filepath = System.Configuration.ConfigurationManager.AppSettings["FilePathBudget"].ToString() + DateTime.Now.ToString("dd-MM-yyyy") + "/" + filename;

            //string delfilename = "PurchaseBudget" + DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy") + ".xls";
            string deldirectorypath = System.Configuration.ConfigurationManager.AppSettings["FilePathBudget"].ToString() + DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");

            if (Directory.Exists(HttpContext.Current.Server.MapPath(deldirectorypath)))
            {
                DirectoryInfo di = new DirectoryInfo(HttpContext.Current.Server.MapPath(deldirectorypath));

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                Directory.Delete(HttpContext.Current.Server.MapPath(deldirectorypath));
            }

            DataTable dt = purchase_budget();
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

                var sumGreen = dt.AsEnumerable()
            .Where(r => r.Field<string>("product_group") == "Green")
            .Sum(r => Convert.ToDouble(r.Field<string>("budgetprice")));

                var sumVeg = dt.AsEnumerable()
                .Where(r => r.Field<string>("product_group") == "Veg")
                .Sum(r => Convert.ToDouble(r.Field<string>("budgetprice")));

                var sumFruit = dt.AsEnumerable()
                .Where(r => r.Field<string>("product_group") == "Fruit")
                .Sum(r => Convert.ToDouble(r.Field<string>("budgetprice")));

                string mailto = System.Configuration.ConfigurationManager.AppSettings["Mail_To"].ToString();
                string mailcredential = System.Configuration.ConfigurationManager.AppSettings["Mail_Credential"].ToString();
                string mailpassword = System.Configuration.ConfigurationManager.AppSettings["Mail_Password"].ToString();

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress(mailcredential);
                mail.To.Add(mailto);
                mail.Subject = "PFA - PurchaseBudget(XLS)";
                mail.Body = "PFA - PurchaseBudget(XLS) <br><br>" + "Vegitable Budget is " + sumVeg.ToString() + "<br> Fruits Budget is " + sumFruit.ToString() + "<br> Greens Budget is " + sumGreen.ToString() + "<br><br><br> Thanks, <br><br><img width='100px' height='100px' alt='FFHP' src='" + HttpContext.Current.Server.MapPath("Images/FFHPLogo1.png")+"'>";
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
        public DataTable getproductprice()
        {

            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(conn);
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_products", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("productid", 0);
                cmd.Parameters.AddWithValue("name", "");
                cmd.Parameters.AddWithValue("operation", "productprice");
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
            }
            catch (Exception ex)
            {
                
            }
            return dt;
        }
        public DataTable getpurchaseweight()
        {

            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(conn);
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_products", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("productid", 0);
                cmd.Parameters.AddWithValue("name", "");
                cmd.Parameters.AddWithValue("operation", "purchaseproduct");
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        public DataTable purchase_budget()
        {
            DataTable LPRData = new DataTable();
            TotalWeightlprdata objlprdata = new TotalWeightlprdata();
            LPRData = objlprdata.getffhpproducts();

            DataTable productpricedt = new DataTable();
            productpricedt = getproductprice();

            DataTable budgetdt = new DataTable();
            budgetdt.Columns.Add("purchasedate");
            budgetdt.Columns.Add("productid");
            budgetdt.Columns.Add("productname");
            budgetdt.Columns.Add("purchaseqty");
            budgetdt.Columns.Add("unit");
            budgetdt.Columns.Add("product_group");
            budgetdt.Columns.Add("price");
            budgetdt.Columns.Add("pricedate");
            budgetdt.Columns.Add("budgetprice");

            //weightcalculate obj = new weightcalculate();
            //DataTable dt = obj.calculate();

            DataTable dt = getpurchaseweight();
            foreach (DataRow row in dt.Rows)
            {
                var productprice = (from DataRow dRow in productpricedt.Rows
                                    where dRow["productid"].ToString() == row["Product_Id"].ToString()
                                    select new { col1 = dRow["price"] });

                var purchase_date = (from DataRow dRow in productpricedt.Rows
                                     where dRow["productid"].ToString() == row["Product_Id"].ToString()
                                     select new { col1 = dRow["purchase_date"] });

                DataTable LPRData1 = new DataTable();
                LPRData1 = LPRData.AsEnumerable().Where(r => Convert.ToInt32(r["productid"]) == Convert.ToInt32(row["Product_Id"].ToString())).AsDataView().ToTable();

                DataRow budgetdtrow = budgetdt.NewRow();
                budgetdtrow["purchasedate"] = DateTime.Today.ToShortDateString();
                budgetdtrow["productid"] = row["Product_Id"].ToString();
                budgetdtrow["productname"] = row["Name"].ToString();
                budgetdtrow["purchaseqty"] = row["PurchaseWeight"].ToString();
                budgetdtrow["unit"] = row["Units"].ToString();
                if (LPRData1.Rows.Count > 0)
                {
                    budgetdtrow["product_group"] = LPRData1.Rows[0]["group"].ToString();
                }
                else
                {
                    budgetdtrow["product_group"] = "";
                }
                if (productprice.Any())
                {
                    budgetdtrow["price"] = productprice.First().col1.ToString();
                }
                else
                {
                    budgetdtrow["price"] = 0;
                }
                if (purchase_date.Any())
                {
                    budgetdtrow["pricedate"] = purchase_date.First().col1.ToString();
                }
                else
                {
                    budgetdtrow["pricedate"] = "";
                }
                if (productprice.Any())
                {
                    budgetdtrow["budgetprice"] = Convert.ToDouble(row["PurchaseWeight"].ToString()) * Convert.ToDouble(productprice.First().col1.ToString());
                }
                else
                {
                    budgetdtrow["budgetprice"] = 0;
                }
                budgetdt.Rows.Add(budgetdtrow);
            }


            var sumGreen = budgetdt.AsEnumerable()
            .Where(r => r.Field<string>("product_group") == "Green")
            .Sum(r => Convert.ToDouble(r.Field<string>("budgetprice")));

            var sumVeg = budgetdt.AsEnumerable()
            .Where(r => r.Field<string>("product_group") == "Veg")
            .Sum(r => Convert.ToDouble(r.Field<string>("budgetprice")));

            var sumFruit = budgetdt.AsEnumerable()
            .Where(r => r.Field<string>("product_group") == "Fruit")
            .Sum(r => Convert.ToDouble(r.Field<string>("budgetprice")));

            return budgetdt;

        }
    }
}
