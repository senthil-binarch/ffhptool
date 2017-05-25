<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true" CodeBehind="purchase_entry_report.aspx.cs" Inherits="FFHPWeb.purchase_entry_report" Theme="Skin1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%">
<tr>
<td class="lblheading">Purchase Entry Report</td>
</tr>
<tr>
<td>
<table width="50%">
<tr>
<td width="18%">From Date</td>
<td width="32%"><asp:TextBox ID="TbxFromDate" Width="75px" runat="server" ></asp:TextBox><asp:Image
                                ID="Image1" ImageUrl="~/Images/cal1.png" runat="server" /><cc2:CalendarExtender ID="CalendarExtender1"
                                    runat="server" Animated="true" ClearTime="true" DefaultView="Days" Format="MM/dd/yyyy"
                                    PopupPosition="BottomRight" TargetControlID="TbxFromDate" PopupButtonID="Image1">
                                </cc2:CalendarExtender><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
        ErrorMessage="*" ControlToValidate="TbxFromDate" 
        ValidationGroup="Date"></asp:RequiredFieldValidator></td>
<td width="18%">To Date</td>
<td width="32%"><asp:TextBox ID="TbxToDate" Width="75px" runat="server" ></asp:TextBox><asp:Image
                                ID="Image2" ImageUrl="~/Images/cal1.png" runat="server" /><cc2:CalendarExtender ID="CalendarExtender2"
                                    runat="server" Animated="true" ClearTime="true" DefaultView="Days" Format="MM/dd/yyyy"
                                    PopupPosition="BottomRight" TargetControlID="TbxToDate" PopupButtonID="Image2">
                                </cc2:CalendarExtender><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
        ErrorMessage="*" ControlToValidate="TbxToDate" ValidationGroup="Date"></asp:RequiredFieldValidator></td>
</tr>
<tr>
<td width="18%">Vendor</td>
<td width="82%" colspan="3"><asp:DropDownList ID="ddlvendor" DataTextField="vendorname" DataValueField="vendorid" OnSelectedIndexChanged="ddlvendor_OnSelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="true" runat="server" Width="100%"></asp:DropDownList></td>
</tr>
<tr>
<td width="18%">Product</td>
<td width="82%" colspan="3"><asp:DropDownList ID="ddlproduct" DataTextField="pidname" DataValueField="productid" AppendDataBoundItems="true" runat="server" Width="100%"></asp:DropDownList></td>
</tr>
<tr>
<td colspan="4" align="center"><asp:Button ID="btnsubmit" Text="Submit" runat="server" OnClick="btnsubmit_OnClick" /></td>
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
<asp:GridView ID="gvpurchasereport" runat="server" OnRowDataBound="gvpurchasereport_OnRowDataBound">
<Columns>
<asp:TemplateField HeaderText="Sno">                   
    <ItemTemplate>
    <%#Container.DataItemIndex + 1 %>
    </ItemTemplate>
    </asp:TemplateField>
<asp:BoundField DataField="purchase_date" HeaderText="Purchase Date" DataFormatString="{0:dd/MM/yyyy}" />
<asp:BoundField DataField="vendorname" HeaderText="Vendor Name" />
<asp:BoundField DataField="productid" HeaderText="Product ID" />
<asp:BoundField DataField="name" HeaderText="Product Name" />
<asp:BoundField DataField="unit" HeaderText="Unit" />
<asp:BoundField DataField="wt_difference" HeaderText="Wt Diff" />
<asp:BoundField DataField="inward_stock" HeaderText="Received Wt" />
<asp:BoundField DataField="billed_weight" HeaderText="Purchase Wt" />
<asp:BoundField DataField="priceperkgpc" HeaderText="Price per kg/pc" />
<asp:BoundField DataField="price" HeaderText="Price" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#.00}" />
</Columns>
</asp:GridView>
</td>
</tr>
</table>
</asp:Content>
