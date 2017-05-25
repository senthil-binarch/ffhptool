<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true" CodeBehind="user.aspx.cs" Inherits="FFHPWeb.user" Theme="Skin1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<table width="100%">
<tr>
<td class="heading">User Master</td>
</tr>
<tr>
<td>
<table>
<tr>
<td>User Name</td>
<td><asp:TextBox ID="tbxusername" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="rfvusername" ControlToValidate="tbxusername" ErrorMessage="*" runat="server"></asp:RequiredFieldValidator></td>
<td></td>
</tr>
<tr>
<td>Password</td>
<td><asp:TextBox ID="tbxpassword" runat="server" TextMode="Password"></asp:TextBox><asp:RequiredFieldValidator ID="rfvpassword" ControlToValidate="tbxpassword" ErrorMessage="*" runat="server"></asp:RequiredFieldValidator></td>
<td></td>
</tr>
<tr>
<td>Role</td>
<td><asp:DropDownList ID="ddlrole" runat="server" DataTextField="rolename" DataValueField="roleid" AppendDataBoundItems="true"></asp:DropDownList><asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="ddlrole" InitialValue="--Select--" ErrorMessage="*" runat="server"></asp:RequiredFieldValidator></td>
<td></td>
</tr>
<tr>
<td>Email</td>
<td><asp:TextBox ID="tbxemail" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="rfvemail" ControlToValidate="tbxemail" ErrorMessage="*" runat="server"></asp:RequiredFieldValidator></td>
<td></td>
</tr>
<tr>
<td>Status</td>
<td><asp:CheckBox ID="cbstatus" runat="server" /></td>
<td></td>
</tr>
<tr>
<td colspan="3">
<table>
<tr>
<td><asp:Button ID="btnsubmit" runat="server" Text="Submit" OnClick="btnsubmit_OnClick" /></td>
<td><asp:Button ID="btncancel" CausesValidation="false" runat="server" Text="Cancel" OnClick="btncancel_OnClick" /></td>
</tr>
</table>
</td>
</tr>
</table>
</td>
</tr>
<tr>
<td><asp:Label ID="lblerror" runat="server"></asp:Label></td>
</tr>
<tr>
<td>
<asp:GridView ID="gvuser" runat="server" DataKeyNames="userid"  OnRowCancelingEdit="gvuser_RowCancelingEdit"
OnRowDeleting="gvuser_RowDeleting" OnRowEditing="gvuser_RowEditing"
OnRowUpdating="gvuser_RowUpdating" OnRowDataBound="gvuser_RowDataBound">
<Columns>
<asp:TemplateField HeaderText="User Name">
<ItemTemplate>
<asp:Label ID="lblusername" Text='<%#Bind("username") %>' runat="server"></asp:Label>
</ItemTemplate>
<EditItemTemplate>
<asp:TextBox ID="tbxeditusername" Text='<%#Bind("username") %>' runat="server" ValidationGroup="update"></asp:TextBox><asp:RequiredFieldValidator ID="rfvusername" ValidationGroup="update" ControlToValidate="tbxeditusername" ErrorMessage="*" runat="server"></asp:RequiredFieldValidator>
</EditItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Password">
<ItemTemplate>
<asp:Label ID="lblpassword" Text="*****" runat="server"></asp:Label>
</ItemTemplate>
<EditItemTemplate>
<asp:TextBox ID="tbxeditpassword" Text='<%#Bind("pwd") %>' TextMode="Password" runat="server"></asp:TextBox>
<asp:HiddenField ID="hfeditpassword" Value='<%#Bind("pwd") %>' runat="server"></asp:HiddenField>
</EditItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Email">
<ItemTemplate>
<asp:Label ID="lblemail" Text='<%#Bind("email") %>' runat="server"></asp:Label>
</ItemTemplate>
<EditItemTemplate>
<asp:TextBox ID="tbxeditemail" ValidationGroup="update" Text='<%#Bind("email") %>' runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="rfvemail" ValidationGroup="update" ControlToValidate="tbxeditemail" ErrorMessage="*" runat="server"></asp:RequiredFieldValidator>
</EditItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Role">
<ItemTemplate>
<asp:Label ID="lblrole" Text='<%#Bind("rolename") %>' runat="server"></asp:Label>
</ItemTemplate>
<EditItemTemplate>
<asp:DropDownList ID="ddleditrole" ValidationGroup="update" DataTextField="rolename" DataValueField="roleid" runat="server"></asp:DropDownList><asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="update" ControlToValidate="ddleditrole" InitialValue="--Select--" ErrorMessage="*" runat="server"></asp:RequiredFieldValidator><asp:HiddenField ID="hfrole" Value='<%#Bind("rolename") %>' runat="server"></asp:HiddenField>
</EditItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Status">
<ItemTemplate>
<asp:Label ID="lblstatus" Text='<%#Bind("status") %>' runat="server"></asp:Label>
</ItemTemplate>
<EditItemTemplate>
<asp:CheckBox ID="cbeditstatus" Text="Status" runat="server" /><asp:HiddenField ID="hfstatus" Value='<%#Bind("userstatus") %>' runat="server"></asp:HiddenField>
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
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
