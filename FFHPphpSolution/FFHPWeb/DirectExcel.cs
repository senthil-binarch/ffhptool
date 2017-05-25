using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;
using System.Text;

namespace FFHPWeb
{
    public class DirectExcel : System.Web.UI.WebControls.GridView

{

//MsoFormats should be all on one line, it’s used to format columns

public string MsoFormats = "<style>.TextFormat { mso-number-format:\\@; } .DateFormat { mso-number-format:'mm\\/dd\\/yy' } .CurrencyFormat { mso-number-format:\"\\0022$\\0022\\#\\,\\#\\#0\\.00\" } .NumberFormat {mso-number-format:0;} .FixedNumberFormat{mso-number-format:Fixed;} .PercentFormat{mso-number-format:0%;} .PercentWithDecimalsFormat{mso-number-format:Percent;}</style>";

 

                // default filename used when saving

                public string ReportName;

                // columns to remove from exported view

                public List<string> ColumnsToRemove = new List<string>();

 

                public DirectExcel(string strReportName)

                {

                                ReportName = strReportName;

                                // default empty data text to display when grid has zero records

                                this.EmptyDataText = "No data record(s) found for criteria.";

                }

 

                public DirectExcel(string strReportName, string strEmptyDataText)

                {

                                ReportName = strReportName;

                                this.EmptyDataText = strEmptyDataText;

                }

 

                public void Export(System.Data.SqlClient.SqlCommand oCmd)

                {

                                SqlConnection objConn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DBConnectionString"].ToString());

                                SqlDataAdapter objAdapter = new System.Data.SqlClient.SqlDataAdapter();

                                DataTable objDt = new DataTable();

 

                                oCmd.Connection = objConn;

                                oCmd.CommandTimeout = 300;

 

                                objAdapter.SelectCommand = oCmd;

                                objAdapter.Fill(objDt);

 

                                //remove any columns specified.

                                foreach (string colName in ColumnsToRemove)

                                {

                                                objDt.Columns.Remove(colName);

                                }

 

                                this.DataSource = objDt;

                                this.DataBind();

 

                                //clear the reponse and change content type

                                HttpContext.Current.Response.Clear();

                                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + ReportName + ".xls\"");

                                HttpContext.Current.Response.Charset = "";

                                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";

                                //add the mso classes that can be used to format columns

                                HttpContext.Current.Response.Write(this. MsoFormats);

 

                                System.IO.StringWriter tw = new System.IO.StringWriter();

                                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);

                                this.RenderControl(hw);

                                HttpContext.Current.Response.Write(tw.ToString());

                                HttpContext.Current.Response.End();

                }

 public System.IO.MemoryStream ExportToStream(DataTable objDt)

{

    //remove any columns specified.

    foreach (string colName in ColumnsToRemove)

    {

        objDt.Columns.Remove(colName);

    }

 

    this.DataSource = objDt;

    this.DataBind();

 

    System.IO.StringWriter sw = new System.IO.StringWriter();

    System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);

    this.RenderControl(hw);

    string content = sw.ToString();

    byte[] byteData = Encoding.Default.GetBytes(content);

 

    System.IO.MemoryStream mem = new System.IO.MemoryStream();

    mem.Write(byteData, 0, byteData.Length);

    mem.Flush();

    mem.Position = 0; //reset position to the begining of the stream

    return mem;

} 

                  // Used to Format Direct Excel Export

                protected override void OnRowCreated(GridViewRowEventArgs e)

                {

                                base.OnRowCreated(e);

                                if (e.Row.RowType == DataControlRowType.DataRow)

                                {

                                                e.Row.VerticalAlign = VerticalAlign.Top;

                                }

                }

    }

}