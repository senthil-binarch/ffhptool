<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true" CodeBehind="partner_payment_transaction.aspx.cs" Inherits="FFHPWeb.partner_payment_transaction" Theme="Skin1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%">
<tr>
<td class="heading">Payment Entry</td>
</tr>
<tr>
<td>
<asp:Label ID="lblerror" runat="server"></asp:Label>
</td>
</tr>
<tr>
<td>
<table>
<tr>
<td>Partner Name</td>
<td><asp:DropDownList ID="ddlpartner" Width="180px" OnSelectedIndexChanged="ddlpartner_OnSelectedIndexChanged" AutoPostBack="true" DataTextField="name" DataValueField="ffhp_partner_id" AppendDataBoundItems="true" runat="server">
                            </asp:DropDownList></td>
<td></td>
</tr>
<tr>
<td>Amount</td>
<td><asp:TextBox ID="tbxamount" Width="180px" runat="server"></asp:TextBox></td>
<td><asp:RequiredFieldValidator ID="rfvtbxamount" runat="server" ControlToValidate="tbxamount" ErrorMessage="*"></asp:RequiredFieldValidator><cc2:FilteredTextBoxExtender ID="ftetbxamount" runat="server" TargetControlID="tbxamount" ValidChars="0123456789."></cc2:FilteredTextBoxExtender></td>
</tr>
<tr>
<td>Ref. Number</td>
<td><asp:TextBox ID="tbxrefnumber" Width="180px" runat="server"></asp:TextBox></td>
<td><asp:RequiredFieldValidator ID="rfvtbxrefnumber" runat="server" ControlToValidate="tbxrefnumber" ErrorMessage="*"></asp:RequiredFieldValidator><%--<cc2:FilteredTextBoxExtender ID="ftetbxrefnumber" runat="server" TargetControlID="tbxrefnumber" FilterType="Numbers"></cc2:FilteredTextBoxExtender>--%></td>
</tr>
<tr>
<td>Payment Date</td>
<td><asp:TextBox ID="TbxFromDate" Width="75px" runat="server" ></asp:TextBox><asp:Image
                                ID="Image1" ImageUrl="~/Images/cal1.png" runat="server" /><cc2:CalendarExtender ID="CalendarExtender1"
                                    runat="server" Animated="true" ClearTime="true" DefaultView="Days" Format="MM/dd/yyyy"
                                    PopupPosition="BottomRight" TargetControlID="TbxFromDate" PopupButtonID="Image1">
                                </cc2:CalendarExtender><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
        ErrorMessage="*" ControlToValidate="TbxFromDate" 
        ValidationGroup="Date"></asp:RequiredFieldValidator></td>
<td></td>
</tr>
<tr>
<td colspan="2"><asp:Button ID="btnsubmit" Text="Submit" OnClick="btnsubmit_OnClick" runat="server" /></td>
<td></td>
</tr>
</table>
</td>
</tr>
<tr>
<td>
<asp:GridView ID="gvpaymenttransaction" runat="server" AutoGenerateColumns="false">
<Columns>
<asp:BoundField DataField="reference_no" HeaderText="Ref. Number" />
<asp:BoundField DataField="payment_amount" HeaderText="Payment Amount" />
<asp:BoundField DataField="payment_date" HeaderText="Payment Date" />
</Columns>
</asp:GridView>
</td>
</tr>
</table>
</asp:Content>
