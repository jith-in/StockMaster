using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Stocktracker.Pages
{
    public partial class WebForm21 : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["stocktracker"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts();
            }

        }
        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtProductQuantity.Text.ToString()) && !string.IsNullOrWhiteSpace(txtProductName.Text.ToString()))
            {
                string name = txtProductName.Text;
                int quantity = int.Parse(txtProductQuantity.Text);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Products (ProductName, Quantity) VALUES (@Name, @Quantity)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.ExecuteNonQuery();
                }

                ClearFields();
                LoadProducts();
            }
        }

        protected void btnUpdateProduct_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtProductQuantity.Text.ToString()) || !string.IsNullOrWhiteSpace(txtProductName.Text.ToString()))
            {
                int productId = int.Parse(txtProductID.Text);
                int quantity = int.Parse(txtProductQuantity.Text);
                string name = txtProductName.Text;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Products SET Quantity = @Quantity,ProductName=@name WHERE ProductID = @ID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@ID", productId);
                    command.ExecuteNonQuery();
                }

                ClearFields();
                LoadProducts();
            }
        }

        protected void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            int productId = int.Parse(txtProductID.Text);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Products WHERE ProductID = @ID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID", productId);
                command.ExecuteNonQuery();
            }

            ClearFields();
            LoadProducts();
        }
        protected void btnclearProduct_Click(object sender, EventArgs e)
        {
           
            ClearFields();
            LoadProducts();
        }
        
        protected void dgvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {

                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = dgvProducts.Rows[rowIndex];
                string productId = row.Cells[0].Text;
                string productName = row.Cells[1].Text;
                string quantity = row.Cells[2].Text;


                txtProductID.Text = productId;
                txtProductName.Text = productName;
                txtProductQuantity.Text = quantity;
            }
        }

        private void LoadProducts()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ProductID, ProductName, Quantity FROM Products";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvProducts.DataSource = dt;
                dgvProducts.DataBind();
            }
        }

        private void ClearFields()
        {
            txtProductID.Text = "";
            txtProductName.Text = "";
            txtProductQuantity.Text = "";
        }
    }
}