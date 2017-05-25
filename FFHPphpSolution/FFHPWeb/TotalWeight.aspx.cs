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
using System.Globalization;
//using Renci.SshNet;

namespace FFHPWeb
{
    public partial class TotalWeight : System.Web.UI.Page
    {
        //string conn = "server=68.178.143.107;userid=stagingffhpin;password=Stag1ngffHP!n;database=stagingffhpin;Convert Zero Datetime=True";  //staging
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
                    calculate();
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
            //getOtherPackDetails();
        }
//        public void getOrderNumbers()
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
                    //sshobj.SshDisconnect(client);
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
        public void getOtherPackDetails()
        {
            try
            {
                //queryString = "SELECT entity_id, customer_firstname, customer_lastname FROM `sales_flat_order` where created_at between '" + Convert.ToDateTime(TbxFromDate.Text.ToString()).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(TbxToDate.Text.ToString()).ToString("yyyy-MM-dd") + "'";
                queryString = @"SELECT a.pack_name, a.name, a.weight
FROM `1_pack_item_ffhp_product` AS a
LEFT OUTER JOIN sales_flat_order_item AS b ON a.pack_name = b.name
WHERE b.order_id = '717'";
                //Response.Write(queryString);
                if (queryString != "")
                {
                    MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                    DataSet orderlist = new DataSet();
                    adapteradminmail.Fill(orderlist, "1_pack_item_ffhp_product");
                    GvPackList.DataSource = orderlist;
                    GvPackList.DataBind();

                }
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg;//ex.ToString();
            }
        }
        protected void btncalculate_OnClick(object sender, EventArgs e)
        {
            calculate();
        }
        public void calculate()
        {
            try
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                ServiceOrderupdate();
                if (txtidlist.Text != "")
                {
                    Session["orderid"] = txtidlist.Text.ToString();

                    tbl1.Visible = true;
                    DataTable testdt = new DataTable("testPackListfinal");
                    testdt.Clear();
                    testdt.Columns.Add("Name");
                    testdt.Columns.Add("TotalWeight");
                    testdt.Columns.Add("Units");

                    //DataTable smsmapping = new DataTable();
                    //smsmapping = getsmsmapping();
                    DataTable yoyopacklist = new DataTable();
                    DataTable otherpacklist = new DataTable();
                    DataTable otherpacklistoptions = new DataTable();

                    DataTable packlist = new DataTable();
                    yoyopacklist = getyoyopack();
                    otherpacklist = getotherpack();
                    //otherpacklistoptions = getotherpackoptions();
                    packlist = yoyopacklist.Copy();
                    packlist.Merge(otherpacklist);
                    packlist.Merge(getsinglewithmultipleproduct());
                    //packlist = otherpacklistoptions.Copy();
                    //packlist.Merge(otherpacklistoptions);


                    string TestName = "";
                    string TestValue1 = "";
                    decimal Testweight = 0;
                    string smsformat = "";
                    string smsformatfinal = "";
                    string Testunits = "";
                    string Testunits1 = "";
                    DataTable dtt1 = resort(packlist, "Name", "ASC");
                    if (dtt1.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtt1.Rows.Count; i++)
                        {

                            TestName = dtt1.Rows[i]["Name"].ToString();
                            //if (TestName.Contains("("))
                            //{
                            //    TestName = TestName.Substring(0, TestName.IndexOf("("));
                            //}
                            //else if (TestName.Contains("["))
                            //{
                            //    TestName = TestName.Substring(0, TestName.IndexOf("["));
                            //}
                            TestName = TestName.Trim();
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
                                if (dtt1.Rows[i]["Units"].ToString() != "")
                                {
                                    Testunits1 = dtt1.Rows[i]["Units"].ToString();
                                }
                            }
                            else if (TestName.Contains(TestValue1.ToString()))//else if (TestName==TestValue1.ToString())
                            {
                                if (Testweight == 0)
                                {

                                    Testweight = Convert.ToDecimal(dtt1.Rows[i]["TotalWeight"].ToString());

                                }
                                else
                                {
                                    Testweight = Testweight + Convert.ToDecimal(dtt1.Rows[i]["TotalWeight"].ToString());

                                }
                                if (dtt1.Rows[i]["Units"].ToString() != "")
                                {
                                    Testunits1 = dtt1.Rows[i]["Units"].ToString();
                                }
                            }
                            else if (i > 0)
                            {

                                DataRow _TestPackList = testdt.NewRow();
                                _TestPackList["Name"] = TestValue1.ToString();
                                _TestPackList["TotalWeight"] = Testweight.ToString();
                                _TestPackList["Units"] = Testunits1.ToString();
                                testdt.Rows.Add(_TestPackList);

                                if (smsformat != "")
                                {
                                    //string added="/"+getsmsmapstring(smsmapping, TestValue1.ToString()) + " " + Convert.ToDouble(Testweight.ToString());
                                    string added = "/" + TestValue1.ToString() + " " + Convert.ToDouble(Testweight.ToString());
                                    if (smsformat.Length + added.Length < 800)
                                    {
                                        smsformat = smsformat + added;
                                    }
                                    else
                                    {
                                        if (smsformatfinal != "")
                                        {
                                            smsformatfinal = smsformatfinal + System.Environment.NewLine + System.Environment.NewLine + smsformat;
                                        }
                                        else
                                        {
                                            smsformatfinal = smsformat;
                                        }
                                        smsformat = "";
                                        //smsformat = getsmsmapstring(smsmapping,TestValue1.ToString()) + " " + Convert.ToDouble(Testweight.ToString());
                                        smsformat = TestValue1.ToString() + " " + Convert.ToDouble(Testweight.ToString());
                                    }

                                }
                                else
                                {
                                    //smsformat = getsmsmapstring(smsmapping,TestValue1.ToString()) + " " + Convert.ToDouble(Testweight.ToString());
                                    smsformat = TestValue1.ToString() + " " + Convert.ToDouble(Testweight.ToString());
                                }

                                Testweight = 0;
                                Testweight = Convert.ToDecimal(dtt1.Rows[i]["TotalWeight"].ToString());
                                Testunits1 = dtt1.Rows[i]["Units"].ToString();
                            }
                            TestValue1 = TestName.ToString();
                            //Testunits1 = Testunits.ToString();
                        }
                        DataRow _TestPackListfinalrecord = testdt.NewRow();
                        _TestPackListfinalrecord["Name"] = TestValue1.ToString();
                        _TestPackListfinalrecord["TotalWeight"] = Testweight.ToString();
                        _TestPackListfinalrecord["Units"] = Testunits1.ToString();
                        testdt.Rows.Add(_TestPackListfinalrecord);
                        if (smsformat != "")
                        {
                            //string added = "/" + getsmsmapstring(smsmapping,TestValue1.ToString()) + " " + Convert.ToDouble(Testweight.ToString());
                            string added = "/" + TestValue1.ToString() + " " + Convert.ToDouble(Testweight.ToString());
                            if (smsformat.Length + added.Length < 800)
                            {
                                smsformat = smsformat + added;
                            }

                            if (smsformatfinal != "")
                            {
                                smsformatfinal = smsformatfinal + System.Environment.NewLine + System.Environment.NewLine + smsformat;
                            }
                            else
                            {
                                smsformatfinal = smsformat;
                            }
                            smsformat = "";

                        }
                        else
                        {
                            //smsformat = getsmsmapstring(smsmapping,TestValue1.ToString() )+ " " + Convert.ToDouble(Testweight.ToString());
                            smsformat = TestValue1.ToString() + " " + Convert.ToDouble(Testweight.ToString());
                            if (smsformatfinal != "")
                            {
                                smsformatfinal = smsformatfinal + System.Environment.NewLine + System.Environment.NewLine + smsformat;
                            }
                            else
                            {
                                smsformatfinal = smsformat;
                            }
                        }
                    }
                    lblsmsformat.Text = smsformatfinal.ToString();
                    //string testmain="";
                    //string tests = smsformat.ToString();
                    //string s=smsformat.ToString();
                    //int k = 0;
                    //int j = 160;
                    //for (int i = j; i < s.Length; i--)
                    //{
                    //    if (s[i].ToString() == "/")
                    //    {

                    //        if (testmain != "")
                    //        {
                    //            testmain = testmain + "#" + tests.Substring(k, i);
                    //            k = i;
                    //        }
                    //        else
                    //        {
                    //            testmain = tests.Substring(0, i);
                    //            k = i;
                    //        }
                    //        if (i + 160 < s.Length)
                    //        {
                    //            j = i + 160;
                    //        }
                    //        else
                    //        {
                    //            j = i + s.Length-i;
                    //            testmain = testmain + "#" + tests.Substring(k,s.Length);
                    //        }

                    //    }
                    //}
                    //lblsmsformat.Text = testmain.ToString();


                    //GvPackList.DataSource = resort(yoyopacklist, "Name", "ASC");
                    //GvPackList.DataBind();
                    //GvPackList2.DataSource = resort(otherpacklist, "Name", "ASC");
                    //GvPackList2.DataBind();
                    //GvPackList3.DataSource = resort(packlist, "Name", "ASC");
                    //GvPackList3.DataBind();
                    GvPackList3.DataSource = resort(testdt, "Name", "ASC");
                    GvPackList3.DataBind();
                    GVMail.DataSource = resort(testdt, "Name", "ASC");
                    GVMail.DataBind();
                    if (testdt.Rows.Count > 0)
                    {
                        btnPDF.Visible = true;
                        btnXLS.Visible = true;
                        btnsendsmsmail.Visible = true;
                        //btnsendexcel.Visible = true;
                        //btnsmstext.Visible = true;
                    }
                    else
                    {
                        btnPDF.Visible = false;
                        btnXLS.Visible = false;
                        btnsendsmsmail.Visible = false;
                        //btnsendexcel.Visible = false;
                        //btnsmstext.Visible = false;
                    }
                }
                else
                {
                    GvPackList3.DataSource = null;
                    GvPackList3.DataBind();
                    GVMail.DataSource = null;
                    GVMail.DataBind();
                }
                //sshobj.SshDisconnect(client);
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg;//ex.ToString();
            }
        }
        public DataSet getoptions()
        {
            DataTable dt = new DataTable("finaloptionlist");
            dt.Clear();
            dt.Columns.Add("order_id");
            dt.Columns.Add("product_id");
            dt.Columns.Add("Name");
            dt.Columns.Add("weight");
            dt.Columns.Add("qty_ordered");

            DataSet orderoptionlist = new DataSet();
            try
            {

                queryString = @"SELECT a.order_id, a.product_id, a.Name, a.weight, a.qty_ordered, value, '' as units
FROM `sales_flat_order_grid` AS z
INNER JOIN `sales_flat_order_item` AS a ON z.entity_id = a.order_id
INNER JOIN sales_flat_quote_item_option AS b ON a.quote_item_id = b.item_id
INNER JOIN catalog_product_option_type_title AS c ON b.value = c.option_type_id
WHERE a.parent_item_id IS NULL 
AND a.product_type = 'simple'
AND c.title != 'No Need'
AND a.product_options LIKE '%Additional Qty%'
AND z.increment_id
IN (" + txtidlist.Text.ToString() + ")";
                //Response.Write(queryString);
                if (queryString != "")
                {
                    MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);


                    adapteradminmail.Fill(orderoptionlist, "orderoptions");

                    queryString = "select * from catalog_product_option_type_title";

                    DataSet orderoption = new DataSet();

                    MySqlDataAdapter adapterproductoption = new MySqlDataAdapter(queryString, conn);


                    adapterproductoption.Fill(orderoption, "orderoptions");
                    foreach (DataRow row in orderoptionlist.Tables[0].Rows)
                    {
                        string[] values = row["value"].ToString().Split(',');
                        Decimal weight = Convert.ToDecimal(row["weight"].ToString());
                        string unit = "";
                        for (int i = 0; i < values.Count(); i++)
                        {
                            int ono = Convert.ToInt32(values[i].ToString());
                            DataTable products1 = null;
                            products1 = orderoption.Tables[0].AsEnumerable().Where(r => Convert.ToInt32(r["option_type_id"]) == ono).AsDataView().ToTable();
                            string title = products1.Rows[0]["title"].ToString();
                            if (products1.Rows[0]["title"].ToString().Contains("Kg"))
                            {
                                title = products1.Rows[0]["title"].ToString().Replace("Kg", "").Trim();
                                unit = "Kg";
                            }
                            else if (products1.Rows[0]["title"].ToString().Contains("Bundle"))
                            {
                                title = products1.Rows[0]["title"].ToString().Replace("Bundle", "").Trim();
                                unit = "Bundle";
                            }
                            else if (products1.Rows[0]["title"].ToString().Contains("Piece"))
                            {
                                title = products1.Rows[0]["title"].ToString().Replace("Piece", "").Trim();
                                unit = "Piece";
                            }
                            else if (products1.Rows[0]["title"].ToString().Contains("Packet"))
                            {
                                title = products1.Rows[0]["title"].ToString().Replace("Packet", "").Trim();
                                unit = "Packet";
                            }
                            weight = weight + Convert.ToDecimal(title);
                        }

                        DataRow _optionList = dt.NewRow();
                        //, product_id, Name, weight, qty_ordered
                        _optionList["order_id"] = row["order_id"].ToString();
                        _optionList["product_id"] = row["product_id"].ToString();
                        _optionList["Name"] = row["Name"].ToString();
                        _optionList["weight"] = weight;
                        _optionList["qty_ordered"] = row["qty_ordered"].ToString();

                        row["weight"] = weight.ToString();
                        row["units"] = unit.ToString();
                    }
                }

            }
            catch (Exception ex)
            {
            }
            return orderoptionlist;
        }
        private DataTable getyoyopack()
        {
            DataTable testdt = new DataTable("testPackList1");
                testdt.Clear();
                testdt.Columns.Add("Name");
                testdt.Columns.Add("TotalWeight");
                testdt.Columns.Add("Units");

                try
                {
                    DataTable dt = new DataTable("PackList");
                    dt.Clear();
                    dt.Columns.Add("Name");
                    dt.Columns.Add("Weight");
                    dt.Columns.Add("Pack");
                    dt.Columns.Add("TotalWeight");
                    dt.Columns.Add("Units");

                    

                    string Packcount = "";
                    string kgcount = "";

                    //queryString = "SELECT entity_id, customer_firstname, customer_lastname FROM `sales_flat_order` where created_at between '" + Convert.ToDateTime(TbxFromDate.Text.ToString()).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(TbxToDate.Text.ToString()).ToString("yyyy-MM-dd") + "'";
                    queryString = @"SELECT a.order_id,a.product_id,a.Name,a.weight,a.qty_ordered
FROM `sales_flat_order_grid` as z inner join `sales_flat_order_item` AS a
ON z.entity_id=a.order_id
WHERE a.parent_item_id is not null and a.product_type not in('grouped') and a.sku IS NULL  and a.Name not like 'YOYO%' and a.Name not like '%Want To Buy This Product/%' 
AND z.increment_id in (" + txtidlist.Text + ")";// union SELECT a.order_id,a.product_id,a.Name,a.weight,a.qty_ordered FROM `sales_flat_order_grid` as z inner join `sales_flat_order_item` AS a ON z.entity_id=a.order_id WHERE a.product_type not in('grouped') and a.sku like 'a-la-carte-%' AND product_options NOT LIKE '%Additional Qty%' AND z.increment_id in (" + txtidlist.Text + ")
//                    queryString = @"SELECT order_id,product_id,Name,weight
//FROM `sales_flat_order_item`
//WHERE sku IS NULL and Name not like 'YOYO%' and Name not like '%Want To Buy This Product/%' 
//AND order_id in (" + txtidlist.Text + ")";
                    //Response.Write(queryString);
                    if (queryString != "")
                    {
                        MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                        DataSet orderlist = new DataSet();
                        adapteradminmail.Fill(orderlist, "sales_flat_order_item");
                        DataTable products = orderlist.Tables[0];
                        //products.Merge(getoptions().Tables[0]);
                        //var distinctRows = (from DataRow dRow in products.Rows
                        //                    select new { col1 = dRow["order_id"], col2 = dRow["dataColumn2"] }).Distinct();
                        var distinctRows = (from DataRow dRow in products.Rows
                                            select new { col1 = dRow["product_id"], col2 = dRow["weight"] }).Distinct();


                        foreach (var value in distinctRows)
                        {
                            string s = value.col1.ToString();
                            int ono = Convert.ToInt16(value.col1.ToString());


                            //var ProductRows = (from DataRow dRow in products.Rows
                            //                   where (dRow.Field<string>("product_id") == "64")
                            //                   select products);
                            //var results = (from myRow in products.AsEnumerable()
                            //               where myRow.Field<int>("order_id") == 711
                            //               select new { col1 = myRow["product_id"] });


                            
          //                  DT = PinCDAO.GetArea().AsEnumerable()
          //.Where(r => r.Field<int>("AreaID") == outText
          //                 || (r.Field<string>("AreaDescription")
          // .Contains(text))).AsDataView().ToTable();

                            DataTable products1 = null;
                            products1 = products.AsEnumerable().Where(r => Convert.ToInt32(r["product_id"]) == ono).AsDataView().ToTable();
                                        
//                            string queryString1 = @"SELECT order_id,product_id,Name,weight
//FROM `sales_flat_order_item`
//WHERE parent_item_id is not null and product_type not in('grouped') and sku IS NULL
//AND order_id in (" + txtidlist.Text + ")  and Name not like '%Want To Buy This Product/%'  and Name not like 'YOYO%'  and product_id=" + s;

//                            string queryString1 = @"SELECT order_id,product_id,Name,weight
//FROM `sales_flat_order_item`
//WHERE sku IS NULL
//AND order_id in (" + txtidlist.Text + ")  and Name not like '%Want To Buy This Product/%'  and Name not like 'YOYO%'  and product_id=" + s;
                            //MySqlDataAdapter adapteradminmail1 = new MySqlDataAdapter(queryString1, conn);

                            //DataSet orderlist1 = new DataSet();
                            //adapteradminmail1.Fill(orderlist1, "sales_flat_order_item");
                            //DataTable products1 = orderlist1.Tables[0];

                            string prodname = "";
                            string prodweight = "0";
                            string _Ispc = "0";
                            int qty = 0;
                            string units = "";
                            foreach (DataRow row in products1.Rows)
                            {
                                qty = qty + Convert.ToInt32(Convert.ToDecimal(row["qty_ordered"].ToString()));
                            }
                            foreach (DataRow row in products1.Rows)
                            {
                                if (Convert.ToDecimal(row["weight"].ToString()) < 1)
                                {
                                    units = "Kg";
                                }
                                else
                                {
                                    units = "Piece/Bundle";
                                }
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
                                //else
                                //{
                                //    pronam = pronam.Substring(pronam.IndexOf("R"), pronam.Length - pronam.IndexOf("R"));
                                //}
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
                                    //weight = Convert.ToInt32(products1.Rows.Count.ToString()) * Convert.ToInt32(pronam);
                                    weight = qty * Convert.ToInt32(pronam);
                                }
                                catch
                                {
                                }
                                if (kgcount == "")
                                {
                                    //kgcount = prodname + products1.Rows.Count.ToString() + "X" + pronam + " = " + weight;
                                    kgcount = prodname + qty.ToString() + "X" + pronam + " = " + weight;
                                }
                                else
                                {
                                    //kgcount = kgcount + "</br>" + prodname + products1.Rows.Count.ToString() + "X" + pronam + " = " + weight;
                                    kgcount = kgcount + "</br>" + prodname + qty.ToString() + "X" + pronam + " = " + weight;
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
                            //_PackList["Pack"] = products1.Rows.Count.ToString();

                            _PackList["Pack"] = qty.ToString();

                            if (_Ispc == "1")
                            {
                                //_PackList["TotalWeight"] = (Convert.ToInt32(Convert.ToDecimal(prodweight.ToString()) * Convert.ToInt32(products1.Rows.Count.ToString()))).ToString();
                                _PackList["TotalWeight"] = (Convert.ToInt32(Convert.ToDecimal(prodweight.ToString()) * qty)).ToString();
                            }
                            else
                            {
                                //_PackList["TotalWeight"] = (Convert.ToDecimal(prodweight.ToString()) * Convert.ToInt32(products1.Rows.Count.ToString())).ToString();
                                _PackList["TotalWeight"] = (Convert.ToDecimal(prodweight.ToString()) * qty).ToString();
                            }
                            _PackList["Units"] = units.ToString();
                            dt.Rows.Add(_PackList);
                            if (Packcount == "")
                            {
                                //Packcount = prodname + products1.Rows.Count.ToString();
                                Packcount = prodname + qty.ToString();
                            }
                            else
                            {
                                //Packcount = Packcount + "</br>" + prodname + products1.Rows.Count.ToString();
                                Packcount = Packcount + "</br>" + prodname + qty.ToString();
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
                        //GvPackList.DataSource = resort(dt, "Name", "ASC");
                        //GvPackList.DataBind();

                        string TestName = "";
                        string TestValue1 = "";
                        decimal Testweight = 0;
                        string Testunits = "";
                        string Testunits1 = "";
                        DataTable dtt1 = resort(dt, "Name", "ASC");
                        //if (dtt1.Rows.Count > 0)
                        //{
                            for (int i = 0; i < dtt1.Rows.Count; i++)
                            {

                                TestName = dtt1.Rows[i]["Name"].ToString();
                                Testunits = dtt1.Rows[i]["Units"].ToString();
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
                                    _TestPackList["Units"] = Testunits1.ToString();
                                    testdt.Rows.Add(_TestPackList);

                                    Testweight = 0;
                                    Testweight = Convert.ToDecimal(dtt1.Rows[i]["TotalWeight"].ToString());
                                }
                                TestValue1 = TestName.ToString();
                                Testunits1 = Testunits.ToString();
                            }
                            DataRow _TestPackListfinalrecord = testdt.NewRow();
                            _TestPackListfinalrecord["Name"] = TestValue1.ToString();
                            _TestPackListfinalrecord["TotalWeight"] = Testweight.ToString();
                            _TestPackListfinalrecord["Units"] = Testunits1.ToString();
                            testdt.Rows.Add(_TestPackListfinalrecord);
                        //}
                        //GvPackList2.DataSource = resort(testdt, "Name", "ASC");
                        //GvPackList2.DataBind();

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
                    }
                }
                catch (Exception ex)
                {
                    lblerror.Text = errormsg;//ex.ToString();
                }
            return testdt;
        }
        private DataTable getsinglewithmultipleproduct()
        {
            DataTable testdt = new DataTable("testPackList1");
            testdt.Clear();
            testdt.Columns.Add("Name");
            testdt.Columns.Add("TotalWeight");
            testdt.Columns.Add("Units");

            try
            {
                DataTable dt = new DataTable("PackList");
                dt.Clear();
                dt.Columns.Add("Name");
                dt.Columns.Add("Weight");
                dt.Columns.Add("Pack");
                dt.Columns.Add("TotalWeight");
                dt.Columns.Add("Units");




                string Packcount = "";
                string kgcount = "";

                //queryString = "SELECT entity_id, customer_firstname, customer_lastname FROM `sales_flat_order` where created_at between '" + Convert.ToDateTime(TbxFromDate.Text.ToString()).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(TbxToDate.Text.ToString()).ToString("yyyy-MM-dd") + "'";
                queryString = @"SELECT a.order_id,a.product_id,a.Name,a.weight,a.qty_ordered,'' as units FROM `sales_flat_order_grid` as z inner join `sales_flat_order_item` AS a ON z.entity_id=a.order_id WHERE a.product_type not in('grouped') and a.sku like 'A-la-c%' AND product_options NOT LIKE '%Additional Qty%' AND z.increment_id in (" + txtidlist.Text + ")";
                //a-la-carte
                //                    queryString = @"SELECT order_id,product_id,Name,weight
                //FROM `sales_flat_order_item`
                //WHERE sku IS NULL and Name not like 'YOYO%' and Name not like '%Want To Buy This Product/%' 
                //AND order_id in (" + txtidlist.Text + ")";
                //Response.Write(queryString);
                if (queryString != "")
                {
                    MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                    DataSet orderlist = new DataSet();
                    adapteradminmail.Fill(orderlist, "sales_flat_order_item");
                    DataTable products = orderlist.Tables[0];
                    products.Merge(getoptions().Tables[0]);
                    //var distinctRows = (from DataRow dRow in products.Rows
                    //                    select new { col1 = dRow["order_id"], col2 = dRow["dataColumn2"] }).Distinct();
                    //var distinctRows = (from DataRow dRow in products.Rows
                    //                    select new { col1 = dRow["product_id"], col2 = dRow["weight"] }).Distinct();
                    var distinctRows = (from DataRow dRow in products.Rows
                                        select new { col1 = dRow["product_id"]}).Distinct();


                    foreach (var value in distinctRows)
                    {
                        string s = value.col1.ToString();
                        int ono = Convert.ToInt16(value.col1.ToString());


                        //var ProductRows = (from DataRow dRow in products.Rows
                        //                   where (dRow.Field<string>("product_id") == "64")
                        //                   select products);
                        //var results = (from myRow in products.AsEnumerable()
                        //               where myRow.Field<int>("order_id") == 711
                        //               select new { col1 = myRow["product_id"] });



                        //                  DT = PinCDAO.GetArea().AsEnumerable()
                        //.Where(r => r.Field<int>("AreaID") == outText
                        //                 || (r.Field<string>("AreaDescription")
                        // .Contains(text))).AsDataView().ToTable();

                        DataTable products1 = null;
                        products1 = products.AsEnumerable().Where(r => Convert.ToInt32(r["product_id"]) == ono).AsDataView().ToTable();

                        //                            string queryString1 = @"SELECT order_id,product_id,Name,weight
                        //FROM `sales_flat_order_item`
                        //WHERE parent_item_id is not null and product_type not in('grouped') and sku IS NULL
                        //AND order_id in (" + txtidlist.Text + ")  and Name not like '%Want To Buy This Product/%'  and Name not like 'YOYO%'  and product_id=" + s;

                        //                            string queryString1 = @"SELECT order_id,product_id,Name,weight
                        //FROM `sales_flat_order_item`
                        //WHERE sku IS NULL
                        //AND order_id in (" + txtidlist.Text + ")  and Name not like '%Want To Buy This Product/%'  and Name not like 'YOYO%'  and product_id=" + s;
                        //MySqlDataAdapter adapteradminmail1 = new MySqlDataAdapter(queryString1, conn);

                        //DataSet orderlist1 = new DataSet();
                        //adapteradminmail1.Fill(orderlist1, "sales_flat_order_item");
                        //DataTable products1 = orderlist1.Tables[0];

                        string prodname = "";
                        string prodweight = "0";
                        string _Ispc = "0";
                        int qty = 0;
                        decimal weightcalc = 0;
                        string units = "";
                        foreach (DataRow row in products1.Rows)
                        {
                            qty = qty + Convert.ToInt32(Convert.ToDecimal(row["qty_ordered"].ToString()));

                            weightcalc = weightcalc + Convert.ToDecimal(row["weight"].ToString()) * Convert.ToInt32(Convert.ToDecimal(row["qty_ordered"].ToString()));

                            prodname = row["Name"].ToString();

                            prodweight = weightcalc.ToString();
                            if (row["units"].ToString() == "")
                            {
                                if (Convert.ToDecimal(row["weight"].ToString()) < 1)
                                {
                                    units = "Kg";
                                }
                                else
                                {
                                    units = "Piece/Bundle";
                                }
                            }
                            else
                            {
                                units = row["units"].ToString();
                            }
                        }
                        DataRow _PackList = dt.NewRow();
                        _PackList["Name"] = prodname.ToString();
                        _PackList["Weight"] = "0.2500";
                        _PackList["Pack"] = qty.ToString();
                        _PackList["TotalWeight"] = prodweight.ToString();
                        _PackList["Units"] = units;
                        dt.Rows.Add(_PackList);

                        //--------------
                        //foreach (DataRow row in products1.Rows)
                        //{
                        //    qty = qty + Convert.ToInt32(Convert.ToDecimal(row["qty_ordered"].ToString()));
                        //}
                        //foreach (DataRow row in products1.Rows)
                        //{
                        //    prodweight = row["weight"].ToString();
                        //    prodname = row["Name"].ToString();
                        //    string pronam = row["Name"].ToString();
                        //    if (pronam.Contains("("))
                        //    {
                        //        pronam = pronam.Substring(pronam.IndexOf("("), pronam.Length - pronam.IndexOf("("));
                        //        pronam = pronam.Replace("(", "");
                        //        pronam = pronam.Replace(")", "").Trim();
                        //    }
                        //    else if (pronam.Contains("["))
                        //    {
                        //        pronam = pronam.Substring(pronam.IndexOf("["), pronam.Length - pronam.IndexOf("["));
                        //        pronam = pronam.Replace("[", "");
                        //        pronam = pronam.Replace("]", "").Trim();
                        //    }
                        //    //else
                        //    //{
                        //    //    pronam = pronam.Substring(pronam.IndexOf("R"), pronam.Length - pronam.IndexOf("R"));
                        //    //}
                        //    pronam = pronam.ToLower();
                        //    if (pronam.Contains("valathandu"))
                        //    {
                        //        pronam = pronam.Replace("valathandu", "");
                        //        _Ispc = "1";
                        //    }
                        //    if (pronam.Contains("kgs"))
                        //    {
                        //        pronam = pronam.Replace("kgs", "");
                        //    }
                        //    else if (pronam.Contains("kg"))
                        //    {
                        //        pronam = pronam.Replace("kg", "");
                        //    }
                        //    else if (pronam.Contains("gms"))
                        //    {
                        //        pronam = pronam.Replace("gms", "");
                        //    }
                        //    else if (pronam.Contains("g"))
                        //    {
                        //        pronam = pronam.Replace("g", "");
                        //    }
                        //    else if (pronam.Contains("pcs"))
                        //    {
                        //        pronam = pronam.Replace("pcs", "");
                        //        _Ispc = "1";
                        //    }
                        //    else if (pronam.Contains("pc"))
                        //    {
                        //        pronam = pronam.Replace("pc", "");
                        //        _Ispc = "1";
                        //    }
                        //    else if (pronam.Contains("bundle"))
                        //    {
                        //        pronam = pronam.Replace("bundle", "");
                        //        _Ispc = "1";
                        //    }
                        //    pronam = pronam.Trim();
                        //    int weight = 0;
                        //    try
                        //    {
                        //        //weight = Convert.ToInt32(products1.Rows.Count.ToString()) * Convert.ToInt32(pronam);
                        //        weight = qty * Convert.ToInt32(pronam);
                        //    }
                        //    catch
                        //    {
                        //    }
                        //    if (kgcount == "")
                        //    {
                        //        //kgcount = prodname + products1.Rows.Count.ToString() + "X" + pronam + " = " + weight;
                        //        kgcount = prodname + qty.ToString() + "X" + pronam + " = " + weight;
                        //    }
                        //    else
                        //    {
                        //        //kgcount = kgcount + "</br>" + prodname + products1.Rows.Count.ToString() + "X" + pronam + " = " + weight;
                        //        kgcount = kgcount + "</br>" + prodname + qty.ToString() + "X" + pronam + " = " + weight;
                        //    }
                        //    break;
                        //}
                        //DataRow _PackList = dt.NewRow();
                        //_PackList["Name"] = prodname.ToString();
                        //if (_Ispc == "1")
                        //{
                        //    _PackList["Weight"] = (Convert.ToInt32(Convert.ToDecimal(prodweight.ToString()))).ToString();
                        //}
                        //else
                        //{
                        //    _PackList["Weight"] = (Convert.ToDecimal(prodweight.ToString())).ToString();
                        //}
                        ////_PackList["Pack"] = products1.Rows.Count.ToString();

                        //_PackList["Pack"] = qty.ToString();

                        //if (_Ispc == "1")
                        //{
                        //    //_PackList["TotalWeight"] = (Convert.ToInt32(Convert.ToDecimal(prodweight.ToString()) * Convert.ToInt32(products1.Rows.Count.ToString()))).ToString();
                        //    _PackList["TotalWeight"] = (Convert.ToInt32(Convert.ToDecimal(prodweight.ToString()) * qty)).ToString();
                        //}
                        //else
                        //{
                        //    //_PackList["TotalWeight"] = (Convert.ToDecimal(prodweight.ToString()) * Convert.ToInt32(products1.Rows.Count.ToString())).ToString();
                        //    _PackList["TotalWeight"] = (Convert.ToDecimal(prodweight.ToString()) * qty).ToString();
                        //}
                        //dt.Rows.Add(_PackList);
                        //if (Packcount == "")
                        //{
                        //    //Packcount = prodname + products1.Rows.Count.ToString();
                        //    Packcount = prodname + qty.ToString();
                        //}
                        //else
                        //{
                        //    //Packcount = Packcount + "</br>" + prodname + products1.Rows.Count.ToString();
                        //    Packcount = Packcount + "</br>" + prodname + qty.ToString();
                        //}

                        //------------
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
                    //GvPackList.DataSource = resort(dt, "Name", "ASC");
                    //GvPackList.DataBind();

                    string TestName = "";
                    string TestValue1 = "";
                    decimal Testweight = 0;
                    string Testunits = "";
                    string Testunits1 = "";

                    DataTable dtt1 = resort(dt, "Name", "ASC");
                    //if (dtt1.Rows.Count > 0)
                    //{
                    for (int i = 0; i < dtt1.Rows.Count; i++)
                    {

                        TestName = dtt1.Rows[i]["Name"].ToString();
                        Testunits=dtt1.Rows[i]["units"].ToString();
                        //if (TestName.Contains("("))
                        //{
                        //    TestName = TestName.Substring(0, TestName.IndexOf("("));
                        //}
                        //else if (TestName.Contains("["))
                        //{
                        //    TestName = TestName.Substring(0, TestName.IndexOf("["));
                        //}

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
                        else if (TestName.Contains(TestValue1.ToString()))//else if (TestName==TestValue1.ToString())
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
                            _TestPackList["Units"] = Testunits1.ToString();
                            testdt.Rows.Add(_TestPackList);

                            Testweight = 0;
                            Testweight = Convert.ToDecimal(dtt1.Rows[i]["TotalWeight"].ToString());
                        }
                        TestValue1 = TestName.ToString();
                        Testunits1 = Testunits.ToString();
                    }
                    DataRow _TestPackListfinalrecord = testdt.NewRow();
                    _TestPackListfinalrecord["Name"] = TestValue1.ToString();
                    _TestPackListfinalrecord["TotalWeight"] = Testweight.ToString();
                    _TestPackListfinalrecord["Units"] = Testunits1.ToString();
                    testdt.Rows.Add(_TestPackListfinalrecord);
                    //}
                    //GvPackList2.DataSource = resort(testdt, "Name", "ASC");
                    //GvPackList2.DataBind();

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
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg;//ex.ToString();
            }
            return testdt;
        }
        private DataTable getotherpack()
        {
            DataTable testdt = new DataTable("testPackList");
                testdt.Clear();
                testdt.Columns.Add("Name");
                testdt.Columns.Add("TotalWeight");
            try
            {
                DataTable dt = new DataTable("PackList");
                dt.Clear();
                dt.Columns.Add("Name");
                dt.Columns.Add("Weight");
                dt.Columns.Add("Pack");
                dt.Columns.Add("TotalWeight");

                

                string Packcount = "";
                string kgcount = "";

                //queryString = "SELECT entity_id, customer_firstname, customer_lastname FROM `sales_flat_order` where created_at between '" + Convert.ToDateTime(TbxFromDate.Text.ToString()).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(TbxToDate.Text.ToString()).ToString("yyyy-MM-dd") + "'";
                queryString = @"SELECT a.pack_name, a.name, a.weight, b.qty_ordered
FROM `1_pack_item_ffhp_product` AS a
INNER JOIN sales_flat_order_item AS b ON a.pack_name = b.name
INNER JOIN `sales_flat_order_grid` as z ON b.order_id=z.entity_id
WHERE z.increment_id in (" + txtidlist.Text + ")";
//                queryString = @"SELECT a.pack_name, a.name, a.weight
//FROM `1_pack_item_ffhp_product` AS a
//LEFT OUTER JOIN sales_flat_order_item AS b ON a.pack_name = b.name
//WHERE b.order_id in (" + txtidlist.Text + @")
//
//union all
//
//SELECT distinct a.pack_name, a.name, a.weight
//FROM `1_pack_item_ffhp_product` AS a
//LEFT OUTER JOIN sales_flat_order_item AS b ON a.pack_name = b.name
//WHERE a.name in (
//
//SELECT c.title
//FROM sales_flat_order_item AS a
//INNER JOIN sales_flat_quote_item_option AS b ON a.quote_item_id = b.item_id
//INNER JOIN catalog_product_option_type_title AS c ON b.value = c.option_type_id
//WHERE a.parent_item_id is null and a.product_type='simple' and a.quote_item_id in((select quote_item_id from sales_flat_order_item where order_id in (" + txtidlist.Text + @"))) AND c.title != 'No Need' AND b.option_id NOT IN (SELECT ua.option_id FROM (SELECT y. * FROM sales_flat_order_item AS x INNER JOIN sales_flat_quote_item_option AS y ON x.quote_item_id = y.item_id WHERE x.quote_item_id in((select quote_item_id from sales_flat_order_item where order_id in (" + txtidlist.Text + @") )) LIMIT 2 ) AS A INNER JOIN sales_flat_quote_item_option AS ua ON ua.option_id = A.option_id)) ";
                //Response.Write(queryString);
                if (queryString != "")
                {
                    MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                    DataSet orderlist = new DataSet();
                    adapteradminmail.Fill(orderlist, "sales_flat_order_item");
                    DataTable products = orderlist.Tables[0];
                    products.Merge(getotherpackoptions().Tables[0]);
                    //var distinctRows = (from DataRow dRow in products.Rows
                    //                    select new { col1 = dRow["order_id"], col2 = dRow["dataColumn2"] }).Distinct();
                    var distinctRows = (from DataRow dRow in products.Rows
                                        select new { col1 = dRow["name"], col2 = dRow["weight"] }).Distinct();


                    foreach (var value in distinctRows)
                    {
                        string s = value.col1.ToString();
                        //int ono = Convert.ToInt32(value.col1.ToString());


                        //var ProductRows = (from DataRow dRow in products.Rows
                        //                   where (dRow.Field<string>("product_id") == "64")
                        //                   select products);
                        //var results = (from myRow in products.AsEnumerable()
                        //               where myRow.Field<int>("order_id") == 711
                        //               select new { col1 = myRow["product_id"] });
                        DataTable products1 = null;
                        products1 = products.AsEnumerable().Where(r => Convert.ToString(r["name"]) == s).AsDataView().ToTable();

//                        string queryString1 = @"SELECT a.pack_name, a.name, a.weight
//FROM `1_pack_item_ffhp_product` AS a
//LEFT OUTER JOIN sales_flat_order_item AS b ON a.pack_name = b.name
//WHERE b.order_id in (" + txtidlist.Text + ") and a.name='" + s + "'";

//                        string queryString1 = @"SELECT a.pack_name, a.name, a.weight
//FROM `1_pack_item_ffhp_product` AS a
//a.name='" + s + "'";

                        //MySqlDataAdapter adapteradminmail1 = new MySqlDataAdapter(queryString1, conn);

                        //DataSet orderlist1 = new DataSet();
                        //adapteradminmail1.Fill(orderlist1, "sales_flat_order_item");
                        //DataTable products1 = orderlist1.Tables[0];

                        string prodname = "";
                        string prodweight = "0";
                        string qty = "0";
                        int qty1 = 0;
                        string _Ispc = "0";
                        foreach (DataRow row in products1.Rows)
                        {

                            qty1 = qty1 + Convert.ToInt32(Convert.ToDecimal(row["qty_ordered"].ToString()));
                        }
                        foreach (DataRow row in products1.Rows)
                        {
                            //qty = row["qty_ordered"].ToString();
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
                                //pronam = pronam.Substring(pronam.IndexOf("R"), pronam.Length - pronam.IndexOf("R"));
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
                            //int weight = 0;
                            
                            //try
                            //{
                            //    weight = Convert.ToInt32(products1.Rows.Count.ToString()) * Convert.ToInt32(pronam);
                            //}
                            //catch
                            //{
                            //}
                            //if (kgcount == "")
                            //{
                            //    kgcount = prodname + products1.Rows.Count.ToString() + "X" + pronam + " = " + weight;
                            //}
                            //else
                            //{
                            //    kgcount = kgcount + "</br>" + prodname + products1.Rows.Count.ToString() + "X" + pronam + " = " + weight;
                            //}
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

                        //_PackList["Pack"] = products1.Rows.Count.ToString();
                        _PackList["Pack"] = (qty1).ToString();
                        if (_Ispc == "1")
                        {
                            //_PackList["TotalWeight"] = (Convert.ToInt32(Convert.ToDecimal(prodweight.ToString()) * Convert.ToInt32(products1.Rows.Count.ToString()))).ToString();
                            _PackList["TotalWeight"] = (Convert.ToInt32(Convert.ToDecimal(prodweight.ToString()) * qty1)).ToString();
                        }
                        else
                        {
                            //_PackList["TotalWeight"] = (Convert.ToDecimal(prodweight.ToString()) * Convert.ToInt32(products1.Rows.Count.ToString())).ToString();
                            _PackList["TotalWeight"] = (Convert.ToDecimal(prodweight.ToString()) * qty1).ToString();
                        }
                        dt.Rows.Add(_PackList);
                        if (Packcount == "")
                        {
                            //Packcount = prodname + products1.Rows.Count.ToString();
                            Packcount = prodname + (qty1).ToString();
                        }
                        else
                        {
                            //Packcount = Packcount + "</br>" + prodname + products1.Rows.Count.ToString();
                            Packcount = Packcount + "</br>" + prodname + (qty1).ToString();
                        }
                        

                    }
                    //GvPackList.DataSource = resort(dt, "Name", "ASC");
                    //GvPackList.DataBind();
                    string TestName = "";
                    string TestValue1 = "";
                    decimal Testweight = 0;

                    DataTable dtt1 = resort(dt, "Name", "ASC");
                    if (dtt1.Rows.Count > 0)
                    {
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
                        DataRow _TestPackListfinalrecord = testdt.NewRow();
                        _TestPackListfinalrecord["Name"] = TestValue1.ToString();
                        _TestPackListfinalrecord["TotalWeight"] = Testweight.ToString();
                        testdt.Rows.Add(_TestPackListfinalrecord);
                    }

                    //string[] myColors = txtidlist.Text.ToString().Split(',');
                    //foreach (string ite in myColors)
                    //{
                    //    string sits = ite.ToString();
                    //    //Console.WriteLine(num);

                    //    DataTable dtt2 = getotherpackoptions(ite.ToString());
                    //    if (dtt2.Rows.Count > 0)
                    //    {
                    //        for (int i = 0; i < dtt2.Rows.Count; i++)
                    //        {
                    //            DataRow _TestPackListfinalrecord = testdt.NewRow();
                    //            _TestPackListfinalrecord["Name"] = dtt2.Rows[i]["Name"].ToString();
                    //            _TestPackListfinalrecord["TotalWeight"] = Convert.ToDecimal(dtt2.Rows[i]["TotalWeight"].ToString());
                    //            testdt.Rows.Add(_TestPackListfinalrecord);
                    //        }
                    //    }
                    //}
                    //GvPackList2.DataSource = resort(testdt, "Name", "ASC");
                    //GvPackList2.DataBind();
                }
                
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg;//ex.ToString();
            }
            return testdt;
        }
        public DataTable getotherpackoptions(string orderid)
        {
            DataTable testdt = new DataTable("testPackList2");
                testdt.Clear();
                testdt.Columns.Add("Name");
                testdt.Columns.Add("TotalWeight");
                try
                {
                    DataTable dt = new DataTable("PackList");
                    dt.Clear();
                    dt.Columns.Add("Name");
                    //dt.Columns.Add("Weight");
                    //dt.Columns.Add("Pack");
                    dt.Columns.Add("TotalWeight");



                    string Packcount = "";
                    string kgcount = "";

                    //queryString = "SELECT entity_id, customer_firstname, customer_lastname FROM `sales_flat_order` where created_at between '" + Convert.ToDateTime(TbxFromDate.Text.ToString()).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(TbxToDate.Text.ToString()).ToString("yyyy-MM-dd") + "'";
//                    queryString = @"SELECT a.pack_name, a.name, a.weight
//FROM `1_pack_item_ffhp_product` AS a
//LEFT OUTER JOIN sales_flat_order_item AS b ON a.pack_name = b.name
//WHERE b.order_id in (" + txtidlist.Text + ")";
//                    queryString = @"                    
//                    SELECT distinct a.name as Name, a.weight as TotalWeight
//                    FROM `1_pack_item_ffhp_product` AS a
//                    LEFT OUTER JOIN sales_flat_order_item AS b ON a.pack_name = b.name
//                    WHERE a.name in (
//                    
//                    SELECT c.title
//                    FROM sales_flat_order_item AS a
//                    INNER JOIN sales_flat_quote_item_option AS b ON a.quote_item_id = b.item_id
//                    INNER JOIN catalog_product_option_type_title AS c ON b.value = c.option_type_id
//                    WHERE a.parent_item_id is null and a.product_type='simple' and a.quote_item_id in((select quote_item_id from sales_flat_order_item where order_id in (" + orderid + @"))) AND c.title != 'No Need' AND b.option_id NOT IN (SELECT ua.option_id FROM (SELECT y. * FROM sales_flat_order_item AS x INNER JOIN sales_flat_quote_item_option AS y ON x.quote_item_id = y.item_id WHERE x.quote_item_id in((select quote_item_id from sales_flat_order_item where order_id in (" + orderid + @") )) LIMIT 2 ) AS A INNER JOIN sales_flat_quote_item_option AS ua ON ua.option_id = A.option_id)) ";

                    queryString = @"SELECT distinct d.name as Name, d.weight as TotalWeight
from sales_flat_order_item as a 

inner join sales_flat_quote_item_option as b on a.quote_item_id = b.item_id

inner join catalog_product_option_type_title AS c ON b.value = c.option_type_id

inner join 1_pack_item_ffhp_product as d on d.name=c.title

where a.parent_item_id is null and a.product_type='simple'

and c.title !='No Need'

and a.order_id in ("+orderid+")";
                    //Response.Write(queryString);
                    if (queryString != "")
                    {
                        MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                        DataSet orderlist = new DataSet();
                        adapteradminmail.Fill(orderlist, "otherpackoptions");
                        testdt = resort(orderlist.Tables[0], "Name", "ASC");
                        //dt = orderlist.Tables[0];

                        //DataTable dtt1 = resort(dt, "Name", "ASC");
                        //if (dtt1.Rows.Count > 0)
                        //{
                        //    for (int i = 0; i < dtt1.Rows.Count; i++)
                        //    {
                        //        DataRow _TestPackList = testdt.NewRow();
                        //        _TestPackList["Name"] = dtt1.Rows[i]["Name"].ToString();
                        //        _TestPackList["TotalWeight"] = Convert.ToDecimal(dtt1.Rows[i]["TotalWeight"].ToString());
                        //        testdt.Rows.Add(_TestPackList);
                        //    }
                        //}
                    }
                    
                }
                catch (Exception ex)
                {
                }
                return testdt;
        }
        public DataSet getotherpackoptions()
        {
            DataSet orderlist = new DataSet();
            try
            {
               
                queryString = @"SELECT distinct d.pack_name,d.name, d.weight, a.qty_ordered
from `sales_flat_order_grid` as z inner join `sales_flat_order_item` AS a
ON z.entity_id=a.order_id

inner join sales_flat_quote_item_option as b on a.quote_item_id = b.item_id

inner join catalog_product_option_type_title AS c ON b.value = c.option_type_id

inner join 1_pack_item_ffhp_product as d on d.name=c.title

where a.parent_item_id is null and a.product_type='simple'

and c.title !='No Need' and d.pack_name like '%(Optional)'

and z.increment_id in (" + txtidlist.Text.ToString() + ")";
                //Response.Write(queryString);
                if (queryString != "")
                {
                    MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                    
                    adapteradminmail.Fill(orderlist, "otherpackoptions");
                    
                }

            }
            catch (Exception ex)
            {
            }
            return orderlist;
        }
        public DataTable getsmsmapping()
        {
            DataTable dtmapping = new DataTable();
            queryString = @"SELECT * FROM `1_sms_ffhp_mapping`";
            //Response.Write(queryString);
            if (queryString != "")
            {
                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                DataSet orderlist = new DataSet();
                adapteradminmail.Fill(orderlist, "smsmapping1");

                dtmapping = resort(orderlist.Tables[0], "name", "ASC");
                //DataTable filteredDT = (from t in dt.AsEnumerable()
                //                        where t.Field<string>("Name").Equals("Ash Gourds")
                //                        select t).CopyToDataTable();
                //var query = from t in dt.AsEnumerable()
                //            where t.Field<string>("name").Equals("Ash Gourd")
                //            select t;

                //DataTable filteredDT = null;
                //if (query != null && query.Count() > 0)
                //{
                //    filteredDT = query.CopyToDataTable();
                //}
            }
            return dtmapping;
        }
        public string getsmsmapstring(DataTable dt1, string name)
        {
            string mapname = "";
            var query = from t in dt1.AsEnumerable()
                        where t.Field<string>("name").Equals(name)
                        select t;

            DataTable filteredDT = null;
            if (query != null && query.Count() > 0)
            {
                filteredDT = query.CopyToDataTable();
                mapname = filteredDT.Rows[0]["tanglish_name"].ToString();
            }
            if (mapname.Trim() == "")
            {
                mapname = name;
            }
            return mapname;
        }
        public string getsms()
        {
            DataTable dtmapping = new DataTable();
            queryString = @"SELECT * FROM `1_sms_ffhp_mapping`";
            //Response.Write(queryString);
            if (queryString != "")
            {
                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                DataSet orderlist = new DataSet();
                adapteradminmail.Fill(orderlist, "smsmapping1");

                dtmapping = resort(orderlist.Tables[0], "name", "ASC");
                //DataTable filteredDT = (from t in dt.AsEnumerable()
                //                        where t.Field<string>("Name").Equals("Ash Gourds")
                //                        select t).CopyToDataTable();
                //var query = from t in dt.AsEnumerable()
                //            where t.Field<string>("name").Equals("Ash Gourd")
                //            select t;

                //DataTable filteredDT = null;
                //if (query != null && query.Count() > 0)
                //{
                //    filteredDT = query.CopyToDataTable();
                //}
            }
            string newsms = "";
            string testsms = lblsmsformat.Text.ToString();
            string mapname = "";

            foreach (GridViewRow itm in GvPackList3.Rows)
            {

                string sits = itm.Cells[1].Text.ToString();

                var query = from t in dtmapping.AsEnumerable()
                            where t.Field<string>("name").Equals(sits.Trim())
                            select t;

                DataTable filteredDT = null;
                if (query != null && query.Count() > 0)
                {
                    filteredDT = query.CopyToDataTable();
                    mapname = filteredDT.Rows[0]["tanglish_name"].ToString();
                    testsms = testsms.Replace(sits, mapname);
                }
                //if (mapname.Trim() == "")
                //{
                //    mapname = sits;
                //}

                //if (newsms != "")
                //{
                //    newsms = newsms + "/" + sits;
                //}
                //else
                //{
                //    newsms = mapname;
                //}
            }
            return testsms;
        }
        public string getsmswithtype()
        {
            DataTable testdt = new DataTable("testsmslist");
            testdt.Clear();
            testdt.Columns.Add("name");
            testdt.Columns.Add("tanglish_name");
            testdt.Columns.Add("type");
            testdt.Columns.Add("weight");
            testdt.Columns.Add("units");

            DataTable dtmapping = new DataTable();
            queryString = @"SELECT * FROM `1_sms_ffhp_mapping`";
            //Response.Write(queryString);
            if (queryString != "")
            {
                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                DataSet orderlist = new DataSet();
                adapteradminmail.Fill(orderlist, "smsmapping1");

                dtmapping = resort(orderlist.Tables[0], "name", "ASC");
                //DataTable filteredDT = (from t in dt.AsEnumerable()
                //                        where t.Field<string>("Name").Equals("Ash Gourds")
                //                        select t).CopyToDataTable();
                //var query = from t in dt.AsEnumerable()
                //            where t.Field<string>("name").Equals("Ash Gourd")
                //            select t;

                //DataTable filteredDT = null;
                //if (query != null && query.Count() > 0)
                //{
                //    filteredDT = query.CopyToDataTable();
                //}
            }
            string newsms = "";
            string testsms = lblsmsformat.Text.ToString();
            string mapname = "";

            foreach (GridViewRow itm in GvPackList3.Rows)
            {
                //byte[] bytes = Encoding.Default.GetBytes(itm.Cells[1].Text.ToString());
                //string nnnn = Encoding.UTF8.GetString(bytes);

                string sits = itm.Cells[1].Text.ToString();
                string weight = (Convert.ToDouble(itm.Cells[2].Text.ToString())).ToString();

                var query = from t in dtmapping.AsEnumerable()
                            where t.Field<string>("name").Equals(sits.Trim())
                            select t;

                DataTable filteredDT = null;
                DataRow _SmsList = testdt.NewRow();
                if (query != null && query.Count() > 0)
                {
                    filteredDT = query.CopyToDataTable();
                    
                    mapname = filteredDT.Rows[0]["tanglish_name"].ToString();
                    //testsms = testsms.Replace(sits, mapname);

                    _SmsList["name"] = filteredDT.Rows[0]["name"].ToString();
                    if (filteredDT.Rows[0]["tanglish_name"].ToString() != "")
                    {
                        _SmsList["tanglish_name"] = filteredDT.Rows[0]["tanglish_name"].ToString();
                        testsms = testsms.Replace(sits, filteredDT.Rows[0]["tanglish_name"].ToString());
                    }
                    else
                    {
                        _SmsList["tanglish_name"] = filteredDT.Rows[0]["name"].ToString();
                    }
                    _SmsList["type"] = filteredDT.Rows[0]["type"].ToString();
                    _SmsList["weight"] = weight.ToString();
                    _SmsList["units"] = filteredDT.Rows[0]["units"].ToString();

                }
                else
                {
                    _SmsList["name"] = sits;
                    _SmsList["tanglish_name"] = sits;
                    _SmsList["type"] = "";
                    _SmsList["weight"] = weight.ToString();
                    _SmsList["units"] = "";
                }
                testdt.Rows.Add(_SmsList);
                //if (mapname.Trim() == "")
                //{
                //    mapname = sits;
                //}

                //if (newsms != "")
                //{
                //    newsms = newsms + "/" + sits;
                //}
                //else
                //{
                //    newsms = mapname;
                //}
            }
            DataTable smsdt = new DataTable("smslist");
            smsdt = resort(testdt, "type", "ASC");

            string testtype="0";
            foreach (DataRow dtrow in smsdt.Rows)
            {
                dtrow["tanglish_name"].ToString();
                dtrow["weight"].ToString();
                dtrow["type"].ToString();
                dtrow["units"].ToString();

                if (testtype == "0")
                {
                    newsms = dtrow["tanglish_name"].ToString() + " " + dtrow["weight"].ToString() + " " + dtrow["units"].ToString();
                }
                else if (testtype == dtrow["type"].ToString())
                {
                    if (newsms == "")
                    {
                        newsms = dtrow["tanglish_name"].ToString() + " " + dtrow["weight"].ToString() + " " + dtrow["units"].ToString();
                    }
                    else
                    {
                        newsms = newsms + System.Environment.NewLine + dtrow["tanglish_name"].ToString() + " " + dtrow["weight"].ToString() + " " + dtrow["units"].ToString();
                    }
                }
                else
                {
                    newsms = newsms + System.Environment.NewLine + System.Environment.NewLine;
                    if (newsms == "")
                    {
                        newsms = dtrow["tanglish_name"].ToString() + " " + dtrow["weight"].ToString() + " " + dtrow["units"].ToString();
                    }
                    else
                    {
                        newsms = newsms + System.Environment.NewLine + dtrow["tanglish_name"].ToString() + " " + dtrow["weight"].ToString() +" "+ dtrow["units"].ToString();
                    }
                }
                testtype = dtrow["type"].ToString();
            }
            //return "Old Format" + System.Environment.NewLine + testsms + System.Environment.NewLine + System.Environment.NewLine + "New Format" + System.Environment.NewLine + newsms;
            return newsms;
        }
        public static DataTable resort(DataTable dt, string colName, string direction)
        {
            dt.DefaultView.Sort = colName + " " + direction;
            dt = dt.DefaultView.ToTable();
            return dt;
        }
        public struct LinkItem
        {
            public string Href;
            public string Text;

            public override string ToString()
            {
                return Href + "\n\t" + Text;
            }
        }
        public static List<LinkItem> Find(string file)
        {
            List<LinkItem> list = new List<LinkItem>();

            // 1.
            // Find all matches in file.@"<li>(.*?)</li>"
            MatchCollection m1 = Regex.Matches(file, @"<li>(.*?)</li>",
                RegexOptions.Singleline);
            
            // 2.
            // Loop over each match.
            foreach (Match m in m1)
            {
                string value = m.Groups[1].Value;
                value = value.Replace(" style=\"color: #000000;\"", "");
                //string tag = "</span>";
                //value = value.Remove(0, value.IndexOf(tag) + tag.Length);
                LinkItem i = new LinkItem();

                // 3.
                // Get href attribute.
                Match m2 = Regex.Match(value, @"<span>(.*?)</span>",
                RegexOptions.Singleline);
                if (m2.Success)
                {
                    //i.Href = m2.Groups[1].Value;
                    value = m2.Groups[1].Value;
                }


                Match m3 = Regex.Match(value, @"<span>(.*?)</span>",
                RegexOptions.Singleline);
                if (m3.Success)
                {
                    //i.Href = m2.Groups[1].Value;
                    value = m3.Groups[1].Value;
                }

                Match m4 = Regex.Match(value, @"<span style=""color: #000000;"">(.*?)</span>",
                RegexOptions.Singleline);
                if (m4.Success)
                {
                    //i.Href = m2.Groups[1].Value;
                    value = m3.Groups[1].Value;
                }
                // Remove inner tags from text.
                value = Regex.Replace(value, @"<span>", "",
                RegexOptions.Singleline);
                // 4.
                // Remove inner tags from text.
                string t = Regex.Replace(value, @"&nbsp;", "",
                RegexOptions.Singleline);
                i.Text = t.Trim();
                if (i.Text != "")
                {
                    list.Add(i);
                }
            }
            return list;
        }
        protected void btnPDF_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //Response.ContentType = "application/pdf";
                //Response.AddHeader("content-disposition", "attachment;filename=TotalweightInfo.pdf");
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //StringWriter sw = new StringWriter();
                //HtmlTextWriter hw = new HtmlTextWriter(sw);
                //System.Web.UI.HtmlControls.HtmlForm f = new System.Web.UI.HtmlControls.HtmlForm();
                ////Panel Tom = new Panel();
                ////Tom.ID = base.UniqueID;
                ////Tom.Controls.Add(myControl);
                ////Page.FindControl("WebForm1").Controls.Add(Tom);

                //GvPackList3.AllowPaging = false;
                //f.Controls.Add(GvPackList3);
                ////GVOrderDetails2.DataBind();
                //GvPackList3.RenderControl(hw);
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

                //To Start second code
                GvPackList3.AllowPaging = false;
                //GridView1.DataBind();
                string s = Server.MapPath("Images/Calibri.ttf");
                BaseFont bf = BaseFont.CreateFont(s, BaseFont.IDENTITY_H, true);
                //BaseFont bf = BaseFont.CreateFont(Environment.GetEnvironmentVariable("windir") + @"\fonts\CALIBRI.TTF", BaseFont.IDENTITY_H, true);



                iTextSharp.text.pdf.PdfPTable table = new iTextSharp.text.pdf.PdfPTable(GvPackList3.Columns.Count);
                int[] widths = new int[GvPackList3.Columns.Count];
                for (int x = 0; x < GvPackList3.Columns.Count; x++)
                {
                    widths[x] = (int)GvPackList3.Columns[x].ItemStyle.Width.Value;
                    string cellText = Server.HtmlDecode(GvPackList3.HeaderRow.Cells[x].Text);

                    //Set Font and Font Color
                    iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 11, iTextSharp.text.Font.NORMAL);
                    //font.Color = new Color(GVOrderDetails2.HeaderStyle.ForeColor);
                    //font.Color = new Color(GVOrderDetails2.RowStyle.ForeColor);
                    iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(new Phrase(12, cellText, font));

                    //Set Header Row BackGround Color
                    //cell.BackgroundColor = new Color(GVOrderDetails2.HeaderStyle.BackColor);


                    table.AddCell(cell);
                }
                table.SetWidths(widths);

                for (int i = 0; i < GvPackList3.Rows.Count; i++)
                {
                    if (GvPackList3.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        for (int j = 0; j < GvPackList3.Columns.Count; j++)
                        {
                            string cellText = "";
                            if (j != 0)
                            {
                                cellText = Server.HtmlDecode(GvPackList3.Rows[i].Cells[j].Text);
                            }
                            else
                            {
                                cellText = Server.HtmlDecode((i + 1).ToString());
                            }
                            //Set Font and Font Color
                            iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 11, iTextSharp.text.Font.NORMAL);
                            //font.Color = new Color(GVOrderDetails2.RowStyle.ForeColor);
                            iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(new Phrase(12, cellText, font));

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

                //Create the PDF Document
                Document pdfDoc = new Document(PageSize.A4, 10f, 240f, 10f, 50f);
                //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                PdfWriter pdfwriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfwriter.PageEvent = new Footer(Server.MapPath("Images/Calibri.ttf"),35);
                pdfDoc.Open();
                pdfDoc.Add(table);
                pdfDoc.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=TotalweightInfo.pdf");
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
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "TotalweightInfo.xls"));
                Response.ContentType = "application/ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                GvPackList3.AllowPaging = false;
                //Change the Header Row back to white color
                GvPackList3.HeaderRow.Style.Add("background-color", "#FFFFFF");
                //Applying stlye to gridview header cells
                for (int i = 0; i < GvPackList3.HeaderRow.Cells.Count; i++)
                {
                    GvPackList3.HeaderRow.Cells[i].Style.Add("background-color", "#FFFFFF");
                    GvPackList3.HeaderRow.Cells[i].Style.Add("color", "#000000");
                }
                int j = 1;
                //This loop is used to apply stlye to cells based on particular row
                foreach (GridViewRow gvrow in GvPackList3.Rows)
                {
                    //gvrow.BackColor = Color.WHITE.ToString;
                    //if (j <= GvPackList3.Rows.Count)
                    //{
                        //if (j % 2 != 0)
                        //{
                            for (int k = 0; k < gvrow.Cells.Count; k++)
                            {
                                gvrow.Cells[k].Style.Add("background-color", "#FFFFFF");
                                gvrow.Cells[k].Style.Add("color", "#000000");
                            }
                        //}
                    //}
                    //j++;
                }
                System.Web.UI.HtmlControls.HtmlForm f = new System.Web.UI.HtmlControls.HtmlForm();
                f.Controls.Add(GvPackList3);
                //GVOrderDetails2.DataBind();

                GvPackList3.RenderControl(htw);
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
                string filename = "TotalweightInfo" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss");
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
                mail.Subject = "PFA - TotalweightInfo(XLS)";
                mail.Body = "PFA - TotalweightInfo(XLS)";

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
        protected void btnsendsmsmail_OnClick(object sender, EventArgs e)
        {
            try
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                //string sms =getsms();
                string sms = getsmswithtype();
                string mailto = System.Configuration.ConfigurationManager.AppSettings["Mail_To"].ToString();
                string mailcc = System.Configuration.ConfigurationManager.AppSettings["Mail_Cc"].ToString();
                string mailcredential = System.Configuration.ConfigurationManager.AppSettings["Mail_Credential"].ToString();
                string mailpassword = System.Configuration.ConfigurationManager.AppSettings["Mail_Password"].ToString();

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress(mailcredential);
                mail.To.Add(mailto);
                mail.CC.Add(mailcc);
                mail.Subject = "SMS format";
                //mail.Body = lblsmsformat.Text.ToString();
                mail.Body = sms;


                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(mailcredential, mailpassword);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                lblerror.Text = "Mail sent successfully.";
                //sshobj.SshDisconnect(client);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
                lblerror.Text = ex.ToString(); //errormsg;
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
        protected void btnsmstext_OnClick(object sender, EventArgs e)
        {
            try
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                string path = Server.MapPath("Images/ffhpsms.txt");
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                string pss1 = string.Format("{0:hh:mm}", DateTime.Now);//System.DateTime.Now.ToShortTimeString();
                FileStream pfs1 = new FileStream(path,
                                        FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter p_streamWriter1 = new StreamWriter(pfs1);
                p_streamWriter1.BaseStream.Seek(0, SeekOrigin.End);
                //p_streamWriter1.WriteLine(pss1 + "FFHP Orders " + " on " + DateTime.Today.Date.ToString("dd:MM:yyyy") + "_" + DateTime.Now.TimeOfDay.ToString()+DateTime.Now.ToString("tt")+" #" + txtidlist.Text.ToString()); p_streamWriter1.Flush();
                //p_streamWriter1.WriteLine(DateTime.Today.Date.ToString("dd:MM:yyyy") + "_" + DateTime.Now.TimeOfDay.ToString() + DateTime.Now.ToString("tt") + " #" + curordnum.ToString()); 
                p_streamWriter1.Write(getsmswithtype());
                p_streamWriter1.Flush();
                p_streamWriter1.Close();

                string path1 = Server.MapPath("Images/ffhpsms1.txt");

                if (File.Exists(path) == true)
                {
                    string attachment = "attachment; filename=" + "ffhpsms1.txt";
                    HttpContext.Current.Response.ClearContent();
                    HttpContext.Current.Response.AddHeader("content-disposition", attachment);
                    HttpContext.Current.Response.ContentType = "application/text";
                    HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Default;
                    HttpContext.Current.Response.TransmitFile(path1);
                    HttpContext.Current.Response.End();
                }
                //sshobj.SshDisconnect(client);
            }
            catch (Exception ex)
            {

            }
        }
        //Here below methods for ffhpservice api.
        public string API_ReadLastorders()
        {
            string ordernumbers = "";
            try
            {
                string path = Server.MapPath("Images/OrderNumbers.txt");
                string ord = File.ReadAllLines(path).Last();
                string[] Orders = ord.Split('#');
                if (Orders.Count() > 0)
                {
                    ordernumbers = Orders.Last();
                }
                if (ordernumbers.ToString() == "0")
                {
                    ordernumbers = "";
                }
            }
            catch (Exception ex)
            {
                
            }
            return ordernumbers;
        }
        public DataTable API_calculate()
        {
            string ordernumbers = API_ReadLastorders();
            //string outputsms = "";
            DataTable sms = new DataTable();
            try
            {

                if (ordernumbers.ToString() != "")
                {
                    //ConnectSsh sshobj = new ConnectSsh();
                    //SshClient client = sshobj.SshConnect();

                    DataTable testdt = new DataTable("testPackListfinal");
                    testdt.Clear();
                    testdt.Columns.Add("Name");
                    testdt.Columns.Add("TotalWeight");

                    //DataTable smsmapping = new DataTable();
                    //smsmapping = getsmsmapping();
                    DataTable yoyopacklist = new DataTable();
                    DataTable otherpacklist = new DataTable();
                    DataTable otherpacklistoptions = new DataTable();

                    DataTable packlist = new DataTable();
                    yoyopacklist = API_getyoyopack(ordernumbers);
                    otherpacklist = API_getotherpack(ordernumbers);
                    //otherpacklistoptions = getotherpackoptions();
                    packlist = yoyopacklist.Copy();
                    packlist.Merge(otherpacklist);
                    //packlist = otherpacklistoptions.Copy();
                    //packlist.Merge(otherpacklistoptions);


                    string TestName = "";
                    string TestValue1 = "";
                    decimal Testweight = 0;
                    string smsformat = "";
                    string smsformatfinal = "";
                    DataTable dtt1 = resort(packlist, "Name", "ASC");
                    if (dtt1.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtt1.Rows.Count; i++)
                        {

                            TestName = dtt1.Rows[i]["Name"].ToString();
                            //if (TestName.Contains("("))
                            //{
                            //    TestName = TestName.Substring(0, TestName.IndexOf("("));
                            //}
                            //else if (TestName.Contains("["))
                            //{
                            //    TestName = TestName.Substring(0, TestName.IndexOf("["));
                            //}
                            TestName = TestName.Trim();
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
                        DataRow _TestPackListfinalrecord = testdt.NewRow();
                        _TestPackListfinalrecord["Name"] = TestValue1.ToString();
                        _TestPackListfinalrecord["TotalWeight"] = Testweight.ToString();
                        testdt.Rows.Add(_TestPackListfinalrecord);
                        
                    }
                    
                    
                    
                    if (testdt.Rows.Count > 0)
                    {
                        
                        sms=API_getsmswithtype(testdt);
                    }
                    //sshobj.SshDisconnect(client);
                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {
                //lblerror.Text = errormsg;//ex.ToString();
            }
            return sms;
        }
        private DataTable API_getyoyopack(string ordernumbers)
        {
            DataTable testdt = new DataTable("testPackList1");
            testdt.Clear();
            testdt.Columns.Add("Name");
            testdt.Columns.Add("TotalWeight");

            try
            {
                DataTable dt = new DataTable("PackList");
                dt.Clear();
                dt.Columns.Add("Name");
                dt.Columns.Add("Weight");
                dt.Columns.Add("Pack");
                dt.Columns.Add("TotalWeight");




                string Packcount = "";
                string kgcount = "";

                //queryString = "SELECT entity_id, customer_firstname, customer_lastname FROM `sales_flat_order` where created_at between '" + Convert.ToDateTime(TbxFromDate.Text.ToString()).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(TbxToDate.Text.ToString()).ToString("yyyy-MM-dd") + "'";
                queryString = @"SELECT a.order_id,a.product_id,a.Name,a.weight,a.qty_ordered
FROM `sales_flat_order_grid` as z inner join `sales_flat_order_item` AS a
ON z.entity_id=a.order_id
WHERE a.parent_item_id is not null and a.product_type not in('grouped') and a.sku IS NULL  and a.Name not like 'YOYO%' and a.Name not like '%Want To Buy This Product/%' 
AND z.increment_id in (" + ordernumbers.ToString() + ")";
                //                    queryString = @"SELECT order_id,product_id,Name,weight
                //FROM `sales_flat_order_item`
                //WHERE sku IS NULL and Name not like 'YOYO%' and Name not like '%Want To Buy This Product/%' 
                //AND order_id in (" + txtidlist.Text + ")";
                //Response.Write(queryString);
                if (queryString != "")
                {
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
                        int ono = Convert.ToInt16(value.col1.ToString());


                        //var ProductRows = (from DataRow dRow in products.Rows
                        //                   where (dRow.Field<string>("product_id") == "64")
                        //                   select products);
                        //var results = (from myRow in products.AsEnumerable()
                        //               where myRow.Field<int>("order_id") == 711
                        //               select new { col1 = myRow["product_id"] });



                        //                  DT = PinCDAO.GetArea().AsEnumerable()
                        //.Where(r => r.Field<int>("AreaID") == outText
                        //                 || (r.Field<string>("AreaDescription")
                        // .Contains(text))).AsDataView().ToTable();

                        DataTable products1 = null;
                        products1 = products.AsEnumerable().Where(r => Convert.ToInt32(r["product_id"]) == ono).AsDataView().ToTable();

                        //                            string queryString1 = @"SELECT order_id,product_id,Name,weight
                        //FROM `sales_flat_order_item`
                        //WHERE parent_item_id is not null and product_type not in('grouped') and sku IS NULL
                        //AND order_id in (" + txtidlist.Text + ")  and Name not like '%Want To Buy This Product/%'  and Name not like 'YOYO%'  and product_id=" + s;

                        //                            string queryString1 = @"SELECT order_id,product_id,Name,weight
                        //FROM `sales_flat_order_item`
                        //WHERE sku IS NULL
                        //AND order_id in (" + txtidlist.Text + ")  and Name not like '%Want To Buy This Product/%'  and Name not like 'YOYO%'  and product_id=" + s;
                        //MySqlDataAdapter adapteradminmail1 = new MySqlDataAdapter(queryString1, conn);

                        //DataSet orderlist1 = new DataSet();
                        //adapteradminmail1.Fill(orderlist1, "sales_flat_order_item");
                        //DataTable products1 = orderlist1.Tables[0];

                        string prodname = "";
                        string prodweight = "0";
                        string _Ispc = "0";
                        int qty = 0;
                        foreach (DataRow row in products1.Rows)
                        {
                            qty = qty + Convert.ToInt32(Convert.ToDecimal(row["qty_ordered"].ToString()));
                        }
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
                            //else
                            //{
                            //    pronam = pronam.Substring(pronam.IndexOf("R"), pronam.Length - pronam.IndexOf("R"));
                            //}
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
                                //weight = Convert.ToInt32(products1.Rows.Count.ToString()) * Convert.ToInt32(pronam);
                                weight = qty * Convert.ToInt32(pronam);
                            }
                            catch
                            {
                            }
                            if (kgcount == "")
                            {
                                //kgcount = prodname + products1.Rows.Count.ToString() + "X" + pronam + " = " + weight;
                                kgcount = prodname + qty.ToString() + "X" + pronam + " = " + weight;
                            }
                            else
                            {
                                //kgcount = kgcount + "</br>" + prodname + products1.Rows.Count.ToString() + "X" + pronam + " = " + weight;
                                kgcount = kgcount + "</br>" + prodname + qty.ToString() + "X" + pronam + " = " + weight;
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
                        //_PackList["Pack"] = products1.Rows.Count.ToString();

                        _PackList["Pack"] = qty.ToString();

                        if (_Ispc == "1")
                        {
                            //_PackList["TotalWeight"] = (Convert.ToInt32(Convert.ToDecimal(prodweight.ToString()) * Convert.ToInt32(products1.Rows.Count.ToString()))).ToString();
                            _PackList["TotalWeight"] = (Convert.ToInt32(Convert.ToDecimal(prodweight.ToString()) * qty)).ToString();
                        }
                        else
                        {
                            //_PackList["TotalWeight"] = (Convert.ToDecimal(prodweight.ToString()) * Convert.ToInt32(products1.Rows.Count.ToString())).ToString();
                            _PackList["TotalWeight"] = (Convert.ToDecimal(prodweight.ToString()) * qty).ToString();
                        }
                        dt.Rows.Add(_PackList);
                        if (Packcount == "")
                        {
                            //Packcount = prodname + products1.Rows.Count.ToString();
                            Packcount = prodname + qty.ToString();
                        }
                        else
                        {
                            //Packcount = Packcount + "</br>" + prodname + products1.Rows.Count.ToString();
                            Packcount = Packcount + "</br>" + prodname + qty.ToString();
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
                    //GvPackList.DataSource = resort(dt, "Name", "ASC");
                    //GvPackList.DataBind();

                    string TestName = "";
                    string TestValue1 = "";
                    decimal Testweight = 0;

                    DataTable dtt1 = resort(dt, "Name", "ASC");
                    //if (dtt1.Rows.Count > 0)
                    //{
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
                    DataRow _TestPackListfinalrecord = testdt.NewRow();
                    _TestPackListfinalrecord["Name"] = TestValue1.ToString();
                    _TestPackListfinalrecord["TotalWeight"] = Testweight.ToString();
                    testdt.Rows.Add(_TestPackListfinalrecord);
                    //}
                    //GvPackList2.DataSource = resort(testdt, "Name", "ASC");
                    //GvPackList2.DataBind();

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
                }
            }
            catch (Exception ex)
            {
                //lblerror.Text = errormsg;//ex.ToString();
            }
            return testdt;
        }
        private DataTable API_getotherpack(string ordernumbers)
        {
            DataTable testdt = new DataTable("testPackList");
            testdt.Clear();
            testdt.Columns.Add("Name");
            testdt.Columns.Add("TotalWeight");
            try
            {
                DataTable dt = new DataTable("PackList");
                dt.Clear();
                dt.Columns.Add("Name");
                dt.Columns.Add("Weight");
                dt.Columns.Add("Pack");
                dt.Columns.Add("TotalWeight");



                string Packcount = "";
                string kgcount = "";

                //queryString = "SELECT entity_id, customer_firstname, customer_lastname FROM `sales_flat_order` where created_at between '" + Convert.ToDateTime(TbxFromDate.Text.ToString()).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(TbxToDate.Text.ToString()).ToString("yyyy-MM-dd") + "'";
                queryString = @"SELECT a.pack_name, a.name, a.weight, b.qty_ordered
FROM `1_pack_item_ffhp_product` AS a
INNER JOIN sales_flat_order_item AS b ON a.pack_name = b.name
INNER JOIN `sales_flat_order_grid` as z ON b.order_id=z.entity_id
WHERE z.increment_id in (" + ordernumbers.ToString() + ")";
                //                queryString = @"SELECT a.pack_name, a.name, a.weight
                //FROM `1_pack_item_ffhp_product` AS a
                //LEFT OUTER JOIN sales_flat_order_item AS b ON a.pack_name = b.name
                //WHERE b.order_id in (" + txtidlist.Text + @")
                //
                //union all
                //
                //SELECT distinct a.pack_name, a.name, a.weight
                //FROM `1_pack_item_ffhp_product` AS a
                //LEFT OUTER JOIN sales_flat_order_item AS b ON a.pack_name = b.name
                //WHERE a.name in (
                //
                //SELECT c.title
                //FROM sales_flat_order_item AS a
                //INNER JOIN sales_flat_quote_item_option AS b ON a.quote_item_id = b.item_id
                //INNER JOIN catalog_product_option_type_title AS c ON b.value = c.option_type_id
                //WHERE a.parent_item_id is null and a.product_type='simple' and a.quote_item_id in((select quote_item_id from sales_flat_order_item where order_id in (" + txtidlist.Text + @"))) AND c.title != 'No Need' AND b.option_id NOT IN (SELECT ua.option_id FROM (SELECT y. * FROM sales_flat_order_item AS x INNER JOIN sales_flat_quote_item_option AS y ON x.quote_item_id = y.item_id WHERE x.quote_item_id in((select quote_item_id from sales_flat_order_item where order_id in (" + txtidlist.Text + @") )) LIMIT 2 ) AS A INNER JOIN sales_flat_quote_item_option AS ua ON ua.option_id = A.option_id)) ";
                //Response.Write(queryString);
                if (queryString != "")
                {
                    MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                    DataSet orderlist = new DataSet();
                    adapteradminmail.Fill(orderlist, "sales_flat_order_item");
                    DataTable products = orderlist.Tables[0];
                    products.Merge(API_getotherpackoptions(ordernumbers).Tables[0]);
                    //var distinctRows = (from DataRow dRow in products.Rows
                    //                    select new { col1 = dRow["order_id"], col2 = dRow["dataColumn2"] }).Distinct();
                    var distinctRows = (from DataRow dRow in products.Rows
                                        select new { col1 = dRow["name"], col2 = dRow["weight"] }).Distinct();


                    foreach (var value in distinctRows)
                    {
                        string s = value.col1.ToString();
                        //int ono = Convert.ToInt32(value.col1.ToString());


                        //var ProductRows = (from DataRow dRow in products.Rows
                        //                   where (dRow.Field<string>("product_id") == "64")
                        //                   select products);
                        //var results = (from myRow in products.AsEnumerable()
                        //               where myRow.Field<int>("order_id") == 711
                        //               select new { col1 = myRow["product_id"] });
                        DataTable products1 = null;
                        products1 = products.AsEnumerable().Where(r => Convert.ToString(r["name"]) == s).AsDataView().ToTable();

                        //                        string queryString1 = @"SELECT a.pack_name, a.name, a.weight
                        //FROM `1_pack_item_ffhp_product` AS a
                        //LEFT OUTER JOIN sales_flat_order_item AS b ON a.pack_name = b.name
                        //WHERE b.order_id in (" + txtidlist.Text + ") and a.name='" + s + "'";

                        //                        string queryString1 = @"SELECT a.pack_name, a.name, a.weight
                        //FROM `1_pack_item_ffhp_product` AS a
                        //a.name='" + s + "'";

                        //MySqlDataAdapter adapteradminmail1 = new MySqlDataAdapter(queryString1, conn);

                        //DataSet orderlist1 = new DataSet();
                        //adapteradminmail1.Fill(orderlist1, "sales_flat_order_item");
                        //DataTable products1 = orderlist1.Tables[0];

                        string prodname = "";
                        string prodweight = "0";
                        string qty = "0";
                        int qty1 = 0;
                        string _Ispc = "0";
                        foreach (DataRow row in products1.Rows)
                        {

                            qty1 = qty1 + Convert.ToInt32(Convert.ToDecimal(row["qty_ordered"].ToString()));
                        }
                        foreach (DataRow row in products1.Rows)
                        {
                            //qty = row["qty_ordered"].ToString();
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
                                //pronam = pronam.Substring(pronam.IndexOf("R"), pronam.Length - pronam.IndexOf("R"));
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
                            //int weight = 0;

                            //try
                            //{
                            //    weight = Convert.ToInt32(products1.Rows.Count.ToString()) * Convert.ToInt32(pronam);
                            //}
                            //catch
                            //{
                            //}
                            //if (kgcount == "")
                            //{
                            //    kgcount = prodname + products1.Rows.Count.ToString() + "X" + pronam + " = " + weight;
                            //}
                            //else
                            //{
                            //    kgcount = kgcount + "</br>" + prodname + products1.Rows.Count.ToString() + "X" + pronam + " = " + weight;
                            //}
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

                        //_PackList["Pack"] = products1.Rows.Count.ToString();
                        _PackList["Pack"] = (qty1).ToString();
                        if (_Ispc == "1")
                        {
                            //_PackList["TotalWeight"] = (Convert.ToInt32(Convert.ToDecimal(prodweight.ToString()) * Convert.ToInt32(products1.Rows.Count.ToString()))).ToString();
                            _PackList["TotalWeight"] = (Convert.ToInt32(Convert.ToDecimal(prodweight.ToString()) * qty1)).ToString();
                        }
                        else
                        {
                            //_PackList["TotalWeight"] = (Convert.ToDecimal(prodweight.ToString()) * Convert.ToInt32(products1.Rows.Count.ToString())).ToString();
                            _PackList["TotalWeight"] = (Convert.ToDecimal(prodweight.ToString()) * qty1).ToString();
                        }
                        dt.Rows.Add(_PackList);
                        if (Packcount == "")
                        {
                            //Packcount = prodname + products1.Rows.Count.ToString();
                            Packcount = prodname + (qty1).ToString();
                        }
                        else
                        {
                            //Packcount = Packcount + "</br>" + prodname + products1.Rows.Count.ToString();
                            Packcount = Packcount + "</br>" + prodname + (qty1).ToString();
                        }


                    }
                    //GvPackList.DataSource = resort(dt, "Name", "ASC");
                    //GvPackList.DataBind();
                    string TestName = "";
                    string TestValue1 = "";
                    decimal Testweight = 0;

                    DataTable dtt1 = resort(dt, "Name", "ASC");
                    if (dtt1.Rows.Count > 0)
                    {
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
                        DataRow _TestPackListfinalrecord = testdt.NewRow();
                        _TestPackListfinalrecord["Name"] = TestValue1.ToString();
                        _TestPackListfinalrecord["TotalWeight"] = Testweight.ToString();
                        testdt.Rows.Add(_TestPackListfinalrecord);
                    }

                    //string[] myColors = txtidlist.Text.ToString().Split(',');
                    //foreach (string ite in myColors)
                    //{
                    //    string sits = ite.ToString();
                    //    //Console.WriteLine(num);

                    //    DataTable dtt2 = getotherpackoptions(ite.ToString());
                    //    if (dtt2.Rows.Count > 0)
                    //    {
                    //        for (int i = 0; i < dtt2.Rows.Count; i++)
                    //        {
                    //            DataRow _TestPackListfinalrecord = testdt.NewRow();
                    //            _TestPackListfinalrecord["Name"] = dtt2.Rows[i]["Name"].ToString();
                    //            _TestPackListfinalrecord["TotalWeight"] = Convert.ToDecimal(dtt2.Rows[i]["TotalWeight"].ToString());
                    //            testdt.Rows.Add(_TestPackListfinalrecord);
                    //        }
                    //    }
                    //}
                    //GvPackList2.DataSource = resort(testdt, "Name", "ASC");
                    //GvPackList2.DataBind();
                }

            }
            catch (Exception ex)
            {
                //lblerror.Text = errormsg;//ex.ToString();
            }
            return testdt;
        }
        public DataTable API_getsmswithtype(DataTable dtcollection)
        {
            DataTable testdt = new DataTable("testsmslist");
            testdt.Clear();
            testdt.Columns.Add("name");
            testdt.Columns.Add("tanglish_name");
            testdt.Columns.Add("type");
            testdt.Columns.Add("weight");
            testdt.Columns.Add("units");

            DataTable dtmapping = new DataTable();
            queryString = @"SELECT * FROM `1_sms_ffhp_mapping`";
            //Response.Write(queryString);
            if (queryString != "")
            {
                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                DataSet orderlist = new DataSet();
                adapteradminmail.Fill(orderlist, "smsmapping1");

                dtmapping = resort(orderlist.Tables[0], "name", "ASC");
                //DataTable filteredDT = (from t in dt.AsEnumerable()
                //                        where t.Field<string>("Name").Equals("Ash Gourds")
                //                        select t).CopyToDataTable();
                //var query = from t in dt.AsEnumerable()
                //            where t.Field<string>("name").Equals("Ash Gourd")
                //            select t;

                //DataTable filteredDT = null;
                //if (query != null && query.Count() > 0)
                //{
                //    filteredDT = query.CopyToDataTable();
                //}
            }
            string newsms = "";
            //string testsms = lblsmsformat.Text.ToString();
            string mapname = "";

            //foreach (GridViewRow itm in GvPackList3.Rows)
            foreach(DataRow itm in dtcollection.Rows)
            {

                string sits = itm["Name"].ToString();//itm.Cells[1].Text.ToString();
                string weight = (Convert.ToDouble(itm["TotalWeight"].ToString())).ToString();
//(Convert.ToDouble(itm.Cells[2].Text.ToString())).ToString();

                var query = from t in dtmapping.AsEnumerable()
                            where t.Field<string>("name").Equals(sits.Trim())
                            select t;

                DataTable filteredDT = null;
                DataRow _SmsList = testdt.NewRow();
                if (query != null && query.Count() > 0)
                {
                    filteredDT = query.CopyToDataTable();

                    mapname = filteredDT.Rows[0]["tanglish_name"].ToString();
                    //testsms = testsms.Replace(sits, mapname);

                    _SmsList["name"] = filteredDT.Rows[0]["name"].ToString();
                    if (filteredDT.Rows[0]["tanglish_name"].ToString() != "")
                    {
                        _SmsList["tanglish_name"] = filteredDT.Rows[0]["tanglish_name"].ToString();
                        //testsms = testsms.Replace(sits, filteredDT.Rows[0]["tanglish_name"].ToString());
                    }
                    else
                    {
                        _SmsList["tanglish_name"] = filteredDT.Rows[0]["name"].ToString();
                    }
                    _SmsList["type"] = filteredDT.Rows[0]["type"].ToString();
                    _SmsList["weight"] = weight.ToString();
                    _SmsList["units"] = filteredDT.Rows[0]["units"].ToString();

                }
                else
                {
                    _SmsList["name"] = sits;
                    _SmsList["tanglish_name"] = sits;
                    _SmsList["type"] = "";
                    _SmsList["weight"] = weight.ToString();
                    _SmsList["units"] = "";
                }
                testdt.Rows.Add(_SmsList);
                //if (mapname.Trim() == "")
                //{
                //    mapname = sits;
                //}

                //if (newsms != "")
                //{
                //    newsms = newsms + "/" + sits;
                //}
                //else
                //{
                //    newsms = mapname;
                //}
            }
            DataTable smsdt = new DataTable("smslist");
            smsdt = resort(testdt, "type", "ASC");

            //string testtype = "0";
            //foreach (DataRow dtrow in smsdt.Rows)
            //{
            //    dtrow["tanglish_name"].ToString();
            //    dtrow["weight"].ToString();
            //    dtrow["type"].ToString();
            //    dtrow["units"].ToString();

            //    if (testtype == "0")
            //    {
            //        newsms = dtrow["tanglish_name"].ToString() + " " + dtrow["weight"].ToString() + " " + dtrow["units"].ToString();
            //    }
            //    else if (testtype == dtrow["type"].ToString())
            //    {
            //        if (newsms == "")
            //        {
            //            newsms = dtrow["tanglish_name"].ToString() + " " + dtrow["weight"].ToString() + " " + dtrow["units"].ToString();
            //        }
            //        else
            //        {
            //            newsms = newsms + System.Environment.NewLine + dtrow["tanglish_name"].ToString() + " " + dtrow["weight"].ToString() + " " + dtrow["units"].ToString();
            //        }
            //    }
            //    else
            //    {
            //        newsms = newsms + System.Environment.NewLine + System.Environment.NewLine;
            //        if (newsms == "")
            //        {
            //            newsms = dtrow["tanglish_name"].ToString() + " " + dtrow["weight"].ToString() + " " + dtrow["units"].ToString();
            //        }
            //        else
            //        {
            //            newsms = newsms + System.Environment.NewLine + dtrow["tanglish_name"].ToString() + " " + dtrow["weight"].ToString() + " " + dtrow["units"].ToString();
            //        }
            //    }
            //    testtype = dtrow["type"].ToString();
            //}
            //return "Old Format" + System.Environment.NewLine + testsms + System.Environment.NewLine + System.Environment.NewLine + "New Format" + System.Environment.NewLine + newsms;
            //return newsms;
            return smsdt;
        }
        public DataSet API_getotherpackoptions(string ordernumbers)
        {
            DataSet orderlist = new DataSet();
            try
            {

                queryString = @"SELECT distinct d.pack_name,d.name, d.weight, a.qty_ordered
from `sales_flat_order_grid` as z inner join `sales_flat_order_item` AS a
ON z.entity_id=a.order_id

inner join sales_flat_quote_item_option as b on a.quote_item_id = b.item_id

inner join catalog_product_option_type_title AS c ON b.value = c.option_type_id

inner join 1_pack_item_ffhp_product as d on d.name=c.title

where a.parent_item_id is null and a.product_type='simple'

and c.title !='No Need' and d.pack_name like '%(Optional)'

and z.increment_id in (" + ordernumbers.ToString() + ")";
                //Response.Write(queryString);
                if (queryString != "")
                {
                    MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);


                    adapteradminmail.Fill(orderlist, "otherpackoptions");

                }

            }
            catch (Exception ex)
            {
            }
            return orderlist;
        }
    }
}
