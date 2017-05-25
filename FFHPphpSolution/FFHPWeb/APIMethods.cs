using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using MySql.Data.MySqlClient;
using System.util.collections;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Globalization;
using System.Data.SqlClient;
using System.Text;
//using Renci.SshNet;

namespace FFHPWeb
{
    public class APIMethods
    {
        string conn = System.Configuration.ConfigurationManager.AppSettings["Connection"].ToString();
        string smsconn = System.Configuration.ConfigurationManager.AppSettings["SmsDBConnection"].ToString();
        string sqlconn = System.Configuration.ConfigurationManager.AppSettings["SqlConnectionString"].ToString();
        string queryString = "";
        MySqlDataAdapter DA;
        string s = "";
        bool t = false;
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
        public DataTable Getallproducts()
        {
            DataTable allproducts = new DataTable();
            try
            {
                queryString = @"SELECT *
FROM `catalog_product_flat_1` AS a
INNER JOIN `catalog_product_entity` AS b ON a.entity_id = b.entity_id
INNER JOIN `catalog_product_entity_text` AS c ON b.entity_id = c.entity_id
AND c.attribute_id =72
INNER JOIN `cataloginventory_stock_item` AS d ON d.product_id = a.entity_id
INNER JOIN `catalog_product_entity_media_gallery` AS e ON e.entity_id = a.entity_id";

                if (queryString != "")
                {
                    //ConnectSsh sshobj = new ConnectSsh();
                    //SshClient client = sshobj.SshConnect();

                    MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                    DataSet allproductlist = new DataSet();
                    adapteradminmail.Fill(allproductlist, "allproducts");
                    allproducts = allproductlist.Tables[0];
                    //sshobj.SshDisconnect(client);
                }
            }
            catch (Exception ex)
            {
            }
            return allproducts;
        }
        public DataTable Getalacartewithnames(string ordernumber)
        {
            //ConnectSsh sshobj = new ConnectSsh();
            //SshClient client = sshobj.SshConnect();

            DataTable products = new DataTable();
            DataTable products1 = new DataTable();
            try
            {
                if (ordernumber != "")
                {
                    //queryString = "SELECT entity_id, customer_firstname, customer_lastname FROM `sales_flat_order` where created_at between '" + Convert.ToDateTime(TbxFromDate.Text.ToString()).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(TbxToDate.Text.ToString()).ToString("yyyy-MM-dd") + "'";
                    //                    queryString = @"SELECT z.increment_id AS entity_id,  CONCAT( IFNULL( customer_firstname, '' ) , ' ', IFNULL( a.customer_lastname, '' ) ) AS customername, b.Name, CAST((b.weight*b.qty_ordered) AS DECIMAL(12,3)) as weight, '' as units,b.weight as testweight
                    //FROM `sales_flat_order_grid` AS z
                    //INNER JOIN `sales_flat_order` AS a ON z.entity_id = a.entity_id
                    //INNER JOIN `sales_flat_order_item` AS b ON a.entity_id = b.order_id
                    //WHERE b.parent_item_id IS NULL 
                    //AND b.sku LIKE 'a-la%'
                    //AND product_options NOT LIKE '%Additional Qty%'
                    //AND z.increment_id in (" + ordernumber + ")";
                    //queryString = @"SELECT DISTINCT z.increment_id AS entity_id,  CONCAT( IFNULL( c.firstname, '' ) , ' ', IFNULL( c.lastname, '' ) ) AS customername,b.order_id, b.product_id, b.Name, CAST((b.weight*b.qty_ordered) AS DECIMAL(12,3)) as weight, '' as units,CAST((b.weight) AS DECIMAL(12,3)) as testweight,d.value as imagename,a.grand_total,CAST((b.base_original_price) AS DECIMAL(12,3)) as base_original_price,CAST((b.base_original_price/b.weight) AS DECIMAL(12,3))as per1kg_pc,CAST(((b.weight*b.qty_ordered)*(b.base_original_price/b.weight)) AS DECIMAL(12,3)) as Actual_Price,a.subtotal,a.discount_amount,IF(STRCMP(a.status,'pending'),'EBS','COD') as order_status,CAST((b.qty_ordered) AS DECIMAL(12,2)) as qty_ordered,CONCAT( IFNULL( c.street, '' ) , ', ', IFNULL( c.city, '' ) , ', ', IFNULL( c.region, '' ) , ', ', IFNULL( c.postcode, '' ) , ', India, T:', IFNULL( c.telephone, '' ),', ', IFNULL( c.fax, '' ) ) AS Address,IFNULL(c.customer_id,0) as customer_id,IFNULL(c.customer_address_id,0) as customer_address_id,e.shipping_amount,0 as weightcalculation,f.weight_productid,(Case WHEN (select Count(billing_name)from `sales_flat_order_grid` where billing_name=z.billing_name)=1 THEN 'New' Else ''  END) as customer_as
                    queryString = @"SELECT DISTINCT z.increment_id AS entity_id,  CONCAT( IFNULL( c.firstname, '' ) , ' ', IFNULL( c.lastname, '' ) ) AS customername,b.order_id, b.product_id, b.Name, CAST((b.weight*b.qty_ordered) AS DECIMAL(12,3)) as weight, '' as units,CAST((b.weight) AS DECIMAL(12,3)) as testweight,d.value as imagename,a.grand_total,CAST((b.price) AS DECIMAL(12,3)) as base_original_price,CAST((b.price/b.weight) AS DECIMAL(12,3))as per1kg_pc,CAST(((b.weight*b.qty_ordered)*(b.price/b.weight)) AS DECIMAL(12,3)) as Actual_Price,a.subtotal,a.discount_amount,IF(STRCMP(a.status,'pending'),'EBS','COD') as order_status,CAST((b.qty_ordered) AS DECIMAL(12,2)) as qty_ordered,CONCAT( IFNULL( c.street, '' ) , ', ', IFNULL( c.city, '' ) , ', ', IFNULL( c.region, '' ) , ', ', IFNULL( c.postcode, '' ) , ', India, T:', IFNULL( c.telephone, '' ),', ', IFNULL( c.fax, '' ) ) AS Address,IFNULL(c.customer_id,0) as customer_id,IFNULL(c.customer_address_id,0) as customer_address_id,e.shipping_amount,0 as weightcalculation,f.weight_productid,(Case WHEN (select Count(billing_name)from `sales_flat_order_grid` where billing_name=z.billing_name)=1 THEN 'New' Else ''  END) as customer_as
FROM `sales_flat_order_grid` AS z
INNER JOIN `sales_flat_order` AS a ON z.entity_id = a.entity_id
INNER JOIN `sales_flat_order_item` AS b ON a.entity_id = b.order_id
INNER JOIN `sales_flat_order_address` AS c ON c.parent_id = b.order_id AND address_type = 'shipping'
INNER JOIN `catalog_product_entity_media_gallery` AS d ON b.product_id=d.entity_id
INNER JOIN `sales_flat_order_payment` As e on e.parent_id=b.order_id
LEFT OUTER JOIN `1_ffhp_kg_pc_weight` As f on f.productid=b.product_id
WHERE b.parent_item_id IS NULL 
AND b.sku LIKE 'a-la%'
AND product_options NOT LIKE '%Additional Qty%'
AND z.increment_id in (" + ordernumber + ")";
                    //Response.Write(queryString);
                    if (queryString != "")
                    {
                        

                        MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                        DataSet orderlist = new DataSet();
                        adapteradminmail.Fill(orderlist, "sales_flat_order_item");                        
                        products = orderlist.Tables[0];
                        products1 = orderlist.Tables[0];
                        foreach (DataRow row in orderlist.Tables[0].Rows)
                        {
                            if (Convert.ToDecimal(row["testweight"]) < 1)
                            {
                                row["units"] = "kg";
                            }
                            else
                            {
                                //row["units"] = "Piece/Bundle";
                                row["units"] = "pc";
                            }
                        }
                        //var distinctRows = (from DataRow dRow in products.Rows
                        //                    select new { col1 = dRow["increment_id"] }).Distinct();
                        //foreach (var value in distinctRows)
                        //{

                        //}
                        DataSet dttest = new DataSet();
                        dttest = getoptions(ordernumber);
                        if (dttest.Tables.Count > 0)
                        {
                            products.Merge(dttest.Tables[0]);
                        }
                        products = resort(products, "entity_id,Name", "ASC");
                        
                        
                        DataTable dtfree = new DataTable();
                        dtfree = getfree();
                        var distinctRows = (from DataRow dRow in products.Rows
                                            select new { entity_id = dRow["entity_id"], customername = dRow["customername"], order_id = dRow["order_id"], grand_total=dRow["grand_total"],subtotal=dRow["subtotal"],discount_amount=dRow["discount_amount"],order_status=dRow["order_status"],Address=dRow["Address"],customer_id=dRow["customer_id"],customer_address_id=dRow["customer_address_id"],shipping_amount=dRow["shipping_amount"] }).Distinct();
                        foreach (var value in distinctRows)
                        {
                            if (Convert.ToDecimal(value.grand_total) > 0)
                            {
                                DataRow rfree = products1.NewRow();

                                //DataRow rfree = products.NewRow();
                                if (dtfree.Rows.Count > 0)
                                {
                                    rfree["entity_id"] = value.entity_id.ToString();
                                    rfree["customername"] = value.customername.ToString();
                                    rfree["order_id"] = value.order_id.ToString();
                                    rfree["product_id"] = dtfree.Rows[0]["product_id"];
                                    rfree["Name"] = dtfree.Rows[0]["name"].ToString();
                                    rfree["weight"] = Convert.ToDecimal(dtfree.Rows[0]["weight"]);
                                    rfree["units"] = dtfree.Rows[0]["units"];
                                    rfree["testweight"] = Convert.ToDecimal(0);
                                    rfree["imagename"] = "";
                                    rfree["base_original_price"] = Convert.ToDecimal(0);
                                    rfree["per1kg_pc"] = Convert.ToDecimal(0);
                                    rfree["Actual_Price"] = Convert.ToDecimal(0);
                                    
                                    rfree["subtotal"]=Convert.ToDecimal(value.subtotal.ToString());
                                    rfree["discount_amount"]=Convert.ToDecimal(value.discount_amount.ToString());
                                    rfree["order_status"]=value.order_status.ToString();
                                    rfree["qty_ordered"]=Convert.ToDecimal(0);
                                    rfree["Address"]=value.Address.ToString();
                                    rfree["customer_id"] = value.customer_id.ToString();
                                    rfree["customer_address_id"] = value.customer_address_id.ToString();
                                    rfree["shipping_amount"] = Convert.ToDecimal(value.shipping_amount.ToString());
                                    rfree["weightcalculation"] = 0;
                                    if (dtfree.Rows[0]["units"].ToString().Trim().ToLower() == "pc")
                                    {
                                        rfree["weight_productid"] = dtfree.Rows[0]["weight_productid"]; 
                                    }
                                    else
                                    {
                                        //rfree["weight_productid"] = "";
                                    }
                                    rfree["Customer_as"] = "";
                                    products1.Rows.Add(rfree);
                                }
                            }
                        }
                        products1 = resort(products1, "entity_id,Name", "ASC");
                        
                        //a.subtotal,a.discount_amount,IF(STRCMP(a.status,'pending'),'EBS','COD') as order_status,CAST((b.qty_ordered) AS DECIMAL(12,2)) as qty_ordered,CONCAT( IFNULL( c.street, '' ) , ', ', IFNULL( c.city, '' ) , ', ', IFNULL( c.region, '' ) , ', ', IFNULL( c.postcode, '' ) , ', India, T:', IFNULL( c.telephone, '' ),', ', IFNULL( c.fax, '' ) ) AS Address
                        //sshobj.SshDisconnect(client);
                    }
                }
            }
            catch (Exception ex)
            {
                //sshobj.SshDisconnect(client);
            }
            return products1;
        }
        public DataTable get_ffhp_mobile_orders()
        {
            DataTable dtfree = new DataTable("ffhp_mobile_orders");
            queryString = "select fmoid,ordernumber,paymentmethod,DATE_FORMAT( orderdate, '%m/%d/%Y' )as orderdate  from ffhp_mobile_orders";
            if (queryString != "")
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, smsconn);
                adapteradminmail.Fill(dtfree);
                //sshobj.SshDisconnect(client);
            }
            return dtfree;
        }
        public DataTable getfree()
        {
            DataTable dtfree = new DataTable();
            //queryString = "select * from 1_ffhp_free";
            queryString = "SELECT * FROM `1_ffhp_free` as a left outer join `1_ffhp_kg_pc_weight` as b on a.product_id=b.productid";
            if (queryString != "")
            {
                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);
                adapteradminmail.Fill(dtfree);
            }
            return dtfree;
        }
        
        public DataSet getoptions(string ordernumber)
        {
            DataTable dt = new DataTable("finaloptionlist");
            dt.Clear();
            dt.Columns.Add("order_id");
            dt.Columns.Add("product_id");
            dt.Columns.Add("Name");
            dt.Columns.Add("weight");
            dt.Columns.Add("qty_ordered");
            dt.Columns.Add("imagename");

            DataSet orderoptionlist = new DataSet();
            try
            {

                queryString = @"SELECT z.increment_id AS entity_id,  CONCAT( IFNULL( ca.firstname, '' ) , ' ', IFNULL( ca.lastname, '' ) ) AS customername, b.order_id, b.product_id, b.Name, CAST((b.weight * b.qty_ordered) AS DECIMAL(12,3)) as weight, round(b.qty_ordered) As qty_ordered, c.value,'' as units,e.value as imagename
FROM `sales_flat_order_grid` AS z
INNER JOIN `sales_flat_order` AS a ON z.entity_id = a.entity_id
INNER JOIN `sales_flat_order_item` AS b ON z.entity_id = b.order_id
INNER JOIN `sales_flat_order_address` AS ca ON ca.parent_id = b.order_id
AND ca.address_type = 'shipping'
INNER JOIN sales_flat_quote_item_option AS c ON b.quote_item_id = c.item_id
INNER JOIN catalog_product_option_type_title AS d ON c.value = d.option_type_id
INNER JOIN `catalog_product_entity_media_gallery` AS e ON b.product_id=e.entity_id
WHERE b.parent_item_id IS NULL 
AND b.product_type = 'simple'
AND d.title != 'No Need'
AND b.product_options LIKE '%Additional Qty%'
AND z.increment_id in (" + ordernumber.ToString() + ")";
//                queryString = @"SELECT z.increment_id AS entity_id,  CONCAT( IFNULL( ca.firstname, '' ) , ' ', IFNULL( ca.lastname, '' ) ) AS customername, b.order_id, b.product_id, b.Name, CAST((b.weight * b.qty_ordered) AS DECIMAL(12,3)) as weight, round(b.qty_ordered) As qty_ordered, value,'' as units
//FROM `sales_flat_order_grid` AS z
//INNER JOIN `sales_flat_order` AS a ON z.entity_id = a.entity_id
//INNER JOIN `sales_flat_order_item` AS b ON z.entity_id = b.order_id
//INNER JOIN `sales_flat_order_address` AS ca ON ca.parent_id = b.order_id
//AND ca.address_type = 'shipping'
//INNER JOIN sales_flat_quote_item_option AS c ON b.quote_item_id = c.item_id
//INNER JOIN catalog_product_option_type_title AS d ON c.value = d.option_type_id
//WHERE b.parent_item_id IS NULL 
//AND b.product_type = 'simple'
//AND d.title != 'No Need'
//AND b.product_options LIKE '%Additional Qty%'
//AND z.increment_id in (" + ordernumber.ToString() + ")";
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
                            row["units"] = "kg";
                        }
                        else
                        {
                            //row["units"] = "Piece/Bundle";
                            row["units"] = "pc";
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
                        _optionList["imagename"] = row["imagename"].ToString();

                        row["weight"] = weight.ToString();
                    }
                }

            }
            catch (Exception ex)
            {
            }
            return orderoptionlist;
        }
        public static DataTable resort(DataTable dt, string colName, string direction)
        {
            dt.DefaultView.Sort = colName + " " + direction;
            dt = dt.DefaultView.ToTable();
            return dt;
        }

        //Start Delivery Details for Route purpose
        public DataTable getOrderDetailsbyorderid(string ordernumber)
        {
            DataTable testdt = new DataTable("testPackList");
            testdt.Clear();
            testdt.Columns.Add("entity_id");
            testdt.Columns.Add("customername");
            testdt.Columns.Add("Address");
            testdt.Columns.Add("Name");
            testdt.Columns.Add("Zipcode");
            testdt.Columns.Add("Amount");
            testdt.Columns.Add("OrderStatus");
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
                dt.Columns.Add("OrderStatus");
                //queryString = "SELECT entity_id, customer_firstname, customer_lastname FROM `sales_flat_order` where created_at between '" + Convert.ToDateTime(TbxFromDate.Text.ToString()).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(TbxToDate.Text.ToString()).ToString("yyyy-MM-dd") + "'";
                //a.customer_id = c.customer_id AND 
                if (ordernumber != "")
                {
                    //                    queryString = @"SELECT  DISTINCT z.increment_id as entity_id,z.grand_total, a.customer_id, CONCAT( IFNULL(c.firstname,''), ' ', IFNULL(c.lastname,'') ) AS customername, CONCAT( IFNULL( c.street, '' ) , ', ', IFNULL( c.city, '' ) , ', ', IFNULL( c.region, '' ) , ', ', IFNULL( c.postcode, '' ) , ', India, T:', IFNULL( c.telephone, '' ) ) AS Address, b.Name,CONCAT(IFNULL( d.area, '' ), ' ',IFNULL( c.postcode, '' ))as Zipcode,b.qty_ordered,b.sku
                    //FROM `sales_flat_order_grid` as z inner join `sales_flat_order` AS a 
                    //ON z.entity_id=a.entity_id
                    //INNER JOIN `sales_flat_order_item` AS b ON a.entity_id = b.order_id
                    //INNER JOIN `sales_flat_order_address` AS c ON c.parent_id = b.order_id
                    //LEFT OUTER JOIN 1_ffhp_pincode as d ON 
                    //c.postcode=d.pincode
                    //WHERE b.parent_item_id IS NULL
                    //AND product_type
                    //IN (
                    //'simple', 'bundle', 'grouped'
                    //) AND c.address_type = 'shipping' AND z.increment_id in (" + txtidlist.Text + ")";

                    queryString = @"SELECT DISTINCT z.increment_id as entity_id,z.grand_total, a.customer_id, CONCAT( IFNULL(c.firstname,''), ' ', IFNULL(c.lastname,'') ) AS customername, CONCAT( IFNULL( c.street, '' ) , ', ', IFNULL( c.city, '' ) , ', ', IFNULL( c.region, '' ) , ', ', IFNULL( c.postcode, '' ) , ', India, T:', IFNULL( c.telephone, '' ) ) AS Address, b.Name,CONCAT(IFNULL( c.postcode, '000000' ), ' ',IFNULL( d.area, '' ))as Zipcode,b.qty_ordered,b.sku,IF(STRCMP(a.status,'pending'),'EBS','COD') as order_status
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
) AND c.address_type = 'shipping' AND b.sku not like 'A-la-c%' AND z.increment_id in (" + ordernumber + ")" + @"

UNION ALL

SELECT DISTINCT z.increment_id as entity_id,z.grand_total, a.customer_id, CONCAT( IFNULL(c.firstname,''), ' ', IFNULL(c.lastname,'') ) AS customername, CONCAT( IFNULL( c.street, '' ) , ', ', IFNULL( c.city, '' ) , ', ', IFNULL( c.region, '' ) , ', ', IFNULL( c.postcode, '' ) , ', India, T:', IFNULL( c.telephone, '' ) ) AS Address, 'alacarte' As Name,CONCAT(IFNULL( c.postcode, '000000' ), ' ',IFNULL( d.area, '' ))as Zipcode,CAST( 1 AS DECIMAL( 10, 3 ) ) AS qty_ordered,'alacarte' as sku,IF(STRCMP(a.status,'pending'),'EBS','COD') as order_status
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
) AND c.address_type = 'shipping' AND b.sku like 'A-la-c%' AND z.increment_id in (" + ordernumber + ")";

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
                        string orderstatus = "";
                        Decimal Amount = 0;
                        Decimal TotalAmount = 0;
                        int qty = 0;
                        string NamewithOptions = "";
                        DataTable dtoptions = null;
                        DataSet dsYOYOopt = new DataSet();
                        DataSet dsREADYopt = new DataSet();
                        dsYOYOopt = getYOYOwithOptions(ordernumber);
                        dsREADYopt = getREADYwithOptions(ordernumber);
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
                                orderstatus = row["order_status"].ToString();
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
                            _TestPackList["OrderStatus"] = orderstatus.ToString();
                            _TestPackList["Amount"] = Amount.ToString("#,##0.00");

                            //int c = Regex.Matches(ReadLastCODorders(), Entityid.ToString()).Count;
                            //if (c == 1)
                            //{
                            //    _TestPackList["Amount"] = Amount.ToString("#,##0.00");//String.Format("{ 0:C}", Amount.ToString()); //(Amount.ToString());
                            //    TotalAmount = TotalAmount + Amount;
                            //}
                            //else
                            //{
                            //    _TestPackList["Amount"] = "";
                            //}
                            testdt.Rows.Add(_TestPackList);
                            TestPack = "";


                        }
                        
                        testdt = resort(testdt, "entity_id", "ASC");
                        //sshobj.SshDisconnect(client);
                    }
                }
            }
            catch (Exception ex)
            {
                testdt = null; 
            }
            return testdt;
        }
        public DataTable getOrderDetailsbyorderid(int route_id)
        {
            string ordernumber = "";
            DataSet ds = new DataSet();
            RouteOrder objroute = new RouteOrder();
            ds = objroute.getroutelistone(route_id);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ordernumber = ds.Tables[0].Rows[0]["ordernumber"].ToString();
                }
            }

            DataTable testdt = new DataTable("testPackList");
            testdt.Clear();
            testdt.Columns.Add("entity_id");
            testdt.Columns.Add("customername");
            testdt.Columns.Add("Address");
            testdt.Columns.Add("Name");
            testdt.Columns.Add("Zipcode");
            testdt.Columns.Add("Amount");
            testdt.Columns.Add("OrderStatus");
            try
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();

                DataTable dt = new DataTable("PackList");
                dt.Clear();
                dt.Columns.Add("entity_id");
                dt.Columns.Add("customername");
                dt.Columns.Add("Address");
                dt.Columns.Add("Name");
                dt.Columns.Add("Zipcode");
                dt.Columns.Add("Amount");
                dt.Columns.Add("OrderStatus");
                //queryString = "SELECT entity_id, customer_firstname, customer_lastname FROM `sales_flat_order` where created_at between '" + Convert.ToDateTime(TbxFromDate.Text.ToString()).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(TbxToDate.Text.ToString()).ToString("yyyy-MM-dd") + "'";
                //a.customer_id = c.customer_id AND 
                if (ordernumber != "")
                {
                    //                    queryString = @"SELECT  DISTINCT z.increment_id as entity_id,z.grand_total, a.customer_id, CONCAT( IFNULL(c.firstname,''), ' ', IFNULL(c.lastname,'') ) AS customername, CONCAT( IFNULL( c.street, '' ) , ', ', IFNULL( c.city, '' ) , ', ', IFNULL( c.region, '' ) , ', ', IFNULL( c.postcode, '' ) , ', India, T:', IFNULL( c.telephone, '' ) ) AS Address, b.Name,CONCAT(IFNULL( d.area, '' ), ' ',IFNULL( c.postcode, '' ))as Zipcode,b.qty_ordered,b.sku
                    //FROM `sales_flat_order_grid` as z inner join `sales_flat_order` AS a 
                    //ON z.entity_id=a.entity_id
                    //INNER JOIN `sales_flat_order_item` AS b ON a.entity_id = b.order_id
                    //INNER JOIN `sales_flat_order_address` AS c ON c.parent_id = b.order_id
                    //LEFT OUTER JOIN 1_ffhp_pincode as d ON 
                    //c.postcode=d.pincode
                    //WHERE b.parent_item_id IS NULL
                    //AND product_type
                    //IN (
                    //'simple', 'bundle', 'grouped'
                    //) AND c.address_type = 'shipping' AND z.increment_id in (" + txtidlist.Text + ")";

                    queryString = @"SELECT DISTINCT z.increment_id as entity_id,z.grand_total, a.customer_id, CONCAT( IFNULL(c.firstname,''), ' ', IFNULL(c.lastname,'') ) AS customername, CONCAT( IFNULL( c.street, '' ) , ', ', IFNULL( c.city, '' ) , ', ', IFNULL( c.region, '' ) , ', ', IFNULL( c.postcode, '' ) , ', India, T:', IFNULL( c.telephone, '' ) ) AS Address, b.Name,CONCAT(IFNULL( c.postcode, '000000' ), ' ',IFNULL( d.area, '' ))as Zipcode,b.qty_ordered,b.sku,IF(STRCMP(a.status,'pending'),'EBS','COD') as order_status
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
) AND c.address_type = 'shipping' AND b.sku not like 'A-la-c%' AND z.increment_id in (" + ordernumber + ")" + @"

UNION ALL

SELECT DISTINCT z.increment_id as entity_id,z.grand_total, a.customer_id, CONCAT( IFNULL(c.firstname,''), ' ', IFNULL(c.lastname,'') ) AS customername, CONCAT( IFNULL( c.street, '' ) , ', ', IFNULL( c.city, '' ) , ', ', IFNULL( c.region, '' ) , ', ', IFNULL( c.postcode, '' ) , ', India, T:', IFNULL( c.telephone, '' ) ) AS Address, 'alacarte' As Name,CONCAT(IFNULL( c.postcode, '000000' ), ' ',IFNULL( d.area, '' ))as Zipcode,CAST( 1 AS DECIMAL( 10, 3 ) ) AS qty_ordered,'alacarte' as sku,IF(STRCMP(a.status,'pending'),'EBS','COD') as order_status
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
) AND c.address_type = 'shipping' AND b.sku like 'A-la-c%' AND z.increment_id in (" + ordernumber + ")";

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
                        string orderstatus = "";
                        Decimal Amount = 0;
                        Decimal TotalAmount = 0;
                        int qty = 0;
                        string NamewithOptions = "";
                        DataTable dtoptions = null;
                        DataSet dsYOYOopt = new DataSet();
                        DataSet dsREADYopt = new DataSet();
                        dsYOYOopt = getYOYOwithOptions(ordernumber);
                        dsREADYopt = getREADYwithOptions(ordernumber);
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
                                orderstatus = row["order_status"].ToString();
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
                            _TestPackList["OrderStatus"] = orderstatus.ToString();
                            _TestPackList["Amount"] = Amount.ToString("#,##0.00");

                            //int c = Regex.Matches(ReadLastCODorders(), Entityid.ToString()).Count;
                            //if (c == 1)
                            //{
                            //    _TestPackList["Amount"] = Amount.ToString("#,##0.00");//String.Format("{ 0:C}", Amount.ToString()); //(Amount.ToString());
                            //    TotalAmount = TotalAmount + Amount;
                            //}
                            //else
                            //{
                            //    _TestPackList["Amount"] = "";
                            //}
                            testdt.Rows.Add(_TestPackList);
                            TestPack = "";


                        }

                        testdt = resort(testdt, "entity_id", "ASC");
                        
                    }
                }
                //sshobj.SshDisconnect(client);
            }
            catch (Exception ex)
            {
                testdt = null;
            }
            return testdt;
        }
        public DataSet getYOYOwithOptions(string ordernumber)
        {
            DataSet yoyowopt = new DataSet();
            try
            {
                queryString = @"SELECT DISTINCT z.increment_id as order_id,c.title, a.Name
FROM `sales_flat_order_grid` as z inner join `sales_flat_order_item` AS a
ON z.entity_id=a.order_id
INNER JOIN `catalog_product_bundle_selection` AS b ON a.product_id = b.product_id
INNER JOIN `catalog_product_bundle_option_value` AS c ON b.option_id = c.option_id
WHERE a.Name NOT LIKE '%t Want To Buy This Product/' and c.title LIKE '%(Optional)' and z.increment_id in (" + ordernumber + ")";
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
        public DataSet getREADYwithOptions(string ordernumber)
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

and z.increment_id in (" + ordernumber + ")";

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
        public string ReadLastCODorders()
        {
            string CODordernumber = "";
            try
            {
                string path = System.Web.Hosting.HostingEnvironment.MapPath("Images/CODOrderNumbers.txt");
                //string path = System.Web.Server.MapPath("Images/CODOrderNumbers.txt");
                if (File.Exists(path))
                {
                    string ord = File.ReadAllLines(path).Last();
                    string[] Orders = ord.Split('#');
                    if (Orders.Count() > 0)
                    {
                        CODordernumber = Orders.Last();
                    }
                    if (CODordernumber.ToString() == "0")
                    {
                        CODordernumber = "";
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return CODordernumber;
        }
        public DataTable getProduct_Weight_Price()
        {
            DataTable dtfree = new DataTable("Product_Weight_Price");
            queryString = "SELECT entity_id, price, weight FROM `catalog_product_flat_1` ORDER BY entity_id";
            if (queryString != "")
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);
                adapteradminmail.Fill(dtfree);
                //sshobj.SshDisconnect(client);
            }
            return dtfree;
        }
        //End Delivery Details for Route purpose
        public DataTable getProduct_Weight_Price_for_Before_After_Sale()
        {
            DataTable dtfree = new DataTable("Product_Weight_Price");
            //queryString = "SELECT '100001' as ordernumber,'FFHP' as customername,entity_id as product_id,name, price, weight,CAST((price/weight) AS DECIMAL(12,3))as per1kg_pc,IF( weight <1, 'kg', 'pc' ) AS unit FROM `catalog_product_flat_1` ORDER BY entity_id";
            queryString = @"SELECT 'LocalSale' as ordernumber,'FFHP' as customername,a.entity_id as product_id, a.name, CAST((a.price) AS DECIMAL(12,2)) as price, CAST((a.weight) AS DECIMAL(12,3)) as weight, CAST( (
a.price / a.weight
) AS DECIMAL( 12, 2 ) ) AS priceperkgpc,  IF( a.weight <1, 'kg', 'pc' ) AS unit, b.value AS imagename,CURDATE() as date
FROM `catalog_product_flat_1` AS a
INNER JOIN (select distinct value,entity_id from `catalog_product_entity_media_gallery` group by entity_id) AS b ON a.entity_id = b.entity_id ORDER BY a.name";
            if (queryString != "")
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);
                adapteradminmail.Fill(dtfree);
                //sshobj.SshDisconnect(client);
            }
            return dtfree;
        }
        //Tray Loading
        public DataTable getTrayloading()
        {
            DataTable dttrayload = new DataTable("TrayLoading");
            //queryString = "SELECT '100001' as ordernumber,'FFHP' as customername,entity_id as product_id,name, price, weight,CAST((price/weight) AS DECIMAL(12,3))as per1kg_pc,IF( weight <1, 'kg', 'pc' ) AS unit FROM `catalog_product_flat_1` ORDER BY entity_id";
            queryString = @"SELECT * from ffhp_tray_loading where date=DATE(NOW())";
            if (queryString != "")
            {
                MySqlConnection con = new MySqlConnection(smsconn);
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, con);
                adapteradminmail.Fill(dttrayload);
                //sshobj.SshDisconnect(client);
            }
            return dttrayload;
        } 
        //public int update_tray_loading(string query)
        //{
        //    int i = 0;
        //    queryString = query;
        //    if (queryString != "")
        //    {
        //        MySqlConnection con = new MySqlConnection(smsconn);
        //        con.Open();
        //        MySqlCommand cmd = new MySqlCommand(queryString, con);
        //        i = cmd.ExecuteNonQuery();
        //    }
        //    return i;
        //}
        public int update_tray_loading(string jsonstring)
        {
            //DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonstring);
            DataTable dt = ConvertJSONToDataTable(jsonstring);

            MySqlConnection con = new MySqlConnection(smsconn);
            int i = 0;
            try
            {
                con.Open();
                foreach (DataRow row in dt.Rows)
                {

                    string queryString = "";
                    //queryString = @"update ffhp_tray_loading set name='" + row[2].ToString().Trim() + "',status='" + row[3].ToString().Trim() + "',unit='" + row[4].ToString().Trim() + "',weight='" + row[5].ToString().Trim() + "',cust_name='" + row[6].ToString().Trim() + "',url='" + row[7].ToString().Trim() + "',pack_status='" + row[8].ToString().Trim() + "',order_status='" + row[9].ToString().Trim() + "',route='" + row[10].ToString().Trim() + "',date=DATE(NOW()) where order_no='" + row[0].ToString().Trim() + "' and product_id='" + row[1].ToString().Trim() + "'";
                    queryString = @"update ffhp_tray_loading set name='" + row["name"].ToString().Replace("'", "").Trim() + "',status='" + row["status"].ToString().Trim() + "',unit='" + row["unit"].ToString().Trim() + "',weight='" + row["weight"].ToString().Trim() + "',cust_name='" + row["cust_name"].ToString().Replace("'", "").Trim() + "',url='" + row["url"].ToString().Trim() + "',pack_status='" + row["pack_status"].ToString().Trim() + "',order_status='" + row["order_status"].ToString().Trim() + "',route='" + row["route"].ToString().Trim() + "',date=DATE(NOW()) where order_no='" + row["order_no"].ToString().Trim() + "' and product_id='" + row["product_id"].ToString().Trim() + "'";
                    //ConnectSsh sshobj = new ConnectSsh();
                    //SshClient client = sshobj.SshConnect();
                    MySqlCommand cmd = new MySqlCommand(queryString, con);
                    i = cmd.ExecuteNonQuery();
                    //sshobj.SshDisconnect(client);
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return i;
        }
        public int insert_tray_laoding(string jsonstring)
        {

            //DataTable dt = ConvertJSONToDataTable(jsonstring);//JsonStringToDataTable(jsonstring);
            //Newtonsoft.Json.Converters.DataTableConverter obj = new Newtonsoft.Json.Converters.DataTableConverter();
            //DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonstring);

            DataTable dt = (DataTable)JsonConvert.DeserializeObject(jsonstring, (typeof(DataTable)));
            MySqlConnection con = new MySqlConnection(smsconn);
            int i = 0;
            try
            {
                con.Open();
                foreach (DataRow row in dt.Rows)
                {
                    
                    string queryString = "";//
                    queryString = @"Insert into ffhp_tray_loading (order_no,product_id,name,status,unit,weight,cust_name,url,pack_status,order_status,route,date)values(
'" + row["order_no"].ToString().Trim() + "'," + row["product_id"].ToString().Trim() + ",'" + row["name"].ToString().Replace("'", "").Trim() + "','" + row["status"].ToString().Trim() + "','" + row["unit"].ToString().Trim() + "','" + row["weight"].ToString().Trim() + "','" + row["cust_name"].ToString().Replace("'", "").Trim() + "','" + row["url"].ToString().Trim() + "','" + row["pack_status"].ToString().Trim() + "','" + row["order_status"].ToString().Trim() + "','" + row["route"].ToString().Trim() + "',DATE(NOW()))";
                    //string order_no1 = row[0].ToString();
                    //string order_no = row["product_id"].ToString();

                    //queryString = @"Insert into ffhp_tray_loading (order_no,product_id,name,status,unit,weight,cust_name,url,pack_status,order_status,route,date)values(
                    //'" + row[0].ToString().Trim() + "'," + row[1].ToString().Trim() + ",'" + row[2].ToString().Trim() + "','" + row[3].ToString().Trim() + "','" + row[4].ToString().Trim() + "','" + row[5].ToString().Trim() + "','" + row[6].ToString().Trim() + "','" + row[7].ToString().Trim() + "','" + row[8].ToString().Trim() + "','" + row[9].ToString().Trim() + "','" + row[10].ToString().Trim() + "',DATE(NOW()))";//,STR_TO_DATE('" + String.Format("{0:MM/dd/yyyy}", lastupdatedate) + "', '%c-%e-%Y')

                    //ConnectSsh sshobj = new ConnectSsh();
                    //SshClient client = sshobj.SshConnect();
                    MySqlCommand cmd = new MySqlCommand(queryString, con);
                    i = cmd.ExecuteNonQuery();
                    //sshobj.SshDisconnect(client);
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return i;
        }
        public int update_Delivery_Status(string jsonstring)
        {
            //DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonstring);
            DataTable dt = ConvertJSONToDataTable(jsonstring);

            MySqlConnection con = new MySqlConnection(smsconn);
            int i = 0;
            try
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                con.Open();
                foreach (DataRow row in dt.Rows)
                {
                    DateTime dtf = DateTime.ParseExact(row["date"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    string queryString = "";
                    //queryString = @"update ffhp_tray_loading set name='" + row[2].ToString().Trim() + "',status='" + row[3].ToString().Trim() + "',unit='" + row[4].ToString().Trim() + "',weight='" + row[5].ToString().Trim() + "',cust_name='" + row[6].ToString().Trim() + "',url='" + row[7].ToString().Trim() + "',pack_status='" + row[8].ToString().Trim() + "',order_status='" + row[9].ToString().Trim() + "',route='" + row[10].ToString().Trim() + "',date=DATE(NOW()) where order_no='" + row[0].ToString().Trim() + "' and product_id='" + row[1].ToString().Trim() + "'";
                    //queryString = @"update ffhp_tray_loading set name='" + row["name"].ToString().Replace("'", "").Trim() + "',status='" + row["status"].ToString().Trim() + "',unit='" + row["unit"].ToString().Trim() + "',weight='" + row["weight"].ToString().Trim() + "',cust_name='" + row["cust_name"].ToString().Replace("'", "").Trim() + "',url='" + row["url"].ToString().Trim() + "',pack_status='" + row["pack_status"].ToString().Trim() + "',order_status='" + row["order_status"].ToString().Trim() + "',route='" + row["route"].ToString().Trim() + "',date=DATE(NOW()) where order_no='" + row["order_no"].ToString().Trim() + "' and product_id='" + row["product_id"].ToString().Trim() + "'";
                    queryString = @"update ffhp_delivery_status set latitude='" + row["latitude"].ToString().Trim() + "',longitude='" + row["longitude"].ToString().Trim() + "',area='" + row["area"].ToString().Trim() + "',payment_status='" + row["payment_status"].ToString().Trim() + "',paid_amount='" + row["paid_amount"].ToString().Trim() + "',refund='" + row["refund"].ToString().Trim() + "',start_time='" + row["start_time"].ToString().Trim() + "',delivery_time='" + row["delivery_time"].ToString().Trim() + "',duration='" + row["duration"].ToString().Trim() + "',kilometer='" + row["kilometer"].ToString().Trim() + "',deliveryboy_name='" + row["deliveryboy_name"].ToString().Trim() + "',deliveryboy_ph='" + row["deliveryboy_ph"].ToString().Trim() + "',delivery_status='" + row["delivery_status"].ToString().Trim() + "',discription='" + row["discription"].ToString().Trim() + "',delivery_sequence='" + row["delivery_sequence"].ToString().Trim() + "' where order_number='" + row["order_number"].ToString().Trim() + "' and date='" + String.Format("{0:yyyy/MM/dd}", dtf) + "'";

                    MySqlCommand cmd = new MySqlCommand(queryString, con);
                    i = cmd.ExecuteNonQuery();
                }
                con.Close();
                //sshobj.SshDisconnect(client);
            }
            catch (Exception ex)
            {

            }
            return i;
        }
        public DataSet Get_Delivery_Status(DateTime fromdate, DateTime todate)
        {
            string queryString = "";
            DataSet ffhp_delivery_status = new DataSet();
            queryString = @"SELECT customer_id,
customer_address_id,
customer_name,
order_number,
route_number,
DATE_FORMAT( datescanned, '%m/%d/%Y' )as datescanned,
TIME_TO_SEC(timescanned)as timescanned,
order_amount,
round(billed_amount)as billed_amount,
payment_mode,
no_of_bags,
latitude,
longitude,
area,
payment_status,
CASE WHEN paid_amount is null THEN 0 else paid_amount END as paid_amount,
refund,
TIME_TO_SEC(start_time) as start_time,
TIME_TO_SEC(delivery_time) as delivery_time,
duration,
kilometer,
deliveryboy_name,
deliveryboy_ph,
delivery_status,
discription,
DATE_FORMAT( date, '%m/%d/%Y' )as date,
delivery_sequence
FROM `ffhp_delivery_status` 
where date between '" + String.Format("{0:yyyy/MM/dd}", fromdate) + "' and '" + String.Format("{0:yyyy/MM/dd}", todate) + "' ORDER BY route_number, payment_mode DESC";
//where date ='" + String.Format("{0:yyyy/MM/dd}", fromdate) + "' ORDER BY route_number, payment_mode DESC";

            if (queryString != "")
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                MySqlConnection con = new MySqlConnection(smsconn);
                con.Open();

                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, smsconn);

                adapteradminmail.Fill(ffhp_delivery_status, "ffhp_delivery_status");
                con.Close();
                //sshobj.SshDisconnect(client);
            }

            return ffhp_delivery_status;
        }
        public DataSet Get_Delivery_Status_Ordernumber(string ordernumber)
        {
            string queryString = "";
            DataSet ffhp_delivery_status = new DataSet();
            queryString = @"SELECT customer_id,
customer_address_id,
customer_name,
order_number,
route_number,
DATE_FORMAT( datescanned, '%m/%d/%Y' )as datescanned,
TIME_TO_SEC(timescanned)as timescanned,
order_amount,
round(billed_amount)as billed_amount,
payment_mode,
no_of_bags,
latitude,
longitude,
area,
payment_status,
CASE WHEN paid_amount is null THEN 0 else paid_amount END as paid_amount,
refund,
TIME_TO_SEC(start_time) as start_time,
TIME_TO_SEC(delivery_time) as delivery_time,
duration,
kilometer,
deliveryboy_name,
deliveryboy_ph,
delivery_status,
discription,
DATE_FORMAT( date, '%m/%d/%Y' )as date,
delivery_sequence
FROM `ffhp_delivery_status` 
where order_number in (" + ordernumber + ") ORDER BY route_number, payment_mode DESC";
            //where date ='" + String.Format("{0:yyyy/MM/dd}", fromdate) + "' ORDER BY route_number, payment_mode DESC";

            if (queryString != "")
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                MySqlConnection con = new MySqlConnection(smsconn);
                con.Open();

                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, smsconn);

                adapteradminmail.Fill(ffhp_delivery_status, "ffhp_delivery_status");
                con.Close();
                //sshobj.SshDisconnect(client);
            }

            return ffhp_delivery_status;
        }
        public DataSet Get_Delivery_Status(string wherecondition)
        {
            string queryString = "";
            DataSet ffhp_delivery_status = new DataSet();
            queryString = @"SELECT customer_id,
customer_address_id,
customer_name,
order_number,
route_number,
DATE_FORMAT( datescanned, '%m/%d/%Y' )as datescanned,
TIME_TO_SEC(timescanned)as timescanned,
order_amount,
billed_amount,
payment_mode,
no_of_bags,
latitude,
longitude,
area,
payment_status,
CASE WHEN paid_amount is null THEN 0 else paid_amount END as paid_amount,
refund,
TIME_TO_SEC(start_time) as start_time,
TIME_TO_SEC(delivery_time) as delivery_time,
duration,
kilometer,
deliveryboy_name,
deliveryboy_ph,
delivery_status,
discription,
DATE_FORMAT( date, '%m/%d/%Y' )as date,
delivery_sequence
FROM `ffhp_delivery_status` " + wherecondition + " ORDER BY Date, payment_mode DESC";
            //where date between '" + String.Format("{0:yyyy/MM/dd}", fromdate) + "' and '" + String.Format("{0:yyyy/MM/dd}", todate) + "' ORDER BY route_number, payment_mode DESC";
            if (queryString != "")
            {
                MySqlConnection con = new MySqlConnection(smsconn);
                con.Open();

                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, smsconn);

                adapteradminmail.Fill(ffhp_delivery_status, "ffhp_delivery_status");
                con.Close();
            }

            return ffhp_delivery_status;
        }
        public DataSet Get_Delivery_Status_ordernumber(string ordernumber)
        {
            string queryString = "";
            DataSet ffhp_delivery_status = new DataSet();
            queryString = @"SELECT customer_id,
customer_address_id,
customer_name,
order_number,
route_number,
DATE_FORMAT( datescanned, '%m/%d/%Y' )as datescanned,
DATE_FORMAT( datescanned, '%Y/%m/%d' )as datescannededit,
TIME_TO_SEC(timescanned)as timescanned,
order_amount,
billed_amount,
payment_mode,
no_of_bags,
latitude,
longitude,
area,
payment_status,
CASE WHEN paid_amount is null THEN 0 else paid_amount END as paid_amount,
refund,
TIME_TO_SEC(start_time) as start_time,
TIME_TO_SEC(delivery_time) as delivery_time,
duration,
kilometer,
deliveryboy_name,
deliveryboy_ph,
delivery_status,
discription,
DATE_FORMAT( date, '%m/%d/%Y' )as date 
FROM `ffhp_delivery_status` where order_number='" + ordernumber + "' ORDER BY Date, payment_mode DESC";
            //where date between '" + String.Format("{0:yyyy/MM/dd}", fromdate) + "' and '" + String.Format("{0:yyyy/MM/dd}", todate) + "' ORDER BY route_number, payment_mode DESC";
            if (queryString != "")
            {
                MySqlConnection con = new MySqlConnection(smsconn);
                con.Open();

                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, smsconn);

                adapteradminmail.Fill(ffhp_delivery_status, "ffhp_delivery_status");
                con.Close();
            }

            return ffhp_delivery_status;
        }
        public int Set_Delivery_Status_ordernumber(string ordernumber,string date,string payment_status,string delivery_status,string description)
        {
            int i = 0;
            try
            {
                string queryString = "";
                DataSet ffhp_delivery_status = new DataSet();
                DateTime dtf = DateTime.ParseExact(date.ToString(), "yyyy/MM/dd", CultureInfo.InvariantCulture);
                queryString = @"Update `ffhp_delivery_status` set payment_status='" + payment_status + "',delivery_status='" + delivery_status + "',discription='" + description.Replace("\"", string.Empty).Replace("'", string.Empty) +"' where datescanned='" + date + "' and order_number='" + ordernumber + "'";
                //where date between '" + String.Format("{0:yyyy/MM/dd}", fromdate) + "' and '" + String.Format("{0:yyyy/MM/dd}", todate) + "' ORDER BY route_number, payment_mode DESC";
                if (queryString != "")
                {
                    MySqlConnection con = new MySqlConnection(smsconn);
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(queryString, con);
                    i = cmd.ExecuteNonQuery();
                    con.Close();
                }
                
            }
            catch (Exception ex)
            {
            }

            return i;
        }
        public DataTable Get_delivery_status_new()
        {
            DataTable ffhp_delivery_status = new DataTable();
            MySqlConnection con = new MySqlConnection(smsconn);

            MySqlCommand cmd = new MySqlCommand("sp_get_ffhp_delivery_status",con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Connection.Open();

            

            //MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            IDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            ffhp_delivery_status.Load(dr);

            return ffhp_delivery_status;
        }
        public int Set_Route_Start(string routename)
        {
            

            MySqlConnection con = new MySqlConnection(smsconn);
            int i = 0;
            try
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                con.Open();
                
                DateTime CDT = DateTime.Now.Date; 
                    string queryString = "";
                    //queryString = @"update ffhp_tray_loading set name='" + row[2].ToString().Trim() + "',status='" + row[3].ToString().Trim() + "',unit='" + row[4].ToString().Trim() + "',weight='" + row[5].ToString().Trim() + "',cust_name='" + row[6].ToString().Trim() + "',url='" + row[7].ToString().Trim() + "',pack_status='" + row[8].ToString().Trim() + "',order_status='" + row[9].ToString().Trim() + "',route='" + row[10].ToString().Trim() + "',date=DATE(NOW()) where order_no='" + row[0].ToString().Trim() + "' and product_id='" + row[1].ToString().Trim() + "'";
                    //queryString = @"update ffhp_tray_loading set name='" + row["name"].ToString().Replace("'", "").Trim() + "',status='" + row["status"].ToString().Trim() + "',unit='" + row["unit"].ToString().Trim() + "',weight='" + row["weight"].ToString().Trim() + "',cust_name='" + row["cust_name"].ToString().Replace("'", "").Trim() + "',url='" + row["url"].ToString().Trim() + "',pack_status='" + row["pack_status"].ToString().Trim() + "',order_status='" + row["order_status"].ToString().Trim() + "',route='" + row["route"].ToString().Trim() + "',date=DATE(NOW()) where order_no='" + row["order_no"].ToString().Trim() + "' and product_id='" + row["product_id"].ToString().Trim() + "'";
                    queryString = @"update ffhp_delivery_status set start_flag=1 where route_number='" + routename.ToString().Trim() + "' and date='" + CDT.ToString("yyyy-MM-dd") + "'";

                    MySqlCommand cmd = new MySqlCommand(queryString, con);
                    i = cmd.ExecuteNonQuery();
                    con.Close();

                    insert_update_ffhp_route_orders_history(routename);

                    //code for one time send data for ffhp route orders history
                    //queryString = @"select count(*) from `ffhp_route_orders_history` where route_id='"+routename.ToString().Trim()+"' and updateddate between '" + CDT.ToString("yyyy-MM-dd") + "' and '" + CDT.ToString("yyyy-MM-dd") + "'";
                    //queryString = @"INSERT INTO `ffhp_route_orders_history` ( route_id, route_name, ordernumber, driver_name, driver_phone, deliveryboy_name, deliveryboy_phone ) 
//(SELECT route_id, route_name, ordernumber, driver_name, driver_phone, deliveryboy_name, deliveryboy_phone
//FROM ffhp_route_orders
//WHERE ordernumber IS NOT NULL
//AND ordernumber != '' AND route_id =Routename)";

                    //sshobj.SshDisconnect(client);
            }
            catch (Exception ex)
            {

            }
            return i;
        }
        public int insert_update_ffhp_route_orders_history(string routename1)
        {
            int i = 0;
            try
            {
                MySqlConnection con = new MySqlConnection(smsconn);
                con.Open();
                DateTime CDT = DateTime.Now.Date;
                string queryString = "";
                DataTable dt = new DataTable();
                string routename="";
                queryString = @"select route_id from `ffhp_route_orders` where route_name='"+routename1+"'";

                MySqlCommand cmdget = new MySqlCommand(queryString, con);
                routename = cmdget.ExecuteScalar().ToString();
                if (routename != "")
                {
                    queryString = @"select count(*) from `ffhp_route_orders_history` where route_id=" + routename.ToString().Trim() + " and updateddate between '" + CDT.ToString("yyyy-MM-dd") + "' and '" + CDT.AddDays(1).ToString("yyyy-MM-dd") + "'";
                    MySqlCommand cmd = new MySqlCommand(queryString, con);

                    if (cmd.ExecuteScalar().ToString() == "0")
                    {
                        queryString = @"INSERT INTO `ffhp_route_orders_history` ( route_id, route_name, ordernumber, driver_name, driver_phone, deliveryboy_name, deliveryboy_phone ) 
(SELECT route_id, route_name, ordernumber, driver_name, driver_phone, deliveryboy_name, deliveryboy_phone
FROM ffhp_route_orders
WHERE ordernumber IS NOT NULL
AND ordernumber != '' AND route_id =" + routename.ToString().Trim() + ")";
                        MySqlCommand cmdinsert = new MySqlCommand(queryString, con);
                        i = cmdinsert.ExecuteNonQuery();
                    }
                    else
                    {
                        queryString = @"SELECT route_id, route_name, ordernumber, driver_name, driver_phone, deliveryboy_name, deliveryboy_phone
FROM ffhp_route_orders
WHERE ordernumber IS NOT NULL
AND ordernumber != '' AND route_id =" + routename.ToString().Trim() + "";

                        MySqlDataAdapter adapterrouteorders = new MySqlDataAdapter(queryString, smsconn);

                        adapterrouteorders.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {

                            queryString = @"update `ffhp_route_orders_history` set route_id=" + dt.Rows[0]["route_id"].ToString() + ", route_name='" + dt.Rows[0]["route_name"].ToString() + "', ordernumber='" + dt.Rows[0]["ordernumber"].ToString() + "', driver_name='" + dt.Rows[0]["driver_name"].ToString() + "', driver_phone='" + dt.Rows[0]["driver_phone"].ToString() + "', deliveryboy_name='" + dt.Rows[0]["deliveryboy_name"].ToString() + "', deliveryboy_phone='" + dt.Rows[0]["deliveryboy_phone"].ToString() + "' where route_id =" + routename.ToString().Trim() + " and updateddate between '" + CDT.ToString("yyyy-MM-dd") + "' and '" + CDT.AddDays(1).ToString("yyyy-MM-dd") + "'";
                            MySqlCommand cmdupdate = new MySqlCommand(queryString, con);
                            i = cmdupdate.ExecuteNonQuery();
                        }
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return i;
        }
        public DataTable JsonStringToDataTable(string jsonString)
        {
            

            DataTable dt = new DataTable();
            string[] jsonStringArray = Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{");
            List<string> ColumnsName = new List<string>();
            foreach (string jSA in jsonStringArray)
            {
                string[] jsonStringData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                foreach (string ColumnsNameData in jsonStringData)
                {
                    try
                    {
                        int idx = ColumnsNameData.IndexOf(":");
                        string ColumnsNameString = ColumnsNameData.Substring(0, idx - 1).Replace("\"", "");
                        if (!ColumnsName.Contains(ColumnsNameString))
                        {
                            ColumnsName.Add(ColumnsNameString);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Error Parsing Column Name : {0}", ColumnsNameData));
                    }
                }
                break;
            }
            foreach (string AddColumnName in ColumnsName)
            {
                dt.Columns.Add(AddColumnName);
            }
            foreach (string jSA in jsonStringArray)
            {
                string[] RowData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                DataRow nr = dt.NewRow();
                foreach (string rowData in RowData)
                {
                    try
                    {
                        int idx = rowData.IndexOf(":");
                        string RowColumns = rowData.Substring(0, idx - 1).Replace("\"", "");
                        string RowDataString = rowData.Substring(idx + 1).Replace("\"", "");
                        nr[RowColumns] = RowDataString;
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                dt.Rows.Add(nr);
            }
            return dt;
        }

        protected DataTable ConvertJSONToDataTable(string jsonString)
        {
            DataTable dt = new DataTable();
            //strip out bad characters
            string[] jsonParts = Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{");

            //hold column names
            List<string> dtColumns = new List<string>();

            //get columns
            foreach (string jp in jsonParts)
            {
                //only loop thru once to get column names
                string[] propData = Regex.Split(jp.Replace("{", "").Replace("}", ""), "\",\"");
                foreach (string rowData in propData)
                {
                    try
                    {
                        int idx = rowData.IndexOf(":");
                        string n = rowData.Substring(0, idx - 1);
                        string v = rowData.Substring(idx + 1);
                        if (!dtColumns.Contains(n))
                        {
                            dtColumns.Add(n.Replace("\"", ""));
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Error Parsing Column Name : {0}", rowData));
                    }

                }
                break; // TODO: might not be correct. Was : Exit For
            }

            //build dt
            int i = 0;
            foreach (string c in dtColumns)
            {
                i = i + 1;
                if (i == dtColumns.Count+1)
                {
                    break;
                }
                dt.Columns.Add(c);
            }
            //get table data
            foreach (string jp in jsonParts)
            {
                string[] propData = Regex.Split(jp.Replace("{", "").Replace("}", ""), "\",\"");
                DataRow nr = dt.NewRow();
                foreach (string rowData in propData)
                {
                    try
                    {
                        int idx = rowData.IndexOf(":");
                        string n = rowData.Substring(0, idx - 1).Replace("\"", "");
                        string v = rowData.Substring(idx + 1).Replace("\"", "");
                        nr[n] = v;
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                }
                dt.Rows.Add(nr);
            }
            return dt;
        }

        public string DataTableToJSONWithStringBuilder(DataTable table)
        {
            var JSONString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }   
        public int insert_payment_collection(string jsonstring)
        {

            DataTable dt = ConvertJSONToDataTable(jsonstring);//JsonStringToDataTable(jsonstring);
            //Newtonsoft.Json.Converters.DataTableConverter obj = new Newtonsoft.Json.Converters.DataTableConverter();
            //DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonstring);

            MySqlConnection con = new MySqlConnection(smsconn);
            int i = 0;
            try
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                con.Open();
                foreach (DataRow row in dt.Rows)
                {

                    DateTime collectiondate = DateTime.ParseExact(row["collection_date"].ToString().Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    string queryString = "";//

                    queryString = @"SELECT count(*) FROM `ffhp_payment_collection` WHERE collection_date='" + collectiondate.ToString("yyyy-MM-dd") + "' and route_number='" + row["route_number"].ToString().Trim() + "'";
                    MySqlCommand cmd1 = new MySqlCommand(queryString, con);
                    var checkcount=cmd1.ExecuteScalar();
                    if (Convert.ToInt32(checkcount.ToString()) == 0)
                    {
                        queryString = @"INSERT INTO `ffhp_payment_collection`(`collection_date`, `route_number`, `amount_receivable`, `amount_received`, `payment_pending`, `recharge`, `dieselpetrol`, `auto`, `refund`, `salary`, `vehicle`, `others`, `final_collection`, `payment_pending_orders`, `returned_orders`, `comments`, `deliveredby`, `receivedby`, `lastupdateddate`) VALUES (
'"+collectiondate.ToString("yyyy-MM-dd")+"','" + row["route_number"].ToString().Trim() + "'," + row["amount_receivable"].ToString().Trim() + ",'" + row["amount_received"].ToString().Trim() + "','" + row["payment_pending"].ToString().Trim() + "','" + row["recharge"].ToString().Trim() + "','" + row["dieselpetrol"].ToString().Trim() + "','" + row["auto"].ToString().Trim() + "','" + row["refund"].ToString().Trim() + "','" + row["salary"].ToString().Trim() + "','" + row["vehicle"].ToString().Trim() + "','" + row["others"].ToString().Trim() + "','" + row["final_collection"].ToString().Trim() + "','" + row["payment_pending_orders"].ToString().Trim() + "','" + row["returned_orders"].ToString().Trim() + "','" + row["comments"].ToString().Trim() + "','" + row["deliveredby"].ToString().Trim() + "','" + row["receivedby"].ToString().Trim() + "',DATE(NOW()))";
                        
                        MySqlCommand cmd = new MySqlCommand(queryString, con);
                        i = cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        DateTime CDT = Convert.ToDateTime(row["collection_date"].ToString());
                        queryString = @"UPDATE `ffhp_payment_collection` SET `amount_receivable`=" + row["amount_receivable"].ToString().Trim() + ",`amount_received`='" + row["amount_received"].ToString().Trim() + "',`payment_pending`='" + row["payment_pending"].ToString().Trim() + "',`recharge`='" + row["recharge"].ToString().Trim() + "',`dieselpetrol`='" + row["dieselpetrol"].ToString().Trim() + "',`auto`='" + row["auto"].ToString().Trim() + "',`refund`='" + row["refund"].ToString().Trim() + "',`salary`='" + row["salary"].ToString().Trim() + "',`vehicle`='" + row["vehicle"].ToString().Trim() + "',`others`='" + row["others"].ToString().Trim() + "',`final_collection`='" + row["final_collection"].ToString().Trim() + "',`payment_pending_orders`='" + row["payment_pending_orders"].ToString().Trim() + "',`returned_orders`='" + row["returned_orders"].ToString().Trim() + "',`comments`='" + row["comments"].ToString().Trim() + "',`deliveredby`='" + row["deliveredby"].ToString().Trim() + "',`receivedby`='" + row["receivedby"].ToString().Trim() + "',`lastupdateddate`=DATE(NOW()) WHERE `collection_date`='" + collectiondate.ToString("yyyy-MM-dd") + "' and `route_number`='" + row["route_number"].ToString().Trim() + "'";

                        MySqlCommand cmd = new MySqlCommand(queryString, con);
                        i = cmd.ExecuteNonQuery();
                    }
                }
                con.Close();
                //sshobj.SshDisconnect(client);
            }
            catch (Exception ex)
            {

            }
            return i;
        }
        public DataSet get_payment_collection(DateTime fromdate,DateTime todate)
        {
            string queryString = "";
            DataSet ffhp_payment_collection = new DataSet();
            queryString = @"SELECT `pcid`, DATE_FORMAT(`collection_date`, '%Y/%m/%d' )as `collection_date`, `route_number`, `amount_receivable`, `amount_received`, `payment_pending`, `recharge`, `dieselpetrol`, `auto`, `refund`, `salary`, `vehicle`, `others`, `final_collection`, `payment_pending_orders`, `returned_orders`, `comments`, `deliveredby`, `receivedby`,`Start_km`,`End_km`,`Start_time`,`End_time`,`vehicle_type`,`petrol_diesel_litre`, `lastupdateddate` FROM `ffhp_payment_collection` 
where collection_date between '" + String.Format("{0:yyyy/MM/dd}", fromdate) + "' and '" + String.Format("{0:yyyy/MM/dd}", todate) + "'"; //and route_number='" + routenumber + "' ORDER BY route_number";
            //where date between '" + String.Format("{0:yyyy/MM/dd}", fromdate) + "' and '" + String.Format("{0:yyyy/MM/dd}", todate) + "' ORDER BY route_number, payment_mode DESC";
            if (queryString != "")
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                MySqlConnection con = new MySqlConnection(smsconn);
                con.Open();

                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, smsconn);

                adapteradminmail.Fill(ffhp_payment_collection, "ffhp_payment_collection");
                con.Close();
                //sshobj.SshDisconnect(client);
            }

            return ffhp_payment_collection;
        }
        public int insert_payment_collection_deliveryboy(string jsonstring)
        {

            DataTable dt = ConvertJSONToDataTable(jsonstring);//JsonStringToDataTable(jsonstring);
            //Newtonsoft.Json.Converters.DataTableConverter obj = new Newtonsoft.Json.Converters.DataTableConverter();
            //DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonstring);

            MySqlConnection con = new MySqlConnection(smsconn);
            int i = 0;
            try
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                con.Open();
                foreach (DataRow row in dt.Rows)
                {

                    DateTime collectiondate = DateTime.ParseExact(row["collection_date"].ToString().Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    string queryString = "";//

                    queryString = @"SELECT count(*) FROM `ffhp_payment_collection` WHERE collection_date='" + collectiondate.ToString("yyyy-MM-dd") + "' and deliveredby='" + row["deliveredby"].ToString().Trim() + "'";
                    MySqlCommand cmd1 = new MySqlCommand(queryString, con);
                    var checkcount = cmd1.ExecuteScalar();
                    if (Convert.ToInt32(checkcount.ToString()) == 0)
                    {
                        queryString = @"INSERT INTO `ffhp_payment_collection`(`collection_date`, `route_number`, `amount_receivable`, `amount_received`, `payment_pending`, `recharge`, `dieselpetrol`, `auto`, `refund`, `salary`, `vehicle`, `others`, `final_collection`, `payment_pending_orders`, `returned_orders`, `comments`, `deliveredby`,`Start_km`,`End_km`,`Start_time`,`End_time`,`vehicle_type`,`petrol_diesel_litre`, `receivedby`, `lastupdateddate`) VALUES (
'" + collectiondate.ToString("yyyy-MM-dd") + "','" + row["route_number"].ToString().Trim() + "'," + row["amount_receivable"].ToString().Trim() + ",'" + row["amount_received"].ToString().Trim() + "','" + row["payment_pending"].ToString().Trim() + "','" + row["recharge"].ToString().Trim() + "','" + row["dieselpetrol"].ToString().Trim() + "','" + row["auto"].ToString().Trim() + "','" + row["refund"].ToString().Trim() + "','" + row["salary"].ToString().Trim() + "','" + row["vehicle"].ToString().Trim() + "','" + row["others"].ToString().Trim() + "','" + row["final_collection"].ToString().Trim() + "','" + row["payment_pending_orders"].ToString().Trim() + "','" + row["returned_orders"].ToString().Trim() + "','" + row["comments"].ToString().Trim() + "','" + row["deliveredby"].ToString().Trim() + "','" + row["receivedby"].ToString().Trim() + "','" + row["Start_km"].ToString()+ "','" + row["End_km"].ToString()+ "','" + row["Start_time"].ToString()+ "','" + row["End_time"].ToString()+ "','" + row["vehicle_type"].ToString()+ "','" + row["petrol_diesel_litre"].ToString()+ "',DATE(NOW()))";

                        MySqlCommand cmd = new MySqlCommand(queryString, con);
                        i = cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        DateTime CDT = Convert.ToDateTime(row["collection_date"].ToString());
                        queryString = @"UPDATE `ffhp_payment_collection` SET `amount_receivable`=" + row["amount_receivable"].ToString().Trim() + ",`amount_received`='" + row["amount_received"].ToString().Trim() + "',`payment_pending`='" + row["payment_pending"].ToString().Trim() + "',`recharge`='" + row["recharge"].ToString().Trim() + "',`dieselpetrol`='" + row["dieselpetrol"].ToString().Trim() + "',`auto`='" + row["auto"].ToString().Trim() + "',`refund`='" + row["refund"].ToString().Trim() + "',`salary`='" + row["salary"].ToString().Trim() + "',`vehicle`='" + row["vehicle"].ToString().Trim() + "',`others`='" + row["others"].ToString().Trim() + "',`final_collection`='" + row["final_collection"].ToString().Trim() + "',`payment_pending_orders`='" + row["payment_pending_orders"].ToString().Trim() + "',`returned_orders`='" + row["returned_orders"].ToString().Trim() + "',`comments`='" + row["comments"].ToString().Trim() + "',`deliveredby`='" + row["deliveredby"].ToString().Trim() + "',`receivedby`='" + row["receivedby"].ToString().Trim() + "',`Start_km`='" + row["Start_km"].ToString()+ "',`End_km`='" + row["End_km"].ToString()+ "',`Start_time`='" + row["Start_time"].ToString()+ "',`End_time`='" + row["End_time"].ToString()+ "',`vehicle_type`='" + row["vehicle_type"].ToString()+ "',`petrol_diesel_litre`='" + row["petrol_diesel_litre"].ToString()+ "',`lastupdateddate`=DATE(NOW()) WHERE `collection_date`='" + collectiondate.ToString("yyyy-MM-dd") + "' and `deliveredby`='" + row["deliveredby"].ToString().Trim() + "'";

                        MySqlCommand cmd = new MySqlCommand(queryString, con);
                        i = cmd.ExecuteNonQuery();
                    }
                }
                con.Close();
                //sshobj.SshDisconnect(client);
            }
            catch (Exception ex)
            {

            }
            return i;
        }
        public DataSet get_payment_collection_deliveryboy(DateTime fromdate, DateTime todate)
        {
            string queryString = "";
            DataSet ffhp_payment_collection = new DataSet();
            queryString = @"SELECT `pcid`, DATE_FORMAT(`collection_date`, '%Y/%m/%d' )as `collection_date`, `route_number`, `amount_receivable`, `amount_received`, `payment_pending`, `recharge`, `dieselpetrol`, `auto`, `refund`, `salary`, `vehicle`, `others`, `final_collection`, `payment_pending_orders`, `returned_orders`, `comments`, `deliveredby`, `receivedby`,`Start_km`,`End_km`,`Start_time`,`End_time`,`vehicle_type`,`petrol_diesel_litre`, `lastupdateddate` FROM `ffhp_payment_collection` 
where collection_date between '" + String.Format("{0:yyyy/MM/dd}", fromdate) + "' and '" + String.Format("{0:yyyy/MM/dd}", todate) + "'"; //and route_number='" + routenumber + "' ORDER BY route_number";
            //where date between '" + String.Format("{0:yyyy/MM/dd}", fromdate) + "' and '" + String.Format("{0:yyyy/MM/dd}", todate) + "' ORDER BY route_number, payment_mode DESC";
            if (queryString != "")
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                MySqlConnection con = new MySqlConnection(smsconn);
                con.Open();

                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, smsconn);

                adapteradminmail.Fill(ffhp_payment_collection, "ffhp_payment_collection");
                con.Close();
                //sshobj.SshDisconnect(client);
            }

            return ffhp_payment_collection;
        }
        public int Upload_Stock_Sale_Data_old(DataTable dt)
        {
            int i = 0;
            if (dt.Rows.Count > 0)
            {
                using (SqlConnection con = new SqlConnection(sqlconn))
                {
                    using (SqlCommand cmd = new SqlCommand("Insert_Stockproducts"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@tblstockproducts", dt);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                StockSale obj = new StockSale();
                i = obj.databasetohistory("stockproducts", "stockproducts_history");
            }
            return i;
        }
        public int Upload_Stock_Sale_Data(DataTable dt)
        {
            int i = 0;
            if (dt.Rows.Count > 0)
            {
                Delete_stockproducts();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlconn))
                {
                    bulkCopy.DestinationTableName =
                        "dbo.stockproducts";

                    try
                    {
                        // Write from the source to the destination.
                        bulkCopy.WriteToServer(dt);
                        OnlineSale obj = new OnlineSale();
                        i = obj.databasetohistory_StockProducts("stockproducts", "stockproducts_history");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    
                }
            }
            return i;
        }
        public int Delete_stockproducts()
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(sqlconn))
            {
                using (SqlCommand cmd = new SqlCommand("sp_purchase_delete"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return i;
        }
        public int Upload_ffhporders_old(DataTable dt)
        {
            int i = 0;
            if (dt.Rows.Count > 0)
            {
                using (SqlConnection con = new SqlConnection(sqlconn))
                {
                    using (SqlCommand cmd = new SqlCommand("Insert_ffhporders"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@tblffhporders", dt);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                OnlineSale obj = new OnlineSale();
                i = obj.databasetohistory("ffhporders", "ffhporders_history");
            }
            return i;
        }
        public int Upload_ffhporders(DataTable dt)
        {
            int i = 0;
            //if (dt.Rows.Count > 0)
            //{
            //    using (SqlConnection con = new SqlConnection(sqlconn))
            //    {
            //        using (SqlCommand cmd = new SqlCommand("Insert_ffhporders"))
            //        {
            //            cmd.CommandType = CommandType.StoredProcedure;
            //            cmd.Connection = con;
            //            cmd.Parameters.AddWithValue("@tblffhporders", dt);
            //            con.Open();
            //            cmd.ExecuteNonQuery();
            //            con.Close();
            //        }
            //    }
            //    OnlineSale obj = new OnlineSale();
            //    i = obj.databasetohistory("ffhporders", "ffhporders_history");
            //}
            //return i;

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlconn))
            {
                bulkCopy.DestinationTableName =
                    "dbo.ffhporders";

                try
                {
                    // Write from the source to the destination.
                    bulkCopy.WriteToServer(dt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                OnlineSale obj = new OnlineSale();
                i = obj.databasetohistory("ffhporders", "ffhporders_history");
            }
            return i;
        }
        public DataSet Get_DeliveryBoy(string username, string password)
        {
            string queryString = "";
            DataSet ffhp_delivery_boy = new DataSet();
            queryString = @"SELECT ddid,name,phone,designation FROM `driver_deliveryboy_details` 
where name='" + username + "' and ddid='" + password + "' and designation='Delivery Boy'";

            if (queryString != "")
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                MySqlConnection con = new MySqlConnection(smsconn);
                con.Open();

                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, smsconn);

                adapteradminmail.Fill(ffhp_delivery_boy, "ffhp_delivery_boy");
                con.Close();
                //sshobj.SshDisconnect(client);
            }

            return ffhp_delivery_boy;
        }
        public DataSet Get_User_Access(string username, string password, string designation)
        {
            string queryString = "";
            DataSet ffhp_delivery_boy = new DataSet();
            queryString = @"SELECT ddid,name,phone,designation FROM `driver_deliveryboy_details` 
where name='" + username + "' and ddid='" + password + "' and designation='"+designation+"'";

            if (queryString != "")
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                MySqlConnection con = new MySqlConnection(smsconn);
                con.Open();

                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, smsconn);

                adapteradminmail.Fill(ffhp_delivery_boy, "ffhp_delivery_boy");
                con.Close();
                //sshobj.SshDisconnect(client);
            }

            return ffhp_delivery_boy;
        }
        public DataTable Getcustomerwithcoupon(string couponcodes)
        {
            DataTable customerorderlist = new DataTable();
            queryString = @"SELECT a.customer_id, a.customer_firstname, a.customer_lastname, a.customer_email,a.coupon_code, z.base_grand_total, z.increment_id, z.created_at
FROM `sales_flat_order_grid` AS z
INNER JOIN `sales_flat_order` AS a ON z.entity_id = a.entity_id
WHERE z.status!='cancelled' and a.coupon_code in ('" + couponcodes + "')";
            if (queryString != "")
            {
                MySqlConnection con = new MySqlConnection(smsconn);
                con.Open();

                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                adapteradminmail.Fill(customerorderlist);
                con.Close();
            }
            return customerorderlist;
        }
        //new
        public int insertjsoncomplaint(string jsoncomplaint)
        {

            //DataTable dt = ConvertJSONToDataTable(jsonstring);//JsonStringToDataTable(jsonstring);
            //Newtonsoft.Json.Converters.DataTableConverter obj = new Newtonsoft.Json.Converters.DataTableConverter();
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsoncomplaint);

            MySqlConnection con = new MySqlConnection(smsconn);
            int i = 0;
            try
            {
                con.Open();
                foreach (DataRow row in dt.Rows)
                {


                    queryString = @"INSERT INTO `ffhp_complaint`(`userid`, `ordernumber`, `productname`, `issuetype`, `discription`, `status`, `opendate`) VALUES (
'" + row["userid"].ToString().Trim() + "'," + row["ordernumber"].ToString().Trim() + ",'" + row["productname"].ToString().Trim() + "','" + row["issuetype"].ToString().Trim() + "','" + row["discription"].ToString().Trim() + "','" + row["status"].ToString().Trim() + "',DATE(NOW()))";

                    MySqlCommand cmd = new MySqlCommand(queryString, con);
                    i = cmd.ExecuteNonQuery();

                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return i;
        }

        public DataSet getcomplaint(string userid, string status)
        {
            string queryString = "";
            DataSet complaints = new DataSet();
            queryString = @"SELECT * FROM `ffhp_complaint` WHERE userid = '" + userid + "' and status = '" + status + "' and closedate is null";

            if (queryString != "")
            {
                MySqlConnection con = new MySqlConnection(smsconn);
                con.Open();

                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, smsconn);

                adapteradminmail.Fill(complaints, "complaints");
                con.Close();
            }

            return complaints;
        }

        public int updatecomplaint(int complaintid, string status)
        {
            MySqlConnection con = new MySqlConnection(smsconn);
            int i = 0;
            try
            {
                con.Open();

                queryString = @"UPDATE ffhp_complaint set status='" + status + "' where complaintid='" + complaintid + "'";

                MySqlCommand cmd = new MySqlCommand(queryString, con);
                i = cmd.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {

            }
            return i;
        }

        public int storeUser(string name, string email, string gcm_regid)
        {

            MySqlConnection con = new MySqlConnection(smsconn);
            int i = 0;
            try
            {
                con.Open();
                queryString = @"INSERT INTO gcm_users(name, email, gcm_regid, created_at) VALUES('" + name + "', '" + email + "', '" + gcm_regid + "',DATE(NOW()))";

                MySqlCommand cmd = new MySqlCommand(queryString, con);
                //cmd.CommandType = CommandType.Text;
                //SqlParameter IDParameter = new SqlParameter("@ID", SqlDbType.Int);
                //IDParameter.Direction = ParameterDirection.Output;
                //cmd.Parameters.Add(IDParameter);
                i = cmd.ExecuteNonQuery();
                //i = (int)IDParameter.Value;

                con.Close();
            }
            catch (Exception ex)
            {

            }
            return i;
        }
        public DataSet getAllUsers()
        {
            string queryString = "";
            DataSet gcmusers = new DataSet();
            queryString = @"select DISTINCT gcm_regid,email,name FROM gcm_users";

            if (queryString != "")
            {
                MySqlConnection con = new MySqlConnection(smsconn);
                con.Open();

                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, smsconn);

                adapteradminmail.Fill(gcmusers, "gcmusers");
                con.Close();
            }

            return gcmusers;
        }
        public DataTable get_Totalweight_from_PQ()
        {
            DataTable dt = new DataTable("PQ_Template");
            try
            {
                queryString = @"";

                SqlConnection con = new SqlConnection(sqlconn);
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_purchaseweight_from_PQ", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
            }
            catch (Exception ex)
            {
            }
            return dt;
        }
        public DataSet get_email(string ordernumber)
        {
            string queryString = "";
            DataSet ffhp_order_number_email = new DataSet();
            queryString = @"SELECT increment_id,customer_email FROM `sales_flat_order` WHERE increment_id in(" + ordernumber + ") ORDER BY increment_id";
            //where date ='" + String.Format("{0:yyyy/MM/dd}", fromdate) + "' ORDER BY route_number, payment_mode DESC";

            if (queryString != "")
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                MySqlConnection con = new MySqlConnection(conn);
                con.Open();

                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                adapteradminmail.Fill(ffhp_order_number_email, "ffhp_order_number_email");
                con.Close();
                //sshobj.SshDisconnect(client);
            }

            return ffhp_order_number_email;
        }
        public int update_ffhp_route_orders(string _deliveryboy_ordernumber, string _payment_pending_ordernumber, string _driver_name, string _driver_phone, string _deliveryboy_name, string _deliveryboy_phone, string _route_id)
        {
            MySqlConnection con = new MySqlConnection(smsconn);
            int i = 0;
            try
            {
                con.Open();

                queryString = @"update `ffhp_route_orders` set deliveryboy_ordernumber='" + _deliveryboy_ordernumber + "', payment_pending_ordernumber='" + _payment_pending_ordernumber + "', driver_name='" + _driver_name + "', driver_phone='" + _driver_phone + "', deliveryboy_name='" + _deliveryboy_name + "', deliveryboy_phone='" + _deliveryboy_phone + "' where route_id=" + _route_id + "";

                MySqlCommand cmd = new MySqlCommand(queryString, con);
                i = cmd.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {

            }
            return i;
        }
        public int clear_ffhp_route_orders(string routeid)
        {
            MySqlConnection con = new MySqlConnection(smsconn);
            int i = 0;
            try
            {
                con.Open();

                queryString = @"update `ffhp_route_orders` set deliveryboy_ordernumber='',payment_pending_ordernumber='', driver_name='', driver_phone='', deliveryboy_name='', deliveryboy_phone='',latitude='',longitude='' where route_id=" + routeid + "";

                MySqlCommand cmd = new MySqlCommand(queryString, con);
                i = cmd.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {

            }
            return i;
        }
        //Stock Status Entry
        public int update_stock_status(string jsonstring,string stocktype)
        {
            //DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonstring);
            DataTable dt = ConvertJSONToDataTable(jsonstring);
            dt = sumduplicate(dt);
            SqlConnection con = new SqlConnection(sqlconn);
            int i = 0;
            try
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                con.Open();
                foreach (DataRow row in dt.Rows)
                {
                    DateTime dtf = indianTime.Date;//DateTime.Now.AddDays(0);//DateTime.ParseExact(row["date"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    string queryString = "";
                    if (stocktype == "morning")
                    {
                        queryString = @"update tool_stockproducts set morningscannedweight='" + row["morningscannedweight"].ToString().Trim() + "',morningpiececount='" + row["morningpiececount"].ToString().Trim() + "',morningdescription='" + row["morningdescription"].ToString().Trim() + "',morningtrayweight='" + row["morningtrayweight"].ToString().Trim() + "' where productid='" + row["productid"].ToString().Trim() + "' and stockdate='" + String.Format("{0:yyyy/MM/dd}", dtf) + "'";
                    }
                    else if (stocktype == "local purchase")
                    {
                        queryString = @"update tool_stockproducts set localpurchasescannedweight='" + row["morningscannedweight"].ToString().Trim() + "',localpurchasepiececount='" + row["morningpiececount"].ToString().Trim() + "',localpurchasedescription='" + row["morningdescription"].ToString().Trim() + "',localpurchasetrayweight='" + row["morningtrayweight"].ToString().Trim() + "' where productid='" + row["productid"].ToString().Trim() + "' and stockdate='" + String.Format("{0:yyyy/MM/dd}", dtf) + "'";
                    }
                    else if (stocktype == "balance")
                    {
                        queryString = @"update tool_stockproducts set balancescannedweight='" + row["morningscannedweight"].ToString().Trim() + "',balancepiececount='" + row["morningpiececount"].ToString().Trim() + "',balancedescription='" + row["morningdescription"].ToString().Trim() + "',balancetrayweight='" + row["morningtrayweight"].ToString().Trim() + "' where productid='" + row["productid"].ToString().Trim() + "' and stockdate='" + String.Format("{0:yyyy/MM/dd}", dtf) + "'";
                    }
                    else if (stocktype == "local sale")
                    {
                        queryString = @"update tool_stockproducts set localsalescannedweight='" + row["morningscannedweight"].ToString().Trim() + "',localsalepiececount='" + row["morningpiececount"].ToString().Trim() + "',localsaledescription='" + row["morningdescription"].ToString().Trim() + "',localsaletrayweight='" + row["morningtrayweight"].ToString().Trim() + "' where productid='" + row["productid"].ToString().Trim() + "' and stockdate='" + String.Format("{0:yyyy/MM/dd}", dtf) + "'";
                    }
                    else if (stocktype == "after sale")
                    {
                        queryString = @"update tool_stockproducts set aftersalescannedweight='" + row["morningscannedweight"].ToString().Trim() + "',aftersalepiececount='" + row["morningpiececount"].ToString().Trim() + "',aftersaledescription='" + row["morningdescription"].ToString().Trim() + "',aftersaletrayweight='" + row["morningtrayweight"].ToString().Trim() + "' where productid='" + row["productid"].ToString().Trim() + "' and stockdate='" + String.Format("{0:yyyy/MM/dd}", dtf) + "'";
                    }

                    SqlCommand cmd = new SqlCommand(queryString, con);
                    i = cmd.ExecuteNonQuery();
                }
                con.Close();
                //sshobj.SshDisconnect(client);
            }
            catch (Exception ex)
            {

            }
            return i;
        }
        public DataTable sumduplicate(DataTable dt)
        {
            var newDt = dt.AsEnumerable()
              .GroupBy(r => int.Parse(r.Field<string>("productid")))
              .Select(g =>
              {
                  var row = dt.NewRow();

                  row["productid"] = g.Key;
                  row["morningscannedweight"] = g.Sum(r => decimal.Parse(r.Field<string>("morningscannedweight")));
                  row["morningpiececount"] = g.Sum(r => decimal.Parse(r.Field<string>("morningpiececount")));
                  row["morningtrayweight"] = g.Sum(r => decimal.Parse(r.Field<string>("morningtrayweight")));
                  row["morningdescription"] = "";

                  return row;
              }).CopyToDataTable();
            return newDt;
        }
        public string get_stock_status(string stocktype)
        {
            string jsonoutput = null;
            DataTable stocksale = new DataTable();
            try
            {
                SqlConnection sqlConnection = new SqlConnection(sqlconn);
                SqlCommand command = new SqlCommand("sp_tool_stockproducts_select", sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@stockdate", SqlDbType.DateTime).Value = indianTime.Date.ToString();
                command.Parameters.Add("@stocktype", SqlDbType.NVarChar).Value = stocktype;
                sqlConnection.Open();
                SqlDataAdapter sda = new SqlDataAdapter(command);
                sda.Fill(stocksale);
                sqlConnection.Close();


                if (stocksale.Rows.Count>0)
                {
                    var result = from o in stocksale.AsEnumerable()
                                 where o.Field<decimal>("morningscannedweight") > 0 || o.Field<decimal>("morningpiececount") > 0
                                 select o;
                                 //select new
                                 //{
                                 //    productid = o.Field<int>("productid"),
                                 //    Name = o.Field<string>("Name"),
                                 //    morningscannedweight = o.Field<decimal>("morningscannedweight"),
                                 //    morningpiececount = o.Field<decimal>("morningpiececount"),
                                 //    morningtrayweight = o.Field<decimal>("morningtrayweight"),
                                 //    morningdescription = o.Field<string>("morningdescription")
                                 //};

                    jsonoutput=DataTableToJSONWithStringBuilder(result.CopyToDataTable());
                }
                
                

            }
            catch (SqlException ex)
            {

            }
            
            return jsonoutput;
        }
        public string getproducts_StockSaleEntry()
        {
            string jsonoutput = null;
            DataTable dtproducts = new DataTable();

            StockSaleEntry obj = new StockSaleEntry();
            dtproducts = obj.Readdata_from_excel_to_datatable("tool_stockproducts");
            DataTable dttemp = new DataTable();
            dttemp.Columns.Add("Name", typeof(string));
            DataRow nrow = null;
            if (dtproducts.Rows.Count > 0)
            {
                var result = from o in dtproducts.AsEnumerable()
                             select new
                             {
                                 name = o.Field<String>("name") + " " + o.Field<double>("productid")
                             };
                if (result.Any())
                {
                    foreach (var rowObj in result)
                    {
                        //nrow = dttemp.NewRow();
                        //dttemp.Rows.Add(rowObj.name);
                        if (jsonoutput == "")
                        {
                            jsonoutput = rowObj.name.ToString();
                        }
                        else
                        {
                            jsonoutput = jsonoutput +"\",\""+ rowObj.name.ToString();
                        }
                    }
                }
                //jsonoutput = DataTableToJSONWithStringBuilder(dttemp);
                jsonoutput = "[\"" + jsonoutput + "\"]";
            }
            //dtproducts = Getallproducts();
            //DataTable dttemp = new DataTable();
            //dttemp.Columns.Add("Name", typeof(string));
            //DataRow nrow = null;
            //if (dtproducts.Rows.Count > 0)
            //{
            //    var result = from o in dtproducts.AsEnumerable()
            //                 select new
            //    {
            //        name = o.Field<String>("name") + " " + o.Field<UInt32>("product_id")
            //    };
            //    if (result.Any())
            //    {
            //        foreach (var rowObj in result)
            //        {
            //            nrow = dttemp.NewRow();
            //            dttemp.Rows.Add(rowObj.name);
            //        }
            //    }
            //    jsonoutput = DataTableToJSONWithStringBuilder(dttemp);
            //}
            return jsonoutput;
        }
    }
}
