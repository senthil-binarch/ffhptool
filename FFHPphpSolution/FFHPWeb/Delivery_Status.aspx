<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true" CodeBehind="Delivery_Status.aspx.cs" Inherits="FFHPWeb.Delivery_Status" Theme="Skin1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table>
<tr>
<td>Delivery Status</td>
</tr>
<tr>
<td><table>
<tr>
<td>Process Date</td>
<td><asp:TextBox ID="TbxFromDate" Width="75px" runat="server" ></asp:TextBox><asp:Image
                                ID="Image1" ImageUrl="~/Images/cal1.png" runat="server" /><cc2:CalendarExtender ID="CalendarExtender1"
                                    runat="server" Animated="true" ClearTime="true" DefaultView="Days" Format="MM/dd/yyyy"
                                    PopupPosition="BottomRight" TargetControlID="TbxFromDate" PopupButtonID="Image1">
                                </cc2:CalendarExtender><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
        ErrorMessage="*" ControlToValidate="TbxFromDate" 
        ValidationGroup="Date"></asp:RequiredFieldValidator></td>
</tr>
<tr>
<td>Route
</td>
<td><asp:DropDownList ID="ddlroutelist" runat="server" AppendDataBoundItems="true" ></asp:DropDownList></td>
</tr>
<tr>
<td colspan="2"><asp:Button ID="btnsubmit" Text="Submit" runat="server" OnClick="btnsubmit_OnClick" /></td>
</tr>
</table></td>
</tr>
<tr>
<td><asp:GridView ID="grddeliverystatus" runat="server" AutoGenerateColumns="false" ShowFooter="true" >
<Columns>
<asp:TemplateField HeaderText="Sno">                   
    <ItemTemplate>
    <%#Container.DataItemIndex + 1 %>
    </ItemTemplate>
    </asp:TemplateField>
<asp:BoundField DataField="date" HeaderText="Date" />
<asp:BoundField DataField="route_number" HeaderText="Route Number" />
<asp:BoundField DataField="order_number" HeaderText="Order Number" />
<asp:BoundField DataField="customer_name" HeaderText="Customer Name" />
<asp:BoundField DataField="payment_mode" HeaderText="Payment Mode" />
<asp:TemplateField HeaderText="Order Amount">
<ItemTemplate>
<asp:Label ID="lblorderamount" Text='<%#Bind("order_amount") %>' runat="server"></asp:Label>
</ItemTemplate>
<FooterTemplate>
<asp:Label ID="lblordertotalamount" runat="server"></asp:Label>
</FooterTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Billed Amount">
<ItemTemplate >
<asp:Label ID="lblbilledamount" Text='<%#Eval("billed_amount","{0:n0}") %>' runat="server"></asp:Label>
</ItemTemplate>
<FooterTemplate>
<asp:Label ID="lblbilledtotalamount" runat="server"></asp:Label>
</FooterTemplate>
</asp:TemplateField>
<%--<asp:TemplateField HeaderText="Paid Amount">
<ItemTemplate>
<asp:Label ID="lblpaidamount" Text='<%#Bind("paid_amount") %>' runat="server"></asp:Label>
</ItemTemplate>
<FooterTemplate>
<asp:Label ID="lblpaidtotalamount" runat="server"></asp:Label>
</FooterTemplate>
</asp:TemplateField>--%>
<%--<asp:BoundField DataField="order_amount" HeaderText="Order Amount" />
<asp:BoundField DataField="billed_amount" HeaderText="Billed Amount" />
<asp:BoundField DataField="paid_amount" HeaderText="Paid Amount" />--%>
<%--<asp:BoundField DataField="refund" HeaderText="Refund" />
<asp:BoundField DataField="payment_status" HeaderText="Payment Status" />
<asp:BoundField DataField="delivery_status" HeaderText="Delivery Status" />
<asp:BoundField DataField="discription" HeaderText="Description" />--%>
</Columns>
</asp:GridView></td>
</tr>
</table>

</asp:Content>
