<%@ Page Title="SYSCKINDDelete" Language="C#" MasterPageFile="~/Site1.Master" CodeBehind="Delete.aspx.cs" Inherits="WebApplication1.Models.SYSCKIND.Delete" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <div>
		<p>&nbsp;</p>
        <h3>Are you sure want to delete this SYSCKIND?</h3>
        <asp:FormView runat="server"
            ItemType="WebApplication1.SYSCKIND" DataKeyNames="CKIND"
            DeleteMethod="DeleteItem" SelectMethod="GetItem"
            OnItemCommand="ItemCommand" RenderOuterTable="false">
            <EmptyDataTemplate>
                Cannot find the SYSCKIND with CKIND <%: Request.QueryString["CKIND"] %>
            </EmptyDataTemplate>
            <ItemTemplate>
                <fieldset class="form-horizontal">
                    <legend>Delete SYSCKIND</legend>
					<asp:DynamicEntity runat="server" Mode="ReadOnly" />
                 	<div class="row">
					  &nbsp;
					</div>
					<div class="form-group">
						<div class="col-sm-offset-2 col-sm-10">
							<asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" CssClass="btn btn-danger" />
							<asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" CssClass="btn btn-default" />
						</div>
					</div>
                </fieldset>
            </ItemTemplate>
        </asp:FormView>
    </div>
</asp:Content>
