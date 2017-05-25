<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true"
    CodeBehind="StockSale.aspx.cs" Inherits="FFHPWeb.StockSale" Theme="Skin1" %>

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
        });
    </script>

    <table width="100%" align="center">
        <tr>
            <td class="heading">
                Stock and Sale
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%">
                    <tr>
                        <td width="25%">
                            <asp:FileUpload ID="FUstocksale" runat="server" />
                        </td>
                        <td width="25%">
                            <asp:Button ID="Btnupload" Text="Upload" runat="server" OnClick="Btnupload_OnClick" />
                        </td>
                        <td align="right" width="50%">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            Process Date :
                        </td>
                        <td>
                            <asp:TextBox ID="TbxFromDate" Width="75px" runat="server"></asp:TextBox><asp:Image
                                ID="Image1" ImageUrl="~/Images/cal1.png" runat="server" /><cc2:CalendarExtender ID="CalendarExtender1"
                                    runat="server" Animated="true" ClearTime="true" DefaultView="Days" Format="MM/dd/yyyy"
                                    PopupPosition="BottomRight" TargetControlID="TbxFromDate" PopupButtonID="Image1">
                                </cc2:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                                ControlToValidate="TbxFromDate" ValidationGroup="Date"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Button ID="btnsubmit" Text="Submit" runat="server" OnClick="btnsubmit_OnClick" />
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
                            <tr bgcolor="#0071BD" style="background-color: #0071BD; font-weight: bold; color: White;">
                                <th scope="col">
                                    Product Id
                                </th>
                                <th scope="col">
                                    Name
                                </th>
                                <th scope="col">
                                    Opening Wgt
                                </th>
                                <th scope="col">
                                    Opening Pc
                                </th>
                                <th scope="col">
                                    Morning Wt
                                </th>
                                <th scope="col">
                                    Morning Pc
                                </th>
                                <th scope="col">
                                    Localpurchase Wt
                                </th>
                                <th scope="col">
                                    Localpurchase Pc
                                </th>
                                <th scope="col">
                                    Online Wt
                                </th>
                                <th scope="col">
                                    Online Pc
                                </th>
                                <th scope="col">
                                    Balance Wt
                                </th>
                                <th scope="col">
                                    Balance Pc
                                </th>
                                <th scope="col">
                                    Localsale Wt
                                </th>
                                <th scope="col">
                                    Localsale Pc
                                </th>
                                <th scope="col">
                                    Aftersale Wt
                                </th>
                                <th scope="col">
                                    Aftersale Pc
                                </th>
                                <th scope="col">
                                    Missed Wt
                                </th>
                                <th scope="col">
                                    Missed Pc
                                </th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblproductid" runat="server" Text='<%# Eval("productid") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblname" runat="server" Text='<%# Eval("name") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblopeningweight" runat="server" Text='<%# Eval("Openingweight") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblopeningpiece" runat="server" Text='<%# Eval("Openingpiece") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblmorningscannedweight" runat="server" Text='<%# Eval("morningscannedweight") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblmorningpiececount" runat="server" Text='<%# Eval("morningpiececount") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lbllocalpurchasescannedweight" runat="server" Text='<%# Eval("localpurchasescannedweight") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lbllocalpurchasepiececount" runat="server" Text='<%# Eval("localpurchasepiececount") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblonlinescannedweight" runat="server" Text='<%# Eval("onlinescannedweight") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblonlinescannedpiece" runat="server" Text='<%# Eval("onlinescannedpiece") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblbalancescannedweight" runat="server" Text='<%# Eval("balancescannedweight") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblbalancepiececount" runat="server" Text='<%# Eval("balancepiececount") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lbllocalsalescannedweight" runat="server" Text='<%# Eval("localsalescannedweight") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="localsalepiececount" runat="server" Text='<%# Eval("localsalepiececount") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblaftersalescannedweight" runat="server" Text='<%# Eval("aftersalescannedweight") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblaftersalepiececount" runat="server" Text='<%# Eval("aftersalepiececount") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblmissedweight" runat="server" Text='<%# Eval("missedweight") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblmissedpiece" runat="server" Text='<%# Eval("missedpiece") %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblproductid" runat="server" Text='<%# Eval("productid") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblname" runat="server" Text='<%# Eval("name") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblopeningweight" runat="server" Text='<%# Eval("Openingweight") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblopeningpiece" runat="server" Text='<%# Eval("Openingpiece") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblmorningscannedweight" runat="server" Text='<%# Eval("morningscannedweight") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblmorningpiececount" runat="server" Text='<%# Eval("morningpiececount") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lbllocalpurchasescannedweight" runat="server" Text='<%# Eval("localpurchasescannedweight") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lbllocalpurchasepiececount" runat="server" Text='<%# Eval("localpurchasepiececount") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblonlinescannedweight" runat="server" Text='<%# Eval("onlinescannedweight") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblonlinescannedpiece" runat="server" Text='<%# Eval("onlinescannedpiece") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblbalancescannedweight" runat="server" Text='<%# Eval("balancescannedweight") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblbalancepiececount" runat="server" Text='<%# Eval("balancepiececount") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lbllocalsalescannedweight" runat="server" Text='<%# Eval("localsalescannedweight") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="localsalepiececount" runat="server" Text='<%# Eval("localsalepiececount") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblaftersalescannedweight" runat="server" Text='<%# Eval("aftersalescannedweight") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblaftersalepiececount" runat="server" Text='<%# Eval("aftersalepiececount") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblmissedweight" runat="server" Text='<%# Eval("missedweight") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblmissedpiece" runat="server" Text='<%# Eval("missedpiece") %>' />
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </td>
        </tr>
        <tr>
            <td class="heading">
                Mismatch List
            </td>
        </tr>
        <tr>
            <td>
                <asp:Repeater ID="rptstocksale_mismatch" runat="server">
                    <HeaderTemplate>
                        <table id="Table2" cellspacing="0" rules="all" border="1">
                            <tr bgcolor="#0071BD" style="background-color: #0071BD; font-weight: bold; color: White;">
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
                                    Missed Wt
                                </th>
                                <th scope="col">
                                    Missed Pc
                                </th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <%#Container.ItemIndex+1 %>
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblproductid" runat="server" Text='<%# Eval("productid") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblname" runat="server" Text='<%# Eval("name") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblmissedweight" runat="server" Text='<%# Eval("missedweight") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblmissedpiece" runat="server" Text='<%# Eval("missedpiece") %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <%#Container.ItemIndex+1 %>
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblproductid" runat="server" Text='<%# Eval("productid") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblname" runat="server" Text='<%# Eval("name") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblmissedweight" runat="server" Text='<%# Eval("missedweight") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblmissedpiece" runat="server" Text='<%# Eval("missedpiece") %>' />
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </td>
        </tr>
        <tr>
        <td><asp:Button ID="btnsendemail" Text="Send Email" runat="server" OnClick="btnsendemail_OnClick" Visible="false" /></td>
        </tr>
    </table>
</asp:Content>
