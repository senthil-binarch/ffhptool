<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="FFHPWeb.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="Stylesheet1.css" type="text/css" rel="Stylesheet" />
    <link href="stylesheet.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table width="100%">
    <tr>
                <td colspan="2" class="heading" align="center" valign="middle" height="100px">
                    FFHP Support Tool
                </td>
            </tr>
<tr>
<td valign="middle" align="center">
<table frame="box">
<tr>
<td colspan="2" class="lblheading">Login</td>
</tr>
<tr>
<td>User Name</td>
<td><asp:TextBox ID="txtusername" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
        ErrorMessage="*" ControlToValidate="txtusername" ValidationGroup="cal"></asp:RequiredFieldValidator></td>
</tr>
<tr>
<td>Password</td>
<td><asp:TextBox ID="txtpassword" TextMode="Password" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
        ErrorMessage="*" ControlToValidate="txtpassword" ValidationGroup="cal"></asp:RequiredFieldValidator></td>
</tr>
<tr>
<td colspan="2" align="center">
<asp:Button ID="btnsubmit" OnClick="btnsubmit_OnClick" Text="Submit" runat="server" ValidationGroup="cal" />
</td>
</tr>
<tr>
<td colspan="2"><asp:Label ID="lblerror" runat="server"></asp:Label></td>
</tr>
</table>
</td>
</tr>
</table>
    </div>
    </form>
</body>
</html>
