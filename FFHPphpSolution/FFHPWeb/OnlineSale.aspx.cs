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

namespace FFHPWeb
{
    public partial class OnlineSale : System.Web.UI.Page
    {
        Excel.Application oXL = null;
        Excel._Workbook oWB = null;

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

            }
            lblerror.Text = "";
            lblerror.ForeColor = System.Drawing.Color.Black;
        }
        protected void Btnupload_OnClick(object sender, EventArgs e)
        {
            try
            {
                int i = 0;
                if (FUstocksale.HasFile)
                {
                    string file = FUstocksale.PostedFile.FileName.ToString().ToLower();
                    if (file.EndsWith(".csv"))
                    {
                        if (File.Exists(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["OnlinesaleFilePath"].ToString() + "ffhporders-Excel.csv")))
                        {
                            File.Delete(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["OnlinesaleFilePath"].ToString() + "ffhporders-Excel.csv"));
                            FUstocksale.SaveAs(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["OnlinesaleFilePath"].ToString() + "ffhporders-Excel.csv"));
                            i = exefiletodatabase(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["OnlinesaleFilePath"].ToString() + "ffhporders-Excel.csv"), "ffhporders");
                            if (i > 0)
                            {
                                i = databasetohistory("ffhporders", "ffhporders_history");
                                if (i > 0)
                                {
                                    lblerror.Text = "Upload Successfully.";
                                    //ScriptManager.RegisterStartupScript(this, GetType(), "ConfirmShowreport", "ConfirmShowreport('PurchaseReport.aspx');", true);
                                    //Response.Redirect("PurchaseReport.aspx", false);
                                }
                            }


                            //databasetohistory();
                        }
                        else
                        {
                            FUstocksale.SaveAs(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["OnlinesaleFilePath"].ToString() + "ffhporders-Excel.csv"));
                            i = exefiletodatabase(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["OnlinesaleFilePath"].ToString() + "ffhporders-Excel.csv"), "ffhporders");
                            if (i > 0)
                            {
                                i = databasetohistory("ffhporders", "ffhporders_history");
                                if (i > 0)
                                {
                                    lblerror.Text = "Upload Successfully.";
                                    //ScriptManager.RegisterStartupScript(this, GetType(), "ConfirmShowreport", "ConfirmShowreport('PurchaseReport.aspx');", true);

                                }
                            }
                            //databasetohistory();
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
                SqlCommand command = new SqlCommand("sp_loadhistory_ffhporders", sqlConnection);
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
        public int databasetohistory_StockProducts(string fromtablename, string totablename)
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
    }
}
