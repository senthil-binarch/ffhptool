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
    public partial class SendSmsTemplate : System.Web.UI.Page
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
        }
        public void Bindsmstype()
        {
            try
            {
                DataTable smstemplate = new DataTable("smstemplate");
                queryString = "SELECT * FROM `1_ffhp_smsformat` where order_based=1";
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
        protected void rblselect_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            tbselect.Visible = true;
            if (rblselect.SelectedValue.ToString() == "0")
            {
                trordernumber.Visible = true;
                trorderlist.Visible = false;
                //trcustomerlist.Visible = false;
            }
            else if (rblselect.SelectedValue.ToString() == "1")
            {
                trordernumber.Visible = false;
                trorderlist.Visible = true;
                //trcustomerlist.Visible = false;
            }
            else if (rblselect.SelectedValue.ToString() == "2")
            {
                trordernumber.Visible = false;
                trorderlist.Visible = false;
                //trcustomerlist.Visible = true;
            }

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
            testdt.Columns.Add("Telephone");
            try
            {
                DataTable dt = new DataTable("OrderList");
                dt.Clear();
                dt.Columns.Add("entity_id");
                dt.Columns.Add("customername");
                dt.Columns.Add("Address");
                dt.Columns.Add("Name");
                dt.Columns.Add("Zipcode");
                dt.Columns.Add("Telephone");

                //queryString = "SELECT entity_id, customer_firstname, customer_lastname FROM `sales_flat_order` where created_at between '" + Convert.ToDateTime(TbxFromDate.Text.ToString()).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(TbxToDate.Text.ToString()).ToString("yyyy-MM-dd") + "'";
                //a.customer_id = c.customer_id AND 
                queryString = @"SELECT  DISTINCT z.increment_id as entity_id, a.customer_id, CONCAT( IFNULL(c.firstname,''), ' ', IFNULL(c.lastname,'') ) AS customername, CONCAT( IFNULL( c.street, '' ) , ', ', IFNULL( c.city, '' ) , ', ', IFNULL( c.region, '' ) , ', ', IFNULL( c.postcode, '' ) , ', India, T:', IFNULL( c.telephone, '' ) ) AS Address, b.Name,IFNULL( c.postcode, '' )as Zipcode,IFNULL( c.telephone, '' ) as telephone
FROM `sales_flat_order_grid` as z inner join `sales_flat_order` AS a 
ON z.entity_id=a.entity_id
INNER JOIN `sales_flat_order_item` AS b ON a.entity_id = b.order_id
INNER JOIN `sales_flat_order_address` AS c ON c.parent_id = b.order_id
WHERE b.parent_item_id IS NULL
AND product_type
IN (
'simple', 'bundle', 'grouped'
) AND c.address_type = 'shipping' and z.status!='canceled' and z.created_at between '" + Convert.ToDateTime(TbxFromDate.Text.ToString()).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(TbxToDate.Text.ToString()).AddDays(1).ToString("yyyy-MM-dd") + "'";

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
                    string Telephone = "";

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
                                _TestPackList["Telephone"] = Telephone.ToString();
                                testdt.Rows.Add(_TestPackList);

                                TestPack = "";
                                TestPack = dtt1.Rows[i]["Name"].ToString();
                            }
                            TestValue1 = Entityid.ToString();

                            customername = dtt1.Rows[i]["customername"].ToString();
                            Address = dtt1.Rows[i]["Address"].ToString();
                            Zipcode = dtt1.Rows[i]["Zipcode"].ToString();
                            Telephone = dtt1.Rows[i]["Telephone"].ToString();
                        }
                        DataRow _TestPackListfinalrecord = testdt.NewRow();
                        _TestPackListfinalrecord["entity_id"] = TestValue1.ToString();
                        _TestPackListfinalrecord["customername"] = customername.ToString();
                        _TestPackListfinalrecord["Address"] = Address.ToString();
                        _TestPackListfinalrecord["Name"] = TestPack.ToString();
                        _TestPackListfinalrecord["Zipcode"] = Zipcode.ToString();
                        _TestPackListfinalrecord["Telephone"] = Telephone.ToString();
                        testdt.Rows.Add(_TestPackListfinalrecord);
                    }

                    Gvorderlist.DataSource = testdt;
                    Gvorderlist.DataBind();
                    
                    
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        
        protected void Confirmedall_OnCheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow itm in Gvorderlist.Rows)
            {
                if (((CheckBox)itm.FindControl("CBConfirmed")).Enabled == true)
                {
                    if (((CheckBox)Gvorderlist.HeaderRow.FindControl("Confirmedall")).Checked)
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
        //protected void CustomerConfirmedall_OnCheckedChanged(object sender, EventArgs e)
        //{
        //    foreach (GridViewRow itm in Gvcustomerlist.Rows)
        //    {
        //        if (((CheckBox)itm.FindControl("CBConfirmed")).Enabled == true)
        //        {
        //            if (((CheckBox)Gvcustomerlist.HeaderRow.FindControl("CustomerConfirmedall")).Checked)
        //            {

        //                ((CheckBox)itm.FindControl("CBConfirmed")).Checked = true;
        //            }
        //            else
        //            {
        //                ((CheckBox)itm.FindControl("CBConfirmed")).Checked = false;
        //            }
        //        }
        //    }
        //}
//        protected void BtnGetCustomers_OnClick(object sender, EventArgs e)
//        {
//            try
//            {
//                queryString = @"SELECT `firstname`.`value` AS `First_Name` , `lastname`.`value` AS `Last_Name` , `telephone`.`value` AS `Telephone` , `customer_entity`.`created_at` , `customer_entity`.`updated_at`
//FROM `customer_address_entity_varchar` AS `country`
//INNER JOIN `customer_address_entity_varchar` AS `firstname`
//USING ( `entity_id` )
//INNER JOIN `customer_address_entity_varchar` AS `lastname`
//USING ( `entity_id` )
//INNER JOIN `customer_address_entity_varchar` AS `telephone`
//USING ( `entity_id` )
//INNER JOIN `customer_entity`
//USING ( `entity_id` )
//WHERE `country`.`attribute_id` =27 && `country`.`value` = 'IN' && `firstname`.`attribute_id` =20 && `lastname`.`attribute_id` =22 && `telephone`.`attribute_id` =31
//GROUP BY `telephone`.`value`";

                
//                if (queryString != "")
//                {
//                    MySqlConnection con = new MySqlConnection(conn);
//                    con.Open();

//                    MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

//                    DataSet customerlist = new DataSet();
//                    adapteradminmail.Fill(customerlist, "customertelephone");
//                    Gvcustomerlist.DataSource = customerlist;
//                    Gvcustomerlist.DataBind();
//                }
//            }
//            catch (Exception ex)
//            {
//                lblerror.Text = ex.ToString();
//            }

//        }
        protected void btnsendsmsConfirmed_OnClick(object sender, EventArgs e)
        {
            try
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();

                if (Lblsmscontent.Text.ToString() != "" && !Lblsmscontent.Text.ToString().Contains("~"))
                {
                    if (rblselect.SelectedValue.ToString() == "0")
                    {
                        if (txtordernumber.Text.ToString() != "")
                        {
                            Sendsms_mail(txtordernumber.Text.ToString(), Lblsmscontent.Text.ToString());
                        }
                        else
                        {
                            lblerror.Text = "Please give at least one order";
                        }
                    }
                    else if (rblselect.SelectedValue.ToString() == "1")
                    {
                        string ConfirmedOrder = "";
                        foreach (GridViewRow itm in Gvorderlist.Rows)
                        {
                            if (((CheckBox)itm.FindControl("CBConfirmed")).Checked && ((CheckBox)itm.FindControl("CBConfirmed")).Enabled)
                            {
                                if (ConfirmedOrder != "")
                                {
                                    ConfirmedOrder = ConfirmedOrder + "," + ((HiddenField)itm.FindControl("HFConfirmedordernum")).Value.ToString();
                                }
                                else
                                {
                                    ConfirmedOrder = ((HiddenField)itm.FindControl("HFConfirmedordernum")).Value.ToString();
                                }
                            }
                        }
                        if (ConfirmedOrder != "")
                        {
                            Sendsms_mail(ConfirmedOrder, Lblsmscontent.Text.ToString());
                        }
                        else
                        {
                            lblerror.Text = "Please select at least one order";
                        }
                    }
                    else
                    {
                        lblerror.Text = "Please select options are Order Numbers OR Order List";
                    }
                    //else if (rblselect.SelectedValue.ToString() == "2")
                    //{
                    //    Sendsms_customer(Lblsmscontent.Text.ToString());
                    //}
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
        public void Sendsms_mail(string OrderNumber, string sms)
        {
            try
            {
                if (OrderNumber != "")
                {
                    string path = Server.MapPath("Images/ordersms.txt");
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    queryString = @"SELECT DISTINCT z.increment_id, CONCAT( IFNULL( c.firstname, '' ) , ' ', IFNULL( c.lastname, '' ) ) AS customername, c.telephone, c.email, z.grand_total, z.created_at
FROM `sales_flat_order_grid` AS z
INNER JOIN `sales_flat_order_address` AS c ON c.parent_id = z.entity_id
WHERE c.address_type = 'shipping'
AND z.increment_id
IN ( " + OrderNumber + " )";
                    if (queryString != "")
                    {
                        MySqlConnection con = new MySqlConnection(conn);
                        con.Open();

                        MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);


                        string _smscontentorg = "";
                        DataSet smslist = new DataSet();
                        adapteradminmail.Fill(smslist, "smslist");

                        DataTable smslist1 = new DataTable();
                        DataTable _smsdt = new DataTable();
                        smslist1 = smslist.Tables[0];
                        if (smslist1.Rows.Count > 0)
                        {
                            _smscontentorg = "";
                            if (sms != "")
                            {
                                _smscontentorg = sms;
                            }
                            
                        }
                        if (_smscontentorg != "")
                        {
                            foreach (DataRow itm in smslist1.Rows)
                            {
                                itm["increment_id"].ToString();
                                itm["customername"].ToString();
                                string telephone = itm["telephone"].ToString().Trim();
                                itm["email"].ToString();
                                itm["grand_total"].ToString();
                                itm["created_at"].ToString();

                                string _smscontent = _smscontentorg;
                                _smscontent = _smscontent.Replace("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX", itm["customername"].ToString());
                                _smscontent = _smscontent.Replace("999999999", itm["increment_id"].ToString());
                                _smscontent = _smscontent.Replace("99999.99", String.Format("{0:0.00}", Convert.ToDecimal(itm["grand_total"].ToString())));

                                int i = telephone.Length;
                                if (telephone.Length >= 10)
                                {
                                    telephone = telephone.Substring(telephone.Length - 10, 10);
                                    telephone = "91" + telephone;
                                    //Lblsmscontent.Text = _smscontent;
                                    string output = sendsms(_smscontent, telephone);
                                    //if (output.Contains(":"))
                                    //{
                                    //    output = output.Replace(":", "=");
                                    //}
                                    //Sentsmsupdate(itm["increment_id"].ToString(), itm["customername"].ToString(), itm["telephone"].ToString(), smstype, output, Convert.ToDateTime(itm["created_at"].ToString()), _smscontent.Length);
                                    OrderSMSupdate(itm["customername"].ToString(), telephone, output);
                                }

                                if (itm["email"].ToString() != "")
                                {
                                    //here below code : mail send.
                                    //string htmlstring = string.Format(@"<html><head><body><table><tr><td><img src='http://192.168.1.14/ffhp/Images/ffhp_main_logo.png'/></td></tr><tr><td>#sms#</td></tr></table></body></head></html>");

                                    //string htmlstring = string.Format(@"<html><head><body><table><tr><td><img src='http://binarch.com/wp-content/uploads/2013/06/final_logo.png'/></td></tr><tr><td>#sms#</td></tr></table></body></head></html>");
                                    //htmlstring = htmlstring.Replace("#sms#", _smscontent);
                                    //sendmaillikesms(itm["email"].ToString(), _smsdt.Rows[0]["sms_type"].ToString(), htmlstring);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        public void OrderSMSupdate(string name, string phone, string message)
        {
            try
            {

                string path = Server.MapPath("Images/ordersms.txt");

                string pss1 = string.Format("{0:hh:mm}", DateTime.Now);//System.DateTime.Now.ToShortTimeString();
                FileStream pfs1 = new FileStream(path,
                                        FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter p_streamWriter1 = new StreamWriter(pfs1);
                p_streamWriter1.BaseStream.Seek(0, SeekOrigin.End);
                //p_streamWriter1.WriteLine(pss1 + "FFHP Orders " + " on " + DateTime.Today.Date.ToString("dd:MM:yyyy") + "_" + DateTime.Now.TimeOfDay.ToString()+DateTime.Now.ToString("tt")+" #" + txtidlist.Text.ToString()); p_streamWriter1.Flush();
                p_streamWriter1.WriteLine(DateTime.Today.Date.ToString("dd:MM:yyyy") + "_" + DateTime.Now.TimeOfDay.ToString() + DateTime.Now.ToString("tt") + " #" + name.ToString() + " " + phone.ToString() + " " + message.ToString()); p_streamWriter1.Flush();
                //p_streamWriter1.WriteLine(DateTime.Today.Date.ToString("dd:MM:yyyy") + "_" + DateTime.Now.TimeOfDay.ToString() + DateTime.Now.ToString("tt") + " #" + message.ToString()); p_streamWriter1.Flush();
                p_streamWriter1.Close();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }

        }
        //public void Sendsms_customer(string sms)
        //{
        //    try
        //    {
        //        foreach (GridViewRow itm in Gvcustomerlist.Rows)
        //        {
        //            if (((CheckBox)itm.FindControl("CBConfirmed")).Checked && ((CheckBox)itm.FindControl("CBConfirmed")).Enabled)
        //            {
        //                string _smscontent = sms;
        //                _smscontent = _smscontent.Replace("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX", itm.Cells[1].Text.ToString());

        //                string telephone = ((HiddenField)itm.FindControl("HFTelephone")).Value.ToString();
        //                if (telephone.Length >= 10)
        //                {
        //                    telephone = telephone.Substring(telephone.Length - 10, 10);
        //                    telephone = "91" + telephone;
        //                    //Lblsmscontent.Text = _smscontent;
        //                    //string output = sendsms(_smscontent, telephone);
        //                    //if (output.Contains(":"))
        //                    //{
        //                    //    output = output.Replace(":", "=");
        //                    //}
        //                    //Sentsmsupdate(itm["increment_id"].ToString(), itm["customername"].ToString(), itm["telephone"].ToString(), smstype, output, Convert.ToDateTime(itm["created_at"].ToString()), _smscontent.Length);
        //                }
        //            }

                    
        //        }
                
        //    }
        //    catch (Exception ex)
        //    {
        //        lblerror.Text = ex.ToString();
        //    }
        //}
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
            string path = Server.MapPath("Images/ordersms.txt");
            string strFileName;
        strFileName = "ordersms.txt";
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
