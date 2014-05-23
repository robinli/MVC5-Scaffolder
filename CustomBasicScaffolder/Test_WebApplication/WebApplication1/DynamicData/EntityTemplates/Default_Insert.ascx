<%@ Control Language="C#" CodeBehind="Default_Insert.ascx.cs" Inherits="WebApplication1.Default_InsertEntityTemplate" %>
<%@ Reference Control="~/DynamicData/EntityTemplates/Default.ascx" %>

<asp:EntityTemplate runat="server" ID="EntityTemplate1">
    <ItemTemplate>
        <div id="Div1" runat="server" class="form-group" onprerender="Div1_PreRender">
            <asp:Label ID="Label1" runat="server" OnInit="Label_Init" OnPreRender="Label_PreRender" CssClass="col-sm-2 control-label" />
            <div class="col-sm-3">
                <asp:DynamicControl runat="server" ID="DynamicControl" Mode="Insert" OnInit="DynamicControl_Init" />
            </div>
        </div>
    </ItemTemplate>
</asp:EntityTemplate>
