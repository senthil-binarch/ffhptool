using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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

namespace FFHPWeb
{
    public partial class AlertMail : System.Web.UI.Page
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
                
                if (Session["username"] != null && Session["username"].ToString() != "")
                {
                    if (!IsPostBack)
                    {
                        if (Session["username"] != null && Session["username"].ToString() != "")
                        {
                            
                        }
                        else
                        {
                            Response.Redirect("Login.aspx", false);
                        }
                        
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
            //if (!IsPostBack)
            //{
                //getCustomerDetails();
            //}
            //getdatas();
        }
        public void getcustomerdetails1()
        {
            queryString = @"select count(customer_id),customer_id,billing_name
from sales_flat_order_grid group by customer_id
having count(customer_id)=1";
        }
        public void getCustomerDetails()
        {
            try
            {
                if (ddltype.SelectedIndex == 1)
                {
                    //queryString = "SELECT entity_id, customer_firstname, customer_lastname FROM `sales_flat_order` where created_at between '" + Convert.ToDateTime(TbxFromDate.Text.ToString()).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(TbxToDate.Text.ToString()).ToString("yyyy-MM-dd") + "'";
                    queryString = @"SELECT c.*,CONCAT( IFNULL(fn.value,''),' ', IFNULL(ln.value,'') ) name,DATE_FORMAT( c.created_at, '%d/%m/%Y' ) AS created FROM customer_entity as c
join customer_entity_varchar fn
        on c.entity_id = fn.entity_id and fn.attribute_id =5
join customer_entity_varchar ln
        on c.entity_id = ln.entity_id and ln.attribute_id =7
where c.entity_id not in (select distinct(customer_id) from sales_flat_order_grid
where customer_id is not null)";
                }
                else if (ddltype.SelectedIndex == 2)
                {
                    queryString = @"SELECT c.*,CONCAT( IFNULL(fn.value,''),' ', IFNULL(ln.value,'') ) name,DATE_FORMAT( c.created_at, '%d/%m/%Y' ) AS created FROM customer_entity as c
join customer_entity_varchar fn
        on c.entity_id = fn.entity_id and fn.attribute_id =5
join customer_entity_varchar ln
        on c.entity_id = ln.entity_id and ln.attribute_id =7
where c.entity_id in (select distinct customer_id
from sales_flat_order_grid group by customer_id 
having count(customer_id)=1)";
                }
                else if (ddltype.SelectedIndex == 3)
                {
                    queryString = @"SELECT c.*,CONCAT( IFNULL(fn.value,''),' ', IFNULL(ln.value,'') ) name,DATE_FORMAT( c.created_at, '%d/%m/%Y' ) AS created FROM customer_entity as c
join customer_entity_varchar fn
        on c.entity_id = fn.entity_id and fn.attribute_id =5
join customer_entity_varchar ln
        on c.entity_id = ln.entity_id and ln.attribute_id =7
where c.entity_id in (select distinct customer_id
from sales_flat_order_grid group by customer_id 
having count(customer_id)>=2 and count(customer_id)<=5)";
                }
                else if (ddltype.SelectedIndex == 4)
                {
                    queryString = @"SELECT c.*,CONCAT( IFNULL(fn.value,''),' ', IFNULL(ln.value,'') ) name,DATE_FORMAT( c.created_at, '%d/%m/%Y' ) AS created FROM customer_entity as c
join customer_entity_varchar fn
        on c.entity_id = fn.entity_id and fn.attribute_id =5
join customer_entity_varchar ln
        on c.entity_id = ln.entity_id and ln.attribute_id =7
where c.entity_id in (select distinct customer_id
from sales_flat_order_grid group by customer_id 
having count(customer_id)>5)";
                }
                //Response.Write(queryString);
                if (queryString != "")
                {
                    MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                    DataSet customerlist = new DataSet();
                    adapteradminmail.Fill(customerlist, "customer_list");
                    GVCustomerList.DataSource = customerlist;
                    GVCustomerList.DataBind();
                }

            }
            catch (Exception ex)
            {
            }
        }
        
        public void getCustomerDetailswithname()
        {
            try
            {
                if (ddltype.SelectedIndex == 1)
                {
                    //queryString = "SELECT entity_id, customer_firstname, customer_lastname FROM `sales_flat_order` where created_at between '" + Convert.ToDateTime(TbxFromDate.Text.ToString()).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(TbxToDate.Text.ToString()).ToString("yyyy-MM-dd") + "'";
                    queryString = @"SELECT c.*,CONCAT( IFNULL(fn.value,''),' ', IFNULL(ln.value,'') ) name,DATE_FORMAT( c.created_at, '%d/%m/%Y' ) AS created FROM customer_entity as c
join customer_entity_varchar fn
        on c.entity_id = fn.entity_id and fn.attribute_id =5
join customer_entity_varchar ln
        on c.entity_id = ln.entity_id and ln.attribute_id =7
where c.entity_id not in (select distinct(customer_id) from sales_flat_order_grid
where customer_id is not null)";
                }
                else if (ddltype.SelectedIndex == 2)
                {
                    queryString = @"SELECT c . * , CONCAT( IFNULL( fn.value, '' ) , ' ', IFNULL( ln.value, '' ) ) name, DATE_FORMAT( 

c.created_at, '%d/%m/%Y' ) AS created
FROM customer_entity AS c
JOIN customer_entity_varchar fn ON c.entity_id = fn.entity_id
AND fn.attribute_id =5
JOIN customer_entity_varchar ln ON c.entity_id = ln.entity_id
AND ln.attribute_id =7
WHERE CONCAT( IFNULL( fn.value, '' ) , ' ', IFNULL( ln.value, '' ) ) 
IN (
SELECT DISTINCT shipping_name
FROM sales_flat_order_grid
GROUP BY shipping_name
HAVING count( CONCAT( IFNULL( fn.value, '' ) , ' ', IFNULL( ln.value, '' ) ) ) =1
)";
                }
                else if (ddltype.SelectedIndex == 3)
                {
                    queryString = @"SELECT c . * , CONCAT( IFNULL( fn.value, '' ) , ' ', IFNULL( ln.value, '' ) ) name, DATE_FORMAT( 

c.created_at, '%d/%m/%Y' ) AS created
FROM customer_entity AS c
JOIN customer_entity_varchar fn ON c.entity_id = fn.entity_id
AND fn.attribute_id =5
JOIN customer_entity_varchar ln ON c.entity_id = ln.entity_id
AND ln.attribute_id =7
WHERE CONCAT( IFNULL( fn.value, '' ) , ' ', IFNULL( ln.value, '' ) ) 
IN (
SELECT DISTINCT shipping_name
FROM sales_flat_order_grid
GROUP BY shipping_name
HAVING count( CONCAT( IFNULL( fn.value, '' ) , ' ', IFNULL( ln.value, '' ) ) ) >=2 and count( CONCAT( IFNULL( fn.value, '' ) , ' ', IFNULL( ln.value, '' ) ) ) <=5
)";
                }
                else if (ddltype.SelectedIndex == 4)
                {
                    queryString = @"SELECT c . * , CONCAT( IFNULL( fn.value, '' ) , ' ', IFNULL( ln.value, '' ) ) name, DATE_FORMAT( 

c.created_at, '%d/%m/%Y' ) AS created
FROM customer_entity AS c
JOIN customer_entity_varchar fn ON c.entity_id = fn.entity_id
AND fn.attribute_id =5
JOIN customer_entity_varchar ln ON c.entity_id = ln.entity_id
AND ln.attribute_id =7
WHERE CONCAT( IFNULL( fn.value, '' ) , ' ', IFNULL( ln.value, '' ) ) 
IN (
SELECT DISTINCT shipping_name
FROM sales_flat_order_grid
GROUP BY shipping_name
HAVING count( CONCAT( IFNULL( fn.value, '' ) , ' ', IFNULL( ln.value, '' ) ) ) >5
)";
                }
                //Response.Write(queryString);
                if (queryString != "")
                {
                    MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                    DataSet customerlist = new DataSet();
                    adapteradminmail.Fill(customerlist, "customer_list");
                    GVCustomerList.DataSource = customerlist;
                    GVCustomerList.DataBind();
                }

            }
            catch (Exception ex)
            {
            }
        }
        public void getCustomerDetails_saleswithname()
        {
            try
            {
                if (ddltype.SelectedIndex == 1)
                {
                    //queryString = "SELECT entity_id, customer_firstname, customer_lastname FROM `sales_flat_order` where created_at between '" + Convert.ToDateTime(TbxFromDate.Text.ToString()).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(TbxToDate.Text.ToString()).ToString("yyyy-MM-dd") + "'";
                    queryString = @"SELECT c.*,CONCAT( IFNULL(fn.value,''),' ', IFNULL(ln.value,'') ) name,ph.value as telephone,DATE_FORMAT( c.created_at, '%d/%m/%Y' ) AS created FROM customer_entity as c
join customer_entity_varchar fn
        on c.entity_id = fn.entity_id and fn.attribute_id =5
join customer_entity_varchar ln
        on c.entity_id = ln.entity_id and ln.attribute_id =7
left outer JOIN customer_address_entity_varchar ph ON c.entity_id = ph.entity_id
AND ph.attribute_id =31
where c.entity_id not in (select distinct(customer_id) from sales_flat_order_grid
where customer_id is not null and )";
                }
                else if (ddltype.SelectedIndex == 2)
                {
                    queryString = @"SELECT DISTINCT email, CONCAT( IFNULL( firstname, '' ) , ' ', IFNULL( lastname, '' ) ) AS name
FROM sales_flat_order_address
WHERE CONCAT( IFNULL( firstname, '' ) , ' ', IFNULL( lastname, '' ) ) 
IN (
SELECT billing_name
FROM sales_flat_order_grid
GROUP BY billing_name
HAVING count( billing_name ) =1
)
AND address_type = 'billing'
AND email IS NOT NULL";
                }
                else if (ddltype.SelectedIndex == 3)
                {
                    queryString = @"SELECT DISTINCT email, CONCAT( IFNULL( firstname, '' ) , ' ', IFNULL( lastname, '' ) ) AS name
FROM sales_flat_order_address
WHERE CONCAT( IFNULL( firstname, '' ) , ' ', IFNULL( lastname, '' ) ) 
IN (
SELECT billing_name
FROM sales_flat_order_grid
GROUP BY billing_name
HAVING count( billing_name ) >=2 and count( billing_name ) <=5
)
AND address_type = 'billing'
AND email IS NOT NULL";
                }
                else if (ddltype.SelectedIndex == 4)
                {
                    queryString = @"SELECT DISTINCT email, CONCAT( IFNULL( firstname, '' ) , ' ', IFNULL( lastname, '' ) ) AS name
FROM sales_flat_order_address
WHERE CONCAT( IFNULL( firstname, '' ) , ' ', IFNULL( lastname, '' ) ) 
IN (
SELECT billing_name
FROM sales_flat_order_grid
GROUP BY billing_name
HAVING count( billing_name ) >5
)
AND address_type = 'billing'
AND email IS NOT NULL";
                }
                //Response.Write(queryString);
                if (queryString != "")
                {
                    MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                    DataSet customerlist = new DataSet();
                    adapteradminmail.Fill(customerlist, "customer_list");

                    if (ddltype.SelectedValue.ToString() == "1")
                    {
                        GVNoOrderCustomerList.DataSource = customerlist;
                        GVNoOrderCustomerList.DataBind();
                        GVNoOrderCustomerList.Visible = true;
                        GVCustomerList.Visible = false;
                    }
                    else
                    {
                        GVCustomerList.DataSource = customerlist;
                        GVCustomerList.DataBind();
                        GVNoOrderCustomerList.Visible = false;
                        GVCustomerList.Visible = true;
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }
        protected void ddltype_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //ConnectSsh sshobj = new ConnectSsh();
            //SshClient client = sshobj.SshConnect();
            //getCustomerDetails_saleswithname();
            if (ddltype.SelectedValue.ToString() != "0" && ddltype.SelectedValue.ToString() != "1")
            {
                if (ddltype.SelectedValue.ToString() == "5")
                {
                    getdatasabovefivewithcompleted();
                }
                else
                {
                    getdatas();
                }
                GVCustList.Visible = true;
                GVNoOrderCustomerList.Visible = false;
            }
            else if (ddltype.SelectedValue.ToString() == "1")
            {
                GVCustList.Visible = false;
                getCustomerDetails_saleswithname();
                GVNoOrderCustomerList.Visible = true;
            }
            //sshobj.SshDisconnect(client);
        }
        protected void btndetails_OnClick(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
            HiddenField HFentityid = (HiddenField)gvr.FindControl("HFentity_id");
            GridView GVdetails = (GridView)gvr.FindControl("gvdetails");
//            queryString = @"SELECT increment_id, created_at
//FROM sales_flat_order_grid
//WHERE customer_id =" + HFentityid.Value.ToString() + " ORDER BY created_at DESC LIMIT 1 ";
//            queryString = @"SELECT count(*) as TotalOrders,increment_id, created_at
//FROM sales_flat_order_grid
//WHERE customer_id =" + HFentityid.Value.ToString() + " ORDER BY created_at DESC LIMIT 1 ";
//            queryString = @"SELECT count(*) as TotalOrders,increment_id, created_at
//FROM sales_flat_order_grid
//WHERE billing_name ='" + HFentityid.Value.ToString() + "' ORDER BY created_at DESC LIMIT 1 ";
            queryString = @"SELECT count(*) as TotalOrders,
(SELECT increment_id FROM sales_flat_order_grid
WHERE billing_name ='" + HFentityid.Value.ToString() + "' ORDER BY created_at DESC LIMIT 1) as increment_id, (SELECT DATE_FORMAT( created_at, '%d/%m/%Y' ) AS created_at FROM sales_flat_order_grid WHERE billing_name ='" + HFentityid.Value.ToString() + "' ORDER BY created_at DESC LIMIT 1) as created_at, (select distinct telephone from sales_flat_order_address where CONCAT( IFNULL( firstname, '' ) , ' ', IFNULL(lastname, '' ) )='" + HFentityid.Value.ToString() + "' and address_type='billing') as telephone,(SELECT DATE_FORMAT( c.created_at, '%d/%m/%Y' ) AS created FROM customer_entity as c join customer_entity_varchar fn on c.entity_id = fn.entity_id and fn.attribute_id =5 join customer_entity_varchar ln on c.entity_id = ln.entity_id and ln.attribute_id =7 where CONCAT( IFNULL(fn.value,''),' ', IFNULL(ln.value,'') ) ='" + HFentityid.Value.ToString() + "' and email not like '%farmfreshhandpicked.com') as registered_date FROM sales_flat_order_grid WHERE billing_name ='" + HFentityid.Value.ToString() + "' ORDER BY created_at DESC";
            if (queryString != "")
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();

                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                DataSet Lastorderlist = new DataSet();
                adapteradminmail.Fill(Lastorderlist, "Last_Order_list");
                GVdetails.DataSource = Lastorderlist;
                GVdetails.DataBind();
                GVdetails.Visible = true;
                //sshobj.SshDisconnect(client);
            }
        }
        protected void GVCustomerList_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    HiddenField HFentityid = (HiddenField)e.Row.FindControl("HFentity_id");
                    GridView GVdetails = (GridView)e.Row.FindControl("gvdetails");
                    queryString = @"SELECT increment_id, created_at
FROM sales_flat_order_grid
WHERE customer_id =" + HFentityid.Value.ToString() + " ORDER BY created_at DESC LIMIT 1 ";
                    if (queryString != "")
                    {
                        MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                        DataSet Lastorderlist = new DataSet();
                        adapteradminmail.Fill(Lastorderlist, "Last_Order_list");
                        GVdetails.DataSource = Lastorderlist;
                        GVdetails.DataBind();
                    }
                }
                queryString = @"SELECT c . * , CONCAT( IFNULL( fn.value, '' ) , ' ', IFNULL( ln.value, '' ) ) name, DATE_FORMAT( 

c.created_at, '%d/%m/%Y' ) AS created
FROM customer_entity AS c
JOIN customer_entity_varchar fn ON c.entity_id = fn.entity_id
AND fn.attribute_id =5
JOIN customer_entity_varchar ln ON c.entity_id = ln.entity_id
AND ln.attribute_id =7
WHERE CONCAT( IFNULL( fn.value, '' ) , ' ', IFNULL( ln.value, '' ) ) 
IN (
SELECT DISTINCT shipping_name
FROM sales_flat_order_grid
GROUP BY shipping_name
HAVING count( CONCAT( IFNULL( fn.value, '' ) , ' ', IFNULL( ln.value, '' ) ) ) >5
)";
            }
            catch (Exception ex)
            {
            }
        }
        public void getdatas()
        {
            DataTable testdt = new DataTable("testPackList");
            testdt.Clear();
            testdt.Columns.Add("totalorder");
            testdt.Columns.Add("ordernumber");
            testdt.Columns.Add("shipping_name");
            testdt.Columns.Add("customer_email");
            testdt.Columns.Add("phone");
            testdt.Columns.Add("customer_id");
            testdt.Columns.Add("email");
            testdt.Columns.Add("user_created_date");
            testdt.Columns.Add("increment_id");
            testdt.Columns.Add("created_at");
            testdt.Columns.Add("status");

//            queryString = @"SELECT b.entity_id AS ordernumber, a.billing_name as shipping_name, b.customer_email, e.telephone,c.value as registered_telephone, d.entity_id, d.email, DATE_FORMAT( d.created_at, '%d/%m/%Y' ) AS user_created_date, a.increment_id, DATE_FORMAT( a.created_at, '%d/%m/%Y' ) as created_at
//FROM `sales_flat_order_grid` AS a
//JOIN sales_flat_order AS b ON a.entity_id = b.entity_id
//JOIN sales_flat_order_address AS e ON a.entity_id = e.parent_id
//AND e.address_type = 'billing'
//LEFT OUTER JOIN customer_address_entity_varchar AS c ON b.entity_id = c.entity_id
//AND c.attribute_id =31
//LEFT OUTER JOIN customer_entity AS d ON b.customer_email = d.email";
            queryString = @"SELECT b.status,b.entity_id AS ordernumber, a.billing_name as shipping_name, b.customer_email, e.telephone, d.entity_id, d.email, DATE_FORMAT( d.created_at, '%d/%m/%Y' ) AS user_created_date, a.increment_id, DATE_FORMAT( a.created_at, '%d/%m/%Y' ) as created_at
FROM `sales_flat_order_grid` AS a
JOIN sales_flat_order AS b ON a.entity_id = b.entity_id
JOIN sales_flat_order_address AS e ON a.entity_id = e.parent_id
AND e.address_type = 'billing'
LEFT OUTER JOIN customer_entity AS d ON b.customer_email = d.email WHERE b.status = 'complete' ORDER BY a.created_at DESC";
            if (queryString != "")
            {
                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                DataSet Orderlist = new DataSet();
                adapteradminmail.Fill(Orderlist, "Order_list");
                DataTable dtt1 = resort(Orderlist.Tables["Order_list"], "customer_email", "ASC");
                var distinctRows = (from DataRow dRow in dtt1.Rows
                                    select new { col1 = dRow["customer_email"] }).Distinct();
                foreach (var value in distinctRows)
                {
                    string s = value.col1.ToString();
                    DataTable products1 = null;
                    products1 = dtt1.AsEnumerable().Where(r => Convert.ToString(r["customer_email"]) == s).AsDataView().ToTable();
                    //products1 = resort(products1, "created_at", "ASC");
                    DataRow _CustList = testdt.NewRow();
                    foreach (DataRow row in products1.Rows)
                    {
                        if (row["entity_id"].ToString() != "")
                        {

                            _CustList["totalorder"] = products1.Rows.Count.ToString();
                            _CustList["ordernumber"] = row["ordernumber"].ToString();
                            _CustList["shipping_name"] = row["shipping_name"].ToString();
                            _CustList["customer_email"] = row["customer_email"].ToString();
                            _CustList["phone"] = row["telephone"].ToString();
                            _CustList["customer_id"] = row["entity_id"].ToString();
                            _CustList["email"] = row["email"].ToString();
                            _CustList["user_created_date"] = row["user_created_date"].ToString();
                            _CustList["increment_id"] = row["increment_id"].ToString();
                            _CustList["created_at"] = row["created_at"].ToString();
                            _CustList["status"] = row["status"].ToString();
                        }
                        else
                        {

                            _CustList["totalorder"] = products1.Rows.Count.ToString();
                            _CustList["ordernumber"] = row["ordernumber"].ToString();
                            _CustList["shipping_name"] = row["shipping_name"].ToString();
                            _CustList["customer_email"] = row["customer_email"].ToString();
                            _CustList["phone"] = row["telephone"].ToString();
                            _CustList["customer_id"] = row["entity_id"].ToString();
                            _CustList["email"] = row["email"].ToString();
                            _CustList["user_created_date"] = row["user_created_date"].ToString();
                            _CustList["increment_id"] = row["increment_id"].ToString();
                            _CustList["created_at"] = row["created_at"].ToString();
                            _CustList["status"] = row["status"].ToString();

                        }
                        break;

                    }
                    testdt.Rows.Add(_CustList);
                    
                }
                DataTable dtfinal = new DataTable();
                //testdt = resort(testdt, "shipping_name", "asc");
                if (ddltype.SelectedValue.ToString() == "2")//order once
                {
                    dtfinal = testdt.AsEnumerable().Where(r => Convert.ToInt32(r["totalorder"]) == 1).AsDataView().ToTable();
                }
                if (ddltype.SelectedValue.ToString()=="3")//order once
                {
                    dtfinal = testdt.AsEnumerable().Where(p => Convert.ToInt32(p["totalorder"]) >= 2 && Convert.ToInt32(p["totalorder"]) <= 5).AsDataView().ToTable();
                }
                if (ddltype.SelectedValue.ToString() == "4")//order once
                {
                    dtfinal = testdt.AsEnumerable().Where(r => Convert.ToInt32(r["totalorder"]) > 5).AsDataView().ToTable();
                }
                if (ddltype.SelectedValue.ToString() == "5")//order once
                {
                    dtfinal = testdt.AsEnumerable().Where(r => Convert.ToString(r["status"]) == "complete" && Convert.ToInt32(r["totalorder"]) > 5).AsDataView().ToTable();
                }
                
                //dtfinal = testdt.AsEnumerable().Where(r => Convert.ToString(r["totalorder"]) == s).AsDataView().ToTable();
                if (dtfinal.Rows.Count > 0)
                {
                    dtfinal = resort(dtfinal, "shipping_name,totalorder", "asc");
                }
                    GVCustList.DataSource = dtfinal;
                    GVCustList.DataBind();
                    //GVCustList.Visible = true;
                    GVCustList1.DataSource = null;
                    GVCustList1.DataBind();
            }

        }
        public void getdatasabovefivewithcompleted()
        {
            DataTable testdt = new DataTable("testPackList");
            testdt.Clear();
            testdt.Columns.Add("totalorder1");
            testdt.Columns.Add("totalorder");
            testdt.Columns.Add("ordernumber");
            testdt.Columns.Add("shipping_name");
            testdt.Columns.Add("customer_email");
            testdt.Columns.Add("phone");
            testdt.Columns.Add("customer_id");
            testdt.Columns.Add("email");
            testdt.Columns.Add("user_created_date");
            testdt.Columns.Add("increment_id");
            testdt.Columns.Add("created_at");
            testdt.Columns.Add("status");

            //            queryString = @"SELECT b.entity_id AS ordernumber, a.billing_name as shipping_name, b.customer_email, e.telephone,c.value as registered_telephone, d.entity_id, d.email, DATE_FORMAT( d.created_at, '%d/%m/%Y' ) AS user_created_date, a.increment_id, DATE_FORMAT( a.created_at, '%d/%m/%Y' ) as created_at
            //FROM `sales_flat_order_grid` AS a
            //JOIN sales_flat_order AS b ON a.entity_id = b.entity_id
            //JOIN sales_flat_order_address AS e ON a.entity_id = e.parent_id
            //AND e.address_type = 'billing'
            //LEFT OUTER JOIN customer_address_entity_varchar AS c ON b.entity_id = c.entity_id
            //AND c.attribute_id =31
            //LEFT OUTER JOIN customer_entity AS d ON b.customer_email = d.email";
            queryString = @"SELECT b.status,b.entity_id AS ordernumber, a.billing_name as shipping_name, b.customer_email, e.telephone, d.entity_id, d.email, DATE_FORMAT( d.created_at, '%d/%m/%Y' ) AS user_created_date, a.increment_id, DATE_FORMAT( a.created_at, '%d/%m/%Y' ) as created_at
FROM `sales_flat_order_grid` AS a
JOIN sales_flat_order AS b ON a.entity_id = b.entity_id
JOIN sales_flat_order_address AS e ON a.entity_id = e.parent_id
AND e.address_type = 'billing'
LEFT OUTER JOIN customer_entity AS d ON b.customer_email = d.email ORDER BY a.created_at DESC";
            if (queryString != "")
            {
                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                DataSet Orderlist = new DataSet();
                adapteradminmail.Fill(Orderlist, "Order_list");
                DataTable dtt1 = resort(Orderlist.Tables["Order_list"], "customer_email", "ASC");
                var distinctRows = (from DataRow dRow in dtt1.Rows
                                    select new { col1 = dRow["customer_email"] }).Distinct();
                foreach (var value in distinctRows)
                {
                    string s = value.col1.ToString();
                    DataTable products1 = null,products2=null;
                    products1 = dtt1.AsEnumerable().Where(r => Convert.ToString(r["customer_email"]) == s && Convert.ToString(r["status"]) == "complete").AsDataView().ToTable();
                    products2 = dtt1.AsEnumerable().Where(r => Convert.ToString(r["customer_email"]) == s).AsDataView().ToTable();
                    //products1 = resort(products1, "created_at", "ASC");
                    DataRow _CustList = testdt.NewRow();
                    //if (products1.Rows.Count > 0)
                    //{
                    //    //if (products1.Rows[0]["entity_id"].ToString() != "")
                    //    //{
                    //        _CustList["totalorder1"] = products1.Rows.Count.ToString();
                    //        _CustList["totalorder"] = products2.Rows.Count.ToString();
                    //        _CustList["ordernumber"] = products1.Rows[0]["ordernumber"].ToString();
                    //        _CustList["shipping_name"] = products1.Rows[0]["shipping_name"].ToString();
                    //        _CustList["customer_email"] = products1.Rows[0]["customer_email"].ToString();
                    //        _CustList["phone"] = products1.Rows[0]["telephone"].ToString();
                    //        _CustList["customer_id"] = products1.Rows[0]["entity_id"].ToString();
                    //        _CustList["email"] = products1.Rows[0]["email"].ToString();
                    //        _CustList["user_created_date"] = products1.Rows[0]["user_created_date"].ToString();
                    //        _CustList["increment_id"] = products1.Rows[0]["increment_id"].ToString();
                    //        _CustList["created_at"] = products1.Rows[0]["created_at"].ToString();
                    //        _CustList["status"] = products1.Rows[0]["status"].ToString();
                    //    //}
                    //}

                    foreach (DataRow row in products1.Rows)
                    {
                        if (row["entity_id"].ToString() != "")
                        {
                            _CustList["totalorder1"] = products2.Rows.Count.ToString();
                            _CustList["totalorder"] = products1.Rows.Count.ToString();
                            _CustList["ordernumber"] = row["ordernumber"].ToString();
                            _CustList["shipping_name"] = row["shipping_name"].ToString();
                            _CustList["customer_email"] = row["customer_email"].ToString();
                            _CustList["phone"] = row["telephone"].ToString();
                            _CustList["customer_id"] = row["entity_id"].ToString();
                            _CustList["email"] = row["email"].ToString();
                            _CustList["user_created_date"] = row["user_created_date"].ToString();
                            _CustList["increment_id"] = row["increment_id"].ToString();
                            _CustList["created_at"] = row["created_at"].ToString();
                            _CustList["status"] = row["status"].ToString();
                        }
                        else
                        {
                            _CustList["totalorder1"] = products2.Rows.Count.ToString();
                            _CustList["totalorder"] = products1.Rows.Count.ToString();
                            _CustList["ordernumber"] = row["ordernumber"].ToString();
                            _CustList["shipping_name"] = row["shipping_name"].ToString();
                            _CustList["customer_email"] = row["customer_email"].ToString();
                            _CustList["phone"] = row["telephone"].ToString();
                            _CustList["customer_id"] = row["entity_id"].ToString();
                            _CustList["email"] = row["email"].ToString();
                            _CustList["user_created_date"] = row["user_created_date"].ToString();
                            _CustList["increment_id"] = row["increment_id"].ToString();
                            _CustList["created_at"] = row["created_at"].ToString();
                            _CustList["status"] = row["status"].ToString();

                        }
                        break;

                    }
                    testdt.Rows.Add(_CustList);

                }
                DataTable dtfinal = new DataTable();
                //testdt = resort(testdt, "shipping_name", "asc");
                if (ddltype.SelectedValue.ToString() == "2")//order once
                {
                    dtfinal = testdt.AsEnumerable().Where(r => Convert.ToInt32(r["totalorder"]) == 1).AsDataView().ToTable();
                }
                if (ddltype.SelectedValue.ToString() == "3")//order once
                {
                    dtfinal = testdt.AsEnumerable().Where(p => Convert.ToInt32(p["totalorder"]) >= 2 && Convert.ToInt32(p["totalorder"]) <= 5).AsDataView().ToTable();
                }
                if (ddltype.SelectedValue.ToString() == "4")//order once
                {
                    dtfinal = testdt.AsEnumerable().Where(r => Convert.ToInt32(r["totalorder"]) > 5).AsDataView().ToTable();
                }
                if (ddltype.SelectedValue.ToString() == "5")//order once
                {
                    dtfinal = testdt.AsEnumerable().Where(r => Convert.ToString(r["status"]) == "complete" && Convert.ToInt32(r["totalorder1"]) > 5).AsDataView().ToTable();
                }

                //dtfinal = testdt.AsEnumerable().Where(r => Convert.ToString(r["totalorder"]) == s).AsDataView().ToTable();
                if (dtfinal.Rows.Count > 0)
                {
                    dtfinal = resort(dtfinal, "shipping_name,totalorder", "asc");
                }
                GVCustList1.DataSource = dtfinal;
                GVCustList1.DataBind();
                //GVCustList1.Visible = true;
                GVCustList.DataSource = null;
                GVCustList.DataBind();
            }

        }
        public static DataTable resort(DataTable dt, string colName, string direction)
        {
            dt.DefaultView.Sort = colName + " " + direction;
            dt = dt.DefaultView.ToTable();
            return dt;
        }
        public void test()
        {
            DataTable dt = new DataTable();
            string ColumnName, ColumnData = "";
            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    ColumnName = column.ColumnName;
                    ColumnData = row[column].ToString();
                }
            }
        }
    }
}
