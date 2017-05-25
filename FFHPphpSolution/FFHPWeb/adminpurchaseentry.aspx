<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true" CodeBehind="adminpurchaseentry.aspx.cs" Inherits="FFHPWeb.adminpurchaseentry" Theme="Skin1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="css/bootstrap-multiselect.css"
        rel="stylesheet" type="text/css" />
<link href="css/bootstrap.min.css"
        rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%--<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>--%>
    <%--<link href="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css"
        rel="stylesheet" type="text/css" />--%>
        <%--<link href="//netdna.bootstrapcdn.com/bootstrap/3.0.3/css/bootstrap.min.css"
        rel="stylesheet" type="text/css" />--%>
        
    <%--<script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>--%>
    <%--<link href="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css"
        rel="stylesheet" type="text/css" />--%>
        
    <%--<script src="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/js/bootstrap-multiselect.js"
        type="text/javascript"></script>--%>
    <%--<script type="text/javascript">
        $(function() {
            $('[id*=lstFruits]').multiselect({
                includeSelectAllOption: true
            });
        });
    </script>--%>
    <asp:UpdatePanel runat="server">
    <ContentTemplate>
    
<table width="100%">
<tr>
<td class="heading">Purchase Entry</td>
</tr>
<tr>
<td></td>
</tr>
<tr>
<td>
<table width="50%">
<tr height="50px">
<td>Purchase Date</td>
<td><div id="divdate" runat="server"><asp:TextBox ID="TbxPurchaseDate" Width="75px" runat="server" ></asp:TextBox><asp:Image
                                ID="Image1" ImageUrl="~/Images/cal1.png" runat="server" /><cc2:CalendarExtender ID="CalendarExtender1"
                                    runat="server" Animated="true" ClearTime="true" DefaultView="Days" Format="MM/dd/yyyy"
                                    PopupPosition="BottomRight" TargetControlID="TbxPurchaseDate" PopupButtonID="Image1">
                                </cc2:CalendarExtender><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
        ErrorMessage="*" ControlToValidate="TbxPurchaseDate" 
        ValidationGroup="Date"></asp:RequiredFieldValidator></div><div id="divdatetext" runat="server" visible="false"><asp:Label ID="lbldatetext" runat="server"></asp:Label></div></td>
        <td><asp:Button ID="btndatesubmit" Text="Submit" OnClick="btndatesubmit_OnClick" runat="server" /></td>
</tr>
<tr id="trvendor" runat="server" visible="false">
<td width="25%">Vendor</td>
<td width="75%" colspan="2"><asp:DropDownList ID="ddlvendor" OnSelectedIndexChanged="ddlvendor_OnSelectedIndexChanged" DataTextField="vendorname" DataValueField="vendorid" AppendDataBoundItems="true" AutoPostBack="true" runat="server"></asp:DropDownList></td>
</tr>
</table>
</td>
</tr>
<tr>
<td><asp:Label ID="lblerror" runat="server"></asp:Label></td>
</tr>
<tr>
<td>
<%--<asp:GridView ID="gvvendorproducts" runat="server" OnRowDataBound="gvvendorproducts_OnRowDataBound" >
<Columns>
<asp:TemplateField HeaderText="Sno">                   
    <ItemTemplate>
    <%#Container.DataItemIndex + 1 %>
    </ItemTemplate>
    </asp:TemplateField>
<asp:BoundField DataField="productid" HeaderText="Product ID" />
<asp:BoundField DataField="name" HeaderText="Name" />
<asp:BoundField DataField="unit" HeaderText="Unit" />
<asp:BoundField DataField="actual_weight" HeaderText="Actual Qty" />
<asp:BoundField DataField="extended_weight" HeaderText="Purchase Qty" />
<asp:BoundField DataField="inward_stock" HeaderText="Received Qty" />
<asp:TemplateField HeaderText="Billed Qty">
<ItemTemplate>
<asp:TextBox ID="tbxweight" Text='<%#Bind("billed_weight") %>' Width="50px" Height="80%" runat="server" MaxLength="6"></asp:TextBox><cc2:FilteredTextBoxExtender ID="ftetbxweight" runat="server" TargetControlID="tbxweight" ValidChars="0123456789."></cc2:FilteredTextBoxExtender>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Price">
<ItemTemplate>
<asp:TextBox ID="tbxprice" Text='<%#Bind("price") %>' Width="50px" Height="80%" runat="server"></asp:TextBox><cc2:FilteredTextBoxExtender ID="ftetbxprice" runat="server" TargetControlID="tbxprice" ValidChars="0123456789."></cc2:FilteredTextBoxExtender>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="">
<ItemTemplate>
<asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_OnClick" /><asp:HiddenField ID="hfpurchase_entry_id" Value='<%#Bind("purchase_entry_id") %>' runat="server" />
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>--%>
</td>
</tr>
<tr>
<td>
<asp:GridView ID="gvvendorproducts1" runat="server" OnRowCancelingEdit="gvvendorproducts1_OnRowCancelingEdit" OnRowEditing="gvvendorproducts1_OnRowEditing" OnRowUpdating="gvvendorproducts1_OnRowUpdating">
<Columns>
<asp:TemplateField HeaderText="Sno">                   
    <ItemTemplate>
    <%#Container.DataItemIndex + 1 %><asp:HiddenField ID="hfpurchase_entry_id" Value='<%#Bind("purchase_entry_id") %>' runat="server" />
    </ItemTemplate>
    </asp:TemplateField>
<asp:BoundField DataField="productid" HeaderText="Product ID" ReadOnly="true" />
<asp:BoundField DataField="name" HeaderText="Name" ReadOnly="true" />
<asp:BoundField DataField="unit" HeaderText="Unit" ReadOnly="true" />
<asp:BoundField DataField="actual_weight" HeaderText="Actual Qty" ReadOnly="true" />
<asp:BoundField DataField="extended_weight" HeaderText="Purchase Qty" ReadOnly="true" />
<asp:BoundField DataField="inward_stock" HeaderText="Received Qty" ControlStyle-Width="50px" />
<asp:BoundField DataField="billed_weight" HeaderText="Billed Qty" ControlStyle-Width="50px" DataFormatString="{0:N2}" />
<asp:BoundField DataField="price" HeaderText="Price" ControlStyle-Width="50px" DataFormatString="{0:N2}" />
<%--<asp:TemplateField HeaderText="Billed Qty">
<ItemTemplate>
<asp:TextBox ID="tbxweight" Text='<%#Bind("billed_weight") %>' Width="50px" Height="80%" runat="server" MaxLength="6"></asp:TextBox><cc2:FilteredTextBoxExtender ID="ftetbxweight" runat="server" TargetControlID="tbxweight" ValidChars="0123456789."></cc2:FilteredTextBoxExtender>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Price">
<ItemTemplate>
<asp:TextBox ID="tbxprice" Text='<%#Bind("price") %>' Width="50px" Height="80%" runat="server"></asp:TextBox><cc2:FilteredTextBoxExtender ID="ftetbxprice" runat="server" TargetControlID="tbxprice" ValidChars="0123456789."></cc2:FilteredTextBoxExtender>
</ItemTemplate>
</asp:TemplateField>--%>
<asp:CommandField ShowEditButton="true" />
</Columns>
</asp:GridView>
</td>
</tr>
<tr>
<td>
<asp:GridView ID="gvmail" runat="server" EnableTheming="False">
<Columns>
<asp:BoundField DataField="productname" />
</Columns>
</asp:GridView>
</td>
</tr>
<%--
<tr>
<td>
<table>
<tr>
<td>Pid</td>
<td>Name</td>
<td>No. of Vendors</td>
</tr>
<tr>
<td><asp:DropDownList ID="ddlpid" OnSelectedIndexChanged="ddlpid_OnSelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="productid" DataValueField="name" AppendDataBoundItems="true"></asp:DropDownList></td>
<td><asp:Label ID="lblproduct" runat="server"></asp:Label> 
<td><asp:TextBox ID="tbxvendorcount" OnTextChanged="tbxvendorcount_OnTextChanged" AutoPostBack="true" runat="server" Width="20px"></asp:TextBox></td>
</tr>
</table>
</td>
</tr>
<tr>
<td>
<asp:GridView ID="grdvendorlist" runat="server">
<Columns>
<asp:TemplateField>
<ItemTemplate>
<table>
<tr>
<td>Sno.</td>
<td>Vendor Name</td>
<td>Purchase Qty</td>
<td>Purchase Price</td>
</tr>
<tr>
<td><asp:Label ID="lblid" runat="server" Text='<%#bind("id") %>' ></asp:Label></td>
<td><asp:ListBox ID="lstFruits" runat="server" SelectionMode="Multiple">
        <asp:ListItem Text="Vendor1" Value="1" />
        <asp:ListItem Text="Vendor2" Value="2" />
        <asp:ListItem Text="Vendor3" Value="3" />
        <asp:ListItem Text="Vendor4" Value="4" />
        <asp:ListItem Text="Vendor5" Value="5" />
    </asp:ListBox></td>
<td><asp:TextBox ID="tbxpurchaseweight" runat="server"></asp:TextBox></td>
<td><asp:TextBox ID="tbxpurchaseprice" runat="server"></asp:TextBox></td>
</tr>
</table>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</td>
</tr>
<tr>
<td><asp:GridView ID="gvpurchase" AutoGenerateColumns="false" runat="server" Width="100%">
<Columns>
<asp:TemplateField HeaderText="Vendor">
<ItemTemplate>
<asp:ListBox ID="lstFruits" runat="server" SelectionMode="Multiple">
        <asp:ListItem Text="Vendor1" Value="1" />
        <asp:ListItem Text="Vendor2" Value="2" />
        <asp:ListItem Text="Vendor3" Value="3" />
        <asp:ListItem Text="Vendor4" Value="4" />
        <asp:ListItem Text="Vendor5" Value="5" />
    </asp:ListBox>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField HeaderText="pid" DataField="product_id" />
<asp:BoundField HeaderText="Name" DataField="name" />
<asp:TemplateField HeaderText="Purchase Qty" ControlStyle-Width="60px" >
<ItemTemplate>
<asp:TextBox ID="tbxpurchaseweight" runat="server" Width="60px" MaxLength="7"></asp:TextBox><cc2:FilteredTextBoxExtender ID="ftetbxweight" runat="server" TargetControlID="tbxpurchaseweight" ValidChars="0123456789."></cc2:FilteredTextBoxExtender>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Purchase Price" ControlStyle-Width="60px">
<ItemTemplate>
<asp:TextBox ID="tbxpurchaseprice" runat="server" Width="60px"></asp:TextBox><cc2:FilteredTextBoxExtender ID="ftetbxprice" runat="server" TargetControlID="tbxpurchaseprice" ValidChars="0123456789."></cc2:FilteredTextBoxExtender>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField HeaderText="unit" DataField="unit" />
<asp:BoundField HeaderText="Sale_Price per kgpc" DataField="priceperkgpc" />

<asp:BoundField HeaderText="purchase date" DataField="date" DataFormatString="{0:MM-dd-yyyy}" />
</Columns>
</asp:GridView></td>
</tr>--%>
</table>

    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
