using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text.RegularExpressions;
//using Renci.SshNet;
namespace FFHPWeb
{
    public partial class OtherPack : System.Web.UI.Page
    {
        //string conn = "server=68.178.143.107;userid=stagingffhpin;password=Stag1ngffHP!n;database=stagingffhpin;Convert Zero Datetime=True";  //staging
        //string conn = "server=68.178.143.11;userid=ffhpmagento;password=D6QpT!KDd0dKHI;database=ffhpmagento;Convert Zero Datetime=True";  //Live
        string conn = System.Configuration.ConfigurationManager.AppSettings["Connection"].ToString();
        string queryString = "";
        MySqlDataAdapter DA;
        string s = "";
        bool t = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            //getOtherPackDetails();
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
                    //ConnectSsh sshobj = new ConnectSsh();
                    //SshClient client = sshobj.SshConnect();
                    MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                    DataSet orderlist = new DataSet();
                    adapteradminmail.Fill(orderlist, "1_pack_item_ffhp_product");
                    GvPackList.DataSource = orderlist;
                    GvPackList.DataBind();
                    //sshobj.SshDisconnect(client);

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
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();

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
                queryString = @"SELECT a.pack_name, a.name, a.weight
FROM `1_pack_item_ffhp_product` AS a
LEFT OUTER JOIN sales_flat_order_item AS b ON a.pack_name = b.name
WHERE b.order_id in (" + txtidlist.Text + ")";
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

                        string queryString1 = @"SELECT a.pack_name, a.name, a.weight
FROM `1_pack_item_ffhp_product` AS a
LEFT OUTER JOIN sales_flat_order_item AS b ON a.pack_name = b.name
WHERE b.order_id in (" + txtidlist.Text + ") and a.name='" + s+"'";
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
                //sshobj.SshDisconnect(client);
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
    }
}
