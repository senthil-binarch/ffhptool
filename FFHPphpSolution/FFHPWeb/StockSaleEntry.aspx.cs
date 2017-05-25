using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Globalization;
using Excel = Microsoft.Office.Interop.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Text;
namespace FFHPWeb
{
    public partial class StockSaleEntry : System.Web.UI.Page
    {
        string conn = System.Configuration.ConfigurationManager.AppSettings["SqlConnectionString"].ToString();
        string queryString = "";
        string s = "";
        bool t = false;
        string errormsg = "Try again";
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Exist_records_TotalWeight(indianTime.Date) > 0)
                {
                    lbltotalweightuploadstatus.Text = "Status: Uploaded";
                }
                else
                {
                    lbltotalweightuploadstatus.Text = "Status: Not Uploaded. Please Upload Totalweight";
                }

                if (Exist_records(indianTime.Date) > 0)
                {
                    ddlstocktype.Enabled = true;
                }
                else
                {
                    ddlstocktype.Enabled = false;
                }
                //getstocktypedetails();
                
            }
            lblerror.Text = "";
            lblerror.ForeColor = System.Drawing.Color.Black;
        }
        protected void ddlstocktype_OnSelectedIndexChanged(object sender,EventArgs e)
        {
            //DataTable stocksale = new DataTable();
            //stocksale = (DataTable)ViewState["stocksale"];

            //if (ddlstocktype.SelectedValue.ToString() == "morning")
            //{
            //    var result = from o in stocksale.AsEnumerable()
            //                 where o.Field<decimal>("morningscannedweight") > 0 || o.Field<decimal>("morningpiececount")>0
            //                 select new
            //                 {
            //                     productid = o.Field<int>("productid"),
            //                     Name = o.Field<string>("Name"),
            //                     morningscannedweight = o.Field<decimal>("morningscannedweight"),
            //                     morningpiececount = o.Field<decimal>("morningpiececount"),
            //                     morningtrayweight = o.Field<decimal>("morningtrayweight"),
            //                     morningdescription = o.Field<string>("morningdescription")
            //                 };
            //    //GVstock.DataSource = result;
            //    //GVstock.DataBind();

            //    //rptstocksale.DataSource = result;
            //    //rptstocksale.DataBind();
            //    if (result.Any()==false)
            //    {
                    
            //    }

            //}
            Repeater1.DataSource = tempdatatable();
            Repeater1.DataBind();
            //Repeater1.Visible = false;

            
        }
        public DataTable tempdatatable()
        {
            DataTable tempDT = new DataTable();
            tempDT.Columns.Add("productid",typeof(int));
            tempDT.Columns.Add("Name", typeof(string));
            tempDT.Columns.Add("morningscannedweight", typeof(decimal));
            tempDT.Columns.Add("morningpiececount", typeof(decimal));
            tempDT.Columns.Add("morningtrayweight", typeof(decimal));
            tempDT.Columns.Add("morningdescription", typeof(string));

            DataRow dr = tempDT.NewRow();
            dr["productid"] = 0;
            dr["Name"] = "";
            dr["morningscannedweight"] = 0;
            dr["morningpiececount"] = 0;
            dr["morningtrayweight"] = 0;
            dr["morningdescription"] = "";

            tempDT.Rows.Add(dr);

            return tempDT;
        }
        //protected void btnsubmit_onclick(object sender, EventArgs e)
        //{
        //    string s = "";
        //    foreach (RepeaterItem i in Repeater1.Items)
        //    {
        //        TextBox txtExample = (TextBox)i.FindControl("txtmorningscannedweight");
        //        if (txtExample != null)
        //        {
        //            s += txtExample.Text + "<br />";
        //        }
        //    }
        //}
        public DataTable getstocktypedetails()
        {
            DataTable stocksale = new DataTable();
            //queryString = "select productid,name,morningscannedweight,morningpiececount,morningtrayweight,morningdescription,stockdate from dbo.tool_stockproducts where stockdate='" + DateTime.Now.Date.ToString("MM/dd/yyyy")+"'";
            //if (queryString != "")
            //{
            //    SqlConnection sqlConnection = new SqlConnection(conn);
            //    SqlDataAdapter adapteradminmail = new SqlDataAdapter(queryString, sqlConnection);
            //    adapteradminmail.Fill(stocksale);
            //}
            
            //return stocksale;

            try
            {
                SqlConnection sqlConnection = new SqlConnection(conn);
                SqlCommand command = new SqlCommand("sp_tool_stockproducts_select", sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@stockdate", SqlDbType.DateTime).Value = indianTime.Date.ToString();
                sqlConnection.Open();
                SqlDataAdapter sda=new SqlDataAdapter(command);
                sda.Fill(stocksale);
                sqlConnection.Close();
                ViewState["stocksale"] = stocksale;
            }
            catch (SqlException ex)
            {
                
            }
            return stocksale;
        }
        protected void Btnupload_OnClick(object sender, EventArgs e)
        {
            try
            {
                int i = 0;
                if (FUtotalweight.HasFile)
                {
                    string file = FUtotalweight.PostedFile.FileName.ToString().ToLower();
                    if (file.EndsWith(".xls"))
                    {
                        if (File.Exists(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["StocksaleFilePath"].ToString() + "totalweight.xls")))
                        {
                            File.Delete(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["StocksaleFilePath"].ToString() + "totalweight.xls"));
                            FUtotalweight.SaveAs(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["StocksaleFilePath"].ToString() + "totalweight.xls"));
                            Upload_totalweight_Data();
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
                            FUtotalweight.SaveAs(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["StocksaleFilePath"].ToString() + "totalweight.xls"));
                            Upload_totalweight_Data();
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
                lblerror.Text = errormsg + " " + ex.ToString();
            }
        }
        public int Upload_totalweight_Data()
        {
            DataTable dt = new DataTable();
            dt = Readdata_from_excel_to_datatable("totalweight.xls");
            int i = 0;
            if (dt.Rows.Count > 0)
            {
                Delete_records(Convert.ToDateTime(dt.Rows[0]["date"].ToString()));

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                {
                    bulkCopy.DestinationTableName =
                        "dbo.tool_totalweight";

                    try
                    {
                        // Write from the source to the destination.
                        bulkCopy.WriteToServer(dt);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    //i = databasetohistory("stockproducts", "stockproducts_history");
                    //if (i > 0)
                    //{
                       lblerror.Text = "Upload Successfully.";
                        
                    //}
                       Upload_stockproducts_Data();
                }
                if (Exist_records_TotalWeight(indianTime.Date) > 0)
                {
                    lbltotalweightuploadstatus.Text = "Status: Uploaded";
                }
                else
                {
                    lbltotalweightuploadstatus.Text = "Status: Not Uploaded. Please Upload Totalweight";
                }
            }
            return i;
        }
        public void Upload_stockproducts_Data()
        {
            DataTable dt = new DataTable();
            dt = Readdata_from_excel_to_datatable("tool_stockproducts.xls");
            if (dt.Rows.Count > 0)
            {
                var rowsToUpdate = dt.AsEnumerable();

                foreach (var row in rowsToUpdate)
                {
                    row.SetField("processdate", indianTime.Date.ToString("yyyy-MM-dd"));
                }
                //dt = rowsToUpdate.CopyToDataTable();
                if (Exist_records(Convert.ToDateTime(dt.Rows[0]["processdate"].ToString())) == 0)
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                    {
                        bulkCopy.DestinationTableName =
                            "dbo.tool_stockproducts";

                        try
                        {
                            // Write from the source to the destination.
                            bulkCopy.WriteToServer(dt);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        
                    }
                    if (Exist_records(indianTime.Date) > 0)
                    {
                        ddlstocktype.Enabled = true;
                    }
                    else
                    {
                        ddlstocktype.Enabled = false;
                    }
                }
            }
        }
        public DataTable Readdata_from_excel_to_datatable(string excelname)
        {
            DataTable dt = new DataTable();
            System.Data.OleDb.OleDbConnection MyConnection;
            //System.Data.OleDb.OleDbCommand myCommand = new System.Data.OleDb.OleDbCommand();
            string filepath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["StocksaleFilePath"].ToString() + excelname);
            string sql = null;
            MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + filepath + "';Extended Properties=Excel 8.0;");
            MyConnection.Open();
            //myCommand.Connection = MyConnection;
            //sql = "select ordernumber,customername,productid,name,weight,units,scannedweight,imagename,weightproductid,weightcalculation from [purchase$] where ordernumber='" + ordernumber + "'";
            sql = "select * from [Sheet1$]";// where purchase_date='" + DateTime.Now.Date.ToString("MM-dd-yyyy") + "'";
            //myCommand.CommandText = sql;
            System.Data.OleDb.OleDbDataAdapter DataAdapter = new System.Data.OleDb.OleDbDataAdapter(sql, MyConnection);
            //DataAdapter.SelectCommand.Connection = MyConnection;
            //DataAdapter.SelectCommand.CommandText = sql;
            DataAdapter.Fill(dt);
            MyConnection.Close();
            return dt;
        }
        public int Delete_records(DateTime execut_date)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(conn);
                SqlCommand command = new SqlCommand("sp_delete_tool_totalweight_by_date", sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@datefield", SqlDbType.DateTime).Value = execut_date;
                sqlConnection.Open();
                int i = command.ExecuteNonQuery();
                sqlConnection.Close();
                return i;
            }
            catch (SqlException ex)
            {
                //Console.WriteLine("SQL Error" + ex.Message.ToString());
                lblerror.Text = errormsg +"This date "+execut_date+ " records not successfully deleted."; //+ex.ToString();
                lblerror.ForeColor = System.Drawing.Color.Red;
                return 0;
            }
        }
        public int Exist_records(DateTime execut_date)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(conn);
                SqlCommand command = new SqlCommand("sp_exist_tool_stockproducts_by_date", sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@datefield", SqlDbType.DateTime).Value = execut_date;
                sqlConnection.Open();
                int i = Convert.ToInt32(command.ExecuteScalar().ToString());
                sqlConnection.Close();
                return i;
            }
            catch (SqlException ex)
            {
                return 0;
            }
        }
        public int Exist_records_TotalWeight(DateTime execut_date)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(conn);
                SqlCommand command = new SqlCommand("sp_exist_tool_totalweight_by_date", sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@datefield", SqlDbType.DateTime).Value = execut_date;
                sqlConnection.Open();
                int i = Convert.ToInt32(command.ExecuteScalar().ToString());
                sqlConnection.Close();
                return i;
            }
            catch (SqlException ex)
            {
                return 0;
            }
        }
        protected void btndownload_OnClick(object sender, EventArgs e)
        {

            try
            {
                DataTable dt=new DataTable();
                SqlConnection sqlConnection = new SqlConnection(conn);
                SqlCommand command = new SqlCommand("sp_weight_difference_totalweight_incoming", sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                sqlConnection.Open();
                SqlDataAdapter sda = new SqlDataAdapter(command);
                sda.Fill(dt);
                sqlConnection.Close();
                if (dt.Rows.Count > 0)
                {
                    exporttocsvnew(dt);
                }
            }
            catch (SqlException ex)
            {
                
            }

        }
        public void exporttocsv(DataTable dt)
        {
            try
            {
                var dataTable = dt;
                StringBuilder builder = new StringBuilder();
                List<string> columnNames = new List<string>();
                List<string> rows = new List<string>();

                foreach (DataColumn column in dataTable.Columns)
                {
                    columnNames.Add(column.ColumnName);
                }

                builder.Append(string.Join(",", columnNames.ToArray())).Append("\n");

                foreach (DataRow row in dataTable.Rows)
                {
                    List<string> currentRow = new List<string>();

                    foreach (DataColumn column in dataTable.Columns)
                    {
                        object item = row[column];

                        //currentRow.Add(item.ToString());
                        currentRow.Add(HttpUtility.HtmlEncode(item.ToString()));
                    }

                    rows.Add(string.Join(",", currentRow.ToArray()));
                }

                builder.Append(string.Join("\n", rows.ToArray()));

                //string filename = "WeightDifference" + DateTime.Now.ToString("dd-MM-yyyy");

                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("Content-Disposition", "attachment;filename=WeightDifference.csv");
                Response.Write(builder.ToString());
                Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        private void ExporttoExcel(DataTable table)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Reports.xls");

            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            //sets font
            HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            HttpContext.Current.Response.Write("<BR><BR><BR>");
            //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
            HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
              "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
              "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
            //am getting my grid's column headers
            //int columnscount = table.Columns.Count;

            //for (int j = 0; j < columnscount; j++)
            //{      //write in new column
            //    HttpContext.Current.Response.Write("<Td>");
            //    //Get column headers  and make it as bold in excel columns
            //    HttpContext.Current.Response.Write("<B>");
            //    HttpContext.Current.Response.Write(GridView_Result.Columns[j].HeaderText.ToString());
            //    HttpContext.Current.Response.Write("</B>");
            //    HttpContext.Current.Response.Write("</Td>");
            //}

            foreach (DataColumn column in table.Columns)
            {
                //write in new column
                HttpContext.Current.Response.Write("<Td>");
                //Get column headers  and make it as bold in excel columns
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(column.ColumnName.ToString());
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
            }

            HttpContext.Current.Response.Write("</TR>");
            foreach (DataRow row in table.Rows)
            {//write in new row
                HttpContext.Current.Response.Write("<TR>");
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    HttpContext.Current.Response.Write("<Td>");
                    HttpContext.Current.Response.Write(row[i].ToString());
                    HttpContext.Current.Response.Write("</Td>");
                }

                HttpContext.Current.Response.Write("</TR>");
            }
            HttpContext.Current.Response.Write("</Table>");
            HttpContext.Current.Response.Write("</font>");
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        public void exporttocsvnew(DataTable dt)
        {
            try
            {
                GridView dg = new GridView();
                dg.DataSource = dt;
                dg.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                string filename = DateTime.Now.ToString("dd-MM-yyyy") + "_WeightDifference";
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", filename + ".xls"));
                Response.ContentType = "application/ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                
                dg.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.Flush();
                Response.SuppressContent = true;
            }
            catch (Exception ex)
            {
            }
        }
        public void ExportDatatabletoExcel(DataTable Tbl)
        {
            object misValue = System.Reflection.Missing.Value;
            string ExcelFilePath = System.Configuration.ConfigurationManager.AppSettings["FilePath"].ToString();
            string filename = "WeightDifference" + DateTime.Now.ToString("dd-MM-yyyy");

            if (Tbl == null || Tbl.Columns.Count == 0)
                throw new Exception("ExportToExcel: Null or empty input table!\n");

            // load excel, and create a new workbook
            Excel.Application excelApp = new Excel.Application();
            excelApp.Workbooks.Add(misValue);

            // single worksheet
            //Excel._Worksheet workSheet = excelApp.ActiveSheet;
            Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;

            // column headings
            for (int i = 0; i < Tbl.Columns.Count; i++)
            {
                workSheet.Cells[1, (i + 1)] = Tbl.Columns[i].ColumnName;
            }

            // rows
            for (int i = 0; i < Tbl.Rows.Count; i++)
            {
                // to do: format datetime values before printing
                for (int j = 0; j < Tbl.Columns.Count; j++)
                {
                    workSheet.Cells[(i + 2), (j + 1)] = Tbl.Rows[i][j];
                }
            }

            // check filepath
            if (ExcelFilePath != null && ExcelFilePath != "")
            {
                try
                {
                    //  workSheet.SaveAs(ExcelFilePath);
                    //workSheet.SaveAs(Server.MapPath(ExcelFilePath + filename + ".xlsx"));
                    workSheet.SaveAs(Server.MapPath(ExcelFilePath + filename + ".xlsx"), Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, false, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue);
                    excelApp.Quit();
                    // MessageBox.Show("Excel file saved!");
                }
                catch (Exception ex)
                {
                    throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n"
                        + ex.Message);
                }
            }
            else    // no filepath is given
            {
                excelApp.Visible = true;
            }


        }
        protected void btndataupload_OnClick(object sender, EventArgs e)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(conn);
                SqlCommand command = new SqlCommand("sp_tool_stockproducts_to_stockproducts_history", sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@stockdate", SqlDbType.DateTime).Value = indianTime.Date.ToString();
                sqlConnection.Open();
                int i = command.ExecuteNonQuery();
                sqlConnection.Close();
                if (i > 0)
                {
                    lblerror.Text = "Updated Successfully";
                }
            }
            catch (SqlException ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
    }
}
