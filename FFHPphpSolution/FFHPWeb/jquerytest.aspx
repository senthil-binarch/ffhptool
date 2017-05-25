<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="jquerytest.aspx.cs" Inherits="FFHPWeb.jquerytest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-3.1.1.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <script>
        function test2() {
            $(document).ready(function() {
                $("p").click(function() {
                    $(this).hide();
                });
            });
        }
</script>
<p onclick="test();">test</p>

<div id="images"></div>
<table id="records_table"></table>
<script>
    function test() {
    var flickerAPI = "http://localhost:4651/ffhpservice.asmx/Getalacartewithnamesjson";
        $.getJSON(flickerAPI, {
            tags: "mount rainier",
            tagmode: "any",
            format: "json"
        })
    .done(
    function(data) 
    {
        $.each(data.items, function(i, item) {
        $('<tr>').html(
                    $('td').text(item.Name)
                ).appendTo('#records_table');
        });
    });
}

$(function() {
    $('#btnCallService').click(function() {
        $.ajax({
            type: 'POST',
            url: 'http://localhost:4651/ffhpservice.asmx/Getalacartewithnamesjson',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function(response) {
                $('#lblData').html(JSON.stringify(response));
            },
            error: function(error) {
                console.log(error);
            }
        });
    });
});
</script>
<script>

    function test1() {
        $.ajax({
            url: 'http://localhost:4651/ffhpservice.asmx/Getalacartewithnamesjson',
            dataType: 'json',
            success: function(response) {
                var trHTML = '';
                for (var f = 0; f < response.length; f++) {
                    trHTML += '<tr><td><strong>' + response[f]['Name'] + '</strong></td><td>';
                }
                $('#records_table').html(trHTML);
            }
        });
    }

</script>
<label>htmltest</label>
<input type="button" id="btnCallService" value="GetEmployeeDetail" />
<label id="lblData"></label>
    </div>
    </form>
</body>
</html>
