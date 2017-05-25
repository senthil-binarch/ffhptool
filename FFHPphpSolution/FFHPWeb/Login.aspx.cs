using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
//using Renci.SshNet;

namespace FFHPWeb
{
    public partial class Login : System.Web.UI.Page
    {
        string conn = System.Configuration.ConfigurationManager.AppSettings["SqlConnectionString"].ToString();
        string queryString = "";
        string s = "";
        bool t = false;
        string errormsg = "Try again";

        string username = System.Configuration.ConfigurationManager.AppSettings["websiteusername"].ToString();
        string password = System.Configuration.ConfigurationManager.AppSettings["pwd"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            //ConnectSsh obj = new ConnectSsh();
            //SshClient client=obj.SshConnect();
            //obj.SshDisconnect(client);
        }
        protected void btnsubmit_OnClick(object sender, EventArgs e)
        {
            //if (txtusername.Text != "")
            //{
            //    if (txtpassword.Text != "")
            //    {
            //        if (txtusername.Text == username && txtpassword.Text == password)
            //        {
            //            Session["orderid"] = "";
            //            Response.Redirect("ffhpnew.aspx", false);
            //            Session["username"] = username;
            //        }
            //    }
            //}
            getlogindetails();
        }
        private void getlogindetails()
        {
            user obj = new user();

            if (txtusername.Text != "")
            {
                if (txtpassword.Text != "")
                {

                    DataTable dt = new DataTable();
                    try
                    {
                        SqlConnection con = new SqlConnection(conn);
                        con.Open();
                        SqlCommand cmd = new SqlCommand("sp_ffhpuser", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("userid", 0);
                        cmd.Parameters.AddWithValue("username", txtusername.Text.ToString());
                        cmd.Parameters.AddWithValue("email", "");
                        cmd.Parameters.AddWithValue("pwd", obj.Encrypt(txtpassword.Text.ToString()));
                        cmd.Parameters.AddWithValue("roleid", 0);
                        cmd.Parameters.AddWithValue("userstatus", "1");
                        cmd.Parameters.AddWithValue("operation", "selectlogin");
                        SqlParameter outputparam = new SqlParameter();
                        outputparam.ParameterName = "@output";
                        outputparam.DbType = DbType.Int32;
                        outputparam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(outputparam);
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        sda.Fill(dt);
                        con.Close();
                        if (dt.Rows.Count > 0)
                        {
                            lblerror.Text = dt.Rows.Count.ToString() + " " + dt.Rows[0]["username"].ToString();
                            Session["orderid"] = "";
                            Session["username"] = dt.Rows[0]["username"].ToString();
                            Session["roleid"] = dt.Rows[0]["roleid"].ToString();
                            getrolemenupages();
                            Response.Redirect("welcome.aspx", false);
                        }
                        else
                        {
                            lblerror.Text = "Username and Password is Incorrect.";
                        }
                    }
                    catch (Exception ex)
                    {
                        lblerror.Text = ex.ToString();
                    }


                }
            }
        }
        private void getrolemenupages()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(conn);
                con.Open();
                SqlCommand cmd = new SqlCommand("Sp_role_page_ref", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("role_page_ref_id", 0);
                cmd.Parameters.AddWithValue("roleid", Convert.ToInt32(Session["roleid"].ToString()));
                cmd.Parameters.AddWithValue("pageid", 0);
                cmd.Parameters.AddWithValue("operation", "selectpages");
                SqlParameter outputparam = new SqlParameter();
                outputparam.ParameterName = "@output";
                outputparam.DbType = DbType.Int32;
                outputparam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputparam);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();

                Session["menu"] = dt;
            }
            catch (Exception ex)
            {
                //lblerror.Text = errormsg.ToString();
            }
        }
    }
}
