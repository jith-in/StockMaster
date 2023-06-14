<%@ Page Title="" Language="C#" MasterPageFile="~/StockMaster.Master" AutoEventWireup="true" CodeBehind="Branch.aspx.cs" Inherits="Stocktracker.Admin.Branch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Manage Branches
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-group">
        <h1 class="text-center">Manage Branches</h1>
        <label for="txtBranchName">Branch Name:</label>
        <asp:TextBox ID="txtBranchName" runat="server"></asp:TextBox>
    </div>
    <div class="form-group">
        <asp:Button ID="btnAddBranch" runat="server" Text="Add Branch" class="btn btn-success" OnClick="btnAddBranch_Click" />

    </div>
    <div class="table-responsive">
        <asp:GridView ID="gridViewBranches" runat="server" AutoGenerateColumns="False" DataKeyNames="BranchID" CssClass="table table-striped table-bordered table-hover"
            OnRowEditing="gridViewBranches_RowEditing" OnRowCancelingEdit="gridViewBranches_RowCancelingEdit"
            OnRowUpdating="gridViewBranches_RowUpdating" OnRowDeleting="gridViewBranches_RowDeleting" OnPageIndexChanging="gridViewBranches_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="BranchName" HeaderText="Branch Name" />
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


