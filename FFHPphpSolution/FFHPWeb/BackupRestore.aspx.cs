using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

using System.Diagnostics;

namespace FFHPWeb
{
    public partial class BackupRestore : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Backup();
        }
        //private void BackupDatabase()
        //{
        //    string time = DateTime.Now.ToString("dd-MM-yyyy");
        //    //string savePath = AppDomain.CurrentDomain.BaseDirectory + @"Backups\" + time + "_" + saveFileDialogBackUp.FileName;
        //    string savePath = @"D:\Senthil\FFHP\backupmysql.sql";
        //    //if (saveFileDialogBackUp.ShowDialog() == DialogResult.OK)
        //    //{
        //        try
        //        {
        //            using (Process mySqlDump = new Process())
        //            {
        //                mySqlDump.StartInfo.FileName = @"D:\Senthil\FFHP\mysqldump.exe";
        //                mySqlDump.StartInfo.UseShellExecute = false;
        //                mySqlDump.StartInfo.Arguments = @"-u" + "admin" + " -p" + "admin" + " -h" + "192.168.1.2" + " " + "ffhp" + " -r \"" + savePath + "\"";
        //                mySqlDump.StartInfo.RedirectStandardInput = false;
        //                mySqlDump.StartInfo.RedirectStandardOutput = false;
        //                mySqlDump.StartInfo.CreateNoWindow = true;
        //                mySqlDump.Start();
        //                mySqlDump.WaitForExit();
        //                mySqlDump.Close();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            //MessageBox.Show("Connot backup database! \n\n" + ex);
        //        }
        //        //MessageBox.Show("Done! database backuped!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    //}
        //}
        //public void DumpIt(MySqlConnection myConnection)
        //{
        //    myConnection.Open();
        //    MySqlDump mySqlDump = new MySqlDump();
        //    mySqlDump.Connection = myConnection;
        //    myConnection.Database = "Test";
        //    mySqlDump.IncludeDrop = true;
        //    mySqlDump.GenerateHeader = true;
        //    mySqlDump.Tables = "Dept;Emp";
        //    mySqlDump.Backup();
        //    StreamWriter stream = new StreamWriter("d:\\tmp\\mysqldump.dmp");
        //    stream.WriteLine(mySqlDump.DumpText);
        //    stream.Close();
        //    Console.WriteLine("Dumped.");
        //    myConnection.Close();
        //} 
        public void Backup()
        {
            string constring = "server=68.178.143.107;userid=stagingffhpin;password=Stag1ngffHP!n;database=stagingffhpin;Connect Timeout=6000; pooling='true'; Max Pool Size=6000;Convert Zero Datetime=True";
            //string constring = "server=192.168.1.2;userid=admin;password=admin;database=ffhp;Convert Zero Datetime=True";
            string file = @"D:\Senthil\FFHP\backupffhpstaging.sql";
            using (MySqlConnection conn = new MySqlConnection(constring))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        cmd.CommandTimeout = 6000;
                        conn.Open();
                        mb.ExportToFile(file);
                        conn.Close();
                    }
                }
            }
        }
        public void Restore()
        {
            string constring = "server=localhost;user=root;pwd=qwerty;database=test;";
            string file = "C:\\backup.sql";
            using (MySqlConnection conn = new MySqlConnection(constring))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ImportFromFile(file);
                        conn.Close();
                    }
                }
            }
        }
    }
}
