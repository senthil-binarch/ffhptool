<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true"
    CodeBehind="stockreport.aspx.cs" Inherits="FFHPWeb.stockreport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

    <script src="Scripts/ScrollableTablePlugin_1.0_min.js" type="text/javascript"></script>

    <script type="text/javascript">
    $(function() {
    $('#Table1').Scrollable({
    ScrollHeight: 350,
        ScrollWidth: 350
    });
    $('#Table2').Scrollable({
            ScrollHeight: 350,
            ScrollWidth: 350
        });
        $('#Table3').Scrollable({
            ScrollHeight: 350,
            ScrollWidth: 350
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
        <tr>
            <td class="heading">
                Morning not received products
            </td>
        </tr>
        <tr>
            <td>
                <asp:Repeater ID="rptstocksale_notbuyed" runat="server">
                    <HeaderTemplate>
                        <table id="Table1" cellspacing="0" rules="all" border="1">
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
                                    Unit
                                </th>
                                <th scope="col">
                                    Actual Weight
                                </th>
                                <th scope="col">
                                    Purchase Weight
                                </th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <%#Container.ItemIndex+1 %>
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblproductid" runat="server" Text='<%# Eval("pid") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblname" runat="server" Text='<%# Eval("name") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("units") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblopeningweight" runat="server" Text='<%# Eval("actualweight") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblopeningpiece" runat="server" Text='<%# Eval("purchaseweight") %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <%#Container.ItemIndex+1 %>
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblproductid" runat="server" Text='<%# Eval("pid") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblname" runat="server" Text='<%# Eval("name") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblopeningweight" runat="server" Text='<%# Eval("units") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblopeningpiece" runat="server" Text='<%# Eval("actualweight") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblmorningscannedweight" runat="server" Text='<%# Eval("purchaseweight") %>' />
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
                Morning received products with weight difference
            </td>
        </tr>
        <tr>
            <td>
                <asp:Repeater ID="rptstocksale_missing" runat="server">
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
                                    Unit
                                </th>
                                <th scope="col">
                                    Actual Weight
                                </th>
                                <th scope="col">
                                    Purchase Weight
                                </th>
                                <th scope="col">
                                    Morning Weight
                                </th>
                                <th scope="col">
                                    Piece Count
                                </th>
                                <th scope="col">
                                    Morning Tray Weight
                                </th>
                                <th scope="col">
                                    Weight Variation
                                </th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <%#Container.ItemIndex+1 %>
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblproductid" runat="server" Text='<%# Eval("pid") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblname" runat="server" Text='<%# Eval("name") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("units") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblopeningweight" runat="server" Text='<%# Eval("actualweight") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblopeningpiece" runat="server" Text='<%# Eval("purchaseweight") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("morningscannedweight") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("morningpiececount") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="Label4" runat="server" Text='<%# Eval("morningtrayweight") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="Label5" runat="server" Text='<%# Eval("weightvariation") %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <%#Container.ItemIndex+1 %>
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblproductid" runat="server" Text='<%# Eval("pid") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblname" runat="server" Text='<%# Eval("name") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblopeningweight" runat="server" Text='<%# Eval("units") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblopeningpiece" runat="server" Text='<%# Eval("actualweight") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblmorningscannedweight" runat="server" Text='<%# Eval("purchaseweight") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("morningscannedweight") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("morningpiececount") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="Label4" runat="server" Text='<%# Eval("morningtrayweight") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="Label5" runat="server" Text='<%# Eval("weightvariation") %>' />
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
                Morning received not required products
            </td>
        </tr>
        <tr>
            <td>
                <asp:Repeater ID="rptstocksale_unwanted" runat="server">
                    <HeaderTemplate>
                        <table id="Table3" cellspacing="0" rules="all" border="1">
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
                                    Morning Weight
                                </th>
                                <th scope="col">
                                    Piece Count
                                </th>
                                <th scope="col">
                                    Morning Tray Weight
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
                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("morningscannedweight") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("morningpiececount") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="Label4" runat="server" Text='<%# Eval("morningtrayweight") %>' />
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
                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("morningscannedweight") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("morningpiececount") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="Label4" runat="server" Text='<%# Eval("morningtrayweight") %>' />
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
