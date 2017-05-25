using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FFHPWeb
{
    public partial class Routewise_Cutlist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //lblerror.Text = "";
                if (Session["username"] != null && Session["username"].ToString() != "")
                {
                    if (!IsPostBack)
                    {
                        if (Session["username"] != null && Session["username"].ToString() != "")
                        {
                            bindroutelist();
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
        public void bindroutelist()
        {
            RouteOrder obj = new RouteOrder();
            GVRouteClear.DataSource=obj.getroutelistnew();
            GVRouteClear.DataBind();
        }
        protected void btncutlist_OnClick(object sender, EventArgs e)
        {

        }
    }
}
