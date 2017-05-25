using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
//using Renci.SshNet;
namespace FFHPWeb
{
    public partial class partner : System.Web.UI.Page
    {
        string sqlcon = System.Configuration.ConfigurationManager.AppSettings["PartnerSqlConnectionString"].ToString();
        string conn = System.Configuration.ConfigurationManager.AppSettings["Connection"].ToString();
        string queryString = "";
        MySqlDataAdapter DA;
        string s = "";
        bool t = false;
        string errormsg = "Try again";
        string alreadyexistcouponcode = "";

        static readonly string PasswordHash = "P@@Sw0rd";
        static readonly string SaltKey = "S@LT&KEY";
        static readonly string VIKey = "@1B2c3D4e5F6g7H8";

        protected void Page_Load(object sender, EventArgs e)
        {
            string password = CreatePassword(5);
            if (!IsPostBack)
            {
                //ConnectSsh sshobj = new ConnectSsh();
                //SshClient client = sshobj.SshConnect();
                bindcoupon();
                bind_customer_group();
                //sshobj.SshDisconnect(client);
                bindpartner();
            }
            lblerror.Text = "";
        }
        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
        private void bindcoupon()
        {
            queryString = @"SELECT * FROM `salesrule_coupon`";

                //Response.Write(queryString);
            if (queryString != "")
            {
                MySqlConnection con = new MySqlConnection(conn);
                con.Open();

                MySqlDataAdapter adaptercoupon = new MySqlDataAdapter(queryString, conn);

                DataSet dtcouponlist = new DataSet();
                adaptercoupon.Fill(dtcouponlist, "coupon");

                DataTable dt = new DataTable();
                dt = get_assigned_couponcode();

                DataTable dtResult = new DataTable();
                //dtResult.Columns.Add("coupon_id", typeof(Int32));
                dtResult.Columns.Add("coupon_code", typeof(string));
                var items = (from p in dtcouponlist.Tables[0].AsEnumerable()
                             join t in dt.AsEnumerable()
                             on p.Field<string>("code") equals t.Field<string>("coupon_code")
                             where p.Field<string>("code") != t.Field<string>("coupon_code")
                             select dtResult.LoadDataRow(new object[]
                                {
                                //p.Field<Int32>("coupon_id"),
                                p.Field<string>("code")
                                }, false));

                checklist.DataSource = dtcouponlist;
                checklist.DataBind();
                con.Close();
            }
        }
        private void bindpartner()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(sqlcon);
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_ffhp_partner", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("ffhp_partner_id", 0);
                cmd.Parameters.AddWithValue("name", "");
                cmd.Parameters.AddWithValue("phone", "");
                cmd.Parameters.AddWithValue("email", "");
                cmd.Parameters.AddWithValue("password", "");
                cmd.Parameters.AddWithValue("address", "");
                cmd.Parameters.AddWithValue("operation", "select");
                SqlParameter outputparam = new SqlParameter();
                outputparam.ParameterName = "@output";
                outputparam.DbType = DbType.Int32;
                outputparam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputparam);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
                ViewState["partnerdetails"] = dt;
                var distinctValues = dt.AsEnumerable()
                        .Select(row => new
                        {
                            ffhp_partner_id= row.Field<int>("ffhp_partner_id"),
                            name = row.Field<string>("name"),
                            phone = row.Field<string>("phone"),
                            email = row.Field<string>("email"),
                            address = row.Field<string>("address")
                        })
                        .Distinct();
                gvpartner.DataSource = distinctValues;
                gvpartner.DataBind();

            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
        }
        private void bind_customer_group()
        {
            queryString = @"SELECT * FROM `customer_group`";

            //Response.Write(queryString);
            if (queryString != "")
            {
                MySqlConnection con = new MySqlConnection(conn);
                con.Open();

                MySqlDataAdapter adaptercoupon = new MySqlDataAdapter(queryString, conn);

                DataSet dtgrouplist = new DataSet();
                adaptercoupon.Fill(dtgrouplist, "group");

                checklistgroup.DataSource = dtgrouplist;
                checklistgroup.DataBind();
                con.Close();
            }
        }
        protected void gvpartner_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblcouponcode = (Label)e.Row.FindControl("lblcouponcode");
                HiddenField hfffhp_partner_id = (HiddenField)e.Row.FindControl("hfffhp_partner_id");
                string couponcode = "";
                DataTable dt = (DataTable)ViewState["partnerdetails"];
                var resultcouponcode = (from DataRow dRow in dt.Rows
                                    where dRow["ffhp_partner_id"].ToString() == hfffhp_partner_id.Value.ToString()
                                    select new { col1 = dRow["coupon_code"] });
                if (resultcouponcode.Any())
                {
                    foreach (var item in resultcouponcode)
                    {
                        if (couponcode != "")
                        {
                            couponcode = couponcode + "," + item.col1.ToString();
                        }
                        else
                        {
                            couponcode = item.col1.ToString();
                        }
                        
                    }
                }
                lblcouponcode.Text = couponcode;
            }
        }
        private void clear()
        {
            tbxname.Text = "";
            tbxphone.Text = "";
            tbxemail.Text = "";
            tbxaddress.Text = "";
            checklist.ClearSelection();
            hfffhp_partner_id.Value = "";
            btnsave.Text = "Save";

        }
        protected void btnsave_OnClick(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(sqlcon);
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_ffhp_partner", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("ffhp_partner_id", 0);
                cmd.Parameters.AddWithValue("name", tbxname.Text.ToString().Trim());
                cmd.Parameters.AddWithValue("phone", tbxphone.Text.ToString().Trim());
                cmd.Parameters.AddWithValue("email", tbxemail.Text.ToString().Trim());
                if (btnsave.Text == "Save")
                {
                    cmd.Parameters.AddWithValue("password", Encrypt(CreatePassword(5)));
                }
                else if (btnsave.Text == "Update")
                {
                    cmd.Parameters.AddWithValue("password", "");
                }
                cmd.Parameters.AddWithValue("address", tbxaddress.Text.ToString().Trim());
                if (btnsave.Text == "Save")
                {
                    cmd.Parameters.AddWithValue("operation", "insert");
                }
                else if (btnsave.Text == "Update")
                {
                    cmd.Parameters.AddWithValue("operation", "update");
                }
                SqlParameter outputparam = new SqlParameter();
                outputparam.ParameterName = "@output";
                outputparam.DbType = DbType.Int32;
                outputparam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputparam);
                cmd.ExecuteNonQuery();
                int i = Convert.ToInt32(cmd.Parameters["@output"].Value.ToString());
                con.Close();
                if (btnsave.Text == "Save")
                {
                    if (i > 0)
                    {
                        lblerror.Text = "Save Successfully";
                        string couponcode = getcouponcode();
                        if (couponcode != "")
                        {
                            deletecouponcode(i);
                            insertcouponcode(i, couponcode);
                        }
                        bindpartner();
                        clear();
                    }
                    else
                    {
                        lblerror.Text = "The Partner already exist.";
                    }
                }
                else if (btnsave.Text == "Update")
                {
                    if (hfffhp_partner_id.Value.ToString()!="")
                    {
                        lblerror.Text = "Update Successfully";
                        string couponcode = getcouponcode();
                        if (couponcode != "")
                        {
                            deletecouponcode(Convert.ToInt32(hfffhp_partner_id.Value.ToString()));
                            insertcouponcode(Convert.ToInt32(hfffhp_partner_id.Value.ToString()), couponcode);
                        }
                        bindpartner();
                        clear();
                    }
                    else
                    {
                        lblerror.Text = errormsg;
                    }
                }
                if (alreadyexistcouponcode != "")
                {
                    lblerror.Text = alreadyexistcouponcode + " this coupon code already assign to another partner.";
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }

            
        }
        private void insertcouponcode(int partnerid,string couponcode)
        {
            try
            {
                
                List<string> lstarrcouponcode = couponcode.Split(',').Reverse().ToList();
                if (lstarrcouponcode.Count > 0)
                {
                    for (int c = 0; c < lstarrcouponcode.Count; c++)
                    {
                        SqlConnection con = new SqlConnection(sqlcon);
                        con.Open();
                        SqlCommand cmd = new SqlCommand("sp_ffhp_partner_coupon", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("ffhp_partner_coupon_id", 0);
                        cmd.Parameters.AddWithValue("ffhp_partner_id", partnerid);
                        cmd.Parameters.AddWithValue("coupon_code", lstarrcouponcode[c].ToString());
                        cmd.Parameters.AddWithValue("operation", "insert");
                        SqlParameter outputparam = new SqlParameter();
                        outputparam.ParameterName = "@output";
                        outputparam.DbType = DbType.Int32;
                        outputparam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(outputparam);
                        cmd.ExecuteNonQuery();
                        int i = Convert.ToInt32(cmd.Parameters["@output"].Value.ToString());
                        con.Close();
                        if (i == 0)
                        {
                            if(alreadyexistcouponcode=="")
                            {
                                alreadyexistcouponcode = lstarrcouponcode[c].ToString();
                            }
                            else
                            {
                                alreadyexistcouponcode = alreadyexistcouponcode+","+lstarrcouponcode[c].ToString();
                            }
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
        }
        private void deletecouponcode(int partnerid)
        {
            try
            {

                SqlConnection con = new SqlConnection(sqlcon);
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_ffhp_partner_coupon", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("ffhp_partner_coupon_id", 0);
                cmd.Parameters.AddWithValue("ffhp_partner_id", partnerid);
                cmd.Parameters.AddWithValue("coupon_code", "");
                cmd.Parameters.AddWithValue("operation", "delete");
                SqlParameter outputparam = new SqlParameter();
                outputparam.ParameterName = "@output";
                outputparam.DbType = DbType.Int32;
                outputparam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputparam);
                cmd.ExecuteNonQuery();
                con.Close();

            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
        }
        private string getcouponcode()
        {
            string couponcode = "";
            if (checklist.Items.Count > 0)
            {
                for (int i = 0; i < checklist.Items.Count; i++)
                {
                    if (checklist.Items[i].Selected)
                    {
                        if (couponcode != "")
                        {
                            couponcode = couponcode + "," + checklist.Items[i].Text.ToString();
                        }
                        else
                        {
                            couponcode = checklist.Items[i].Text.ToString();
                        }
                    }
                }
            }
            return couponcode;
        }
        private void setcouponcode(string couponcode)
        {
            checklist.ClearSelection();
            
            List<string> lstarrcouponcode = couponcode.Split(',').ToList();
            if (lstarrcouponcode.Count > 0)
            {
                for (int c = 0; c < lstarrcouponcode.Count; c++)
                {
                    lstarrcouponcode[c].ToString();
                    if (checklist.Items.Count > 0)
                    {
                        for (int i = 0; i < checklist.Items.Count; i++)
                        {
                            if (checklist.Items[i].Text.ToString() == lstarrcouponcode[c].ToString())
                            {
                                checklist.Items[i].Selected = true;
                                //checklist.Items[i].Attributes.Clear();
                                checklist.Items[i].Attributes.Add("style", "background-color:gold;");
                            }
                        }
                    }
                }
            }
        }
        private string getgroupcode()
        {
            string groupcode = "";
            if (checklistgroup.Items.Count > 0)
            {
                for (int i = 0; i < checklistgroup.Items.Count; i++)
                {
                    if (checklistgroup.Items[i].Selected)
                    {
                        if (groupcode != "")
                        {
                            groupcode = groupcode + "," + checklistgroup.Items[i].Text.ToString();
                        }
                        else
                        {
                            groupcode = checklistgroup.Items[i].Text.ToString();
                        }
                    }
                }
            }
            return groupcode;
        }
        private void setgroupcode(string groupcode)
        {
            checklistgroup.ClearSelection();

            List<string> lstarrgroupcode = groupcode.Split(',').ToList();
            if (lstarrgroupcode.Count > 0)
            {
                for (int c = 0; c < lstarrgroupcode.Count; c++)
                {
                    lstarrgroupcode[c].ToString();
                    if (checklistgroup.Items.Count > 0)
                    {
                        for (int i = 0; i < checklistgroup.Items.Count; i++)
                        {
                            if (checklistgroup.Items[i].Text.ToString() == lstarrgroupcode[c].ToString())
                            {
                                checklistgroup.Items[i].Selected = true;
                                //checklist.Items[i].Attributes.Clear();
                                checklistgroup.Items[i].Attributes.Add("style", "background-color:gold;");
                            }
                        }
                    }
                }
            }
        }
        protected void btnedit_OnClick(object sender, EventArgs e)
        {
            GridViewRow row = (GridViewRow)(((Control)sender).NamingContainer);
            Label lblcouponcode = (Label)row.FindControl("lblcouponcode");
            HiddenField hfeditffhp_partner_id = (HiddenField)row.FindControl("hfffhp_partner_id");

            hfffhp_partner_id.Value = hfeditffhp_partner_id.Value.ToString();

            tbxname.Text = row.Cells[0].Text.ToString();
            tbxphone.Text = row.Cells[1].Text.ToString();
            tbxemail.Text = row.Cells[2].Text.ToString();
            tbxaddress.Text = row.Cells[3].Text.ToString();
            if (lblcouponcode.Text != "")
            {
                setcouponcode(lblcouponcode.Text.ToString());
            }
            else
            {
                checklist.ClearSelection();
            }
            btnsave.Text = "Update";
        }
        protected void btnclear_OnClick(object sender, EventArgs e)
        {
            clear();
        }
        private DataTable get_assigned_couponcode()
        {
            DataTable dt = new DataTable();
            try
            {

                SqlConnection con = new SqlConnection(sqlcon);
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_ffhp_partner_coupon", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("ffhp_partner_coupon_id", 0);
                cmd.Parameters.AddWithValue("ffhp_partner_id", 0);
                cmd.Parameters.AddWithValue("coupon_code", "");
                cmd.Parameters.AddWithValue("operation", "select_assigned_coupon");
                SqlParameter outputparam = new SqlParameter();
                outputparam.ParameterName = "@output";
                outputparam.DbType = DbType.Int32;
                outputparam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputparam);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
                ViewState["assigned_couponcode"] = dt;
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
            return dt;
        }
        private DataTable get_assigned_couponcode_without_partner(int partnerid)
        {
            DataTable dt = new DataTable();
            try
            {

                SqlConnection con = new SqlConnection(sqlcon);
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_ffhp_partner_coupon", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("ffhp_partner_coupon_id", 0);
                cmd.Parameters.AddWithValue("ffhp_partner_id", partnerid);
                cmd.Parameters.AddWithValue("coupon_code", "");
                cmd.Parameters.AddWithValue("operation", "select_assigned_coupon_without_partner");
                SqlParameter outputparam = new SqlParameter();
                outputparam.ParameterName = "@output";
                outputparam.DbType = DbType.Int32;
                outputparam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputparam);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
                ViewState["assigned_couponcode_without_partner"] = dt;
            }
            catch (Exception ex)
            {
                lblerror.Text = errormsg.ToString();
            }
            return dt;
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
