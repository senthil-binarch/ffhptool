<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true" CodeBehind="SmsTemplate.aspx.cs" Inherits="FFHPWeb.SmsTemplate" Theme="Skin1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%">
<tr>
<td>

<table>
<%--<tr>
<td>Sms Type : <asp:DropDownList ID="ddlsmstype" AutoPostBack="true" OnSelectedIndexChanged="ddlsmstype_OnSelectedIndexChanged" runat="server"></asp:DropDownList><asp:Button ID="btnaddnew" Text="Add New" runat="server" OnClick="btnaddnew_OnClick" /></td>
</tr>--%>
<tr>
<td><asp:Button ID="btnaddnew" Text="Add New" runat="server" OnClick="btnaddnew_OnClick" /></td>
</tr>
<tr>
<td><asp:GridView ID="GVsmstemplate" AutoGenerateColumns="false" runat="server">
<Columns>
<asp:BoundField HeaderText="Sms Type" DataField="sms_type" />
<asp:BoundField HeaderText="Sms Content" DataField="sms_content" />
<asp:TemplateField>
<ItemTemplate>
<asp:Button ID="btnedit" Text="Edit" runat="server" OnClick="btnedit_OnClick" /><asp:HiddenField ID="HFsmsid" Value='<%#Bind("sms_id") %>' runat="server" /><asp:HiddenField ID="HForderbased" Value='<%#Bind("order_based") %>' runat="server" />
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView></td>
</tr>
<tr>
<td>
<asp:Label ID="lblerror" runat="server" ></asp:Label>
</td>
</tr>
<tr>
<td>
<table id="tblentry" runat="server" visible="false">
<tr>
<td>Sms Type</td>
<td><asp:TextBox ID="txtsmstype" runat="server"></asp:TextBox></td>
<td><asp:RequiredFieldValidator ID="rfvtxtsmstype" ControlToValidate="txtsmstype" ErrorMessage="*" ValidationGroup="saveupdate" runat="server"></asp:RequiredFieldValidator></td>
</tr>
<tr>
<td>Content</td>
<td><asp:TextBox ID="txttemplate" TextMode="MultiLine" runat="server"></asp:TextBox></td>
<td><asp:RequiredFieldValidator ID="rfvtxttemplate" ControlToValidate="txttemplate" ErrorMessage="*" ValidationGroup="saveupdate" runat="server"></asp:RequiredFieldValidator></td>
</tr>
<tr>
<td>Order Based</td>
<td><asp:DropDownList ID="ddlorderbased" runat="server">
<asp:ListItem Text="Yes" Value="1"></asp:ListItem>
<asp:ListItem Text="No" Value="0"></asp:ListItem>
</asp:DropDownList></td>
<td></td>
</tr>
<tr>
<td><asp:Button ID="BtnSave" Text="Save" OnClick="BtnSave_OnClick" ValidationGroup="saveupdate" runat="server" /></td>
<td><asp:Button ID="Btncancel" Text="Cancel" OnClick="BtnCancel_OnClick" runat="server" /></td>
<td><asp:HiddenField ID="HFsmsid" runat="server" /><asp:HiddenField ID="HFsmstype" runat="server" /><asp:HiddenField ID="HForderbased" runat="server" /></td>
</tr>
</table>
</td>
</tr>
</table>

</td>
</tr>
</table>
</asp:Content>
