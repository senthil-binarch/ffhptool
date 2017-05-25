using System;
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
    public partial class alacartewithNames : System.Web.UI.Page
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
                        //getOrderDetails();
                        Getalacartewithnames();
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
        public void Getalacartewithnames()
        {
            try
            {
                if (txtidlist.Text != "")
                {
                    //queryString = "SELECT entity_id, customer_firstname, customer_lastname FROM `sales_flat_order` where created_at between '" + Convert.ToDateTime(TbxFromDate.Text.ToString()).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(TbxToDate.Text.ToString()).ToString("yyyy-MM-dd") + "'";
//                    queryString = @"SELECT z.increment_id AS entity_id,  CONCAT( IFNULL( customer_firstname, '' ) , ' ', IFNULL( a.customer_lastname, '' ) ) AS customername, b.Name, CAST((b.weight*b.qty_ordered) AS DECIMAL(12,3)) as weight, '' as units,b.weight as testweight
//FROM `sales_flat_order_grid` AS z
//INNER JOIN `sales_flat_order` AS a ON z.entity_id = a.entity_id
//INNER JOIN `sales_flat_order_item` AS b ON a.entity_id = b.order_id
//WHERE b.parent_item_id IS NULL 
//AND b.sku LIKE 'a-la%'
//AND product_options NOT LIKE '%Additional Qty%'
//AND z.increment_id in (" + txtidlist.Text + ")";
                    queryString = @"SELECT DISTINCT z.increment_id AS entity_id,  CONCAT( IFNULL( c.firstname, '' ) , ' ', IFNULL( c.lastname, '' ) ) AS customername, b.Name, CAST((b.weight*b.qty_ordered) AS DECIMAL(12,3)) as weight, '' as units,b.weight as testweight,b.product_id,b.base_row_total_incl_tax as amount,a.subtotal,a.discount_amount,a.grand_total,IF(STRCMP(a.status,'pending'),'EBS','COD') as order_status
FROM `sales_flat_order_grid` AS z
INNER JOIN `sales_flat_order` AS a ON z.entity_id = a.entity_id
INNER JOIN `sales_flat_order_item` AS b ON a.entity_id = b.order_id
INNER JOIN `sales_flat_order_address` AS c ON c.parent_id = b.order_id AND address_type = 'shipping'
WHERE b.parent_item_id IS NULL 
AND b.sku LIKE 'a-la%'
AND product_options NOT LIKE '%Additional Qty%'
AND z.increment_id in (" + txtidlist.Text + ")";
                    //Response.Write(queryString);
                    if (queryString != "")
                    {
                        //ConnectSsh sshobj = new ConnectSsh();
                        //SshClient client=sshobj.SshConnect();
                        MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                        DataSet orderlist = new DataSet();
                        adapteradminmail.Fill(orderlist, "sales_flat_order_item");
                        DataTable products = orderlist.Tables[0];
                        foreach (DataRow row in orderlist.Tables[0].Rows)
                        {
                            if (Convert.ToDecimal(row["testweight"]) < 1)
                            {
                                row["units"] = "kg";
                            }
                            else
                            {
                                //row["units"] = "Piece/Bundle";
                                row["units"] = "Pc";
                            }
                        }
                        //var distinctRows = (from DataRow dRow in products.Rows
                        //                    select new { col1 = dRow["increment_id"] }).Distinct();
                        //foreach (var value in distinctRows)
                        //{

                        //}
                        DataSet dttest = new DataSet();
                        dttest = getoptions();
                        if (dttest.Tables.Count > 0)
                        {
                            products.Merge(dttest.Tables[0]);
                        }
                        products = resort(products, "entity_id,Name", "ASC");

                        DataTable LPRData = new DataTable();
                        TotalWeightlprdata objlprdata = new TotalWeightlprdata();
                        LPRData = objlprdata.getffhpproducts();


                        //var JoinResult = (from p in products.AsEnumerable()
                        //                  join t in LPRData.AsEnumerable()
                        //                  on p.Field<int>("product_id") equals t.Field<int>("productid")
                        //                  select p); 


                        //var distinctRows = (from DataRow dRow in products.Rows
                        //                    select new { entity_id = dRow["entity_id"], customername = dRow["customername"] }).Distinct();
                        var distinctRows = (from DataRow dRow in products.Rows
                                            select new { entity_id = dRow["entity_id"], customername = dRow["customername"], subtotal = dRow["subtotal"], discount_amount = dRow["discount_amount"], grand_total = dRow["grand_total"], order_status = dRow["order_status"] }).Distinct();
                        GVAlacarte.DataSource = distinctRows;
                        GVAlacarte.DataBind();
                        DataTable dtfree = new DataTable();
                        dtfree = getfree();
                        
                        foreach (GridViewRow row in GVAlacarte.Rows)
                        {
                            int ono = Convert.ToInt32(((Label)row.FindControl("lblorderid")).Text.ToString());
                            
                            DataTable products1 = null;
                            
                                products1 = products.AsEnumerable().Where(r => Convert.ToInt32(r["entity_id"]) == ono).AsDataView().ToTable();
                                //for add product group
                                products1.Columns.Add("product_group");

                                foreach (DataRow productrow in products1.Rows)
                                {
                                    DataTable LPRData1 = new DataTable();
                                    LPRData1 = LPRData.AsEnumerable().Where(r => Convert.ToInt32(r["productid"]) == Convert.ToInt32(productrow["product_id"].ToString())).AsDataView().ToTable();
                                    if (LPRData1.Rows.Count > 0)
                                    {
                                        productrow["product_group"] = LPRData1.Rows[0]["group"].ToString();
                                    }
                                }
                                products1 = resort(products1, "product_group", "ASC");
                                if (((Label)row.FindControl("lblgrandtotal")).Text.ToString() != "0.00")
                                {
                                    DataRow rfree = products1.NewRow();

                                    if (dtfree.Rows.Count > 0)
                                    {
                                        DataTable LPRData1 = new DataTable();
                                        LPRData1 = LPRData.AsEnumerable().Where(r => Convert.ToInt32(r["productid"]) == Convert.ToInt32(dtfree.Rows[0]["product_id"].ToString())).AsDataView().ToTable();


                                        rfree["entity_id"] = "";
                                        rfree["customername"] = "";
                                        rfree["Name"] = dtfree.Rows[0]["name"].ToString();
                                        rfree["weight"] = Convert.ToDecimal(dtfree.Rows[0]["weight"]);
                                        rfree["units"] = dtfree.Rows[0]["units"];
                                        rfree["testweight"] = Convert.ToDecimal(0);
                                        rfree["product_id"] = dtfree.Rows[0]["product_id"];
                                        rfree["amount"] = Convert.ToDecimal(0);
                                        rfree["subtotal"] = Convert.ToDecimal(0);
                                        rfree["discount_amount"] = Convert.ToDecimal(0);
                                        rfree["product_group"] = "";
                                        //if (LPRData1.Rows.Count > 0)
                                        //{
                                        //    rfree["product_group"] = LPRData1.Rows[0]["weightgroup"].ToString();
                                        //}
                                        //else
                                        //{
                                        //    rfree["product_group"] = "";
                                        //}
                                        products1.Rows.Add(rfree);
                                    }
                                }
                                
                            ((GridView)row.FindControl("GvalacartewithNameList")).DataSource = products1;
                            ((GridView)row.FindControl("GvalacartewithNameList")).DataBind();
                        }
                        if (products.Rows.Count > 0)
                        {
                            btnPDF.Visible = true;
                            btnXLS.Visible = true;
                        }
                        else
                        {
                            btnPDF.Visible = false;
                            btnXLS.Visible = false;
                        }

                        //GvPackList3.DataSource = resort(products, "entity_id,Name", "ASC");
                        //GvPackList3.DataBind();
                        //sshobj.SshDisconnect(client);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg+" "+ex.ToString();
            }

        }
        public DataTable getfree()
        {
            DataTable dtfree = new DataTable();
            queryString = "select * from 1_ffhp_free";
            if (queryString != "")
            {
                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);
                adapteradminmail.Fill(dtfree);
            }
            return dtfree;
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
            dt.Columns.Add("amount");
            dt.Columns.Add("subtotal");
            dt.Columns.Add("discount_amount");
            dt.Columns.Add("grand_total");
            dt.Columns.Add("order_status");

            DataSet orderoptionlist = new DataSet();
            try
            {

                queryString = @"SELECT z.increment_id AS entity_id,  CONCAT( IFNULL( ca.firstname, '' ) , ' ', IFNULL( ca.lastname, '' ) ) AS customername, b.order_id, b.product_id, b.Name, CAST((b.weight * b.qty_ordered) AS DECIMAL(12,3)) as weight, round(b.qty_ordered) As qty_ordered, value,'' as units,b.base_row_total_incl_tax as amount,a.subtotal,a.discount_amount,a.grand_total,IF(STRCMP(a.status,'pending'),'EBS','COD') as order_status
FROM `sales_flat_order_grid` AS z
INNER JOIN `sales_flat_order` AS a ON z.entity_id = a.entity_id
INNER JOIN `sales_flat_order_item` AS b ON z.entity_id = b.order_id
INNER JOIN `sales_flat_order_address` AS ca ON ca.parent_id = b.order_id
AND ca.address_type = 'shipping'
INNER JOIN sales_flat_quote_item_option AS c ON b.quote_item_id = c.item_id
INNER JOIN catalog_product_option_type_title AS d ON c.value = d.option_type_id
WHERE b.parent_item_id IS NULL 
AND b.product_type = 'simple'
AND d.title != 'No Need'
AND b.product_options LIKE '%Additional Qty%'
AND z.increment_id in (" + txtidlist.Text.ToString() + ")";
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
                        Decimal qty = Convert.ToDecimal(row["qty_ordered"].ToString());
                        if ((weight / qty) < 1)
                        {
                            row["units"] = "Kg";
                        }
                        else
                        {
                            //row["units"] = "Piece/Bundle";
                            row["units"] = "Pc";
                        }
                        for (int i = 0; i < values.Count(); i++)
                        {
                            int ono = Convert.ToInt32(values[i].ToString());
                            DataTable products1 = null;
                            products1 = orderoption.Tables[0].AsEnumerable().Where(r => Convert.ToInt32(r["option_type_id"]) == ono).AsDataView().ToTable();
                            string title = products1.Rows[0]["title"].ToString();
                            if (products1.Rows[0]["title"].ToString().Contains("Kg"))
                            {
                                title = products1.Rows[0]["title"].ToString().Replace("Kg", "").Trim();
                            }
                            else if (products1.Rows[0]["title"].ToString().Contains("Bundle"))
                            {
                                title = products1.Rows[0]["title"].ToString().Replace("Bundle", "").Trim();
                            }
                            else if (products1.Rows[0]["title"].ToString().Contains("Piece"))
                            {
                                title = products1.Rows[0]["title"].ToString().Replace("Piece", "").Trim();
                            }
                            else if (products1.Rows[0]["title"].ToString().Contains("Packet"))
                            {
                                title = products1.Rows[0]["title"].ToString().Replace("Packet", "").Trim();
                            }
                            weight = weight + (Convert.ToDecimal(title) * qty);
                        }

                        DataRow _optionList = dt.NewRow();
                        //, product_id, Name, weight, qty_ordered
                        _optionList["order_id"] = row["order_id"].ToString();
                        _optionList["product_id"] = row["product_id"].ToString();
                        _optionList["Name"] = row["Name"].ToString();
                        _optionList["weight"] = weight;
                        _optionList["qty_ordered"] = row["qty_ordered"].ToString();
                        _optionList["amount"] = row["amount"].ToString();
                        _optionList["subtotal"] = row["subtotal"].ToString();
                        _optionList["discount_amount"] = row["discount_amount"].ToString();
                        _optionList["grand_total"] = row["grand_total"].ToString();
                        _optionList["order_status"] = row["order_status"].ToString();

                        row["weight"] = weight.ToString();
                    }
                }

            }
            catch (Exception ex)
            {
            }
            return orderoptionlist;
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

                    //getOrderDetails();
                    Getalacartewithnames();
                }
                else
                {
                    GVOrderDetails.DataSource = null;
                    GVOrderDetails.DataBind();
                    GVMail.DataSource = null;
                    GVMail.DataBind();
                    GvPackList3.DataSource = null;
                    GvPackList3.DataBind();
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
                    //ConnectSsh sshobj = new ConnectSsh();
                    //SshClient client=sshobj.SshConnect();
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
                //iTextSharp.text.pdf.PdfPTable table = new iTextSharp.text.pdf.PdfPTable(GVAlacarte.Columns.Count+1);
                iTextSharp.text.pdf.PdfPTable table = new iTextSharp.text.pdf.PdfPTable(4);
                int k = 1;
                foreach (GridViewRow row in GVAlacarte.Rows)
                {

                    GridView gvtest = ((GridView)row.FindControl("GvalacartewithNameList"));
                    Label gvfname = ((Label)row.FindControl("lblname"));
                    Label gvorderid = ((Label)row.FindControl("lblorderid"));
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
                        if (x == 1)
                        {
                            cellText= Server.HtmlDecode(gvfname.Text.ToString());
                        }
                        else if (x == 2)
                        {
                            cellText = Server.HtmlDecode(gvorderid.Text.ToString());
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
                                    cellText = Server.HtmlDecode((i + 1).ToString());
                                    //cellText = Server.HtmlDecode(gvtest.Rows[i].Cells[j].Text);
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
                    Response.AddHeader("content-disposition", "attachment;filename=alacartewithNamesInfo.pdf");
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
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "alacartewithNamesInfo.xls"));
                Response.ContentType = "application/ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                GVAlacarte.AllowPaging = false;
                //Change the Header Row back to white color
                GVAlacarte.HeaderRow.Style.Add("background-color", "#FFFFFF");
                //Applying stlye to gridview header cells
                for (int i = 0; i < GVAlacarte.HeaderRow.Cells.Count; i++)
                {
                    GVAlacarte.HeaderRow.Cells[i].Style.Add("background-color", "#507CD1");
                }
                int j = 1;
                //This loop is used to apply stlye to cells based on particular row
                foreach (GridViewRow gvrow in GVAlacarte.Rows)
                {
                    //gvrow.BackColor = Color.WHITE.ToString;
                    if (j <= GVAlacarte.Rows.Count)
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
                GVAlacarte.Parent.Controls.Add(frm);
                frm.Attributes["runat"] = "server";
                frm.Controls.Add(GVAlacarte);
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
