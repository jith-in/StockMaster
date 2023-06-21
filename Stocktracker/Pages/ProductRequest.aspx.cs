using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Stocktracker.Pages
{
    public partial class ProductRequest : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["stocktracker"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts();
            }
        }
        protected void btnRequest_Click(object sender, EventArgs e)
        {
            string productId = ddlProducts.SelectedValue; // Assuming you have a textbox with ID "txtProductId" for user input

            string Quantity = txtQuantity.Text.Trim();

            string user = string.Empty;
            if (Session["Name"] != null)
            {

                user = Session["Name"].ToString();
            }
            else
            {
                user = "Admin";
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Insert the requested product into the database
                string query = "INSERT INTO ProductRequest (ProductID, Quantity, RequestedBy, RequestDate) VALUES (@ProductId, @Quantity, @RequestedBy, @RequestDate)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductId", productId);
                command.Parameters.AddWithValue("@Quantity", Quantity);
                command.Parameters.AddWithValue("@RequestedBy", user); // Assuming you're using authentication and storing the username
                command.Parameters.AddWithValue("@RequestDate", DateTime.Now); // Add the RequestDate parameter with the current date and time

                command.ExecuteNonQuery();

            }

            string message = "Product requested successfully!";
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + message + "');", true);
        }
        private void LoadProducts()
        {
            // Assuming you have a connection string to your SQL database
            //string connectionString = "YourConnectionString";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Retrieve all products from the database
                string query = "SELECT ProductID, ProductName FROM Products";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                // Clear any existing items in the dropdown
                ddlProducts.Items.Clear();

                // Add each product as an item in the dropdown
                while (reader.Read())
                {
                    string productId = reader["ProductID"].ToString();
                    string productName = reader["ProductName"].ToString();
                    ddlProducts.Items.Add(new ListItem(productName, productId));
                }

                reader.Close();
            }
        }
    }
}