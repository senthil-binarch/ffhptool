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
//using Renci.SshNet;
namespace FFHPWeb
{
    public partial class Product_Update : System.Web.UI.Page
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
            try
            {
                lblerror.Text = "";
                if (Session["username"] != null && Session["username"].ToString() != "")
                {
                    if (!IsPostBack)
                    {
                        if (Session["username"] != null && Session["username"].ToString() != "")
                        {
                            getdropdownbind("dropdown");
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
        }
        protected void Btntest_OnClick(object sender, EventArgs e)
        {
            //ConnectSsh sshobj = new ConnectSsh();
            //SshClient client = sshobj.SshConnect();
            getdropdownbind("gridview");
            //sshobj.SshDisconnect(client);
        }
        public void getdropdownbind(string controlname)
        {
            try
            {
                queryString = @"SELECT count( pack_name ) AS counts, pack_name FROM 1_pack_item_ffhp_product GROUP BY pack_name ORDER BY count( pack_name )";
                if (queryString != "")
                {
                    MySqlConnection con = new MySqlConnection(conn);
                    con.Open();

                    MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                    DataSet productlist = new DataSet();
                    adapteradminmail.Fill(productlist, "productlist");
                    if (controlname == "dropdown")
                    {
                        ddlproductlist.Items.Clear();
                        ddlproductlist.DataSource = productlist;
                        ddlproductlist.Items.Add("Select");
                        ddlproductlist.DataTextField = "pack_name";
                        ddlproductlist.DataValueField = "pack_name";
                        ddlproductlist.DataBind();

                        GVProductDetails.Visible = false;
                    }
                    else if (controlname == "gridview")
                    {
                        GVPackDetails.DataSource = productlist;
                        GVPackDetails.DataBind();
                        GVPackDetails.Visible = true;
                    }
                    else
                    {
                        ddlproductlist.Items.Clear();
                        ddlproductlist.DataSource = productlist;
                        ddlproductlist.Items.Add("Select");
                        ddlproductlist.DataTextField = "pack_name";
                        ddlproductlist.DataValueField = "pack_name";
                        ddlproductlist.DataBind();

                        GVProductDetails.Visible = false;

                        GVPackDetails.DataSource = productlist;
                        GVPackDetails.DataBind();

                    }
                    
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString() + "Try again later";
                //StatusMessage("Error", ex.ToString() + "Try again later");
            }
        }
        
        public void getProductDetails()
        {
            try
            {
                //queryString = "SELECT * FROM 1_pack_item_ffhp_product";
                if (ddlproductlist.SelectedIndex != 0)
                {
                    queryString = @"SELECT * FROM 1_pack_item_ffhp_product where pack_name='" + ddlproductlist.SelectedValue.ToString() + "'";
                    if (queryString != "")
                    {
                        MySqlConnection con = new MySqlConnection(conn);
                        con.Open();

                        MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                        DataSet orderlist = new DataSet();
                        adapteradminmail.Fill(orderlist, "ordernumber");

                        GVProductDetails.DataSource = orderlist;
                        GVProductDetails.DataBind();

                        if (orderlist.Tables[0].Rows.Count == 0)
                        {
                            getdropdownbind("dropdown");
                            GVPackDetails.Visible = false;
                        }

                    }
                    traddnew.Visible = false;
                }
                else
                {
                    lblerror.Text = "Please select pack name";
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString() + "Try again later";
                //StatusMessage("Error", ex.ToString() + "Try again later");
            }
        }
        protected void BtnGetPackDetails_OnClick(object sender, EventArgs e)
        {
            //ConnectSsh sshobj = new ConnectSsh();
            //SshClient client = sshobj.SshConnect();
            getdropdownbind("gridview");
            //sshobj.SshDisconnect(client);
        }
        protected void BtnGetPackDetailsCancel_OnClick(object sender, EventArgs e)
        {
            GVPackDetails.Visible = false;
        }
        protected void ddlproductlist_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //ConnectSsh sshobj = new ConnectSsh();
            //SshClient client = sshobj.SshConnect();
            if (ddlproductlist.SelectedIndex != 0)
            {
                getProductDetails();
                GVProductDetails.Visible = true;
            }
            else
            {
                GVProductDetails.Visible = false;
                traddnew.Visible = false;
            }
            //sshobj.SshDisconnect(client);
        }
        protected void GVPackDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //ConnectSsh sshobj = new ConnectSsh();
            //SshClient client = sshobj.SshConnect();
            GVPackDetails.EditIndex = e.NewEditIndex;
            getdropdownbind("gridview");
            //sshobj.SshDisconnect(client);
        }

        protected void GVPackDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            //ConnectSsh sshobj = new ConnectSsh();
            //SshClient client = sshobj.SshConnect();
            GVPackDetails.EditIndex = -1;
            getdropdownbind("gridview");
            //sshobj.SshDisconnect(client);
        }

        protected void GVPackDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = (GridViewRow)GVPackDetails.Rows[e.RowIndex];

                TextBox productname = (TextBox)row.FindControl("txtpackname");

                string Hfpackname = ((HiddenField)row.FindControl("HFpackname")).Value.ToString();

                string txtpackname = ((TextBox)row.FindControl("txtpackname")).Text.ToString();

                if (txtpackname != "")
                {

                    if (Hfpackname != txtpackname)
                    {
                        queryString = @"UPDATE 1_pack_item_ffhp_product set pack_name='" + txtpackname + "' where pack_name='" + Hfpackname + "'";
                        if (queryString != "")
                        {
                            //ConnectSsh sshobj = new ConnectSsh();
                            //SshClient client = sshobj.SshConnect();
                            MySqlConnection con = new MySqlConnection(conn);
                            con.Open();
                            MySqlCommand cmd = new MySqlCommand(queryString, con);
                            cmd.ExecuteNonQuery();
                            GVPackDetails.EditIndex = -1;
                            getdropdownbind("");
                            //sshobj.SshDisconnect(client);
                        }

                    }
                    else
                    {
                        lblerror.Text = "Values must be different";
                        //StatusMessage("Error", "Directory name must be different");
                    }
                }
                else
                {
                    lblerror.Text = "Enter the Pack Nam";
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString() + "Try again later";
                //StatusMessage("Error", ex.ToString() + "Try again later");
            }
        }
        protected void BtnPackdelete_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = (GridViewRow)(((Control)sender).NamingContainer);
                string packname = ((HiddenField)row.FindControl("HFpacknameout")).Value.ToString();

                queryString = @"DELETE FROM 1_pack_item_ffhp_product where pack_name='" + packname + "'";
                if (queryString != "")
                {
                    //ConnectSsh sshobj = new ConnectSsh();
                    //SshClient client = sshobj.SshConnect();
                    MySqlConnection con = new MySqlConnection(conn);
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(queryString, con);
                    cmd.ExecuteNonQuery();
                    getdropdownbind("");
                    lblerror.Text = "Delete Successfully";
                    //sshobj.SshDisconnect(client);
                }

                //StatusMessage("Information", "Delete Successfully");
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString() + "Try again later";
                //StatusMessage("Error", ex.ToString() + "Try again later");
            }
        }
        protected void GVProductDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //ConnectSsh sshobj = new ConnectSsh();
            //SshClient client = sshobj.SshConnect();
            GVProductDetails.EditIndex = e.NewEditIndex;
            getProductDetails();
            //sshobj.SshDisconnect(client);
        }

        protected void GVProductDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            //ConnectSsh sshobj = new ConnectSsh();
            //SshClient client = sshobj.SshConnect();
            GVProductDetails.EditIndex = -1;
            getProductDetails();
            //sshobj.SshDisconnect(client);
        }

        protected void GVProductDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = (GridViewRow)GVProductDetails.Rows[e.RowIndex];

                TextBox productname = (TextBox)row.FindControl("txtproductname");
                TextBox weight = (TextBox)row.FindControl("txtweight");

                string entity_id = ((HiddenField)row.FindControl("HFent_id")).Value.ToString();

                string Hfproductname = ((HiddenField)row.FindControl("HFproductname")).Value.ToString();
               
                string txtproductname = ((TextBox)row.FindControl("txtproductname")).Text.ToString();

                string Hfweight = ((HiddenField)row.FindControl("HFweight")).Value.ToString();
               
                string txtweight = ((TextBox)row.FindControl("txtweight")).Text.ToString();

                string packname = ((HiddenField)row.FindControl("HFpackname")).Value.ToString();

                if (txtproductname != "")
                {
                    if (txtweight != "")
                    {
                        if (Hfproductname != txtproductname)
                        {
                            queryString = @"select count(*) from 1_pack_item_ffhp_product where pack_name='" + packname + "' and name='" + txtproductname + "'";
                        }
                        else
                        {
                            queryString="";
                        }

                        if (Hfproductname != txtproductname || Hfweight != txtweight)
                        {
                            //ConnectSsh sshobj = new ConnectSsh();
                            //SshClient client = sshobj.SshConnect();

                            MySqlConnection con = new MySqlConnection(conn);
                            con.Open();
                            int productcount = 0;
                            if (queryString != "")
                            {
                                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                                DataSet countlist = new DataSet();
                                adapteradminmail.Fill(countlist, "countlist");
                                if (countlist.Tables["countlist"].Rows.Count > 0)
                                {

                                    productcount = Convert.ToInt32(countlist.Tables["countlist"].Rows[0][0].ToString());

                                }
                            }
                            if (productcount == 0)
                            {
                                queryString = @"UPDATE 1_pack_item_ffhp_product set name='" + txtproductname + "',weight='" + txtweight + "' where entity_id=" + entity_id;
                                if (queryString != "")
                                {
                                    //MySqlConnection con = new MySqlConnection(conn);
                                    //con.Open();
                                    MySqlCommand cmd = new MySqlCommand(queryString, con);
                                    cmd.ExecuteNonQuery();
                                    GVProductDetails.EditIndex = -1;
                                    getProductDetails();
                                }
                            }
                            else
                            {
                                lblerror.Text = "This pack (" + packname.ToString() + ") already have this product" + txtproductname.ToString();
                            }
                            //sshobj.SshDisconnect(client);
                        }
                        else
                        {
                            lblerror.Text = "Values must be different";
                            //StatusMessage("Error", "Directory name must be different");
                        }
                    }
                    else
                    {
                        lblerror.Text = "Enter the Weight";
                    }
                }
                else
                {
                    lblerror.Text = "Enter the Product Name";
                }
                
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString() + "Try again later";
                //StatusMessage("Error", ex.ToString() + "Try again later");
            }
        }
        protected void Btndelete_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = (GridViewRow)(((Control)sender).NamingContainer);
                string entity_id = ((HiddenField)row.FindControl("HFentity_id")).Value.ToString();

                queryString = @"DELETE FROM 1_pack_item_ffhp_product where entity_id=" + entity_id;
                if (queryString != "")
                {
                    //ConnectSsh sshobj = new ConnectSsh();
                    //SshClient client = sshobj.SshConnect();
                    MySqlConnection con = new MySqlConnection(conn);
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(queryString, con);
                    cmd.ExecuteNonQuery();
                    getProductDetails();
                    lblerror.Text = "Delete Successfully";
                    //sshobj.SshDisconnect(client);
                }

                //StatusMessage("Information", "Delete Successfully");
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString()+"Try again later";
                //StatusMessage("Error", ex.ToString() + "Try again later");
            }
        }
        protected void BtnAddnew_OnClick(object sender, EventArgs e)
        {
            clear();
            traddnew.Visible = true;
            if(ddlproductlist.SelectedIndex>0)
            {
                txtpackname.Text = ddlproductlist.SelectedValue.ToString();
                txtpackname.Enabled = false;
            }
            else
            {
                txtpackname.Text = "";
                txtpackname.Enabled = true;
            }
        }
        protected void BtnSave_OnClick(object sender, EventArgs e)
        {
            try
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                if (txtpackname.Text != "")
                {
                    if (txtproductname.Text != "")
                    {
                        if (txtweight.Text != "")
                        {
                            MySqlConnection con = new MySqlConnection(conn);
                            con.Open();

                            queryString = @"select count(*) from 1_pack_item_ffhp_product where pack_name='" + txtpackname.Text.ToString() + "' and name='" + txtproductname.Text.ToString() + "'";

                            if (queryString != "")
                            {
                                
                                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                                DataSet countlist = new DataSet();
                                adapteradminmail.Fill(countlist, "countlist");
                                if (countlist.Tables["countlist"].Rows.Count > 0)
                                {
                                    if (countlist.Tables["countlist"].Rows[0][0].ToString() == "0")
                                    {
                                        queryString = @"INSERT INTO 1_pack_item_ffhp_product(pack_name,name,weight) VALUES('" + txtpackname.Text.ToString() + "', '" + txtproductname.Text.ToString() + "'," + txtweight.Text.ToString() + ")";

                                        if (queryString != "")
                                        {
                                            //MySqlConnection con = new MySqlConnection(conn);
                                            //con.Open();
                                            MySqlCommand cmd = new MySqlCommand(queryString, con);
                                            cmd.ExecuteNonQuery();
                                            getdropdownbind("dropdown");
                                            //getProductDetails();
                                            clear();
                                            lblerror.Text = "Successfully Saved";
                                            GVPackDetails.Visible = false;
                                        }
                                        traddnew.Visible = false;
                                    }
                                    else
                                    {
                                        lblerror.Text = "This Product (" + txtproductname.Text.ToString() + ") is already exist";
                                    }
                                }
                            }
                            
                        }
                        else
                        {
                            lblerror.Text = "Enter the weight";
                        }
                    }
                    else
                    {
                        lblerror.Text = "Enter the Product Name";
                    }
                }
                else
                {
                    lblerror.Text = "Enter the Pack Name";
                }
                //sshobj.SshDisconnect(client);
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString() + "Try again later";
                //StatusMessage("Error", ex.ToString() + "Try again later");
            }
        }
        protected void BtnCancel_OnClick(object sender, EventArgs e)
        {
            traddnew.Visible = false;
        }
        public void clear()
        {
            txtpackname.Text = "";
            txtproductname.Text = "";
            txtweight.Text = "";
        }
    }
}
