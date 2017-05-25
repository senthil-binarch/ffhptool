<%@ Page Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true" CodeBehind="ffhpweight.aspx.cs" Inherits="FFHPWeb.ffhpweight" Theme="Skin1"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

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
        <td><asp:Button ID="btnsubmit" Text="Submit" OnClick="btnsubmit_OnClick" runat="server" /></td>
        
</tr>
</table>
    </td>
    </tr>
    <tr>
    <td>
    <asp:GridView ID="GVOrderDetails" runat="server" >
        <Columns>
        <%--<asp:TemplateField>
        <ItemTemplate>
        <asp:CheckBox ID="chkorderid" runat="server" />
        </ItemTemplate>
        </asp:TemplateField>--%>
        <asp:BoundField DataField="entity_id" HeaderText="Order Number" />
        <asp:BoundField DataField="customer_firstname" HeaderText="First Name" />
        <asp:BoundField DataField="customer_lastname" HeaderText="Last Name" />
        <asp:BoundField DataField="Name" HeaderText="Pack Name" />
        </Columns>
        </asp:GridView>
    </td>
    </tr>
    <tr>
    <td>
    <table>
    <tr>
    <td>Order No: </td>
    <td><asp:TextBox ID="txtidlist" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
        ErrorMessage="*" ControlToValidate="txtidlist" ValidationGroup="cal"></asp:RequiredFieldValidator></td>
    <td><asp:Button ID="btncalculate" Text="Calculate" OnClick="btncalculate_OnClick" runat="server" ValidationGroup="cal"/></td>
    </tr>
    </table>
    </td>
    </tr>
    <tr>
    <td>
    <table>
    <%--<tr>
    <td class="header">Pack Details</td>
    <td class="header">Weight Details</td>
    </tr>--%>
    <tr>
    <td><asp:Label ID="lblpackcount" CssClass="lbl" runat="server"></asp:Label></td>
    <td><asp:Label ID="lblkgcount" CssClass="lbl" runat="server"></asp:Label></td>
    </tr>
    </table>
    </td>
    </tr>
    <tr>
    <td>
    <asp:GridView ID="GvPackList" runat="server">
    <Columns>
    <asp:TemplateField HeaderText="Serial No">                   
    <ItemTemplate>
    <%#Container.DataItemIndex + 1 %>
    </ItemTemplate>
    </asp:TemplateField>
    <asp:BoundField DataField="Name" HeaderText="Name" />
<%--    <asp:BoundField DataField="Weight" HeaderText="Weight" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
    <asp:BoundField DataField="Pack" HeaderText="No. of Pack" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
--%>    <asp:BoundField DataField="TotalWeight" HeaderText="Total Weight" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
    </Columns>
    </asp:GridView>
    </td>
    </tr>
    <tr>
    <td>
    <asp:GridView ID="GvPackList2" runat="server">
    <Columns>
    <asp:TemplateField HeaderText="Serial No">                   
    <ItemTemplate>
    <%#Container.DataItemIndex + 1 %>
    </ItemTemplate>
    </asp:TemplateField>
    <asp:BoundField DataField="Name" HeaderText="Name" />
<%--    <asp:BoundField DataField="Weight" HeaderText="Weight" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
    <asp:BoundField DataField="Pack" HeaderText="No. of Pack" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
--%>    <asp:BoundField DataField="TotalWeight"  HtmlEncode="False" HeaderText="Total Weight" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
    </Columns>
    </asp:GridView>
    </td>
    </tr>
    <tr>
    <td>
        <asp:GridView ID="GridView1" runat="server" OnRowDataBound="GridView1_OnRowDataBound">
        <Columns>
        <asp:BoundField DataField="Name" HeaderText="Name" />
        <%--<asp:BoundField DataField="Weight" HeaderText="Weight" />--%>
        </Columns>
        </asp:GridView>
    </td>
    </tr>
    <tr>
    <td>
    <asp:Label ID="lblcount" runat="server"></asp:Label>
    </td>
    </tr>
    </table>
</asp:Content>
