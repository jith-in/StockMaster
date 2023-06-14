<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/StockMaster.Master" CodeBehind="ProductRequest.aspx.cs" Inherits="Stocktracker.Pages.ProductRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Product Request
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Product Request</h3>
    <div class="form-outline w-50">
        <label for="txtProductId">Product ID:</label>
        <asp:DropDownList ID="ddlProducts" CssClass="form-control" runat="server"></asp:DropDownList>
    </div>
    <div class="form-outline w-50">
        <label for="txtProductName">Quantity:</label>
        <asp:TextBox ID="txtQuantity" runat="server" onkeypress="return isNumberKey(event)" class="form-control"></asp:TextBox>

    </div>
    <div class="table-responsive mt-4 pt-2">
        <asp:Button ID="btnRequest" OnClick="btnRequest_Click" class="btn btn-success" runat="server" Text="Request" />
    </div>
    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>
</asp:Content>

