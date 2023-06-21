using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Stocktracker.Login
{
    public partial class LoginPage : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["stocktracker"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Validate username and password
            if (ValidateUser(username, password))
            {
                // Store user details in session
                StoreUserDetailsInSession(username);

                // Redirect to the home page
                Response.Redirect("../Admin/User.aspx");
            }
            else
            {
                lblErrorMessage.Text = "Invalid username or password";
            }
        }

        private bool ValidateUser(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    connection.Open();

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }

        private void StoreUserDetailsInSession(string username)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Users JOIN Roles ON Users.RoleID = Roles.RoleID JOIN Branches ON Users.BranchID = Branches.BranchID WHERE Username = @Username";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        // Store user details in session variables
                        Session["UserID"] = reader["UserID"];
                        Session["Name"] = reader["Name"];
                        Session["RoleID"] = reader["RoleID"];
                        Session["RoleName"] = reader["RoleName"];
                        Session["BranchID"] = reader["BranchID"];
                        Session["BranchName"] = reader["BranchName"];
                    }

                    reader.Close();
                    connection.Close();
                }
            }
        }
    }
}
