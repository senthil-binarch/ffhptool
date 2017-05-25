<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true" CodeBehind="Product_Update.aspx.cs" Inherits="FFHPWeb.Product_Update" Theme="Skin1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script language="javascript" type="text/javascript">
function myFunction() {
    var person = prompt("Are you sure to delete? Please enter 'Yes' for confirmation.", "");
    var t = false;
    if (person != "Yes") {
        //alert(person + " fail");
        //return false;
        t = false;
    }
    else {
        //alert(person + " Success");
        //return true;
        t = true;
    }
    return t;
}
</script>
<table width="50%">
<%--<tr>
<td><asp:Button ID="Btntest" runat="server" Text="Test" OnClick="Btntest_OnClick" OnClientClick="javascript:return myFunction();" /></td>
</tr>--%>
<tr>
<td>
<table>
<tr>
<td class="lblheading">Pack Details</td>
<td><asp:Button ID="BtnGetPackDetails" Text="Get Pack List" runat="server" OnClick="BtnGetPackDetails_OnClick" /></td>
<td><asp:Button ID="BtnGetPackDetailsCancel" Text="Cancel" runat="server" OnClick="BtnGetPackDetailsCancel_OnClick" /></td>
</tr>
</table>
</td>
</tr>
<tr>
<td>
<asp:GridView ID="GVPackDetails" runat="server" onrowediting="GVPackDetails_RowEditing" 
            onrowcancelingedit="GVPackDetails_RowCancelingEdit" 
            onrowupdating="GVPackDetails_RowUpdating">
<Columns>
<asp:TemplateField HeaderText="Pack Name">
            <ItemTemplate>
            <asp:Label ID="lblpackname" runat="server" Text='<%#Bind("pack_name") %>'></asp:Label><asp:HiddenField
                ID="HFpacknameout" Value='<%#Bind("pack_name") %>' runat="server" />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:HiddenField ID="HFpackname" Value='<%#Bind("pack_name") %>' runat="server" />
                <asp:TextBox ID="txtpackname" Width="100%" Text='<%#Bind("pack_name") %>' runat="server"></asp:TextBox>
            </EditItemTemplate>
            <ItemStyle Width="80%" />
            </asp:TemplateField>
            <asp:CommandField ShowEditButton="True" />
            <asp:TemplateField>
            <ItemTemplate>
                <asp:Button ID="BtnPackdelete" runat="server" Text="Delete" 
                    onclick="BtnPackdelete_Click" OnClientClick="javascript:return myFunction();" />
            </ItemTemplate>
                <ItemStyle Width="10%" />
            </asp:TemplateField>
            
</Columns>
</asp:GridView>
</td>
</tr>
<%--<tr>
<td>
<div id="divpopup" runat="server">
<table>
<tr>
<td colspan="2"></td>
</tr>
<tr>
<td>Please Type Yes</td>
<td><asp:TextBox ID="txtYes" runat="server"></asp:TextBox></td>
</tr>
</table>
</div>
</td>
</tr>--%>
<tr>
<td class="lblheading">Product Details</td>
</tr>
<tr>
<td>
<table>
<tr>
<td>Pack Name</td>
<td><asp:DropDownList ID="ddlproductlist" runat="server" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlproductlist_OnSelectedIndexChanged" >
</asp:DropDownList></td>
<td><asp:Button ID="BtnAddnew" Text="Add New" OnClick="BtnAddnew_OnClick" runat="server" /></td>
</tr>
</table>
</td>
</tr>
<tr id="traddnew" visible="false" runat="server">
<td>
<table>
<tr>
<td>Pack Name</td>
<td><asp:TextBox ID="txtpackname" Text="" runat="server" ValidationGroup="AddNew"></asp:TextBox><asp:RequiredFieldValidator ID="rfvtxtpackname" runat="server" ControlToValidate="txtpackname" ErrorMessage="*" ValidationGroup="AddNew" ></asp:RequiredFieldValidator></td>
</tr>
<tr>
<td>Product Name</td>
<td><asp:TextBox ID="txtproductname" Text="" runat="server" ValidationGroup="AddNew"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtproductname" ErrorMessage="*" ValidationGroup="AddNew" ></asp:RequiredFieldValidator></td>
</tr>
<tr>
<td>Weight</td>
<td><asp:TextBox ID="txtweight" Text="" runat="server" ValidationGroup="AddNew"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtweight" ErrorMessage="*" ValidationGroup="AddNew" ></asp:RequiredFieldValidator><cc2:FilteredTextBoxExtender
                                    ID="FTEweight" FilterType="Custom" TargetControlID="txtweight"
                                    BehaviorID="txtweight" runat="server" ValidChars="1234567890.">
                                </cc2:FilteredTextBoxExtender></td>
</tr>
<tr align="center">
<td colspan="2" align="center">
<table align="center">
<tr>
<td><asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_OnClick" CausesValidation="true" ValidationGroup="AddNew" /></td>
<td><asp:Button ID="BtnCancel" runat="server" Text="Cancel" OnClick="BtnCancel_OnClick"  /></td>
</tr>
</table>
</td>
</tr>
</table>
</td>
</tr>
<tr>
<td><asp:Label ID="lblerror" Text="" runat="server"></asp:Label></td>
</tr>
<tr>
<td><asp:GridView ID="GVProductDetails" runat="server" onrowediting="GVProductDetails_RowEditing" 
            onrowcancelingedit="GVProductDetails_RowCancelingEdit" 
            onrowupdating="GVProductDetails_RowUpdating">
<Columns>
<asp:TemplateField HeaderText="Product Name">
            <ItemTemplate>
            <asp:Label ID="lblproductname" runat="server" Text='<%#Bind("name") %>'></asp:Label><asp:HiddenField
                ID="HFentity_id" Value='<%#Bind("entity_id") %>' runat="server" /><asp:HiddenField
                ID="HFpacknameout" Value='<%#Bind("pack_name") %>' runat="server" />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:HiddenField ID="HFent_id" Value='<%#Bind("entity_id") %>' runat="server" />
                <asp:HiddenField ID="HFpackname" Value='<%#Bind("pack_name") %>' runat="server" />
                <asp:HiddenField ID="HFproductname" Value='<%#Bind("name") %>' runat="server" />
                <asp:TextBox ID="txtproductname" Width="100%" Text='<%#Bind("name") %>' runat="server"></asp:TextBox>
            </EditItemTemplate>
            <ItemStyle Width="80%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Weight">
            <ItemTemplate>
            <asp:Label ID="lblweight" runat="server" Text='<%#Bind("weight") %>'></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
            <asp:HiddenField ID="HFweight" Value='<%#Bind("weight") %>' runat="server" />
                <asp:TextBox ID="txtweight" Width="100%" Text='<%#Bind("weight") %>' runat="server"></asp:TextBox><cc2:FilteredTextBoxExtender
                                    ID="FTEweight" FilterType="Custom" TargetControlID="txtweight"
                                    BehaviorID="txtweight" runat="server" ValidChars="1234567890.">
                                </cc2:FilteredTextBoxExtender>
            </EditItemTemplate>
            <ItemStyle Width="80%" />
            </asp:TemplateField>
            <asp:CommandField ShowEditButton="True" />
            <asp:TemplateField>
            <ItemTemplate>
                <asp:Button ID="Btndelete" runat="server" Text="Delete" 
                    onclick="Btndelete_Click" OnClientClick="return confirm('Are you sure you want to delete?');" />
            </ItemTemplate>
                <ItemStyle Width="10%" />
            </asp:TemplateField>
            
</Columns>
</asp:GridView></td>
</tr>
</table>
</asp:Content>
