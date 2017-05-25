<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true" CodeBehind="SendSmsTemplate.aspx.cs" Inherits="FFHPWeb.SendSmsTemplate" Theme="Skin1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%">
<tr>
<td align="right" ><asp:Button ID="btndownload" Text="download" runat="server" OnClick="btndownload_OnClick" /></td>
</tr>
<tr>
<td>Send Sms Template</td>
</tr>
<tr>
<td>
<table>
<tr>
<td>Select :</td>
<td><asp:RadioButtonList ID="rblselect" runat="server" OnSelectedIndexChanged="rblselect_OnSelectedIndexChanged" AutoPostBack="true" >
<asp:ListItem Text="Order Numbers" Value="0"></asp:ListItem>
<asp:ListItem Text="Order List" Value="1"></asp:ListItem>
<%--<asp:ListItem Text="Customer List" Value="2"></asp:ListItem>--%>
</asp:RadioButtonList></td>
</tr>
<tr>
<td>Sms to </td>
<td>
<table id="tbselect" runat="server" visible="false">
<tr id="trordernumber" runat="server" visible="false">
<td><asp:TextBox ID="txtordernumber" runat="server" TextMode="MultiLine"></asp:TextBox></td>
</tr>
<tr id="trorderlist" runat="server" visible="false">
<td><table>
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
<tr>
<td colspan="5"></td>
</tr>
</table><asp:GridView ID="Gvorderlist"  runat="server" AutoGenerateColumns="false">
        <Columns>
        <asp:TemplateField HeaderText="Sno">                   
    <ItemTemplate>
    <%#Container.DataItemIndex + 1 %>
    </ItemTemplate>
    </asp:TemplateField>
        <asp:BoundField DataField="entity_id" HeaderText="Order #"  />
        <asp:BoundField DataField="customername" HeaderText="Name" />
        <%--<asp:BoundField DataField="Address" HeaderText="Address" />
        <asp:BoundField DataField="Name" HeaderText="Pack" />--%>
        <asp:TemplateField HeaderText="Confirmed">
    <HeaderTemplate>
    <asp:CheckBox ID="Confirmedall" Text="Confirmed All" OnCheckedChanged="Confirmedall_OnCheckedChanged" runat="server" AutoPostBack="true" /><%--<asp:HiddenField ID="HFcodordernum" Value='<%#Bind("entity_id") %>' runat="server" />--%>
    </HeaderTemplate>
    <ItemTemplate>
    <asp:CheckBox ID="CBConfirmed" runat="server" /><asp:HiddenField ID="HFConfirmedordernum" Value='<%#Bind("entity_id") %>' runat="server" />
    </ItemTemplate>
    </asp:TemplateField>
    </Columns>
    </asp:GridView></td>
</tr>
</table>
</td>
</tr>
<tr>
<td>Sms Type:</td>
<td><asp:DropDownList ID="ddlsmstype" AutoPostBack="true" OnSelectedIndexChanged="ddlsmstype_OnSelectedIndexChanged" runat="server"></asp:DropDownList></td>
</tr>
<tr>
<td>Template:</td>
<td><asp:Label ID="Lblcontent" runat="server"></asp:Label><%--<asp:TextBox ID="tbxcontent" TextMode="MultiLine" runat="server"></asp:TextBox>--%></td>
</tr>
<tr>
<td></td>
<td><asp:Panel ID="Panel1" runat="server"></asp:Panel>
<asp:GridView ID="GvTestboxlist" AutoGenerateColumns="false" runat="server">
<Columns>
<asp:BoundField DataField="id" HeaderText="Fields" />
<asp:TemplateField>
<ItemTemplate>
<asp:TextBox ID="Textboxlist" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="rfvTextboxlist" ControlToValidate="Textboxlist" ErrorMessage="*" ValidationGroup="InsertFields" runat="server"></asp:RequiredFieldValidator>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView></td>
</tr>
<tr>
<td></td>
<td><asp:Button ID="BtnGetReplaced" Text="Insert Fields" OnClick="BtnGetReplaced_OnClick" ValidationGroup="InsertFields" runat="server" /></td>
</tr>
<tr>
<td></td>
<td><asp:Label ID="Lblsmscontent" runat="server"></asp:Label></td>
</tr>
<tr>
<td></td>
<td><asp:Button ID="btnsendsmsConfirmed" OnClick="btnsendsmsConfirmed_OnClick" Text="Send" runat="server" /></td>
</tr>
</table>
</td>
</tr>
<tr>
<td><asp:HiddenField ID="HFsms_content" runat="server" /></td>
</tr>
<tr>
<td>
<asp:Label ID="lblerror" runat="server"></asp:Label>
</td>
</tr>
</table>
</asp:Content>
