using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class sCourseManagement : System.Web.UI.Page
    {
        protected void btnBackDashboard_Click(object sender, EventArgs e)
        {
            // Redirect back to adminhomepage.aspx
            Response.Redirect("adminhomepage.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if the user has admin access during page load
            if (Session["SAdminAccess"] == null)
            {
                // Redirect to the login page for users without admin access
                Response.Redirect("userlogin.aspx");
            }

            if (!IsPostBack)
            {
                if (Request.QueryString["status"] == "success")
                {
                    // Register a client-side script to show the success message
                    string script = "swal('Success', 'Course created successfully.', 'success');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowSuccessMessage", script, true);
                }

                BindGridViewCourses();

            }
        }

        protected void txtSearchCourse_TextChanged(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                string query = @"
            SELECT CourseID, CourseCode, CourseName, CourseCategory, CourseLevel, CourseLanguage, Status
            FROM Course
            WHERE CourseCode LIKE @searchText OR CourseName LIKE @searchText";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // Add the search text parameter
                    cmd.Parameters.AddWithValue("@searchText", "%" + txtSearchCourse.Text.Trim() + "%");

                    con.Open();

                    // Execute the query and bind the results to GridViewCourses
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        GridViewCourses.DataSource = dt;
                        GridViewCourses.DataBind();
                    }

                    con.Close();
                }
            }
        }


        protected void GridViewCourses_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewCourses.PageIndex = e.NewPageIndex;
            BindGridViewCourses();
        }

        private void BindGridViewCourses()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // The query to select data from the Course table
                string query = "SELECT CourseID, CourseCode, CourseName, CourseCategory, CourseLevel, CourseLanguage, Status FROM Course";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        GridViewCourses.DataSource = dt;
                        GridViewCourses.DataBind();
                    }

                    con.Close();
                }
            }
        }

        private void ShowSweetAlert(string title, string message, string type)
        {
            string script = $"Swal.fire({{ title: '{title}', text: '{message}', icon: '{type}', confirmButtonText: 'Ok' }});";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "sweetAlert", script, true);
        }


        protected void GridViewCourses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Example: Change the color based on the course status
                string status = e.Row.Cells[6].Text; // Assuming the 'Status' is in the 7th column (index 6)

                switch (status)
                {
                    case "Approved":
                        e.Row.Cells[6].ForeColor = System.Drawing.Color.Green;
                        break;
                    case "Pending":
                        e.Row.Cells[6].ForeColor = System.Drawing.Color.Orange;
                        break;
                    case "Rejected":
                        e.Row.Cells[6].ForeColor = System.Drawing.Color.Red;
                        break;
                    case "Uncompleted Course":
                        e.Row.Cells[6].ForeColor = System.Drawing.Color.Red;
                        break;
                        // Add more cases as needed
                }

                // Other custom row operations can be added here
            }
        }

        protected void GridViewCourses_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dataTable = GetCourseData(); // Fetch the current data to be sorted

            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                string sortDirection = GetSortDirection(e.SortExpression);

                dataView.Sort = e.SortExpression + " " + sortDirection;
                GridViewCourses.DataSource = dataView;
                GridViewCourses.DataBind();
            }
        }

        private string GetSortDirection(string column)
        {
            // By default, set the sort direction to ascending
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                // Check if the same column is being sorted
                // Otherwise, the default value can be returned
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            // Save new values in ViewState
            ViewState["SortExpression"] = column;
            ViewState["SortDirection"] = sortDirection;

            return sortDirection;
        }

        private DataTable GetCourseData()
        {
            DataTable dataTable = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Query to select data from the Course table
                string query = "SELECT CourseID, CourseCode, CourseName, CourseCategory, CourseLevel, CourseLanguage, ImageURL, CreationDate, Status, InstructorID, RequiredHours FROM Course";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();

                    // Using SqlDataAdapter to fill the dataTable
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }

                    con.Close();
                }
            }

            return dataTable;
        }

        protected void GridViewCourses_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "View")
            {
                string courseCode = (string)e.CommandArgument;
                Response.Redirect("viewCourse.aspx?CourseCode=" + Server.UrlEncode(courseCode));
            }
            else if (e.CommandName == "Edit")
            {

                int index = Convert.ToInt32(e.CommandArgument);
                string courseCode = GridViewCourses.DataKeys[index].Value.ToString();

                Response.Redirect("editCourse.aspx?CourseCode=" + Server.UrlEncode(courseCode));
            }
        }

        protected void btnRefreshCourses_ServerClick(object sender, EventArgs e)
        {
            BindGridViewCourses();
            string script = "showSuccessMessage();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccessScript", script, true);
        }


        protected void btnShowCreateCourseModal_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddCourse.aspx");
        }


        protected void MarkAsCorrect_Click(object sender, EventArgs e)
        {
            // Get the CourseCode from the button's CommandArgument
            Button button = (Button)sender;
            string courseCode = button.CommandArgument.ToString();

            // Update the course status to Approved based on the courseCode
            bool success = UpdateCourseStatus(courseCode, "Approved");

            if (success)
            {
                ShowSweetAlert("Approved!", "Course approved!", "success");
            }


            // Refresh the GridView or perform any other necessary actions
            BindGridViewCourses();
        }

        protected void MarkAsFalse_Click(object sender, EventArgs e)
        {
            // Get the CourseCode from the button's CommandArgument
            Button button = (Button)sender;
            string courseCode = button.CommandArgument.ToString();

            // Update the course status to Rejected based on the courseCode
            bool success = UpdateCourseStatus(courseCode, "Rejected");

            if (success)
            {
                ShowSweetAlert("Rejected!", "Course has been rejected!", "success");
            }


            // Refresh the GridView or perform any other necessary actions
            BindGridViewCourses();
        }

        private bool UpdateCourseStatus(string courseCode, string newStatus)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Define the SQL query to update the course status
                    string updateQuery = "UPDATE Course SET Status = @NewStatus WHERE CourseCode = @CourseCode";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@NewStatus", newStatus);
                        command.Parameters.AddWithValue("@CourseCode", courseCode);

                        int rowsAffected = command.ExecuteNonQuery();

                        // Return true if at least one row was affected (update successful)
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the database operation
                Console.WriteLine("Error updating course status: " + ex.Message);

                // Return false to indicate that the update failed
                return false;
            }
        }
    }
}