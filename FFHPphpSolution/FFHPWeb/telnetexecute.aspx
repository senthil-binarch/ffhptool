<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="telnetexecute.aspx.cs" Inherits="FFHPWeb.telnetexecute" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

       <center>

           <div>

         <table>

             <tr><td><h1>PowerShell Test</h1></td></tr>

            <tr><td><h3>PowerShell Code</h3></td></tr>



              <tr><td>

                   <asp:TextBox ID="PowerShellCodeBox" runat="server" TextMode="MultiLine" Width=700 Height=100></asp:TextBox>

              </td></tr>

    

            <tr><td>

                   <asp:Button ID="ExecuteCode" runat="server" Text="Execute" Width=200 onclick="ExecuteCode_Click" 

                     />

         </td></tr>

  

          <tr><td><h3>Result</h3></td></tr>

    
            <tr><td>
              <asp:TextBox ID="ResultBox" TextMode="MultiLine" Width=700 Height=200 runat="server"></asp:TextBox>
           </td></tr>

           </table>

          </div>

       </center>

    </form>
</body>
</html>
