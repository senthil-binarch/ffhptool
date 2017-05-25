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
using System.Text.RegularExpressions;
using System.Net;
//using Renci.SshNet;
namespace FFHPWeb
{
    public partial class SendSmsCustomerTemplate : System.Web.UI.Page
    {
        string conn = System.Configuration.ConfigurationManager.AppSettings["Connection"].ToString();
        string smsconn = System.Configuration.ConfigurationManager.AppSettings["SmsDBConnection"].ToString();//"server=68.178.143.39;userid=ffhplog;password=FFHPl0g!;database=ffhplog;Convert Zero Datetime=True";  //sms connection
        string queryString = "";
        MySqlDataAdapter DA;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();

                Bindsmstype();

                //sshobj.SshDisconnect(client);
            }
            lblerror.Text = "";
        }
        public void Bindsmstype()
        {
            try
            {
                DataTable smstemplate = new DataTable("smstemplate");
                queryString = "SELECT * FROM `1_ffhp_smsformat` where order_based=0";
                if (queryString != "")
                {
                    MySqlConnection con = new MySqlConnection(smsconn);
                    con.Open();

                    MySqlDataAdapter adaptersmstemplate = new MySqlDataAdapter(queryString, smsconn);


                    adaptersmstemplate.Fill(smstemplate);
                    ddlsmstype.DataSource = smstemplate;
                    ddlsmstype.AppendDataBoundItems = true;
                    ddlsmstype.Items.Add("-Select-");
                    ddlsmstype.DataTextField = "sms_type";
                    ddlsmstype.DataValueField = "sms_id";
                    ddlsmstype.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
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
                    DataTable products1 = null, products2 = null;
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
                GVCustList1.Visible = true;
                GVCustList.DataSource = null;
                GVCustList.DataBind();
                GVCustList.Visible = false;
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
                    dtfinal = testdt.AsEnumerable().Where(r => Convert.ToString(r["status"]) == "complete" && Convert.ToInt32(r["totalorder"]) > 5).AsDataView().ToTable();
                }

                //dtfinal = testdt.AsEnumerable().Where(r => Convert.ToString(r["totalorder"]) == s).AsDataView().ToTable();
                if (dtfinal.Rows.Count > 0)
                {
                    dtfinal = resort(dtfinal, "shipping_name,totalorder", "asc");
                }
                GVCustList.DataSource = dtfinal;
                GVCustList.DataBind();
                GVCustList.Visible = true;
                GVCustList1.DataSource = null;
                GVCustList1.DataBind();
                GVCustList1.Visible = false;
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
JOIN customer_address_entity_varchar ph ON c.entity_id = ph.entity_id
AND ph.attribute_id =31
where c.entity_id not in (select distinct(customer_id) from sales_flat_order_grid
where customer_id is not null)";
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
                        GVCustomerList1.Visible = false;
                    }
                    else
                    {
                        GVCustomerList1.DataSource = customerlist;
                        GVCustomerList1.DataBind();
                        GVNoOrderCustomerList.Visible = false;
                        GVCustomerList1.Visible = true;
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }
        protected void rblselect_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            tbselect.Visible = true;
            if (rblselect.SelectedValue.ToString() == "1")
            {
                trcustomerlist1.Visible = true;
                trcustomerlist2.Visible = false;
            }
            else if (rblselect.SelectedValue.ToString() == "2")
            {
                trcustomerlist2.Visible = true;
                trcustomerlist1.Visible = false;
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
                //GVCustList.Visible = true;
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
        protected void CustomerConfirmedall_GVNoOrderCustomerList_OnCheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow itm in GVNoOrderCustomerList.Rows)
            {
                if (((CheckBox)itm.FindControl("CBConfirmed")).Enabled == true)
                {
                    if (((CheckBox)GVNoOrderCustomerList.HeaderRow.FindControl("CBCustomerConfirmedall_GVNoOrderCustomerList")).Checked)
                    {

                        ((CheckBox)itm.FindControl("CBConfirmed")).Checked = true;
                    }
                    else
                    {
                        ((CheckBox)itm.FindControl("CBConfirmed")).Checked = false;
                    }
                }
            }
        }
        protected void CustomerConfirmedall_GVCustomerList1_OnCheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow itm in GVCustomerList1.Rows)
            {
                if (((CheckBox)itm.FindControl("CBConfirmed")).Enabled == true)
                {
                    if (((CheckBox)GVCustomerList1.HeaderRow.FindControl("CBCustomerConfirmedall_GVCustomerList1")).Checked)
                    {

                        ((CheckBox)itm.FindControl("CBConfirmed")).Checked = true;
                    }
                    else
                    {
                        ((CheckBox)itm.FindControl("CBConfirmed")).Checked = false;
                    }
                }
            }
        }
        protected void CustomerConfirmedall_GVCustList_OnCheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow itm in GVCustList.Rows)
            {
                if (((CheckBox)itm.FindControl("CBConfirmed")).Enabled == true)
                {
                    if (((CheckBox)GVCustList.HeaderRow.FindControl("CBCustomerConfirmedall_GVCustList")).Checked)
                    {

                        ((CheckBox)itm.FindControl("CBConfirmed")).Checked = true;
                    }
                    else
                    {
                        ((CheckBox)itm.FindControl("CBConfirmed")).Checked = false;
                    }
                }
            }
        }
        protected void CustomerConfirmedall_GVCustList1_OnCheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow itm in GVCustList1.Rows)
            {
                if (((CheckBox)itm.FindControl("CBConfirmed")).Enabled == true)
                {
                    if (((CheckBox)GVCustList1.HeaderRow.FindControl("CBCustomerConfirmedall_GVCustList1")).Checked)
                    {

                        ((CheckBox)itm.FindControl("CBConfirmed")).Checked = true;
                    }
                    else
                    {
                        ((CheckBox)itm.FindControl("CBConfirmed")).Checked = false;
                    }
                }
            }
        }
        protected void CustomerConfirmedall_OnCheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow itm in Gvcustomerlist.Rows)
            {
                if (((CheckBox)itm.FindControl("CBConfirmed")).Enabled == true)
                {
                    if (((CheckBox)Gvcustomerlist.HeaderRow.FindControl("CustomerConfirmedall")).Checked)
                    {

                        ((CheckBox)itm.FindControl("CBConfirmed")).Checked = true;
                    }
                    else
                    {
                        ((CheckBox)itm.FindControl("CBConfirmed")).Checked = false;
                    }
                }
            }
        }
        protected void BtnGetCustomers_OnClick(object sender, EventArgs e)
        {
            try
            {
                queryString = @"SELECT CONCAT( IFNULL( `firstname`.`value` , '' ) , ' ', IFNULL( `lastname`.`value` , '' ) ) AS customername,`firstname`.`value` AS `First_Name` , `lastname`.`value` AS `Last_Name` , `telephone`.`value` AS `Telephone` , `customer_entity`.`created_at` , `customer_entity`.`updated_at`
FROM `customer_address_entity_varchar` AS `country`
INNER JOIN `customer_address_entity_varchar` AS `firstname`
USING ( `entity_id` )
INNER JOIN `customer_address_entity_varchar` AS `lastname`
USING ( `entity_id` )
INNER JOIN `customer_address_entity_varchar` AS `telephone`
USING ( `entity_id` )
INNER JOIN `customer_entity`
USING ( `entity_id` )
WHERE `country`.`attribute_id` =27 && `country`.`value` = 'IN' && `firstname`.`attribute_id` =20 && `lastname`.`attribute_id` =22 && `telephone`.`attribute_id` =31
GROUP BY `telephone`.`value`";

                
                if (queryString != "")
                {
                    //ConnectSsh sshobj = new ConnectSsh();
                    //SshClient client = sshobj.SshConnect();

                    MySqlConnection con = new MySqlConnection(conn);
                    con.Open();

                    MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                    DataSet customerlist = new DataSet();
                    adapteradminmail.Fill(customerlist, "customertelephone");
                    Gvcustomerlist.DataSource = customerlist;
                    Gvcustomerlist.DataBind();

                    //sshobj.SshDisconnect(client);
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }

        }
        protected void btnsendsmsConfirmed_OnClick(object sender, EventArgs e)
        {
            try
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();

                if (Lblsmscontent.Text.ToString() != "" && !Lblsmscontent.Text.ToString().Contains("~"))
                {
                    if (rblselect.SelectedValue.ToString() == "1")
                    {
                        if (ddltype.SelectedValue.ToString() != "0" && ddltype.SelectedValue.ToString() != "1")
                        {
                            if (ddltype.SelectedValue.ToString() == "5")
                            {
                                //getdatasabovefivewithcompleted();
                                if (GVCustList1.Rows.Count > 0)
                                {
                                    Sendsms_customer(Lblsmscontent.Text.ToString(), GVCustList1);
                                }
                                else
                                {
                                    lblerror.Text = "Please select customer list";
                                }
                            }
                            else
                            {
                                //getdatas();
                                if (GVCustList.Rows.Count > 0)
                                {
                                    Sendsms_customer(Lblsmscontent.Text.ToString(), GVCustList);
                                }
                                else
                                {
                                    lblerror.Text = "Please select customer list";
                                }
                            }
                            
                        }
                        else if (ddltype.SelectedValue.ToString() == "1")
                        {
                            if (GVNoOrderCustomerList.Rows.Count > 0)
                            {
                                Sendsms_customer(Lblsmscontent.Text.ToString(), GVNoOrderCustomerList);
                            }
                            else
                            {
                                lblerror.Text = "Please select customer list";
                            }
                        }
                    }
                    else if (rblselect.SelectedValue.ToString() == "2")
                    {
                        if (Gvcustomerlist.Rows.Count > 0)
                        {
                            Sendsms_customer(Lblsmscontent.Text.ToString(), Gvcustomerlist);
                        }
                        else
                        {
                            lblerror.Text = "Please select customer list";
                        }
                    }
                }
                else
                {
                    lblerror.Text = "Please Click Insert Fields";
                }
                //sshobj.SshDisconnect(client);
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        protected void ddlsmstype_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable smstemplate = new DataTable("smstemplate");
                queryString = "SELECT * FROM `1_ffhp_smsformat` where sms_id="+ddlsmstype.SelectedValue.ToString();
                if (queryString != "")
                {
                    //ConnectSsh sshobj = new ConnectSsh();
                    //SshClient client = sshobj.SshConnect();

                    MySqlConnection con = new MySqlConnection(smsconn);
                    con.Open();

                    MySqlDataAdapter adaptersmstemplate = new MySqlDataAdapter(queryString, smsconn);


                    adaptersmstemplate.Fill(smstemplate);
                    if (smstemplate.Rows.Count > 0)
                    {
                        HFsms_content.Value = smstemplate.Rows[0]["sms_content"].ToString();
                        Lblcontent.Text = smstemplate.Rows[0]["sms_content"].ToString();
                        int count1 = Lblcontent.Text.ToString().TakeWhile(c => c == '~').Count();
                        int count = Lblcontent.Text.ToString().Count(x => x == '~');

                        string testcontent = smstemplate.Rows[0]["sms_content"].ToString();
                        int idx = -1;
                        string idxes = "";

                            DataTable testdt = new DataTable("textboxList");
                            testdt.Clear();
                            testdt.Columns.Add("id");
                        if (count > 0)
                        {
                            

                            for (int i = 0; i < count; i++)
                            {
                                 
                                idx = testcontent.IndexOf("~", idx + 1);
                                testcontent = testcontent.Remove(idx, 1);
                                testcontent = testcontent.Insert(idx, "<b><font color='red'>" + (i + 1).ToString() + "</font></b>");
                                if (idxes == "")
                                {
                                    idxes = idx.ToString();
                                }
                                else
                                {
                                    idxes = idxes + "," + idx.ToString();
                                }
                                DataRow _TextboxList = testdt.NewRow();
                                _TextboxList["id"] = (i + 1).ToString();

                                testdt.Rows.Add(_TextboxList);
                                
                                //TextBox TxtBoxU = new TextBox();

                                

                                //TxtBoxU.ID = "TextBoxU" + i.ToString();
                                
                                //Add the textboxes to the Panel.
                                
                                //Panel1.Controls.Add(TxtBoxU);
                            }


                        }
                        GvTestboxlist.DataSource = testdt;
                        GvTestboxlist.DataBind();
                        Lblcontent.Text = testcontent.ToString();
                        if (testdt.Rows.Count > 0)
                        {
                            Lblsmscontent.Text = "";
                        }
                        else
                        {
                            Lblsmscontent.Text = testcontent.ToString();
                        }
                        
                    }
                    //sshobj.SshDisconnect(client);
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        protected void BtnGetReplaced_OnClick(object sender, EventArgs e)
        {
            try
            {
                //TextBox txt = (TextBox)Panel1.FindControl("TextBoxU1");
                //string s = txt.Text.ToString();

                //foreach (Control c in Panel1.Controls)
                //{
                //    if (c is TextBox)
                //    {
                //        TextBox txt1 = (TextBox)c;
                //        string str = txt1.Text;
                //    }
                //}

                if (HFsms_content.Value != "")
                {
                    int count = HFsms_content.Value.ToString().Count(x => x == '~');

                    string testcontent = HFsms_content.Value.ToString();
                    int idx = -1;
                    string idxes = "";

                    if (count > 0)
                    {
                        int i = 0;
                        //Loop thru all the control available in Panel
                        foreach (GridViewRow row in GvTestboxlist.Rows)
                        {
                            idx = testcontent.IndexOf("~", idx + 1);
                            testcontent = testcontent.Remove(idx, 1);
                            //if ("TextBoxU" + i.ToString().Contains(txtBox.ClientID.ToString()))
                            //{
                            //string s = txtBox.Text.Trim();
                            //}
                            //string s = ((TextBox)Page.FindControl("TextBoxU" + i.ToString())).Text.ToString();
                            testcontent = testcontent.Insert(idx, ((TextBox)row.FindControl("Textboxlist")).Text.ToString());
                            //((TextBox)FindControl("TextBoxU" + i.ToString())).Text.ToString()

                            if (idxes == "")
                            {
                                idxes = idx.ToString();
                            }
                            else
                            {
                                idxes = idxes + "," + idx.ToString();
                            }
                            i++;
                        }
                    }
                    Lblsmscontent.Text = testcontent.ToString();

                }
                else
                {
                    lblerror.Text = "Please select sms type";
                }

            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        public void Sendsms_customer(string sms, GridView Gvlist)
        {
            try
            {
                string path = Server.MapPath("Images/customersms.txt");
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                int i = 0;
                foreach (GridViewRow itm in Gvlist.Rows)
                {
                    if (((CheckBox)itm.FindControl("CBConfirmed")).Checked && ((CheckBox)itm.FindControl("CBConfirmed")).Enabled)
                    {
                        i++;
                        string _smscontent = sms;
                        _smscontent = _smscontent.Replace("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX", itm.Cells[1].Text.ToString());

                        string telephone = ((HiddenField)itm.FindControl("HFTelephone")).Value.ToString();
                        if (telephone.Length >= 10)
                        {
                            telephone = telephone.Substring(telephone.Length - 10, 10);
                            telephone = "91" + telephone;
                            //Lblsmscontent.Text = _smscontent;
                            string output = sendsms(_smscontent, telephone);
                            
                            //if (smsoutput == "")
                            //{
                            //    smsoutput = itm.Cells[1].Text.ToString() + " " + telephone + " " + output;
                            //}
                            //else
                            //{
                            //    smsoutput = smsoutput +"#"+ itm.Cells[1].Text.ToString() + " " + telephone + " " + output;
                            //}
                            
                            //string smsoutput = itm.Cells[1].Text.ToString() + " " + telephone + " " + output;
                            CustomerSMSupdate(itm.Cells[1].Text.ToString(), telephone, output);
                            //if (output.Contains(":"))
                            //{
                            //    output = output.Replace(":", "=");
                            //}
                            //Sentsmsupdate(itm["increment_id"].ToString(), itm["customername"].ToString(), itm["telephone"].ToString(), smstype, output, Convert.ToDateTime(itm["created_at"].ToString()), _smscontent.Length);
                        }
                    }
                    
                }
                //if (smsoutput != "")
                //{
                //    CustomerSMSupdate(smsoutput);
                //}
                if (i == 0)
                {
                    lblerror.Text = "Please select any one customer";
                }
                
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        
        //public void CustomerSMSupdate(string message)
        public void CustomerSMSupdate(string name,string phone,string message)
        {
            try
            {
                
                string path = Server.MapPath("Images/customersms.txt");
                
                string pss1 = string.Format("{0:hh:mm}", DateTime.Now);//System.DateTime.Now.ToShortTimeString();
                FileStream pfs1 = new FileStream(path,
                                        FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter p_streamWriter1 = new StreamWriter(pfs1);
                p_streamWriter1.BaseStream.Seek(0, SeekOrigin.End);
                //p_streamWriter1.WriteLine(pss1 + "FFHP Orders " + " on " + DateTime.Today.Date.ToString("dd:MM:yyyy") + "_" + DateTime.Now.TimeOfDay.ToString()+DateTime.Now.ToString("tt")+" #" + txtidlist.Text.ToString()); p_streamWriter1.Flush();
                p_streamWriter1.WriteLine(DateTime.Today.Date.ToString("dd:MM:yyyy") + "_" + DateTime.Now.TimeOfDay.ToString() + DateTime.Now.ToString("tt") + " #" +name.ToString()+" "+phone.ToString()+" "+ message.ToString()); p_streamWriter1.Flush();
                //p_streamWriter1.WriteLine(DateTime.Today.Date.ToString("dd:MM:yyyy") + "_" + DateTime.Now.TimeOfDay.ToString() + DateTime.Now.ToString("tt") + " #" + message.ToString()); p_streamWriter1.Flush();
                p_streamWriter1.Close();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }

        }
        public string sendsms(string message, string mobilenumber)
        {
            WebClient client = new WebClient();
            //string baseurl = "http://bulksms.mysmsmantra.com:8080/WebSMS/SMSAPI.jsp?username=username&password=password&sendername=sender id&mobileno=919999999999&message=Hello";//Authentication Fail:UserName or Password is incorrect.
            //string baseurl = "http://bulksms.mysmsmantra.com:8080/WebSMS/balance.jsp?username=ffhp&password=169639334";
            //string baseurl = "http://bulksms.mysmsmantra.com:8080/WebSMS/SMSAPI.jsp?username=demouser&password=763475132&sendername=dm&mobileno=918680939328&message=Hello Binarch Test";//DND//Your message is successfully sent to:919999999999
            //string baseurl = "http://bulksms.mysmsmantra.com:8080/WebSMS/sentreport.jsp?username=demouser&password=763475132&fromdate=01-12-2012&todate=30-12-2012";

            string _username = System.Configuration.ConfigurationManager.AppSettings["username"].ToString();
            string _password = System.Configuration.ConfigurationManager.AppSettings["password"].ToString();
            string _senderid = System.Configuration.ConfigurationManager.AppSettings["senderid"].ToString();

            string baseurl = System.Configuration.ConfigurationManager.AppSettings["smslink"].ToString();
            string apiurl = baseurl + "username=" + _username + "&password=" + _password + "&sendername=" + _senderid + "&mobileno=" + mobilenumber + "&message=" + message;//Authentication Fail:UserName or Password is incorrect.

            Stream data = client.OpenRead(apiurl);
            StreamReader reader = new StreamReader(data);
            string s = reader.ReadToEnd().Trim();
            data.Close();
            reader.Close();
            //return "Your message is successfully sent";
            return s;
        }
        public void btndownload_OnClick(object sender, EventArgs e)
        {
            string path = Server.MapPath("Images/customersms.txt");
            string strFileName;
            strFileName = "customersms.txt";
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.AppendHeader("Content-Disposition", String.Format("attachment;filename={0}", strFileName));
            Response.TransmitFile(path);
            Response.End();
        }
        public static DataTable resort(DataTable dt, string colName, string direction)
        {
            dt.DefaultView.Sort = colName + " " + direction;
            dt = dt.DefaultView.ToTable();
            return dt;
        }
    }
}
