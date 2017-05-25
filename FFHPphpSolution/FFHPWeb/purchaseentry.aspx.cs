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
using System.Web.UI.HtmlControls;
using System.Globalization;

namespace FFHPWeb
{
    public partial class purchaseentry : System.Web.UI.Page
    {
        string conn = System.Configuration.ConfigurationManager.AppSettings["SqlConnectionString"].ToString();
        string queryString = "";
        MySqlDataAdapter DA;
        string s = "";
        bool t = false;
        string errormsg = "Try again";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //missingitems obj = new missingitems();
                //obj.exportToExcel();
                //PurchaseBudget obj = new PurchaseBudget();
                //obj.exportToExcel();
                
                //bind();
                //bindpurchaseentry();
                bindvendordetails();
            }
            lblerror.Text = "";
            lblerror.ForeColor = System.Drawing.Color.Black;
        }
        public void bind()
        {

            DataTable dt = new DataTable();
            try
            {
                SqlConnection con=new SqlConnection(conn);
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_products", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("productid", 0);
                cmd.Parameters.AddWithValue("name", "");
                cmd.Parameters.AddWithValue("operation", "select");
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
                //APIMethods objmobileorders = new APIMethods();
                //dt = objmobileorders.getProduct_Weight_Price_for_Before_After_Sale();


                //dtpurchase.Columns.Add("purchase_date", typeof(DateTime), System.DateTime.Now.ToShortDateString());
                //gvpurchase.DataSource = dt;
                //gvpurchase.DataBind();
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
            ViewState["product_weight_price"] = dt;

        }
        public void bindvendordetails()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(conn);
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_vendordetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("vendorid", 0);
                cmd.Parameters.AddWithValue("vendorname", "");
                cmd.Parameters.AddWithValue("telephone", "");
                cmd.Parameters.AddWithValue("operation", "select");
                SqlParameter outputparam = new SqlParameter();
                outputparam.ParameterName = "@output";
                outputparam.DbType = DbType.Int32;
                outputparam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputparam);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
                ddlvendor.DataSource = dt;
                ddlvendor.Items.Add("--Select--");
                ddlvendor.DataBind();
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
        }
        protected void ddlvendor_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            gvvendorproducts1.EditIndex = -1;
            Bindvendorproducts();
        }
        void Bindvendorproducts()
        {
            
                DataTable dt = new DataTable();
                try
                {
                    if (ddlvendor.SelectedIndex > 0)
                    {
                        //SqlConnection con = new SqlConnection(conn);
                        //con.Open();
                        //SqlCommand cmd = new SqlCommand("sp_vendor_products", con);
                        //cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("id", 0);
                        //cmd.Parameters.AddWithValue("vendorid", Convert.ToInt32(ddlvendor.SelectedValue.ToString()));
                        //cmd.Parameters.AddWithValue("productid", "");
                        //cmd.Parameters.AddWithValue("operation", "select_vendorwise");
                        //SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        //sda.Fill(dt);
                        //con.Close();
                        //DateTime dtf = DateTime.ParseExact(DateTime.Now.Date.ToString("dd-MM-yyyy"), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        using (SqlConnection con = new SqlConnection(conn))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_purchase_entry", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@purchase_entry_id", SqlDbType.Int).Value = 0;
                                cmd.Parameters.Add("@vendorid", SqlDbType.Int).Value = Convert.ToInt32(ddlvendor.SelectedValue.ToString());
                                cmd.Parameters.Add("@productid", SqlDbType.Int).Value = 0;
                                cmd.Parameters.Add("@actual_weight", SqlDbType.Decimal).Value = 0;
                                cmd.Parameters.Add("@extended_weight", SqlDbType.Decimal).Value = 0;
                                cmd.Parameters.Add("@inward_stock", SqlDbType.Decimal).Value = 0;
                                cmd.Parameters.Add("@billed_weight", SqlDbType.Decimal).Value = 0;
                                cmd.Parameters.Add("@price", SqlDbType.Decimal).Value = 0;
                                cmd.Parameters.Add("@lastupdateddate", SqlDbType.DateTime).Value = DateTime.Now.Date.ToShortDateString();
                                cmd.Parameters.Add("@operation", SqlDbType.VarChar).Value = "selectbyvendor";
                                SqlParameter outputparam = new SqlParameter();
                                outputparam.ParameterName = "@output";
                                outputparam.DbType = DbType.Int32;
                                outputparam.Direction = ParameterDirection.Output;
                                cmd.Parameters.Add(outputparam);

                                con.Open();
                                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                                sda.Fill(dt);
                                con.Close();
                            }
                        }
                    }
                    //gvvendorproducts.DataSource = dt;
                    //gvvendorproducts.DataBind();
                    gvvendorproducts1.DataSource = dt;
                    gvvendorproducts1.DataBind();
                }
                catch (Exception ex)
                {
                    lblerror.Text = errormsg.ToString();
                }
            
        }
        

        protected void gvvendorproducts1_OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            gvvendorproducts1.EditIndex = e.NewEditIndex;
            Bindvendorproducts();
        }

        protected void gvvendorproducts1_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                if (ddlvendor.SelectedIndex > 0)
                {
                    //string tbxbilledweight = gvvendorproducts.Rows[e.RowIndex].Cells[7].Text.ToString();
                    //string tbxprice = gvvendorproducts.Rows[e.RowIndex].Cells[8].Text.ToString();
                    string tbxinwardstock = (gvvendorproducts1.Rows[e.RowIndex].Cells[6].Controls[0] as TextBox).Text;
                    string tbxbilledweight = (gvvendorproducts1.Rows[e.RowIndex].Cells[7].Controls[0] as TextBox).Text;
                    string tbxprice = (gvvendorproducts1.Rows[e.RowIndex].Cells[8].Controls[0] as TextBox).Text;
                    HiddenField hfpurchase_entry_id = (HiddenField)gvvendorproducts1.Rows[e.RowIndex].FindControl("hfpurchase_entry_id");
                    string s = gvvendorproducts1.Rows[e.RowIndex].Cells[2].Text.ToString();
                    //double receivedqty = Convert.ToDouble(gvvendorproducts1.Rows[e.RowIndex].Cells[6].Text.ToString());
                    double receivedqty = Convert.ToDouble((gvvendorproducts1.Rows[e.RowIndex].Cells[6].Controls[0] as TextBox).Text);
                    //bool t = false;
                    //if (Convert.ToDouble(tbxbilledweight.Text) <= receivedqty)
                    //{
                    //    t = true;
                    //}
                    //else
                    //{
                    //    double qtydiff = Convert.ToDouble(tbxbilledweight.Text) - receivedqty;
                    //    if (qtydiff < 0.100)
                    //    {
                    //        t = true;
                    //    }
                    //}
                    //if (t)
                    //{
                    if (tbxbilledweight != "" && tbxbilledweight != "0")
                    {
                        if (tbxprice != "" && tbxprice != "0")
                        {
                            using (SqlConnection con = new SqlConnection(conn))
                            {
                                using (SqlCommand cmd = new SqlCommand("sp_purchase_entry", con))
                                {
                                    if (hfpurchase_entry_id.Value.ToString()=="")
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@purchase_entry_id", SqlDbType.Int).Value = 0;
                                        cmd.Parameters.Add("@vendorid", SqlDbType.Int).Value = Convert.ToInt32(ddlvendor.SelectedValue.ToString());
                                        cmd.Parameters.Add("@productid", SqlDbType.Int).Value = Convert.ToInt32(gvvendorproducts1.Rows[e.RowIndex].Cells[1].Text.ToString());
                                        cmd.Parameters.Add("@actual_weight", SqlDbType.Float).Value = Convert.ToDouble(gvvendorproducts1.Rows[e.RowIndex].Cells[4].Text.ToString());
                                        cmd.Parameters.Add("@extended_weight", SqlDbType.Float).Value = Convert.ToDouble(gvvendorproducts1.Rows[e.RowIndex].Cells[5].Text.ToString());
                                        cmd.Parameters.Add("@inward_stock", SqlDbType.Float).Value = Convert.ToDouble(tbxinwardstock);
                                        cmd.Parameters.Add("@billed_weight", SqlDbType.Float).Value = Convert.ToDouble(tbxbilledweight);
                                        cmd.Parameters.Add("@price", SqlDbType.Float).Value = tbxprice;
                                        cmd.Parameters.Add("@lastupdateddate", SqlDbType.DateTime).Value = DateTime.Now;
                                        cmd.Parameters.Add("@operation", SqlDbType.VarChar).Value = "insert";
                                        SqlParameter outputparam = new SqlParameter();
                                        outputparam.ParameterName = "@output";
                                        outputparam.DbType = DbType.Int32;
                                        outputparam.Direction = ParameterDirection.Output;
                                        cmd.Parameters.Add(outputparam);
                                    }
                                    else
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@purchase_entry_id", SqlDbType.Int).Value = Convert.ToInt32(hfpurchase_entry_id.Value.ToString());
                                        cmd.Parameters.Add("@vendorid", SqlDbType.Int).Value = Convert.ToInt32(ddlvendor.SelectedValue.ToString());
                                        cmd.Parameters.Add("@productid", SqlDbType.Int).Value = Convert.ToInt32(gvvendorproducts1.Rows[e.RowIndex].Cells[1].Text.ToString());
                                        cmd.Parameters.Add("@actual_weight", SqlDbType.Float).Value = Convert.ToDouble(gvvendorproducts1.Rows[e.RowIndex].Cells[4].Text.ToString());
                                        cmd.Parameters.Add("@extended_weight", SqlDbType.Float).Value = Convert.ToDouble(gvvendorproducts1.Rows[e.RowIndex].Cells[5].Text.ToString());
                                        cmd.Parameters.Add("@inward_stock", SqlDbType.Float).Value = Convert.ToDouble(tbxinwardstock);
                                        cmd.Parameters.Add("@billed_weight", SqlDbType.Float).Value = Convert.ToDouble(tbxbilledweight);
                                        cmd.Parameters.Add("@price", SqlDbType.Float).Value = tbxprice;
                                        cmd.Parameters.Add("@lastupdateddate", SqlDbType.DateTime).Value = DateTime.Now;
                                        cmd.Parameters.Add("@operation", SqlDbType.VarChar).Value = "update";
                                        SqlParameter outputparam = new SqlParameter();
                                        outputparam.ParameterName = "@output";
                                        outputparam.DbType = DbType.Int32;
                                        outputparam.Direction = ParameterDirection.Output;
                                        cmd.Parameters.Add(outputparam);
                                    }
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    int i = Convert.ToInt32(cmd.Parameters["@output"].Value.ToString());
                                    if (hfpurchase_entry_id.Value.ToString()=="")
                                    {
                                        if (i > 0)
                                        {
                                            lblerror.Text = "Successfully Saved";
                                            gvvendorproducts1.EditIndex = -1;
                                            Bindvendorproducts();
                                        }
                                        else
                                        {
                                            lblerror.Text = "Not Successfully Saved";
                                        }
                                    }
                                    else
                                    {
                                        if (i > 0)
                                        {
                                            lblerror.Text = "Successfully Updated";
                                            gvvendorproducts1.EditIndex = -1;
                                            Bindvendorproducts();
                                        }
                                        else
                                        {
                                            lblerror.Text = "Not Successfully Updated";
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //}
                    //else
                    //{
                    //    lblerror.Text = "Please check your Billed qty and Received qty";
                    //}
                }
                else
                {
                    lblerror.Text = "Please select a vendor";
                }
            }
            catch (Exception ex)
            {

            }
            Bindvendorproducts();
        }
        protected void gvvendorproducts1_OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvvendorproducts1.EditIndex = -1; 
            Bindvendorproducts();
        }

        // Start code for budget calculation
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
                lblerror.Text = errormsg.ToString();
            }
            return dt;
        }
        public DataTable purchase_budget()
        {
            DataTable LPRData = new DataTable();
            TotalWeightlprdata objlprdata = new TotalWeightlprdata();
            LPRData = objlprdata.getffhpproducts();

            DataTable productpricedt = new DataTable();
            productpricedt=getproductprice();

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
            
            weightcalculate obj = new weightcalculate();
            DataTable dt= obj.calculate();
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
                budgetdtrow["purchasedate"]=DateTime.Today.ToShortDateString();
                budgetdtrow["productid"]=row["Product_Id"].ToString();
                budgetdtrow["productname"]=row["Name"].ToString();
                budgetdtrow["purchaseqty"] = row["TotalWeight"].ToString();
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
                    budgetdtrow["budgetprice"] = Convert.ToDouble(row["TotalWeight"].ToString()) * Convert.ToDouble(productprice.First().col1.ToString());
                }
                else
                {
                    budgetdtrow["budgetprice"] = 0;
                }
                budgetdt.Rows.Add(budgetdtrow);
            }


            var sumGreen = budgetdt.AsEnumerable()
            .Where(r => r.Field<string>("product_group")=="Green")
            .Sum(r =>  Convert.ToDouble(r.Field<string>("budgetprice")));

            var sumVeg = budgetdt.AsEnumerable()
            .Where(r => r.Field<string>("product_group") == "Veg")
            .Sum(r => Convert.ToDouble(r.Field<string>("budgetprice")));

            var sumFruit = budgetdt.AsEnumerable()
            .Where(r => r.Field<string>("product_group") == "Fruit")
            .Sum(r => Convert.ToDouble(r.Field<string>("budgetprice")));

            return budgetdt;

        }
        private void exportToExcel()
        {
            //create a folder
            string directorypath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["FilePathBudget"].ToString() + DateTime.Now.ToString("dd-MM-yyyy"));
            if (Directory.Exists(directorypath)==false)
            {
                Directory.CreateDirectory(directorypath);
            }
            string filename = "PurchaseBudget" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss") + ".xls";
            //string filename = "PurchaseBudget" + DateTime.Now.ToString("dd-MM-yyyy") +".xls";
            string filepath = System.Configuration.ConfigurationManager.AppSettings["FilePathBudget"].ToString()+ DateTime.Now.ToString("dd-MM-yyyy")+"/" + filename;

            //string delfilename = "PurchaseBudget" + DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy") + ".xls";
            string deldirectorypath = System.Configuration.ConfigurationManager.AppSettings["FilePathBudget"].ToString() + DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");

            if (Directory.Exists(Server.MapPath(deldirectorypath)))
            {
                DirectoryInfo di = new DirectoryInfo(Server.MapPath(deldirectorypath));

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                Directory.Delete(Server.MapPath(deldirectorypath));
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
                obook.SaveAs(Server.MapPath(filepath), Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, false, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
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
                mail.Body = "PFA - PurchaseBudget(XLS) <br><br>" + "Vegitable Budget is " + sumVeg.ToString() + "<br> Fruits Budget is " + sumFruit.ToString() + "<br> Greens Budget is " + sumGreen.ToString();
                mail.IsBodyHtml = true;

                System.Net.Mail.Attachment attachment;
                //attachment = new System.Net.Mail.Attachment(Server.MapPath("MailFiles/CustomerInformation/" + Session.SessionID +s+ ".xls"));
                attachment = new System.Net.Mail.Attachment(Server.MapPath(filepath));
                mail.Attachments.Add(attachment);

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(mailcredential, mailpassword);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                lblerror.Text = "Mail sent successfully.";
            }
            catch (Exception ex)
            {
                xlApp.Quit();
            }
        }
        // End code for budget calculation

        // Start code no use and just reference
        public void exportmail()
        {
            try
            {
                
                string filepath = System.Configuration.ConfigurationManager.AppSettings["FilePath"].ToString();

                gvmail.Visible = true;
                string filename = "PurchaseBudget" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss");
                //Response.ContentType = "application/ms-excel";
                //Response.AddHeader("content-disposition", "attachment;filename=CustomerInfo.xls");
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                System.Web.UI.HtmlControls.HtmlForm f = new System.Web.UI.HtmlControls.HtmlForm();
                //Panel Tom = new Panel();
                //Tom.ID = base.UniqueID;
                //Tom.Controls.Add(myControl);
                //Page.FindControl("WebForm1").Controls.Add(Tom);

                gvmail.AllowPaging = false;
                HtmlForm frm = new HtmlForm();
                gvmail.Parent.Controls.Add(frm);
                frm.Attributes["runat"] = "server";
                frm.Controls.Add(gvmail);
                frm.RenderControl(hw);
                

                //f.Controls.Add(GVMail);
                //GVOrderDetails2.DataBind();
                //GVMail.RenderControl(hw);
                //GVOrderDetails2.HeaderRow.Style.Add("width", "15%");
                //GVOrderDetails2.HeaderRow.Style.Add("font-size", "10px");
                //GVOrderDetails2.Style.Add("text-decoration", "none");
                //GVOrderDetails2.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                //GVOrderDetails2.Style.Add("font-size", "8px");

                StreamWriter writer = File.AppendText(Server.MapPath(filepath + filename + ".xls"));
                //Response.WriteFile(Server.MapPath("MailFiles/CustomerInformation/" + Session.SessionID + ".xls"));
                writer.WriteLine(sw.ToString());
                writer.Close();
                gvmail.Visible = false;


                string mailto = System.Configuration.ConfigurationManager.AppSettings["Mail_To"].ToString();
                string mailcredential = System.Configuration.ConfigurationManager.AppSettings["Mail_Credential"].ToString();
                string mailpassword = System.Configuration.ConfigurationManager.AppSettings["Mail_Password"].ToString();

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress(mailcredential);
                mail.To.Add(mailto);
                mail.Subject = "PFA - PurchaseBudget(XLS)";
                mail.Body = "PFA - PurchaseBudget(XLS)"+sw.ToString();
                mail.IsBodyHtml = true;

                System.Net.Mail.Attachment attachment;
                //attachment = new System.Net.Mail.Attachment(Server.MapPath("MailFiles/CustomerInformation/" + Session.SessionID +s+ ".xls"));
                attachment = new System.Net.Mail.Attachment(Server.MapPath(filepath + filename + ".xls"));
                mail.Attachments.Add(attachment);

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(mailcredential, mailpassword);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                lblerror.Text = "Mail sent successfully.";
                //MessageBox.Show("mail Send");

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
                lblerror.Text = errormsg;//ex.ToString();
            }
        }
        private byte[] ConvertDataSetToByteArray(DataTable dataTable)
        {
            byte[] binaryDataResult = null;
            using (MemoryStream memStream = new MemoryStream())
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter brFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                dataTable.RemotingFormat = SerializationFormat.Binary;
                brFormatter.Serialize(memStream, dataTable);
                binaryDataResult = memStream.ToArray();
            }
            return binaryDataResult;
        }
        public System.IO.MemoryStream ExportToStream(DataTable objDt)
        {

            //remove any columns specified.

            //foreach (string colName in ColumnsToRemove)
            //{

            //    objDt.Columns.Remove(colName);

            //}



            gvmail.DataSource = objDt;

            DataBind();



            System.IO.StringWriter sw = new System.IO.StringWriter();

            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);

            HtmlForm frm = new HtmlForm();
            gvmail.Parent.Controls.Add(frm);
            frm.Attributes["runat"] = "server";
            frm.Controls.Add(gvmail);
            frm.RenderControl(hw);

            //gvmail.RenderControl(hw);

            string content = sw.ToString();

            byte[] byteData = Encoding.Default.GetBytes(content);



            System.IO.MemoryStream mem = new System.IO.MemoryStream();

            mem.Write(byteData, 0, byteData.Length);

            mem.Flush();

            mem.Position = 0; //reset position to the begining of the stream

            return mem;

        }
//        public void exportexcel1()
//        {
//            DirectExcel excel = new DirectExcel();

//System.IO.MemoryStream ms = excel.ExportToStream(myDataTable);

//Attachment attachFile = new Attachment(ms, "filename.xls", "application/vnd.ms-excel");

//MailMessage mail = new MailMessage();

//mail.Attachments.Add(attachFile);
//        }
        public void ExportExcel()
        {
            //Get the GridView Data from database.
            DataTable dt = purchase_budget();
            
            //byte[] bytes = ConvertDataSetToByteArray(dt);
            System.IO.MemoryStream ms=ExportToStream(dt);
            
            //Send Email with Excel attachment.
            using (MailMessage mm = new MailMessage("senthil@binarch.in", "senthil@binarch.in"))
            {
                mm.Subject = "GridView Exported Excel";
                mm.Body = "GridView Exported Excel Attachment";

                //Add Byte array as Attachment.
                mm.Attachments.Add(new Attachment(ms, "GridView.xls"));
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential();
                credentials.UserName = "demo.binarch@gmail.com";
                credentials.Password = "binarch.demo";
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = credentials;
                smtp.Port = 587;
                smtp.Send(mm);
            }

        }
        public void ExportToExcel(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                string filename = "DownloadPurchaseBudgetExcel.xls";
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                DataGrid dgGrid = new DataGrid();
                dgGrid.DataSource = dt;
                dgGrid.DataBind();

                //Get the HTML for the control.
                dgGrid.RenderControl(hw);
                //Write the HTML back to the browser.
                //Response.ContentType = application/vnd.ms-excel;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
                this.EnableViewState = false;
                Response.Write(tw.ToString());
                Response.End();
                
            }
        }
        public void SendAutomatedEmail()
        {
        try
        {
            DataTable dt = purchase_budget();
            ExportToExcel(dt);

            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();

            msg.Subject = "Testing Email";

            msg.From = new System.Net.Mail.MailAddress("senthil@binarch.in");

            msg.To.Add(new System.Net.Mail.MailAddress("senthil@binarch.in"));

            msg.Body = dt.ToString()+"Testmail";



            System.Net.Mail.SmtpClient smpt = new System.Net.Mail.SmtpClient();

            smpt.Host = "smtp.gmail.com";

            smpt.Port = 587;

            smpt.EnableSsl = true;

            smpt.Credentials = new System.Net.NetworkCredential("demo.binarch@gmail.com", "binarch.demo");

            smpt.Send(msg);
        }
        catch (Exception e)
        {

        }

        }
        // End code no use and just reference

        /*protected void gvvendorproducts_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //TextBox tbxbilledweight = (TextBox)gvvendorproducts.Rows[e.Row.RowIndex].FindControl("tbxweight");
                //TextBox tbxprice = (TextBox)gvvendorproducts.Rows[e.Row.RowIndex].FindControl("tbxprice");
                HiddenField hfpurchase_entry_id = (HiddenField)e.Row.FindControl("hfpurchase_entry_id");
                Button btnsave = (Button)e.Row.FindControl("btnsave");
                if (hfpurchase_entry_id.Value!="")
                {
                    btnsave.Text = "Update";
                }
                else
                {
                    btnsave.Text = "Save";
                }
            }
        }
        protected void btnsave_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (ddlvendor.SelectedIndex > 0)
                {
                    GridViewRow gvrow = (GridViewRow)(((Control)sender).NamingContainer);
                    TextBox tbxbilledweight = (TextBox)gvvendorproducts.Rows[gvrow.RowIndex].FindControl("tbxweight");
                    TextBox tbxprice = (TextBox)gvvendorproducts.Rows[gvrow.RowIndex].FindControl("tbxprice");
                    Button btnsave = (Button)gvvendorproducts.Rows[gvrow.RowIndex].FindControl("btnsave");
                    HiddenField hfpurchase_entry_id = (HiddenField)gvvendorproducts.Rows[gvrow.RowIndex].FindControl("hfpurchase_entry_id");
                    string s = gvvendorproducts.Rows[gvrow.RowIndex].Cells[2].Text.ToString();
                    double receivedqty = Convert.ToDouble(gvvendorproducts.Rows[gvrow.RowIndex].Cells[6].Text.ToString());
                    //bool t = false;
                    //if (Convert.ToDouble(tbxbilledweight.Text) <= receivedqty)
                    //{
                    //    t = true;
                    //}
                    //else
                    //{
                    //    double qtydiff = Convert.ToDouble(tbxbilledweight.Text) - receivedqty;
                    //    if (qtydiff < 0.100)
                    //    {
                    //        t = true;
                    //    }
                    //}
                    //if (t)
                    //{
                        if (tbxbilledweight.Text != "" && tbxbilledweight.Text != "0")
                        {
                            if (tbxprice.Text != "" && tbxprice.Text != "0")
                            {
                                using (SqlConnection con = new SqlConnection(conn))
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_purchase_entry", con))
                                    {
                                        if (btnsave.Text.ToString() == "Save")
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.Add("@purchase_entry_id", SqlDbType.Int).Value = 0;
                                            cmd.Parameters.Add("@vendorid", SqlDbType.Int).Value = Convert.ToInt32(ddlvendor.SelectedValue.ToString());
                                            cmd.Parameters.Add("@productid", SqlDbType.Int).Value = Convert.ToInt32(gvvendorproducts.Rows[gvrow.RowIndex].Cells[1].Text.ToString());
                                            cmd.Parameters.Add("@actual_weight", SqlDbType.Float).Value = Convert.ToDouble(gvvendorproducts.Rows[gvrow.RowIndex].Cells[4].Text.ToString());
                                            cmd.Parameters.Add("@extended_weight", SqlDbType.Float).Value = Convert.ToDouble(gvvendorproducts.Rows[gvrow.RowIndex].Cells[5].Text.ToString());
                                            cmd.Parameters.Add("@inward_stock", SqlDbType.Float).Value = Convert.ToDouble(gvvendorproducts.Rows[gvrow.RowIndex].Cells[6].Text.ToString());
                                            cmd.Parameters.Add("@billed_weight", SqlDbType.Float).Value = Convert.ToDouble(tbxbilledweight.Text);
                                            cmd.Parameters.Add("@price", SqlDbType.Float).Value = tbxprice.Text;
                                            cmd.Parameters.Add("@lastupdateddate", SqlDbType.DateTime).Value = DateTime.Now;
                                            cmd.Parameters.Add("@operation", SqlDbType.VarChar).Value = "insert";
                                            SqlParameter outputparam = new SqlParameter();
                                            outputparam.ParameterName = "@output";
                                            outputparam.DbType = DbType.Int32;
                                            outputparam.Direction = ParameterDirection.Output;
                                            cmd.Parameters.Add(outputparam);
                                        }
                                        else
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.Add("@purchase_entry_id", SqlDbType.Int).Value = Convert.ToInt32(hfpurchase_entry_id.Value.ToString());
                                            cmd.Parameters.Add("@vendorid", SqlDbType.Int).Value = Convert.ToInt32(ddlvendor.SelectedValue.ToString());
                                            cmd.Parameters.Add("@productid", SqlDbType.Int).Value = Convert.ToInt32(gvvendorproducts.Rows[gvrow.RowIndex].Cells[1].Text.ToString());
                                            cmd.Parameters.Add("@actual_weight", SqlDbType.Float).Value = Convert.ToDouble(gvvendorproducts.Rows[gvrow.RowIndex].Cells[4].Text.ToString());
                                            cmd.Parameters.Add("@extended_weight", SqlDbType.Float).Value = Convert.ToDouble(gvvendorproducts.Rows[gvrow.RowIndex].Cells[5].Text.ToString());
                                            cmd.Parameters.Add("@inward_stock", SqlDbType.Float).Value = Convert.ToDouble(gvvendorproducts.Rows[gvrow.RowIndex].Cells[6].Text.ToString());
                                            cmd.Parameters.Add("@billed_weight", SqlDbType.Float).Value = Convert.ToDouble(tbxbilledweight.Text);
                                            cmd.Parameters.Add("@price", SqlDbType.Float).Value = tbxprice.Text;
                                            cmd.Parameters.Add("@lastupdateddate", SqlDbType.DateTime).Value = DateTime.Now;
                                            cmd.Parameters.Add("@operation", SqlDbType.VarChar).Value = "update";
                                            SqlParameter outputparam = new SqlParameter();
                                            outputparam.ParameterName = "@output";
                                            outputparam.DbType = DbType.Int32;
                                            outputparam.Direction = ParameterDirection.Output;
                                            cmd.Parameters.Add(outputparam);
                                        }
                                        con.Open();
                                        cmd.ExecuteNonQuery();
                                        int i = Convert.ToInt32(cmd.Parameters["@output"].Value.ToString());
                                        if (btnsave.Text == "Save")
                                        {
                                            if (i > 0)
                                            {
                                                lblerror.Text = "Successfully Saved";
                                                Bindvendorproducts();
                                            }
                                            else
                                            {
                                                lblerror.Text = "Not Successfully Saved";
                                            }
                                        }
                                        else
                                        {
                                            if (i > 0)
                                            {
                                                lblerror.Text = "Successfully Updated";
                                                Bindvendorproducts();
                                            }
                                            else
                                            {
                                                lblerror.Text = "Not Successfully Updated";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    //}
                    //else
                    //{
                    //    lblerror.Text = "Please check your Billed qty and Received qty";
                    //}
                }
                else
                {
                    lblerror.Text = "Please select a vendor";
                }
            }
            catch (Exception ex)
            {

            }
        }
          public void bindpurchaseentry()
        {
            DataTable dt = (DataTable)ViewState["product_weight_price"];

            
            ddlpid.DataSource = dt;
            ddlpid.DataBind();

            //ddlname.DataSource = dt;
            //ddlname.DataBind();
            lblproduct.Text = ddlpid.SelectedValue.ToString();

            bindvendordetails1();
        }
        public void bindvendordetails1()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[1] { new DataColumn("Id") });
            dt.Rows.Add(1);
            grdvendorlist.DataSource = dt;
            grdvendorlist.DataBind();
        }
        protected void ddlpid_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            lblproduct.Text = ddlpid.SelectedValue.ToString();
        }
        protected void tbxvendorcount_OnTextChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(tbxvendorcount.Text) > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[1] { new DataColumn("Id") });
                for (int i = 1; i <= Convert.ToInt32(tbxvendorcount.Text); i++)
                {
                    dt.Rows.Add(i);
                }
                grdvendorlist.DataSource = dt;
                grdvendorlist.DataBind();
            }
        }
        */
    }
    
}
