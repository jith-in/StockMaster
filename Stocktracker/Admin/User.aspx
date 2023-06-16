<%@ Page Title="" Language="C#" MasterPageFile="~/StockMaster.Master" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="Stocktracker.Admin.User" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Create User
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="text-center">Manage Users</h1>
    <div class="form-outline w-50">
        <label for="txtName">Name:</label>
        <asp:TextBox ID="txtName" class="form-control" runat="server"></asp:TextBox>
    </div>
    <div class="form-outline w-50">
        <label for="ddlRole">Role:</label>
        <asp:DropDownList ID="ddlRole" class="form-control" runat="server"></asp:DropDownList>
    </div>
    <div class="form-outline w-50">
        <label for="ddlBranch">Branch:</label>
        <asp:DropDownList ID="ddlBranch" class="form-control" runat="server"></asp:DropDownList>
    </div>
    <div class="table-responsive mt-6 pt-2">
        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" class="btn btn-success" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" class="btn btn-secondary" />
    </div>
    <div class="table-responsive mt-6 pt-4">
        <asp:GridView ID="gridViewUsers" runat="server" AutoGenerateColumns="False" DataKeyNames="UserID"
            CssClass="table table-striped table-bordered table-hover"
            OnRowEditing="gridViewUsers_RowEditing" OnRowCancelingEdit="gridViewUsers_RowCancelingEdit"
            OnRowUpdating="gridViewUsers_RowUpdating" OnRowDeleting="gridViewUsers_RowDeleting">
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Name" />
                <asp:TemplateField HeaderText="Role">
                    <ItemTemplate>
                        <%# GetRoleName(Convert.ToInt32(Eval("RoleID"))) %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlEditRole" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Branch">
                    <ItemTemplate>
                        <%# GetBranchName(Convert.ToInt32(Eval("BranchID"))) %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlEditBranch" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowEditButton="true" ButtonType="Button" ControlStyle-CssClass="btn btn-primary" />
                <asp:CommandField ShowDeleteButton="true" ButtonType="Button" ControlStyle-CssClass="btn btn-primary" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
