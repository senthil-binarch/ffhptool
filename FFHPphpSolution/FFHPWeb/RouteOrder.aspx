<%@ Page Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true" CodeBehind="RouteOrder.aspx.cs" Inherits="FFHPWeb.RouteOrder" Theme="Skin1"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" ChildrenAsTriggers="True">
                        <ContentTemplate>
                            <div>
        <table width="100%">
<tr>
<td class="lblheading">Route Information</td>
</tr>
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
        ValidationGroup="Date"></asp:RequiredFieldValidator></td>
<td>To Date</td>
<td><asp:TextBox ID="TbxToDate" Width="75px" runat="server" ></asp:TextBox><asp:Image
                                ID="Image2" ImageUrl="~/Images/cal1.png" runat="server" /><cc2:CalendarExtender ID="CalendarExtender2"
                                    runat="server" Animated="true" ClearTime="true" DefaultView="Days" Format="MM/dd/yyyy"
                                    PopupPosition="BottomRight" TargetControlID="TbxToDate" PopupButtonID="Image2">
                                </cc2:CalendarExtender><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
        ErrorMessage="*" ControlToValidate="TbxToDate" ValidationGroup="Date"></asp:RequiredFieldValidator></td>
        <td><asp:Button ID="btnsubmit" Text="Submit" OnClick="btnsubmit_OnClick" runat="server" ValidationGroup="Date" /></td>
        
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
        <asp:TemplateField HeaderText="Sno">                   
    <ItemTemplate>
    <%#Container.DataItemIndex + 1 %>
    </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField>
    <HeaderTemplate>
    <asp:CheckBox ID="CBorderall" OnCheckedChanged="CBorderall_OnCheckedChanged" runat="server" AutoPostBack="true" /><asp:HiddenField ID="HFordernum" Value='<%#Bind("entity_id") %>' runat="server" />
    </HeaderTemplate>
    <ItemTemplate>
    <asp:CheckBox ID="CBorder" OnCheckedChanged="CBorder_OnCheckedChanged" runat="server" AutoPostBack="true" /><asp:HiddenField ID="HFordernum" Value='<%#Bind("entity_id") %>' runat="server" />
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
    <asp:GridView Width="50%" ID="GVRouteClear" ShowFooter="true" runat="server" AutoGenerateColumns="false">
    <Columns>
    <asp:BoundField HeaderText="Route Id" DataField="route_id" />
    <asp:BoundField HeaderText="Route Name" DataField="route_name" />
    <asp:BoundField ItemStyle-Width="100px" HeaderText="Order Number" FooterStyle-HorizontalAlign="Right" DataField="ordernumber" />
    <asp:BoundField ItemStyle-Width="100px" HeaderText="Count" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" DataField="count" />
    <asp:TemplateField>
    <ItemTemplate>
    <asp:Button ID="btnclear" Text="Clear" runat="server" OnClick="btnclear_OnClick"  />
    <asp:HiddenField ID="HFrouteid" Value='<%#bind("route_id") %>' runat="server" />
    </ItemTemplate>
    </asp:TemplateField>
    </Columns>
    </asp:GridView>
    </td>
    </tr>
    <tr>
    <td>
    <table>
    <tr>
    <td>Route</td>
    <td><asp:DropDownList ID="ddlroutelist" runat="server" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlroutelist_OnSelectedIndexChanged" ></asp:DropDownList></td>
    </tr>
    <tr>
    <td>Order Number</td>
    <td><asp:TextBox ID="txtidlist" TextMode="MultiLine" runat="server"></asp:TextBox></td>
    </tr>
    <tr id="trdriver" runat="server" visible="false">
    <td visible="false">Driver</td>
    <td><asp:DropDownList Visible="false" ID="ddldriver" runat="server" AppendDataBoundItems="true"></asp:DropDownList></td>
    </tr>
    <tr id="trdeliveryboy" runat="server" visible="false">
    <td visible="false">Delivery Boy</td>
    <td><asp:DropDownList Visible="false" ID="ddldeliveryboy" runat="server" AppendDataBoundItems="true"></asp:DropDownList></td>
    </tr>
    <tr>
    <td><asp:Button ID="btnsave" Text="Save" OnClick="btnsave_OnClick" runat="server" /></td>
    </tr>
    </table>
    </td>
    </tr>
    <tr>
    <td>
    <asp:Label ID="lblerror" runat="server"></asp:Label>
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
    <asp:Label ID="lblcount" runat="server"></asp:Label>
    </td>
    </tr>
    <tr>
    <td>
    <asp:GridView ID="gvordernumber" runat="server">
    <Columns>
    <asp:BoundField DataField="Data" />
    </Columns>
    </asp:GridView>
    <asp:DataList ID="dlordernumber" runat="server" RepeatDirection="Horizontal" RepeatColumns="4" EnableTheming="false" Width="70%" Visible="false">
    <ItemTemplate>
    <asp:Label ID="lblordernumber"  Text='<%#Bind("Data") %>' runat="server" Font-Names="Calibri" Font-Size="12pt" ></asp:Label>
    </ItemTemplate>
    </asp:DataList>
    </td>
    </tr>
    <tr>
    <td><asp:Button ID="btnpdf" OnClick="btnpdf_OnClick" runat="server" Text="PDF" Visible="false" />
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
