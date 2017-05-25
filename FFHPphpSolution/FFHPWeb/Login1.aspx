<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true" CodeBehind="Login1.aspx.cs" Inherits="FFHPWeb.Login1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%">
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
</table>
</td>
</tr>
</table>
</asp:Content>
