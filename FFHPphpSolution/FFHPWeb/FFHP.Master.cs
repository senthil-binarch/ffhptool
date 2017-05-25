using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using AjaxControlToolkit;
namespace FFHPWeb
{
    public partial class FFHP : System.Web.UI.MasterPage
    {
        string conn = System.Configuration.ConfigurationManager.AppSettings["SqlConnectionString"].ToString();
        string queryString = "";
        string s = "";
        bool t = false;
        string errormsg = "Try again";

        string username = System.Configuration.ConfigurationManager.AppSettings["websiteusername"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["menu"] != null)
            {
                bindrolemenupages();
            }
            if (Session["username"] != null)
            {
                DataTable dt = (DataTable)Session["menu"];
                DataTable dtresult = new DataTable();
                dtresult = dt.AsEnumerable().Where(r => this.Page.Request.Url.AbsolutePath.ToString().Contains(Convert.ToString(r["pagepath"]))).Where(r => Convert.ToString(r["pagepath"]) != "").AsDataView().ToTable();
                //&& r => Convert.ToString(r["pagepath"])!=""
                if (dtresult.Rows.Count > 0)
                {
                    //if (Session["username"].ToString() == username)
                    //{
                    lblusername.Text = Session["username"].ToString();
                    string s = this.Page.Request.FilePath;
                    if (s.Contains("Login.aspx") || s.Contains("Logout.aspx") || s.Contains("/Login.aspx"))
                    {
                        //M1.Attributes.Add("class", "vercurrent");
                        tdmenu.Visible = false;
                    }
                    else
                    {
                        tdmenu.Visible = true;
                    }
                    //}
                    //else
                    //{
                    //    Response.Redirect("Login.aspx", false);
                    //}
                }
                else
                {
                    Response.Redirect("Logout.aspx", false);
                }
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
            
        }
        public void bindrolemenupages()
        {
            try
            {
                DataTable dt = new DataTable();
                try
                {
                    //SqlConnection con = new SqlConnection(conn);
                    //con.Open();
                    //SqlCommand cmd = new SqlCommand("Sp_role_page_ref", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("role_page_ref_id", 0);
                    //cmd.Parameters.AddWithValue("roleid", Convert.ToInt32(Session["roleid"].ToString()));
                    //cmd.Parameters.AddWithValue("pageid", 0);
                    //cmd.Parameters.AddWithValue("operation", "selectpages");
                    //SqlParameter outputparam = new SqlParameter();
                    //outputparam.ParameterName = "@output";
                    //outputparam.DbType = DbType.Int32;
                    //outputparam.Direction = ParameterDirection.Output;
                    //cmd.Parameters.Add(outputparam);
                    //SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    //sda.Fill(dt);
                    //con.Close();

                    //ViewState["menu"] = dt;
                    dt = (DataTable)Session["menu"];

                    DataTable dtresult=new DataTable();
                    dtresult=dt.AsEnumerable().Where(r => Convert.ToString(r["parentpageid"]) == "").AsDataView().ToTable();
                    
                    //AccordionMenu.DataSource = (SiteMapDataSource1.GetView("") as SiteMapDataSourceView).Select(DataSourceSelectArguments.Empty);
                    AccordionMenu.DataSource = dtresult.DefaultView;
                    AccordionMenu.DataBind();
                }
                catch (Exception ex)
                {
                    //lblerror.Text = errormsg.ToString();
                }
            }
            catch (Exception ex)
            {
            }
        }
        protected void OnItemDataBound(object sender, AccordionItemEventArgs e)
        {
            //if (e.ItemType == AccordionItemType.Header)
            //{
            //    string menuText = (e.AccordionItem.FindControl("lnkMenu") as HyperLink).Text;
            //    if (menuText == SiteMap.CurrentNode.Title || menuText == SiteMap.CurrentNode.ParentNode.Title)
            //    {
            //        AccordionMenu.SelectedIndex = e.ItemIndex;
            //    }
            //}
            //if (e.ItemType == AccordionItemType.Content)
            //{
            //    AccordionContentPanel cPanel = e.AccordionItem;
            //    Repeater rptMenu = (Repeater)cPanel.FindControl("rptMenu");
            //    rptMenu.DataSource = (e.AccordionItem.DataItem as SiteMapNode).ChildNodes;
            //    rptMenu.DataBind();
            //}
            DataTable dt = (DataTable)Session["menu"];
            if (e.ItemType == AccordionItemType.Content)
            {
                AccordionContentPanel cPanel = e.AccordionItem;
                Repeater rptMenu = (Repeater)cPanel.FindControl("rptMenu");
                HiddenField pageid = (HiddenField)cPanel.FindControl("hfpageid");
                //string parentpageid = ((HiddenField)e.AccordionItem.FindControl("hfpageid")).Value;
                if (pageid.Value.ToString() != "")
                {
                    DataTable dtresult = new DataTable();
                    dtresult = dt.AsEnumerable().Where(r => Convert.ToString(r["parentpageid"]) == pageid.Value.ToString()).AsDataView().ToTable();
                    if (dtresult.Rows.Count > 0)
                    {
                        //rptMenu.DataSource = (e.AccordionItem.DataItem as SiteMapNode).ChildNodes;
                        rptMenu.DataSource = dtresult.DefaultView;
                        rptMenu.DataBind();
                    }
                }
                
            }
        }
        
    }
}
