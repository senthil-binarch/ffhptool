using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

namespace FFHPWeb
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        //string conn = "server=68.178.143.107;userid=stagingffhpin;password=Stag1ngffHP!n;database=stagingffhpin;Convert Zero Datetime=True";  //staging
        //string conn = "server=68.178.143.11;userid=ffhpmagento;password=D6QpT!KDd0dKHI;database=ffhpmagento;Convert Zero Datetime=True";  //Live
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
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        public void getOrderDetails()
        {
            try
            {
                //queryString = "SELECT entity_id, customer_firstname, customer_lastname FROM `sales_flat_order` where created_at between '" + Convert.ToDateTime(TbxFromDate.Text.ToString()).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(TbxToDate.Text.ToString()).ToString("yyyy-MM-dd") + "'";
                queryString = @"SELECT a.entity_id, a.customer_firstname, a.customer_lastname, b.Name
FROM `sales_flat_order` AS a
LEFT OUTER JOIN `sales_flat_order_item` AS b ON a.entity_id = b.order_id
WHERE b.parent_item_id IS NULL and b.Name like 'YOYO%'  and a.created_at between '" + Convert.ToDateTime(TbxFromDate.Text.ToString()).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(TbxToDate.Text.ToString()).ToString("yyyy-MM-dd") + "'";
                //Response.Write(queryString);
                if (queryString != "")
                {
                    MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                    DataSet orderlist = new DataSet();
                    adapteradminmail.Fill(orderlist, "sales_flat_order_item");
                    GVOrderDetails.DataSource = orderlist;
                    GVOrderDetails.DataBind();

                }
            }
            catch (Exception ex)
            {
            }
        }
        protected void btncalculate_OnClick(object sender, EventArgs e)
        {
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
                queryString = @"SELECT order_id,product_id,Name,weight
FROM `sales_flat_order_item`
WHERE sku IS NULL  and Name not like 'YOYO%' and Name not like '%Want To Buy This Product/%' 
AND order_id in (" + txtidlist.Text + ")";
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

                    }
                    //lblpackcount.Text = Packcount;
                    //lblkgcount.Text = kgcount;

                    GvPackList.DataSource = resort(dt, "Name", "ASC");
                    GvPackList.DataBind();

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
    }
}
