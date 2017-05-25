<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true"
    CodeBehind="collectionsheet.aspx.cs" Inherits="FFHPWeb.collectionsheet" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%">
        <tr>
            <td class="lblheading">
                Collection Sheet Details
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            Delivery Boy
                        </td>
                        <td>
                        <asp:DropDownList ID="ddldeliveryboy" runat="server" DataTextField="name" DataValueField="ddid" AppendDataBoundItems="true" ></asp:DropDownList>
                            <%--<asp:TextBox ID="TbxFromDate" Width="75px" runat="server"></asp:TextBox><asp:Image
                                ID="Image1" ImageUrl="~/Images/cal1.png" runat="server" /><cc2:calendarextender id="CalendarExtender1"
                                    runat="server" animated="true" cleartime="true" defaultview="Days" format="MM/dd/yyyy"
                                    popupposition="BottomRight" targetcontrolid="TbxFromDate" popupbuttonid="Image1">
                                </cc2:calendarextender><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                    ErrorMessage="*" ControlToValidate="TbxFromDate" ValidationGroup="Date"></asp:RequiredFieldValidator>--%>
                        </td>
                        <td><asp:Button ID="btnsubmit" Text="Submit" OnClick="btnsubmit_OnClick" runat="server" ValidationGroup="Date" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
        <td>
        <asp:GridView ID="gvcollectionsheet" runat="server" AutoGenerateColumns="false" ShowFooter="true">
        <Columns>
        <asp:TemplateField HeaderText="Sno">                   
        <ItemTemplate>
        <%#Container.DataItemIndex + 1 %>
        </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="date" HeaderText="Date" />
        <asp:BoundField DataField="route_number" HeaderText="Route Number" FooterText="B2B Amount" />
        <asp:BoundField DataField="order_number" HeaderText="Order Number" FooterText="" />
        <asp:BoundField DataField="customer_name" HeaderText="Customer Name" FooterText="Payment Pending" />
        <asp:BoundField DataField="payment_mode" HeaderText="Payment Mode" />
        <asp:BoundField DataField="order_amount" HeaderText="Ordered Amount" FooterText="COD" />
        <asp:BoundField DataField="billed_amount" HeaderText="Billed Amount" />
        <asp:BoundField DataField="paid_amount" HeaderText="Paid Amount" FooterText="EBS AMOUNT" />
        <asp:BoundField DataField="refund" HeaderText="Refund / to be collected" />
        <asp:BoundField DataField="deliveryboy_name" HeaderText="Delivery Person" FooterText="Refund" />
        <asp:BoundField DataField="payment_status" HeaderText="Payment Status" />
        <asp:BoundField DataField="delivery_status" HeaderText="Delivery Status" FooterText="EBS Extra" />
        <asp:BoundField DataField="discription" HeaderText="Discription" />
        
        </Columns>
        </asp:GridView>
        </td>
        </tr>
    </table>
</asp:Content>
