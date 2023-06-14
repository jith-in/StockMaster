using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;

namespace Stocktracker.Admin
{
    public partial class Role : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["stocktracker"].ConnectionString;
        private const int PageSize = 2;
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindRolesGrid();
            }
        }

        protected void btnAddRole_Click(object sender, EventArgs e)
        {
            string RoleName = txtRoleName.Text.Trim();

            // Insert the Role into the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Roles (RoleName) VALUES (@RoleName)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoleName", RoleName);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            ClearForm();
            BindRolesGrid();
        }

        protected void gridViewRoles_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridViewRoles.EditIndex = e.NewEditIndex;
            BindRolesGrid();
        }

        protected void gridViewRoles_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gridViewRoles.EditIndex = -1;
            BindRolesGrid();
        }
        protected void gridViewRoles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridViewRoles.PageIndex = e.NewPageIndex;
            BindRolesGrid();
        }
        protected void gridViewRoles_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gridViewRoles.Rows[e.RowIndex];
            int RoleID = Convert.ToInt32(gridViewRoles.DataKeys[e.RowIndex].Value);

            // Find the TextBox control within the GridView cell
            TextBox txtRoleName = row.Cells[0].Controls[0] as TextBox;
            string RoleName = txtRoleName.Text;

            // Update the Role in the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Roles SET RoleName = @RoleName WHERE RoleID = @RoleID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoleName", RoleName);
                    command.Parameters.AddWithValue("@RoleID", RoleID);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            gridViewRoles.EditIndex = -1;
            BindRolesGrid();
        }


        protected void gridViewRoles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int RoleID = Convert.ToInt32(gridViewRoles.DataKeys[e.RowIndex].Value);

            // Delete the Role from the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Roles WHERE RoleID = @RoleID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoleID", RoleID);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            BindRolesGrid();
        }

        private void ClearForm()
        {
            txtRoleName.Text = string.Empty;
        }

        private void BindRolesGrid()
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

            // Apply pagination logic
            int pageSize = 5; // Number of items to display per page
            int pageIndex = 0; // Current page index (0-based)
            if (Request.QueryString["page"] != null)
            {
                int.TryParse(Request.QueryString["page"], out pageIndex);
            }

            // Calculate the starting index and ending index of the items to display
            int startIndex = pageIndex * pageSize;
            int endIndex = Math.Min(startIndex + pageSize - 1, roles.Count - 1);

            // Create a new list to hold the items for the current page
            List<RoleTemp> pageData = new List<RoleTemp>();

            // Populate the new list with the items for the current page
            for (int i = startIndex; i <= endIndex; i++)
            {
                RoleTemp tempRole = new RoleTemp();
                tempRole.RoleID = roles[i].RoleID;
                tempRole.RoleName = roles[i].RoleName;
                pageData.Add(tempRole);
            }

            // Bind the GridView control to the data for the current page
            gridViewRoles.DataSource = pageData;
            gridViewRoles.DataBind();

            // Generate pagination links
            GeneratePaginationLinks(pageIndex, pageSize, roles.Count);
        }

        private void GeneratePaginationLinks(int currentPageIndex, int pageSize, int totalItemCount)
        {
            int pageCount = (int)Math.Ceiling((double)totalItemCount / pageSize);

            // Generate pagination links using a Repeater control, or any other method you prefer
            // Here's a simple example using a Repeater control
            Repeater1.DataSource = Enumerable.Range(0, pageCount);
            Repeater1.DataBind();
        }
    }

    public class RoleTemp
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
    }
}
