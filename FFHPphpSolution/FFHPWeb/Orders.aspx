<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="FFHPWeb.Orders" Theme="Skin1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" ChildrenAsTriggers="True">
                        <ContentTemplate>
                            <div>
<table width="100%">
<tr>
<td class="lblheading">Orders Information</td>
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
    <asp:GridView ID="GVOrderDetails" runat="server" EnableTheming="true" >
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
    <table>
    <tr>
    <td>Order No: </td>
    <td><asp:TextBox ID="txtidlist" TextMode="MultiLine" Height="50px" Width="250px" runat="server"></asp:TextBox><%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
        ErrorMessage="*" ControlToValidate="txtidlist" ValidationGroup="cal"></asp:RequiredFieldValidator>--%></td>
    <td><asp:Button ID="btncalculate" Text="Submit" OnClick="btnsubmit1_OnClick" runat="server" /><%--ValidationGroup="cal"--%></td>
    <%--<td><asp:Button ID="btnsendexcel" Text="Send Mail" Visible="false" runat="server" OnClick="btnsendexcel_OnClick" /></td>--%>
<td><asp:ImageButton ID="btnPDF" runat="server" Visible="false" ImageUrl="~/Images/PDF.jpg" Width="32px" 
Height="32px" ToolTip="Download" onclick="btnPDF_Click"/><asp:ImageButton ID="btnXLS" runat="server" Visible="false" ImageUrl="~/Images/xls-2.png" Width="32px" 
Height="32px" ToolTip="Download" onclick="btnXLS_Click"/>
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
<tr>
<td>
<asp:GridView ID="GvOrders" ShowHeader="false" runat="server" EnableTheming="false">
    </asp:GridView>
</td>
</tr>
<tr>
<td>
<asp:GridView ID="GVMail" ShowHeader="false" Visible="false" runat="server">
    </asp:GridView>
</td>
</tr>
<tr>
    <td>
    
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
                                        <div style="margin-left: auto; margin-right: auto; text-align: center; vertical-align: middle">
                                            <asp:Image ID="AjaxImage" runat="server" ImageUrl="~/Images/load.gif" />
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                        <asp:PostBackTrigger ControlID="btnPDF" />
                        <asp:PostBackTrigger ControlID="btnXLS" />
                        </Triggers>
                    </asp:UpdatePanel>
</asp:Content>
