//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using MySql.Data.MySqlClient;
//using System.Data;
//using System.IO;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using iTextSharp.text.html.simpleparser;
////using System.Web.Mail;
//using System.Net.Mail;
//using iTextSharp.tool.xml;
//using Excel = Microsoft.Office.Interop.Excel;
//using System.Text.RegularExpressions;
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
    public partial class RouteOrder : System.Web.UI.Page
    {
        //string conn = "server=68.178.143.107;userid=stagingffhpin;password=Stag1ngffHP!n;database=stagingffhpin;Convert Zero Datetime=True";  //staging
        //string conn = "server=68.178.143.11;userid=ffhpmagento;password=D6QpT!KDd0dKHI;database=ffhpmagento;Convert Zero Datetime=True";  //Live
        //string conn = "server=192.168.1.2;userid=;password=;database=ffhp;Convert Zero Datetime=True";  //Local
        string conn = System.Configuration.ConfigurationManager.AppSettings["Connection"].ToString();
        string smsconn = System.Configuration.ConfigurationManager.AppSettings["SmsDBConnection"].ToString();//"server=68.178.143.39;userid=ffhplog;password=FFHPl0g!;database=ffhplog;Convert Zero Datetime=True";  //sms connection
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
                            Binddropdownroute();
                            //Binddropdowndriver_deliveryboy();
                        }
                        else
                        {
                            Response.Redirect("Login.aspx", false);
                        }
                        bindclearlist();
                    }
                }
                else
                {
                    Response.Redirect("Login.aspx", false);
                }
                
            }
            catch (Exception ex)
            {
                lblerror.Text = "Error:"+ex.ToString();
                //Response.Redirect("Login.aspx", false);
            }
        }
        public void bindclearlist()
        {
            string sum="";
            DataSet ordenumberlist = new DataSet();
            ordenumberlist = getroutelistnew();
            if (ordenumberlist.Tables.Count > 0)
            {
                if (ordenumberlist.Tables[0].Rows.Count > 0)
                {
                    sum = ordenumberlist.Tables[0].AsEnumerable().Sum(x => x.Field<decimal>("count")).ToString();
                }
            }

            GVRouteClear.DataSource = ordenumberlist;
            GVRouteClear.DataBind();
//            ((Label)GVRouteClear.FooterRow.FindControl("lbltotoal")).Text = sum;
            GVRouteClear.FooterRow.Cells[2].Text="Total :";
            GVRouteClear.FooterRow.Cells[3].Text = sum;
        }
        public DataSet getroutelist()
        {
            string queryString = "";
            DataSet routelist = new DataSet();
            queryString = @"SELECT * FROM `ffhp_route_orders`";
            if (queryString != "")
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                MySqlConnection con = new MySqlConnection(smsconn);
                con.Open();

                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, smsconn);

                adapteradminmail.Fill(routelist, "RouteName");
                con.Close();
                //sshobj.SshDisconnect(client);;
            }
            return routelist;
        }
        public DataSet getroutecurrentlist()
        {
            string queryString = "";
            DataSet routelist = new DataSet();
            queryString = @"SELECT route_id,route_name FROM `ffhp_route_orders` where ordernumber is not null and ordernumber!=''";
            if (queryString != "")
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                MySqlConnection con = new MySqlConnection(smsconn);
                con.Open();

                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, smsconn);

                adapteradminmail.Fill(routelist, "RouteName");
                con.Close();
                //sshobj.SshDisconnect(client);
            }
            return routelist;
        }
        public DataSet getroutelistone(int routeid)
        {
            string queryString = "";
            DataSet ordenumberlist = new DataSet();
            queryString = @"SELECT route_id,route_name,ordernumber,driver_name,driver_phone,deliveryboy_name,deliveryboy_phone FROM `ffhp_route_orders` where route_id=" + routeid;
            if (queryString != "")
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();

                MySqlConnection con = new MySqlConnection(smsconn);
                con.Open();

                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, smsconn);

                adapteradminmail.Fill(ordenumberlist, "RouteName");
                con.Close();
                //sshobj.SshDisconnect(client);;
            }
            return ordenumberlist;
        }
        public DataSet getroutelistnew()
        {
            string queryString = "";
            DataSet ordenumberlist = new DataSet();
            queryString = @"SELECT route_id,route_name,ordernumber,driver_name,driver_phone,deliveryboy_name,deliveryboy_phone,latitude,longitude,ROUND (  (LENGTH(ordernumber)- LENGTH( REPLACE ( ordernumber, ',', '') ) ) / LENGTH(',') )+1 AS count FROM `ffhp_route_orders` where ordernumber is not null and ordernumber!=''";
            if (queryString != "")
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();

                MySqlConnection con = new MySqlConnection(smsconn);
                con.Open();

                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, smsconn);

                adapteradminmail.Fill(ordenumberlist, "RouteName");
                con.Close();
                //sshobj.SshDisconnect(client);;
                 var sum = ordenumberlist.Tables[0].AsEnumerable().Sum(x=>x.Field<decimal>("count"));
            }
            return ordenumberlist;
        }
        public int set_latitude_longitude(int routeid,string latitude,string longitude)
        {
            int i = 0;
            try
            {
                queryString = @"update `ffhp_route_orders` set latitude='" + latitude + "', longitude='" + longitude + "' where route_id=" + routeid + "";
                if (queryString != "")
                {
                    //ConnectSsh sshobj = new ConnectSsh();
                    //SshClient client = sshobj.SshConnect();

                    MySqlConnection con = new MySqlConnection(smsconn);
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(queryString, con);
                    i = cmd.ExecuteNonQuery();
                    con.Close();
                    insert_latitude_longitude_history_transaction(routeid, latitude, longitude);
                    //sshobj.SshDisconnect(client);
                }

            }
            catch (Exception ex)
            {
            }
            return i;
        }
        public int insert_latitude_longitude_history_transaction(int routeid, string latitude, string longitude)
        {
            int i = 0;
            MySqlConnection con = new MySqlConnection(smsconn);
            con.Open();
            try
            {
                DateTime CDT = DateTime.Now.Date;
                queryString = @"select rhid from ffhp_route_orders_history where route_id='" + routeid.ToString().Trim() + "' and updateddate between '" + CDT.ToString("yyyy-MM-dd") + "' and '" + CDT.AddDays(1).ToString("yyyy-MM-dd") + "'";
                MySqlCommand cmd = new MySqlCommand(queryString, con);
                string rhid = cmd.ExecuteScalar().ToString();
                if (rhid != "")
                {
                    queryString = @"insert into `ffhp_route_orders_history_transaction` (rhid,latitude,longitude) values("+rhid+",'" + latitude + "', '" + longitude + "')";

                    MySqlCommand cmdinsert = new MySqlCommand(queryString, con);
                    i = cmdinsert.ExecuteNonQuery();
                    
                }
                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
            }
            return i;
        }
        public DataTable get_latitude_longitude(int route_id, DateTime updateddate)
        {
            DateTime CDT = updateddate;
            string queryString = "";
            DataTable route_lat_lon = new DataTable("Route_lat_lon");
            queryString = @"SELECT * FROM `ffhp_route_orders_history` as a inner join
`ffhp_route_orders_history_transaction` as b on a.rhid=b.rhid and a.updateddate between '"+CDT.ToString("yyyy-MM-dd")+"' and '"+CDT.AddDays(1).ToString("yyyy-MM-dd")+"' WHERE a.route_id="+route_id+"";
            if (queryString != "")
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();

                MySqlConnection con = new MySqlConnection(smsconn);
                con.Open();

                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, smsconn);

                adapteradminmail.Fill(route_lat_lon);
                con.Close();
                //sshobj.SshDisconnect(client);
            }
            return route_lat_lon;
        }
        public void Binddropdownroute()
        {
            ddlroutelist.Items.Clear();
            ddlroutelist.Items.Add("-Select-");
            ddlroutelist.DataSource = getroutelist();
            ddlroutelist.DataTextField = "route_name";
            ddlroutelist.DataValueField = "route_id";
            ddlroutelist.DataBind();
        }
        public void Binddropdowndriver_deliveryboy()
        {
            DataSet ds = new DataSet();
            ds = getdriver_deliveryboylist();

            DataTable DTdriver = new DataTable();
            var result = from r in ds.Tables[0].AsEnumerable()
                         where r["designation"].ToString() == "Driver"
                         select r;
            DTdriver = result.CopyToDataTable();

            DataTable DTdeliveryboy = new DataTable();
            var resultdeliveryboy = from r in ds.Tables[0].AsEnumerable()
                                    where r["designation"].ToString() == "Delivery Boy"
                                    select r;
            DTdeliveryboy = resultdeliveryboy.CopyToDataTable();
            
            ddldriver.Items.Clear();
            ddldriver.Items.Add("-Select-");
            ddldriver.DataSource = DTdriver;
            ddldriver.DataTextField = "name_phone";
            ddldriver.DataValueField = "ddid";
            ddldriver.DataBind();

            ddldeliveryboy.Items.Clear();
            ddldeliveryboy.Items.Add("-Select-");
            ddldeliveryboy.DataSource = DTdeliveryboy;
            ddldeliveryboy.DataTextField = "name_phone";
            ddldeliveryboy.DataValueField = "ddid";
            ddldeliveryboy.DataBind();
        }
        public DataSet getdriver_deliveryboylist()
        {
            string queryString = "";
            DataSet driver_deliveryboy_list = new DataSet();
            queryString = @"SELECT ddid,name,phone,designation,CONCAT( IFNULL( name, '' ) , '#', IFNULL( phone, '' ) ) AS name_phone FROM `driver_deliveryboy_details`";
            if (queryString != "")
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();

                MySqlConnection con = new MySqlConnection(smsconn);
                con.Open();

                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, smsconn);

                adapteradminmail.Fill(driver_deliveryboy_list, "driver_deliveryboy");
                con.Close();
                //sshobj.SshDisconnect(client);;
            }
            
            return driver_deliveryboy_list;
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
                    con.Close();
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
                    //sshobj.SshDisconnect(client);;
                }
            }
            catch (Exception ex)
            {
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
        protected void CBorder_OnCheckedChanged(object sender, EventArgs e)
        {
            if (ddlroutelist.SelectedIndex != 0)
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
            else
            {
                lblerror.Text = "Please select route";
            }

        }
        protected void CBorderall_OnCheckedChanged(object sender, EventArgs e)
        {
            if (ddlroutelist.SelectedIndex != 0)
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
            else
            {
                lblerror.Text = "Please select route";
            }
        }
        protected void btnsave_OnClick(object sender, EventArgs e)
        {
            string _driver_name, _driver_phone, _deliveryboy_name, _deliveryboy_phone = "";

            if (ddlroutelist.SelectedIndex != 0)
            {
                //if (ddldriver.SelectedIndex != 0)
                //{
                //    string[] driver=ddldriver.SelectedItem.Text.ToString().Split('#');
                //    _driver_name = driver[0].ToString();
                //    _driver_phone = driver[1].ToString();
                //    if (ddldeliveryboy.SelectedIndex != 0)
                //    {
                //        string[] deliveryboy = ddldeliveryboy.SelectedItem.Text.ToString().Split('#');
                //        _deliveryboy_name = deliveryboy[0].ToString();
                //        _deliveryboy_phone = deliveryboy[1].ToString();

                        //queryString = @"update `ffhp_route_orders` set ordernumber='" + txtidlist.Text.ToString() + "', driver_name='" + _driver_name + "', driver_phone='" + _driver_phone + "', deliveryboy_name='" + _deliveryboy_name + "', deliveryboy_phone='" + _deliveryboy_phone + "' where route_id=" + ddlroutelist.SelectedValue.ToString() + "";
                        queryString = @"update `ffhp_route_orders` set ordernumber='" + txtidlist.Text.ToString() + "' where route_id=" + ddlroutelist.SelectedValue.ToString() + "";
                        if (queryString != "")
                        {
                            //ConnectSsh sshobj = new ConnectSsh();
                            //SshClient client = sshobj.SshConnect();

                            MySqlConnection con = new MySqlConnection(smsconn);
                            con.Open();
                            MySqlCommand cmd = new MySqlCommand(queryString, con);
                            int i = cmd.ExecuteNonQuery();
                            con.Close();
                            lblerror.Text = "Save Successfully";
                            //sshobj.SshDisconnect(client);
                            bindclearlist();
                        }
                //    }
                //    else
                //    {
                //        lblerror.Text = "Please select Delivery Boy";
                //    }
                //}
                //else
                //{
                //    lblerror.Text = "Please select Driver";
                //}
            }
            else
            {
                lblerror.Text = "Please select route";
            }
        }
        protected void ddlroutelist_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet ordernumberlist = new DataSet();
            string ordernumber = "";
            if (ddlroutelist.SelectedIndex != 0)
            {
                ordernumberlist=getroutelistone(Convert.ToInt32(ddlroutelist.SelectedValue.ToString()));
            }
            if (ordernumberlist.Tables.Count > 0)
            {
                if (ordernumberlist.Tables[0].Rows.Count > 0)
                {
                    ordernumber = ordernumberlist.Tables[0].Rows[0]["ordernumber"].ToString();
                    //ddldriver.SelectedIndex = ddldriver.Items.IndexOf(ddldriver.Items.FindByText(ordernumberlist.Tables[0].Rows[0]["driver_name"].ToString() + "#" + ordernumberlist.Tables[0].Rows[0]["driver_phone"].ToString()));
                    //ddldeliveryboy.SelectedIndex = ddldeliveryboy.Items.IndexOf(ddldeliveryboy.Items.FindByText(ordernumberlist.Tables[0].Rows[0]["deliveryboy_name"].ToString() + "#" + ordernumberlist.Tables[0].Rows[0]["deliveryboy_phone"].ToString()));
                }
            }
            if (ordernumber != "")
            {
                txtidlist.Text = ordernumber;
                //string[] ordernumberprintlist = ordernumber.Split(',');
                //var te=ordernumberprintlist.Select(a => new { data = a }).ToList().AsEnumerable();

                //dlordernumber.DataSource = ordernumberprintlist.Select(a => new { data = a }).ToList().AsEnumerable();
                //dlordernumber.DataBind();
            }
            else
            {
                txtidlist.Text = "";
            }
            
        }
        protected void btnpdf_OnClick(object sender, EventArgs e)
        {
            generatepdf1();
        }
        public void generatepdf()
        {
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            dlordernumber.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=OrderPrintView.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End(); 
        }
        public void generatepdf1()
        {
            //dlordernumber.AllowPaging = false;
            //GridView1.DataBind();
            string s = Server.MapPath("Images/Calibri.ttf");
            BaseFont bf = BaseFont.CreateFont(s, BaseFont.IDENTITY_H, true);
            //BaseFont bf = BaseFont.CreateFont(Environment.GetEnvironmentVariable("windir") + @"\fonts\CALIBRI.TTF", BaseFont.IDENTITY_H, true);


            iTextSharp.text.pdf.PdfPTable table = new iTextSharp.text.pdf.PdfPTable(dlordernumber.RepeatColumns);
            int[] widths = new int[dlordernumber.RepeatColumns];
            for (int x = 0; x < dlordernumber.RepeatColumns; x++)
            {
                widths[x] = (int)dlordernumber.ItemStyle.Width.Value;
                //string cellText = Server.HtmlDecode(GvPackList3.HeaderRow.Cells[x].Text);

                //Set Font and Font Color
                iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 11, iTextSharp.text.Font.NORMAL);
                //font.Color = new Color(GVOrderDetails2.HeaderStyle.ForeColor);
                //font.Color = new Color(GVOrderDetails2.RowStyle.ForeColor);
                iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(new Phrase(12, "test", font));

                //Set Header Row BackGround Color
                //cell.BackgroundColor = new Color(GVOrderDetails2.HeaderStyle.BackColor);


                table.AddCell(cell);
            }
            table.SetWidths(widths);

            for (int i = 0; i < dlordernumber.Items.Count; i++)
            {
                
                    for (int j = 0; j < dlordernumber.RepeatColumns; j++)
                    {
                        string cellText = "";
                        if (j != 0)
                        {
                            Label ordno = dlordernumber.Items[i].FindControl("lblordernumber") as Label;
                            string item = ordno.Text;
                            cellText = Server.HtmlDecode(item);
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

            //Create the PDF Document
            Document pdfDoc = new Document(PageSize.A4, 10f, 200f, 10f, 50f);
            //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            PdfWriter pdfwriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfwriter.PageEvent = new Footer(Server.MapPath("Images/Calibri.ttf"), 40);
            pdfDoc.Open();
            pdfDoc.Add(table);
            pdfDoc.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=OrderNumberPrint.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();
        }
        public int Insert_Delivery_Status(string querycount,string query)
        {
            string queryString = "";
            int i = 0;
            queryString = query;
            if (queryString != "")
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                MySqlConnection con = new MySqlConnection(smsconn);
                con.Open();
                MySqlCommand cmdcount = new MySqlCommand(querycount, con);
                int j=Convert.ToInt32(cmdcount.ExecuteScalar());
                if (j == 0)
                {
                    MySqlCommand cmd = new MySqlCommand(queryString, con);
                    i = cmd.ExecuteNonQuery();
                }
                con.Close();
                //sshobj.SshDisconnect(client);
            }
            return i;
        }
        public int Delete_Delivery_Status(string deletequery)
        {
            string queryString = "";
            int i = 0;
            queryString = deletequery;
            if (queryString != "")
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                MySqlConnection con = new MySqlConnection(smsconn);
                con.Open();
                MySqlCommand cmd = new MySqlCommand(queryString, con);
                i = cmd.ExecuteNonQuery();
                con.Close();
                //sshobj.SshDisconnect(client);
            }
            return i;
        }
        public int Get_Delivery_Status_Flag(string selectcountquery)
        {
            string queryString = "";
            int i = 0;
            queryString = selectcountquery;
            if (queryString != "")
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                MySqlConnection con = new MySqlConnection(smsconn);
                con.Open();
                MySqlCommand cmdcount = new MySqlCommand(queryString, con);
                i = Convert.ToInt32(cmdcount.ExecuteScalar());
                con.Close();
                //sshobj.SshDisconnect(client);
            }
            return i;
        }
        public int Insert_Delivery_Detail(string query)
        {
            string queryString = "";
            int i = 0;
            queryString = query;
            if (queryString != "")
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                MySqlConnection con = new MySqlConnection(smsconn);
                con.Open();
                MySqlCommand cmd = new MySqlCommand(queryString, con);
                i = cmd.ExecuteNonQuery();
                con.Close();
                //sshobj.SshDisconnect(client);
            }
            return i;
        }
        public DataSet Get_Delivery_Detail(DateTime fromdate,DateTime todate)
        {
            string queryString = "";
            DataSet ffhp_delivery_details = new DataSet();
            queryString = @"SELECT delivery_detail_id,
route_name,
ordernumber,
order_status,
amount,
collection_amount,
reduction,
balance,TIME_TO_SEC(delivery_starttime)as
delivery_starttime,
TIME_TO_SEC(delivery_closetime) as delivery_closetime,
kilometer,
driver_name,
driver_phone,
deliveryboy_name,
deliveryboy_phone,
status,
TIME_TO_SEC(start_close_time)as start_close_time,
latitude,longitude,description,
DATE_FORMAT( lastupdatedate, '%m/%d/%Y' )as lastupdatedate 
FROM `ffhp_delivery_details` where lastupdatedate between '" + String.Format("{0:yyyy/MM/dd}", fromdate)+"' and '"+String.Format("{0:yyyy/MM/dd}", todate)+"'";
            if (queryString != "")
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                MySqlConnection con = new MySqlConnection(smsconn);
                con.Open();

                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, smsconn);

                adapteradminmail.Fill(ffhp_delivery_details, "ffhp_delivery_details");
                con.Close();
                //sshobj.SshDisconnect(client);
            }

            return ffhp_delivery_details;
        }

        protected void btnclear_OnClick(object sender, EventArgs e)
        {
            //GridViewRow row = (GridViewRow)((Control)sender).NamingContainer;
            //GridViewRow row = (GridViewRow)(((Control)sender).NamingContainer.FindControl("HFrouteid"));
            
            //Label lblCode = (Label)GVRouteClear.Rows(rowindex).FindControl("label1");

            GridViewRow row = (GridViewRow)(((Control)sender).NamingContainer);
            string routeid= ((HiddenField)row.FindControl("HFrouteid")).Value.ToString();

            //queryString = @"update `ffhp_route_orders` set ordernumber='', driver_name='', driver_phone='', deliveryboy_name='', deliveryboy_phone='',latitude='',longitude='' where route_id=" + routeid + "";
            queryString = @"update `ffhp_route_orders` set ordernumber='',latitude='',longitude='' where route_id=" + routeid + "";
            if (queryString != "")
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();

                MySqlConnection con = new MySqlConnection(smsconn);
                con.Open();
                MySqlCommand cmd = new MySqlCommand(queryString, con);
                int i = cmd.ExecuteNonQuery();
                con.Close();
                //sshobj.SshDisconnect(client);;
                if (i > 0)
                {
                    lblerror.Text = "Update Successfully";
                    bindclearlist();
                }
                else
                {
                    lblerror.Text = errormsg;
                }
                
            }
        }
        
    }
    
}
