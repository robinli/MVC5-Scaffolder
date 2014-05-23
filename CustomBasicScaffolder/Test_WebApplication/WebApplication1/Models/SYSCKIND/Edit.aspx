<%@ Page Title="SYSCKINDEdit" Language="C#" MasterPageFile="~/Site1.Master" CodeBehind="Edit.aspx.cs" Inherits="WebApplication1.Models.SYSCKIND.Edit" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <div>
		<p>&nbsp;</p>
        <asp:FormView runat="server"
            ItemType="WebApplication1.SYSCKIND" DefaultMode="Edit" DataKeyNames="CKIND"
            UpdateMethod="UpdateItem" SelectMethod="GetItem"
            OnItemCommand="ItemCommand" RenderOuterTable="false">
            <EmptyDataTemplate>
                Cannot find the SYSCKIND with CKIND <%: Request.QueryString["CKIND"] %>
            </EmptyDataTemplate>
            <EditItemTemplate>
                <fieldset class="form-horizontal">
                    <legend>Edit SYSCKIND</legend>
					<asp:ValidationSummary runat="server" CssClass="alert alert-danger"  />
                    <asp:DynamicEntity runat="server" Mode="Edit" />
                    <div class="form-group">
                        <div class="col-sm-offset-2 col-sm-10">
							<asp:Button runat="server" ID="UpdateButton" CommandName="Update" Text="Update" CssClass="btn btn-primary" />
							<asp:Button runat="server" ID="CancelButton" CommandName="Cancel" Text="Cancel" CausesValidation="false" CssClass="btn btn-default" />
						</div>
					</div>
                </fieldset>
            </EditItemTemplate>
        </asp:FormView>
    </div>
</asp:Content>
