<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true" CodeBehind="partner.aspx.cs" Inherits="FFHPWeb.partner" Theme="Skin1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%">
<tr>
<td class="heading">Partner</td>
</tr>
<tr>
<td>
<table>
<tr>
<td>Name</td>
<td><asp:TextBox ID="tbxname" Width="200px" runat="server"></asp:TextBox></td>
<td><asp:RequiredFieldValidator ID="rfvname" ControlToValidate="tbxname" ErrorMessage="*" runat="server"></asp:RequiredFieldValidator></td>
</tr>
<tr>
<td>Phone</td>
<td><asp:TextBox ID="tbxphone" Width="200px" runat="server" MaxLength="12"></asp:TextBox></td>
<td><asp:RequiredFieldValidator ID="rfvphone" ControlToValidate="tbxphone" ErrorMessage="*" runat="server"></asp:RequiredFieldValidator><cc2:FilteredTextBoxExtender ID="ftephone" runat="server" TargetControlID="tbxphone" FilterType="Numbers"></cc2:FilteredTextBoxExtender></td>
</tr>
<tr>
<td>Email</td>
<td><asp:TextBox ID="tbxemail" Width="200px" runat="server"></asp:TextBox></td>
<td><asp:RequiredFieldValidator ID="rfvemail" ControlToValidate="tbxemail" ErrorMessage="*" runat="server"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="revemail" runat="server" 
       ErrorMessage="*"
       ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
       ControlToValidate="tbxemail" /></td>
</tr>
<tr>
<td>Address</td>
<td><asp:TextBox ID="tbxaddress" Width="200px" runat="server" TextMode="MultiLine"></asp:TextBox></td>
<td><asp:RequiredFieldValidator ID="rfvaddress" ControlToValidate="tbxaddress" ErrorMessage="*" runat="server"></asp:RequiredFieldValidator></td>
</tr>
<tr>
<td>Coupon</td>
<td><%--<select id="checklist" runat="server" name="cars" datatextfield='<%#Eval("code") %>' datavaluefield='<%#Eval("coupon_id") %>' multiple>
</select>--%><div style="border:1px solid black;height:100px;overflow-y:scroll; overflow-x:hidden;"><asp:CheckBoxList ID="checklist" Width="180px" DataTextField="code" DataValueField="coupon_id" runat="server"></asp:CheckBoxList></div></td>
<td></td>
</tr>
<tr>
<td>Group</td>
<td><%--<select id="checklist" runat="server" name="cars" datatextfield='<%#Eval("code") %>' datavaluefield='<%#Eval("coupon_id") %>' multiple>
</select>--%><div style="border:1px solid black;height:100px;overflow-y:scroll; overflow-x:hidden;"><asp:CheckBoxList ID="checklistgroup" Width="180px" DataTextField="customer_group_code" DataValueField="customer_group_id" runat="server"></asp:CheckBoxList></div></td>
<td></td>
</tr>
<tr>
<td colspan="3" align="center">
<table>
<tr>
<td><asp:Button ID="btnsave" Text="Save" OnClick="btnsave_OnClick" runat="server" /></td>
<td><asp:Button ID="btnclear" CausesValidation="false" Text="Clear" OnClick="btnclear_OnClick" runat="server" /></td>
</tr>
</table>
</td>
</tr>
<tr>
<td colspan="3"><asp:Label ID="lblerror" runat="server"></asp:Label></td>
</tr>
</table>
</td>
</tr>
<tr>
<td>
<asp:GridView ID="gvpartner" OnRowDataBound="gvpartner_OnRowDataBound" runat="server" >
<Columns>
<asp:BoundField DataField="name" HeaderText="Name" />
<asp:BoundField DataField="phone" HeaderText="Phone" />
<asp:BoundField DataField="email" HeaderText="Email" />
<asp:BoundField DataField="address" HeaderText="Address" />
<asp:TemplateField HeaderText="Coupon Code">
<ItemTemplate>
<asp:Label ID="lblcouponcode" runat="server" ></asp:Label><asp:HiddenField ID="hfffhp_partner_id" runat="server" Value='<%#Bind("ffhp_partner_id") %>' />
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Edit">
<ItemTemplate>
<asp:Button ID="btnedit" CausesValidation="false" runat="server" Text="Edit" OnClick="btnedit_OnClick" />
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView><asp:HiddenField ID="hfffhp_partner_id" runat="server" Value='<%#Bind("ffhp_partner_id") %>' />
</td>
</tr>
</table>
</asp:Content>
