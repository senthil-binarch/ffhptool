using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Net.Mail;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using System.Security.Cryptography;

namespace FFHPWeb
{
    public partial class user : System.Web.UI.Page
    {
        string conn = System.Configuration.ConfigurationManager.AppSettings["SqlConnectionString"].ToString();
        string queryString = "";
        MySqlDataAdapter DA;
        string s = "";
        bool t = false;
        string errormsg = "Try again";

        static readonly string PasswordHash = "P@@Sw0rd";
        static readonly string SaltKey = "S@LT&KEY";
        static readonly string VIKey = "@1B2c3D4e5F6g7H8";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindrole();
                binduser();
            }
            lblerror.Text = "";
        }
        public void bindrole()
        {
            try
            {
                DataTable dt = new DataTable();
                try
                {
                    SqlConnection con = new SqlConnection(conn);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_ffhprole", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("roleid", 0);
                    cmd.Parameters.AddWithValue("rolename", "");
                    cmd.Parameters.AddWithValue("rolestatus", "1");
                    cmd.Parameters.AddWithValue("operation", "select");
                    SqlParameter outputparam = new SqlParameter();
                    outputparam.ParameterName = "@output";
                    outputparam.DbType = DbType.Int32;
                    outputparam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outputparam);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                    con.Close();
                    ddlrole.DataSource = dt;
                    ddlrole.Items.Clear();
                    ddlrole.Items.Add("--Select--");
                    ddlrole.DataBind();
                    ViewState["role"] = dt;
                }
                catch (Exception ex)
                {
                    lblerror.Text = errormsg.ToString();
                }
            }
            catch (Exception ex)
            {
            }
        }
        protected void btncancel_OnClick(object sender, EventArgs e)
        {
            clear();
        }
        protected void btnsubmit_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (validation())
                {
                    SqlConnection con = new SqlConnection(conn);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_ffhpuser", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("userid", 0);
                    cmd.Parameters.AddWithValue("username", tbxusername.Text.ToString().Trim());
                    cmd.Parameters.AddWithValue("email", tbxemail.Text.ToString().Trim());
                    cmd.Parameters.AddWithValue("pwd", Encrypt(tbxpassword.Text.ToString().Trim()));
                    cmd.Parameters.AddWithValue("roleid", Convert.ToInt32(ddlrole.SelectedValue.ToString()));
                    if (cbstatus.Checked)
                    {
                        cmd.Parameters.AddWithValue("userstatus", "1");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("userstatus", "0");
                    }
                    cmd.Parameters.AddWithValue("operation", "insert");

                    SqlParameter outputparam = new SqlParameter();
                    outputparam.ParameterName = "@output";
                    outputparam.DbType = DbType.Int32;
                    outputparam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outputparam);

                    cmd.ExecuteNonQuery();
                    int i = Convert.ToInt32(cmd.Parameters["@output"].Value.ToString());
                    con.Close();
                    if (i > 0)
                    {
                        lblerror.Text = "Save Successfully";
                        clear();
                        binduser();
                    }
                    else
                    {
                        lblerror.Text = "The Role already exist.";
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

            if (tbxusername.Text != "")
            {
                if (tbxemail.Text != "")
                {
                    if (tbxpassword.Text != "")
                    {
                        t = true;
                    }
                    else
                    {
                        lblerror.Text = "Please enter password";
                    }
                }
                else
                {
                    lblerror.Text = "Please enter email";
                }
            }
            else
            {
                lblerror.Text = "Please enter user name";
            }

            return t;
        }
        public void binduser()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(conn);
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_ffhpuser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("userid", 0);
                cmd.Parameters.AddWithValue("username", "");
                cmd.Parameters.AddWithValue("email", "");
                cmd.Parameters.AddWithValue("pwd", "");
                cmd.Parameters.AddWithValue("roleid", 0);
                cmd.Parameters.AddWithValue("userstatus", "1");
                cmd.Parameters.AddWithValue("operation", "select");
                SqlParameter outputparam = new SqlParameter();
                outputparam.ParameterName = "@output";
                outputparam.DbType = DbType.Int32;
                outputparam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputparam);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
                gvuser.DataSource = dt;
                gvuser.DataBind();
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
        }
        protected void gvuser_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && gvuser.EditIndex == e.Row.RowIndex)
            {
                DropDownList ddleditrole = (DropDownList)e.Row.FindControl("ddleditrole");
                ddleditrole.DataSource = (DataTable)ViewState["role"];
                ddleditrole.Items.Clear();
                ddleditrole.DataTextField = "rolename";
                ddleditrole.DataValueField = "roleid";
                ddleditrole.DataBind();
                ddleditrole.Items.FindByText((e.Row.FindControl("hfrole") as HiddenField).Value).Selected = true;

                CheckBox cbeditstatus = (CheckBox)e.Row.FindControl("cbeditstatus");
                if ((e.Row.FindControl("hfstatus") as HiddenField).Value == "1")
                {
                    cbeditstatus.Checked = true;
                }
                else
                {
                    cbeditstatus.Checked = false;
                }

                //string s = (e.Row.FindControl("lblemail") as Label).Text.ToString();
                string oripwd = Decrypt((e.Row.FindControl("hfeditpassword") as HiddenField).Value.ToString());
                TextBox tbxeditpassword = (TextBox)e.Row.FindControl("tbxeditpassword");
                tbxeditpassword.Text = oripwd;
            }
        }
        protected void gvuser_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvuser.EditIndex = e.NewEditIndex;
            binduser();
        }
        protected void gvuser_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int userid = Convert.ToInt32(gvuser.DataKeys[e.RowIndex].Value.ToString());
            
            TextBox txtuser = (TextBox)gvuser.Rows[e.RowIndex].FindControl("tbxeditusername");
            TextBox txtpassword = (TextBox)gvuser.Rows[e.RowIndex].FindControl("tbxeditpassword");
            TextBox txtemail = (TextBox)gvuser.Rows[e.RowIndex].FindControl("tbxeditemail");
            DropDownList ddlrole = (DropDownList)gvuser.Rows[e.RowIndex].FindControl("ddleditrole");
            CheckBox cbstatus = (CheckBox)gvuser.Rows[e.RowIndex].FindControl("cbeditstatus");
            HiddenField currentpassword = (HiddenField)gvuser.Rows[e.RowIndex].FindControl("hfeditpassword");

            SqlConnection con = new SqlConnection(conn);
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_ffhpuser", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("userid", userid);
            cmd.Parameters.AddWithValue("username", txtuser.Text.ToString().Trim());
            cmd.Parameters.AddWithValue("email", txtemail.Text.ToString().Trim());
            if (txtpassword.Text != "")
            {
                cmd.Parameters.AddWithValue("pwd", Encrypt(txtpassword.Text.ToString().Trim()));
            }
            else
            {
                cmd.Parameters.AddWithValue("pwd", Encrypt(currentpassword.Value.ToString().Trim()));
            }
            cmd.Parameters.AddWithValue("roleid", Convert.ToInt32(ddlrole.SelectedValue.ToString()));
            if (cbstatus.Checked)
            {
                cmd.Parameters.AddWithValue("userstatus", "1");
            }
            else
            {
                cmd.Parameters.AddWithValue("userstatus", "0");
            }
            cmd.Parameters.AddWithValue("operation", "update");

            SqlParameter outputparam = new SqlParameter();
            outputparam.ParameterName = "@output";
            outputparam.DbType = DbType.Int32;
            outputparam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(outputparam);

            cmd.ExecuteNonQuery();
            int i = Convert.ToInt32(cmd.Parameters["@output"].Value.ToString());
            con.Close();
            if (i > 0)
            {
                lblerror.Text = "Updated Successfully";
                gvuser.EditIndex = -1;
                binduser();
            }
            else
            {
                lblerror.Text = "Not Updated Successfully";
            }

            lblerror.ForeColor = System.Drawing.Color.Green;
            lblerror.Text = "Updated successfully";
            gvuser.EditIndex = -1;
            binduser();
        }
        protected void gvuser_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvuser.EditIndex = -1;
            binduser();
        }
        protected void gvuser_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int userid = Convert.ToInt32(gvuser.DataKeys[e.RowIndex].Values["userid"].ToString());

            SqlConnection con = new SqlConnection(conn);
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_ffhpuser", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("userid", userid);
            cmd.Parameters.AddWithValue("username", "");
            cmd.Parameters.AddWithValue("email", "");
            cmd.Parameters.AddWithValue("pwd", "");
            cmd.Parameters.AddWithValue("roleid", 1);
            cmd.Parameters.AddWithValue("userstatus", "0");
            cmd.Parameters.AddWithValue("operation", "delete");

            SqlParameter outputparam = new SqlParameter();
            outputparam.ParameterName = "@output";
            outputparam.DbType = DbType.Int32;
            outputparam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(outputparam);

            cmd.ExecuteNonQuery();
            int i = Convert.ToInt32(cmd.Parameters["@output"].Value.ToString());
            con.Close();
            if (i > 0)
            {
                lblerror.Text = "Deleted Successfully";
                gvuser.EditIndex = -1;
                binduser();
            }
            else
            {
                lblerror.Text = "Not Deleted Successfully";
            }

            lblerror.ForeColor = System.Drawing.Color.Green;
            lblerror.Text = "Deleted successfully";
            gvuser.EditIndex = -1;
            binduser();
        }
        public void clear()
        {
            tbxusername.Text = "";
            tbxpassword.Text = "";
            ddlrole.SelectedIndex = -1;
            tbxemail.Text = "";
            cbstatus.Checked = false;
        }
        public string Encrypt(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }
        public static string Decrypt(string encryptedText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }
    }
}
