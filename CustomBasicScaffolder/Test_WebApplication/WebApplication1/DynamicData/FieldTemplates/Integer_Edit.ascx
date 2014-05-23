<%@ Control Language="C#" CodeBehind="Integer_Edit.ascx.cs" Inherits="WebApplication1.Integer_EditField" %>



<div id="Div1" runat="server" class="form-group">
    <asp:Label ID="Label1" runat="server" CssClass="col-sm-2 control-label" />
    <div class="col-sm-3">
		<asp:TextBox ID="TextBox1" type="Number" runat="server" Text='<%# FieldValueEditString %>' CssClass="form-control DDTextBox"></asp:TextBox>
    </div>
</div>