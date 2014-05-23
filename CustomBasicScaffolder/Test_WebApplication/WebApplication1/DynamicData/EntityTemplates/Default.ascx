<%@ Control Language="C#" CodeBehind="Default.ascx.cs" Inherits="WebApplication1.DefaultEntityTemplate" %>

<asp:EntityTemplate runat="server" ID="EntityTemplate1">
    <ItemTemplate>
    <div class="row">
		<div class="col-sm-2 text-right">
			<strong><asp:Label ID="Label1" runat="server" OnInit="Label_Init" /></strong>
		</div>
		<div class="col-sm-2">
			<asp:DynamicControl ID="DynamicControl1" runat="server" OnInit="DynamicControl_Init" />
		</div>
    </div>
    </ItemTemplate>
</asp:EntityTemplate>

