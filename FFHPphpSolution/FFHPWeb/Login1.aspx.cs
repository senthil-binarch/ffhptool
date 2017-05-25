using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FFHPWeb
{
    public partial class Login1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("ffhpnew.aspx", false);
            Session["username"] = "admin";
        }
        protected void btnsubmit_OnClick(object sender, EventArgs e)
        {
            if (txtusername.Text != "")
            {
                if (txtpassword.Text != "")
                {
                    if (txtusername.Text == "admin" && txtpassword.Text == "admin123")
                    {
                        Session["orderid"] = "";
                        Response.Redirect("ffhpnew.aspx",false);
                        Session["username"] = "admin";
                    }
                }
            }
        }
    }
}
