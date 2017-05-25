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
using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.Spreadsheets;
using DotNetOpenAuth.OAuth2.AuthServer;

//using Renci.SshNet;

namespace FFHPWeb
{
    public partial class Purchase : System.Web.UI.Page
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
                    //readgoogle_Spreadsheets();
                    bind();
                }
                lblerror.Text = "";
                lblerror.ForeColor = System.Drawing.Color.Black;
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        public void readgoogle_Spreadsheets()
        {
            

            //////////////////////////////////////////////////////////////////////////////
            //// STEP 1: Configure how to perform OAuth 2.0
            //////////////////////////////////////////////////////////////////////////////

            //// TODO: Update the following information with that obtained from
            //// https://code.google.com/apis/console. After registering
            //// your application, these will be provided for you.

            //string CLIENT_ID = "288520312827-6ror482pav1okqtlk3pl1n7iu3r15ek6.apps.googleusercontent.com";

            //// This is the OAuth 2.0 Client Secret retrieved
            //// above.  Be sure to store this value securely.  Leaking this
            //// value would enable others to act on behalf of your application!
            //string CLIENT_SECRET = "R4DVBDAVFo1Wi1UmEuN1q6zC";

            //// Space separated list of scopes for which to request access.
            //string SCOPE = "https://spreadsheets.google.com/feeds https://docs.google.com/feeds";

            //// This is the Redirect URI for installed applications.
            //// If you are building a web application, you have to set your
            //// Redirect URI at https://code.google.com/apis/console.
            //string REDIRECT_URI = "urn:ietf:wg:oauth:2.0:oob";

            //////////////////////////////////////////////////////////////////////////////
            //// STEP 2: Set up the OAuth 2.0 object
            //////////////////////////////////////////////////////////////////////////////

            //// OAuth2Parameters holds all the parameters related to OAuth 2.0.
            
            //OAuth2Parameters parameters;// = new OAuth2Parameters();

            //// Set your OAuth 2.0 Client Id (which you can register at
            //// https://code.google.com/apis/console).
            //parameters.ClientId = CLIENT_ID;

            //// Set your OAuth 2.0 Client Secret, which can be obtained at
            //// https://code.google.com/apis/console.
            //parameters.ClientSecret = CLIENT_SECRET;

            //// Set your Redirect URI, which can be registered at
            //// https://code.google.com/apis/console.
            //parameters.RedirectUri = REDIRECT_URI;

            //////////////////////////////////////////////////////////////////////////////
            //// STEP 3: Get the Authorization URL
            //////////////////////////////////////////////////////////////////////////////

            //// Set the scope for this particular service.
            //parameters.Scope = SCOPE;

            //// Get the authorization url.  The user of your application must visit
            //// this url in order to authorize with Google.  If you are building a
            //// browser-based application, you can redirect the user to the authorization
            //// url.
            
            //string authorizationUrl = Google.GData.Client.OAuthUtil.CreateOAuth2AuthorizationUrl(parameters);
            //Console.WriteLine(authorizationUrl);
            //Console.WriteLine("Please visit the URL above to authorize your OAuth "
            //  + "request token.  Once that is complete, type in your access code to "
            //  + "continue...");
            //parameters.AccessCode = Console.ReadLine();

            //////////////////////////////////////////////////////////////////////////////
            //// STEP 4: Get the Access Token
            //////////////////////////////////////////////////////////////////////////////

            //// Once the user authorizes with Google, the request token can be exchanged
            //// for a long-lived access token.  If you are building a browser-based
            //// application, you should parse the incoming request token from the url and
            //// set it in OAuthParameters before calling GetAccessToken().
            //OAuthUtil.GetAccessToken(parameters);
            //string accessToken = parameters.AccessToken;
            //Console.WriteLine("OAuth Access Token: " + accessToken);

            //////////////////////////////////////////////////////////////////////////////
            //// STEP 5: Make an OAuth authorized request to Google
            //////////////////////////////////////////////////////////////////////////////

            //// Initialize the variables needed to make the request
            //GOAuth2RequestFactory requestFactory =
            //    new GOAuth2RequestFactory(null, "MySpreadsheetIntegration-v1", parameters);
            //SpreadsheetsService service = new SpreadsheetsService("MySpreadsheetIntegration-v1");
            //service.RequestFactory = requestFactory;

            //// Make the request to Google
            //// See other portions of this guide for code to put here...


            SpreadsheetsService myService = new SpreadsheetsService("ffhptoolWebclient1");
            myService.setUserCredentials("demo.binarch@gmail.com", "binarch.demo");

            SpreadsheetQuery query = new SpreadsheetQuery();
            SpreadsheetFeed feed = myService.Query(query);

            foreach (SpreadsheetEntry entry in feed.Entries)
            {
                Console.WriteLine(entry.Title.Text);
            }
        }
        public DataTable bind()
        {

            DataTable dt = new DataTable();
            try
            {
                APIMethods objmobileorders = new APIMethods();
                dt = objmobileorders.getProduct_Weight_Price_for_Before_After_Sale();


                //dtpurchase.Columns.Add("purchase_date", typeof(DateTime), System.DateTime.Now.ToShortDateString());
                gvpurchase.DataSource = dt;
                gvpurchase.DataBind();
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
            return dt;

        }
        public DataTable getPurchase()
        {
            DataTable DT = new DataTable("purchase");
            queryString = "select *,((price_per_kgpc-purchase_price)/purchase_price)*100 as '%' from purchase_history where purchase_date=(select max(purchase_date) from dbo.purchase_history)";
            //ConnectSsh sshobj = new ConnectSsh();
            //SshClient client = sshobj.SshConnect();
            SqlDataAdapter sda = new SqlDataAdapter(queryString, conn);
            sda.Fill(DT);
            //sshobj.SshDisconnect(client);
            return DT;
        }
        public DataTable getPurchase_from_CSV()
        {
            DataTable dt = new DataTable();
            System.Data.OleDb.OleDbConnection MyConnection;
            //System.Data.OleDb.OleDbCommand myCommand = new System.Data.OleDb.OleDbCommand();
            string filepath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["PurchaseFilePath"].ToString() + "purchase.xls");
            string sql = null;
            MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + filepath + "';Extended Properties=Excel 8.0;");
            MyConnection.Open();
            //myCommand.Connection = MyConnection;
            //sql = "select ordernumber,customername,productid,name,weight,units,scannedweight,imagename,weightproductid,weightcalculation from [purchase$] where ordernumber='" + ordernumber + "'";
            sql = "select *  from [purchase$]";// where purchase_date='" + DateTime.Now.Date.ToString("MM-dd-yyyy") + "'";
            //myCommand.CommandText = sql;
            System.Data.OleDb.OleDbDataAdapter DataAdapter = new System.Data.OleDb.OleDbDataAdapter(sql, MyConnection);
            //DataAdapter.SelectCommand.Connection = MyConnection;
            //DataAdapter.SelectCommand.CommandText = sql;
            DataAdapter.Fill(dt);
            MyConnection.Close();
            return dt;
        }
        public int Upload_Purchase_Data()
        {
            DataTable dt = new DataTable();
            dt = getPurchase_from_CSV();
            int i = 0;
            if (dt.Rows.Count > 0)
            {
                Truncate_Table("purchase");

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                {
                    bulkCopy.DestinationTableName =
                        "dbo.purchase";

                    try
                    {
                        // Write from the source to the destination.
                        bulkCopy.WriteToServer(dt);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    i = databasetohistory("purchase", "purchase_History");
                    if (i > 0)
                    {
                        lblerror.Text = "Upload Successfully.";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ConfirmShowreport", "ConfirmShowreport('PurchaseReport.aspx');", true);
                        //Response.Redirect("PurchaseReport.aspx", false);
                    }
                }
            }
            return i;
        }
        public DataTable getPurchase(string from, string to)
        {
            DataTable DT = new DataTable("purchase");
            queryString = "select *,((price_per_kgpc-purchase_price)/purchase_price)*100 as '%' from purchase_history where purchase_date between '" + String.Format("{0:MM/dd/yyyy}", from) + "' and '" + String.Format("{0:MM/dd/yyyy}", to) + "'";
            //ConnectSsh sshobj = new ConnectSsh();
            //SshClient client = sshobj.SshConnect();
            SqlDataAdapter sda = new SqlDataAdapter(queryString, conn);
            sda.Fill(DT);
            //sshobj.SshDisconnect(client);
            return DT;
        }
        public int Truncate_Table(string tablename)
        {
            int i = 0;
            queryString = "TRUNCATE TABLE " +tablename;
            SqlConnection con = new SqlConnection(conn);
            con.Open();
            SqlCommand cmd = new SqlCommand(queryString,con);
            i=cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }
        public DataTable getPurchase(string from, string to, int pid)
        {
            DataTable DT = new DataTable("purchase");
            queryString = "select *,((price_per_kgpc-purchase_price)/purchase_price)*100 as '%' from purchase_history where pid=" + pid + " and purchase_date between '" + String.Format("{0:MM/dd/yyyy}", from) + "' and '" + String.Format("{0:MM/dd/yyyy}", to) + "'";
            //ConnectSsh sshobj = new ConnectSsh();
            //SshClient client = sshobj.SshConnect();
            SqlDataAdapter sda = new SqlDataAdapter(queryString, conn);
            sda.Fill(DT);
            //sshobj.SshDisconnect(client);
            return DT;
        }
        protected void btndownload_OnClick(object sender, EventArgs e)
        {
            exceldownload();
            //ExportGridToCSV();
        }
        private void ExportGridToCSV()
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=purchase.csv");
            Response.Charset = "";
            Response.ContentType = "application/text";
            gvpurchase.AllowPaging = false;


            StringBuilder columnbind = new StringBuilder();
            for (int k = 0; k < gvpurchase.Columns.Count; k++)
            {

                columnbind.Append(gvpurchase.Columns[k].HeaderText + ',');
            }

            columnbind.Append("\r\n");
            for (int i = 0; i < gvpurchase.Rows.Count; i++)
            {
                for (int k = 0; k < gvpurchase.Columns.Count; k++)
                {

                    columnbind.Append(gvpurchase.Rows[i].Cells[k].Text + ',');
                }

                columnbind.Append("\r\n");
            }
            Response.Output.Write(columnbind.ToString());
            Response.Flush();
            Response.End();

        }
        public void exceldownload()
        {
            try
            {
                
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "purchase.xls"));
                Response.ContentType = "application/ms-excel";
                //Response.ContentType = "application/text";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gvpurchase.AllowPaging = false;
                //Change the Header Row back to white color
                //gvpurchase.HeaderRow.Style.Add("background-color", "#FFFFFF");
                ////Applying stlye to gridview header cells
                //for (int i = 0; i < gvpurchase.HeaderRow.Cells.Count; i++)
                //{
                //    gvpurchase.HeaderRow.Cells[i].Style.Add("background-color", "#FFFFFF");
                //    gvpurchase.HeaderRow.Cells[i].Style.Add("color", "#000000");
                //}
                //int j = 1;
                ////This loop is used to apply stlye to cells based on particular row
                //foreach (GridViewRow gvrow in gvpurchase.Rows)
                //{
                //    //gvrow.BackColor = Color.WHITE.ToString;
                //    //if (j <= GvPackList3.Rows.Count)
                //    //{
                //    //if (j % 2 != 0)
                //    //{
                //    for (int k = 0; k < gvrow.Cells.Count; k++)
                //    {
                //        gvrow.Cells[k].Style.Add("background-color", "#FFFFFF");
                //        gvrow.Cells[k].Style.Add("color", "#000000");
                //    }
                //    //}
                //    //}
                //    //j++;
                //}
                System.Web.UI.HtmlControls.HtmlForm f = new System.Web.UI.HtmlControls.HtmlForm();
                f.Controls.Add(gvpurchase);
                //GVOrderDetails2.DataBind();

                gvpurchase.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
            catch (Exception ex)
            {
            }
        }
        protected void Btnupload_OnClick(object sender, EventArgs e)
        {
            try
            {

                int i = 0;
                if (FUpurchase.HasFile)
                {
                    string file = FUpurchase.PostedFile.FileName.ToString().ToLower();
                    if (file.EndsWith(".xls"))
                    {
                        if (File.Exists(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["PurchaseFilePath"].ToString() + "purchase.xls")))
                        {
                            File.Delete(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["PurchaseFilePath"].ToString() + "purchase.xls"));
                            FUpurchase.SaveAs(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["PurchaseFilePath"].ToString() + "purchase.xls"));

                            Upload_Purchase_Data();
                            //i = exefiletodatabase(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["PurchaseFilePath"].ToString() + "purchase.csv"), "purchase");
                            //if (i > 0)
                            //{
                            //    i = databasetohistory("purchase", "purchase_History");
                            //    if (i > 0)
                            //    {
                            //        lblerror.Text = "Upload Successfully.";
                            //        ScriptManager.RegisterStartupScript(this, GetType(), "ConfirmShowreport", "ConfirmShowreport('PurchaseReport.aspx');", true);
                            //        //Response.Redirect("PurchaseReport.aspx", false);
                            //    }
                            //}


                            //databasetohistory();
                        }
                        else
                        {
                            FUpurchase.SaveAs(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["PurchaseFilePath"].ToString() + "purchase.xls"));
                            Upload_Purchase_Data();
                            //i = exefiletodatabase(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["PurchaseFilePath"].ToString() + "purchase.csv"), "purchase");
                            //if (i > 0)
                            //{
                            //    i = databasetohistory("purchase", "purchase_History");
                            //    if (i > 0)
                            //    {
                            //        lblerror.Text = "Upload Successfully.";
                            //        ScriptManager.RegisterStartupScript(this, GetType(), "ConfirmShowreport", "ConfirmShowreport('PurchaseReport.aspx');", true);
                            //        //Response.Redirect("PurchaseReport.aspx", false);
                            //    }
                            //}
                            //databasetohistory();
                        }
                    }
                    else
                    {
                        lblerror.Text = "upload only .xls file";
                    }
                }
                else
                {
                    lblerror.Text = "Plese select a file";
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg + ex.ToString();
            }
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
                SqlCommand command = new SqlCommand("sp_loadhistory", sqlConnection);
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

        
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                //MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        public void databasetohistory()
        {
            try
            {
                System.Data.OleDb.OleDbConnection MyConnection;
                System.Data.OleDb.OleDbCommand myCommand = new System.Data.OleDb.OleDbCommand();
                string sql = null;
                string filepath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["PurchaseFilePath"].ToString()) + "purchase.xls";
                MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + filepath + "';Extended Properties=Excel 8.0;");
                MyConnection.Open();
                //sql = "select ordernumber,customername,productid,name,(weight+' '+units)as weight,(baseweight+'/'+baseprice)as units,actualprice,scannedweight,scannedprice,subtotal,discountamount,orderstatus from [Sheet1$] where ordernumber='" + lblordernumber.Text.ToString() + "'";
                //sql = "select ordernumber,customername,productid,name,(baseweight+' '+units) as weight,qtyordered as units,baseprice,scannedweight,scannedprice,subtotal,discountamount,orderstatus,address from [Sheet1$] where ordernumber='" + lblordernumber.Text.ToString() + "'";
                sql = "select * from [purchase$]";
                DataTable dt = new DataTable();
                System.Data.OleDb.OleDbDataAdapter DataAdapter = new System.Data.OleDb.OleDbDataAdapter(sql, MyConnection);
                //DataAdapter.SelectCommand.Connection = MyConnection;
                //DataAdapter.SelectCommand.CommandText = sql;
                DataAdapter.Fill(dt);
                //Response.Write(dt.Rows[0][0].ToString());
                MyConnection.Close();
                if (dt.Rows.Count > 0)
                {
                    Boolean s = BulkInsertDataTable("purchase", dt);
                    if (s == false)
                    {
                        lblerror.Text = "Try Again Later";
                    }
                    else
                    {
                        lblerror.Text = "Success";
                        Response.Redirect("PurchaseReport.aspx", false);
                    }
                }
            }
            catch(Exception ex)
            {
                //Response.Write(ex.ToString);
                lblerror.Text = "Try Again. "+ex.ToString();
            }

        }


        public bool BulkInsertDataTable(string tableName, DataTable dataTable)
        {
            bool isSuccuss;
            try
            {
                SqlConnection SqlConnectionObj = new SqlConnection(conn);
                SqlConnectionObj.Open();
                SqlCommand cmd = new SqlCommand("delete from purchase", SqlConnectionObj);
                int i=cmd.ExecuteNonQuery();
                
                SqlBulkCopy bulkCopy = new SqlBulkCopy(SqlConnectionObj, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction, null);
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.WriteToServer(dataTable);
                isSuccuss = true;
                SqlConnectionObj.Close();

                databasetohistory("purchase", "purchase_History");
            }
            catch (Exception ex)
            {
                isSuccuss = false;
            }
            return isSuccuss;
        }

        //public void databasetohistory1()
        //{

        //    try
        //    {
        //        oXL = new Excel.Application();
        //        Excel._Worksheet oSheet = null;

        //        //Open the Excel 
        //        oWB = oXL.Workbooks.Open(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["PurchaseFilePath"].ToString() + "purchase.xls"), 0, false, 5, "", "",
        //            false, Excel.XlPlatform.xlWindows, "", true, false,
        //            0, true, false, false);


        //        //Get Existing available object throgh session                   
        //        oSheet = (Excel._Worksheet)oWB.Worksheets["purchase"];
        //        //Get Existing available object throgh session                       

        //        //#region "Insert Basic Information"

        //        if (((Excel.Range)oSheet.Cells[5, 4]).Value2 != null && ((Excel.Range)oSheet.Cells[5, 4]).Value2 != "")
        //        {
        //            Response.Write(((Excel.Range)oSheet.Cells[5, 4]).Value2.ToString());
        //        }
        //        oWB.Close(true, null, null);
        //        oXL.Quit();

        //        releaseObject(oSheet);
        //        releaseObject(oWB);
        //        releaseObject(oXL);
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write(ex.ToString());
        //    }
        //}
        public DataTable get_ffhp_scan_orders(DateTime fromdate, DateTime todate)
        {
            DataTable DT = new DataTable("ffhpscanorders");
            try
            {
                
                DateTime cd = DateTime.Now;
                SqlConnection SqlConnectionObj = new SqlConnection(conn);
                SqlConnectionObj.Open();
                
                queryString = "select * from ffhporders where updateddate between '" + fromdate.ToString("yyyy-MM-dd") + "' and '" + todate.AddDays(1).ToString("yyyy-MM-dd") + "'";
                SqlDataAdapter sda = new SqlDataAdapter(queryString,SqlConnectionObj);
                sda.Fill(DT);
                SqlConnectionObj.Close();
                
                             
            }
            catch (Exception ex)
            {

            }
            return DT;   
        }
        public DataTable get_ffhp_before_after_sale(DateTime fromdate, DateTime todate)
        {
            DataTable DT = new DataTable("ffhpbeforeaftersale");
            try
            {
                
                SqlConnection SqlConnectionObj = new SqlConnection(conn);
                SqlConnectionObj.Open();

                queryString = "select * from ffhp_before_after_sale where updateddate between '" + fromdate.ToString("yyyy-MM-dd") + "' and '" + todate.AddDays(1).ToString("yyyy-MM-dd") + "'";
                SqlDataAdapter sda = new SqlDataAdapter(queryString, SqlConnectionObj);
                sda.Fill(DT);
                SqlConnectionObj.Close();

            }
            catch (Exception ex)
            {

            }
            return DT;
        }
    }
}
        
