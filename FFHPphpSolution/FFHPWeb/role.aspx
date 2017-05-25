<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true" CodeBehind="role.aspx.cs" Inherits="FFHPWeb.role" Theme="Skin1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<table width="100%">
<tr>
<td class="heading">Role Master</td>
</tr>
<tr>
<td>
<table>
<tr>
<td>Role Name</td>
<td><asp:TextBox ID="tbxrolename" runat="server"></asp:TextBox></td>
<td><asp:Button ID="btnadd" runat="server" Text="Add" OnClick="btnadd_OnClick" /></td>
</tr>
</table>
</td>
</tr>
<tr>
<td><asp:Label ID="lblerror" runat="server"></asp:Label></td>
</tr>
<tr>
<td>
<asp:GridView ID="gvrole" runat="server" AutoGenerateEditButton="true" OnRowEditing="gvrole_OnRowEditing" OnRowCancelingEdit="gvrole_OnRowCancelingEdit" OnRowUpdating="gvrole_OnRowUpdating">
<Columns>
<asp:TemplateField>
<ItemTemplate>
<asp:Label ID="lblrolename" runat="server" Text='<%#Bind("rolename") %>'></asp:Label>
</ItemTemplate>
<EditItemTemplate>
<asp:TextBox ID="tbxeditrolename" runat="server" Text='<%#Bind("rolename") %>'></asp:TextBox><asp:HiddenField ID="hfroleid" runat="server" Value='<%#Bind("roleid") %>' />
</EditItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</td>
</tr>
</table>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
