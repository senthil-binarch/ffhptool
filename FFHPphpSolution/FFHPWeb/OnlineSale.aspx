<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true" CodeBehind="OnlineSale.aspx.cs" Inherits="FFHPWeb.OnlineSale" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%" align="center">
<tr>
<td class="heading">Online Sale</td>
</tr>
<tr>
<td>
<table width="100%">
<tr>
<td width="25%"><asp:FileUpload ID="FUstocksale" runat="server" /></td>
<td width="25%"><asp:Button ID="Btnupload" Text="Upload" runat="server" OnClick="Btnupload_OnClick" /></td>
<td align="right" width="50%"></td>
</tr>
</table>
</td>
</tr>
<tr>
<td><asp:Label ID="lblerror" runat="server"></asp:Label></td>
</tr>
</table>
</asp:Content>
