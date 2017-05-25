<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true" CodeBehind="vendor_products.aspx.cs" Inherits="FFHPWeb.vendor_products" Theme="Skin1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:UpdatePanel runat="server">
<ContentTemplate>
<table width="100%">
<tr>
<td class="heading">Assign Vendor Products</td>
</tr>
<tr>
<td>
<table>
<tr>
<td>Vendor Name</td>
<td><asp:DropDownList ID="ddlvendor" Width="100%" OnSelectedIndexChanged="ddlvendor_OnSelectedIndexChanged" DataTextField="vendorname" DataValueField="vendorid" AppendDataBoundItems="true" AutoPostBack="true" runat="server"></asp:DropDownList></td>
<td><a href="vendor_details.aspx">Add Vendor</a></td>
</tr>
<tr>
<td>Product Id</td>
<td><asp:DropDownList ID="ddlproduct" DataTextField="pidname" DataValueField="productid" AppendDataBoundItems="true" runat="server"></asp:DropDownList></td>
<td></td>
</tr>
<tr>
<td colspan="2" align="center"><asp:Button ID="btnadd" OnClick="btnadd_OnClick" runat="server" Text="ADD" /></td>
</tr>
</table>
</td>
</tr>
<tr>
<td><asp:Label ID="lblerror" runat="server"></asp:Label></td>
</tr>
<tr>
<td>
<asp:GridView ID="gvvendorproducts" OnRowDataBound="gvvendorproducts_OnRowDataBound" runat="server">
<Columns>
<asp:TemplateField HeaderText="Sno">                   
    <ItemTemplate>
    <%#Container.DataItemIndex + 1 %>
    </ItemTemplate>
    </asp:TemplateField>
<asp:BoundField DataField="productid" HeaderText="Product Id" />
<asp:BoundField DataField="name" HeaderText="Name" />
<asp:BoundField DataField="unit" HeaderText="Unit" />
<asp:TemplateField HeaderText="">
<ItemTemplate>
<asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_OnClick" /><asp:HiddenField ID="hfid" Value='<%#Bind("id") %>' runat="server" /><asp:HiddenField ID="hfstatus" Value='<%#Bind("status") %>' runat="server" />
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</td>
</tr>
</table>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
