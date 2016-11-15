<%@ Page Title="View" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="WebAssignment.Account.View" %>


<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: PageName %></h2>

    <div>
        <asp:PlaceHolder runat="server" ID="errorMessage" Visible="false" ViewStateMode="Disabled">
            <p class="text-success"><%: ErrorMessage %></p>
        </asp:PlaceHolder>
    </div>
</asp:Content>