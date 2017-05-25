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
using System.Globalization;
//using Renci.SshNet;

namespace FFHPWeb
{
    public partial class OrderNumber : System.Web.UI.Page
    {
        string conn = System.Configuration.ConfigurationManager.AppSettings["Connection"].ToString();
        string queryString = "";
        MySqlDataAdapter DA;
        string s = "";
        bool t = false;
        int DewCount = 0;
        int ShineCount = 0;
        int GrandCount = 0;
        int JainCount = 0;
        int YOURPACKv1Count = 0;
        string errormsg = "Try again";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReadLastorders();
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
                lblerror.Text = errormsg.ToString();
            }
        }
        protected void CBorder_OnCheckedChanged(object sender, EventArgs e)
        {
            string orderlist = "";
            foreach (GridViewRow itm in GVOrderDetails.Rows)
            {
                if (((CheckBox)itm.FindControl("CBorder")).Checked)
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
            txtidlist.Text = orderlist;
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
            txtidlist.Text = orderlist;
        }
        protected void btnsubmit1_OnClick(object sender, EventArgs e)
        {
            try
            {
                string path = Server.MapPath("Images/OrderNumbers.txt");
                string pss1 = string.Format("{0:hh:mm}", DateTime.Now);//System.DateTime.Now.ToShortTimeString();
                FileStream pfs1 = new FileStream(path,
                                        FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter p_streamWriter1 = new StreamWriter(pfs1);
                p_streamWriter1.BaseStream.Seek(0, SeekOrigin.End);
                //p_streamWriter1.WriteLine(pss1 + "FFHP Orders " + " on " + DateTime.Today.Date.ToString("dd:MM:yyyy") + "_" + DateTime.Now.TimeOfDay.ToString()+DateTime.Now.ToString("tt")+" #" + txtidlist.Text.ToString()); p_streamWriter1.Flush();
                p_streamWriter1.WriteLine(DateTime.Today.Date.ToString("dd:MM:yyyy") + "_" + DateTime.Now.TimeOfDay.ToString() + DateTime.Now.ToString("tt") + " #" + txtidlist.Text.ToString()); p_streamWriter1.Flush();
                p_streamWriter1.Close();
                lblerror.Text = "Submitted Successfully";
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }

        }
    }
}
