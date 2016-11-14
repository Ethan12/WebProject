<%@ Page Title="Join" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Join.aspx.cs" Inherits="WebAssignment.Account.Join" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>

    <div>
        <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
            <p class="text-success"><%: SuccessMessage %></p>
        </asp:PlaceHolder>
    </div>

    <div class="row">
        <div class="col-md-12">
            <div class="form-horizontal">
                <h4>Choose your Club & Society!</h4>
                <hr />
                <dl class="dl-horizontal">
                    <dt>Clubs:</dt>
                    <dd>
                      <asp:DropDownList ID="ddClubs" runat="server" DataSourceID="SqlDataSource1" DataTextField="Name" DataValueField="Id"></asp:DropDownList>                       
                    </dd>

                    <dt>Societies:</dt>
                    <dd>
                       <asp:DropDownList ID="ddSocieties" runat="server" DataSourceID="SqlDataSource2" DataTextField="Name" DataValueField="Id"></asp:DropDownList>
                    </dd>
                    <asp:Button ID="btnSubmit" runat="server" Text="Join!" OnClick="btnSubmit_Click" />
                </dl>
                </div>
            </div>  
    </div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="SELECT [Id], [Name] FROM [Club]"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="SELECT [Id], [Name] FROM [Society]"></asp:SqlDataSource>
    </asp:Content>
