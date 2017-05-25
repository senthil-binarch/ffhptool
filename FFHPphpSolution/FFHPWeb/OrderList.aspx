<%@ Page Language="C#" MasterPageFile="~/FFHP.Master"  AutoEventWireup="true" CodeBehind="OrderList.aspx.cs" Inherits="FFHPWeb.OrderList" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table>
    <tr>
    <td>
    <table>
<tr>
<td>From Date</td>
<td><asp:TextBox ID="TbxFromDate" Width="75px" runat="server" ></asp:TextBox><asp:Image
                                ID="Image1" ImageUrl="~/Images/cal1.png" runat="server" /><cc2:CalendarExtender ID="CalendarExtender1"
                                    runat="server" Animated="true" ClearTime="true" DefaultView="Days" Format="MM/dd/yyyy"
                                    PopupPosition="BottomRight" TargetControlID="TbxFromDate" PopupButtonID="Image1">
                                </cc2:CalendarExtender><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
        ErrorMessage="*" ControlToValidate="TbxFromDate" 
        ValidationGroup="Report"></asp:RequiredFieldValidator></td>
<td>To Date</td>
<td><asp:TextBox ID="TbxToDate" Width="75px" runat="server" ></asp:TextBox><asp:Image
                                ID="Image2" ImageUrl="~/Images/cal1.png" runat="server" /><cc2:CalendarExtender ID="CalendarExtender2"
                                    runat="server" Animated="true" ClearTime="true" DefaultView="Days" Format="MM/dd/yyyy"
                                    PopupPosition="BottomRight" TargetControlID="TbxToDate" PopupButtonID="Image2">
                                </cc2:CalendarExtender><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
        ErrorMessage="*" ControlToValidate="TbxToDate"></asp:RequiredFieldValidator></td>
        <td><asp:Button ID="btnsubmit" OnClick="btnsubmit_OnClick" runat="server" /></td>
        
</tr>
</table>
    </td>
    </tr>
    <tr>
    <td>
        <asp:GridView ID="GridView1" runat="server">
        </asp:GridView>
    </td>
    </tr>
    </table>
</asp:Content>