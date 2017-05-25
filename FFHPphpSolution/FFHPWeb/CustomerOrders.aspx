<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true" CodeBehind="CustomerOrders.aspx.cs" Inherits="FFHPWeb.CustomerOrders" Theme="Skin1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table>
<tr>
<td class="lblheading">Customer Orders</td>
</tr>
<tr>
<td>customer Id:<asp:TextBox ID="txtcustomerid" runat="server"></asp:TextBox><asp:Button ID="btnsubmit" Text="Submit" runat="server" OnClick="btnsubmit_OnClick" /></td>
</tr>
<tr>
<td>
<asp:GridView ID="gvcustomerlist" runat="server">
<Columns>
<asp:BoundField DataField="billing_name" HeaderText="Name" />
<asp:BoundField DataField="increment_id" HeaderText="Order Number" />
<asp:BoundField DataField="ordered_date" HeaderText="Ordered Date" />
<asp:BoundField DataField="status" HeaderText="Status" />
</Columns>
</asp:GridView>
</td>
</tr>
</table>
</asp:Content>
