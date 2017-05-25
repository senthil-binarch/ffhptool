<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="event.aspx.cs" Inherits="FFHPWeb._event" Theme="Skin1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
<style> 
 .textbox { 
    -webkit-border-radius: 5px; 
    -moz-border-radius: 5px; 
    border-radius: 5px; 
    border: 1px solid #848484; 
    outline:0; 
    height:25px; 
    width: 275px; 
 } 
 .label
 {
 	color: #533517;
    font-size: 19px;
 }
 .button {
  display: inline-block;
  border-radius: 4px;
  background-color: Green;
  border: none;
  color: #FFFFFF;
  text-align: center;
  font-size: 18px;
  padding: 5px;
  /*width: 80px;*/
  transition: all 0.5s;
  cursor: pointer;
  margin: 5px;
}

.button span {
  cursor: pointer;
  display: inline-block;
  position: relative;
  transition: 0.5s;
}

.button span:after {
  content: "»";
  position: absolute;
  opacity: 0;
  top: 0;
  right: -20px;
  transition: 0.5s;
}

.button:hover span {
  padding-right: 10px;
}

.button:hover span:after {
  opacity: 1;
  right: 0;
}
</style> 
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
    <table width="100%">
    <tr>
    <td width="100%" colspan="3">
    <table width="80%">
    <tr>
    <td width="100%" align="center"><img src="event/Header-animated-file.gif" alt="FFHP" runat="server" /></td>
    </tr>
    </table>
    </td>
    </tr>
    <tr>
    <td width="25%"><img id="Img1" height="370" width="300" src="event/FlyerFront1.jpg" alt="FFHP" runat="server" /></td>
    <td align="center" width="40%">
    <table width="100%">
    <tr>
    <td colspan="3" align="center" class="label">Registration</td>
    </tr>
    <tr>
    <td width="20%" class="label">Name</td>
    <td width="90%"><asp:TextBox ID="tbxname" CssClass="textbox"  runat="server" Width="100%"></asp:TextBox></td>
    <td width="10%"><span style="color:Red;"><asp:RequiredFieldValidator runat="server" id="reqName" controltovalidate="tbxname" errormessage="*" /></span></td>
    </tr>
    <tr>
    <td class="label">Email</td>
    <td><asp:TextBox ID="tbxemail" CssClass="textbox"  runat="server"  Width="100%"></asp:TextBox></td>
    <td style="color:Red;"><asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator2" controltovalidate="tbxemail" errormessage="*" /><asp:RegularExpressionValidator ID="TxtEmailRegEx" runat="server" 
       ErrorMessage="*"
       ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
       ControlToValidate="tbxemail" /></td>
    </tr>
    <tr>
    <td class="label">Phone</td>
    <td><asp:TextBox ID="tbxphone" MaxLength="12" CssClass="textbox" runat="server"  Width="100%"></asp:TextBox></td>
    <td style="color:Red;"><asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator1" controltovalidate="tbxphone" errormessage="*" /><cc2:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tbxphone" FilterType="Numbers"></cc2:FilteredTextBoxExtender></td>
    </tr>
    <tr>
    <td class="label">Other Info</td>
    <td><asp:TextBox ID="tbxcomment" CssClass="textbox" TextMode="MultiLine" Width="100%" Height="100px" runat="server" ></asp:TextBox></td>
    <td></td>
    </tr>
    <tr>
    <td colspan="3" align="center">
    <table>
    <tr>
    <td align="center"><asp:Button ID="btnsubmit" OnClick="btnsubmit_OnClick" CssClass="button" Text="Submit" runat="server" /></td>
    <td align="center"><asp:Button ID="btnclear" OnClick="btnclear_OnClick" CssClass="button" Text="Clear" runat="server" CausesValidation="false" /></td>
    </tr>
    </table>
    </td>
    </tr>
    <tr>
    <td colspan="3"><asp:Label ID="lblerror" runat="server"></asp:Label></td>
    </tr>
    </table>
    </td>
    <td width="35%"><img id="Img2" height="370" width="300" src="event/FlyerFront2.jpg" alt="FFHP" runat="server" /></td>
    </tr>
    <tr>
    <td height="100px"></td>
    </tr>
    <tr>
    <td colspan="3"><asp:Button ID="btneventusers" CssClass="button" CausesValidation="false" Text="Event Users" OnClick="btneventusers_OnClick" runat="server" />
    <asp:GridView ID="gveventusers" runat="server">
    <Columns>
    <asp:TemplateField HeaderText="Sno">                   
    <ItemTemplate>
    <%#Container.DataItemIndex + 1 %>
    </ItemTemplate>
    </asp:TemplateField>
    <asp:BoundField DataField="name" HeaderText="Name" />
    <asp:BoundField DataField="email" HeaderText="Email" />
    <asp:BoundField DataField="phone" HeaderText="Phone" />
    <asp:BoundField DataField="comments" HeaderText="Comments" />
    </Columns>
    </asp:GridView>
    </td>
    </tr>
    </table>
</ContentTemplate>
</asp:UpdatePanel> 
    </div>
    </form>
    
</body>
</html>
