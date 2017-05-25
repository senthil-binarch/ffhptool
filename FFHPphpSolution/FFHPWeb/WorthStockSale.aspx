<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true" CodeBehind="WorthStockSale.aspx.cs" Inherits="FFHPWeb.WorthStockSale" Theme="Skin1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script src="Scripts/ScrollableTablePlugin_1.0_min.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function() {
        $('#Table1').Scrollable({
            ScrollHeight: 350
        });

        $('#Table2').Scrollable({
            ScrollHeight: 350
        });

        $('#Table3').Scrollable({
            ScrollHeight: 350
        });

        $('#Table4').Scrollable({
            ScrollHeight: 350
        });
    });
</script>

<table width="100%" align="center">
<tr>
<td class="heading">Worth of various quantity</td>
</tr>
<tr>
<td>
<table>
<tr>
<td>Process Date :</td>
<td> <asp:TextBox ID="TbxFromDate" Width="75px" runat="server" ></asp:TextBox><asp:Image
                                ID="Image1" ImageUrl="~/Images/cal1.png" runat="server" /><cc2:CalendarExtender ID="CalendarExtender1"
                                    runat="server" Animated="true" ClearTime="true" DefaultView="Days" Format="MM/dd/yyyy"
                                    PopupPosition="BottomRight" TargetControlID="TbxFromDate" PopupButtonID="Image1">
                                </cc2:CalendarExtender><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
        ErrorMessage="*" ControlToValidate="TbxFromDate" 
        ValidationGroup="Date"></asp:RequiredFieldValidator></td>
<td><asp:Button ID="btnsubmit" Text="Submit" runat="server" OnClick="btnsubmit_OnClick" /></td>
</tr>
</table>
</td>
</tr>
<tr>
<td><asp:DropDownList ID="ddlworthtype" Visible="false" runat="server" OnSelectedIndexChanged="ddlworthtype_OnSelectedIndexChanged" AutoPostBack="true">
<asp:ListItem Text="All" Value="0"></asp:ListItem>
<asp:ListItem Text="Incoming" Value="1"></asp:ListItem>
<asp:ListItem Text="Local Sale" Value="2"></asp:ListItem>
<asp:ListItem Text="Balance" Value="3"></asp:ListItem>
</asp:DropDownList></td>
</tr>
<tr>
<td><asp:Label ID="lblerror" runat="server"></asp:Label></td>
</tr>
<%--<tr>
<td>
<asp:GridView ID="gvstocksale" runat="server" HeaderStyle-CssClass="HeaderFreez">
<Columns>
<asp:BoundField DataField="productid" HeaderText="Product Id" />
<asp:BoundField DataField="name" HeaderText="Name" />
<asp:BoundField DataField="Openingweight" HeaderText="Opening Wgt" />
<asp:BoundField DataField="Openingpiece" HeaderText="Opening Pc" />
<asp:BoundField DataField="morningscannedweight" HeaderText="Morning Wgt" />
<asp:BoundField DataField="morningpiececount" HeaderText="Morning Pc" />
<asp:BoundField DataField="onlinescannedweight" HeaderText="Online Wgt" />
<asp:BoundField DataField="onlinescannedpiece" HeaderText="Online Pc" />
<asp:BoundField DataField="balancescannedweight" HeaderText="Balance Wgt" />
<asp:BoundField DataField="balancepiececount" HeaderText="Balance Pc" />
<asp:BoundField DataField="localsalescannedweight" HeaderText="Localsale Wgt" />
<asp:BoundField DataField="localsalepiececount" HeaderText="Localsale Pc" />
<asp:BoundField DataField="aftersalescannedweight" HeaderText="Aftersale Wgt" />
<asp:BoundField DataField="aftersalepiececount" HeaderText="Aftersale Pc" />
<asp:BoundField DataField="missedweight" HeaderText="Missed Wgt" />
<asp:BoundField DataField="missedpiece" HeaderText="Missed Pc" />
</Columns>
</asp:GridView>
</td>
</tr>--%>
<tr>
<td>

<asp:Repeater ID="rptstocksale" runat="server">
    <HeaderTemplate>
        <table id="Table1" cellspacing="0" rules="all" border="1">
            <%--<tr bgcolor="#0071BD" style="background-color:#0071BD;font-weight:bold;color:White;">
            <th scope="col"></th>
            <th scope="col"></th>
            <th scope="col"></th>
            <th scope="col"></th>
            <th scope="col" colspan="3">Opening Stock</th>
            <th scope="col" colspan="3">instock wt and price</th>
            <th scope="col" colspan="3">Extened wt and price</th>
            <th scope="col" colspan="3">Market purchase wt and price</th>
            <th scope="col" colspan="3">Local purchase wt and price</th>
            <th scope="col" colspan="3">Online wt for purchase price</th>
            <th scope="col" colspan="3">Online wt for selling price</th>
            <th scope="col" colspan="3">Balance wt and price</th>
            <th scope="col" colspan="3">Local Sale wt and price</th>
            </tr>--%>
            <tr bgcolor="#0071BD" style="background-color:#0071BD;font-weight:bold;color:White;">
                <th scope="col">
                    S.No.
                </th>
                <th scope="col">
                    Product Id
                </th>
                <th scope="col">
                    Name
                </th>
                <th scope="col">
                    Purchase Unit
                </th>
                <th scope="col">
                    Online Sales Per Unit
                </th>
                <th scope="col">
                    Stock Wt
                </th>
                <th scope="col">
                    Stock Purchase Price
                </th>
                <th scope="col">
                    Stock Total
                </th>
                <th scope="col">
                    Instock Wt
                </th>
                <th scope="col">
                    Instock Purchase Price
                </th>
                <th scope="col">
                    Instock Total
                </th>
                <th scope="col">
                    Extended Wt
                </th>
                <th scope="col">
                    Extended Purchase Price
                </th>
                <th scope="col">
                    Extended Total
                </th>
                <th scope="col">
                    Market Purchase
                </th>
                <th scope="col">
                    Market Purchase Price
                </th>
                <th scope="col">
                    Market Purchase Total
                </th>
                <th scope="col">
                    Local Purchase Wt
                </th>
                <th scope="col">
                    Local Purchase Price
                </th>
                <th scope="col">
                    Local Purchase Total
                </th>
                <th scope="col">
                    Online Scanned Wt
                </th>
                <th scope="col">
                    Online Scanned Purchase Price
                </th>
                <th scope="col">
                    Online Scanned Total
                </th>
                <th scope="col">
                    Online Scanned Selling weight
                </th>
                <th scope="col">
                    Online Scanned Selling Price
                </th>
                <th scope="col">
                    Online Scanned Total
                </th>
                <th scope="col">
                    Balance Wt
                </th>
                <th scope="col">
                    Balance purchase Price
                </th>
                <th scope="col">
                    Balance Total
                </th>
                <th scope="col">
                    Local Sale Wt
                </th>
                <th scope="col">
                    Local Sale Purchase Price
                </th>
                <th scope="col">
                    Local Sale Total
                </th>
                <%--<th scope="col">
                    After Sale Wt
                </th>
                <th scope="col">
                    After Sale Purchase Price
                </th>
                <th scope="col">
                    After Sale Total
                </th>
                <th scope="col">
                    Missed Wt
                </th>
                <th scope="col">
                    Missed Purchase Price
                </th>
                <th scope="col">
                    Missed Total
                </th>--%>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td style="background-color:#F7F7DE;color:#212463;">
                <asp:Label ID="lblsno" runat="server" Text='<%#(((RepeaterItem)Container).ItemIndex+1).ToString()%>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
                <asp:Label ID="lblproductid" runat="server" Text='<%# Eval("productid") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
                <asp:Label ID="lblname" runat="server" Text='<%# Eval("name") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblopeningweight" runat="server" Text='<%# Eval("purchaseunit") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblopeningpiece" runat="server" Text='<%# Eval("onlinesalesperunit") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblmorningscannedweight" runat="server" Text='<%# Eval("stockweight") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblmorningpiececount" runat="server" Text='<%# Eval("stockpurchaseprice") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lbllocalpurchasescannedweight" runat="server" Text='<%# Eval("stocktotal") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lbllocalpurchasepiececount" runat="server" Text='<%# Eval("instockweight") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblonlinescannedweight" runat="server" Text='<%# Eval("instockpurchaseprice") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblonlinescannedpiece" runat="server" Text='<%# Eval("instocktotal") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblbalancescannedweight" runat="server" Text='<%# Eval("extendedweight") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblbalancepiececount" runat="server" Text='<%# Eval("extendedpurchaseprice") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lbllocalsalescannedweight" runat="server" Text='<%# Eval("extendedtotal") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="localsalepiececount" runat="server" Text='<%# Eval("marketpurchase") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblaftersalescannedweight" runat="server" Text='<%# Eval("marketpurchaseprice") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblaftersalepiececount" runat="server" Text='<%# Eval("marketpurchasetotal") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblmissedweight" runat="server" Text='<%# Eval("localpurchaseweight") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblmissedpiece" runat="server" Text='<%# Eval("localpurchaseprice") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label1" runat="server" Text='<%# Eval("localpurchasetotal") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label2" runat="server" Text='<%# Eval("onlinescannedweight") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label3" runat="server" Text='<%# Eval("onlinescannedpurchaseprice") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label4" runat="server" Text='<%# Eval("onlinescannedtotal") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label5" runat="server" Text='<%# Eval("onlinescannedsellingweight") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label6" runat="server" Text='<%# Eval("onlinescannedsellingprice") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label7" runat="server" Text='<%# Eval("onlinescannedsellingtotal") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label8" runat="server" Text='<%# Eval("balanceweight") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label9" runat="server" Text='<%# Eval("balancepurchaseprice") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label10" runat="server" Text='<%# Eval("balancetotal") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label11" runat="server" Text='<%# Eval("localsaleweight") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label12" runat="server" Text='<%# Eval("localsalepurchaseprice") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label13" runat="server" Text='<%# Eval("localsaletotal") %>' />
            </td>
            <%--<td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label14" runat="server" Text='<%# Eval("aftersaleweight") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label15" runat="server" Text='<%# Eval("aftersalepurchaseprice") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label16" runat="server" Text='<%# Eval("aftersaletotal") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label17" runat="server" Text='<%# Eval("missedweight") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label18" runat="server" Text='<%# Eval("missedpurchaseprice") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label19" runat="server" Text='<%# Eval("missedtotal") %>' />
            </td>--%>
        </tr>
    </ItemTemplate>
    <AlternatingItemTemplate>
                            <tr>
            <td style="background-color:#F7F7DE;color:#212463;">
                <asp:Label ID="lblsno" runat="server" Text='<%#(((RepeaterItem)Container).ItemIndex+1).ToString()%>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
                <asp:Label ID="lblproductid" runat="server" Text='<%# Eval("productid") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
                <asp:Label ID="lblname" runat="server" Text='<%# Eval("name") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblopeningweight" runat="server" Text='<%# Eval("purchaseunit") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblopeningpiece" runat="server" Text='<%# Eval("onlinesalesperunit") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblmorningscannedweight" runat="server" Text='<%# Eval("stockweight") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblmorningpiececount" runat="server" Text='<%# Eval("stockpurchaseprice") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lbllocalpurchasescannedweight" runat="server" Text='<%# Eval("stocktotal") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lbllocalpurchasepiececount" runat="server" Text='<%# Eval("instockweight") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblonlinescannedweight" runat="server" Text='<%# Eval("instockpurchaseprice") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblonlinescannedpiece" runat="server" Text='<%# Eval("instocktotal") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblbalancescannedweight" runat="server" Text='<%# Eval("extendedweight") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblbalancepiececount" runat="server" Text='<%# Eval("extendedpurchaseprice") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lbllocalsalescannedweight" runat="server" Text='<%# Eval("extendedtotal") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="localsalepiececount" runat="server" Text='<%# Eval("marketpurchase") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblaftersalescannedweight" runat="server" Text='<%# Eval("marketpurchaseprice") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblaftersalepiececount" runat="server" Text='<%# Eval("marketpurchasetotal") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblmissedweight" runat="server" Text='<%# Eval("localpurchaseweight") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblmissedpiece" runat="server" Text='<%# Eval("localpurchaseprice") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label1" runat="server" Text='<%# Eval("localpurchasetotal") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label2" runat="server" Text='<%# Eval("onlinescannedweight") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label3" runat="server" Text='<%# Eval("onlinescannedpurchaseprice") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label4" runat="server" Text='<%# Eval("onlinescannedtotal") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label5" runat="server" Text='<%# Eval("onlinescannedsellingweight") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label6" runat="server" Text='<%# Eval("onlinescannedsellingprice") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label7" runat="server" Text='<%# Eval("onlinescannedsellingtotal") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label8" runat="server" Text='<%# Eval("balanceweight") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label9" runat="server" Text='<%# Eval("balancepurchaseprice") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label10" runat="server" Text='<%# Eval("balancetotal") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label11" runat="server" Text='<%# Eval("localsaleweight") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label12" runat="server" Text='<%# Eval("localsalepurchaseprice") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label13" runat="server" Text='<%# Eval("localsaletotal") %>' />
            </td>
            <%--<td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label14" runat="server" Text='<%# Eval("aftersaleweight") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label15" runat="server" Text='<%# Eval("aftersalepurchaseprice") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label16" runat="server" Text='<%# Eval("aftersaletotal") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label17" runat="server" Text='<%# Eval("missedweight") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label18" runat="server" Text='<%# Eval("missedpurchaseprice") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label19" runat="server" Text='<%# Eval("missedtotal") %>' />
            </td>--%>
        </tr>
    </AlternatingItemTemplate>
    <FooterTemplate>
        </table>
         <asp:Label ID="lblEmptyData" runat="server" Visible='<%# ((Repeater)Container.NamingContainer).Items.Count == 0 %>' Text="No items found" />
    </FooterTemplate>
</asp:Repeater>

</td>
</tr>
<tr>
<td>
<asp:Repeater ID="rptstocksale_incoming" runat="server">
    <HeaderTemplate>
        <table id="Table2" cellspacing="0" rules="all" border="1">
            <%--<tr bgcolor="#0071BD" style="background-color:#0071BD;font-weight:bold;color:White;">
            <th scope="col"></th>
            <th scope="col"></th>
            <th scope="col"></th>
            <th scope="col"></th>
            <th scope="col" colspan="3">Opening Stock</th>
            <th scope="col" colspan="3">instock wt and price</th>
            <th scope="col" colspan="3">Extened wt and price</th>
            <th scope="col" colspan="3">Market purchase wt and price</th>
            <th scope="col" colspan="3">Local purchase wt and price</th>
            <th scope="col" colspan="3">Online wt for purchase price</th>
            <th scope="col" colspan="3">Online wt for selling price</th>
            <th scope="col" colspan="3">Balance wt and price</th>
            <th scope="col" colspan="3">Local Sale wt and price</th>
            </tr>--%>
            <tr bgcolor="#0071BD" style="background-color:#0071BD;font-weight:bold;color:White;">
                <th scope="col">
                    S.No.
                </th>
                <th scope="col">
                    Product Id
                </th>
                <th scope="col">
                    Name
                </th>
                <th scope="col">
                    Purchase Unit
                </th>
                <th scope="col">
                    Instock Wt
                </th>
                <th scope="col">
                    Instock Purchase Price
                </th>
                <th scope="col">
                    Instock Total
                </th>
                <th scope="col">
                    Group
                </th>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
        <td style="background-color:#F7F7DE;color:#212463;">
                <asp:Label ID="lblsno" runat="server" Text='<%#(((RepeaterItem)Container).ItemIndex+1).ToString()%>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
                <asp:Label ID="lblproductid" runat="server" Text='<%# Eval("productid") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
                <asp:Label ID="lblname" runat="server" Text='<%# Eval("name") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblopeningweight" runat="server" Text='<%# Eval("purchaseunit") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lbllocalpurchasepiececount" runat="server" Text='<%# Eval("instockweight") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblonlinescannedweight" runat="server" Text='<%# Eval("instockpurchaseprice") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblonlinescannedpiece" runat="server" Text='<%# Eval("instocktotal") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblgroup" runat="server" Text='<%# Eval("group") %>' />
            </td>
        </tr>
    </ItemTemplate>
    <AlternatingItemTemplate>
                            <tr>
            <td style="background-color:#F7F7DE;color:#212463;">
                <asp:Label ID="lblsno" runat="server" Text='<%#(((RepeaterItem)Container).ItemIndex+1).ToString()%>' />
            </td>      
            <td style="background-color:#F7F7DE;color:#212463;">
                <asp:Label ID="lblproductid" runat="server" Text='<%# Eval("productid") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
                <asp:Label ID="lblname" runat="server" Text='<%# Eval("name") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblopeningweight" runat="server" Text='<%# Eval("purchaseunit") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lbllocalpurchasepiececount" runat="server" Text='<%# Eval("instockweight") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblonlinescannedweight" runat="server" Text='<%# Eval("instockpurchaseprice") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblonlinescannedpiece" runat="server" Text='<%# Eval("instocktotal") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblgroup" runat="server" Text='<%# Eval("group") %>' />
            </td>
        </tr>
    </AlternatingItemTemplate>
    <FooterTemplate>
        </table>
         <asp:Label ID="lblEmptyData" runat="server" Visible='<%# ((Repeater)Container.NamingContainer).Items.Count == 0 %>' Text="No items found" />
    </FooterTemplate>
</asp:Repeater>
</td>
</tr>
<tr>
<td>
<asp:Repeater ID="rptstocksale_local" runat="server">
    <HeaderTemplate>
        <table id="Table3" cellspacing="0" rules="all" border="1">
            <%--<tr bgcolor="#0071BD" style="background-color:#0071BD;font-weight:bold;color:White;">
            <th scope="col"></th>
            <th scope="col"></th>
            <th scope="col"></th>
            <th scope="col"></th>
            <th scope="col" colspan="3">Opening Stock</th>
            <th scope="col" colspan="3">instock wt and price</th>
            <th scope="col" colspan="3">Extened wt and price</th>
            <th scope="col" colspan="3">Market purchase wt and price</th>
            <th scope="col" colspan="3">Local purchase wt and price</th>
            <th scope="col" colspan="3">Online wt for purchase price</th>
            <th scope="col" colspan="3">Online wt for selling price</th>
            <th scope="col" colspan="3">Balance wt and price</th>
            <th scope="col" colspan="3">Local Sale wt and price</th>
            </tr>--%>
            <tr bgcolor="#0071BD" style="background-color:#0071BD;font-weight:bold;color:White;">
                <th scope="col">
                    S.No.
                </th>
                <th scope="col">
                    Product Id
                </th>
                <th scope="col">
                    Name
                </th>
                <th scope="col">
                    Purchase Unit
                </th>
                <th scope="col">
                    Local Sale Wt
                </th>
                <th scope="col">
                    Local Sale Purchase Price
                </th>
                <th scope="col">
                    Local Sale Total
                </th>
                <th scope="col">
                    Group
                </th>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td style="background-color:#F7F7DE;color:#212463;">
                <asp:Label ID="lblsno" runat="server" Text='<%#(((RepeaterItem)Container).ItemIndex+1).ToString()%>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
                <asp:Label ID="lblproductid" runat="server" Text='<%# Eval("productid") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
                <asp:Label ID="lblname" runat="server" Text='<%# Eval("name") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblopeningweight" runat="server" Text='<%# Eval("purchaseunit") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label11" runat="server" Text='<%# Eval("localsaleweight") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label12" runat="server" Text='<%# Eval("localsalepurchaseprice") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label13" runat="server" Text='<%# Eval("localsaletotal") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label14" runat="server" Text='<%# Eval("group") %>' />
            </td>
        </tr>
    </ItemTemplate>
    <AlternatingItemTemplate>
                            <tr>
            <td style="background-color:#F7F7DE;color:#212463;">
                <asp:Label ID="lblsno" runat="server" Text='<%#(((RepeaterItem)Container).ItemIndex+1).ToString()%>' />
            </td>        
            <td style="background-color:#F7F7DE;color:#212463;">
                <asp:Label ID="lblproductid" runat="server" Text='<%# Eval("productid") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
                <asp:Label ID="lblname" runat="server" Text='<%# Eval("name") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblopeningweight" runat="server" Text='<%# Eval("purchaseunit") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label11" runat="server" Text='<%# Eval("localsaleweight") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label12" runat="server" Text='<%# Eval("localsalepurchaseprice") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label13" runat="server" Text='<%# Eval("localsaletotal") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label14" runat="server" Text='<%# Eval("group") %>' />
            </td>
        </tr>
    </AlternatingItemTemplate>
    <FooterTemplate>
        </table>
         <asp:Label ID="lblEmptyData" runat="server" Visible='<%# ((Repeater)Container.NamingContainer).Items.Count == 0 %>' Text="No items found" />
    </FooterTemplate>
</asp:Repeater>
</td>
</tr>
<tr>
<td>
<asp:Repeater ID="rptstocksale_balance" runat="server">
    <HeaderTemplate>
        <table id="Table4" cellspacing="0" rules="all" border="1">
            <%--<tr bgcolor="#0071BD" style="background-color:#0071BD;font-weight:bold;color:White;">
            <th scope="col"></th>
            <th scope="col"></th>
            <th scope="col"></th>
            <th scope="col"></th>
            <th scope="col" colspan="3">Opening Stock</th>
            <th scope="col" colspan="3">instock wt and price</th>
            <th scope="col" colspan="3">Extened wt and price</th>
            <th scope="col" colspan="3">Market purchase wt and price</th>
            <th scope="col" colspan="3">Local purchase wt and price</th>
            <th scope="col" colspan="3">Online wt for purchase price</th>
            <th scope="col" colspan="3">Online wt for selling price</th>
            <th scope="col" colspan="3">Balance wt and price</th>
            <th scope="col" colspan="3">Local Sale wt and price</th>
            </tr>--%>
            <tr bgcolor="#0071BD" style="background-color:#0071BD;font-weight:bold;color:White;">
                <th scope="col">
                    S.No.
                </th>
                <th scope="col">
                    Product Id
                </th>
                <th scope="col">
                    Name
                </th>
                <th scope="col">
                    Purchase Unit
                </th>
                <th scope="col">
                    Balance Wt
                </th>
                <th scope="col">
                    Balance purchase Price
                </th>
                <th scope="col">
                    Balance Total
                </th>
                <th scope="col">
                    Group
                </th>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td style="background-color:#F7F7DE;color:#212463;">
                <asp:Label ID="lblsno" runat="server" Text='<%#(((RepeaterItem)Container).ItemIndex+1).ToString()%>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
                <asp:Label ID="lblproductid" runat="server" Text='<%# Eval("productid") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
                <asp:Label ID="lblname" runat="server" Text='<%# Eval("name") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblopeningweight" runat="server" Text='<%# Eval("purchaseunit") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label8" runat="server" Text='<%# Eval("balanceweight") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label9" runat="server" Text='<%# Eval("balancepurchaseprice") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label10" runat="server" Text='<%# Eval("balancetotal") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label15" runat="server" Text='<%# Eval("group") %>' />
            </td>
        </tr>
    </ItemTemplate>
    <AlternatingItemTemplate>
                            <tr>
            <td style="background-color:#F7F7DE;color:#212463;">
                <asp:Label ID="lblsno" runat="server" Text='<%#(((RepeaterItem)Container).ItemIndex+1).ToString()%>' />
            </td>           
            <td style="background-color:#F7F7DE;color:#212463;">
                <asp:Label ID="lblproductid" runat="server" Text='<%# Eval("productid") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
                <asp:Label ID="lblname" runat="server" Text='<%# Eval("name") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="lblopeningweight" runat="server" Text='<%# Eval("purchaseunit") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label8" runat="server" Text='<%# Eval("balanceweight") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label9" runat="server" Text='<%# Eval("balancepurchaseprice") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label10" runat="server" Text='<%# Eval("balancetotal") %>' />
            </td>
            <td style="background-color:#F7F7DE;color:#212463;">
               <asp:Label ID="Label15" runat="server" Text='<%# Eval("group") %>' />
            </td>
        </tr>
    </AlternatingItemTemplate>
    <FooterTemplate>
        </table>
         <asp:Label ID="lblEmptyData" runat="server" Visible='<%# ((Repeater)Container.NamingContainer).Items.Count == 0 %>' Text="No items found" />
    </FooterTemplate>
</asp:Repeater>
</td>
</tr>
<tr>
<td><asp:Button ID="btnsendemail" Text="Send Email" runat="server" OnClick="btnsendemail_OnClick" Visible="false" /></td>
</tr>
</table>
</asp:Content>
