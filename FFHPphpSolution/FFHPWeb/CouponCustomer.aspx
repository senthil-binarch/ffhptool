<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true" CodeBehind="CouponCustomer.aspx.cs" Inherits="FFHPWeb.CouponCustomer" Theme="Skin1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table>
<tr>
<td class="lblheading">Coupon Code Used Customers</td>
</tr>
<tr>
<td>Coupon Code:<asp:TextBox ID="txtcouponcode" runat="server"></asp:TextBox><asp:Button ID="btnsubmit" Text="Submit" runat="server" OnClick="btnsubmit_OnClick" /></td>
</tr>
<tr>
<td><asp:GridView ID="gvcouponcodecustomers" runat="server">
<Columns>
<asp:BoundField DataField="increment_id" HeaderText="Order Number" />
<asp:BoundField DataField="status" HeaderText="Status" />
<asp:BoundField DataField="coupon_code" HeaderText="Coupon Code" />
<asp:BoundField DataField="customer_id" HeaderText="Customer ID" />
<asp:BoundField DataField="ordered_date" HeaderText="Ordered Date" />
</Columns>
</asp:GridView></td>
</tr>
</table>
</asp:Content>
