<%@ Page Language="C#" Title="Products" MasterPageFile="~/StockMaster.Master" AutoEventWireup="true"  CodeBehind="Product.aspx.cs" Inherits="Stocktracker.Pages.WebForm21" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Products
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <div class="container">
            <h1 class="text-center">Product</h1>
            <div class="form-group">
                <asp:TextBox ID="txtProductID" runat="server" Visible="false"></asp:TextBox>
            </div>
            <div class="form-outline w-50">
                <label for="txtProductName">ProductName:</label>
                <asp:TextBox ID="txtProductName" runat="server" class="form-control"></asp:TextBox>
            </div>
            <div class="form-outline w-50">
                <label for="txtProductQuantity">Quantity:</label>
                <asp:TextBox ID="txtProductQuantity" class="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="text-center container-fluid mt-4 pt-2">
            <asp:Button ID="btnAddProduct" OnClick="btnAddProduct_Click" class="btn btn-success" runat="server" Text="Add" />
            <asp:Button ID="btnUpdateProduct" OnClick="btnUpdateProduct_Click" class="btn btn-primary" runat="server" Text="Update" />
            <asp:Button ID="btnDeleteProduct" OnClick="btnDeleteProduct_Click" class="btn btn-danger" runat="server" Text="Delete" />
            <asp:Button ID="txtClear" OnClick="btnclearProduct_Click" class="btn btn-primary" runat="server" Text="Clear" />
        </div>
        <div class="container">
            <div class="table-responsive mt-4 pt-2">
                <asp:GridView ID="dgvProducts" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" OnRowCommand="dgvProducts_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="ProductID" HeaderText="Product ID" />
                        <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                        <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                        <asp:ButtonField ButtonType="Button" Text="Select" CommandName="Select" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
</asp:Content>
