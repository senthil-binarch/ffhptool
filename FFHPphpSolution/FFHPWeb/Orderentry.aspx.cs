using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FFHPWeb
{
    public partial class Orderentry : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] != null && Session["username"].ToString() != "")
                {
                    txtidlist.Text = Session["orderid"].ToString();
                }
                else
                {
                    Response.Redirect("Login.aspx", false);
                }
            }
        }
        protected void btnsubmit1_OnClick(object sender, EventArgs e)
        {
            if (Session["username"] != null)
            {
                Session["orderid"] = txtidlist.Text.ToString();
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }
    }
}
