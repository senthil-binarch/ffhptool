using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Drawing;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Globalization;
//using Renci.SshNet;

namespace FFHPWeb
{
    public partial class Orders : System.Web.UI.Page
    {//string conn = "server=68.178.143.107;userid=stagingffhpin;password=Stag1ngffHP!n;database=stagingffhpin;Convert Zero Datetime=True";  //staging
        //string conn = "server=68.178.143.11;userid=ffhpmagento;password=D6QpT!KDd0dKHI;database=ffhpmagento;Convert Zero Datetime=True";  //Live
        string conn = System.Configuration.ConfigurationManager.AppSettings["Connection"].ToString();
        string queryString = "";
        MySqlDataAdapter DA;
        string s = "";
        bool t = false;
        int DewCount = 0;
        int ShineCount = 0;
        int GrandCount = 0;
        int JainCount = 0;
        int YOURPACKv1Count = 0;
        string errormsg = "Try again";
        protected void Page_Load(object sender, EventArgs e)
        {
            //string s = Regex.Matches("Himayat 5Kgs, Alphonso 2.5Kgs,Prime , Banganapalli 2.5Kgs,Banganapalli 2.5Kgs, Taaza", "Banganapalli 2.5Kgs").Count.ToString();
            try
            {
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
                        getOrderDetailsbyorderid();
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

            lblerror.Text = "";
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
        public void getOrderDetails()
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
                    //ConnectSsh sshobj = new ConnectSsh();
                    //SshClient client = sshobj.SshConnect();
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

                    GVOrderDetails.DataSource = testdt;
                    GVOrderDetails.DataBind();
                    if (txtidlist.Text != "")
                    {
                        string[] txtid = txtidlist.Text.ToString().Split(',');
                        if (txtid.Count() > 0)
                        {
                            foreach (GridViewRow row in GVOrderDetails.Rows)
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
                    //sshobj.SshDisconnect(client);
                }
            }
            catch (Exception ex)
            {
            }
        }
//        public void getOrderDetails()
//        {
//            try
//            {
//                //queryString = "SELECT entity_id, customer_firstname, customer_lastname FROM `sales_flat_order` where created_at between '" + Convert.ToDateTime(TbxFromDate.Text.ToString()).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(TbxToDate.Text.ToString()).ToString("yyyy-MM-dd") + "'";
//                //a.customer_id = c.customer_id AND 
//                queryString = @"SELECT  DISTINCT a.entity_id, a.customer_id, CONCAT( IFNULL(c.firstname,''), ' ', IFNULL(c.lastname,'') ) AS customername, CONCAT( IFNULL( c.street, '' ) , ', ', IFNULL( c.city, '' ) , ', ', IFNULL( c.region, '' ) , ', ', IFNULL( c.postcode, '' ) , ', India, T:', IFNULL( c.telephone, '' ) ) AS Address, b.Name
//FROM `sales_flat_order` AS a
//LEFT OUTER JOIN `sales_flat_order_item` AS b ON a.entity_id = b.order_id
//LEFT OUTER JOIN `sales_flat_order_address` AS c ON c.parent_id = b.order_id
//WHERE b.parent_item_id IS NULL
//AND product_type
//IN (
//'simple', 'bundle', 'grouped'
//) AND c.address_type = 'shipping' and a.created_at between '" + Convert.ToDateTime(TbxFromDate.Text.ToString()).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(TbxToDate.Text.ToString()).ToString("yyyy-MM-dd") + "'";
//                //Response.Write(queryString);
//                if (queryString != "")
//                {
//                    MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

//                    DataSet orderlist = new DataSet();
//                    adapteradminmail.Fill(orderlist, "ordernumber");
//                    //GVOrderDetails.DataSource = orderlist;
//                    //GVOrderDetails.DataBind();
//                    string Orderid = "";
//                    for (int i = 0; i < orderlist.Tables[0].Rows.Count; i++)
//                    {
//                        if (Orderid != "")
//                        {
//                            Orderid = Orderid + "," + orderlist.Tables[0].Rows[i]["entity_id"].ToString();
//                        }
//                        else
//                        {
//                            Orderid = orderlist.Tables[0].Rows[i]["entity_id"].ToString();
//                        }
//                    }
//                    if (Orderid != "")
//                    {
//                        txtidlist.Text = Orderid.ToString();
//                    }

//                }
//            }
//            catch (Exception ex)
//            {
//            }
//        }
        protected void btnsubmit_OnClick(object sender, EventArgs e)
        {
            try
            {
                getOrderDetails();
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
        public void getOrderDetailsbyorderid()
        {
            try
            {
                //queryString = "SELECT entity_id, customer_firstname, customer_lastname FROM `sales_flat_order` where created_at between '" + Convert.ToDateTime(TbxFromDate.Text.ToString()).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(TbxToDate.Text.ToString()).ToString("yyyy-MM-dd") + "'";
                //a.customer_id = c.customer_id AND 
                if (txtidlist.Text != "")
                {
                    queryString = @"SELECT  DISTINCT z.increment_id as entity_id, a.customer_id, CONCAT( IFNULL(c.firstname,''), ' ', IFNULL(c.lastname,'') ) AS customername, CONCAT( IFNULL( c.street, '' ) , ', ', IFNULL( c.city, '' ) , ', ', IFNULL( c.region, '' ) , ', ', IFNULL( c.postcode, '' ) , ', India, T:', IFNULL( c.telephone, '' ) ) AS Address, b.Name,CONCAT(IFNULL( d.area, '' ), ' ',IFNULL( c.postcode, '' ))as Zipcode,IFNULL( d.area, '' )as area,b.qty_ordered
FROM `sales_flat_order_grid` as z inner join `sales_flat_order` AS a 
ON z.entity_id=a.entity_id
INNER JOIN `sales_flat_order_item` AS b ON a.entity_id = b.order_id
INNER JOIN `sales_flat_order_address` AS c ON c.parent_id = b.order_id
LEFT OUTER JOIN 1_ffhp_pincode as d ON 
c.postcode=d.pincode
WHERE b.parent_item_id IS NULL
AND product_type
IN (
'simple', 'bundle', 'grouped'
) AND c.address_type = 'shipping' AND z.increment_id in (" + txtidlist.Text + ")";
                    //Response.Write(queryString);
                    if (queryString != "")
                    {
                        //ConnectSsh sshobj = new ConnectSsh();
                        //SshClient client = sshobj.SshConnect();

                        MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                        DataSet orderlist = new DataSet();
                        adapteradminmail.Fill(orderlist, "sales_flat_order_item");
                        //GVOrderDetails2.DataSource = orderlist;
                        //GVOrderDetails2.DataBind();
                        DataTable products = orderlist.Tables[0];

                        var distinctRows = (from DataRow dRow in products.Rows
                                            select new { col1 = dRow["Name"] }).Distinct();

                        //DataTable DTColumns = new DataTable();
                        //DTColumns = (DataTable)distinctRows;

                        int rows = orderlist.Tables[0].Rows.Count;
                        int columns = distinctRows.Count();

                        DataTable dt = new DataTable("PackListYOYO");
                        dt.Clear();
                        dt.Columns.Add("ColumnName");
                        dt.Columns.Add("ColumnZipcode");
                        dt.Columns.Add("Column0");
                        for (int i = 0; i < distinctRows.Count(); i++)
                        {
                            dt.Columns.Add("Column" + (i + 1).ToString());
                        }
                        dt.Columns.Add("Total");
                        //DataRow _PackOrder = dt.NewRow();
                        //for (int i = 0; i < DTColumns.Rows.Count; i++)
                        //{
                        //    _PackOrder["Column" + i+1.ToString()] = DTColumns.Rows[i]["Name"].ToString();
                        //}
                        //dt.Rows.Add(_PackOrder);

                        DataRow _PackOrderColumn = dt.NewRow();
                        _PackOrderColumn["Column0"] = "";
                        DataRow _PackOrder = dt.NewRow();
                        //_PackOrder["ColumnName"] = "Customer Name";
                        _PackOrder["ColumnName"] = "Order No.";
                        _PackOrder["ColumnZipcode"] = "Area";
                        _PackOrder["Column0"] = "Pack Name";
                        int j = 0;
                        foreach (var value in distinctRows)
                        {
                            j = j + 1;
                            _PackOrder["Column" + j.ToString()] = value.col1.ToString().Replace("Pack", "").Trim();

                        }
                        _PackOrder["Total"] = "Total";
                        dt.Rows.Add(_PackOrder);

                        DataSet orderlist1 = new DataSet();
                        orderlist1 = getorderlist(orderlist);

                        for (int i = 0; i < orderlist1.Tables[0].Rows.Count; i++)
                        {
                            DataRow _PackOrderRows = dt.NewRow();
                            //_PackOrderRows["ColumnName"] = orderlist1.Tables[0].Rows[i]["customername"].ToString().Trim();
                            _PackOrderRows["ColumnName"] = orderlist1.Tables[0].Rows[i]["entity_id"].ToString().Trim();
                            _PackOrderRows["ColumnZipcode"] = orderlist1.Tables[0].Rows[i]["Zipcode"].ToString().Trim();
                            _PackOrderRows["Column0"] = orderlist1.Tables[0].Rows[i]["Name"].ToString().Replace("Pack", "").Trim();
                            
                            int k = 0;
                            int t = 0;
                            
                            string[] qtyarr = orderlist1.Tables[0].Rows[i]["qty_ordered"].ToString().Trim().Split(',');
                            string[] namearr = orderlist1.Tables[0].Rows[i]["Name"].ToString().Replace("Pack", "").Trim().Split(',');
                            foreach (var value in distinctRows)
                            {
                                int qty = 0;
                                
                                for (int q = 0; q < namearr.Count(); q++)
                                {
                                    string q1 = value.col1.ToString().Replace("Pack", "").Trim().ToString();
                                    string q2 = namearr[q].ToString();
                                    if (namearr[q].ToString().Trim().ToString() == value.col1.ToString().Replace("Pack", "").Trim().ToString())
                                    {

                                        if (qty == 0)
                                        {
                                            qty = Convert.ToInt32(Convert.ToDecimal(qtyarr[q].ToString()));
                                        }
                                        else
                                        {
                                            qty = qty + Convert.ToInt32(Convert.ToDecimal(qtyarr[q].ToString()));
                                        }
                                        //removeIndex = Array.IndexOf(namearr, q2);
                                        //break;
                                    }
                                }
                                

                                    k = k + 1;
                                    int c = qty;//Regex.Matches(orderlist1.Tables[0].Rows[i]["Name"].ToString().Replace("Pack", "").Trim(), value.col1.ToString().Replace("Pack", "").Trim()).Count * qty;
                                
                                //DataTable products1 = null;
                                //products1 = products.AsEnumerable().Where(r => Convert.ToString(r["name"]) == s).AsDataView().ToTable();
                                //products1 = products.AsEnumerable().Where(r => Convert.ToString(r["name"]).Contains(s));
                                //filteredItems = filteredItems.Where(i => drawings.Contains(i => i.WorkItemNumber));
                                //int c = Regex.Matches(orderlist1.Tables[0].Rows[i]["Name"].ToString().Replace("Pack", "").Trim(), value.col1.ToString().Replace("Pack", "").Trim()).Count * Convert.ToInt32(Convert.ToDecimal(value.col2.ToString()));
                                //if (orderlist1.Tables[0].Rows[i]["Name"].ToString().Replace("Pack", "").Trim().Contains(value.col1.ToString().Replace("Pack", "").Trim()))
                                if (c>0)
                                {
                                    _PackOrderRows["Column" + k.ToString()] = c;
                                    t = t + c;
                                }
                            }
                            _PackOrderRows["Total"] = t.ToString();
                            dt.Rows.Add(_PackOrderRows);
                        }
                        int total = 0;
                        DataRow _PackOrderRowTotal = dt.NewRow();
                        _PackOrderRowTotal["ColumnName"] = "";
                        _PackOrderRowTotal["ColumnZipcode"] = "Sub Total";
                        _PackOrderRowTotal["Column0"] = "";
                        for (int l = 1; l < dt.Columns.Count-3; l++)
                        {
                            int tot = 0;
                            for (int m = 0; m < dt.Rows.Count; m++)
                            {
                                string s = dt.Rows[m]["Column" + l.ToString()].ToString();
                                int che;
                                bool isNumeric = int.TryParse(s, out che);
                                if (isNumeric)
                                {
                                    if (dt.Rows[m]["Column" + l.ToString()].ToString() != "")
                                    {

                                        tot = tot + Convert.ToInt32(dt.Rows[m]["Column" + l.ToString()].ToString());
                                    }
                                }
                            }
                            _PackOrderRowTotal["Column" + l.ToString()] = tot;

                            total = total + tot;
                        }
                        _PackOrderRowTotal["Total"] = total.ToString();
                        dt.Rows.Add(_PackOrderRowTotal);

                        DataRow _PackOrderColumnEnd = dt.NewRow();
                        _PackOrderColumnEnd["Column0"] = "";
                        DataRow _PackOrderEnd = dt.NewRow();
                        _PackOrderEnd["ColumnName"] = "";
                        _PackOrderEnd["ColumnZipcode"] = "Total";
                        _PackOrderEnd["Column0"] = total.ToString();
                        int n = 0;
                        foreach (var value in distinctRows)
                        {
                            n = n + 1;
                            _PackOrderEnd["Column" + n.ToString()] = value.col1.ToString().Replace("Pack", "").Trim();

                        }
                        _PackOrderEnd["Total"] = "";
                        dt.Rows.Add(_PackOrderEnd);
                        if (dt.Rows.Count > 0)
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
                        GvOrders.DataSource = dt;
                        GvOrders.DataBind();
                        GVMail.DataSource = dt;
                        GVMail.DataBind();
                        //sshobj.SshDisconnect(client);
                    }
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
        }
        
        public DataSet getorderlist(DataSet orderlist)
        {
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataTable testdt = new DataTable("testPackList");
            testdt.Clear();
            testdt.Columns.Add("entity_id");
            testdt.Columns.Add("customername");
            testdt.Columns.Add("Address");
            testdt.Columns.Add("Name");
            testdt.Columns.Add("Zipcode");
            testdt.Columns.Add("qty_ordered");
            try
            {
              
                DataTable orderlist1 = new DataTable();
                orderlist1 = orderlist.Tables[0];
                string Entityid = "";
                string TestValue1 = "";
                string customername = "";
                string Address = "";
                string TestPack = "";
                string Zipcode = "";
                string qty = "";
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
                            if (qty == "")
                            {
                                qty = dtt1.Rows[i]["qty_ordered"].ToString();
                            }
                            else
                            {
                                qty = qty + dtt1.Rows[i]["qty_ordered"].ToString();
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
                            if (qty == "")
                            {
                                qty = dtt1.Rows[i]["qty_ordered"].ToString();
                            }
                            else
                            {
                                qty = qty + ", " + dtt1.Rows[i]["qty_ordered"].ToString();
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
                            _TestPackList["qty_ordered"] = qty.ToString();
                            testdt.Rows.Add(_TestPackList);

                            TestPack = "";
                            qty = "";
                            TestPack = dtt1.Rows[i]["Name"].ToString();
                            qty = dtt1.Rows[i]["qty_ordered"].ToString();
                        }
                        TestValue1 = Entityid.ToString();

                        customername = dtt1.Rows[i]["customername"].ToString();
                        Address = dtt1.Rows[i]["Address"].ToString();
                        Zipcode = dtt1.Rows[i]["Zipcode"].ToString();
                        //qty = dtt1.Rows[i]["qty_ordered"].ToString();
                    }
                    DataRow _TestPackListfinalrecord = testdt.NewRow();
                    _TestPackListfinalrecord["entity_id"] = TestValue1.ToString();
                    _TestPackListfinalrecord["customername"] = customername.ToString();
                    _TestPackListfinalrecord["Address"] = Address.ToString();
                    _TestPackListfinalrecord["Name"] = TestPack.ToString();
                    _TestPackListfinalrecord["Zipcode"] = Zipcode.ToString();
                    _TestPackListfinalrecord["qty_ordered"] = qty.ToString();
                    testdt.Rows.Add(_TestPackListfinalrecord);
                }
                ds.Tables.Add(testdt);
                ds1 = getorderlistwithzipcod(ds);
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
            return ds1;
        }
        public DataSet getorderlistwithzipcod(DataSet orderlist)
        {
            DataSet ds = new DataSet();
            DataTable testdt = new DataTable("testPackList");
            testdt.Clear();
            testdt.Columns.Add("entity_id");
            testdt.Columns.Add("customername");
            testdt.Columns.Add("Address");
            testdt.Columns.Add("Name");
            testdt.Columns.Add("Zipcode");
            testdt.Columns.Add("qty_ordered");
            try
            {

                DataTable orderlist1 = new DataTable();
                orderlist1 = orderlist.Tables[0];
                string Entityid = "";
                string TestValue1 = "";
                string customername = "";
                string Address = "";
                string TestPack = "";
                string Zipcode = "";
                string qty = "";

                DataTable dtt1 = resort(orderlist1, "Zipcode", "ASC");

                if (dtt1.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt1.Rows.Count; i++)
                    {
                        Zipcode = dtt1.Rows[i]["Zipcode"].ToString();
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

                            if (customername == "")
                            {
                                customername = dtt1.Rows[i]["customername"].ToString();
                            }
                            else
                            {
                                customername = customername + dtt1.Rows[i]["customername"].ToString();
                            }

                            if (Entityid == "")
                            {
                                Entityid = dtt1.Rows[i]["entity_id"].ToString();
                            }
                            else
                            {
                                Entityid = Entityid + dtt1.Rows[i]["entity_id"].ToString();
                            }
                            if (qty == "")
                            {
                                qty = dtt1.Rows[i]["qty_ordered"].ToString();
                            }
                            else
                            {
                                qty = qty + dtt1.Rows[i]["qty_ordered"].ToString();
                            }
                        }
                        else if (Zipcode.Equals(TestValue1.ToString()))
                        {
                            if (TestPack == "")
                            {
                                TestPack = dtt1.Rows[i]["Name"].ToString();
                            }
                            else
                            {
                                TestPack = TestPack + ", " + dtt1.Rows[i]["Name"].ToString();
                            }

                            if (customername == "")
                            {
                                customername = dtt1.Rows[i]["customername"].ToString();
                            }
                            else
                            {
                                customername = customername + ", " + dtt1.Rows[i]["customername"].ToString();
                            }

                            if (Entityid == "")
                            {
                                Entityid = dtt1.Rows[i]["entity_id"].ToString();
                            }
                            else
                            {
                                Entityid = Entityid + ", " + dtt1.Rows[i]["entity_id"].ToString();
                            }
                            if (qty == "")
                            {
                                qty = dtt1.Rows[i]["qty_ordered"].ToString();
                            }
                            else
                            {
                                qty = qty + ", " + dtt1.Rows[i]["qty_ordered"].ToString();
                            }
                        }
                        else if (i > 0)
                        {

                            DataRow _TestPackList = testdt.NewRow();
                            _TestPackList["entity_id"] = Entityid.ToString();
                            _TestPackList["customername"] = customername.ToString();//dtt1.Rows[i]["customername"].ToString();
                            _TestPackList["Address"] = Address.ToString();//dtt1.Rows[i]["Address"].ToString();
                            _TestPackList["Name"] = TestPack.ToString();
                            _TestPackList["Zipcode"] = TestValue1.ToString();
                            _TestPackList["qty_ordered"] = qty.ToString(); 
                            testdt.Rows.Add(_TestPackList);

                            TestPack = "";
                            customername = "";
                            Entityid = "";
                            qty = "";
                            TestPack = dtt1.Rows[i]["Name"].ToString();
                            customername = dtt1.Rows[i]["customername"].ToString();
                            Entityid = dtt1.Rows[i]["entity_id"].ToString();
                            qty = dtt1.Rows[i]["qty_ordered"].ToString();
                        }
                        TestValue1 = Zipcode.ToString();
                        
                        //customername = dtt1.Rows[i]["customername"].ToString();
                        Address = dtt1.Rows[i]["Address"].ToString();
                        //Entityid = dtt1.Rows[i]["entity_id"].ToString();
                        
                    }
                    DataRow _TestPackListfinalrecord = testdt.NewRow();
                    _TestPackListfinalrecord["entity_id"] = Entityid.ToString();
                    _TestPackListfinalrecord["customername"] = customername.ToString();
                    _TestPackListfinalrecord["Address"] = Address.ToString();
                    _TestPackListfinalrecord["Name"] = TestPack.ToString();
                    _TestPackListfinalrecord["Zipcode"] = TestValue1.ToString();
                    _TestPackListfinalrecord["qty_ordered"] = qty.ToString();
                    testdt.Rows.Add(_TestPackListfinalrecord);
                }
                ds.Tables.Add(testdt);
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
            return ds;
        }
        protected void btnsubmit1_OnClick(object sender, EventArgs e)
        {
            try
            {
                ServiceOrderupdate();
                if (txtidlist.Text != "")
                {
                    Session["orderid"] = txtidlist.Text.ToString();

                    getOrderDetailsbyorderid();
                }
                else
                {
                    GvOrders.DataSource = null;
                    GvOrders.DataBind();
                    GVMail.DataSource = null;
                    GVMail.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg;//ex.ToString();
            }
        }
        protected void btnPDF_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=OrderInfo.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                System.Web.UI.HtmlControls.HtmlForm f = new System.Web.UI.HtmlControls.HtmlForm();
                //Panel Tom = new Panel();
                //Tom.ID = base.UniqueID;
                //Tom.Controls.Add(myControl);
                //Page.FindControl("WebForm1").Controls.Add(Tom);

                GvOrders.AllowPaging = false;
                f.Controls.Add(GvOrders);
                //GVOrderDetails2.DataBind();
                GvOrders.RenderControl(hw);
                //GVOrderDetails2.HeaderRow.Style.Add("width", "15%");
                //GVOrderDetails2.HeaderRow.Style.Add("font-size", "10px");
                //GVOrderDetails2.Style.Add("text-decoration", "none");
                //GVOrderDetails2.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                //GVOrderDetails2.Style.Add("font-size", "8px");

                StringReader sr = new StringReader(sw.ToString());
                Document pdfDoc = new Document(PageSize.A2, 7f, 7f, 7f, 0f);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                PdfWriter pdfwriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfwriter.PageEvent = new Footer(Server.MapPath("Images/Calibri.ttf"), 0);
                pdfDoc.Open();
                htmlparser.Parse(sr);
                pdfDoc.Close();
                Response.Write(pdfDoc);
                Response.End();
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg;//ex.ToString();
            }
        }
        protected void btnXLS_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "OrderInfo.xls"));
                Response.ContentType = "application/ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                GvOrders.AllowPaging = false;
                //Change the Header Row back to white color
                GvOrders.HeaderRow.Style.Add("background-color", "#FFFFFF");
                //Applying stlye to gridview header cells
                for (int i = 0; i < GvOrders.HeaderRow.Cells.Count; i++)
                {
                    GvOrders.HeaderRow.Cells[i].Style.Add("background-color", "#FFFFFF");
                    GvOrders.HeaderRow.Cells[i].Style.Add("color", "#000000");
                }
                int j = 1;
                //This loop is used to apply stlye to cells based on particular row
                foreach (GridViewRow gvrow in GvOrders.Rows)
                {
                    //gvrow.BackColor = Color.WHITE.ToString;
                    if (j <= GvOrders.Rows.Count)
                    {
                        if (j % 2 != 0)
                        {
                            for (int k = 0; k < gvrow.Cells.Count; k++)
                            {
                                gvrow.Cells[k].Style.Add("background-color", "#FFFFFF");
                                gvrow.Cells[k].Style.Add("color", "#000000");
                            }
                        }
                    }
                    j++;
                }
                System.Web.UI.HtmlControls.HtmlForm f = new System.Web.UI.HtmlControls.HtmlForm();
                f.Controls.Add(GvOrders);
                //GVOrderDetails2.DataBind();

                GvOrders.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
            catch (Exception ex)
            {
            }
        }
        protected void btnsendexcel_OnClick(object sender, EventArgs e)
        {
            try
            {
                string filepath = System.Configuration.ConfigurationManager.AppSettings["FilePath"].ToString();

                GVMail.Visible = true;
                string filename = "OrderInfo" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss");
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
                f.Controls.Add(GVMail);
                //GVOrderDetails2.DataBind();
                GVMail.RenderControl(hw);
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
                mail.Subject = "PFA - OrderInfo(XLS)";
                mail.Body = "PFA - OrderInfo(XLS)";

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
            foreach (GridViewRow itm in GVOrderDetails.Rows)
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
                        txtidlist.Text = txtidlist.Text.ToString().Remove(0,1);
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

            foreach (GridViewRow itm in GVOrderDetails.Rows)
            {
                if (((CheckBox)GVOrderDetails.HeaderRow.FindControl("CBorderall")).Checked)
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
                        txtidlist.Text = txtidlist.Text.ToString().Remove(0,1);
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
