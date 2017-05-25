<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true" CodeBehind="Orderentry.aspx.cs" Inherits="FFHPWeb.Orderentry" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table>
<tr>
<td></td>
</tr>
<tr>
<td>
<table>
    <tr>
    <td>Order No: </td>
    <td><asp:TextBox ID="txtidlist" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
        ErrorMessage="*" ControlToValidate="txtidlist" ValidationGroup="cal"></asp:RequiredFieldValidator></td>
    <td><asp:Button ID="btncalculate" Text="Submit" OnClick="btnsubmit1_OnClick" runat="server" ValidationGroup="cal"/></td>
    </tr>
    </table>
</td>
</tr>
</table>
</asp:Content>
