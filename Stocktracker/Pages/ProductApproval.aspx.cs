using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace Stocktracker.Pages
{
    public partial class ProductApproval : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["stocktracker"].ConnectionString;
        string Quantity = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Populate the gridview with the requested products for approval
                LoadData();
            }
        }

        protected void dgvProductApproval_RowCommand(object sender, GridViewCommandEventArgs e)
        {
           
            if (e.CommandName == "Approve")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                // Assuming you have a gridview with ID "gvRequestedProducts" to display the requested products
                GridViewRow row = dgvProductApproval.Rows[rowIndex];
                TextBox txtQuantity = row.FindControl("txtQuantity") as TextBox;
                if (txtQuantity != null)
                {
                    Quantity = txtQuantity.Text;
                    // Use the quantityValue as needed
                }
                // Get the requested product ID from the gridview
                string RequestID = row.Cells[0].Text; // Assuming the requestID is in the first cell
                int productID = Convert.ToInt32(dgvProductApproval.DataKeys[row.RowIndex].Value);
                var msg = DeductProductQuantity(productID, Convert.ToInt32(Quantity));
                if(msg != "Success")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + msg + "');", true);
                    return;
                }
                // TODO: Approve the product and perform any necessary actions
                // You can add your own logic here, such as updating the product status, notifying the requester, etc.




                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Update the product status in the database
                    string query = "UPDATE ProductRequest SET ApprovedBy = @ApprovedBy, ApprovedDate = @ApprovedDate, ApprovedStatus = 'Y',Quantity=@Quantity  WHERE RequestID = @RequestID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@RequestID", RequestID);
                    command.Parameters.AddWithValue("@Quantity", Quantity);
                    command.Parameters.AddWithValue("@ApprovedBy", User.Identity.Name); // Assuming you're using authentication and storing the username
                    command.Parameters.AddWithValue("@ApprovedDate", DateTime.Now);

                    command.ExecuteNonQuery();
                }

                string message = "Product approved successfully!";
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + message + "');", true);

                // Refresh the gridview after approving the product
                LoadData();
            }
        }

        private void LoadData()
        {
            // Assuming you have a connection string to your SQL database
            //string connectionString = "YourConnectionString";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Retrieve data from the ProductRequest table
                string query = @"SELECT RequestID,Products.ProductName ,Products.ProductID, ProductRequest.Quantity, RequestedBy, RequestDate,ApprovedStatus FROM ProductRequest 
                                join Products on ProductRequest.ProductID = Products.ProductID
                                where ApprovedStatus is NULL";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Bind the DataTable to the GridView
                dgvProductApproval.DataSource = dataTable;
                dgvProductApproval.DataBind();
            }
        }

        public string DeductProductQuantity(int productId, int quantityRequested)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create a command object for the stored procedure
                    using (SqlCommand command = new SqlCommand("DeductProductQuantity", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add input parameters to the stored procedure
                        command.Parameters.AddWithValue("@ProductID", productId);
                        command.Parameters.AddWithValue("@QuantityRequested", quantityRequested);

                        // Add an output parameter to capture the error message
                        SqlParameter errorMessageParam = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 100);
                        errorMessageParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(errorMessageParam);

                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        // Retrieve the error message from the output parameter
                        string errorMessage = errorMessageParam.Value.ToString();
                        return errorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the database operation
                Console.WriteLine("An error occurred: " + ex.Message);
                return null;
            }
        }

    }
}