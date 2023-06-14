using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;

namespace Stocktracker.Admin
{
    public partial class Branch : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["stocktracker"].ConnectionString;
        private const int PageSize = 2;
        public int BranchID { get; set; }
        public string BranchName { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindBranchesGrid();
            }
        }

        protected void btnAddBranch_Click(object sender, EventArgs e)
        {
            string branchName = txtBranchName.Text.Trim();

            // Insert the branch into the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Branches (BranchName) VALUES (@BranchName)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BranchName", branchName);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            ClearForm();
            BindBranchesGrid();
        }

        protected void gridViewBranches_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridViewBranches.EditIndex = e.NewEditIndex;
            BindBranchesGrid();
        }

        protected void gridViewBranches_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gridViewBranches.EditIndex = -1;
            BindBranchesGrid();
        }
        protected void gridViewBranches_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridViewBranches.PageIndex = e.NewPageIndex;
            BindBranchesGrid();
        }
        protected void gridViewBranches_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gridViewBranches.Rows[e.RowIndex];
            int branchID = Convert.ToInt32(gridViewBranches.DataKeys[e.RowIndex].Value);

            // Find the TextBox control within the GridView cell
            TextBox txtBranchName = row.Cells[0].Controls[0] as TextBox;
            string branchName = txtBranchName.Text;

            // Update the branch in the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Branches SET BranchName = @BranchName WHERE BranchID = @BranchID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BranchName", branchName);
                    command.Parameters.AddWithValue("@BranchID", branchID);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            gridViewBranches.EditIndex = -1;
            BindBranchesGrid();
        }


        protected void gridViewBranches_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int branchID = Convert.ToInt32(gridViewBranches.DataKeys[e.RowIndex].Value);

            // Delete the branch from the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Branches WHERE BranchID = @BranchID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BranchID", branchID);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            BindBranchesGrid();
        }

        private void ClearForm()
        {
            txtBranchName.Text = string.Empty;
        }
        
        private void BindBranchesGrid()
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

            // Apply pagination logic
            int pageSize = 5; // Number of items to display per page
            int pageIndex = 0; // Current page index (0-based)
            if (Request.QueryString["page"] != null)
            {
                int.TryParse(Request.QueryString["page"], out pageIndex);
            }

            // Calculate the starting index and ending index of the items to display
            int startIndex = pageIndex * pageSize;
            int endIndex = Math.Min(startIndex + pageSize - 1, branches.Count - 1);

            // Create a new list to hold the items for the current page
            List<BranchTemp> pageData = new List<BranchTemp>();

            // Populate the new list with the items for the current page
            for (int i = startIndex; i <= endIndex; i++)
            {
                BranchTemp tempBranch = new BranchTemp();
                tempBranch.BranchID = branches[i].BranchID;
                tempBranch.BranchName = branches[i].BranchName;
                pageData.Add(tempBranch);
            }

            // Bind the GridView control to the data for the current page
            gridViewBranches.DataSource = pageData;
            gridViewBranches.DataBind();

            // Generate pagination links
            GeneratePaginationLinks(pageIndex, pageSize, branches.Count);
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

    public class BranchTemp
    {
        public int BranchID { get; set; }
        public string BranchName { get; set; }
    }
}







