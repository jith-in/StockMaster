<%@ Page Title="" Language="C#" MasterPageFile="~/StockMaster.Master" AutoEventWireup="true" CodeBehind="Role.aspx.cs" Inherits="Stocktracker.Admin.Role" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Manage Roles
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-group">
        <h1 class="text-center">Manage Roles</h1>
        <label for="txtRoleName">Role:</label>
        <asp:TextBox ID="txtRoleName" runat="server"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Button ID="btnAddRole" runat="server" Text="Add Role" class="btn btn-success" OnClick="btnAddRole_Click" />

    </div>
    <div class="table-responsive">
        <asp:GridView ID="gridViewRoles" runat="server" AutoGenerateColumns="False" DataKeyNames="RoleID" CssClass="table table-striped table-bordered table-hover"
            OnRowEditing="gridViewRoles_RowEditing" OnRowCancelingEdit="gridViewRoles_RowCancelingEdit"
            OnRowUpdating="gridViewRoles_RowUpdating" OnRowDeleting="gridViewRoles_RowDeleting" OnPageIndexChanging="gridViewRoles_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="RoleName" HeaderText="Role" />
                <asp:CommandField ShowEditButton="true" ButtonType="Button" ControlStyle-CssClass="btn btn-primary" />
                <asp:CommandField ShowDeleteButton="true" ButtonType="Button" ControlStyle-CssClass="btn btn-primary" />
            </Columns>
            <PagerSettings Mode="NumericFirstLast" PageButtonCount="5" />
        </asp:GridView>
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <a href="?page=<%# Container.DataItem %>"
                    class='<%# Convert.ToInt32(Request.QueryString["page"]) == Convert.ToInt32(Container.DataItem) ? "active" : "" %>'>
                    <%# Container.DataItem %>
                </a>
            </ItemTemplate>
        </asp:Repeater>
    </div>


</asp:Content>
