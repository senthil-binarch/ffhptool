<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true" CodeBehind="OtherPack.aspx.cs" Inherits="FFHPWeb.OtherPack" Theme="Skin1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table>
<tr>
    <td>
    <table>
    <tr>
    <td>Order No: </td>
    <td><asp:TextBox ID="txtidlist" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
        ErrorMessage="*" ControlToValidate="txtidlist" ValidationGroup="cal"></asp:RequiredFieldValidator></td>
    <td><asp:Button ID="btncalculate" Text="Calculate" OnClick="btncalculate_OnClick" runat="server" ValidationGroup="cal"/></td>
    </tr>
    </table>
    </td>
    </tr>
    <tr>
    <td>
    <asp:GridView ID="GvPackList" runat="server">
    <Columns>
    <asp:TemplateField HeaderText="Serial No">                   
    <ItemTemplate>
    <%#Container.DataItemIndex + 1 %>
    </ItemTemplate>
    </asp:TemplateField>
    <asp:BoundField DataField="Name" HeaderText="Name" />
<%--    <asp:BoundField DataField="Weight" HeaderText="Weight" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
    <asp:BoundField DataField="Pack" HeaderText="No. of Pack" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
--%>    <asp:BoundField DataField="TotalWeight" HeaderText="Total Weight" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
    </Columns>
    </asp:GridView>
    </td>
    </tr>
    <tr>
    <td>
    <asp:GridView ID="GvPackList2" runat="server">
    <Columns>
    <asp:TemplateField HeaderText="Serial No">                   
    <ItemTemplate>
    <%#Container.DataItemIndex + 1 %>
    </ItemTemplate>
    </asp:TemplateField>
    <asp:BoundField DataField="Name" HeaderText="Name" />
<%--    <asp:BoundField DataField="Weight" HeaderText="Weight" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
    <asp:BoundField DataField="Pack" HeaderText="No. of Pack" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
--%>    <asp:BoundField DataField="TotalWeight"  HtmlEncode="False" HeaderText="Total Weight" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
    </Columns>
    </asp:GridView>
    </td>
    </tr>
</table>
</asp:Content>
