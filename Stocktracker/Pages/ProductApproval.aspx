<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/StockMaster.Master" CodeBehind="ProductApproval.aspx.cs" Inherits="Stocktracker.Pages.ProductApproval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Product Approval
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Product Approval</h3>

    <div class="container">
        <div class="table-responsive mt-4 pt-2">
            <asp:GridView ID="dgvProductApproval" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" OnRowCommand="dgvProductApproval_RowCommand">
                <Columns>
                    <asp:BoundField DataField="RequestID" HeaderText="Request ID" />
                    <asp:BoundField DataField="ProductName" HeaderText="Product" />
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <asp:TextBox ID="txtQuantity" onkeypress="return isNumberKey(event)" runat="server" Text='<%# Eval("Quantity") %>' CssClass="form-control"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RequestedBy" HeaderText="Requested By" />
                    <asp:BoundField DataField="RequestDate" HeaderText="Request Date" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:ButtonField ButtonType="Button" Text="Approve" CommandName="Approve" />
                </Columns>
            </asp:GridView>
        </div>
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
