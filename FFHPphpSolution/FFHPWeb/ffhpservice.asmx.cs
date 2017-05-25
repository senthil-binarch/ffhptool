using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data;
using System.Xml;
using System.IO;
using System.Net.Mail;
using System.Globalization;
//using Renci.SshNet;
using System.Web.Script.Serialization;
namespace FFHPWeb
{
    /// <summary>
    /// Summary description for ffhpservice
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ffhpservice : System.Web.Services.WebService
    {
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

        string _output = "";
        //[WebMethod]
        //public string HelloWorld()
        //{
        //    return "Hello World";
        //}
        [WebMethod]
        public DataTable Getallproducts()
        {
            DataTable _output = new DataTable();
            try
            {
                APIMethods obj = new APIMethods();
                _output = obj.Getallproducts();
            }
            catch (Exception ex)
            {
                //_output = "Try Again Later";
            }
            return _output;
        }
        [WebMethod]
        public DataTable Getalacartewithnames(string ordernumber)
        {
            DataTable _output = new DataTable();
            try
            {
                APIMethods obj = new APIMethods();
                _output = obj.Getalacartewithnames(ordernumber);
            }
            catch (Exception ex)
            {
                //_output = "Try Again Later";
            }
            return _output;
        }
        [WebMethod]
        public string Getalacartewithnamesxml(string ordernumber)
        {
            System.IO.StringWriter objwr = new System.IO.StringWriter();
            DataTable _output = new DataTable();
            try
            {
                APIMethods obj = new APIMethods();
                _output = obj.Getalacartewithnames(ordernumber);
                if (_output.Rows.Count > 0)
                {
                    _output.WriteXml(objwr);
                }
            }
            catch (Exception ex)
            {
                //_output = "Try Again Later";
            }
            return objwr.ToString();
        }
        [WebMethod]
        public string GetCalculateWeight()
        {
            try
            {
                
                TotalWeight objsms = new TotalWeight();
                DataTable sms = new DataTable();
                sms = objsms.API_calculate();
                System.IO.StringWriter obj = new System.IO.StringWriter();
                if (sms.Rows.Count > 0)
                {
                    //DT = ds.Tables[0];
                    sms.WriteXml(obj);
                    _output = obj.ToString();
                }
                else
                {
                    _output = "No Records Found";
                }
                
            }
            catch (Exception ex)
            {
                _output = "Try Again Later";
            }
            return _output;
        }
        [WebMethod]
        public DataTable GetCalculateWeightNew()
        {
            DataTable _output = new DataTable();
            try
            {
                
                weightcalculate objwtcal = new weightcalculate();

                _output = objwtcal.calculate();
                
            }
            catch (Exception ex)
            {
                
            }
            return _output;
        }
        [WebMethod]
        public string GetOrderNumbers()
        {
            string ordernumbers = "";
            try
            {
                string path = Server.MapPath("Images/OrderNumbers.txt");
                string ord = File.ReadAllLines(path).Last();
                string[] Orders = ord.Split('#');
                if (Orders.Count() > 0)
                {
                    ordernumbers = Orders.Last();
                }
                if (ordernumbers.ToString() == "0")
                {
                    ordernumbers = null;
                }
            }
            catch (Exception ex)
            {
                ordernumbers = "Try Again Later";
            }
            return ordernumbers;
        }
        [WebMethod]
        public string GetCODorderNumbers()
        {
            string _codordernumbers = "";
            try
            {
                string path = Server.MapPath("Images/CODOrderNumbers.txt");
                if (File.Exists(path))
                {
                    string ord = File.ReadAllLines(path).Last();
                    string[] Orders = ord.Split('#');
                    if (Orders.Count() > 0)
                    {
                        _codordernumbers = Orders.Last();
                    }
                    if (_codordernumbers.ToString() == "0")
                    {
                        _codordernumbers = null;
                    }
                }
                else
                {
                    File.Create(path);
                }
            }
            catch (Exception ex)
            {
                _codordernumbers = "Try Again Later";
            }
            return _codordernumbers;
        }
        [WebMethod]
        public string Getsentsmslog(DateTime FromDate,DateTime ToDate)
        {
            try
            {
                

                DataTable DTsentsms = new DataTable();
                sms objsms = new sms();
                DTsentsms = objsms.API_Readsentsms(FromDate, ToDate);
                System.IO.StringWriter obj = new System.IO.StringWriter();
                if (DTsentsms.Rows.Count > 0)
                {
                    //DT = ds.Tables[0];
                    DTsentsms.WriteXml(obj);
                    _output = obj.ToString();
                }
                else
                {
                    _output = "No Records Found";
                }
                
            }
            catch (Exception ex)
            {
                _output = "Try again Later";
            }
            return _output;
        }
        [WebMethod]
        public string GetOrderDetailsbyorderid(string ordernumbers,string codordernumbers)
        {
            try
            {
                

                DataTable DTCustomerDetails = new DataTable();
                sms objsms = new sms();
                DTCustomerDetails = objsms.API_getOrderDetailsbyorderid(ordernumbers, codordernumbers);
                System.IO.StringWriter obj = new System.IO.StringWriter();
                if (DTCustomerDetails.Rows.Count > 0)
                {
                    //DT = ds.Tables[0];
                    DTCustomerDetails.WriteXml(obj);
                    _output = obj.ToString();
                }
                else
                {
                    _output = "No Records Found";
                }
                
            }
            catch (Exception ex)
            {
                _output = "Try Again Later";
            }
            return _output;
        }
        [WebMethod]
        public string GetOrderDetails(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                
                DataTable DTOrderDetails = new DataTable();
                sms objsms = new sms();
                DTOrderDetails = objsms.API_getOrderDetails(FromDate, ToDate);
                System.IO.StringWriter obj = new System.IO.StringWriter();
                if (DTOrderDetails.Rows.Count > 0)
                {
                    //DT = ds.Tables[0];
                    DTOrderDetails.WriteXml(obj);
                    _output = obj.ToString();
                }
                else
                {
                    _output = "No Records Found";
                }
                
            }
            catch (Exception ex)
            {
                _output = "Try Again Later";
            }
            return _output;
        }
        [WebMethod]
        public string Sendsms_mail(string OrderNumber, string smstype)
        {
            try
            {
                sms objsms = new sms();
                _output = objsms.API_Sendsms_mail(OrderNumber, smstype);
                
            }
            catch (Exception ex)
            {
                _output = "Try Again Later";
            }
            return _output;
        }
        [WebMethod]
        public string ServiceOrderupdate(string ConfirmOrderNumbers, string CODOrderNumbers)
        {
            try
            {
                sms objsms = new sms();
                _output = objsms.API_ServiceOrderupdate(ConfirmOrderNumbers, CODOrderNumbers);
            }
            catch (Exception ex)
            {
                _output = "Try Again Later";
            }
            return _output;
        }
        [WebMethod]
        public bool MailSend(string OrderNumber,string Content)
        {
            bool t = false;
            try
            {
                
                string mailto = System.Configuration.ConfigurationManager.AppSettings["Mail_To"].ToString();
                string mailcc = System.Configuration.ConfigurationManager.AppSettings["Mail_Cc"].ToString();
                string mailcredential = System.Configuration.ConfigurationManager.AppSettings["Mail_Credential"].ToString();
                string mailpassword = System.Configuration.ConfigurationManager.AppSettings["Mail_Password"].ToString();

                //MailMessage mail = new MailMessage();
                //SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                //mail.From = new MailAddress(mailcredential);
                //mail.To.Add(mailto);
                //mail.CC.Add(mailcc);
                //mail.Subject = "Order Number " + OrderNumber + " is Completed";
                
                //mail.Body = Content;

                //SmtpServer.UseDefaultCredentials = false; 
                //SmtpServer.Port = 587;
                //SmtpServer.Credentials = new System.Net.NetworkCredential(mailcredential, mailpassword);
                //SmtpServer.EnableSsl = true;

                //SmtpServer.Send(mail);
                //t = true;

                // Setup mail message
                MailMessage msg = new MailMessage();
                msg.Subject = "Order Number " + OrderNumber + " is Completed";
                msg.Body = Content;
                msg.From = new MailAddress(mailcredential);
                msg.To.Add(mailto);
                msg.CC.Add(mailcc);
                msg.IsBodyHtml = false; // Can be true or false

                // Setup SMTP client and send message
                SmtpClient smtpClient = new SmtpClient();
                
                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.EnableSsl = false;
                    
                    smtpClient.Port = 587; // Gmail uses port 587
                    smtpClient.UseDefaultCredentials = false; // Must be set BEFORE Credentials below...
                    smtpClient.Credentials = new System.Net.NetworkCredential(mailcredential, mailpassword);
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.Send(msg);
                t=true;
                
            }
            catch (Exception ex)
            {
                t = false;
                
            }
            return t;
        }
        [WebMethod]
        public string getcurrentroutelist()
        {
            try
            {
                
                DataTable ds = new DataTable();
                RouteOrder objroute = new RouteOrder();
                ds = objroute.getroutecurrentlist().Tables[0];
                System.IO.StringWriter obj = new System.IO.StringWriter();
                if (ds.Rows.Count > 0)
                {
                    //DT = ds.Tables[0];
                    ds.WriteXml(obj);
                    _output = obj.ToString();
                }
                else
                {
                    _output = "No Records Found";
                }
                
            }
            catch (Exception ex)
            {
                _output = "Try Again Later";
            }
            return _output;
        }
        [WebMethod]
        public string getRoute_Ordernumber(int route_id)
        {
            try
            {
                
                DataTable dt = new DataTable();
                RouteOrder objroute = new RouteOrder();
                dt = objroute.getroutelistone(route_id).Tables[0];
                System.IO.StringWriter obj = new System.IO.StringWriter();
                if (dt.Rows.Count > 0)
                {
                    //DT = ds.Tables[0];
                    dt.WriteXml(obj);
                    _output = obj.ToString();
                }
                else
                {
                    _output = "No Records Found";
                }
                
            }
            catch (Exception ex)
            {
                _output = "Try Again Later";
            }
            return _output;
        }
        [WebMethod]
        public string getAllRoute_Ordernumber()
        {
            try
            {
                
                DataTable dt = new DataTable();
                RouteOrder objroute = new RouteOrder();
                dt = objroute.getroutelistnew().Tables[0];
                System.IO.StringWriter obj = new System.IO.StringWriter();
                if (dt.Rows.Count > 0)
                {
                    //DT = ds.Tables[0];
                    dt.WriteXml(obj);
                    _output = obj.ToString();
                }
                else
                {
                    _output = "No Records Found";
                }
                
            }
            catch (Exception ex)
            {
                _output = "Try Again Later";
            }
            return _output;
        }
        
        [WebMethod]
        public int setRoute_Latitude_Longitude(int route_id, string latitude,string longitude)
        {
            int i=0;
            try
            {
                
                RouteOrder objroute = new RouteOrder();
                i = objroute.set_latitude_longitude(route_id, latitude, longitude);
                
            }
            catch (Exception ex)
            {
            }
            return i;
        }
        [WebMethod]
        public string get_Latitude_Longitude_History(int route_id, DateTime updateddate)
        {
            try
            {
                
                DataTable dt = new DataTable();
                RouteOrder objroute = new RouteOrder();
                dt = objroute.get_latitude_longitude(route_id, updateddate);
                System.IO.StringWriter obj = new System.IO.StringWriter();
                if (dt.Rows.Count > 0)
                {
                    //DT = ds.Tables[0];
                    dt.WriteXml(obj);
                    _output = obj.ToString();
                }
                else
                {
                    _output = "No Records Found";
                }
                
            }
            catch (Exception ex)
            {
                _output = "Try Again Later";
            }
            return _output;
        }
        [WebMethod]
        public string getRoute_Delivery_Details(int route_id)
        {
            try
            {
                
                //string ordernumber = "";
                //DataSet ds = new DataSet();
                //RouteOrder objroute = new RouteOrder();
                //ds=objroute.getroutelistone(route_id);
                //if (ds.Tables.Count > 0)
                //{
                //    if (ds.Tables[0].Rows.Count > 0)
                //    {
                //        ordernumber = ds.Tables[0].Rows[0]["ordernumber"].ToString();
                //    }
                //}
                //if (ordernumber != "")
                //{
                //    DataTable DTRoute_DeliveryDetails = new DataTable();
                //    APIMethods objRDD = new APIMethods();
                //    DTRoute_DeliveryDetails = objRDD.getOrderDetailsbyorderid(ordernumber);
                //    System.IO.StringWriter obj = new System.IO.StringWriter();
                //    if (DTRoute_DeliveryDetails.Rows.Count > 0)
                //    {
                //        //DT = ds.Tables[0];
                //        DTRoute_DeliveryDetails.WriteXml(obj);
                //        _output = obj.ToString();
                //    }
                //    else
                //    {
                //        _output = "No Records Found";
                //    }
                //}

                DataTable DTRoute_DeliveryDetails = new DataTable();
                APIMethods objRDD = new APIMethods();
                DTRoute_DeliveryDetails = objRDD.getOrderDetailsbyorderid(route_id);
                System.IO.StringWriter obj = new System.IO.StringWriter();
                if (DTRoute_DeliveryDetails.Rows.Count > 0)
                {
                    //DT = ds.Tables[0];
                    DTRoute_DeliveryDetails.WriteXml(obj);
                    _output = obj.ToString();
                }
                else
                {
                    _output = "No Records Found";
                }
                
            }
            catch (Exception ex)
            {
                _output = "Try Again Later";
            }
            return _output;
        }

        [WebMethod]
        public string Insert_Delivery_Detail(string route_name,string ordernumber,string order_status,double amount,string driver_name,string driver_phone,string deliveryboy_name,
string deliveryboy_phone,string status)//,DateTime lastupdatedate
        {
            try
            {
                
                string queryString = "";//
                queryString = @"Insert into ffhp_delivery_details (route_name,ordernumber,order_status,amount,driver_name,driver_phone,deliveryboy_name,deliveryboy_phone,status,lastupdatedate)values(
'" + route_name + "','" + ordernumber + "','" + order_status + "'," + amount + ",'" + driver_name + "','" + driver_phone + "','" + deliveryboy_name + "','" + deliveryboy_phone + "','" + status + "',DATE(NOW()))";//,STR_TO_DATE('" + String.Format("{0:MM/dd/yyyy}", lastupdatedate) + "', '%c-%e-%Y')
                RouteOrder objroute = new RouteOrder();
                int i=objroute.Insert_Delivery_Detail(queryString);
                _output = Convert.ToString(i);
                
            }
            catch (Exception ex)
            {
                _output = ex.ToString();
            }
            return _output;
        }
        [WebMethod]
        public string Update_Delivery_Detail(string route_name, string ordernumber,double collection_amount,double reduction,double balance,
string delivery_starttime, string delivery_closetime, float kilometer, string status, string start_close_time,string latitude,string longitude,string description)//, DateTime lastupdatedate
        {
            try
            {
                      
                string queryString = "";
                queryString = @"update ffhp_delivery_details set collection_amount=" + collection_amount + ",reduction=" + reduction + ",balance=" + balance + ",delivery_starttime='" + delivery_starttime + "',delivery_closetime='" + delivery_closetime + "',kilometer=" + kilometer + ",status='" + status + "',start_close_time='" + start_close_time + "',latitude='" + latitude + "',longitude='" + longitude + "',description='" + description + "',lastupdatedate=DATE(NOW()) where route_name='" + route_name + "' and ordernumber='" + ordernumber + "'";//STR_TO_DATE('" + String.Format("{0:MM/dd/yyyy}", lastupdatedate) + "', '%c-%e-%Y')
                RouteOrder objroute = new RouteOrder();
                int i = objroute.Insert_Delivery_Detail(queryString);
                _output = Convert.ToString(i);
            }
            catch (Exception ex)
            {
                _output = ex.ToString();
            }
            return _output;
        }
        [WebMethod]
        public string Get_Delivery_Detail(DateTime fromdate,DateTime todate)
        {
            try
            {
                
                DataTable dt = new DataTable();
                RouteOrder objroute = new RouteOrder();
                dt = objroute.Get_Delivery_Detail(fromdate,todate).Tables[0];
                System.IO.StringWriter obj = new System.IO.StringWriter();
                if (dt.Rows.Count > 0)
                {
                    //DT = ds.Tables[0];
                    dt.WriteXml(obj);
                    _output = obj.ToString();
                }
                else
                {
                    _output = "No Records Found";
                }
                
            }
            catch (Exception ex)
            {
                _output = "Try Again Later";
            }
            return _output;
        }
        [WebMethod]
        public string Get_Mobile_Orders()
        {
            try
            {
                
                DataTable dt = new DataTable();
                APIMethods objmobileorders = new APIMethods();
                dt = objmobileorders.get_ffhp_mobile_orders();
                System.IO.StringWriter obj = new System.IO.StringWriter();
                if (dt.Rows.Count > 0)
                {
                    //DT = ds.Tables[0];
                    dt.WriteXml(obj);
                    _output = obj.ToString();
                }
                else
                {
                    _output = "No Records Found";
                }
                
            }
            catch (Exception ex)
            {
                _output = "Try Again Later";
            }
            return _output;
        }
        [WebMethod]
        public string Get_Product_Weight_Price()
        {
            try
            {
                
                DataTable dt = new DataTable();
                APIMethods objmobileorders = new APIMethods();
                dt = objmobileorders.getProduct_Weight_Price();
                System.IO.StringWriter obj = new System.IO.StringWriter();
                if (dt.Rows.Count > 0)
                {
                    //DT = ds.Tables[0];
                    dt.WriteXml(obj);
                    _output = obj.ToString();
                }
                else
                {
                    _output = "No Records Found";
                }
                
            }
            catch (Exception ex)
            {
                _output = "Try Again Later";
            }
            return _output;
        }
        [WebMethod]
        public DataTable getProduct_Weight_Price_for_Before_After_Sale()
        {
            DataTable dt = new DataTable();
            try
            {
                
                APIMethods objmobileorders = new APIMethods();
                dt = objmobileorders.getProduct_Weight_Price_for_Before_After_Sale();
                
            }
            catch (Exception ex)
            {
                //_output = "Try Again Later";
            }
            return dt;
        }
        [WebMethod]
        public string getPurchase()
        {
            System.IO.StringWriter objwr = new System.IO.StringWriter();
            DataTable _output = new DataTable();
            try
            {
                
                Purchase objpurchase = new Purchase();
                _output = objpurchase.getPurchase();
                if (_output.Rows.Count > 0)
                {
                    _output.WriteXml(objwr);
                }
                
            }
            catch (Exception ex)
            {
                //_output = "Try Again Later";
            }
            return objwr.ToString();
        }
        [WebMethod]
        public string getPurchase_datewise(string from, string to)
        {
            System.IO.StringWriter objwr = new System.IO.StringWriter();
            DataTable _output = new DataTable();
            try
            {
                
                Purchase objpurchase = new Purchase();
                _output = objpurchase.getPurchase(Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}",from)).ToString("yyyy-MM-dd"), Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}",to)).ToString("yyyy-MM-dd"));
                if (_output.Rows.Count > 0)
                {
                    _output.WriteXml(objwr);
                }
                
            }
            catch (Exception ex)
            {
                //_output = "Try Again Later";
            }
            return objwr.ToString();
        }
        [WebMethod]
        public string getPurchase_Datewise_pid(string from, string to, int pid)
        {
            System.IO.StringWriter objwr = new System.IO.StringWriter();
            DataTable _output = new DataTable();
            try
            {
                
                Purchase objpurchase = new Purchase();
                _output = objpurchase.getPurchase(Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", from)).ToString("yyyy-MM-dd"), Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", to)).ToString("yyyy-MM-dd"), pid);
                if (_output.Rows.Count > 0)
                {
                    _output.WriteXml(objwr);
                }
                
            }
            catch (Exception ex)
            {
                //_output = "Try Again Later";
            }
            return objwr.ToString();
        }
        //Tray Loading

        //[WebMethod]
        //public string Update_Tray_Loading(string order_no,string product_id,string name,string status,string unit,string weight,string cust_name,string url,string pack_status,string order_status,string route)//,DateTime lastupdatedate
        //{
        //    try
        //    {
        //        string queryString = "";//
        //        queryString = @"update ffhp_tray_loading set name='" + name + "',status='" + status + "',unit='" + unit + "',weight='" + weight + "',cust_name='" + cust_name + "',url='" + url + "',pack_status='" + pack_status + "',order_status='" + order_status + "',route='" + route + "',date=DATE(NOW()) where order_no='" + order_no + "' and product_id='" + product_id + "'";//,STR_TO_DATE('" + String.Format("{0:MM/dd/yyyy}", lastupdatedate) + "', '%c-%e-%Y')
        //        APIMethods objtryloading = new APIMethods();
        //        int i = objtryloading.update_tray_loading(queryString);
        //        _output = i.ToString();//
        //    }
        //    catch (Exception ex)
        //    {
        //        _output = ex.ToString();
        //    }
        //    return _output;
        //}
        [WebMethod]
        public string Update_Tray_Loading(string jsonstring)
        {
            try
            {
                
                //string queryString = "";//
                //queryString = @"update ffhp_tray_loading set name='" + name + "',status='" + status + "',unit='" + unit + "',weight='" + weight + "',cust_name='" + cust_name + "',url='" + url + "',pack_status='" + pack_status + "',order_status='" + order_status + "',route='" + route + "',date=DATE(NOW()) where order_no='" + order_no + "' and product_id='" + product_id + "'";//,STR_TO_DATE('" + String.Format("{0:MM/dd/yyyy}", lastupdatedate) + "', '%c-%e-%Y')
                APIMethods objtryloading = new APIMethods();
                int i = objtryloading.update_tray_loading(jsonstring);
                _output = Convert.ToString(i);//
            }
            catch (Exception ex)
            {
                _output = ex.ToString();
            }
            return _output;
        }
        [WebMethod]
        public string Insert_Tray_Loading(string jsonstring)
        {
            try
            {
                
                APIMethods objtrayloading = new APIMethods();
                int i = objtrayloading.insert_tray_laoding(jsonstring);
                _output = i.ToString();
            }
            catch (Exception ex)
            {
                _output = ex.ToString();
            }
            return _output;
        }
        [WebMethod]
        public string Get_Tray_Loading()
        {
            System.IO.StringWriter objwr = new System.IO.StringWriter();
            DataTable _output = new DataTable();
            try
            {
                
                APIMethods objtrayloading = new APIMethods();
                _output = objtrayloading.getTrayloading();
                if (_output.Rows.Count > 0)
                {
                    _output.WriteXml(objwr);
                }
            }
            catch (Exception ex)
            {
                //_output = "Try Again Later";
            }
            return objwr.ToString();
        }
        [WebMethod]
        //public string Insert_Delivery_Status(int customer_id,int customer_address_id,string customer_name,string order_number,string route_number,DateTime datescanned,DateTime timescanned,decimal order_amount,decimal billed_amount,string payment_mode)//,DateTime lastupdatedate
        public string Insert_Delivery_Status(DataTable dtdeliverystatus)
        {
            try
            {
                
                int i = 0;
                if (dtdeliverystatus.Rows.Count > 0)
                {

                    int flagcount = 0;
                    string queryStringcount = "";
                    string queryString = "";//
                    string deletequery = "";

                    DateTime CDT = indianTime;//DateTime.Now.Date; //DateTime.ParseExact(DateTime.Now.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);//Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    RouteOrder objroute = new RouteOrder();
                    string queryStringflagcount = @"select count(*) from ffhp_delivery_status where route_number='" + dtdeliverystatus.Rows[0]["route_number"].ToString() + "' and date='" + CDT.ToString("yyyy-MM-dd") + "' and start_flag=1";
                    flagcount = objroute.Get_Delivery_Status_Flag(queryStringflagcount);
                    if (flagcount == 0)
                    {
                        deletequery = @"delete from ffhp_delivery_status where route_number='" + dtdeliverystatus.Rows[0]["route_number"].ToString() + "' and date='" + CDT.ToString("yyyy-MM-dd") + "'";
                        objroute.Delete_Delivery_Status(deletequery);
                        foreach (DataRow row in dtdeliverystatus.Rows)
                        {
                            queryStringcount = @"select count(*) from ffhp_delivery_status where order_number='" + row["order_number"].ToString() + "' and date='" + CDT.ToString("yyyy-MM-dd") + "'";

                            DateTime DT = DateTime.ParseExact(row["datescanned"].ToString(), "MM-dd-yyyy", CultureInfo.InvariantCulture);//Convert.ToDateTime(row["datescanned"].ToString());


                            queryString = @"Insert into ffhp_delivery_status (customer_id,customer_address_id,customer_name,order_number,route_number,datescanned,timescanned,order_amount,billed_amount,payment_mode,no_of_bags,date)values(
" + row["customer_id"].ToString() + "," + row["customer_address_id"].ToString() + ",'" + row["customer_name"].ToString() + "','" + row["order_number"].ToString() + "','" + row["route_number"].ToString() + "','" + DT.ToString("yyyy-MM-dd") + "','" + row["timescanned"].ToString() + "','" + row["order_amount"].ToString() + "','" + row["billed_amount"].ToString() + "','" + row["payment_mode"].ToString() + "','" + row["no_of_bags"].ToString() + "','" + CDT.ToString("yyyy-MM-dd") + "')";

                            i = objroute.Insert_Delivery_Status(queryStringcount, queryString);
                        }
                    }
                    else
                    {
                        i = -1;
                    }
                    //queryString = @"Insert into ffhp_delivery_status (customer_id,customer_address_id,customer_name,order_number,route_number,datescanned,timescanned,order_amount,billed_amount,payment_mode)values(
                    //" + customer_id + "," + customer_address_id + ",'" + customer_name + "'," + order_number + ",'" + route_number + "','" + datescanned + "','" + timescanned + "','" + order_amount + "','" + billed_amount + "','" + payment_mode + "')";//,STR_TO_DATE('" + String.Format("{0:MM/dd/yyyy}", lastupdatedate) + "', '%c-%e-%Y')

                }
                _output = Convert.ToString(i);
            }
            catch (Exception ex)
            {
                _output = "0";//ex.ToString();
            }
            return _output;
        }
        [WebMethod]
        public string Update_Delivery_Status(string jsonstring)
        {
            try
            {
                
                //string queryString = "";//
                //queryString = @"update ffhp_tray_loading set name='" + name + "',status='" + status + "',unit='" + unit + "',weight='" + weight + "',cust_name='" + cust_name + "',url='" + url + "',pack_status='" + pack_status + "',order_status='" + order_status + "',route='" + route + "',date=DATE(NOW()) where order_no='" + order_no + "' and product_id='" + product_id + "'";//,STR_TO_DATE('" + String.Format("{0:MM/dd/yyyy}", lastupdatedate) + "', '%c-%e-%Y')
                APIMethods objupdatedeliverystatus = new APIMethods();
                int i = objupdatedeliverystatus.update_Delivery_Status(jsonstring);
                _output = Convert.ToString(i);//
            }
            catch (Exception ex)
            {
                _output = ex.ToString();
            }
            return _output;
        }
        [WebMethod]
        public string Get_Delivery_Status(DateTime fromdate, DateTime todate)
        {
            try
            {
                
                DataTable dt = new DataTable();
                APIMethods objdeliverystatus = new APIMethods();
                dt = objdeliverystatus.Get_Delivery_Status(fromdate, todate).Tables[0];//objdeliverystatus.Get_delivery_status_new();
                System.IO.StringWriter obj = new System.IO.StringWriter();
                if (dt.Rows.Count > 0)
                {
                    //DT = ds.Tables[0];
                    dt.WriteXml(obj);
                    _output = obj.ToString();
                }
                else
                {
                    _output = "No Records Found";
                }
            }
            catch (Exception ex)
            {
                _output = "Try Again Later";
            }
            return _output;
        }
        [WebMethod]
        public string Get_Delivery_Status_Ordernumber(String Ordernumber)
        {
            try
            {

                DataTable dt = new DataTable();
                APIMethods objdeliverystatus = new APIMethods();
                dt = objdeliverystatus.Get_Delivery_Status_Ordernumber(Ordernumber).Tables[0];//objdeliverystatus.Get_delivery_status_new();
                System.IO.StringWriter obj = new System.IO.StringWriter();
                if (dt.Rows.Count > 0)
                {
                    //DT = ds.Tables[0];
                    dt.WriteXml(obj);
                    _output = obj.ToString();
                }
                else
                {
                    _output = "No Records Found";
                }
            }
            catch (Exception ex)
            {
                _output = "Try Again Later";
            }
            return _output;
        }
        [WebMethod]
        public string Set_Start_Route(string Routename)
        {
            try
            {
                
                DataTable dt = new DataTable();
                APIMethods objdeliverystatus = new APIMethods();
                int i = objdeliverystatus.Set_Route_Start(Routename);
                System.IO.StringWriter obj = new System.IO.StringWriter();
                if (i > 0)
                {
                    //DT = ds.Tables[0];
                    //dt.WriteXml(obj);
                    _output = Convert.ToString(i);

                    //code for one time send data for ffhp route orders history


//                    INSERT INTO `ffhp_route_orders_history` ( route_id, route_name, ordernumber, driver_name, driver_phone, deliveryboy_name, deliveryboy_phone ) (

//SELECT route_id, route_name, ordernumber, driver_name, driver_phone, deliveryboy_name, deliveryboy_phone
//FROM ffhp_route_orders
//WHERE ordernumber IS NOT NULL
//AND ordernumber != '' AND route_id =Routename)


//                    select * from `ffhp_route_orders_history` as a right outer join 
//`ffhp_route_orders_history_transaction` as b on a.rhid=b.rhid and a.updateddate between '2015-09-04' and '2015-09-05'
                }
                else
                {
                    _output = Convert.ToString(i);
                }
                
            }
            catch (Exception ex)
            {
                _output = "Try Again Later";
            }
            return _output;
        }

        [WebMethod]
        public string get_ffhp_scan_orders(DateTime fromdate, DateTime todate)
        {
            try
            {
                DataTable dt = new DataTable();
                Purchase objffhpscanorder = new Purchase();
                dt = objffhpscanorder.get_ffhp_scan_orders(fromdate, todate);
                System.IO.StringWriter obj = new System.IO.StringWriter();
                if (dt.Rows.Count > 0)
                {
                    //DT = ds.Tables[0];
                    dt.WriteXml(obj);
                    _output = obj.ToString();
                }
                else
                {
                    _output = "No Records Found";
                }
            }
            catch (Exception ex)
            {
                _output = "Try Again Later";
            }
            return _output;
        }

        [WebMethod]
        public string get_ffhp_before_after_sale(DateTime fromdate, DateTime todate)
        {
            try
            {
                DataTable dt = new DataTable();
                Purchase objffhpafsale = new Purchase();
                dt = objffhpafsale.get_ffhp_before_after_sale(fromdate, todate);
                System.IO.StringWriter obj = new System.IO.StringWriter();
                if (dt.Rows.Count > 0)
                {
                    //DT = ds.Tables[0];
                    dt.WriteXml(obj);
                    _output = obj.ToString();
                }
                else
                {
                    _output = "No Records Found";
                }
            }
            catch (Exception ex)
            {
                _output = "Try Again Later";
            }
            return _output;
        }

        [WebMethod]
        public string getLPRdata()
        {
            System.IO.StringWriter objwr = new System.IO.StringWriter();
            DataTable _output = new DataTable();
            try
            {
                
                TotalWeightlprdata obj = new TotalWeightlprdata();
                _output = obj.getffhpproducts_api();
                if (_output.Rows.Count > 0)
                {
                    _output.WriteXml(objwr);
                }
                
            }
            catch (Exception ex)
            {
                //_output = "Try Again Later";
            }
            return objwr.ToString();
        }
        [WebMethod]
        public string Insert_Payment_Collection(string jsonstring)
        {
            try
            {
                
                APIMethods objpaymentcollection = new APIMethods();
                int i = objpaymentcollection.insert_payment_collection(jsonstring);
                _output = i.ToString();
            }
            catch (Exception ex)
            {
                _output = ex.ToString();
            }
            return _output;
        }
        [WebMethod]
        public string Insert_Payment_Collection_deliveryboy(string jsonstring)
        {
            try
            {

                APIMethods objpaymentcollection = new APIMethods();
                int i = objpaymentcollection.insert_payment_collection_deliveryboy(jsonstring);
                _output = i.ToString();
            }
            catch (Exception ex)
            {
                _output = ex.ToString();
            }
            return _output;
        }
        [WebMethod]
        public string Get_Payment_Collection(DateTime fromdate,DateTime todate)
        {
            try
            {
                
                DataTable dt = new DataTable();
                APIMethods objpaymentcollection = new APIMethods();
                dt = objpaymentcollection.get_payment_collection(fromdate,todate).Tables[0];//objdeliverystatus.Get_delivery_status_new();
                System.IO.StringWriter obj = new System.IO.StringWriter();
                if (dt.Rows.Count > 0)
                {
                    //DT = ds.Tables[0];
                    dt.WriteXml(obj);
                    _output = obj.ToString();
                }
                else
                {
                    _output = "No Records Found";
                }
            }
            catch (Exception ex)
            {
                _output = "Try Again Later";
            }
            return _output;
        }
        [WebMethod]
        public string Upload_Stock_Sale_Data(DataTable dt)
        {
            try
            {
                int i = 0;
                APIMethods objupload = new APIMethods();
                i = objupload.Upload_Stock_Sale_Data(dt);
                _output = i.ToString();
            }
            catch (Exception ex)
            {
                _output = ex.ToString();
            }
            return _output;
        }
        [WebMethod]
        public string Upload_ffhporders(DataTable dt)
        {
            try
            {
                int i = 0;
                APIMethods objupload = new APIMethods();
                i = objupload.Upload_ffhporders(dt);
                _output = i.ToString();
            }
            catch (Exception ex)
            {
                _output = ex.ToString();
            }
            return _output;
        }
        //[WebMethod]
        //public string Send_InPacking_SMS(string ordernumber,string smstype)
        //{
        //    try
        //    {

        //        sms objsendinpackingsms = new sms();
        //        objsendinpackingsms.Sendsms_mail(ordernumber, smstype);
        //        _output = "success";
        //    }
        //    catch (Exception ex)
        //    {
        //        _output = "Try Again Later";
        //    }
        //    return _output;
        //}

        //for delivery boy
        [WebMethod]
        public string Get_DeliveryBoy(string username, string password)
        {
            try
            {
                DataTable dt = new DataTable();
                APIMethods objroute = new APIMethods();
                dt = objroute.Get_DeliveryBoy(username,password).Tables[0];
                System.IO.StringWriter obj = new System.IO.StringWriter();
                if (dt.Rows.Count > 0)
                {
                    //DT = ds.Tables[0];
                    dt.WriteXml(obj);
                    _output = obj.ToString();
                }
                else
                {
                    _output = "No Records Found";
                }
            }
            catch (Exception ex)
            {
                _output = "Try Again Later";
            }
            return _output;
        }
        //for delivery boy login
        [WebMethod]
        public string Get_User_Access(string username, string password, string designation)
        {
            try
            {
                DataTable dt = new DataTable();
                APIMethods objroute = new APIMethods();
                dt = objroute.Get_User_Access(username, password, designation).Tables[0];
                System.IO.StringWriter obj = new System.IO.StringWriter();
                if (dt.Rows.Count > 0)
                {
                    //DT = ds.Tables[0];
                    dt.WriteXml(obj);
                    _output = obj.ToString();
                }
                else
                {
                    _output = "No Records Found";
                }
            }
            catch (Exception ex)
            {
                _output = "Try Again Later";
            }
            return _output;
        }
        [WebMethod]
        public string getAllDeliveryboy_Ordernumber()
        {
            try
            {

                DataTable dt = new DataTable();
                deliveryboy_order objroute = new deliveryboy_order();
                dt = objroute.get_deliveryboy_routelistnew().Tables[0];
                System.IO.StringWriter obj = new System.IO.StringWriter();
                if (dt.Rows.Count > 0)
                {
                    //DT = ds.Tables[0];
                    dt.WriteXml(obj);
                    _output = obj.ToString();
                }
                else
                {
                    _output = "No Records Found";
                }

            }
            catch (Exception ex)
            {
                _output = "Try Again Later";
            }
            return _output;
        }
        public DataTable Getcustomerwithcoupon(string couponcodes)
        {
            DataTable _output = new DataTable();
            try
            {
                APIMethods obj = new APIMethods();
                _output = obj.Getcustomerwithcoupon(couponcodes);
            }
            catch (Exception ex)
            {
                
            }
            return _output;
        }
        //new
        [WebMethod]
        public string insertjsoncomplaint(string jsoncomplaint)
        {
            try
            {
                APIMethods objcomplaint = new APIMethods();
                int i = objcomplaint.insertjsoncomplaint(jsoncomplaint);
                _output = i.ToString();
            }
            catch (Exception ex)
            {
                _output = ex.ToString();
            }
            return _output;
        }
        [WebMethod]
        public string getcomplaint(string userid, string status)
        {
            try
            {
                DataTable dt = new DataTable();
                APIMethods objcomplaint = new APIMethods();
                dt = objcomplaint.getcomplaint(userid, status).Tables[0];
                System.IO.StringWriter obj = new System.IO.StringWriter();
                if (dt.Rows.Count > 0)
                {
                    //DT = ds.Tables[0];
                    dt.WriteXml(obj);
                    _output = obj.ToString();
                }
                else
                {
                    _output = "No Records Found";
                }
            }
            catch (Exception ex)
            {
                _output = "Try Again Later";
            }
            return _output;
        }
        [WebMethod]
        public string updatecomplaint(int complaintid, string status)
        {
            try
            {
                APIMethods objcomplaint = new APIMethods();
                int i = objcomplaint.updatecomplaint(complaintid, status);
                _output = Convert.ToString(i);//
            }
            catch (Exception ex)
            {
                _output = ex.ToString();
            }
            return _output;
        }
        [WebMethod]
        public string storeUser(string name, string email, string gcm_regid)
        {
            try
            {
                APIMethods objstoreusers = new APIMethods();
                int i = objstoreusers.storeUser(name, email, gcm_regid);
                _output = i.ToString();
            }
            catch (Exception ex)
            {
                _output = ex.ToString();
            }
            return _output;
        }
        [WebMethod]
        public string getAllUsers()
        {
            try
            {
                DataTable dt = new DataTable();
                APIMethods objstoreusers = new APIMethods();
                dt = objstoreusers.getAllUsers().Tables[0];
                System.IO.StringWriter obj = new System.IO.StringWriter();
                if (dt.Rows.Count > 0)
                {
                    //DT = ds.Tables[0];
                    dt.WriteXml(obj);
                    _output = obj.ToString();
                }
                else
                {
                    _output = "No Records Found";
                }
            }
            catch (Exception ex)
            {
                _output = "Try Again Later";
            }
            return _output;
        }
        [WebMethod]
        public DataTable get_Totalweight_from_PQ()
        {
            DataTable _output = new DataTable("PQ_Template");
            try
            {
                APIMethods obj = new APIMethods();
                _output = obj.get_Totalweight_from_PQ();

            }
            catch (Exception ex)
            {
                //_output = "Try Again Later";
            }
            return _output;
        }
        [WebMethod]
        public string Getalacartewithnamesjson()
        {
            string s = "";
            DataTable _output = new DataTable();
            try
            {
                APIMethods obj = new APIMethods();
                _output = obj.Getalacartewithnames("100040203");
                s = obj.DataTableToJSONWithStringBuilder(_output);
            }
            catch (Exception ex)
            {
                //_output = "Try Again Later";
            }
            return s;
        }
        [WebMethod]
        public string Get_Email(string ordernumber)
        {
            try
            {
                DataTable dt = new DataTable();
                APIMethods objroute = new APIMethods();
                dt = objroute.get_email(ordernumber).Tables[0];
                System.IO.StringWriter obj = new System.IO.StringWriter();
                if (dt.Rows.Count > 0)
                {
                    //DT = ds.Tables[0];
                    dt.WriteXml(obj);
                    _output = obj.ToString();
                }
                else
                {
                    _output = "No Records Found";
                }
            }
            catch (Exception ex)
            {
                _output = "Try Again Later";
            }
            return _output;
        }
        [WebMethod]
        public string getdriver_deliveryboylist()
        {
            try
            {
                DataTable dt = new DataTable();
                deliveryboy_order obj_delivery = new deliveryboy_order();
                dt = obj_delivery.getdriver_deliveryboylist().Tables[0];

                System.IO.StringWriter obj = new System.IO.StringWriter();
                if (dt.Rows.Count > 0)
                {
                    //DT = ds.Tables[0];
                    dt.WriteXml(obj);
                    _output = obj.ToString();
                }
                else
                {
                    _output = "No Records Found";
                }
            }
            catch (Exception ex)
            {
                _output = "Try Again Later";
            }
            return _output;
        }
        [WebMethod]
        public string update_ffhp_route_orders(string _deliveryboy_ordernumber, string _payment_pending_ordernumber, string _driver_name, string _driver_phone, string _deliveryboy_name, string _deliveryboy_phone, string _route_id)
        {
            try
            {
                APIMethods objassign_deliveryboy_ordernumber = new APIMethods();
                int i = objassign_deliveryboy_ordernumber.update_ffhp_route_orders(_deliveryboy_ordernumber,_payment_pending_ordernumber,_driver_name,_driver_phone,_deliveryboy_name,_deliveryboy_phone,_route_id);
                _output = Convert.ToString(i);//
            }
            catch (Exception ex)
            {
                _output = ex.ToString();
            }
            return _output;
        }
        [WebMethod]
        public string clear_ffhp_route_orders(string _route_id)
        {
            try
            {
                APIMethods objclear_deliveryboy_ordernumber = new APIMethods();
                int i = objclear_deliveryboy_ordernumber.clear_ffhp_route_orders(_route_id);
                _output = Convert.ToString(i);//
            }
            catch (Exception ex)
            {
                _output = ex.ToString();
            }
            return _output;
        }
        [WebMethod]
        public string getproducts(string name)
        {
            //string jsonstring = "";
            //try
            //{
            //    DataTable dt = new DataTable();
            //    StockSaleEntry obj = new StockSaleEntry();
            //    dt=obj.Readdata_from_excel_to_datatable("tool_stockproducts");
            //    //var result = from o in dt.AsEnumerable() select o.Field<string>("Name");
            //    var result = from o in dt.AsEnumerable() select o.Field<string>("Name") + " " + o.Field<double>("productid");
            //    //var result = from o in dt.AsEnumerable()
            //    //             select new
            //    //               {
            //    //                   Name = o.Field<string>("Name") + " " + o.Field<double>("productid")
            //    //               };
            //    result.ToList();
            //    var jsonSerialiser = new JavaScriptSerializer();
            //    var json = jsonSerialiser.Serialize(result.ToList());
            //    jsonstring = json;
            //}
            //catch (Exception ex)
            //{

            //}
            //return jsonstring;

            try
            {
                APIMethods objgetproducts = new APIMethods();
                string jsonstring = objgetproducts.getproducts_StockSaleEntry();
                _output = jsonstring;
            }
            catch (Exception ex)
            {
                _output = ex.ToString();
            }
            return _output;
        }
        [WebMethod]
        public string Update_Stock_Status(string jsonstring,string stocktype)
        {
            try
            {

                //string queryString = "";//
                //queryString = @"update ffhp_tray_loading set name='" + name + "',status='" + status + "',unit='" + unit + "',weight='" + weight + "',cust_name='" + cust_name + "',url='" + url + "',pack_status='" + pack_status + "',order_status='" + order_status + "',route='" + route + "',date=DATE(NOW()) where order_no='" + order_no + "' and product_id='" + product_id + "'";//,STR_TO_DATE('" + String.Format("{0:MM/dd/yyyy}", lastupdatedate) + "', '%c-%e-%Y')
                APIMethods objupdatestockstatus = new APIMethods();
                int i = objupdatestockstatus.update_stock_status(jsonstring, stocktype);
                _output = Convert.ToString(i);//
            }
            catch (Exception ex)
            {
                _output = ex.ToString();
            }

            return jsonstring;
        }
        [WebMethod]
        public string Get_Stock_Status(string stocktype)
        {
            try
            {
                APIMethods objgetstockstatus = new APIMethods();
                string jsonstring = objgetstockstatus.get_stock_status(stocktype);
                _output = jsonstring;
            }
            catch (Exception ex)
            {
                _output = ex.ToString();
            }
            return _output;
        }
        public string getproductsstocksaleentry(string name)
        {
            try
            {
                APIMethods objgetproducts = new APIMethods();
                string jsonstring = objgetproducts.getproducts_StockSaleEntry();
                _output = jsonstring;
            }
            catch (Exception ex)
            {
                _output = ex.ToString();
            }
            return _output;
        }
    }
}
