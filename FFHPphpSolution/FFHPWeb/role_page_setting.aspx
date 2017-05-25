<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true" CodeBehind="role_page_setting.aspx.cs" Inherits="FFHPWeb.role_page_setting" Theme="Skin1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%">
<tr>
<td class="heading">Role Page Setting</td>
</tr>
<tr>
<td>
<table width="50%">
<tr>
<td>Role Name</td>
<td><asp:DropDownList ID="ddlrole" OnSelectedIndexChanged="ddlrole_OnSelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="rolename" DataValueField="roleid" AppendDataBoundItems="true"></asp:DropDownList></td>
</tr>
<tr>
<td colspan="2">
<asp:GridView ID="gvpages" runat="server" OnRowCancelingEdit="gvpages_RowCancelingEdit" OnRowEditing="gvpages_RowEditing"
OnRowUpdating="gvpages_RowUpdating" OnRowDataBound="gvpages_RowDataBound" >
<Columns>
<asp:TemplateField HeaderText="Page Name">
<ItemTemplate>
<asp:Label ID="lblpagename" Text='<%#Bind("pagename") %>' runat="server"></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Status">
<ItemTemplate>
<asp:Label ID="lblstatus" runat="server" Text='<%#Bind("status") %>'></asp:Label><asp:HiddenField ID="hfroleid" Value='<%#bind("roleid") %>' runat="server" />
</ItemTemplate>
<EditItemTemplate>
<asp:CheckBox ID="cbeditstatus" runat="server"  /><asp:HiddenField ID="hfeditroleid" Value='<%#bind("roleid") %>' runat="server" /><asp:HiddenField ID="hfpageid" Value='<%#bind("pageid") %>' runat="server" /><asp:HiddenField ID="hfrole_page_ref_id" Value='<%#bind("role_page_ref_id") %>' runat="server" /><asp:HiddenField ID="hfrefpageid" Value='<%#bind("refpageid") %>' runat="server" />
</EditItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<EditItemTemplate>
<asp:ImageButton ID="imgbtnUpdate" ValidationGroup="update" ImageUrl="~/Images/save.png" CommandName="Update" runat="server" ToolTip="Update"  />
<asp:ImageButton ID="imgbtnCancel" CausesValidation="false" ImageUrl="~/Images/cancel.png" runat="server" CommandName="Cancel" ToolTip="Cancel"  />
</EditItemTemplate>
<ItemTemplate>
<asp:ImageButton ID="imgbtnEdit" CausesValidation="false" ImageUrl="~/Images/edit.png" CommandName="Edit" runat="server" ToolTip="Edit"  />
<asp:ImageButton ID="imgbtnDelete" Visible="false" CausesValidation="false" CommandName="Delete" ImageUrl="~/Images/delete.png" runat="server" ToolTip="Delete"  />
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</td>
</tr>
</table>
</td>
</tr>
</table>
</asp:Content>
