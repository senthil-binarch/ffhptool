<%@ Page Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true" CodeBehind="ffhp.aspx.cs" Inherits="FFHPWeb.CalculateWeight" Theme="Skin1"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" ChildrenAsTriggers="True">
                        <ContentTemplate>
                            <div>
        <table width="100%">
<tr>
<td class="lblheading">Delivery Information</td>
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
    <asp:GridView ID="GVOrderDetails" runat="server" >
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
        <asp:TemplateField HeaderText="COD">
    <%--<HeaderTemplate>
    <asp:CheckBox ID="CODall" OnCheckedChanged="CODall_OnCheckedChanged" runat="server" AutoPostBack="true" /><asp:HiddenField ID="HFcodordernum" Value='<%#Bind("entity_id") %>' runat="server" />
    </HeaderTemplate>--%>
    <ItemTemplate>
    <asp:CheckBox ID="COD" OnCheckedChanged="COD_OnCheckedChanged" runat="server" AutoPostBack="true" /><asp:HiddenField ID="HFcodordernum" Value='<%#Bind("entity_id") %>' runat="server" />
    </ItemTemplate>
    </asp:TemplateField>
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
    <td><asp:Button ID="btncalculate" Text="Submit" OnClick="btnsubmit1_OnClick" runat="server" /></td><%--ValidationGroup="cal"--%>
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
    <asp:Label ID="lblerror" runat="server"></asp:Label><asp:HiddenField ID="HFcodorderlist" runat="server" />
    </td>
    </tr>
    <tr>
    <td>
    <table>
    <%--<tr>
    <td class="header">Pack Details</td>
    <td class="header">Weight Details</td>
    </tr>--%>
    <tr>
    <td><asp:Label ID="lblpackcount" CssClass="lbl" runat="server"></asp:Label></td>
    <td><asp:Label ID="lblkgcount" CssClass="lbl" runat="server"></asp:Label></td>
    </tr>
    </table>
    </td>
    </tr>
    <tr>
    <td>
    <asp:GridView ID="GVOrderDetails2" runat="server" ShowFooter="true" >
        <Columns>
        <%--<asp:TemplateField>
        <ItemTemplate>
        <asp:CheckBox ID="chkorderid" runat="server" />
        </ItemTemplate>
        </asp:TemplateField>--%>
        <asp:TemplateField HeaderText="Sno" ItemStyle-Width="55px">                   
    <ItemTemplate>
    <asp:Label ID="lblsno" Text='<%#Container.DataItemIndex + 1 %>' runat="server"></asp:Label>
    <%--<%#Container.DataItemIndex + 1 %>--%>
    </ItemTemplate>
    </asp:TemplateField>
        <asp:BoundField DataField="entity_id" HeaderText="Order #" ItemStyle-Width="150px"  />
        <asp:BoundField DataField="customername" HeaderText="Name" ItemStyle-Width="240px" />
        <asp:BoundField DataField="Address" HeaderText="Address" ItemStyle-Width="400px" />
        <asp:BoundField DataField="Name" HeaderText="Pack" ItemStyle-Width="170px" />
        <%--<asp:BoundField DataField="Zipcode" HeaderText="Zipcode" ItemStyle-Width="140px" />--%>
        <asp:BoundField DataField="Amount" HeaderText="Amount" HeaderStyle-HorizontalAlign="Right" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
        </Columns>
        </asp:GridView>
    </td>
    </tr>
    <tr>
    <td>
    <asp:GridView ID="GVMail" Visible="false" runat="server" >
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
        <asp:BoundField DataField="entity_id" HeaderText="Order #" />
        <asp:BoundField DataField="customername" HeaderText="Name" />
        <asp:BoundField DataField="Address" HeaderText="Address" />
        <asp:BoundField DataField="Name" HeaderText="Pack" />
        <asp:BoundField DataField="Zipcode" HeaderText="Zipcode" />
        </Columns>
        </asp:GridView>
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
    <tr>
    <td>
        <asp:GridView ID="GridView1" runat="server" OnRowDataBound="GridView1_OnRowDataBound">
        <Columns>
        <asp:BoundField DataField="Name" HeaderText="Name" />
        <%--<asp:BoundField DataField="Weight" HeaderText="Weight" />--%>
        </Columns>
        </asp:GridView>
    </td>
    </tr>
    <tr>
    <td>
    <asp:Label ID="lblcount" runat="server"></asp:Label>
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
                                        <div style="margin-right: auto; text-align: center; vertical-align: middle">
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
