<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true"
    CodeBehind="OrderNumber.aspx.cs" Inherits="FFHPWeb.OrderNumber" Theme="Skin1" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" ChildrenAsTriggers="True">
        <ContentTemplate>
            <div>
                <table width="100%">
                    <tr>
                        <td class="lblheading">
                            Order Numbers for service
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        From Date
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TbxFromDate" Width="75px" runat="server"></asp:TextBox><asp:Image
                                            ID="Image1" ImageUrl="~/Images/cal1.png" runat="server" /><cc2:calendarextender id="CalendarExtender1"
                                                runat="server" animated="true" cleartime="true" defaultview="Days" format="MM/dd/yyyy"
                                                popupposition="BottomRight" targetcontrolid="TbxFromDate" popupbuttonid="Image1">
                                </cc2:calendarextender><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                    ErrorMessage="*" ControlToValidate="TbxFromDate" ValidationGroup="Date"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        To Date
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TbxToDate" Width="75px" runat="server"></asp:TextBox><asp:Image
                                            ID="Image2" ImageUrl="~/Images/cal1.png" runat="server" /><cc2:calendarextender id="CalendarExtender2"
                                                runat="server" animated="true" cleartime="true" defaultview="Days" format="MM/dd/yyyy"
                                                popupposition="BottomRight" targetcontrolid="TbxToDate" popupbuttonid="Image2">
                                </cc2:calendarextender><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    ErrorMessage="*" ControlToValidate="TbxToDate" ValidationGroup="Date"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnsubmit" Text="Submit" OnClick="btnsubmit_OnClick" runat="server"
                                            ValidationGroup="Date" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="GVOrderDetails" runat="server">
                                <Columns>
                                    <%--<asp:TemplateField>
        <ItemTemplate>
        <asp:CheckBox ID="chkorderid" runat="server" />
        </ItemTemplate>
        </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Sno">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="CBorderall" OnCheckedChanged="CBorderall_OnCheckedChanged" runat="server"
                                                AutoPostBack="true" /><asp:HiddenField ID="HFordernum" Value='<%#Bind("entity_id") %>'
                                                    runat="server" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CBorder" OnCheckedChanged="CBorder_OnCheckedChanged" runat="server"
                                                AutoPostBack="true" /><asp:HiddenField ID="HFordernum" Value='<%#Bind("entity_id") %>'
                                                    runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="entity_id" HeaderText="Order #" />
                                    <asp:BoundField DataField="customername" HeaderText="Name" />
                                    <%--<asp:BoundField DataField="Address" HeaderText="Address" />
        <asp:BoundField DataField="Name" HeaderText="Pack" />--%>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        Order Numbers for service:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtidlist" TextMode="MultiLine" Height="50px" Width="250px" runat="server"></asp:TextBox><asp:RequiredFieldValidator
                                            ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ControlToValidate="txtidlist"
                                            ValidationGroup="cal"></asp:RequiredFieldValidator><cc2:FilteredTextBoxExtender ID="ftbe" runat="server" TargetControlID="txtidlist" ValidChars=", 1234567890" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btncalculate" Text="Submit" OnClick="btnsubmit1_OnClick" runat="server"
                                            ValidationGroup="cal" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblerror" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
