<%@ Control Language="C#" CodeBehind="ForeignKey_Edit.ascx.cs" Inherits="WebApplication1.ForeignKey_EditField" %>

<div id="Div1" runat="server" class="form-group">
    <asp:Label ID="Label1" runat="server" CssClass="col-sm-2 control-label" />
    <div class="col-sm-3">
		<asp:DropDownList 
			ID="DropDownList1"
			SelectMethod="GetData"
			DataTextField="<%#DataTextField%>"
			DataValueField="<%#DataValueField%>"
			SelectedValue="<%#FieldValueString%>"
			CssClass="form-control"
            AppendDataBoundItems="true"
			Runat="server">
                <asp:ListItem Text="Select An Option" Value=""></asp:ListItem>
            </asp:DropDownList>
    </div>
</div>

