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
//using System.Web.Mail;
using System.Net.Mail;
using iTextSharp.tool.xml;
using Excel = Microsoft.Office.Interop.Excel;
//using Renci.SshNet;
using System.Globalization;
namespace FFHPWeb
{
    public partial class ffhp1 : System.Web.UI.Page
    {
        //string conn = "server=68.178.143.107;userid=stagingffhpin;password=Stag1ngffHP!n;database=stagingffhpin;Convert Zero Datetime=True";  //staging
        //string conn = "server=68.178.143.11;userid=ffhpmagento;password=D6QpT!KDd0dKHI;database=ffhpmagento;Convert Zero Datetime=True";  //Live
        //string conn = "server=192.168.1.2;userid=;password=;database=ffhp;Convert Zero Datetime=True";  //Local
        string conn = System.Configuration.ConfigurationManager.AppSettings["Connection"].ToString();
        string queryString = "";
        MySqlDataAdapter DA;
        string s = "";
        bool t = false;
        int DewCount = 0;
        int ShineCount = 0;
        int GrandCount=0;
        int JainCount = 0;
        int YOURPACKv1Count = 0;
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
) AND c.address_type = 'shipping' and z.status!='canceled' and z.created_at between '" + dtf.ToString("yyyy-MM-dd") + "' and '" + dtt.AddDays(1).ToString("yyyy-MM-dd") + "'";

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

                    MySqlConnection con = new MySqlConnection(conn);
                    con.Open();

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
                                string s2= row.Cells[2].Text.ToString();
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
        public void getOrderDetailsbyorderid()
        {
            DataTable testdt = new DataTable("testPackList");
            testdt.Clear();
            testdt.Columns.Add("entity_id");
            testdt.Columns.Add("customername");
            testdt.Columns.Add("Address");
            testdt.Columns.Add("Name");
            testdt.Columns.Add("Zipcode");
            testdt.Columns.Add("Amount");
            try
            {
                DataTable dt = new DataTable("PackList");
                dt.Clear();
                dt.Columns.Add("entity_id");
                dt.Columns.Add("customername");
                dt.Columns.Add("Address");
                dt.Columns.Add("Name");
                dt.Columns.Add("Zipcode");
                dt.Columns.Add("Amount");

                //queryString = "SELECT entity_id, customer_firstname, customer_lastname FROM `sales_flat_order` where created_at between '" + Convert.ToDateTime(TbxFromDate.Text.ToString()).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(TbxToDate.Text.ToString()).ToString("yyyy-MM-dd") + "'";
                //a.customer_id = c.customer_id AND 
                if (txtidlist.Text != "")
                {
                    queryString = @"SELECT  DISTINCT z.increment_id as entity_id,z.grand_total, a.customer_id, CONCAT( IFNULL(c.firstname,''), ' ', IFNULL(c.lastname,'') ) AS customername, CONCAT( IFNULL( c.street, '' ) , ', ', IFNULL( c.city, '' ) , ', ', IFNULL( c.region, '' ) , ', ', IFNULL( c.postcode, '' ) , ', India, T:', IFNULL( c.telephone, '' ) ) AS Address, b.Name,CONCAT(IFNULL( d.area, '' ), ' ',IFNULL( c.postcode, '' ))as Zipcode,b.qty_ordered
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

                        DataTable orderlist1 = new DataTable();
                        orderlist1 = orderlist.Tables[0];
                        string Entityid = "";
                        string TestValue1 = "";
                        string customername = "";
                        string Address = "";
                        string TestPack = "";
                        string Zipcode = "";
                        Decimal Amount = 0;
                        Decimal TotalAmount = 0;
                        int qty = 0;
                        string NamewithOptions = "";
                        DataTable dtoptions = null;
                        DataSet dsYOYOopt = new DataSet();
                        DataSet dsREADYopt = new DataSet();
                        dsYOYOopt = getYOYOwithOptions();
                        dsREADYopt = getREADYwithOptions();
                        DataTable dtt1 = resort(orderlist1, "entity_id", "ASC");
                        var distinctRows = (from DataRow dRow in dtt1.Rows
                                            select new { col1 = dRow["entity_id"] }).Distinct();


                        foreach (var value in distinctRows)
                        {
                            string s = value.col1.ToString();
                            DataTable products1 = null;
                            products1 = dtt1.AsEnumerable().Where(r => Convert.ToString(r["entity_id"]) == s).AsDataView().ToTable();
                            foreach (DataRow row in products1.Rows)
                            {
                                Entityid = row["entity_id"].ToString();
                                customername = row["customername"].ToString();
                                Address = row["Address"].ToString();
                                Zipcode = row["Zipcode"].ToString();
                                Amount = Convert.ToDecimal(row["grand_total"].ToString());
                                qty = Convert.ToInt32(Convert.ToDecimal(row["qty_ordered"].ToString()));
                                TestValue1 = row["Name"].ToString();
                                string optname = "";
                                if (TestValue1.Contains("YOYO"))
                                {
                                    if (dsYOYOopt.Tables.Count > 0)
                                    {
                                        if (dsYOYOopt.Tables[0].Rows.Count > 0)
                                        {
                                            try
                                            {
                                                //var result = from r in dsYOYOopt.Tables[0].AsEnumerable()
                                                //             where r["order_id"].ToString() == row["entity_id"].ToString() && r["Name"].ToString() == row["Name"].ToString()
                                                //             select r;
                                                var result = from r in dsYOYOopt.Tables[0].AsEnumerable()
                                                             where r["order_id"].ToString() == row["entity_id"].ToString()
                                                             select r;
                                                dtoptions = result.CopyToDataTable();
                                                foreach (DataRow yoyooptrow in dtoptions.Rows)
                                                {
                                                    if (optname != "")
                                                    {
                                                        optname = optname + "," + yoyooptrow["Name"].ToString();
                                                    }
                                                    else
                                                    {
                                                        optname = yoyooptrow["Name"].ToString();
                                                    }
                                                }
                                            }
                                            catch
                                            {
                                            }
                                            if (optname != "")
                                            {
                                                optname = "(" + optname + ")";
                                            }
                                            else
                                            {
                                                optname = "";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (dsREADYopt.Tables.Count > 0)
                                    {
                                        if (dsREADYopt.Tables[0].Rows.Count > 0)
                                        {
                                            try
                                            {
                                                var result = from r in dsREADYopt.Tables[0].AsEnumerable()
                                                             where r["order_id"].ToString() == row["entity_id"].ToString() && r["Packname"].ToString() == row["Name"].ToString()
                                                             select r;
                                                dtoptions = result.CopyToDataTable();

                                                foreach (DataRow readyoptrow in dtoptions.Rows)
                                                {
                                                    if (optname != "")
                                                    {
                                                        optname = optname + "," + readyoptrow["Name"].ToString();
                                                    }
                                                    else
                                                    {
                                                        optname = readyoptrow["Name"].ToString();
                                                    }
                                                }
                                            }
                                            catch
                                            {
                                            }
                                            if (optname != "")
                                            {
                                                optname = "(" + optname + ")";
                                            }
                                            else
                                            {
                                                optname = "";
                                            }
                                        }
                                    }
                                }
                                if (TestPack == "")
                                {
                                    //TestPack = row["Name"].ToString() + optname;
                                    if (qty > 1)
                                    {
                                        TestPack = row["Name"].ToString() + "[" + qty + "]" + optname;
                                    }
                                    else
                                    {
                                        TestPack = row["Name"].ToString() + optname;
                                    }
                                }
                                else
                                {
                                    //TestPack = TestPack + "," + row["Name"].ToString() + optname;
                                    if (qty > 1)
                                    {
                                        TestPack = TestPack + "," + row["Name"].ToString() + "[" + qty + "]" + optname;
                                    }
                                    else
                                    {
                                        TestPack = TestPack + "," + row["Name"].ToString() + optname;
                                    }
                                }
                            }
                            DataRow _TestPackList = testdt.NewRow();
                            _TestPackList["entity_id"] = Entityid.ToString();
                            _TestPackList["customername"] = customername.ToString();//dtt1.Rows[i]["customername"].ToString();
                            _TestPackList["Address"] = Address.ToString();//dtt1.Rows[i]["Address"].ToString();
                            _TestPackList["Name"] = TestPack.ToString();
                            _TestPackList["Zipcode"] = Zipcode.ToString();
                            _TestPackList["Amount"] = Amount.ToString("#,##0.00");//String.Format("{ 0:C}", Amount.ToString()); //(Amount.ToString());
                            testdt.Rows.Add(_TestPackList);
                            TestPack = "";

                            TotalAmount = TotalAmount + Amount;
                        }


                        if (orderlist.Tables[0].Rows.Count > 0)
                        {
                            btnPDF.Visible = true;
                            btnXLS.Visible = true;
                            btnsendexcel.Visible = true;
                        }
                        else
                        {
                            btnPDF.Visible = false;
                            btnXLS.Visible = false;
                            btnsendexcel.Visible = false;
                        }

                        //GVOrderDetails2.DataSource = orderlist;
                        //GVOrderDetails2.DataBind();
                        GVOrderDetails2.DataSource = resort(testdt, "entity_id", "ASC");
                        GVOrderDetails2.DataBind();
                        GVOrderDetails2.FooterRow.Cells[5].Text = "Total";
                        GVOrderDetails2.FooterRow.Cells[6].Text = TotalAmount.ToString("#,##0.00");

                        GVMail.DataSource = resort(testdt, "entity_id", "ASC");
                        GVMail.DataBind();

                        //sshobj.SshDisconnect(client);
                    }
                }
                else
                {
                    GVOrderDetails2.DataSource = null;
                    GVMail.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
        }
        public void getOrderDetailsbyorderid1()
        {
            DataTable testdt = new DataTable("testPackList");
            testdt.Clear();
            testdt.Columns.Add("entity_id");
            testdt.Columns.Add("customername");
            testdt.Columns.Add("Address");
            testdt.Columns.Add("Name");
            testdt.Columns.Add("Zipcode");
            try
            {
                DataTable dt = new DataTable("PackList");
                dt.Clear();
                dt.Columns.Add("entity_id");
                dt.Columns.Add("customername");
                dt.Columns.Add("Address");
                dt.Columns.Add("Name");
                dt.Columns.Add("Zipcode");

                //queryString = "SELECT entity_id, customer_firstname, customer_lastname FROM `sales_flat_order` where created_at between '" + Convert.ToDateTime(TbxFromDate.Text.ToString()).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(TbxToDate.Text.ToString()).ToString("yyyy-MM-dd") + "'";
                //a.customer_id = c.customer_id AND 
                if (txtidlist.Text != "")
                {
                    queryString = @"SELECT  DISTINCT z.increment_id as entity_id, a.customer_id, CONCAT( IFNULL(c.firstname,''), ' ', IFNULL(c.lastname,'') ) AS customername, CONCAT( IFNULL( c.street, '' ) , ', ', IFNULL( c.city, '' ) , ', ', IFNULL( c.region, '' ) , ', ', IFNULL( c.postcode, '' ) , ', India, T:', IFNULL( c.telephone, '' ) ) AS Address, b.Name,CONCAT(IFNULL( d.area, '' ), ' ',IFNULL( c.postcode, '' ))as Zipcode
FROM `sales_flat_order_grid` as z inner join `sales_flat_order` AS a 
ON z.entity_id=a.entity_id
LEFT OUTER JOIN `sales_flat_order_item` AS b ON a.entity_id = b.order_id
LEFT OUTER JOIN `sales_flat_order_address` AS c ON c.parent_id = b.order_id
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
                        MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                        DataSet orderlist = new DataSet();
                        adapteradminmail.Fill(orderlist, "sales_flat_order_item");

                        DataTable orderlist1 = new DataTable();
                        orderlist1 = orderlist.Tables[0];
                        string Entityid = "";
                        string TestValue1 = "";
                        string customername = "";
                        string Address = "";
                        string TestPack = "";
                        string Zipcode = "";
                        string NamewithOptions = "";
                        DataTable dtoptions = null;
                        DataSet dsYOYOopt = new DataSet();
                        DataSet dsREADYopt = new DataSet();
                        dsYOYOopt = getYOYOwithOptions();
                        dsREADYopt = getREADYwithOptions();
                        DataTable dtt1 = resort(orderlist1, "entity_id", "ASC");

                        if (dtt1.Rows.Count > 0)
                        {
                            int singlewithyoyo = 0;
                            for (int i = 0; i < dtt1.Rows.Count; i++)
                            {
                                
                                Entityid = dtt1.Rows[i]["entity_id"].ToString();
                                
                                if (Entityid.Equals(TestValue1.ToString()))
                                {
                                    dtoptions = null;
                                    singlewithyoyo = 2;
                                    string jumbo = dtt1.Rows[i]["Name"].ToString();
                                    
                                    //if (TestPack.Contains("YOYO"))
                                    //{
                                    //    TestPack = TestPack + getYOYOwithOptions(TestPack, TestValue1.ToString());
                                    //}
                                    //{
                                    //    TestPack = TestPack + getREADYwithOptions(TestPack, TestValue1.ToString());
                                    //}

                                    if (dtt1.Rows[i]["Name"].ToString().Contains("YOYO"))
                                    {
                                        string optname = "";
                                        if (dsYOYOopt.Tables.Count > 0)
                                        {
                                            if (dsYOYOopt.Tables[0].Rows.Count > 0)
                                            {
                                                var result = from r in dsYOYOopt.Tables[0].AsEnumerable()
                                                             where r["order_id"].ToString() == TestValue1.ToString() || r["Name"].ToString() == TestPack.ToString()
                                                             select r;
                                                dtoptions = result.CopyToDataTable();
                                                foreach (DataRow row in dtoptions.Rows)
                                                {
                                                    if (optname != "")
                                                    {
                                                        optname = optname + "," + row["Name"].ToString();
                                                    }
                                                    else
                                                    {
                                                        optname = row["Name"].ToString();
                                                    }
                                                }
                                                if (optname != "")
                                                {
                                                    optname = "(" + optname + ")";
                                                }
                                                else
                                                {
                                                    optname = "";
                                                }
                                            }
                                        }
                                        
                                        
                                        //NamewithOptions = getYOYOwithOptions(dtt1.Rows[i]["Name"].ToString(), dtt1.Rows[i]["entity_id"].ToString());
                                        if (TestPack == "")
                                        {

                                            TestPack = optname;//getYOYOwithOptions(TestPack, TestValue1.ToString());

                                        }
                                        else
                                        {
                                            TestPack = TestPack + ", " + dtt1.Rows[i]["Name"].ToString() + optname;//getYOYOwithOptions(TestPack, TestValue1.ToString());

                                        }
                                    }
                                    else
                                    {
                                        string optname = "";
                                        if (dsREADYopt.Tables.Count > 0)
                                        {
                                            if (dsREADYopt.Tables[0].Rows.Count > 0)
                                            {
                                                var result = from r in dsREADYopt.Tables[0].AsEnumerable()
                                                             where r["order_id"].ToString() == TestValue1.ToString() || r["Packname"].ToString() == TestPack.ToString()
                                                             select r;
                                                dtoptions = result.CopyToDataTable();

                                                foreach (DataRow row in dtoptions.Rows)
                                                {
                                                    if (optname != "")
                                                    {
                                                        optname = optname + "," + row["Name"].ToString();
                                                    }
                                                    else
                                                    {
                                                        optname = row["Name"].ToString();
                                                    }
                                                }
                                                if (optname != "")
                                                {
                                                    optname = "(" + optname + ")";
                                                }
                                                else
                                                {
                                                    optname = "";
                                                }
                                            }
                                        }
                                        
                                        if (TestPack == "")
                                        {

                                            //TestPack = dtt1.Rows[i]["Name"].ToString();
                                            TestPack = optname;//getREADYwithOptions(TestPack, TestValue1.ToString());
                                        }
                                        else
                                        {
                                            //TestPack = TestPack + ", " + dtt1.Rows[i]["Name"].ToString();
                                            TestPack = TestPack + ", " + dtt1.Rows[i]["Name"].ToString() + optname;//getREADYwithOptions(TestPack, TestValue1.ToString());
                                        }
                                    }

                                }
                                else
                                {
                                    if (i == 0)
                                    {
                                        dtoptions = null;
                                        singlewithyoyo = 1;
                                        if (dtt1.Rows[i]["Name"].ToString().Contains("YOYO"))
                                        {
                                            string optname = "";
                                            //dtoptions = dsYOYOopt.Tables[0].AsEnumerable().Where(r => Convert.ToString(r["order_id"]) == dtt1.Rows[i]["entity_id"].ToString()).AsDataView().ToTable();
                                            if (dsYOYOopt.Tables.Count > 0)
                                            {
                                                if (dsYOYOopt.Tables[0].Rows.Count > 0)
                                                {
                                                    var result = from r in dsYOYOopt.Tables[0].AsEnumerable()
                                                                 where r["order_id"].ToString() == dtt1.Rows[i]["entity_id"].ToString() || r["Name"].ToString() == dtt1.Rows[i]["Name"].ToString()
                                                                 select r;
                                                    dtoptions = result.CopyToDataTable();
                                                    foreach (DataRow row in dtoptions.Rows)
                                                    {
                                                        if (optname != "")
                                                        {
                                                            optname = optname + "," + row["Name"].ToString();
                                                        }
                                                        else
                                                        {
                                                            optname = row["Name"].ToString();
                                                        }
                                                    }
                                                    if (optname != "")
                                                    {
                                                        optname = "(" + optname + ")";
                                                    }
                                                    else
                                                    {
                                                        optname = "";
                                                    }
                                                }
                                            }


                                            //NamewithOptions = getYOYOwithOptions(dtt1.Rows[i]["Name"].ToString(), dtt1.Rows[i]["entity_id"].ToString());
                                            if (TestPack == "")
                                            {
                                                TestPack = dtt1.Rows[i]["Name"].ToString() + optname;//getYOYOwithOptions(dtt1.Rows[i]["Name"].ToString(), dtt1.Rows[i]["entity_id"].ToString());
                                            }
                                            else
                                            {
                                                TestPack = TestPack + dtt1.Rows[i]["Name"].ToString() + optname;//getYOYOwithOptions(dtt1.Rows[i]["Name"].ToString(), dtt1.Rows[i]["entity_id"].ToString());
                                            }
                                        }
                                        else
                                        {
                                            string optname = "";
                                            //dtoptions = dsREADYopt.Tables[0].AsEnumerable().Where(r => Convert.ToString(r["order_id"]) == dtt1.Rows[i]["entity_id"].ToString()).AsDataView().ToTable();
                                            if (dsREADYopt.Tables.Count > 0)
                                            {
                                                if (dsREADYopt.Tables[0].Rows.Count > 0)
                                                {
                                                    var result = from r in dsREADYopt.Tables[0].AsEnumerable()
                                                                 where r["order_id"].ToString() == dtt1.Rows[i]["entity_id"].ToString() || r["Packname"].ToString() == dtt1.Rows[i]["Name"].ToString()
                                                                 select r;
                                                    dtoptions = result.CopyToDataTable();
                                                    foreach (DataRow row in dtoptions.Rows)
                                                    {
                                                        if (optname != "")
                                                        {
                                                            optname = optname + "," + row["Name"].ToString();
                                                        }
                                                        else
                                                        {
                                                            optname = row["Name"].ToString();
                                                        }
                                                    }
                                                    if (optname != "")
                                                    {
                                                        optname = "(" + optname + ")";
                                                    }
                                                    else
                                                    {
                                                        optname = "";
                                                    }
                                                }
                                            }

                                            if (TestPack == "")
                                            {
                                                TestPack = dtt1.Rows[i]["Name"].ToString() + optname;//;getREADYwithOptions(dtt1.Rows[i]["Name"].ToString(), dtt1.Rows[i]["entity_id"].ToString());
                                            }
                                            else
                                            {
                                                TestPack = TestPack + dtt1.Rows[i]["Name"].ToString() + optname;//getREADYwithOptions(dtt1.Rows[i]["Name"].ToString(), dtt1.Rows[i]["entity_id"].ToString());
                                            }
                                        }
                                    }
                                    //else if (singlewithyoyo != 1 && singlewithyoyo != 2)
                                    //{

                                    //    dtoptions = null;
                                    //    if (TestPack.ToString().Contains("YOYO"))
                                    //    {
                                    //        string optname = "";
                                    //        if (dsYOYOopt.Tables.Count > 0)
                                    //        {
                                    //            if (dsYOYOopt.Tables[0].Rows.Count > 0)
                                    //            {
                                    //                var result = from r in dsYOYOopt.Tables[0].AsEnumerable()
                                    //                             where r["order_id"].ToString() == TestValue1.ToString() || r["Name"].ToString() == TestPack.ToString()
                                    //                             select r;
                                    //                dtoptions = result.CopyToDataTable();
                                    //                foreach (DataRow row in dtoptions.Rows)
                                    //                {
                                    //                    if (optname != "")
                                    //                    {
                                    //                        optname = optname + "," + row["Name"].ToString();
                                    //                    }
                                    //                    else
                                    //                    {
                                    //                        optname = row["Name"].ToString();
                                    //                    }
                                    //                }
                                    //                if (optname != "")
                                    //                {
                                    //                    optname = "(" + optname + ")";
                                    //                }
                                    //                else
                                    //                {
                                    //                    optname = "";
                                    //                }
                                    //            }
                                    //        }


                                    //        if (TestPack == "")
                                    //        {
                                    //            TestPack = TestPack + optname;//getYOYOwithOptions(TestPack.ToString(), TestValue1.ToString());
                                    //        }
                                    //        else
                                    //        {
                                    //            TestPack = TestPack + optname;//getYOYOwithOptions(TestPack.ToString(), TestValue1.ToString());
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        string optname = "";
                                    //        if (dsREADYopt.Tables.Count > 0)
                                    //        {
                                    //            if (dsREADYopt.Tables[0].Rows.Count > 0)
                                    //            {
                                    //                var result = from r in dsREADYopt.Tables[0].AsEnumerable()
                                    //                             where r["order_id"].ToString() == TestValue1.ToString() || r["Packname"].ToString() == TestPack.ToString()
                                    //                             select r;
                                    //                dtoptions = result.CopyToDataTable();
                                    //                foreach (DataRow row in dtoptions.Rows)
                                    //                {
                                    //                    if (optname != "")
                                    //                    {
                                    //                        optname = optname + "," + row["Name"].ToString();
                                    //                    }
                                    //                    else
                                    //                    {
                                    //                        optname = row["Name"].ToString();
                                    //                    }
                                    //                }
                                    //                if (optname != "")
                                    //                {
                                    //                    optname = "(" + optname + ")";
                                    //                }
                                    //                else
                                    //                {
                                    //                    optname = "";
                                    //                }
                                    //            }
                                    //        }

                                    //        if (TestPack == "")
                                    //        {
                                    //            TestPack = TestPack + optname;//getREADYwithOptions(TestPack.ToString(), TestValue1.ToString());
                                    //        }
                                    //        else
                                    //        {
                                    //            TestPack = TestPack + optname;//getREADYwithOptions(TestPack.ToString(), TestValue1.ToString());
                                    //        }
                                    //    }

                                    //}
                                    else if (Entityid!=TestValue1.ToString())
                                    {
                                        if (dtt1.Rows[i]["Name"].ToString().Contains("YOYO"))
                                        {
                                            string optname = "";
                                            if (dsYOYOopt.Tables.Count > 0)
                                            {
                                                if (dsYOYOopt.Tables[0].Rows.Count > 0)
                                                {
                                                    var result = from r in dsYOYOopt.Tables[0].AsEnumerable()
                                                                 where r["order_id"].ToString() == dtt1.Rows[i]["entity_id"].ToString() || r["Packname"].ToString() == dtt1.Rows[i]["Name"].ToString()
                                                                 select r;
                                                    dtoptions = result.CopyToDataTable();
                                                    foreach (DataRow row in dtoptions.Rows)
                                                    {
                                                        if (optname != "")
                                                        {
                                                            optname = optname + "," + row["Name"].ToString();
                                                        }
                                                        else
                                                        {
                                                            optname = row["Name"].ToString();
                                                        }
                                                    }
                                                    if (optname != "")
                                                    {
                                                        optname = "(" + optname + ")";
                                                    }
                                                    else
                                                    {
                                                        optname = "";
                                                    }
                                                }
                                            }


                                            //NamewithOptions = getYOYOwithOptions(dtt1.Rows[i]["Name"].ToString(), dtt1.Rows[i]["entity_id"].ToString());
                                            if (TestPack == "")
                                            {

                                                TestPack = optname;//getYOYOwithOptions(TestPack, TestValue1.ToString());

                                            }
                                            else
                                            {
                                                TestPack = TestPack + ", " + dtt1.Rows[i]["Name"].ToString() + optname;//getYOYOwithOptions(TestPack, TestValue1.ToString());

                                            }
                                        }
                                        else
                                        {
                                            string optname = "";
                                            if (dsREADYopt.Tables.Count > 0)
                                            {
                                                if (dsREADYopt.Tables[0].Rows.Count > 0)
                                                {
                                                    var result = from r in dsREADYopt.Tables[0].AsEnumerable()
                                                                 where r["order_id"].ToString() == dtt1.Rows[i]["entity_id"].ToString() || r["Packname"].ToString() == dtt1.Rows[i]["Name"].ToString()
                                                                 select r;
                                                    dtoptions = result.CopyToDataTable();

                                                    foreach (DataRow row in dtoptions.Rows)
                                                    {
                                                        if (optname != "")
                                                        {
                                                            optname = optname + "," + row["Name"].ToString();
                                                        }
                                                        else
                                                        {
                                                            optname = row["Name"].ToString();
                                                        }
                                                    }
                                                    if (optname != "")
                                                    {
                                                        optname = "(" + optname + ")";
                                                    }
                                                    else
                                                    {
                                                        optname = "";
                                                    }
                                                }
                                            }

                                            if (TestPack == "")
                                            {

                                                //TestPack = dtt1.Rows[i]["Name"].ToString();
                                                TestPack = optname;//getREADYwithOptions(TestPack, TestValue1.ToString());
                                            }
                                            else
                                            {
                                                //TestPack = TestPack + ", " + dtt1.Rows[i]["Name"].ToString();
                                                TestPack = TestPack + ", " + dtt1.Rows[i]["Name"].ToString() + optname;//getREADYwithOptions(TestPack, TestValue1.ToString());
                                            }
                                        }
                                    }
                                    DataRow _TestPackList = testdt.NewRow();

                                    if (singlewithyoyo > 0)
                                    {
                                        singlewithyoyo = 0;
                                        _TestPackList["entity_id"] = Entityid.ToString();
                                        _TestPackList["customername"] = dtt1.Rows[i]["customername"].ToString();
                                        _TestPackList["Address"] = dtt1.Rows[i]["Address"].ToString();
                                        _TestPackList["Name"] = dtt1.Rows[i]["Name"].ToString();
                                        _TestPackList["Zipcode"] = dtt1.Rows[i]["Zipcode"].ToString();
                                    }
                                    else
                                    {
                                        _TestPackList["entity_id"] = TestValue1.ToString();
                                        _TestPackList["customername"] = customername.ToString();//dtt1.Rows[i]["customername"].ToString();
                                        _TestPackList["Address"] = Address.ToString();//dtt1.Rows[i]["Address"].ToString();
                                        _TestPackList["Name"] = TestPack.ToString();
                                        _TestPackList["Zipcode"] = Zipcode.ToString();
                                    }


                                    testdt.Rows.Add(_TestPackList);

                                    TestPack = "";
                                    TestPack = dtt1.Rows[i]["Name"].ToString();
                                    
                                }
                                TestValue1 = Entityid.ToString();

                                customername = dtt1.Rows[i]["customername"].ToString();
                                Address = dtt1.Rows[i]["Address"].ToString();
                                Zipcode = dtt1.Rows[i]["Zipcode"].ToString();
                                
                            }
                            if (singlewithyoyo != 1 && singlewithyoyo != 2)
                            {
                                if (TestPack.Contains("YOYO"))
                                {
                                    string optname = "";
                                    if (dsYOYOopt.Tables.Count > 0)
                                    {
                                        if (dsYOYOopt.Tables[0].Rows.Count > 0)
                                        {
                                            var result = from r in dsYOYOopt.Tables[0].AsEnumerable()
                                                         where r["order_id"].ToString() == TestValue1.ToString() || r["Name"].ToString() == TestPack.ToString()
                                                         select r;
                                            dtoptions = result.CopyToDataTable();
                                            foreach (DataRow row in dtoptions.Rows)
                                            {
                                                if (optname != "")
                                                {
                                                    optname = optname + "," + row["Name"].ToString();
                                                }
                                                else
                                                {
                                                    optname = row["Name"].ToString();
                                                }
                                            }
                                            if (optname != "")
                                            {
                                                optname = "(" + optname + ")";
                                            }
                                            else
                                            {
                                                optname = "";
                                            }
                                        }
                                    }
                                    
                                    //NamewithOptions = getYOYOwithOptions(TestPack, TestValue1.ToString());
                                    if (TestPack == "")
                                    {
                                        TestPack = optname;//getYOYOwithOptions(TestPack, TestValue1.ToString());
                                    }
                                    else
                                    {
                                        TestPack = TestPack + optname;//getYOYOwithOptions(TestPack, TestValue1.ToString());
                                    }
                                }
                                else
                                {
                                    string optname = "";
                                    if (dsREADYopt.Tables.Count > 0)
                                    {
                                        if (dsREADYopt.Tables[0].Rows.Count > 0)
                                        {
                                            var result = from r in dsREADYopt.Tables[0].AsEnumerable()
                                                         where r["order_id"].ToString() == TestValue1.ToString() || r["Packname"].ToString() == TestPack.ToString()
                                                         select r;
                                            dtoptions = result.CopyToDataTable();
                                            foreach (DataRow row in dtoptions.Rows)
                                            {
                                                if (optname != "")
                                                {
                                                    optname = optname + "," + row["Name"].ToString();
                                                }
                                                else
                                                {
                                                    optname = row["Name"].ToString();
                                                }
                                            }
                                            if (optname != "")
                                            {
                                                optname = "(" + optname + ")";
                                            }
                                            else
                                            {
                                                optname = "";
                                            }
                                        }
                                    }
                                    
                                    if (TestPack == "")
                                    {
                                        TestPack = optname;//getREADYwithOptions(TestPack, TestValue1.ToString());
                                    }
                                    else
                                    {
                                        TestPack = TestPack + optname;//getREADYwithOptions(TestPack, TestValue1.ToString());
                                    }
                                }
                            }
                            DataRow _TestPackListfinalrecord = testdt.NewRow();
                            _TestPackListfinalrecord["entity_id"] = TestValue1.ToString();
                            _TestPackListfinalrecord["customername"] = customername.ToString();
                            _TestPackListfinalrecord["Address"] = Address.ToString();
                            _TestPackListfinalrecord["Name"] = TestPack.ToString();
                            _TestPackListfinalrecord["Zipcode"] = Zipcode.ToString();
                            testdt.Rows.Add(_TestPackListfinalrecord);
                        }

                        if (orderlist.Tables[0].Rows.Count > 0)
                        {
                            btnPDF.Visible = true;
                            btnXLS.Visible = true;
                            btnsendexcel.Visible = true;
                        }
                        else
                        {
                            btnPDF.Visible = false;
                            btnXLS.Visible = false;
                            btnsendexcel.Visible = false;
                        }

                        //GVOrderDetails2.DataSource = orderlist;
                        //GVOrderDetails2.DataBind();
                        GVOrderDetails2.DataSource = resort(testdt, "entity_id", "ASC");
                        GVOrderDetails2.DataBind();
                        GVMail.DataSource = resort(testdt, "entity_id", "ASC");
                        GVMail.DataBind();


                    }
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
        }
        public DataSet getYOYOwithOptions()
        {
            DataSet yoyowopt = new DataSet();
            try
            {
                queryString = @"SELECT DISTINCT z.increment_id as order_id,c.title, a.Name
FROM `sales_flat_order_grid` as z inner join `sales_flat_order_item` AS a
ON z.entity_id=a.order_id
INNER JOIN `catalog_product_bundle_selection` AS b ON a.product_id = b.product_id
INNER JOIN `catalog_product_bundle_option_value` AS c ON b.option_id = c.option_id
WHERE a.Name NOT LIKE '%t Want To Buy This Product/' and c.title LIKE '%(Optional)' and z.increment_id in (" + txtidlist.Text.ToString() + ")";
                //Response.Write(queryString);

                
                if (queryString != "")
                {
                    MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                    adapteradminmail.Fill(yoyowopt, "YOYOwopt");
                    
                }
            }
            catch (Exception ex)
            {
            }
            return yoyowopt;
        }
        public string getYOYOwithOptions(string Name,string orderid)
        {
            DataTable dt = new DataTable("YOYONames");
            dt.Clear();
            dt.Columns.Add("Name");
            dt.Columns.Add("Sno");
            string testname = "";
            try
            {
                queryString = @"SELECT DISTINCT c.title, a.Name
FROM `sales_flat_order_grid` as z inner join `sales_flat_order_item` AS a
ON z.entity_id=a.order_id
INNER JOIN `catalog_product_bundle_selection` AS b ON a.product_id = b.product_id
INNER JOIN `catalog_product_bundle_option_value` AS c ON b.option_id = c.option_id
WHERE a.Name NOT LIKE '%t Want To Buy This Product/' and c.title LIKE '%(Optional)' and z.increment_id in (" + orderid + ")";
                //Response.Write(queryString);

                DataSet orderlist = new DataSet();
                if (queryString != "")
                {
                    MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);


                    adapteradminmail.Fill(orderlist, "YOYOwithNames");
                    
                    foreach (DataRow row in orderlist.Tables[0].Rows)
                    {
                        DataRow _YOYONames = dt.NewRow();


                        if (row["title"].ToString().Contains("(Optional)"))
                        {
                            if (Name != "")
                            {
                                if (testname != "")
                                {
                                    testname = testname + "," + row["Name"].ToString();
                                }
                                else
                                {
                                    testname = row["Name"].ToString();
                                } 
                            }
                        }
                    }
                    if (testname != "")
                    {
                        Name = "(" + testname + ")";
                    }
                    else
                    {
                        Name = "";
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return Name;
        }
        public DataSet getREADYwithOptions()
        {
            DataSet READYwopt = new DataSet();
            try
            {

                queryString = @"SELECT distinct z.increment_id as order_id,a.Name as Packname,d.name  as Name, d.weight as TotalWeight
from `sales_flat_order_grid` as z inner join `sales_flat_order_item` AS a
ON z.entity_id=a.order_id

inner join sales_flat_quote_item_option as b on a.quote_item_id = b.item_id

inner join catalog_product_option_type_title AS c ON b.value = c.option_type_id

inner join 1_pack_item_ffhp_product as d on d.name=c.title

where a.parent_item_id is null and a.product_type='simple'

and c.title !='No Need' and d.pack_name like '%(Optional)'

and z.increment_id in (" + txtidlist.Text.ToString() + ")";
                
                if (queryString != "")
                {
                    MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                    adapteradminmail.Fill(READYwopt, "READYwopt");
                    
                }
            }
            catch (Exception ex)
            {
            }
            return READYwopt;
        }
        public string getREADYwithOptions(string Name, string orderid)
        {
            
            string testname = "";
            try
            {
//                queryString = @"SELECT distinct a.name as Name, a.weight as TotalWeight
//                    FROM `1_pack_item_ffhp_product` AS a
//                    LEFT OUTER JOIN sales_flat_order_item AS b ON a.pack_name = b.name
//                    WHERE a.name in (
//                    
//                    SELECT c.title
//                    FROM sales_flat_order_item AS a
//                    INNER JOIN sales_flat_quote_item_option AS b ON a.quote_item_id = b.item_id
//                    INNER JOIN catalog_product_option_type_title AS c ON b.value = c.option_type_id
//                    WHERE a.parent_item_id is null and a.product_type='simple' and a.quote_item_id in((select quote_item_id from sales_flat_order_item where order_id in (" + orderid + @"))) AND c.title != 'No Need' AND b.option_id NOT IN (SELECT ua.option_id FROM (SELECT y. * FROM sales_flat_order_item AS x INNER JOIN sales_flat_quote_item_option AS y ON x.quote_item_id = y.item_id WHERE x.quote_item_id in((select quote_item_id from sales_flat_order_item where order_id in (" + orderid + @") )) LIMIT 2 ) AS A INNER JOIN sales_flat_quote_item_option AS ua ON ua.option_id = A.option_id)) ";
                //Response.Write(queryString);
                queryString = @"SELECT distinct d.name  as Name, d.weight as TotalWeight
from `sales_flat_order_grid` as z inner join `sales_flat_order_item` AS a
ON z.entity_id=a.order_id

inner join sales_flat_quote_item_option as b on a.quote_item_id = b.item_id

inner join catalog_product_option_type_title AS c ON b.value = c.option_type_id

inner join 1_pack_item_ffhp_product as d on d.name=c.title

where a.parent_item_id is null and a.product_type='simple'

and c.title !='No Need' and d.pack_name like '%(Optional)'

and z.increment_id in (" + orderid.ToString() + ")";
                DataSet orderlist = new DataSet();
                if (queryString != "")
                {
                    MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);


                    adapteradminmail.Fill(orderlist, "READYwithOptions");

                    foreach (DataRow row in orderlist.Tables[0].Rows)
                    {

                        
                            if (Name != "")
                            {
                                if (testname != "")
                                {
                                    testname = testname + "," + row["Name"].ToString();
                                }
                                else
                                {
                                    testname = row["Name"].ToString();
                                }
                            }
                    }
                    if (testname != "")
                    {
                        Name = "(" + testname + ")";
                    }
                    else
                    {
                        Name = "";
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return Name;
        }
        protected void btncalculate_OnClick(object sender, EventArgs e)
        {

            try
            {
                
                if (txtidlist.Text != "")
                {
                    Session["orderid"] = txtidlist.Text.ToString();
                    ServiceOrderupdate();
                    DataTable dt = new DataTable("PackList");
                    dt.Clear();
                    dt.Columns.Add("Name");
                    dt.Columns.Add("Weight");
                    dt.Columns.Add("Pack");
                    dt.Columns.Add("TotalWeight");


                    DataTable testdt = new DataTable("testPackList");
                    testdt.Clear();
                    testdt.Columns.Add("Name");
                    testdt.Columns.Add("TotalWeight");


                    string Packcount = "";
                    string kgcount = "";

                    //queryString = "SELECT entity_id, customer_firstname, customer_lastname FROM `sales_flat_order` where created_at between '" + Convert.ToDateTime(TbxFromDate.Text.ToString()).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(TbxToDate.Text.ToString()).ToString("yyyy-MM-dd") + "'";
                    queryString = @"SELECT order_id,product_id,Name,weight
FROM `sales_flat_order_item`
WHERE sku IS NULL  and Name not like 'YOYO%' and Name not like '%Want To Buy This Product/%' 
AND order_id in (" + txtidlist.Text + ")";
                    //Response.Write(queryString);
                    if (queryString != "")
                    {
                        //ConnectSsh sshobj = new ConnectSsh();
                        //SshClient client = sshobj.SshConnect();
                        MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                        DataSet orderlist = new DataSet();
                        adapteradminmail.Fill(orderlist, "sales_flat_order_item");
                        DataTable products = orderlist.Tables[0];

                        //var distinctRows = (from DataRow dRow in products.Rows
                        //                    select new { col1 = dRow["order_id"], col2 = dRow["dataColumn2"] }).Distinct();
                        var distinctRows = (from DataRow dRow in products.Rows
                                            select new { col1 = dRow["product_id"], col2 = dRow["weight"] }).Distinct();


                        foreach (var value in distinctRows)
                        {
                            string s = value.col1.ToString();
                            int ono = Convert.ToInt32(value.col1.ToString());


                            //var ProductRows = (from DataRow dRow in products.Rows
                            //                   where (dRow.Field<string>("product_id") == "64")
                            //                   select products);
                            //var results = (from myRow in products.AsEnumerable()
                            //               where myRow.Field<int>("order_id") == 711
                            //               select new { col1 = myRow["product_id"] });

                            string queryString1 = @"SELECT order_id,product_id,Name,weight
FROM `sales_flat_order_item`
WHERE sku IS NULL
AND order_id in (" + txtidlist.Text + ")  and Name not like '%Want To Buy This Product/%'  and Name not like 'YOYO%'  and product_id=" + s;
                            MySqlDataAdapter adapteradminmail1 = new MySqlDataAdapter(queryString1, conn);

                            DataSet orderlist1 = new DataSet();
                            adapteradminmail1.Fill(orderlist1, "sales_flat_order_item");
                            DataTable products1 = orderlist1.Tables[0];

                            string prodname = "";
                            string prodweight = "0";
                            string _Ispc = "0";
                            foreach (DataRow row in products1.Rows)
                            {
                                prodweight = row["weight"].ToString();
                                prodname = row["Name"].ToString();
                                string pronam = row["Name"].ToString();
                                if (pronam.Contains("("))
                                {
                                    pronam = pronam.Substring(pronam.IndexOf("("), pronam.Length - pronam.IndexOf("("));
                                    pronam = pronam.Replace("(", "");
                                    pronam = pronam.Replace(")", "").Trim();
                                }
                                else if (pronam.Contains("["))
                                {
                                    pronam = pronam.Substring(pronam.IndexOf("["), pronam.Length - pronam.IndexOf("["));
                                    pronam = pronam.Replace("[", "");
                                    pronam = pronam.Replace("]", "").Trim();
                                }
                                else
                                {
                                    pronam = pronam.Substring(pronam.IndexOf("R"), pronam.Length - pronam.IndexOf("R"));
                                }
                                pronam = pronam.ToLower();
                                if (pronam.Contains("valathandu"))
                                {
                                    pronam = pronam.Replace("valathandu", "");
                                    _Ispc = "1";
                                }
                                if (pronam.Contains("kgs"))
                                {
                                    pronam = pronam.Replace("kgs", "");
                                }
                                else if (pronam.Contains("kg"))
                                {
                                    pronam = pronam.Replace("kg", "");
                                }
                                else if (pronam.Contains("gms"))
                                {
                                    pronam = pronam.Replace("gms", "");
                                }
                                else if (pronam.Contains("g"))
                                {
                                    pronam = pronam.Replace("g", "");
                                }
                                else if (pronam.Contains("pcs"))
                                {
                                    pronam = pronam.Replace("pcs", "");
                                    _Ispc = "1";
                                }
                                else if (pronam.Contains("pc"))
                                {
                                    pronam = pronam.Replace("pc", "");
                                    _Ispc = "1";
                                }
                                else if (pronam.Contains("bundle"))
                                {
                                    pronam = pronam.Replace("bundle", "");
                                    _Ispc = "1";
                                }
                                pronam = pronam.Trim();
                                int weight = 0;
                                try
                                {
                                    weight = Convert.ToInt32(products1.Rows.Count.ToString()) * Convert.ToInt32(pronam);
                                }
                                catch
                                {
                                }
                                if (kgcount == "")
                                {
                                    kgcount = prodname + products1.Rows.Count.ToString() + "X" + pronam + " = " + weight;
                                }
                                else
                                {
                                    kgcount = kgcount + "</br>" + prodname + products1.Rows.Count.ToString() + "X" + pronam + " = " + weight;
                                }
                                break;
                            }
                            DataRow _PackList = dt.NewRow();
                            _PackList["Name"] = prodname.ToString();
                            if (_Ispc == "1")
                            {
                                _PackList["Weight"] = (Convert.ToInt32(Convert.ToDecimal(prodweight.ToString()))).ToString();
                            }
                            else
                            {
                                _PackList["Weight"] = (Convert.ToDecimal(prodweight.ToString())).ToString();
                            }
                            _PackList["Pack"] = products1.Rows.Count.ToString();
                            if (_Ispc == "1")
                            {
                                _PackList["TotalWeight"] = (Convert.ToInt32(Convert.ToDecimal(prodweight.ToString()) * Convert.ToInt32(products1.Rows.Count.ToString()))).ToString();
                            }
                            else
                            {
                                _PackList["TotalWeight"] = (Convert.ToDecimal(prodweight.ToString()) * Convert.ToInt32(products1.Rows.Count.ToString())).ToString();
                            }
                            dt.Rows.Add(_PackList);
                            if (Packcount == "")
                            {
                                Packcount = prodname + products1.Rows.Count.ToString();
                            }
                            else
                            {
                                Packcount = Packcount + "</br>" + prodname + products1.Rows.Count.ToString();
                            }
                            //DataRow _PackList = dt.NewRow();
                            //_PackList["Name"] = prodname.ToString();
                            //if (_Ispc == "1")
                            //{
                            //    _PackList["Weight"] = (Convert.ToInt32(Convert.ToDecimal(prodweight.ToString()))).ToString();
                            //}
                            //else
                            //{
                            //    //_PackList["Weight"] = (Convert.ToInt32(Convert.ToDecimal(prodweight.ToString()) * 1000)).ToString();
                            //    _PackList["Weight"] = (Convert.ToInt32(Convert.ToDecimal(prodweight.ToString()))).ToString();
                            //}
                            //_PackList["Pack"] = products1.Rows.Count.ToString();
                            //if (_Ispc == "1")
                            //{
                            //    _PackList["TotalWeight"] = (Convert.ToInt32(Convert.ToDecimal(prodweight.ToString()) * Convert.ToInt32(products1.Rows.Count.ToString()))).ToString();
                            //}
                            //else
                            //{
                            //    //_PackList["TotalWeight"] = (Convert.ToInt32(Convert.ToDecimal(prodweight.ToString()) * 1000 * Convert.ToInt32(products1.Rows.Count.ToString()))).ToString();
                            //    _PackList["TotalWeight"] = (Convert.ToInt32(Convert.ToDecimal(prodweight.ToString()) * Convert.ToInt32(products1.Rows.Count.ToString()))).ToString();
                            //}
                            //dt.Rows.Add(_PackList);
                            //if (Packcount == "")
                            //{
                            //    Packcount = prodname + products1.Rows.Count.ToString();
                            //}
                            //else
                            //{
                            //    Packcount = Packcount + "</br>" + prodname + products1.Rows.Count.ToString();
                            //}

                        }
                        //lblpackcount.Text = Packcount;
                        //lblkgcount.Text = kgcount;
                        GvPackList.DataSource = resort(dt, "Name", "ASC");
                        GvPackList.DataBind();

                        string TestName = "";
                        string TestValue1 = "";
                        decimal Testweight = 0;

                        DataTable dtt1 = resort(dt, "Name", "ASC");
                        for (int i = 0; i < dtt1.Rows.Count; i++)
                        {

                            TestName = dtt1.Rows[i]["Name"].ToString();
                            if (TestName.Contains("("))
                            {
                                TestName = TestName.Substring(0, TestName.IndexOf("("));
                            }
                            else if (TestName.Contains("["))
                            {
                                TestName = TestName.Substring(0, TestName.IndexOf("["));
                            }

                            if (i == 0)
                            {
                                if (Testweight == 0)
                                {

                                    Testweight = Convert.ToDecimal(dtt1.Rows[i]["TotalWeight"].ToString());

                                }
                                else
                                {
                                    Testweight = Testweight + Convert.ToDecimal(dtt1.Rows[i]["TotalWeight"].ToString());

                                }
                            }
                            else if (TestName.Contains(TestValue1.ToString()))
                            {
                                if (Testweight == 0)
                                {

                                    Testweight = Convert.ToDecimal(dtt1.Rows[i]["TotalWeight"].ToString());

                                }
                                else
                                {
                                    Testweight = Testweight + Convert.ToDecimal(dtt1.Rows[i]["TotalWeight"].ToString());

                                }
                            }
                            else if (i > 0)
                            {

                                DataRow _TestPackList = testdt.NewRow();
                                _TestPackList["Name"] = TestValue1.ToString();
                                _TestPackList["TotalWeight"] = Testweight.ToString();
                                testdt.Rows.Add(_TestPackList);

                                Testweight = 0;
                                Testweight = Convert.ToDecimal(dtt1.Rows[i]["TotalWeight"].ToString());
                            }
                            TestValue1 = TestName.ToString();
                        }
                        GvPackList2.DataSource = resort(testdt, "Name", "ASC");
                        GvPackList2.DataBind();

                        //DataTable dtt1=resort(dt, "Name", "ASC");
                        //DataTable dtt2=resort(dt, "Name", "ASC");
                        //string TestName = "";
                        //string TestValue1 = "";
                        //string TestValue2 = "";
                        //decimal Testweight = 0;


                        //    //foreach (DataRow row1 in dtt1.Rows)

                        //for (int i = 0; i < dtt1.Rows.Count; i++)
                        //    {

                        //        //TestName = row1["Name"].ToString();
                        //        //TestValue1 = row1["Name"].ToString();
                        //        TestName = dtt1.Rows[i]["Name"].ToString();
                        //        TestValue1 = dtt1.Rows[i]["Name"].ToString();

                        //        if (TestValue1.Contains("("))
                        //        {
                        //            TestValue1 = TestValue1.Substring(0, TestValue1.IndexOf("("));
                        //        }
                        //        else if (TestValue1.Contains("["))
                        //        {
                        //            TestValue1 = TestValue1.Substring(0, TestValue1.IndexOf("["));
                        //        }
                        //        Testweight = 0;
                        //        //foreach (DataRow row2 in dtt2.Rows)
                        //        int jplus=i;
                        //        int cou = 0;
                        //        for (int j = jplus; j < dtt2.Rows.Count; j++)
                        //        {
                        //            //TestValue2 = row2["Name"].ToString();
                        //            TestValue2 = dtt2.Rows[j]["Name"].ToString();

                        //            if (TestValue2.Contains(TestValue1))
                        //            {
                        //                if (Testweight == 0)
                        //                {
                        //                    cou = 1;
                        //                    Testweight = Convert.ToDecimal(dtt2.Rows[j]["TotalWeight"].ToString());
                        //                    //jplus=j+1;
                        //                }
                        //                else
                        //                {
                        //                    Testweight = Testweight + Convert.ToDecimal(dtt2.Rows[j]["TotalWeight"].ToString());
                        //                    jplus = j + 1;
                        //                }
                        //            }
                        //            else
                        //            {
                        //                if (cou == 1)
                        //                {
                        //                    break;
                        //                }
                        //            }

                        //        }
                        //        i = jplus-1;
                        //        DataRow _TestPackList = testdt.NewRow();
                        //        _TestPackList["Name"] = TestValue1.ToString();
                        //        _TestPackList["TotalWeight"] = Testweight.ToString();
                        //        testdt.Rows.Add(_TestPackList);
                        //    }

                        //GvPackList2.DataSource = resort(testdt, "Name", "ASC");
                        //GvPackList2.DataBind();

                        //IEnumerable<DataRow> query =
                        //    from product in products.AsEnumerable()
                        //    select product;
                        //foreach (DataRow p in query)
                        //{
                        //    Console.WriteLine(p.Field<string>("Name"));
                        //}

                        //foreach (DataRow row in orderlist.Tables[0].Rows)
                        //{
                        //    String ProductName = row["Name"].ToString();
                        //}
                        //sshobj.SshDisconnect(client);
                    }
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
        }
        public static DataTable resort(DataTable dt, string colName, string direction)
        {
            dt.DefaultView.Sort = colName + " " + direction;
            dt = dt.DefaultView.ToTable();
            return dt;
        }
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
                    GVOrderDetails2.DataSource = null;
                    GVOrderDetails2.DataBind();
                    GVMail.DataSource = null;
                    GVMail.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg;//ex.ToString();
            }
        }
        protected void GridView1_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
                if (e.Row.Cells[0].Text == "Dew Pack")
                {
                    if (DewCount == 0)
                    {
                        DewCount = 1;
                    }
                    else
                    {
                        DewCount = DewCount + 1;
                    }
                    

                }
                else if (e.Row.Cells[0].Text == "Shine Pack")
                {
                    if (ShineCount == 0)
                    {
                        ShineCount = 1;
                    }
                    else
                    {
                        ShineCount = ShineCount + 1;
                    }


                }
                else if (e.Row.Cells[0].Text == "Grand Pack")
                {
                    if (GrandCount == 0)
                    {
                        GrandCount = 1;
                    }
                    else
                    {
                        GrandCount = GrandCount + 1;
                    }


                }
                else if (e.Row.Cells[0].Text == "Jain Pack")
                {
                    if (JainCount == 0)
                    {
                        JainCount = 1;
                    }
                    else
                    {
                        JainCount = JainCount + 1;
                    }


                }
                else if (e.Row.Cells[0].Text == "YOUR PACK v1")
                {
                    if (YOURPACKv1Count == 0)
                    {
                        YOURPACKv1Count = 1;
                    }
                    else
                    {
                        YOURPACKv1Count = YOURPACKv1Count + 1;
                    }


                }
                
                //Response.Write("Dew Pack Count is "+DewCount);
                //e.Row.Cells[1].Text = "<i>" + e.Row.Cells[1].Text + "</i>";

            }
            lblcount.Text = "Dew Pack Count is " + DewCount + "</br> Shine Pack Count is " + ShineCount + "</br> Jain Pack Count is " + JainCount + "</br> Grand Pack Count is " + GrandCount + "</br> Your Pack V1 Count is " + YOURPACKv1Count;
        }
        protected void btnPDF_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //Response.ContentType = "application/pdf";
                //Response.AddHeader("content-disposition", "attachment;filename=CustomerInfo.pdf");
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //StringWriter sw = new StringWriter();
                //HtmlTextWriter hw = new HtmlTextWriter(sw);
                //System.Web.UI.HtmlControls.HtmlForm f = new System.Web.UI.HtmlControls.HtmlForm();
                ////Panel Tom = new Panel();
                ////Tom.ID = base.UniqueID;
                ////Tom.Controls.Add(myControl);
                ////Page.FindControl("WebForm1").Controls.Add(Tom);

                //GVOrderDetails2.AllowPaging = false;
                //f.Controls.Add(GVOrderDetails2);
                ////GVOrderDetails2.DataBind();
                //GVOrderDetails2.RenderControl(hw);
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
                
                
            //    //here starat second code
            //    GVOrderDetails2.AllowPaging = false;

            //    //GridView1.DataBind();



            //    //Create a table

            //    iTextSharp.text.Table table = new iTextSharp.text

            //                 .Table(GVOrderDetails2.Columns.Count);

            //    table.Cellpadding = 5;
                
            //    //GridViewRow row = (GridViewRow)(((Control)sender).NamingContainer);
                
            //    //Set the column widths

            //    int[] widths = new int[GVOrderDetails2.Columns.Count];

            //    for (int x = 0; x < GVOrderDetails2.Columns.Count; x++)
            //    {

            //        widths[x] = (int)GVOrderDetails2.Columns[x].ItemStyle.Width.Value;

            //        string cellText = Server.HtmlDecode(GVOrderDetails2.HeaderRow.Cells[x].Text);

            //        iTextSharp.text.Cell cell = new iTextSharp.text.Cell(cellText);

            //        //cell.BackgroundColor = new Color(System

            //        //                   .Drawing.ColorTranslator.FromHtml("#008000"));

            //        table.AddCell(cell);

            //    }

            //    table.SetWidths(widths);
                


            //    //Transfer rows from GridView to table

            //    for (int i = 0; i < GVOrderDetails2.Rows.Count; i++)
            //    {

            //        if (GVOrderDetails2.Rows[i].RowType == DataControlRowType.DataRow)
            //        {

            //            for (int j = 0; j < GVOrderDetails2.Columns.Count; j++)
            //            {
            //                string cellText="";
            //                if (j != 0)
            //                {
            //                    cellText = Server.HtmlDecode

            //                                      (GVOrderDetails2.Rows[i].Cells[j].Text);
            //                }
            //                else
            //                {
            //                    cellText = Server.HtmlDecode((i+1).ToString());
            //                    Response.Write(cellText);
            //                }
            //                iTextSharp.text.Cell cell = new iTextSharp.text.Cell(cellText);

                            

            //                //Set Color of Alternating row

            //                //if (i % 2 != 0)
            //                //{

            //                //    cell.BackgroundColor = new Color(System.Drawing

            //                //                        .ColorTranslator.FromHtml("#C2D69B"));

            //                //}

            //                table.AddCell(cell);

            //            }

            //        }

            //    }



            //    //Create the PDF Document

            //    Document pdfDoc = new Document(PageSize.A4, 5f, 5f, 5f, 0f);
                
            //    PdfWriter.GetInstance(pdfDoc, Response.OutputStream);

            //    pdfDoc.Open();

            //    pdfDoc.Add(table);

            //    pdfDoc.Close();

            //    Response.ContentType = "application/pdf";

            //    Response.AddHeader("content-disposition", "attachment;" +

            //                                   "filename=CustomerInfo.pdf");

            //    Response.Cache.SetCacheability(HttpCacheability.NoCache);

            //    Response.Write(pdfDoc);

            //    Response.End(); 


                //here staratd third code

                GVOrderDetails2.AllowPaging = false;
                //GridView1.DataBind();
                string s = Server.MapPath("Images/Calibri.ttf");
                BaseFont bf = BaseFont.CreateFont(s, BaseFont.IDENTITY_H, true);
                //BaseFont bf = BaseFont.CreateFont(Environment.GetEnvironmentVariable("windir") + @"\fonts\CALIBRI.TTF", BaseFont.IDENTITY_H, true);
                
                iTextSharp.text.pdf.PdfPTable table = new iTextSharp.text.pdf.PdfPTable(GVOrderDetails2.Columns.Count);
                int[] widths = new int[GVOrderDetails2.Columns.Count];
                for (int x = 0; x < GVOrderDetails2.Columns.Count; x++)
                {
                    widths[x] = (int)GVOrderDetails2.Columns[x].ItemStyle.Width.Value;
                    string cellText = Server.HtmlDecode(GVOrderDetails2.HeaderRow.Cells[x].Text);

                    //Set Font and Font Color
                    iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 11, iTextSharp.text.Font.NORMAL);
                    //font.Color = new Color(GVOrderDetails2.HeaderStyle.ForeColor);
                    //font.Color = new Color(GVOrderDetails2.RowStyle.ForeColor);
                    iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(new Phrase(12, cellText, font));
                    if (x == GVOrderDetails2.Columns.Count - 1)
                    {
                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    }
                    //Set Header Row BackGround Color
                    //cell.BackgroundColor = new Color(GVOrderDetails2.HeaderStyle.BackColor);


                    table.AddCell(cell);
                }
                table.SetWidths(widths);

                for (int i = 0; i < GVOrderDetails2.Rows.Count; i++)
                {
                    if (GVOrderDetails2.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        for (int j = 0; j < GVOrderDetails2.Columns.Count; j++)
                        {
                            string cellText = "";
                            if (j != 0)
                            {
                                cellText = Server.HtmlDecode(GVOrderDetails2.Rows[i].Cells[j].Text);
                            }
                            else
                            {
                                cellText = Server.HtmlDecode((i + 1).ToString());
                            }
                            //Set Font and Font Color
                            iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 11, iTextSharp.text.Font.NORMAL);
                            //font.Color = new Color(GVOrderDetails2.RowStyle.ForeColor);
                            iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(new Phrase(12, cellText, font));
                            if (j == GVOrderDetails2.Columns.Count - 1)
                            {
                                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
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

                for (int j = 0; j < GVOrderDetails2.Columns.Count; j++)
                {
                    string cellText = "";
                    if (j == GVOrderDetails2.Columns.Count-2)
                    {
                        cellText = GVOrderDetails2.FooterRow.Cells[5].Text.ToString();
                    }
                    else if (j == GVOrderDetails2.Columns.Count-1)
                    {
                        cellText = GVOrderDetails2.FooterRow.Cells[6].Text.ToString();
                    }
                    else
                    {
                        cellText = "";
                    }
                    iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 11, iTextSharp.text.Font.NORMAL);
                    iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(new Phrase(12, cellText, font));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell);
                }
                

                

                //Create the PDF Document
                Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 0f);
                //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                PdfWriter pdfwriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfwriter.PageEvent = new Footer(Server.MapPath("Images/Calibri.ttf"), 80);
                pdfDoc.Open();
                pdfDoc.Add(table);
                pdfDoc.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=CustomerInfo.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
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
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "CustomersInfo.xls"));
                Response.ContentType = "application/ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                GVOrderDetails2.AllowPaging = false;
                
                //Change the Header Row back to white color
                GVOrderDetails2.HeaderRow.Style.Add("background-color", "#FFFFFF");
                GVOrderDetails2.HeaderRow.Style.Add("fore-color", "#000000");
                //Applying stlye to gridview header cells
                for (int i = 0; i < GVOrderDetails2.HeaderRow.Cells.Count; i++)
                {
                    GVOrderDetails2.HeaderRow.Cells[i].Style.Add("background-color", "#FFFFFF");
                    GVOrderDetails2.HeaderRow.Cells[i].Style.Add("color", "#000000");
                }
                int j = 1;
                //This loop is used to apply stlye to cells based on particular row
                foreach (GridViewRow gvrow in GVOrderDetails2.Rows)
                {
                    //gvrow.BackColor = Color.WHITE.ToString;
                    //if (j <= GVOrderDetails2.Rows.Count)
                    //{
                        //if (j % 2 != 0)
                        //{
                            for (int k = 0; k < gvrow.Cells.Count; k++)
                            {
                                gvrow.Cells[k].Style.Add("background-color", "#FFFFFF");
                                gvrow.Cells[k].Style.Add("color", "#000000");//ItemStyle-ForeColor
                            }
                        //}
                    //}
                    //j++;
                }
                for (int k = 0; k < GVOrderDetails2.Columns.Count; k++)
                {
                    GVOrderDetails2.FooterRow.Cells[k].Style.Add("background-color", "#FFFFFF");
                    GVOrderDetails2.FooterRow.Cells[k].Style.Add("color", "#000000");
                }
                System.Web.UI.HtmlControls.HtmlForm f = new System.Web.UI.HtmlControls.HtmlForm();
                //GVOrderDetails2.EnableTheming = false;
                f.Controls.Add(GVOrderDetails2);
                //GVOrderDetails2.DataBind();

                GVOrderDetails2.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
                //GVOrderDetails2.EnableTheming = true;
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
                string filename = "CustomerInfo" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss");
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

                // Open an existing Excel 2007 file.

                //IWorkbook workbook = excelEngine.Excel.Workbooks.Open(Server.MapPath(filepath + "Book1.xlsx"), ExcelOpenType.Automatic);



                // Select the version to be saved.

                //workbook.Version = ExcelVersion.Excel2007;



                // Save it as "Excel 2007" format.

                //workbook.SaveAs("Sample.xlsx");
                StreamWriter writer = File.AppendText(Server.MapPath(filepath + filename + ".xlsx"));
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
                mail.Subject = "PFA - CustomerInfo(XLS)";
                mail.Body = "PFA - CustomerInfo(XLS)";

                System.Net.Mail.Attachment attachment;
                //attachment = new System.Net.Mail.Attachment(Server.MapPath("MailFiles/CustomerInformation/" + Session.SessionID +s+ ".xls"));
                attachment = new System.Net.Mail.Attachment(Server.MapPath(filepath + filename + ".xlsx"));
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

        protected void btnsendpdf_OnClick(object sender, EventArgs e)
        {
            try
            {
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=CustomerInfo.xls");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                System.Web.UI.HtmlControls.HtmlForm f = new System.Web.UI.HtmlControls.HtmlForm();
                //Panel Tom = new Panel();
                //Tom.ID = base.UniqueID;
                //Tom.Controls.Add(myControl);
                //Page.FindControl("WebForm1").Controls.Add(Tom);

                GVOrderDetails2.AllowPaging = false;
                f.Controls.Add(GVOrderDetails2);
                //GVOrderDetails2.DataBind();
                GVOrderDetails2.RenderControl(hw);
                //GVOrderDetails2.HeaderRow.Style.Add("width", "15%");
                //GVOrderDetails2.HeaderRow.Style.Add("font-size", "10px");
                //GVOrderDetails2.Style.Add("text-decoration", "none");
                //GVOrderDetails2.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                //GVOrderDetails2.Style.Add("font-size", "8px");

                StreamWriter writer = File.AppendText(Server.MapPath("MailFiles/CustomerInformation/" + Session.SessionID + ".xls"));
                //Response.WriteFile(Server.MapPath("MailFiles/CustomerInformation/" + Session.SessionID + ".xls"));
                writer.WriteLine(sw.ToString());
                writer.Close();

                //string renderedGridView = sw.ToString();
                //File.WriteAllText(Server.MapPath("MailFiles/CustomerInformation/" + Session.SessionID + ".pdf"), renderedGridView);

                //StringReader sr = new StringReader(sw.ToString());
                //Document pdfDoc = new Document(PageSize.A2, 7f, 7f, 7f, 0f);
                //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                //pdfDoc.Open();
                //htmlparser.Parse(sr);
                //pdfDoc.Close();
                ////Response.Write(pdfDoc);
                //Response.WriteFile(Server.MapPath("MailFiles/CustomerInformation/CustomerInfo.pdf"));
                //Response.End();


                //MailMessage mail = new MailMessage();
                //SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                //mail.From = new MailAddress("binarch.developer1@gmail.com");
                //mail.To.Add("binarch.developer1@gmail.com");
                //mail.Subject = "Test Mail - 1";
                //mail.Body = "mail with attachment";

                //System.Net.Mail.Attachment attachment;
                //attachment = new System.Net.Mail.Attachment(Server.MapPath("MailFiles/CustomerInformation/" + Session.SessionID + ".pdf"));
                //mail.Attachments.Add(attachment);

                //SmtpServer.Port = 587;
                //SmtpServer.Credentials = new System.Net.NetworkCredential("demo.binarch@gmail.com", "binarch.demo");
                //SmtpServer.EnableSsl = true;

                //SmtpServer.Send(mail);
                ////MessageBox.Show("mail Send");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //try
            //{
            //    MailMessage Msg = new MailMessage();
            //    // Sender e-mail address.
            //    Msg.From = "binarch.developer1@gmail.com";
            //    // Recipient e-mail address.
            //    Msg.To = "binarch.developer1@gmail.com";
            //    // Subject of e-mail
            //    Msg.Subject = "Customer Details as PDF";
            //    //if (fileUpload1.HasFile)
            //    //{
            //        // File Upload path
            //    String FileName = @"C:\Inetpub\wwwroot\Projects\FFHPphpSolution\FFHPWeb\MailFiles\CustomerInformation\CustomerInfo.pdf";//fileUpload1.PostedFile.FileName;
            //        //Getting Attachment file
            //        MailAttachment mailAttachment = new MailAttachment(FileName, MailEncoding.Base64);
            //        //Attaching uploaded file
            //        Msg.Attachments.Add(mailAttachment);
            //    //}

            //    Msg.Body = "PFA Customer Details from FFHP";
            //    // your remote SMTP server IP.
            //    SmtpMail.SmtpServer = "10.120.0.21";
            //    SmtpMail.Send(Msg);
            //    Msg = null;
            //    //Page.RegisterStartupScript("UserMsg", "<script>alert('Mail sent thank you...');if(alert){ window.location='SendMailWithAttachment.aspx';}</script>");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("{0} Exception caught.", ex);
            //}
        }
        //public void savepdf()
        //{
        //    StringWriter sw = new StringWriter();
        //    HtmlTextWriter htw = new HtmlTextWriter(sw);

        //    StringReader sr;
        //    string fileName = Server.MapPath("PATH TO PDF");

        //    var doc = new Document(PageSize.A3, 45, 5, 5, 5);
        //    var pdf = fileName;


        //    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(pdf, FileMode.Create));

        //    doc.Open();
            
        //    HtmlPipelineContext htmlContext = new HtmlPipelineContext(null);
        //    htmlContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());
        //    ICSSResolver cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(false);

        //    cssResolver.AddCssFile(Server.MapPath("PATH TO CSS"), true);
        //    IPipeline pipeline = new CssResolverPipeline(cssResolver, new HtmlPipeline(htmlContext, new PdfWriterPipeline(doc, writer)));

        //    XMLWorker worker = new XMLWorker(pipeline, true);
        //    XMLParser xmlParse = new XMLParser(true, worker);

        //    control.RenderControl(htw);
        //    sr = new StringReader(sw.ToString());
        //    xmlParse.Parse(sr);
        //    xmlParse.Flush();
        //}
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
                        txtidlist.Text = txtidlist.Text.ToString().Replace(((HiddenField)itm.FindControl("HFordernum")).Value.ToString(),"");
                    }
                    txtidlist.Text = txtidlist.Text.ToString().Replace(",,", ",");
                    if (txtidlist.Text.ToString().EndsWith(","))
                    {
                        txtidlist.Text=txtidlist.Text.ToString().Remove(txtidlist.Text.ToString().Length - 1);
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
    }

}
