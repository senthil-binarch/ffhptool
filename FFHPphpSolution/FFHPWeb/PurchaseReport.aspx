<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true"
    CodeBehind="PurchaseReport.aspx.cs" Inherits="FFHPWeb.PurchaseReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%">
        <tr>
            <td class="heading">
                Purchase Report
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            From Date
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
                            To Date
                        </td>
                        <td>
                            <asp:TextBox ID="TbxToDate" Width="75px" runat="server"></asp:TextBox><asp:Image
                                ID="Image2" ImageUrl="~/Images/cal1.png" runat="server" /><cc2:CalendarExtender ID="CalendarExtender2"
                                    runat="server" Animated="true" ClearTime="true" DefaultView="Days" Format="MM/dd/yyyy"
                                    PopupPosition="BottomRight" TargetControlID="TbxToDate" PopupButtonID="Image2">
                                </cc2:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                                ControlToValidate="TbxToDate" ValidationGroup="Date"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            PID
                        </td>
                        <td>
                            <asp:TextBox ID="Tbxpid" runat="server"></asp:TextBox><cc2:FilteredTextBoxExtender
                                ID="ftetbxpid" FilterType="Numbers" TargetControlID="Tbxpid" runat="server">
                            </cc2:FilteredTextBoxExtender>
                        </td>
                        <td>
                            <asp:Button ID="btnsubmit" Text="Submit" OnClick="btnsubmit_OnClick" runat="server"
                                ValidationGroup="Date" />
                        </td>
                        <td>
                            <asp:Button ID="btnclear" Text="Clear" OnClick="btnclear_OnClick" runat="server" />
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
            <td>

                <script src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.5.js" type="text/javascript"></script>

                <style type="text/css">
                    .hover_row
                    {
                        background-color: #ffffff;
                    }
                </style>

                <script type="text/javascript">
    $(function() {
    $("[id*=gvpurchase] td").hover(function() {
            $("td", $(this).closest("tr")).addClass("hover_row");
        }, function() {
            $("td", $(this).closest("tr")).removeClass("hover_row");
        });
    });
                </script>

                <script language="javascript" type="text/javascript">
        
            //var b = document.getElementById();
            //var testingvalue = document.getElementById("ctl00_ContentPlaceHolder1_gvpurchase_ctl02_tbxnewsellingprice").value.toString();
            //alert(testingvalue.toString());

        function test() {
            var grid = document.getElementById("<%= gvpurchase.ClientID%>");


            for (var i = 0; i < grid.rows.length - 1; i++) {
                var tbxnewsellingprice = $("input[id*=tbxnewsellingprice]")
                var tbxnewpercentage = $("input[id*=tbxnewpercentage]")
                var hfpurchaseprice = $("input[id*=hfpurchaseprice]")
                var hfprice_per_kgpc = $("input[id*=hfprice_per_kgpc]")

               
                //percentage using for get price
                if (tbxnewpercentage[i].value != '') {
                    //alert(tbxnewsellingprice[i].value);
                    tbxnewsellingprice[i].value = ((parseFloat(hfpurchaseprice[i].value) / 100) * (parseFloat(tbxnewpercentage[i].value))) + parseFloat(hfpurchaseprice[i].value);

                    //tbxnewpercentage[i].value = ((parseInt(tbxnewsellingprice[i].value) - parseInt(hfpurchaseprice[i].value)) / parseInt(hfpurchaseprice[i].value)) * 100;
                }
                else {
                    tbxnewsellingprice[i].value = "";
                }

                //price using for get percentage
                //if (tbxnewsellingprice[i].value != '') {
                    //tbxnewpercentage[i].value = ((parseInt(tbxnewsellingprice[i].value) - parseInt(hfpurchaseprice[i].value)) / parseInt(hfpurchaseprice[i].value)) * 100;
                //}

                //if (tbxnewpercentage[i].value != '') {
                //    alert(tbxnewpercentage[i].value);
                //}
                //if (hfpurchaseprice[i].value != '') {
                //    alert(hfpurchaseprice[i].value);
                //}
                //if (hfprice_per_kgpc[i].value != '') {
                //    alert(hfprice_per_kgpc[i].value);
                //}
            }
        }
        function Calculate() {
            var grid = document.getElementById("<%=gvpurchase.ClientID%>");
            var inputs = grid.getElementsByTagName("input");
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "text") {
                    if (inputs[i].name.indexOf("tbxnewsellingprice").toString() != "") {
                        alert(inputs[i].name.indexOf("tbxnewsellingprice").toString());
                        var amnt = $("input[id*=tbxnewsellingprice]")
                        alert(amnt.toString()); // Getting Nan here
                        
                    }
                }
            }
        }
//        function getemail() {
//            $("#btntest").click(function() {
//            $.get('http://192.168.1.15/ffhptoolnew/ffhpservice.asmx/GetCODorderNumbers', function(data) {
//                $("#data").text(data);
//            });
//            });
//        }
//        function getemail1() {
//            $("#btntest").click(function() {
//                $.ajax({
//                    type: "GET",
//                    url: "http://localhost:4651/ffhpservice.asmx/Get_DeliveryBoy",
//                    data: "username=Kasi&password=2", // the data in form-encoded format, ie as it would appear on a querystring
//                    contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
//                    dataType: "text", // the data type we want back, so text.  The data will come wrapped in xml
//                    success: function(data) {
//                    $("#searchresultsA").html(data); // show the string that was returned, this will be the data inside the xml wrapper
//                    }
//                });

//            });
//        }
                </script>

                <%--<button id="btntest" type="button" onclick="javascript:getemail1();">Test</button>
    <div id="searchresultsA"></div>--%>
                <asp:GridView ID="gvpurchase" AutoGenerateColumns="false" runat="server" OnRowDataBound="gvpurchase_OnRowDataBound"
                    HeaderStyle-BackColor="#0071BD" HeaderStyle-ForeColor="White" RowStyle-BackColor="#F7F7DE">
                    <Columns>
                        <asp:TemplateField HeaderText="Sno">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Pur.Date" DataField="purchase_date" DataFormatString="{0:MM-dd-yyyy}"
                            HeaderStyle-Width="8%" />
                        <asp:BoundField HeaderText="Id" DataField="pid" ItemStyle-HorizontalAlign="Center"
                            HeaderStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Product Name" DataField="pname" />
                        <asp:BoundField HeaderText="Purchase Wt" DataField="purchase_weight" HeaderStyle-Width="6%"
                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                        <asp:BoundField HeaderText="unit" DataField="unit" HeaderStyle-Width="3%" />
                        <asp:BoundField HeaderText="Purchase Price" DataField="purchase_price" HeaderStyle-Width="6%"
                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                        <asp:BoundField HeaderText="Selling Price" DataField="price_per_kgpc" HeaderStyle-Width="6%"
                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                        <asp:BoundField HeaderText="%" DataField="%" DataFormatString="{0:###}" ItemStyle-HorizontalAlign="Center"
                            HeaderStyle-HorizontalAlign="Center" />
                        <%--<asp:templatefield headertext="Color"> <itemtemplate> <asp:label id="lblDate" runat="server" text='<%# Eval("date" , "{0:MMMM d, yyyy}") %>' /> </itemtemplate> </asp:templatefield>--%>
                        <asp:TemplateField HeaderText="New Selling Price" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:TextBox ID="tbxnewsellingprice" Width="50%" runat="server"></asp:TextBox>
                                <asp:HiddenField ID="hfpurchaseprice" Value='<%#Bind("purchase_price") %>' runat="server" />
                                <asp:HiddenField ID="hfprice_per_kgpc" Value='<%#Bind("price_per_kgpc") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="New Percentage" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:TextBox ID="tbxnewpercentage" Width="50%" onkeyup="javascript:test(this.value);"
                                    runat="server" AutoCompleteType="None"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Group" DataField="group" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btngeneratebulkprice" Text="Generate Bulk Price" runat="server" OnClick="btngeneratebulkprice_OnClick" />
            </td>
        </tr>
        <tr>
            <td>
                <table width="30%">
                    <tr>
                        <td width="50%">
                            <asp:FileUpload ID="FUbulkprices" runat="server" />
                        </td>
                        <td width="50%">
                            <asp:Button ID="Btnupload" Text="Upload Bulk Prices" runat="server" OnClick="Btnupload_OnClick" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
