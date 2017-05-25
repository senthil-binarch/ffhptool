<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true"
    CodeBehind="partner_dashboard.aspx.cs" Inherits="FFHPWeb.partner_dashboard" Theme="Skin1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%">
        <tr>
            <td class="heading">
                Partners Dashboard
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            Parter
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlpartner" Width="180px" DataTextField="name" DataValueField="ffhp_partner_id"
                                AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlpartner_OnSelectedIndexChanged"
                                runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Coupon
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcoupon" Width="180px" DataTextField="coupon_code" DataValueField="ffhp_partner_coupon_id"
                                AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlcoupon_OnSelectedIndexChanged"
                                runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
        <td>
        <table>
        <tr>
        <td><asp:Button ID="btnlastweek" Text="Last Week" runat="server" /></td>
        <td><asp:Button ID="btnlastmonth" Text="Last Month" runat="server" /></td>
        <td><asp:Button ID="btnhistory" Text="History" runat="server" /></td>
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
                <asp:GridView ID="gvpartnercoupon" runat="server">
                    <Columns>
                        <asp:TemplateField HeaderText="Sno">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="customer_id" HeaderText="Custome Id" />
                        <asp:BoundField DataField="customer_firstname" HeaderText="Firstname" />
                        <asp:BoundField DataField="customer_lastname" HeaderText="Lastname" />
                        <asp:BoundField DataField="customer_email" HeaderText="Email" />
                        <asp:BoundField DataField="coupon_code" HeaderText="Coupon" />
                        <asp:BoundField DataField="base_grand_total" HeaderText="Order value" />
                        <asp:BoundField DataField="increment_id" HeaderText="Order number" />
                        <asp:BoundField DataField="created_at" HeaderText="Created Date" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
