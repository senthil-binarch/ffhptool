<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true"
    CodeBehind="SendSmsCustomerTemplate.aspx.cs" Inherits="FFHPWeb.SendSmsCustomerTemplate"
    Theme="Skin1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%">
        <tr>
            <td align="right">
                <asp:Button ID="btndownload" Text="download" runat="server" OnClick="btndownload_OnClick" />
            </td>
        </tr>
        <tr>
            <td>
                Send Sms Customer Template
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            Select :
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblselect" RepeatDirection="Horizontal" runat="server" OnSelectedIndexChanged="rblselect_OnSelectedIndexChanged"
                                AutoPostBack="true">
                                <asp:ListItem Text="Customer List" Value="1"></asp:ListItem>
                                <asp:ListItem Text="All Customer" Value="2"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Sms to
                        </td>
                        <td>
                            <table id="tbselect" runat="server">
                                <tr id="trcustomerlist1" runat="server" visible="false">
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:DropDownList ID="ddltype" AutoPostBack="true" OnSelectedIndexChanged="ddltype_OnSelectedIndexChanged"
                                                        runat="server">
                                                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Not ordered" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Ordered Ones" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="1<orders<6" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="Above 5 orders" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="Above 5 orders(completed)" Value="5"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="GVNoOrderCustomerList" runat="server">
                                                        <%--OnRowDataBound="GVCustomerList_OnRowDataBound"--%>
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
                                                            <asp:BoundField DataField="telephone" HeaderText="Phone" />
                                                            <asp:BoundField DataField="email" HeaderText="email" />
                                                            <asp:BoundField DataField="created" HeaderText="Created Date" />
                                                            <asp:TemplateField HeaderText="Confirmed">
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="CBCustomerConfirmedall_GVNoOrderCustomerList" Text="Confirmed All"
                                                                        OnCheckedChanged="CustomerConfirmedall_GVNoOrderCustomerList_OnCheckedChanged"
                                                                        runat="server" AutoPostBack="true" /><%--<asp:HiddenField ID="HFcodordernum" Value='<%#Bind("entity_id") %>' runat="server" />--%>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CBConfirmed" runat="server" /><asp:HiddenField ID="HFTelephone"
                                                                        Value='<%#Bind("telephone") %>' runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
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
                                                    <asp:GridView ID="GVCustomerList1" runat="server">
                                                        <%--OnRowDataBound="GVCustomerList_OnRowDataBound"--%>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Sno">
                                                                <ItemTemplate>
                                                                    <%#Container.DataItemIndex + 1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="CBorderall" runat="server" AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CBorder" runat="server" AutoPostBack="true" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="name" HeaderText="Name" />
                                                            <asp:BoundField DataField="email" HeaderText="email" />
                                                            <%--<asp:BoundField DataField="created" HeaderText="Created Date" />--%>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <%--<asp:Button ID="btndetails" Text="Details" runat="server" OnClick="btndetails_OnClick" />--%>
                                                                    <asp:HiddenField ID="HFentity_id" runat="server" Value='<%#Bind("name") %>' />
                                                                    <asp:GridView ID="gvdetails" runat="server" Visible="false">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="registered_date" HeaderText="Created Date" />
                                                                            <asp:BoundField DataField="telephone" HeaderText="Phone" />
                                                                            <asp:BoundField DataField="TotalOrders" HeaderText="Total Orders" />
                                                                            <asp:BoundField DataField="increment_id" HeaderText="Order Number" />
                                                                            <asp:BoundField DataField="created_at" HeaderText="Date" />
                                                                            <asp:TemplateField HeaderText="Confirmed">
                                                                                <HeaderTemplate>
                                                                                    <asp:CheckBox ID="CBCustomerConfirmedall_GVCustomerList1" Text="Confirmed All" OnCheckedChanged="CustomerConfirmedall_GVCustomerList1_OnCheckedChanged"
                                                                                        runat="server" AutoPostBack="true" /><%--<asp:HiddenField ID="HFcodordernum" Value='<%#Bind("entity_id") %>' runat="server" />--%>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="CBConfirmed" runat="server" /><asp:HiddenField ID="HFTelephone"
                                                                                        Value='<%#Bind("telephone") %>' runat="server" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
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
                                                            <asp:BoundField DataField="shipping_name" HeaderText="Customer Name" />
                                                            <asp:BoundField DataField="customer_id" HeaderText="Customer ID" />
                                                            <asp:BoundField DataField="user_created_date" HeaderText="Customer Registered Date" />
                                                            <asp:BoundField DataField="totalorder" HeaderText="Total Order Placed" />
                                                            <asp:BoundField DataField="customer_email" HeaderText="Email" />
                                                            <%--<asp:BoundField DataField="email" HeaderText="Registered Email" />--%>
                                                            <asp:BoundField DataField="phone" HeaderText="Phone" />
                                                            <asp:BoundField DataField="increment_id" HeaderText="Last Order Number" />
                                                            <asp:BoundField DataField="created_at" HeaderText="Last Order Date" />
                                                            <%--<asp:BoundField DataField="ordernumber" HeaderText="Order Number" />--%>
                                                            <asp:TemplateField HeaderText="Confirmed">
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="CBCustomerConfirmedall_GVCustList" Text="Confirmed All" OnCheckedChanged="CustomerConfirmedall_GVCustList_OnCheckedChanged"
                                                                        runat="server" AutoPostBack="true" /><%--<asp:HiddenField ID="HFcodordernum" Value='<%#Bind("entity_id") %>' runat="server" />--%>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CBConfirmed" runat="server" /><asp:HiddenField ID="HFTelephone"
                                                                        Value='<%#Bind("phone") %>' runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
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
                                                            <asp:BoundField DataField="shipping_name" HeaderText="Customer Name" />
                                                            <asp:BoundField DataField="customer_id" HeaderText="Customer ID" />
                                                            <asp:BoundField DataField="user_created_date" HeaderText="Customer Registered Date" />
                                                            <asp:BoundField DataField="totalorder1" HeaderText="Total Order Placed" />
                                                            <asp:BoundField DataField="totalorder" HeaderText="Total Order Placed(Completed)" />
                                                            <asp:BoundField DataField="customer_email" HeaderText="Email" />
                                                            <%--<asp:BoundField DataField="email" HeaderText="Registered Email" />--%>
                                                            <asp:BoundField DataField="phone" HeaderText="Phone" />
                                                            <asp:BoundField DataField="increment_id" HeaderText="Last Order Number" />
                                                            <asp:BoundField DataField="created_at" HeaderText="Last Order Date" />
                                                            <%--<asp:BoundField DataField="ordernumber" HeaderText="Order Number" />--%>
                                                            <asp:TemplateField HeaderText="Confirmed">
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="CBCustomerConfirmedall_GVCustList1" Text="Confirmed All" OnCheckedChanged="CustomerConfirmedall_GVCustList1_OnCheckedChanged"
                                                                        runat="server" AutoPostBack="true" /><%--<asp:HiddenField ID="HFcodordernum" Value='<%#Bind("entity_id") %>' runat="server" />--%>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CBConfirmed" runat="server" /><asp:HiddenField ID="HFTelephone"
                                                                        Value='<%#Bind("phone") %>' runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trcustomerlist2" runat="server" visible="false">
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="BtnGetCustomers" runat="server" Text="Get Customer List" OnClick="BtnGetCustomers_OnClick" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="Gvcustomerlist" runat="server">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Sno">
                                                                <ItemTemplate>
                                                                    <%#Container.DataItemIndex + 1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="customername" HeaderText="Customer Name" />
                                                            <%--<asp:BoundField DataField="First_Name" HeaderText="First Name" />
                                                            <asp:BoundField DataField="Last_Name" HeaderText="Last Name" />--%>
                                                            <asp:BoundField DataField="Telephone" HeaderText="Telephone" />
                                                            <asp:TemplateField HeaderText="Confirmed">
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="CustomerConfirmedall" Text="Confirmed All" OnCheckedChanged="CustomerConfirmedall_OnCheckedChanged"
                                                                        runat="server" AutoPostBack="true" /><%--<asp:HiddenField ID="HFcodordernum" Value='<%#Bind("entity_id") %>' runat="server" />--%>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CBConfirmed" runat="server" /><asp:HiddenField ID="HFTelephone"
                                                                        Value='<%#Bind("Telephone") %>' runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Sms Type:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlsmstype" AutoPostBack="true" OnSelectedIndexChanged="ddlsmstype_OnSelectedIndexChanged"
                                runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Template:
                        </td>
                        <td>
                            <asp:Label ID="Lblcontent" runat="server"></asp:Label><%--<asp:TextBox ID="tbxcontent" TextMode="MultiLine" runat="server"></asp:TextBox>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Panel ID="Panel1" runat="server">
                            </asp:Panel>
                            <asp:GridView ID="GvTestboxlist" AutoGenerateColumns="false" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="id" HeaderText="Fields" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:TextBox ID="Textboxlist" runat="server"></asp:TextBox><asp:RequiredFieldValidator
                                                ID="rfvTextboxlist" ControlToValidate="Textboxlist" ErrorMessage="*" ValidationGroup="InsertFields"
                                                runat="server"></asp:RequiredFieldValidator>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="BtnGetReplaced" Text="Insert Fields" OnClick="BtnGetReplaced_OnClick"
                                ValidationGroup="InsertFields" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="Lblsmscontent" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnsendsmsConfirmed" OnClick="btnsendsmsConfirmed_OnClick" Text="Send"
                                runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HiddenField ID="HFsms_content" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblerror" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbloutputmessage" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
