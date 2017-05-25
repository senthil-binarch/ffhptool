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
using System.Globalization;
//using Renci.SshNet;
namespace FFHPWeb
{
    public partial class FreeUpdate : System.Web.UI.Page
    {
        string conn = System.Configuration.ConfigurationManager.AppSettings["Connection"].ToString();
        string queryString = "";
        MySqlDataAdapter DA;
        string errormsg = "Try again";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                BindFreeProduct();
                //sshobj.SshDisconnect(client);
            }
        }
        public void BindFreeProduct()
        {
            queryString = "SELECT * FROM `1_ffhp_free`";
            if (queryString != "")
            {
                MySqlDataAdapter adapteradminmail = new MySqlDataAdapter(queryString, conn);

                DataSet freeproduct = new DataSet();
                adapteradminmail.Fill(freeproduct, "sales_flat_order_free_item");
                gvfree.DataSource = freeproduct;
                gvfree.DataBind();
            }

        }
        protected void btnedit_OnClick(object sender, EventArgs e)
        {
            int rowIndex = ((sender as Button).NamingContainer as GridViewRow).RowIndex;
            hfproductid.Value = gvfree.Rows[rowIndex].Cells[0].Text;
            tbxname.Text = gvfree.Rows[rowIndex].Cells[1].Text;
            tbxweightpiece.Text = gvfree.Rows[rowIndex].Cells[2].Text;
            tbxkgpc.Text = gvfree.Rows[rowIndex].Cells[3].Text;
            tbxparentproductid.Text = gvfree.Rows[rowIndex].Cells[4].Text;
            tbl.Visible = true;
        }
        protected void btnupdate_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (hfproductid.Value != "" && hfproductid.Value != null)
                {
                    if (validation())
                    {
                        //ConnectSsh sshobj = new ConnectSsh();
                        //SshClient client = sshobj.SshConnect();
                        int i = 0;
                        queryString = "update `1_ffhp_free` set name='" + tbxname.Text.ToString().Trim() + "',weight='" + tbxweightpiece.Text.ToString().Trim() + "', units='" + tbxkgpc.Text.ToString().Trim() + "', ParentProductid='" + tbxparentproductid.Text.ToString().Trim() + "' where product_id=" + hfproductid.Value.ToString();
                        if (queryString != "")
                        {
                            MySqlConnection con = new MySqlConnection(conn);
                            con.Open();
                            MySqlCommand cmd = new MySqlCommand(queryString, con);
                            i = cmd.ExecuteNonQuery();
                        }
                        if (i > 0)
                        {
                            lblerror.Text = "Successfully Updated";
                            BindFreeProduct();
                        }
                        else
                        {
                            lblerror.Text = "Not Successfully Updated";
                        }
                        //sshobj.SshDisconnect(client);
                    }
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
        }
        public bool validation()
        {
            bool t = false;
            if (tbxname.Text != "")
            {
                if (tbxweightpiece.Text != "")
                {
                    if (tbxkgpc.Text != "")
                    {
                        if (tbxparentproductid.Text != "")
                        {
                            t = true;
                        }
                    }
                }
            }
            return t;
        }
    }
}
