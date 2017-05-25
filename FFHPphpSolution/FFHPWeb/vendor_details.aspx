<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true" CodeBehind="vendor_details.aspx.cs" Inherits="FFHPWeb.vendor_details" Theme="Skin1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:UpdatePanel runat="server">
<ContentTemplate>
<table width="100%">
<tr>
<td class="heading">Vedor Details</td>
</tr>
<tr>
<td>
<table width="80%">
<tr>
<td width="20%">Vendor Name</td>
<td><asp:TextBox ID="tbxvendorname" runat="server" width="80%"></asp:TextBox></td>
</tr>
<tr>
<td width="20%">Telephone</td>
<td><asp:TextBox ID="tbxtelephone" runat="server" width="80%"></asp:TextBox></td>
</tr>
<tr>
<td colspan="2" align="center">
<table>
<tr>
<td><asp:Button ID="btnsave" OnClick="btnsave_OnClick" Text="Save" runat="server" /></td>
<td><asp:Button ID="btnclear" OnClick="btnclear_OnClick" Text="Clear" runat="server" /></td>
<td><asp:Button ID="btnback" OnClick="btnback_OnClick" Text="Back" runat="server" /></td>
</tr>
</table>
</td>
</tr>
</table>
</td>
</tr>
<tr>
<td>
<asp:Label ID="lblerror" runat="server"></asp:Label>
</td>
</tr>
<tr>
<td>
<asp:GridView ID="gvvendordetails" runat="server">
<Columns>
<asp:TemplateField HeaderText="Sno">                   
    <ItemTemplate>
    <%#Container.DataItemIndex + 1 %>
    </ItemTemplate>
    </asp:TemplateField>
<asp:BoundField DataField="vendorid" HeaderText="Vendor ID" />
<asp:BoundField DataField="vendorname" HeaderText="Vendor Name" />
<asp:BoundField DataField="telephone" HeaderText="Telephone" />
<asp:TemplateField>
<ItemTemplate>
<asp:Button ID="btnedit" Text="Edit" OnClick="btnedit_OnClick" runat="server" />
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView><asp:HiddenField ID="hfvendorid" runat="server" />
</td>
</tr>

</table>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
