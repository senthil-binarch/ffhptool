using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using MySql.Data.MySqlClient;
using System.Text;
using System.IO;
using System.Net;
using Renci.SshNet;

namespace FFHPWeb
{
    public partial class PurchaseReport : System.Web.UI.Page
    {
        

        string sqlconn = System.Configuration.ConfigurationManager.AppSettings["SqlConnectionString"].ToString();
        string conn = System.Configuration.ConfigurationManager.AppSettings["Connection"].ToString();

        string queryString = "";
        string s = "";
        bool t = false;
        string errormsg = "Try again";
        static string group = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bind();
            }
            lblerror.Text = "";
            lblerror.ForeColor = Color.Black;
        }
        public void bind()
        {
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dtproductcutproduct = new DataTable();
            DataTable dtnonpurchase = new DataTable();

            try
            {
                Purchase objpur = new Purchase();
                dt = objpur.getPurchase();
                dt2 = getapiweightprice();
                dtproductcutproduct = getproductcutproduct();

                dtnonpurchase = getallproducts(dt, dt2, dtproductcutproduct);

                dt.Merge(dtnonpurchase);
                //dt.Merge(dt2);
                //foreach (DataRow row in dt.Rows)
                //{
                //    foreach (DataRow row2 in dt2.Rows)
                //    {
                //        if (row["pid"].ToString() == row2["product_id"].ToString())
                //        {
                //            row["pname"] = row2["name"].ToString();
                //            break;
                //        }
                //    }
                //}

                //dtpurchase.Columns.Add("purchase_date", typeof(DateTime), System.DateTime.Now.ToShortDateString());


                gvpurchase.DataSource = resort(Process(dt), "group, %, pid", "ASC");
                gvpurchase.DataBind();
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
            
        }
        public DataTable getallproducts(DataTable dt, DataTable dt2, DataTable dtproductcutproduct)
        {
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("pid", typeof(int));
            dtResult.Columns.Add("pname", typeof(string));
            dtResult.Columns.Add("purchase_weight", typeof(decimal));
            dtResult.Columns.Add("purchase_price", typeof(decimal));
            dtResult.Columns.Add("unit", typeof(string));
            dtResult.Columns.Add("price_per_kgpc", typeof(decimal));
            dtResult.Columns.Add("purchase_date", typeof(DateTime));
            dtResult.Columns.Add("%", typeof(decimal));
            
                foreach (DataRow row2 in dt2.Rows)
                {


                    DataTable products1 = null;
                    products1 = dt.AsEnumerable().Where(r => Convert.ToString(r["pid"]) == row2["product_id"].ToString()).AsDataView().ToTable();

                    //var qqq = from r in dt.AsEnumerable()
                    //          where r["pid"].ToString() == row2["product_id"].ToString()
                    //          select r;

                    if (products1.Rows.Count==0)
                    {
                        DataTable products2 = null;
                        products2 = dtproductcutproduct.AsEnumerable().Where(r => Convert.ToString(r["productid"]) == row2["product_id"].ToString()).AsDataView().ToTable();

                        if (products2.Rows.Count > 0)
                        {
                            DataTable products3 = null;
                            products3 = dt.AsEnumerable().Where(r => Convert.ToString(r["pid"]) == products2.Rows[0]["mainproductid"].ToString()).AsDataView().ToTable();

                            if (products3.Rows.Count > 0)
                            {
                                DataRow row = dtResult.NewRow();
                                row["pid"] = Convert.ToInt32(row2["product_id"].ToString());
                                row["pname"] = row2["name"].ToString();
                                row["purchase_weight"] = products3.Rows[0]["purchase_weight"].ToString();//1;
                                row["purchase_price"] = products3.Rows[0]["purchase_price"].ToString();
                                row["unit"] = row2["unit"].ToString();
                                row["price_per_kgpc"] = Convert.ToDecimal(row2["priceperkgpc"].ToString());
                                row["purchase_date"] = Convert.ToDateTime(products3.Rows[0]["purchase_date"].ToString());//Convert.ToDateTime(row2["date"].ToString());
                                row["%"] = ((Convert.ToDecimal(row2["priceperkgpc"].ToString()) - Convert.ToDecimal(products3.Rows[0]["purchase_price"].ToString())) / Convert.ToDecimal(products3.Rows[0]["purchase_price"].ToString())) * 100;//0;

                                dtResult.Rows.Add(row);
                            }
                        }
                    } 
              
                }
            


                          

            //DataTable dtResult = new DataTable();
            //dtResult.Columns.Add("pid", typeof(int));
            //dtResult.Columns.Add("pname", typeof(string));
            //dtResult.Columns.Add("purchase_weight", typeof(decimal));
            //dtResult.Columns.Add("purchase_price", typeof(decimal));
            //dtResult.Columns.Add("unit", typeof(string));
            //dtResult.Columns.Add("price_per_kgpc", typeof(decimal));
            //dtResult.Columns.Add("purchase_date", typeof(DateTime));
            //dtResult.Columns.Add("%", typeof(decimal));

            //foreach (DataRow row in dt.Rows)
            //{
            //    foreach (DataRow row2 in dt2.Rows)
            //    {
            //        if (row["pid"].ToString() == row2["product_id"].ToString())
            //        {
            //            row["pname"] = row2["name"].ToString();
            //            break;
            //        }
            //    }
            //}

            //var result = from dataRows1 in dt.AsEnumerable()
            //             join dataRows2 in dt2.AsEnumerable()
            //             on dataRows1.Field<int>("pid") equals dataRows2.Field<int>("productid")

            //             select dtResult.LoadDataRow(new object[]
            // {
            //    dataRows2.Field<int>("productid"),
            //    dataRows2.Field<string>("name"),
            //    dataRows1.Field<decimal>("purchase_weight"),
            //    dataRows1.Field<decimal>("purchase_price"),
            //    dataRows2.Field<string>("unit"),
            //    dataRows2.Field<decimal>("price_per_kgpc"),
            //    dataRows1.Field<DateTime>("purchase_date"),
            //    dataRows1.Field<decimal>("%"),
            //  }, false);
                return dtResult;
        }
        public DataTable Process(DataTable dt)
        {
            DataTable dt3 = new DataTable();
            DataTable resultdt = new DataTable();

            dt3 = getffhpproducts();

            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("pid", typeof(int));
            dtResult.Columns.Add("pname", typeof(string));
            dtResult.Columns.Add("purchase_weight", typeof(decimal));
            dtResult.Columns.Add("purchase_price", typeof(decimal));
            dtResult.Columns.Add("unit", typeof(string));
            dtResult.Columns.Add("price_per_kgpc", typeof(decimal));
            dtResult.Columns.Add("purchase_date", typeof(DateTime));
            dtResult.Columns.Add("%", typeof(decimal));
            dtResult.Columns.Add("group", typeof(string));

            var result = from dataRows1 in dt.AsEnumerable()
                         join dataRows2 in dt3.AsEnumerable()
                         on dataRows1.Field<int>("pid") equals dataRows2.Field<int>("productid")

                         select dtResult.LoadDataRow(new object[]
             {
                dataRows1.Field<int>("pid"),
                dataRows1.Field<string>("pname"),
                dataRows1.Field<decimal>("purchase_weight"),
                dataRows1.Field<decimal>("purchase_price"),
                dataRows1.Field<string>("unit"),
                dataRows1.Field<decimal>("price_per_kgpc"),
                dataRows1.Field<DateTime>("purchase_date"),
                dataRows1.Field<decimal>("%"),
                dataRows2.Field<string>("group"),
              }, false);

            resultdt = result.CopyToDataTable();
            resultdt = resort(resultdt, "group, %, pid", "ASC");

            return resultdt;
        }
        public static DataTable resort(DataTable dt, string colName, string direction)
        {
            dt.DefaultView.Sort = colName + " " + direction;
            dt = dt.DefaultView.ToTable();
            return dt;
        }
        public DataTable getapiweightprice()
        {
            DataTable dt = new DataTable();
            try
            {
                APIMethods objmobileorders = new APIMethods();
                dt = objmobileorders.getProduct_Weight_Price_for_Before_After_Sale();


                //dtpurchase.Columns.Add("purchase_date", typeof(DateTime), System.DateTime.Now.ToShortDateString());
                //gvpurchase.DataSource = dt;
                //gvpurchase.DataBind();
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
            return dt;
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
        protected void gvpurchase_OnRowDataBound(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool t = false;
                if(group=="" || group!=e.Row.Cells[11].Text.Trim())
                {
                group=e.Row.Cells[11].Text.Trim();
                t = true;
                }
                string Stylestring="";
                // Display the company name in italics.
                //e.Row.Cells[1].Text = "<i>" + e.Row.Cells[1].Text + "</i>";
                if (e.Row.Cells[8].Text.Trim() != "&nbsp;")
                {
                    if (Convert.ToDecimal(e.Row.Cells[8].Text) < 70)
                    {
                        //e.Row.Cells[3].ForeColor = Color.DarkRed;
                        //e.Row.Cells[3].BorderColor = Color.Black;

                        for (int i = 1; i < gvpurchase.Columns.Count-1; i++)
                        {
                            e.Row.Cells[i].Attributes.Add("style", "color:darkred");
                            //e.Row.Cells[i].ForeColor = Color.DarkRed;
                            e.Row.Cells[i].BorderColor = Color.Black;
                            
                        }
                        Stylestring = "color:darkred;";
                    }
                    else if (Convert.ToDecimal(e.Row.Cells[8].Text) > 70)
                    {
                        for (int i = 1; i < gvpurchase.Columns.Count-1; i++)
                        {
                            e.Row.Cells[i].Attributes.Add("style", "color:darkblue");
                            //e.Row.Cells[i].ForeColor = Color.DarkBlue;
                            e.Row.Cells[i].BorderColor = Color.Black;
                        }
                        Stylestring = "color:darkblue;";
                    }
                }
                
                if (t)
                {
                    //foreach (TableCell cell in e.Row.Cells)
                    //    cell.Attributes.Add("Style", "border-top: black 10px double");
                    Stylestring = Stylestring + "border-top: gray 5px solid;";
                    foreach (TableCell cell in e.Row.Cells)//border-style: inset;border-width: 1px;
                    {
                        if (e.Row.Cells.GetCellIndex(cell) != 0 && e.Row.Cells.GetCellIndex(cell) != 11)
                        {
                            cell.Attributes.Add("Style", Stylestring);
                        }
                        else
                        {
                            cell.Attributes.Add("Style", "border-top: gray 5px solid;");
                        }
                        
                    }
                    
                }
            }
        }
        protected void btnsubmit_OnClick(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            try
            {
                Purchase objpur = new Purchase();
                if (TbxFromDate.Text != "" && TbxToDate.Text != "")
                {
                    string format = "MM/dd/yyyy";

	                //DateTime dateTime = DateTime.ParseExact(TbxFromDate.Text.ToString(), format,CultureInfo.InvariantCulture);

                    DateTime dtf = DateTime.ParseExact(TbxFromDate.Text.ToString(), format, CultureInfo.InvariantCulture);//Convert.ToDateTime(TbxFromDate.Text);
                    DateTime dtt = DateTime.ParseExact(TbxToDate.Text.ToString(), format, CultureInfo.InvariantCulture);//Convert.ToDateTime(TbxToDate.Text);
                    if (dtf == dtt)
                    {
                        if (Tbxpid.Text != "")
                        {
                            dt = objpur.getPurchase(dtf.ToString("yyyy-MM-dd"), dtt.ToString("yyyy-MM-dd"), Convert.ToInt32(Tbxpid.Text));
                        }
                        else
                        {
                            dt = objpur.getPurchase(dtf.ToString("yyyy-MM-dd"), dtt.ToString("yyyy-MM-dd"));
                        }
                    }
                    else
                    {
                        if (Tbxpid.Text != "")
                        {
                            dt = objpur.getPurchase(dtf.ToString("yyyy-MM-dd"), dtt.ToString("yyyy-MM-dd"), Convert.ToInt32(Tbxpid.Text));
                        }
                        else
                        {
                            lblerror.ForeColor = Color.Red;
                            lblerror.Text = "Please enter the PID";
                        }
                    }
                }
                else
                {
                    dt = objpur.getPurchase();
                }
                if (dt.Rows.Count > 0)
                {
                    DataTable dt2 = getapiweightprice();
                    //dt.Merge(dt2);
                    foreach (DataRow row in dt.Rows)
                    {
                        foreach (DataRow row2 in dt2.Rows)
                        {
                            if (row["pid"].ToString() == row2["product_id"].ToString())
                            {
                                row["pname"] = row2["name"].ToString();
                                break;
                            }
                        }
                    }

                    //dtpurchase.Columns.Add("purchase_date", typeof(DateTime), System.DateTime.Now.ToShortDateString());


                    gvpurchase.DataSource = resort(Process(dt), "purchase_date", "ASC");
                    gvpurchase.DataBind();
                }
                else
                {
                    gvpurchase.DataSource = null;
                    gvpurchase.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
        }
        protected void btngeneratebulkprice_OnClick(object sender, EventArgs e)
        {
            try
            {
                DataTable newTable = new DataTable("price_list");
                newTable.Columns.Add("sku", typeof(string));
                newTable.Columns.Add("price", typeof(decimal));

                //queryString = @"";
                DataTable allproducts = new DataTable();
                //SqlConnection con1 = new SqlConnection(conn);
                //con1.Open();
                //SqlCommand cmd1 = new SqlCommand("select pid as entity_id,sku,weight,price from product_sku", con1);
                //cmd1.CommandType = CommandType.Text;
                //SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
                //sda1.Fill(allproducts);
                //con1.Close();

                //queryString = @"SELECT entity_id,Name,sku,weight,price FROM `catalog_product_flat_1` where visibility=4";

                //if (queryString != "")
                //{
                //    MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                //    DataSet allproductlist = new DataSet();
                //    adapteradminmail.Fill(allproductlist, "allproducts");
                //    allproducts = allproductlist.Tables[0];
                //}

                allproducts = Getallproducts();
                string notenableproducts = "";
                foreach (GridViewRow row in gvpurchase.Rows)
                {

                    TextBox tbxnewsellingprice = (TextBox)row.FindControl("tbxnewsellingprice");
                    TextBox tbxnewpercentage = (TextBox)row.FindControl("tbxnewpercentage");



                    if (tbxnewsellingprice.Text != "" && tbxnewpercentage.Text != "")
                    {

                        var rows =
        (from row1 in allproducts.AsEnumerable()
         where row1.Field<UInt32>("entity_id") == Convert.ToInt32(row.Cells[2].Text.ToString())
         select new
         {
             sku = row1.Field<string>("sku"),
             weight = row1.Field<decimal>("weight")
         }).ToList();
                        if (rows.Any())
                        {
                            string s = rows[0].weight.ToString();
                            decimal tbxnewsellingprice1 = Convert.ToDecimal(tbxnewsellingprice.Text.ToString());
                            decimal tbxnewpercentage1 = Convert.ToDecimal(tbxnewpercentage.Text.ToString());
                            decimal weight = Convert.ToDecimal(row.Cells[4].Text.ToString());
                            decimal purchaseprice = Convert.ToDecimal(row.Cells[6].Text.ToString());

                            decimal price_per_actual_weight = 0;
                            if (row.Cells[5].Text.ToString().ToLower() == "kg")
                            {
                                //price_per_actual_weight=((rowInfo.price_per_kgpc / 1000) * rowInfo.weight);
                                //price_per_actual_weight = ((((tbxnewsellingprice1 / 100) * tbxnewpercentage1 + tbxnewsellingprice1) / 1000) * weight * 1000);
                                price_per_actual_weight = ((tbxnewsellingprice1 / 1000) * (Convert.ToDecimal(rows[0].weight.ToString()) * 1000));
                            }
                            else
                            {
                                //price_per_actual_weight=(Convert.ToInt32(rowInfo.price_per_kgpc) * Convert.ToInt32(rowInfo.weight));
                                //price_per_actual_weight = (tbxnewsellingprice1 / 100) * tbxnewpercentage1 + purchaseprice;
                                price_per_actual_weight = tbxnewsellingprice1;
                            }
                            //newTable.Rows.Add(rowInfo.entity_id, rowInfo.name, rowInfo.sku,rowInfo.weight, rowInfo.price, rowInfo.purchase_weight,rowInfo.purchase_price, rowInfo.unit, rowInfo.price_per_kgpc,price_per_actual_weight);
                            newTable.Rows.Add(rows[0].sku.ToString(), price_per_actual_weight);
                        }
                        else
                        {
                            if (notenableproducts == "")
                            {
                                notenableproducts = row.Cells[2].Text.ToString();
                            }
                            else
                            {
                                notenableproducts = notenableproducts + "," + row.Cells[2].Text.ToString();
                            }
                        }
                    }
                }
                if (newTable.Rows.Count > 0)
                {
                    //DataTable dt = new DataTable();
                    //dt = newTable;
                    //string attachment = "attachment; filename=prices.xls";
                    //Response.ClearContent();
                    //Response.AddHeader("content-disposition", attachment);
                    //Response.ContentType = "application/vnd.ms-excel";
                    //string tab = "";
                    //foreach (DataColumn dc in dt.Columns)
                    //{
                    //    Response.Write(tab + dc.ColumnName);
                    //    tab = "\t";
                    //}
                    //Response.Write("\n");
                    //int i;
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    tab = "";
                    //    for (i = 0; i < dt.Columns.Count; i++)
                    //    {
                    //        Response.Write(tab + dr[i].ToString());
                    //        tab = "\t";
                    //    }
                    //    Response.Write("\n");
                    //}
                    //Response.End();
                    //lblerror.Text = notenableproducts.ToString()+" these pid is not enable currently";
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('"+notenableproducts+"')", true);
                    exporttocsv(newTable);
                }
                
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg + ex.ToString();
            }
        }
        public DataTable Getallproducts()
        {
            DataTable allproducts = new DataTable();

            queryString = @"SELECT entity_id,Name,sku,weight,price FROM `catalog_product_flat_1` where visibility=4";

            if (queryString != "")
            {
                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                DataSet allproductlist = new DataSet();
                adapteradminmail.Fill(allproductlist, "allproducts");
                allproducts = allproductlist.Tables[0];
            }
            return allproducts;
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

                        currentRow.Add(item.ToString());
                    }

                    rows.Add(string.Join(",", currentRow.ToArray()));
                }

                builder.Append(string.Join("\n", rows.ToArray()));

                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("Content-Disposition", "attachment;filename=prices.csv");
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
        protected void btnclear_OnClick(object sender, EventArgs e)
        {
            TbxFromDate.Text = "";
            TbxToDate.Text = "";
            Tbxpid.Text = "";
            bind();
        }
        protected void Btnupload_OnClick(object sender, EventArgs e)
        {
            try
            {
                test();

                //int i = 0;
                //if (FUbulkprices.HasFile)
                //{
                //    string file = FUbulkprices.PostedFile.FileName.ToString().ToLower();
                //    if (file.EndsWith(".csv"))
                //    {
                //        if (File.Exists(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["PurchaseFilePath"].ToString() + "prices.csv")))
                //        {
                //            File.Delete(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["PurchaseFilePath"].ToString() + "prices.csv"));
                //            FUbulkprices.SaveAs(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["PurchaseFilePath"].ToString() + "prices.csv"));

                //            UploadFileToFTP(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["PurchaseFilePath"].ToString() + "prices.csv"));
                //            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Upload Successfully')", true);
                //            lblerror.Text = "Upload Successfully";
                //        }
                //        else
                //        {
                //            FUbulkprices.SaveAs(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["PurchaseFilePath"].ToString() + "prices.csv"));
                //            UploadFileToFTP(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["PurchaseFilePath"].ToString() + "prices.csv"));
                //            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Upload Successfully')", true);
                //            lblerror.Text = "Upload Successfully";
                //        }
                //    }
                //    else
                //    {
                //        lblerror.Text = "upload only .csv file";
                //    }
                //}
                //else
                //{
                //    lblerror.Text = "Plese select a file";
                //}
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg + ex.ToString();
            }
        }
        private static void UploadFileToFTP(string source)
        {
            try
            {
                string ftpurl = System.Configuration.ConfigurationManager.AppSettings["priceuploadurl"].ToString();// "ftp://50.63.208.191/Test/prices.csv";
                string ftpusername = System.Configuration.ConfigurationManager.AppSettings["priceuploadusername"].ToString();// "ffhp1";
                string ftppassword = System.Configuration.ConfigurationManager.AppSettings["priceuploadpassword"].ToString();// "Stag1ngffHP!n";
                string filename = Path.GetFileName(source);
                string ftpfullpath = ftpurl;
                
                FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);
                ftp.Credentials = new NetworkCredential(ftpusername, ftppassword);

                ftp.KeepAlive = true;
                ftp.UseBinary = true;
                ftp.Method = WebRequestMethods.Ftp.UploadFile;
                //ftp.UsePassive = false;

                FileStream fs = File.OpenRead(source);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();

                Stream ftpstream = ftp.GetRequestStream();
                ftpstream.Write(buffer, 0, buffer.Length);
                ftpstream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void test()
        {
            try
            {
                FileInfo f = new FileInfo("C:\\mdu\\abcd.xml");
                string uploadfile = f.FullName;
                Console.WriteLine(f.Name);
                Console.WriteLine("uploadfile" + uploadfile);

                //Passing the sftp host without the "sftp://"
                //var client = new SftpClient("ftp.example.com", port, username, password);
                //string filepath = Server.MapPath("privatekey/opensshprivatekey_pricesupload.ppk");
                string filepath = Server.MapPath("privatekey/opensshftpffhp.ppk");
                var keyFile = new PrivateKeyFile(filepath);
                var keyFiles = new[] { keyFile };

                //var client = new SftpClient("192.124.249.18:22", "root", keyFiles);
                //var client = new SftpClient("205.147.103.250:22", "root", keyFiles);
                //var client = new SftpClient("ftp.farmfreshhandpicked.com", 22,"farmfreshhandpicked", keyFiles);
                var client = new SftpClient("ftp.farmfreshhandpicked.com", 22, "farmfreshhandpicked", "farmfresh");
                
                client.Connect();
                if (client.IsConnected)
                {
                    //var fileStream = new FileStream(uploadfile, FileMode.Open);
                    //if (fileStream != null)
                    //{
                    //    //If you have a folder located at sftp://ftp.example.com/share
                    //    //then you can add this like:
                    //    client.UploadFile(fileStream, "/share/" + f.Name, null);
                    //    client.Disconnect();
                    //    client.Dispose();
                    //}
                }
            }
            catch (Exception ex)
            {

            }
        }
        public DataTable getproductcutproduct()
        {
            DataTable resultdt = new DataTable();
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(sqlconn);
                //SqlDataAdapter sda=new SqlDataAdapter("select * from dbo.productcutproduct where mainproductid!=productid",con);
                SqlDataAdapter sda = new SqlDataAdapter("select Parent_product_Id as mainproductid,Product_Id as productid,Name as productname from dbo.Products_ExtraWeights where product_id!=parent_product_id", con);
                sda.Fill(dt);

                
             //   DataTable allproducts = new DataTable();
             //   allproducts = Getallproducts();

             //   DataTable dtResult = new DataTable();
             //   dtResult.Columns.Add("mainproductid", typeof(int));
             //   dtResult.Columns.Add("productid", typeof(int));
             //   dtResult.Columns.Add("productname", typeof(string));

             //   var result = from dataRows1 in dt.AsEnumerable()
             //                join dataRows2 in allproducts.AsEnumerable()
             //                on Convert.ToInt32(dataRows1.Field<Int32>("productid")) equals Convert.ToInt32(dataRows2.Field<UInt32>("entity_id"))

             //                select dtResult.LoadDataRow(new object[]
             //{
             //   dataRows1.Field<int>("mainproductid"),
             //   dataRows1.Field<int>("productid"),
             //   dataRows1.Field<string>("productname"),
             // }, false);

             //   resultdt = result.CopyToDataTable();
                
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
            return dt;
        }
        
    }
}
