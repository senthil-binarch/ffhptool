<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true" CodeBehind="AlertMail.aspx.cs" Inherits="FFHPWeb.AlertMail" Theme="Skin1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" ChildrenAsTriggers="True">
                        <ContentTemplate>
                            <div>
<table width="100%">
<tr>
<td class="lblheading">Customer List</td>
</tr>
<tr>
<td><table>
<tr>
<td>Select</td>
<td><asp:DropDownList ID="ddltype" AutoPostBack="true" OnSelectedIndexChanged="ddltype_OnSelectedIndexChanged" runat="server">
<asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
<asp:ListItem Text="Not ordered" Value="1"></asp:ListItem>
<asp:ListItem Text="Ordered Ones" Value="2"></asp:ListItem>
<asp:ListItem Text="1<orders<6" Value="3"></asp:ListItem>
<asp:ListItem Text="Above 5 orders" Value="4"></asp:ListItem>
<asp:ListItem Text="Above 5 orders(completed)" Value="5"></asp:ListItem>
</asp:DropDownList></td>
</tr>
</table></td>
</tr>
<tr>
<td>
<asp:GridView ID="GVNoOrderCustomerList" runat="server" ><%--OnRowDataBound="GVCustomerList_OnRowDataBound"--%>
<Columns>
<asp:TemplateField HeaderText="Sno">                   
    <ItemTemplate>
    <%#Container.DataItemIndex + 1 %>
    </ItemTemplate>
    </asp:TemplateField>
   <%-- <asp:TemplateField>
    <HeaderTemplate>
    <asp:CheckBox ID="CBorderall"  runat="server" AutoPostBack="true" />
    </HeaderTemplate>
    <ItemTemplate>
    <asp:CheckBox ID="CBorder"  runat="server" AutoPostBack="true" />
    </ItemTemplate>
    </asp:TemplateField>--%>
<asp:BoundField DataField="name" HeaderText="Name" />
<%--<asp:BoundField DataField="telephone" HeaderText="Phone" />--%>
<asp:BoundField DataField="email" HeaderText="email" />
<asp:BoundField DataField="created" HeaderText="Created Date" />
<%--<asp:TemplateField>
<ItemTemplate>
<asp:Button ID="btndetails" Text="Details" runat="server" OnClick="btndetails_OnClick" />
<asp:HiddenField ID="HFentity_id" runat="server" Value='<%#Bind("name") %>' />
<asp:GridView ID="gvdetails" runat="server" Visible="false">
<Columns>
<asp:BoundField DataField="TotalOrders" HeaderText="Total Orders" />
<asp:BoundField DataField="increment_id" HeaderText="Order Number" />
<asp:BoundField DataField="created_at" HeaderText="Date" />
</Columns>
</asp:GridView>
</ItemTemplate>
</asp:TemplateField>--%>
</Columns>
</asp:GridView>
<asp:GridView ID="GVCustomerList" runat="server" ><%--OnRowDataBound="GVCustomerList_OnRowDataBound"--%>
<Columns>
<asp:TemplateField HeaderText="Sno">                   
    <ItemTemplate>
    <%#Container.DataItemIndex + 1 %>
    </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField>
    <HeaderTemplate>
    <asp:CheckBox ID="CBorderall"  runat="server" AutoPostBack="true" />
    </HeaderTemplate>
    <ItemTemplate>
    <asp:CheckBox ID="CBorder"  runat="server" AutoPostBack="true" />
    </ItemTemplate>
    </asp:TemplateField>
<asp:BoundField DataField="name" HeaderText="Name" />
<asp:BoundField DataField="email" HeaderText="email" />
<%--<asp:BoundField DataField="created" HeaderText="Created Date" />--%>
<asp:TemplateField>
<ItemTemplate>
<asp:Button ID="btndetails" Text="Details" runat="server" OnClick="btndetails_OnClick" />
<asp:HiddenField ID="HFentity_id" runat="server" Value='<%#Bind("name") %>' />
<asp:GridView ID="gvdetails" runat="server" Visible="false">
<Columns>
<asp:BoundField DataField="registered_date" HeaderText="Created Date" />
<asp:BoundField DataField="telephone" HeaderText="Phone" />
<asp:BoundField DataField="TotalOrders" HeaderText="Total Orders" />
<asp:BoundField DataField="increment_id" HeaderText="Order Number" />
<asp:BoundField DataField="created_at" HeaderText="Date" />
</Columns>
</asp:GridView>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</td>
</tr>
<tr>
<td>
<asp:GridView ID="GVCustList" runat="server">
<Columns>
<asp:TemplateField HeaderText="Sno">                   
    <ItemTemplate>
    <%#Container.DataItemIndex + 1 %>
    </ItemTemplate>
    </asp:TemplateField>
<asp:BoundField DataField="customer_id" HeaderText="Customer ID" />
<asp:BoundField DataField="user_created_date" HeaderText="Customer Registered Date" />
<asp:BoundField DataField="shipping_name" HeaderText="Customer Name" />
<asp:BoundField DataField="totalorder" HeaderText="Total Order Placed" />
<asp:BoundField DataField="customer_email" HeaderText="Email" />
<%--<asp:BoundField DataField="email" HeaderText="Registered Email" />--%>
<asp:BoundField DataField="phone" HeaderText="Phone" />
<asp:BoundField DataField="increment_id" HeaderText="Last Order Number" />
<asp:BoundField DataField="created_at" HeaderText="Last Order Date" />
<%--<asp:BoundField DataField="ordernumber" HeaderText="Order Number" />--%>
</Columns>
</asp:GridView>
</td>
</tr>
<tr>
<td>
<asp:GridView ID="GVCustList1" runat="server">
<Columns>
<asp:TemplateField HeaderText="Sno">                   
    <ItemTemplate>
    <%#Container.DataItemIndex + 1 %>
    </ItemTemplate>
    </asp:TemplateField>
<asp:BoundField DataField="customer_id" HeaderText="Customer ID" />
<asp:BoundField DataField="user_created_date" HeaderText="Customer Registered Date" />
<asp:BoundField DataField="shipping_name" HeaderText="Customer Name" />
<asp:BoundField DataField="totalorder1" HeaderText="Total Order Placed" />
<asp:BoundField DataField="totalorder" HeaderText="Total Order Placed(Completed)" />
<asp:BoundField DataField="customer_email" HeaderText="Email" />
<%--<asp:BoundField DataField="email" HeaderText="Registered Email" />--%>
<asp:BoundField DataField="phone" HeaderText="Phone" />
<asp:BoundField DataField="increment_id" HeaderText="Last Order Number" />
<asp:BoundField DataField="created_at" HeaderText="Last Order Date" />
<%--<asp:BoundField DataField="ordernumber" HeaderText="Order Number" />--%>
</Columns>
</asp:GridView>
</td>
</tr>
</table>
</div>
<cc2:AlwaysVisibleControlExtender ID="AlwaysVisibleControlExtender1" runat="server"
                                TargetControlID="Panel1" VerticalSide="Middle" HorizontalSide="Center">
                            </cc2:AlwaysVisibleControlExtender>
                            <asp:Panel ID="Panel1" runat="server">
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="2">
                                    <ProgressTemplate>
                                        <div style="margin-right: auto; text-align: center; vertical-align: middle">
                                            <asp:Image ID="AjaxImage" runat="server" ImageUrl="~/Images/load.gif" />
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
