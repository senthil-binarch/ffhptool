<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true" CodeBehind="vendorpayment.aspx.cs" Inherits="FFHPWeb.vendorpayment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%">
<tr>
<td class="heading">Vendor Payment</td>
</tr>
<tr>
<td></td>
</tr>
<tr>
<td>
<table width="50%">
<tr>
<td width="20%">Vendor</td>
<td width="80%"><asp:DropDownList ID="ddlvendor" OnSelectedIndexChanged="ddlvendor_OnSelectedIndexChanged" DataTextField="vendorname" DataValueField="vendorid" AppendDataBoundItems="true" AutoPostBack="true" runat="server"></asp:DropDownList></td>
</tr>
</table>
</td>
</tr>
<tr>
<td><asp:Label ID="lblerror" runat="server"></asp:Label></td>
</tr>
<tr>
<td>
</td>
</tr>
</table>
</asp:Content>
