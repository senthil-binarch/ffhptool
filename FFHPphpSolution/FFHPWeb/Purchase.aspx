<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true" CodeBehind="Purchase.aspx.cs" Inherits="FFHPWeb.Purchase" Theme="Skin1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript" >
function ConfirmShowreport(URL)
    {       
        var Ok = confirm('Successfully uploaded, do you want show the report?');
        if(Ok == true)
        {
            document.location.href=URL;           
        }       
        //else
            //return false;
    } 
</script>

<table width="100%" align="center">
<tr>
<td class="heading">Purchase</td>
</tr>
<tr>
<td>
<table width="100%">
<tr>
<td width="25%"><asp:FileUpload ID="FUpurchase" runat="server" /></td>
<td width="25%"><asp:Button ID="Btnupload" Text="Upload" runat="server" OnClick="Btnupload_OnClick" /></td>
<td align="right" width="50%"><asp:Button ID="btndownload" Text="excel download" OnClick="btndownload_OnClick" runat="server" /></td>
</tr>
</table>
</td>
</tr>
<tr>
<td><asp:Label ID="lblerror" runat="server"></asp:Label></td>
</tr>
<tr>
<td><asp:GridView ID="gvpurchase" AutoGenerateColumns="false" runat="server" Width="100%">
<Columns>
<asp:BoundField HeaderText="pid" DataField="product_id" />
<asp:BoundField HeaderText="pname" DataField="name" />
<asp:BoundField HeaderText="purchaseweight"  />
<asp:BoundField HeaderText="purchaseprice"  />
<asp:BoundField HeaderText="unit" DataField="unit" />
<asp:BoundField HeaderText="price_per_kgpc" DataField="priceperkgpc" />

<asp:BoundField HeaderText="purchase_date" DataField="date" DataFormatString="{0:MM-dd-yyyy}" />
<%--<asp:templatefield headertext="Color"> <itemtemplate> <asp:label id="lblDate" runat="server" text='<%# Eval("date" , "{0:MMMM d, yyyy}") %>' /> </itemtemplate> </asp:templatefield>--%>
</Columns>
</asp:GridView></td>
</tr>
<tr>
<td> 
    </td>
</tr>
</table>
</asp:Content>
