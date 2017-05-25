<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true"
    CodeBehind="Routewise_Cutlist.aspx.cs" Inherits="FFHPWeb.Routewise_Cutlist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <tr>
            <td>
                <asp:GridView Width="50%" ID="GVRouteClear" runat="server" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField HeaderText="Route Id" DataField="route_id" />
                        <asp:BoundField HeaderText="Route Name" DataField="route_name" />
                        <asp:BoundField ItemStyle-Width="100px" HeaderText="Order Number" DataField="ordernumber" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btncutlist" Text="Get Cut List" runat="server" OnClick="btncutlist_OnClick" />
                                <asp:HiddenField ID="HFrouteid" Value='<%#bind("route_id") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
