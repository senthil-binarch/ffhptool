<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true" CodeBehind="FreeUpdate.aspx.cs" Inherits="FFHPWeb.FreeUpdate" Theme="Skin1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table>
<tr>
<td class="lblheading">Free Update</td>
</tr>
<tr>
<td><asp:GridView ID="gvfree" runat="server">
<Columns>
<asp:BoundField DataField="product_id" HeaderText="ID" />
<asp:BoundField DataField="name" HeaderText="Free Product Name" />
<asp:BoundField DataField="weight" HeaderText="Weight/Piece" />
<asp:BoundField DataField="units" HeaderText="Kg/Pc" />
<asp:BoundField DataField="ParentProductid" HeaderText="Parent Product Id" />
<asp:TemplateField>
<ItemTemplate>
<asp:Button ID="btnedit" Text="Edit" runat="server" OnClick="btnedit_OnClick" />
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView></td>
</tr>
<tr>
<td><asp:Label ID="lblerror" runat="server"></asp:Label></td>
</tr>
<tr>
<td>
<table id="tbl" runat="server" visible="false">
<tr>
<td>Name</td>
<td>:<asp:TextBox ID="tbxname" runat="server"></asp:TextBox></td>
</tr>
<tr>
<td>Weight/Piece</td>
<td>:<asp:TextBox ID="tbxweightpiece" runat="server" ></asp:TextBox><cc2:FilteredTextBoxExtender ID="ftetbxweightpiece" runat="server" TargetControlID="tbxweightpiece" ValidChars="0123456789."></cc2:FilteredTextBoxExtender></td>
</tr>
<tr>
<td>Kg/Pc</td>
<td>:<asp:TextBox ID="tbxkgpc" runat="server" MaxLength="2"></asp:TextBox><cc2:FilteredTextBoxExtender ID="ftetbxkgpc" runat="server" TargetControlID="tbxkgpc" FilterType="LowercaseLetters"></cc2:FilteredTextBoxExtender></td>
</tr>
<tr>
<td>Parent Product Id</td>
<td>:<asp:TextBox ID="tbxparentproductid" runat="server"></asp:TextBox><cc2:FilteredTextBoxExtender ID="ftetbxparentproductid" runat="server" TargetControlID="tbxparentproductid" FilterType="Numbers"></cc2:FilteredTextBoxExtender></td>
</tr>
<tr>
<td colspan="2"><asp:Button ID="btnupdate" Text="Update" OnClick="btnupdate_OnClick" runat="server" /></td>
</tr>
</table><asp:HiddenField ID="hfproductid" runat="server" />
</td>
</tr>
</table>
</asp:Content>
