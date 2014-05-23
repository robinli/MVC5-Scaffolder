<%@ Page Title="SYSCKINDList" Language="C#" MasterPageFile="~/Site1.Master" CodeBehind="Default.aspx.cs" Inherits="WebApplication1.Models.SYSCKIND.Default" ViewStateMode="Disabled" %>
<%@ Register TagPrefix="FriendlyUrls" Namespace="Microsoft.AspNet.FriendlyUrls" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <h2>SYSCKIND List</h2>
    <p>
        <asp:HyperLink runat="server" NavigateUrl="Insert.aspx" Text="Create new" />
    </p>
    <div>
        <asp:ListView runat="server"
            DataKeyNames="CKIND" ItemType="WebApplication1.SYSCKIND"
            AutoGenerateColumns="false"
            AllowPaging="true" AllowSorting="true"
            SelectMethod="GetData">
            <EmptyDataTemplate>
                There are no entries found for SYSCKIND
            </EmptyDataTemplate>
            <LayoutTemplate>
                <table class="table">
                    <thead>
                        <tr>
                            <th>DESCTXT</th>
                            <th>&nbsp;</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr runat="server" id="itemPlaceholder" />
                    </tbody>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:DynamicControl runat="server" DataField="DESCTXT" ID="DESCTXT" Mode="ReadOnly" />
                    </td>
                    <td>
                        <a href="Edit.aspx?CKIND=<%#: Item.CKIND%>">Edit</a> | 
                        <a href="Delete.aspx?CKIND=<%#: Item.CKIND%>">Delete</a>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
    </div>
</asp:Content>
