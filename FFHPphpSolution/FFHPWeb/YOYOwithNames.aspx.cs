﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.html;
using System.Net.Mail;
using System.Text.RegularExpressions;
//using Renci.SshNet;
using System.Globalization;

namespace FFHPWeb
{
    public partial class YOYOwithNames : System.Web.UI.Page
    {
        //string conn = "server=68.178.143.11;userid=ffhpmagento;password=D6QpT!KDd0dKHI;database=ffhpmagento;Convert Zero Datetime=True";  //Live
        string conn = System.Configuration.ConfigurationManager.AppSettings["Connection"].ToString();
        string queryString = "";
        MySqlDataAdapter DA;
        string s = "";
        bool t = false;
        string errormsg = "Try again";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblerror.Text = "";
                if (Session["username"] != null && Session["username"].ToString() != "")
                {
                    if (!IsPostBack)
                    {
                        if (Session["username"] != null && Session["username"].ToString() != "")
                        {
                            //txtidlist.Text = Session["orderid"].ToString();
                            ReadLastorders();
                        }
                        else
                        {
                            Response.Redirect("Login.aspx", false);
                        }
                        getOrderDetails();
                    }
                }
                else
                {
                    Response.Redirect("Login.aspx", false);
                }

            }
            catch (Exception ex)
            {
                Response.Redirect("Login.aspx", false);
            }

        }
        public void ServiceOrderupdate()
        {
            try
            {
                string curordnum = "0";
                if (txtidlist.Text != "")
                {
                    curordnum = txtidlist.Text.ToString();
                }
                string path = Server.MapPath("Images/OrderNumbers.txt");
                string pss1 = string.Format("{0:hh:mm}", DateTime.Now);//System.DateTime.Now.ToShortTimeString();
                FileStream pfs1 = new FileStream(path,
                                        FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter p_streamWriter1 = new StreamWriter(pfs1);
                p_streamWriter1.BaseStream.Seek(0, SeekOrigin.End);
                //p_streamWriter1.WriteLine(pss1 + "FFHP Orders " + " on " + DateTime.Today.Date.ToString("dd:MM:yyyy") + "_" + DateTime.Now.TimeOfDay.ToString()+DateTime.Now.ToString("tt")+" #" + txtidlist.Text.ToString()); p_streamWriter1.Flush();
                p_streamWriter1.WriteLine(DateTime.Today.Date.ToString("dd:MM:yyyy") + "_" + DateTime.Now.TimeOfDay.ToString() + DateTime.Now.ToString("tt") + " #" + curordnum.ToString()); p_streamWriter1.Flush();
                p_streamWriter1.Close();
                //lblerror.Text = "Submitted Successfully";
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }

        }
        public void ReadLastorders()
        {
            try
            {
                string path = Server.MapPath("Images/OrderNumbers.txt");
                string ord = File.ReadAllLines(path).Last();
                string[] Orders = ord.Split('#');
                if (Orders.Count() > 0)
                {
                    txtidlist.Text = Orders.Last();
                }
                if (txtidlist.Text == "0")
                {
                    txtidlist.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
        }
        protected void btncalculate_OnClick(object sender, EventArgs e)
        {
            try
            {
                ServiceOrderupdate();
                if (txtidlist.Text != "")
                {
                    Session["orderid"] = txtidlist.Text.ToString();

                    getOrderDetails();
                }
                else
                {
                    GVOrderDetails.DataSource = null;
                    GVOrderDetails.DataBind();
                    GVMail.DataSource = null;
                    GVMail.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg;//ex.ToString();
            }
        }
        public void getOrderNumbers()
        {
            DataTable testdt = new DataTable("testOrderlist");
            testdt.Clear();
            testdt.Columns.Add("entity_id");
            testdt.Columns.Add("customername");
            testdt.Columns.Add("Address");
            testdt.Columns.Add("Name");
            testdt.Columns.Add("Zipcode");
            try
            {
                DataTable dt = new DataTable("OrderList");
                dt.Clear();
                dt.Columns.Add("entity_id");
                dt.Columns.Add("customername");
                dt.Columns.Add("Address");
                dt.Columns.Add("Name");
                dt.Columns.Add("Zipcode");

                DateTime dtf = DateTime.ParseExact(TbxFromDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                DateTime dtt = DateTime.ParseExact(TbxToDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);


                //queryString = "SELECT entity_id, customer_firstname, customer_lastname FROM `sales_flat_order` where created_at between '" + Convert.ToDateTime(TbxFromDate.Text.ToString()).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(TbxToDate.Text.ToString()).ToString("yyyy-MM-dd") + "'";
                //a.customer_id = c.customer_id AND 
                queryString = @"SELECT  DISTINCT z.increment_id as entity_id, a.customer_id, CONCAT( IFNULL(c.firstname,''), ' ', IFNULL(c.lastname,'') ) AS customername, CONCAT( IFNULL( c.street, '' ) , ', ', IFNULL( c.city, '' ) , ', ', IFNULL( c.region, '' ) , ', ', IFNULL( c.postcode, '' ) , ', India, T:', IFNULL( c.telephone, '' ) ) AS Address, b.Name,IFNULL( c.postcode, '' )as Zipcode
FROM `sales_flat_order_grid` as z inner join `sales_flat_order` AS a 
ON z.entity_id=a.entity_id
INNER JOIN `sales_flat_order_item` AS b ON a.entity_id = b.order_id
INNER JOIN `sales_flat_order_address` AS c ON c.parent_id = b.order_id
WHERE b.parent_item_id IS NULL
AND product_type
IN (
'simple', 'bundle', 'grouped'
) AND c.address_type = 'shipping' and z.status!='canceled' and a.created_at between '" + dtf.ToString("yyyy-MM-dd") + "' and '" + dtt.AddDays(1).ToString("yyyy-MM-dd") + "'";

                //                queryString = @"SELECT  DISTINCT a.entity_id, a.customer_id, CONCAT( IFNULL(c.firstname,''), ' ', IFNULL(c.lastname,'') ) AS customername, CONCAT( IFNULL( c.street, '' ) , ', ', IFNULL( c.city, '' ) , ', ', IFNULL( c.region, '' ) , ', ', IFNULL( c.postcode, '' ) , ', India, T:', IFNULL( c.telephone, '' ) ) AS Address, b.Name,IFNULL( c.postcode, '' )as Zipcode
                //FROM `sales_flat_order` AS a
                //LEFT OUTER JOIN `sales_flat_order_item` AS b ON a.entity_id = b.order_id
                //LEFT OUTER JOIN `sales_flat_order_address` AS c ON c.parent_id = b.order_id
                //WHERE b.parent_item_id IS NULL
                //AND product_type
                //IN (
                //'simple', 'bundle', 'grouped'
                //) AND c.address_type = 'shipping' AND and a.created_at between '" + Convert.ToDateTime(TbxFromDate.Text.ToString()).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(TbxToDate.Text.ToString()).ToString("yyyy-MM-dd") + "'";

                //Response.Write(queryString);
                if (queryString != "")
                {
                    MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                    DataSet orderlist = new DataSet();
                    adapteradminmail.Fill(orderlist, "ordernumber");

                    DataTable orderlist1 = new DataTable();
                    orderlist1 = orderlist.Tables[0];
                    string Entityid = "";
                    string TestValue1 = "";
                    string customername = "";
                    string Address = "";
                    string TestPack = "";
                    string Zipcode = "";

                    DataTable dtt1 = resort(orderlist1, "entity_id", "ASC");

                    if (dtt1.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtt1.Rows.Count; i++)
                        {
                            Entityid = dtt1.Rows[i]["entity_id"].ToString();
                            if (i == 0)
                            {
                                if (TestPack == "")
                                {
                                    TestPack = dtt1.Rows[i]["Name"].ToString();
                                }
                                else
                                {
                                    TestPack = TestPack + dtt1.Rows[i]["Name"].ToString();
                                }
                            }
                            else if (Entityid.Equals(TestValue1.ToString()))
                            {
                                if (TestPack == "")
                                {

                                    TestPack = dtt1.Rows[i]["Name"].ToString();

                                }
                                else
                                {
                                    TestPack = TestPack + ", " + dtt1.Rows[i]["Name"].ToString();

                                }
                            }
                            else if (i > 0)
                            {

                                DataRow _TestPackList = testdt.NewRow();
                                _TestPackList["entity_id"] = TestValue1.ToString();
                                _TestPackList["customername"] = customername.ToString();//dtt1.Rows[i]["customername"].ToString();
                                _TestPackList["Address"] = Address.ToString();//dtt1.Rows[i]["Address"].ToString();
                                _TestPackList["Name"] = TestPack.ToString();
                                _TestPackList["Zipcode"] = Zipcode.ToString();
                                testdt.Rows.Add(_TestPackList);

                                TestPack = "";
                                TestPack = dtt1.Rows[i]["Name"].ToString();
                            }
                            TestValue1 = Entityid.ToString();

                            customername = dtt1.Rows[i]["customername"].ToString();
                            Address = dtt1.Rows[i]["Address"].ToString();
                            Zipcode = dtt1.Rows[i]["Zipcode"].ToString();
                        }
                        DataRow _TestPackListfinalrecord = testdt.NewRow();
                        _TestPackListfinalrecord["entity_id"] = TestValue1.ToString();
                        _TestPackListfinalrecord["customername"] = customername.ToString();
                        _TestPackListfinalrecord["Address"] = Address.ToString();
                        _TestPackListfinalrecord["Name"] = TestPack.ToString();
                        _TestPackListfinalrecord["Zipcode"] = Zipcode.ToString();
                        testdt.Rows.Add(_TestPackListfinalrecord);
                    }

                    GVOrdernumbers.DataSource = testdt;
                    GVOrdernumbers.DataBind();
                    if (txtidlist.Text != "")
                    {
                        string[] txtid = txtidlist.Text.ToString().Split(',');
                        if (txtid.Count() > 0)
                        {
                            foreach (GridViewRow row in GVOrdernumbers.Rows)
                            {
                                string s1 = row.Cells[1].Text.ToString();
                                string s2 = row.Cells[2].Text.ToString();
                                string s0 = row.Cells[0].Text.ToString();

                                //(CheckBox)GVOrderDetails.Rows[row].FindControl("CBorder)
                                CheckBox CB = (CheckBox)row.FindControl("CBorder");
                                if (row.Cells[2].Text.ToString() != "")
                                {
                                    for (int i = 0; i < txtid.Count(); i++)
                                    {
                                        if (row.Cells[2].Text.ToString() == txtid[i].ToString())
                                        {
                                            CB.Checked = true;
                                            break;
                                        }
                                        else
                                        {
                                            CB.Checked = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //string Orderid = "";
                    //for (int i = 0; i < orderlist.Tables[0].Rows.Count; i++)
                    //{
                    //    if (Orderid != "")
                    //    {
                    //        Orderid = Orderid + "," + orderlist.Tables[0].Rows[i]["entity_id"].ToString();
                    //    }
                    //    else
                    //    {
                    //        Orderid = orderlist.Tables[0].Rows[i]["entity_id"].ToString();
                    //    }
                    //}
                    //if (Orderid != "")
                    //{
                    //    txtidlist.Text = Orderid.ToString();
                    //}

                }
            }
            catch (Exception ex)
            {
            }
        }
        protected void btnsubmit_OnClick(object sender, EventArgs e)
        {
            try
            {
                getOrderNumbers();
            }
            catch (Exception ex)
            {
            }
            //queryString = "SELECT * FROM bb_email_config WHERE is_deleted='true' AND email_type_id='24'";
            //queryString = "SELECT * FROM sales_flat_order_item WHERE order_id='287'";

            //queryString = "SELECT * FROM sales_flat_order_item where parent_item_id is null and Name like 'YOYO%' and created_at between '" + Convert.ToDateTime(TbxFromDate.Text.ToString()).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(TbxToDate.Text.ToString()).ToString("yyyy-MM-dd") + "'";
            //Response.Write(queryString);
            //if (queryString != "")
            //{
            //    MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

            //    DataSet orderlist = new DataSet();
            //    adapteradminmail.Fill(orderlist, "sales_flat_order_item");
            //    GridView1.DataSource = orderlist;
            //    GridView1.DataBind();

            //}
        }
        public void getOrderDetails()
        {
            try
            {
                if (txtidlist.Text != "")
                {
                    //queryString = "SELECT entity_id, customer_firstname, customer_lastname FROM `sales_flat_order` where created_at between '" + Convert.ToDateTime(TbxFromDate.Text.ToString()).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(TbxToDate.Text.ToString()).ToString("yyyy-MM-dd") + "'";
                    queryString = @"SELECT z.increment_id as entity_id, a.customer_firstname, a.customer_lastname, b.Name
FROM `sales_flat_order_grid` as z inner join `sales_flat_order` AS a 
ON z.entity_id=a.entity_id
INNER JOIN `sales_flat_order_item` AS b ON a.entity_id = b.order_id
WHERE b.parent_item_id IS NULL and b.Name like 'YOYO%'  and z.increment_id in (" + txtidlist.Text + ")";
                    //Response.Write(queryString);
                    if (queryString != "")
                    {
                        //ConnectSsh sshobj = new ConnectSsh();
                        //SshClient client = sshobj.SshConnect();
                        MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                        DataSet orderlist = new DataSet();
                        adapteradminmail.Fill(orderlist, "sales_flat_order_item");
                        GVOrderDetails.DataSource = orderlist;
                        GVOrderDetails.DataBind();
                        GVMail.DataSource = orderlist;
                        GVMail.DataBind();
                        if (orderlist.Tables[0].Rows.Count > 0)
                        {
                            btnPDF.Visible = true;
                            btnXLS.Visible = true;
                            //btnsendexcel.Visible = true;
                        }
                        else
                        {
                            btnPDF.Visible = false;
                            btnXLS.Visible = false;
                            //btnsendexcel.Visible = false;
                        }
                        //sshobj.SshDisconnect(client);
                    }
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg;//ex.ToString();
            }

        }
        protected void GVOrderDetails_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            //GridViewRow row = (GridViewRow)(((Control)sender).NamingContainer);
            //GridViewRow row = (GridViewRow)((Control)sender).NamingContainer;
            GridViewRow row = e.Row;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((GridView)row.FindControl("GvYOYOwithNameList")).DataSource = getYOYOwithNames(((Label)row.FindControl("lblorderid")).Text.ToString());
                ((GridView)row.FindControl("GvYOYOwithNameList")).DataBind();
            }
        }
        protected void GVMail_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            //GridViewRow row = (GridViewRow)(((Control)sender).NamingContainer);
            //GridViewRow row = (GridViewRow)((Control)sender).NamingContainer;
            GridViewRow row = e.Row;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((GridView)row.FindControl("GvYOYOwithNameList")).DataSource = getYOYOwithNames(((Label)row.FindControl("lblorderid")).Text.ToString());
                ((GridView)row.FindControl("GvYOYOwithNameList")).DataBind();
            }
        }
        public DataTable getYOYOwithNames(string orderid)
        {
            DataTable dt = new DataTable("YOYONames");
            dt.Clear();
            dt.Columns.Add("Name");
            dt.Columns.Add("Sno");
            try
            {
                queryString = @"SELECT DISTINCT c.title, a.Name,a.qty_ordered
FROM `sales_flat_order_grid` as z inner join `sales_flat_order_item` AS a
ON z.entity_id=a.order_id
INNER JOIN `catalog_product_bundle_selection` AS b ON a.product_id = b.product_id
INNER JOIN `catalog_product_bundle_option_value` AS c ON b.option_id = c.option_id 
WHERE a.Name NOT LIKE '%t Want To Buy This Product/' and z.increment_id in (" + orderid + ")";
                //Response.Write(queryString);
                
                DataSet orderlist = new DataSet();
                if (queryString != "")
                {
                    //ConnectSsh sshobj = new ConnectSsh();
                    //SshClient client = sshobj.SshConnect();

                    MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);


                    adapteradminmail.Fill(orderlist, "YOYOwithNames");
                    //GvYOYOwithNameList.DataSource = orderlist;
                    //GvYOYOwithNameList.DataBind();



                    string nam = "";
                    int n = 0;
                    int countpro = 0;
                    foreach (DataRow row in orderlist.Tables[0].Rows)
                    {
                        DataRow _YOYONames = dt.NewRow();

                        int qty = Convert.ToInt32(Convert.ToDecimal(row["qty_ordered"].ToString()));
                        string qtyvalue = "";
                        if (qty > 1)
                        {
                            qtyvalue = " x " + qty.ToString();
                        }
                        if (nam != row["title"].ToString())
                        {
                            if (n == 1)
                            {
                                DataRow _YOYONames1 = dt.NewRow();

                                if (row["Name"].ToString() != "Don't Want To Buy This Product/")
                                {
                                    _YOYONames1["Name"] = "<b>" + row["title"].ToString() + "</b>";
                                    dt.Rows.Add(_YOYONames1);
                                }
                                
                                if (countpro != 0)
                                {
                                    if (row["Name"].ToString() != "Don't Want To Buy This Product/" && countpro != 0)
                                    {
                                        //_YOYONames["Name"] = countpro.ToString() + ". " + row["Name"].ToString();
                                        _YOYONames["Name"] = row["Name"].ToString() + qtyvalue;
                                        _YOYONames["Sno"] = countpro.ToString();
                                    }
                                    else
                                    {
                                        _YOYONames["Name"] = "  " + row["Name"].ToString() + qtyvalue;
                                    }
                                }
                                else
                                {
                                    _YOYONames["Name"] = row["Name"].ToString() + qtyvalue;
                                }
                            }
                            else
                            {
                                if (row["Name"].ToString() != "Don't Want To Buy This Product/" && countpro != 0)
                                {
                                    //_YOYONames["Name"] = countpro.ToString() + ". " + row["Name"].ToString();
                                    _YOYONames["Name"] = row["Name"].ToString() + qtyvalue;
                                    _YOYONames["Sno"] = countpro.ToString();
                                }
                                else
                                {
                                    //Here only add the pack title like 'YOYO 10'
                                    _YOYONames["Name"] = "<b>" + row["Name"].ToString() + qtyvalue + "</b>";
                                    _YOYONames["Sno"] = "<b>" + orderid.ToString() + "</b>";
                                }
                            }
                        }
                        else
                        {
                            if (row["Name"].ToString() != "Don't Want To Buy This Product/" && countpro!=0)
                            {
                                //_YOYONames["Name"] = countpro.ToString() + ". " + row["Name"].ToString();
                                _YOYONames["Name"] = row["Name"].ToString() + qtyvalue;
                                _YOYONames["Sno"] = countpro.ToString();
                            }
                            else
                            {
                                //_YOYONames["Name"] = "  " + row["Name"].ToString();
                            }
                        }
                        if (row["Name"].ToString() != "Don't Want To Buy This Product/")
                        {
                            dt.Rows.Add(_YOYONames);
                        }
                        nam = row["title"].ToString();
                        n = 1;
                        if (row["Name"].ToString() != "Don't Want To Buy This Product/")
                        {
                            countpro = countpro + 1;
                        }
                    }
                    //sshobj.SshDisconnect(client);
                }
                
            }
            catch(Exception ex)
            {
            }
            return dt;
        }

        private void PrintAllPages()
        {
            GVOrderDetails.AllowPaging = false;
            //GVOrderDetails.DataBind();
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            System.Web.UI.HtmlControls.HtmlForm frm = new System.Web.UI.HtmlControls.HtmlForm();
            //f.Controls.Add(GVOrderDetails);
            //GVOrderDetails.RenderControl(hw);
            GVOrderDetails.AllowPaging = false;
            //HtmlForm frm = new HtmlForm();
            GVOrderDetails.Parent.Controls.Add(frm);
            //frm.Attributes["runat"] = "server";
            frm.Controls.Add(GVOrderDetails);
            frm.RenderControl(hw);

            string gridHTML = sw.ToString().Replace("\"", "'")
                .Replace(System.Environment.NewLine, "");
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("window.onload = new function(){");
            sb.Append("var printWin = window.open('', '', 'left=0");
            sb.Append(",top=0,width=1000,height=600,status=0');");
            sb.Append("printWin.document.write(\"");
            sb.Append(gridHTML);
            sb.Append("\");");
            sb.Append("printWin.document.close();");
            sb.Append("printWin.focus();");
            sb.Append("printWin.print();");
            sb.Append("printWin.close();};");
            sb.Append("</script>");
            ClientScript.RegisterStartupScript(this.GetType(), "GridPrint", sb.ToString());
            //GVOrderDetails.AllowPaging = true;
            //GVOrderDetails.DataBind();
        }
        //protected void btnPDF_Click(object sender, ImageClickEventArgs e)
        //{
            //PrintAllPages();
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("content-disposition", "attachment;filename=UserDetails.pdf");
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter hw = new HtmlTextWriter(sw);
            //System.Web.UI.HtmlControls.HtmlForm f = new System.Web.UI.HtmlControls.HtmlForm();
            ////Panel Tom = new Panel();
            ////Tom.ID = base.UniqueID;
            ////Tom.Controls.Add(myControl);
            ////Page.FindControl("WebForm1").Controls.Add(Tom);

            //GVOrderDetails.AllowPaging = false;
            //f.Controls.Add(GVOrderDetails);
            ////f.Controls.Add(((GridView)GVOrderDetails.FindControl("GvYOYOwithNameList")));
            ////GVOrderDetails2.DataBind();
            //GVOrderDetails.RenderControl(hw);
            ////GVOrderDetails2.HeaderRow.Style.Add("width", "15%");
            ////GVOrderDetails2.HeaderRow.Style.Add("font-size", "10px");
            ////GVOrderDetails2.Style.Add("text-decoration", "none");
            ////GVOrderDetails2.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
            ////GVOrderDetails2.Style.Add("font-size", "8px");

            //StringReader sr = new StringReader(sw.ToString());
            //Document pdfDoc = new Document(PageSize.A2, 7f, 7f, 7f, 0f);
            //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            //pdfDoc.Open();
            //htmlparser.Parse(sr);
            //pdfDoc.Close();
            //Response.Write(pdfDoc);
            //Response.End();
        //}
        protected void btnPDF_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //Response.ContentType = "application/pdf";
                //Response.AddHeader("content-disposition", "attachment;filename=YOYOwithNamesInfo.pdf");
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //StringWriter sw = new StringWriter();
                //HtmlTextWriter hw = new HtmlTextWriter(sw);
                ////System.Web.UI.HtmlControls.HtmlForm f = new System.Web.UI.HtmlControls.HtmlForm();
                ////Panel Tom = new Panel();
                ////Tom.ID = base.UniqueID;
                ////Tom.Controls.Add(myControl);
                ////Page.FindControl("WebForm1").Controls.Add(Tom);

                //GVOrderDetails.AllowPaging = false;
                //HtmlForm frm = new HtmlForm();
                //GVOrderDetails.Parent.Controls.Add(frm);
                //frm.Attributes["runat"] = "server";
                //frm.Controls.Add(GVOrderDetails);
                //frm.RenderControl(hw);

                ////GVOrderDetails2.DataBind();
                ////GVOrderDetails.RenderControl(hw);
                ////GVOrderDetails2.HeaderRow.Style.Add("width", "15%");
                ////GVOrderDetails2.HeaderRow.Style.Add("font-size", "10px");
                ////GVOrderDetails2.Style.Add("text-decoration", "none");
                ////GVOrderDetails2.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                ////GVOrderDetails2.Style.Add("font-size", "8px");

                //StringReader sr = new StringReader(sw.ToString());
                //Document pdfDoc = new Document(PageSize.A4, 10f, 250f, 10f, 0f);
                //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                //pdfDoc.Open();
                //htmlparser.Parse(sr);
                //pdfDoc.Close();
                //Response.Write(pdfDoc);
                //Response.End();
                ////ExportGridViewToPDF(GVOrderDetails);

                //To Start second code
                Document pdfDoc = new Document(PageSize.A4, 10f, 240f, 10f, 0f);
                //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                PdfWriter pdfwriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfwriter.PageEvent = new Footer(Server.MapPath("Images/Calibri.ttf"),35);
                pdfDoc.Open();
                iTextSharp.text.pdf.PdfPTable table = new iTextSharp.text.pdf.PdfPTable(GVOrderDetails.Columns.Count);
                int k = 1;
                foreach (GridViewRow row in GVOrderDetails.Rows)
                {
                    
                    GridView gvtest = ((GridView)row.FindControl("GvYOYOwithNameList"));
                    Label gvfname = ((Label)row.FindControl("lblname"));
                    Label gvlname = ((Label)row.FindControl("Label1"));
                    gvtest.AllowPaging = false;
                    //GridView1.DataBind();
                    string s = Server.MapPath("Images/Calibri.ttf");
                    BaseFont bf = BaseFont.CreateFont(s, BaseFont.IDENTITY_H, true);
                    //BaseFont bf = BaseFont.CreateFont(Environment.GetEnvironmentVariable("windir") + @"\fonts\CALIBRI.TTF", BaseFont.IDENTITY_H, true);



                    //table = new iTextSharp.text.pdf.PdfPTable(GVOrderDetails.Columns.Count);
                    int[] widths = new int[gvtest.Columns.Count];
                    for (int x = 0; x < gvtest.Columns.Count; x++)
                    {
                        widths[x] = (int)gvtest.Columns[x].ItemStyle.Width.Value;
                        //string cellText = Server.HtmlDecode(gvtest.HeaderRow.Cells[x].Text);
                        string cellText = "";
                        if (x != 0)
                        {
                            cellText= Server.HtmlDecode(gvfname.Text.ToString()+" "+gvlname.Text.ToString());
                        }
                        else
                        {
                            cellText = Server.HtmlDecode(k.ToString());
                        }
                         
                        //Set Font and Font Color
                        iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 11, iTextSharp.text.Font.BOLD);
                        //font.Color = new Color(GVOrderDetails2.HeaderStyle.ForeColor);
                        //font.Color = new Color(GVOrderDetails2.RowStyle.ForeColor);
                        iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(new Phrase(12, cellText, font));
                        //Set Header Row BackGround Color
                        //cell.BackgroundColor = new Color(GVOrderDetails2.HeaderStyle.BackColor);


                        table.AddCell(cell);
                    }
                    table.SetWidths(widths);



                    for (int i = 0; i < gvtest.Rows.Count; i++)
                    {
                        if (gvtest.Rows[i].RowType == DataControlRowType.DataRow)
                        {
                            for (int j = 0; j < gvtest.Columns.Count; j++)
                            {
                                string cellText = "";
                                if (j != 0)
                                {
                                    //cellText = Server.HtmlDecode(GVOrderDetails.Rows[i].Cells[j].Text);
                                    cellText = Server.HtmlDecode(gvtest.Rows[i].Cells[j].Text);
                                    
                                }
                                else
                                {
                                    //cellText = Server.HtmlDecode((i + 1).ToString());
                                    cellText = Server.HtmlDecode(gvtest.Rows[i].Cells[j].Text);
                                }
                                iTextSharp.text.Font font;
                                if (cellText.Contains("<"))
                                {
                                     font= new iTextSharp.text.Font(bf, 11, iTextSharp.text.Font.BOLD);
                                     string regex = @"(<.+?>)";
                                     cellText = Regex.Replace(cellText, regex, "").Trim();
                                }
                                else
                                {
                                    font = new iTextSharp.text.Font(bf, 11, iTextSharp.text.Font.NORMAL);
                                }
                                //Set Font and Font Color
                                //iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 11, iTextSharp.text.Font.NORMAL);
                                //font.Color = new Color(GVOrderDetails2.RowStyle.ForeColor);
                                
                                iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(new Phrase(12, cellText, font));
                                if (j == 0)
                                {
                                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                                }
                                //Set Color of row
                                //if (i % 2 == 0)
                                //{
                                //    //Set Row BackGround Color
                                //    cell.BackgroundColor = new Color(GVOrderDetails2.RowStyle.BackColor);
                                //}
                                table.AddCell(cell);
                                
                            }
                        }
                    }
                    //iTextSharp.text.pdf.PdfPCell emptycell = new iTextSharp.text.pdf.PdfPCell(new Phrase(12, ""));
                    //emptycell.PaddingTop = 50;
                    
                    //table.AddCell(emptycell);
                    //table.AddCell(emptycell);

                    pdfDoc.Add(table);
                    table.DeleteBodyRows();
                    pdfDoc.NewPage();
                    k++;
                }
                    //Create the PDF Document
                    
                    
                    pdfDoc.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=YOYOwithNamesInfo.pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(pdfDoc);
                    Response.End();
                
                
            }
            catch (Exception ex)
            {
            }
        }
        protected void btnXLS_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "YOYOwithNamesInfo.xls"));
                Response.ContentType = "application/ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                GVOrderDetails.AllowPaging = false;
                //Change the Header Row back to white color
                GVOrderDetails.HeaderRow.Style.Add("background-color", "#FFFFFF");
                //Applying stlye to gridview header cells
                for (int i = 0; i < GVOrderDetails.HeaderRow.Cells.Count; i++)
                {
                    GVOrderDetails.HeaderRow.Cells[i].Style.Add("background-color", "#507CD1");
                }
                int j = 1;
                //This loop is used to apply stlye to cells based on particular row
                foreach (GridViewRow gvrow in GVOrderDetails.Rows)
                {
                    //gvrow.BackColor = Color.WHITE.ToString;
                    if (j <= GVOrderDetails.Rows.Count)
                    {
                        if (j % 2 != 0)
                        {
                            for (int k = 0; k < gvrow.Cells.Count; k++)
                            {
                                gvrow.Cells[k].Style.Add("background-color", "#EFF3FB");
                            }
                        }
                    }
                    j++;
                    
                }
                HtmlForm frm = new HtmlForm();
                GVOrderDetails.Parent.Controls.Add(frm);
                frm.Attributes["runat"] = "server";
                frm.Controls.Add(GVOrderDetails);
                frm.RenderControl(htw);

                //System.Web.UI.HtmlControls.HtmlForm f = new System.Web.UI.HtmlControls.HtmlForm();
                //f.Controls.Add(GVOrderDetails);
                //GVOrderDetails2.DataBind();

                //GVOrderDetails.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
                //ExportGridViewToExcel();
            }
            catch (Exception ex)
            {
            }
        }
        private void ExportGridViewToExcel()
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Export.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.xls";
            StringWriter stringWrite = new StringWriter();
            HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            /*--------------Change GridView control under a Form------------------*/
            HtmlForm frm = new HtmlForm();
            GVOrderDetails.Parent.Controls.Add(frm);
            frm.Attributes["runat"] = "server";
            frm.Controls.Add(GVOrderDetails);
            frm.RenderControl(htmlWrite);
            /*--------------------------------*/
            //gvRegion.RenderControl(htmlWrite);
            Response.Write(stringWrite);
            Response.End();
        }

        private void ExportGridViewToPDF(GridView GridView1)
        {
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Export.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            HtmlForm frm = new HtmlForm();
            GridView1.Parent.Controls.Add(frm);
            frm.Attributes["runat"] = "server";
            frm.Controls.Add(GridView1);
            frm.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 5f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
        }
        private void ExportGridViewToWord(GridView GridView1)
        {
            Response.AddHeader("content-disposition", "attachment;filename=Export.doc");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.word";

            StringWriter stringWrite = new StringWriter();
            HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

            HtmlForm frm = new HtmlForm();
            GridView1.Parent.Controls.Add(frm);
            frm.Attributes["runat"] = "server";
            frm.Controls.Add(GridView1);
            frm.RenderControl(htmlWrite);

            Response.Write(stringWrite.ToString());
            Response.End();
        }
        protected void btnsendexcel_OnClick(object sender, EventArgs e)
        {
            try
            {
                string filepath = System.Configuration.ConfigurationManager.AppSettings["FilePath"].ToString();

                GVMail.Visible = true;
                string filename = "YOYOwithNamesInfo" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss");
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

                GVMail.AllowPaging = false;
                HtmlForm frm = new HtmlForm();
                GVMail.Parent.Controls.Add(frm);
                frm.Attributes["runat"] = "server";
                frm.Controls.Add(GVMail);
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
                GVMail.Visible = false;


                string mailto = System.Configuration.ConfigurationManager.AppSettings["Mail_To"].ToString();
                string mailcredential = System.Configuration.ConfigurationManager.AppSettings["Mail_Credential"].ToString();
                string mailpassword = System.Configuration.ConfigurationManager.AppSettings["Mail_Password"].ToString();

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress(mailcredential);
                mail.To.Add(mailto);
                mail.Subject = "PFA - YOYOwithNamesInfo(XLS)";
                mail.Body = "PFA - YOYOwithNamesInfo(XLS)";

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

        protected void CBorder_OnCheckedChanged(object sender, EventArgs e)
        {
            string orderlist = "";
            foreach (GridViewRow itm in GVOrdernumbers.Rows)
            {
                if (((CheckBox)itm.FindControl("CBorder")).Checked)
                {
                    if (txtidlist.Text.ToString().Contains(((HiddenField)itm.FindControl("HFordernum")).Value.ToString()) == false)
                    {
                        if (orderlist != "")
                        {
                            orderlist = orderlist + "," + ((HiddenField)itm.FindControl("HFordernum")).Value.ToString();
                        }
                        else
                        {
                            orderlist = ((HiddenField)itm.FindControl("HFordernum")).Value.ToString();
                        }
                    }
                }
                else
                {
                    if (txtidlist.Text.ToString().Contains(((HiddenField)itm.FindControl("HFordernum")).Value.ToString()) == true)
                    {
                        txtidlist.Text = txtidlist.Text.ToString().Replace(((HiddenField)itm.FindControl("HFordernum")).Value.ToString(), "");
                    }
                    txtidlist.Text = txtidlist.Text.ToString().Replace(",,", ",");
                    if (txtidlist.Text.ToString().EndsWith(","))
                    {
                        txtidlist.Text = txtidlist.Text.ToString().Remove(txtidlist.Text.ToString().Length - 1);
                    }
                    if (txtidlist.Text.ToString().StartsWith(","))
                    {
                        txtidlist.Text = txtidlist.Text.ToString().Remove(0, 1);
                    }
                }
            }
            if (orderlist != "")
            {
                if (txtidlist.Text != "")
                {
                    txtidlist.Text = txtidlist.Text + "," + orderlist;
                }
                else
                {
                    txtidlist.Text = orderlist;
                }
            }
        }
        protected void CBorderall_OnCheckedChanged(object sender, EventArgs e)
        {
            string orderlist = "";

            //((CheckBox)GVOrderDetails.HeaderRow.FindControl("CBorderall")).Checked

            foreach (GridViewRow itm in GVOrdernumbers.Rows)
            {
                if (((CheckBox)GVOrdernumbers.HeaderRow.FindControl("CBorderall")).Checked)
                {
                    ((CheckBox)itm.FindControl("CBorder")).Checked = true;
                }
                else
                {
                    ((CheckBox)itm.FindControl("CBorder")).Checked = false;
                }

                if (((CheckBox)itm.FindControl("CBorder")).Checked)
                {
                    if (txtidlist.Text.ToString().Contains(((HiddenField)itm.FindControl("HFordernum")).Value.ToString()) == false)
                    {
                        if (orderlist != "")
                        {
                            orderlist = orderlist + "," + ((HiddenField)itm.FindControl("HFordernum")).Value.ToString();
                        }
                        else
                        {
                            orderlist = ((HiddenField)itm.FindControl("HFordernum")).Value.ToString();
                        }
                    }
                    
                }
                else
                {
                    if (txtidlist.Text.ToString().Contains(((HiddenField)itm.FindControl("HFordernum")).Value.ToString()) == true)
                    {
                        txtidlist.Text = txtidlist.Text.ToString().Replace(((HiddenField)itm.FindControl("HFordernum")).Value.ToString(), "");
                    }
                    txtidlist.Text = txtidlist.Text.ToString().Replace(",,", ",");
                    if (txtidlist.Text.ToString().EndsWith(","))
                    {
                        txtidlist.Text = txtidlist.Text.ToString().Remove(txtidlist.Text.ToString().Length - 1);
                    }
                    if (txtidlist.Text.ToString().StartsWith(","))
                    {
                        txtidlist.Text = txtidlist.Text.ToString().Remove(0, 1);
                    }
                }
            }
            if (orderlist != "")
            {
                if (txtidlist.Text != "")
                {
                    txtidlist.Text = txtidlist.Text + "," + orderlist;
                }
                else
                {
                    txtidlist.Text = orderlist;
                }
            }
        }
        public static DataTable resort(DataTable dt, string colName, string direction)
        {
            dt.DefaultView.Sort = colName + " " + direction;
            dt = dt.DefaultView.ToTable();
            return dt;
        }
    }
}