using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Renci.SshNet;
using System.Text;
namespace FFHPWeb
{
    
    public class ConnectSsh
    {
        public SshClient SshConnect()
        {
            
            
                SshClient client;
                string filepath = System.Web.Hosting.HostingEnvironment.MapPath("~/privatekey/opensshkeynew.ppk");
                var keyFile = new PrivateKeyFile(filepath);
                var keyFiles = new[] { keyFile };
                //var portForwarded = new ForwardedPortLocal("127.0.0.1", 3307, "127.0.0.1", 3306);

                client = new SshClient("www.farmfreshhandpicked.com", "farmfreshhandpicked", keyFiles); // establishing ssh connection to server where MySql is hosted
                //var portForwarded = new ForwardedPortLocal("127.0.0.1", 3306, "127.0.0.1", 3306);
                //portForwarded.RequestReceived += delegate(object sender, Renci.SshNet.Common.PortForwardEventArgs e)
                //{
                //    Console.WriteLine("Requested connection to " + e.OriginatorHost + ":" + e.OriginatorPort);
                //};
                client.Connect();
                if (client.IsConnected)
                {
                    string cou = client.ForwardedPorts.Count().ToString();
                    string s = client.ForwardedPorts.ToString();
                    var portForwarded = new ForwardedPortLocal("127.0.0.1", 3306, "127.0.0.1", 3306);
                    client.AddForwardedPort(portForwarded);
                    //client.SendKeepAlive();
                    //client.SendKeepAlive();
                    //portForwarded.Start();
                    if (portForwarded.IsStarted)
                    {
                    }
                    else
                    {
                        
                        portForwarded.Start();
                        //portForwarded.Stop();
                        //portForwarded.Start();
                        //portForwarded.Dispose();
                        //portForwarded.Start();
                    }
                }
            
            return client;
        }
        public void SshDisconnect(SshClient client)
        {
            if (client.IsConnected)
            {
                var portForwarded = new ForwardedPortLocal("127.0.0.1", 3306, "127.0.0.1", 3306);
                portForwarded.Stop();
                portForwarded.Dispose();
                client.Disconnect();
            }
        }
        //http://sshnet3.rssing.com/chan-6841276/all_p8.html
    }
}
