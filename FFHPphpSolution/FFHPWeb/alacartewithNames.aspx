<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true" CodeBehind="alacartewithNames.aspx.cs" Inherits="FFHPWeb.alacartewithNames" Theme="Skin1"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" ChildrenAsTriggers="True">
                        <ContentTemplate>
                            <div>
<table width="100%">
<tr>
<td class="lblheading">Alacarte with names</td>
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
    <asp:GridView ID="GVOrdernumbers" runat="server" EnableTheming="true" >
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
<td><asp:Button ID="btncalculate" Text="Submit" OnClick="btncalculate_OnClick" runat="server"   /><%--ValidationGroup="cal"--%></td>
<%--<td><asp:Button ID="btnsendexcel" Text="Send Mail" Visible="false" runat="server" OnClick="btnsendexcel_OnClick" /></td>--%>
<td><asp:ImageButton ID="btnPDF" runat="server" Visible="false" ToolTip="Download" ImageUrl="~/Images/PDF.jpg" Width="32px" 
Height="32px" onclick="btnPDF_Click"/><asp:ImageButton ID="btnXLS" runat="server" ToolTip="Download" Visible="false" ImageUrl="~/Images/xls-2.png" Width="32px" 
Height="32px" onclick="btnXLS_Click"/></td>
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
    <td><%--BorderStyle="None" GridLines="None"--%>
    
    <asp:GridView ID="GVOrderDetails" EnableTheming="false"  runat="server" ShowHeader="false"  OnRowDataBound="GVOrderDetails_OnRowDataBound" AutoGenerateColumns="false" >
        <Columns>
        <asp:TemplateField HeaderText="Sno" ItemStyle-CssClass="lblheading" ItemStyle-Width="30px" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Right">                   
        <ItemTemplate>
        <%#Container.DataItemIndex + 1 %>
        </ItemTemplate>
        </asp:TemplateField>
        <%--<asp:BoundField DataField="entity_id" HeaderText="Order Number" />
        <asp:BoundField DataField="customer_firstname" HeaderText="First Name" />
        <asp:BoundField DataField="customer_lastname" HeaderText="Last Name" />
        <asp:BoundField DataField="Name" HeaderText="Pack Name" />--%>
        <asp:TemplateField>
        <ItemTemplate>
        <table id="gvtbl" runat="server" width="100%">
        <%--<tr>
        <td>Order Number</td>
        <td>Name</td>
        </tr>--%>
        <tr>
        <td><asp:Label ID="lblname" CssClass="lblheading" Text='<%#Bind("customer_firstname") %>' runat="server"></asp:Label> <asp:Label ID="Label1" CssClass="lblheading" Text='<%#Bind("customer_lastname") %>' runat="server"></asp:Label></td>
        <td><asp:Label ID="lblorderid" Visible="false" CssClass="lblheading" Text='<%#Bind("entity_id") %>' runat="server"></asp:Label></td>
        </tr>
        <tr>
        <td colspan="2"><asp:GridView Width="100%" ID="GvYOYOwithNameList" ShowHeader="false" AutoGenerateColumns="false" runat="server">
    <Columns>
<%--    <asp:BoundField DataField="title" HeaderText="Title" />--%>
    <asp:BoundField DataField="Sno" HeaderText="Sno" ItemStyle-HorizontalAlign="Right" HtmlEncode="false" ItemStyle-Width="49px" />
    <asp:BoundField DataField="Name" HeaderText="Name" HtmlEncode="false"  ItemStyle-Width="200px" />
    </Columns>
    </asp:GridView></td>
        </tr>
        </table>
        </ItemTemplate>
        </asp:TemplateField>
        </Columns>
        </asp:GridView>
     
    </td>
    </tr>
    <tr>
    <td>
    <asp:GridView ID="GVAlacarte" EnableTheming="false"  runat="server" ShowHeader="false" AutoGenerateColumns="false" >
        <Columns>
        <asp:TemplateField HeaderText="Sno" ItemStyle-CssClass="lblheading" ItemStyle-Width="30px" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Right">                   
        <ItemTemplate>
        <%#Container.DataItemIndex + 1 %>
        </ItemTemplate>
        </asp:TemplateField>
        <%--<asp:BoundField DataField="entity_id" HeaderText="Order Number" />
        <asp:BoundField DataField="customer_firstname" HeaderText="First Name" />
        <asp:BoundField DataField="customer_lastname" HeaderText="Last Name" />
        <asp:BoundField DataField="Name" HeaderText="Pack Name" />--%>
        <asp:TemplateField>
        <ItemTemplate>
        <table id="gvtbl" runat="server" width="100%">
        <%--<tr>
        <td>Order Number</td>
        <td>Name</td>
        </tr>--%>
        <tr>
        <td><asp:Label ID="lblname" CssClass="lblheading" Text='<%#Bind("customername") %>' runat="server"></asp:Label></td>
        <td></td>
        <td></td>
        <td align="right"><asp:Label ID="lblorderid" Visible="true" CssClass="lblheading" Text='<%#Bind("entity_id") %>' runat="server"></asp:Label></td>
        </tr>
        <tr>
        <td colspan="4"><asp:GridView Width="100%" ID="GvalacartewithNameList" ShowHeader="false" AutoGenerateColumns="false" runat="server">
    <Columns>
<%--    <asp:BoundField DataField="title" HeaderText="Title" />--%>
    <%--<asp:BoundField DataField="Sno" HeaderText="Sno" ItemStyle-HorizontalAlign="Right" HtmlEncode="false" ItemStyle-Width="49px" />ItemStyle-CssClass="lblheading"--%>
    <asp:TemplateField HeaderText="Sno"  ItemStyle-Width="40px" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center">                   
        <ItemTemplate>
        <%#Container.DataItemIndex + 1 %>
        </ItemTemplate>
        </asp:TemplateField>
    <asp:BoundField DataField="product_id" HeaderText="pid" HtmlEncode="false"  ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center"/>
    <asp:BoundField DataField="Name" HeaderText="Name" HtmlEncode="false"  ItemStyle-Width="400px" />
    <asp:BoundField DataField="product_group" HeaderText="Group" HtmlEncode="false"  ItemStyle-Width="70px" />
    <asp:BoundField DataField="weight" HeaderText="weight" HtmlEncode="false"  ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right" />
    <asp:BoundField DataField="units" HeaderText="units" HtmlEncode="false"  ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Left" />
    <asp:BoundField DataField="amount" HeaderText="price" HtmlEncode="false" DataFormatString="{0:0.00}"  ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Right" />
    
    </Columns>
    </asp:GridView></td>
        </tr>
        <tr>
        <td><asp:Label ID="lblorderstatus" CssClass="lblheading" Text='<%#Bind("order_status")%>' runat="server"></asp:Label> : Sub Total : <b class="lblheading">Rs.</b> <asp:Label ID="lblsubtotal" CssClass="lblheading" Text='<%#Bind("subtotal","{0:0.00}")%>' runat="server"></asp:Label> - Discount Amount : <b class="lblheading">Rs.</b> <asp:Label ID="lbldiscount_amount" CssClass="lblheading" Text='<%#Bind("discount_amount","{0:0.00}") %>' runat="server"></asp:Label>
        </td>
        <td colspan="3" align="right">Grand Total : <b class="lblheading">Rs.</b> <asp:Label ID="lblgrandtotal" CssClass="lblheading" Text='<%#Bind("grand_total","{0:0.00}") %>' runat="server"></asp:Label></td>
        </tr>
        </table>
        </ItemTemplate>
        </asp:TemplateField>
        </Columns>
        </asp:GridView>
    </td>
    </tr>
    <tr>
    <td><%--BorderStyle="None" GridLines="None"--%>
    
    <asp:GridView ID="GVMail" Visible="false"  runat="server" ShowHeader="false"  OnRowDataBound="GVMail_OnRowDataBound" AutoGenerateColumns="false" >
        <Columns>
        <asp:TemplateField HeaderText="Sno" ItemStyle-CssClass="lblheading" ItemStyle-Width="30px" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Right">                   
        <ItemTemplate>
        <%#Container.DataItemIndex + 1 %>
        </ItemTemplate>
        </asp:TemplateField>
        <%--<asp:BoundField DataField="entity_id" HeaderText="Order Number" />
        <asp:BoundField DataField="customer_firstname" HeaderText="First Name" />
        <asp:BoundField DataField="customer_lastname" HeaderText="Last Name" />
        <asp:BoundField DataField="Name" HeaderText="Pack Name" />--%>
        <asp:TemplateField>
        <ItemTemplate>
        <table width="100%">
        <%--<tr>
        <td>Order Number</td>
        <td>Name</td>
        </tr>--%>
        <tr>
        <td><asp:Label ID="lblname" CssClass="lblheading" Text='<%#Bind("customer_firstname") %>' runat="server"></asp:Label> <asp:Label ID="Label1" CssClass="lblheading" Text='<%#Bind("customer_lastname") %>' runat="server"></asp:Label></td>
        <td><asp:Label ID="lblorderid" Visible="false" CssClass="lblheading" Text='<%#Bind("entity_id") %>' runat="server"></asp:Label></td>
        </tr>
        <tr>
        <td colspan="2"><asp:GridView Width="100%" ID="GvYOYOwithNameList" ShowHeader="false" AutoGenerateColumns="false" runat="server">
    <Columns>
<%--    <asp:BoundField DataField="title" HeaderText="Title" />--%>
    <asp:BoundField DataField="Sno" HeaderText="Sno" ItemStyle-HorizontalAlign="Right" HtmlEncode="false" />
    <asp:BoundField DataField="Name" HeaderText="Name" HtmlEncode="false" />
    </Columns>
    </asp:GridView></td>
        </tr>
        </table>
        </ItemTemplate>
        </asp:TemplateField>
        </Columns>
        </asp:GridView>
     
    </td>
    </tr>
    <tr>
    <td>
    <asp:GridView ID="GvPackList3" runat="server">
    <Columns>
    <asp:TemplateField HeaderText="Sno" ItemStyle-Width="40px">                   
    <ItemTemplate>
    <%#Container.DataItemIndex + 1 %>
    </ItemTemplate>
    </asp:TemplateField>
    <asp:BoundField DataField="entity_id" HeaderText="Order No"  />
    <asp:BoundField DataField="customername" HeaderText="Customer Name"   />
    <asp:BoundField DataField="Name" HeaderText="Name"  />
    <asp:BoundField DataField="Weight" HeaderText="Weight" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
    </Columns>
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
