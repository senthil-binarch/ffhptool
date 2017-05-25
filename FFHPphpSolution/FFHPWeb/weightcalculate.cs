using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
//using Renci.SshNet;
namespace FFHPWeb
{
    public class weightcalculate
    {
        string conn = System.Configuration.ConfigurationManager.AppSettings["Connection"].ToString();
        string queryString = "";
        MySqlDataAdapter DA;
        string s = "";
        bool t = false;
        string errormsg = "Try again";
        string ordernumbers="";
        public DataTable calculate()
        {
            DataTable testdt = new DataTable("testPackListfinal");
            try
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();

                TotalWeightlprdata objcalwt = new TotalWeightlprdata();
                ordernumbers = objcalwt.API_ReadLastorders();
                    
                    testdt.Clear();
                    testdt.Columns.Add("Name");
                    testdt.Columns.Add("TotalWeight");
                    testdt.Columns.Add("Units");
                    testdt.Columns.Add("PackingStyle");
                    testdt.Columns.Add("Product_Id");

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
                    DataTable LPRData = new DataTable();
                    LPRData = getffhpproducts();//getLPR_Data();

                    string TestName = "";
                    string TestValue1 = "";
                    decimal Testweight = 0;
                    string smsformat = "";
                    string smsformatfinal = "";
                    string Testunits = "";
                    string Testunits1 = "";
                    string TestPackingStyle = "";
                    string TestPackingStyle1 = "";
                    string Testproduct_id = "";
                    string Testproduct_id1 = "";
                    DataTable dtt1 = resort(packlist, "Name", "ASC");
                    if (dtt1.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtt1.Rows.Count; i++)
                        {

                            TestName = dtt1.Rows[i]["Name"].ToString();
                            Testproduct_id = dtt1.Rows[i]["Product_Id"].ToString();
                            int ono = 0;
                            TestPackingStyle = "";
                            if (LPRData != null)
                            {
                                if (dtt1.Rows[i]["Product_Id"].ToString() != "")
                                {
                                    ono = Convert.ToInt32(dtt1.Rows[i]["Product_Id"].ToString());
                                    DataTable LPRData1 = new DataTable();
                                    LPRData1 = LPRData.AsEnumerable().Where(r => Convert.ToInt32(r["productid"]) == ono).AsDataView().ToTable();
                                    if (LPRData1.Rows.Count > 0)
                                    {
                                        TestPackingStyle = LPRData1.Rows[0]["weightgroup"].ToString();
                                    }

                                }
                            }

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
                                _TestPackList["PackingStyle"] = TestPackingStyle1.ToString();
                                _TestPackList["Product_Id"] = Testproduct_id1.ToString();
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
                            TestPackingStyle1 = TestPackingStyle.ToString();
                            Testproduct_id1 = Testproduct_id.ToString();
                        }
                        DataRow _TestPackListfinalrecord = testdt.NewRow();
                        _TestPackListfinalrecord["Name"] = TestValue1.ToString();
                        _TestPackListfinalrecord["TotalWeight"] = Testweight.ToString();
                        _TestPackListfinalrecord["Units"] = Testunits1.ToString();
                        _TestPackListfinalrecord["PackingStyle"] = TestPackingStyle1.ToString();
                        _TestPackListfinalrecord["Product_Id"] = Testproduct_id1.ToString();
                        testdt.Rows.Add(_TestPackListfinalrecord);

                    }
                    //sshobj.SshDisconnect(client); 
            }
            catch (Exception ex)
            {
                //lblerror.Text = errormsg + ex.ToString();
            }
            return testdt;
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
AND z.increment_id in (" + ordernumbers + ")";// union SELECT a.order_id,a.product_id,a.Name,a.weight,a.qty_ordered FROM `sales_flat_order_grid` as z inner join `sales_flat_order_item` AS a ON z.entity_id=a.order_id WHERE a.product_type not in('grouped') and a.sku like 'a-la-carte-%' AND product_options NOT LIKE '%Additional Qty%' AND z.increment_id in (" + txtidlist.Text + ")
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
WHERE z.increment_id in (" + ordernumbers + ")";
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
                //lblerror.Text = errormsg;//ex.ToString();
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

and z.increment_id in (" + ordernumbers + ")";
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
        private DataTable getsinglewithmultipleproduct()
        {
            DataTable testdt = new DataTable("testPackList1");
            testdt.Clear();
            testdt.Columns.Add("Product_Id");
            testdt.Columns.Add("Name");
            testdt.Columns.Add("TotalWeight");
            testdt.Columns.Add("Units");

            try
            {
                DataTable dt = new DataTable("PackList");
                dt.Clear();
                dt.Columns.Add("Product_Id");
                dt.Columns.Add("Name");
                dt.Columns.Add("Weight");
                dt.Columns.Add("Pack");
                dt.Columns.Add("TotalWeight");
                dt.Columns.Add("Units");




                string Packcount = "";
                string kgcount = "";

                //queryString = "SELECT entity_id, customer_firstname, customer_lastname FROM `sales_flat_order` where created_at between '" + Convert.ToDateTime(TbxFromDate.Text.ToString()).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(TbxToDate.Text.ToString()).ToString("yyyy-MM-dd") + "'";
                queryString = @"SELECT a.order_id,a.product_id,a.Name,a.weight,a.qty_ordered,'' as units FROM `sales_flat_order_grid` as z inner join `sales_flat_order_item` AS a ON z.entity_id=a.order_id WHERE a.product_type not in('grouped') and a.sku like 'A-la-c%' AND product_options NOT LIKE '%Additional Qty%' AND z.increment_id in (" + ordernumbers + ")";
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
                                        select new { col1 = dRow["product_id"] }).Distinct();


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
                        _PackList["Product_Id"] = ono.ToString();
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
                    string Testproductid = "";
                    string Testproductid1 = "";

                    DataTable dtt1 = resort(dt, "Name", "ASC");
                    //if (dtt1.Rows.Count > 0)
                    //{
                    for (int i = 0; i < dtt1.Rows.Count; i++)
                    {

                        TestName = dtt1.Rows[i]["Name"].ToString();
                        Testunits = dtt1.Rows[i]["units"].ToString();
                        Testproductid = dtt1.Rows[i]["Product_Id"].ToString();
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
                            _TestPackList["Product_Id"] = Testproductid1.ToString();
                            _TestPackList["Name"] = TestValue1.ToString();
                            _TestPackList["TotalWeight"] = Testweight.ToString();
                            _TestPackList["Units"] = Testunits1.ToString();
                            testdt.Rows.Add(_TestPackList);

                            Testweight = 0;
                            Testweight = Convert.ToDecimal(dtt1.Rows[i]["TotalWeight"].ToString());
                        }
                        TestValue1 = TestName.ToString();
                        Testunits1 = Testunits.ToString();
                        Testproductid1 = Testproductid.ToString();
                    }
                    DataRow _TestPackListfinalrecord = testdt.NewRow();
                    _TestPackListfinalrecord["Product_Id"] = Testproductid1.ToString();
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
                //lblerror.Text = errormsg;//ex.ToString();
            }
            return testdt;
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
IN (" + ordernumbers + ")";
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
        public DataTable getffhpproducts()
        {
            DataTable ffhpproducts = new DataTable("LBRData");
            queryString = "SELECT * FROM  1_ffhp_products";
            if (queryString != "")
            {
                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);
                adapteradminmail.Fill(ffhpproducts);
            }
            return ffhpproducts;
        }
        public static DataTable resort(DataTable dt, string colName, string direction)
        {
            dt.DefaultView.Sort = colName + " " + direction;
            dt = dt.DefaultView.ToTable();
            return dt;
        }
    }
}
