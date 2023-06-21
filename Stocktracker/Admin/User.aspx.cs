using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace Stocktracker.Admin
{
    public partial class User : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["stocktracker"].ConnectionString;
        public int UserID { get; set; }
        public string Username { get; set; } // Added username field
        public string Name { get; set; }
        public int RoleID { get; set; }
        public int BranchID { get; set; }
        public string Password { get; set; } // New password field

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindUsersGrid();
                BindRolesDropDown();
                BindBranchesDropDown();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim(); // Get the username value
            string name = txtName.Text.Trim();
            string password = txtPassword.Text.Trim(); // Get the password value
            int roleId = Convert.ToInt32(ddlRole.SelectedValue);
            int branchId = Convert.ToInt32(ddlBranch.SelectedValue);

            if ((!string.IsNullOrWhiteSpace(username)) && (!string.IsNullOrWhiteSpace(name)) && (!string.IsNullOrWhiteSpace(password)) && ddlRole.SelectedValue != "0" && ddlBranch.SelectedValue != "0")
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Users (Username, Name, RoleID, BranchID, Password) VALUES (@Username, @Name, @RoleID, @BranchID, @Password)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@RoleID", roleId);
                        command.Parameters.AddWithValue("@BranchID", branchId);
                        command.Parameters.AddWithValue("@Password", password);
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }

                ClearForm();
                BindUsersGrid();
            }
            else
            {
                string message = "Enter all Data!";
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + message + "');", true);
                return;
            }
        }


        protected void gridViewUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridViewUsers.EditIndex = e.NewEditIndex;
            BindUsersGrid();

            // Get the reference to the DropDownList controls in the edited row
            DropDownList ddlEditRole = (DropDownList)gridViewUsers.Rows[e.NewEditIndex].FindControl("ddlEditRole");
            DropDownList ddlEditBranch = (DropDownList)gridViewUsers.Rows[e.NewEditIndex].FindControl("ddlEditBranch");

            // Bind the dropdowns with updated values
            BindRolesDropDown(ddlEditRole);
            BindBranchesDropDown(ddlEditBranch);

            // Set the selected values for the dropdowns
            int userId = Convert.ToInt32(gridViewUsers.DataKeys[e.NewEditIndex].Value);
            SetSelectedDropdownValues(userId, ddlEditRole, ddlEditBranch);
        }
        private void BindRolesDropDown(DropDownList ddlRole)
        {
            string query = "SELECT RoleID, RoleName FROM Roles";
            List<Role> roles = new List<Role>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Role role = new Role();
                        role.RoleID = Convert.ToInt32(reader["RoleID"]);
                        role.RoleName = reader["RoleName"].ToString();

                        roles.Add(role);
                    }

                    connection.Close();
                }
            }

            ddlRole.DataSource = roles;
            ddlRole.DataTextField = "RoleName";
            ddlRole.DataValueField = "RoleID";
            ddlRole.DataBind();

            // Add an empty item
            ddlRole.Items.Insert(0, new ListItem("-- Select Role --", "0"));
        }

        private void BindBranchesDropDown(DropDownList ddlBranch)
        {
            string query = "SELECT BranchID, BranchName FROM Branches";
            List<Branch> branches = new List<Branch>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Branch branch = new Branch();
                        branch.BranchID = Convert.ToInt32(reader["BranchID"]);
                        branch.BranchName = reader["BranchName"].ToString();

                        branches.Add(branch);
                    }

                    connection.Close();
                }
            }

            ddlBranch.DataSource = branches;
            ddlBranch.DataTextField = "BranchName";
            ddlBranch.DataValueField = "BranchID";
            ddlBranch.DataBind();

            // Add an empty item
            ddlBranch.Items.Insert(0, new ListItem("-- Select Branch --", "0"));
        }

        private void SetSelectedDropdownValues(int userId, DropDownList ddlEditRole, DropDownList ddlEditBranch)
        {
            // Retrieve the role ID and branch ID for the specified user ID from the database
            // Assuming you have a method to retrieve the role ID and branch ID based on the user ID
            int roleId = Convert.ToInt32(GetRoleID(userId));
            int branchId = Convert.ToInt32(GetBranchId(userId));

            // Set the selected values in the dropdowns
            ddlEditRole.SelectedValue = roleId.ToString();
            ddlEditBranch.SelectedValue = branchId.ToString();
        }

        protected void gridViewUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gridViewUsers.EditIndex = -1;
            BindUsersGrid();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // Perform any necessary actions when the cancel button is clicked
        }
        protected void gridViewUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gridViewUsers.Rows[e.RowIndex];
            int userId = Convert.ToInt32(gridViewUsers.DataKeys[e.RowIndex].Value);

            TextBox txtEditName = row.Cells[0].Controls[0] as TextBox;
            DropDownList ddlEditRole = row.FindControl("ddlEditRole") as DropDownList;
            DropDownList ddlEditBranch = row.FindControl("ddlEditBranch") as DropDownList;

            string name = txtEditName.Text.Trim();
            int roleId = Convert.ToInt32(ddlEditRole.SelectedValue);
            int branchId = Convert.ToInt32(ddlEditBranch.SelectedValue);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Users SET Name = @Name, RoleID = @RoleID, BranchID = @BranchID WHERE UserID = @UserID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@RoleID", roleId);
                    command.Parameters.AddWithValue("@BranchID", branchId);
                    command.Parameters.AddWithValue("@UserID", userId);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            gridViewUsers.EditIndex = -1;
            BindUsersGrid();
        }

        protected void gridViewUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int userId = Convert.ToInt32(gridViewUsers.DataKeys[e.RowIndex].Value);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Users WHERE UserID = @UserID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            BindUsersGrid();
        }

        private void BindUsersGrid()
        {
            string query = "SELECT UserID, Name, RoleID, BranchID FROM Users";
            List<User> users = new List<User>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        User user = new User();
                        user.UserID = Convert.ToInt32(reader["UserID"]);
                        user.Name = reader["Name"].ToString();
                        user.RoleID = Convert.ToInt32(reader["RoleID"]);
                        user.BranchID = Convert.ToInt32(reader["BranchID"]);

                        users.Add(user);
                    }

                    connection.Close();
                }
            }

            gridViewUsers.DataSource = users;
            gridViewUsers.DataBind();
        }

        private void BindRolesDropDown()
        {
            string query = "SELECT RoleID, RoleName FROM Roles";
            List<Role> roles = new List<Role>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Role role = new Role();
                        role.RoleID = Convert.ToInt32(reader["RoleID"]);
                        role.RoleName = reader["RoleName"].ToString();

                        roles.Add(role);
                    }

                    connection.Close();
                }
            }

            ddlRole.DataSource = roles;
            ddlRole.DataTextField = "RoleName";
            ddlRole.DataValueField = "RoleID";
            ddlRole.DataBind();

            // Add an empty item
            ddlRole.Items.Insert(0, new ListItem("-- Select Role --", "0"));
        }

        private void BindBranchesDropDown()
        {
            string query = "SELECT BranchID, BranchName FROM Branches";
            List<Branch> branches = new List<Branch>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Branch branch = new Branch();
                        branch.BranchID = Convert.ToInt32(reader["BranchID"]);
                        branch.BranchName = reader["BranchName"].ToString();

                        branches.Add(branch);
                    }

                    connection.Close();
                }
            }

            ddlBranch.DataSource = branches;
            ddlBranch.DataTextField = "BranchName";
            ddlBranch.DataValueField = "BranchID";
            ddlBranch.DataBind();

            // Add an empty item
            ddlBranch.Items.Insert(0, new ListItem("-- Select Branch --", "0"));
        }

        public string GetRoleName(int roleId)
        {
            string roleName = string.Empty;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT RoleName FROM Roles WHERE RoleID = @RoleID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoleID", roleId);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        roleName = result.ToString();
                    }

                    connection.Close();
                }
            }

            return roleName;
        }

        public string GetBranchName(int branchId)
        {
            string branchName = string.Empty;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT BranchName FROM Branches WHERE BranchID = @branchId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@branchId", branchId);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        branchName = result.ToString();
                    }

                    connection.Close();
                }
            }



            return branchName;
        }
        public string GetRoleID(int userId)
        {
            string RoleID = string.Empty;

            // Assuming you have a connection string named "stocktracker" in your web.config


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT RoleID FROM users WHERE userId = @userId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        RoleID = result.ToString();
                    }

                    connection.Close();
                }
            }

            return RoleID;
        }

        public string GetBranchId(int userId)
        {
            string BranchId = string.Empty;

            // Assuming you have a connection string named "stocktracker" in your web.config


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT BranchId FROM users WHERE userId = @userId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        BranchId = result.ToString();
                    }

                    connection.Close();
                }
            }



            return BranchId;
        }
        private void ClearForm()
        {
            txtName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtUsername.Text = string.Empty;
            ddlRole.SelectedIndex = 0;
            ddlBranch.SelectedIndex = 0;
        }
    }
}
