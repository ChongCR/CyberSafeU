using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class courseevaluation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int loggedInUserID =  GetLoggedInUserID();

                if (loggedInUserID != -1)
                {
                    // User is logged in, use the user ID as needed
                    BindCourseCompletedGridView();
                }
                else
                {
                   // // User is not logged in, redirect to the login or signup page
                   // Response.Redirect("~/userlogin.aspx");
                }
            }
        }



        private int GetLoggedInUserID()
        {
            // Check if the user ID is stored in the session
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["user_id"] != null)
            {
                // Convert the stored user ID to an integer and return it
                if (int.TryParse(HttpContext.Current.Session["user_id"].ToString(), out int userId))
                {
                    return userId;
                }
            }

            // Return -1 if the user is not logged in or if the user ID is not available
            return -1;
        }




        protected void btnRefresh_ServerClick(object sender, EventArgs e)
        {
          

        }

        private void BindCourseCompletedGridView()
        {
            // Retrieve the connection string from configuration
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            // Create a SqlConnection with the retrieved connection string
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Define a SQL query to retrieve data from Course_Completed
                string query = "SELECT CompletionID, UserID, CourseID, CompletionDate, Status FROM Course_Completed";

                // Create a SqlCommand object with the query and connection
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        // Execute the query and retrieve data into a DataTable
                        DataTable dt = new DataTable();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            
                            dt.Load(reader);
                        }

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            if (reader.Read()) // Check if there is a row
                            {
                                // Access the CompletionID for the first row and store it in the Session variable
                                Session["CompletionID"] = reader["CompletionID"];

                            }
                        }

                       

                        // Bind the DataTable to the GridView control
                        GridViewReview.DataSource = dt;
                        GridViewReview.DataBind();
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions (e.g., display an error message)
                        // You can replace this with your error handling logic
                        ShowSweetAlert("Error", "An error occurred while fetching course completions: " + ex.Message, "error");
                    }
                }
            }
        }



        protected void GridViewReview_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Get the current data source for the GridView
            DataTable dt = (DataTable)GridViewReview.DataSource;

            // Check if the data source exists
            if (dt != null)
            {
                // Use the DataView to perform sorting on the DataTable
                DataView dv = new DataView(dt);

                // Set the Sort direction based on the GridViewSortEventArgs
                string sortDirection = "ASC";
                if (ViewState["SortDirection"] != null)
                {
                    if (ViewState["SortDirection"].ToString() == "ASC")
                    {
                        sortDirection = "DESC";
                    }
                }

                // Set the SortExpression for sorting
                dv.Sort = e.SortExpression + " " + sortDirection;

                // Rebind the GridView with the sorted data
                GridViewReview.DataSource = dv;
                GridViewReview.DataBind();

                // Update the ViewState to store the current SortDirection
                ViewState["SortDirection"] = sortDirection;
            }
        }



        private DataTable GetGVDataSource()
        {
            return null;
        }



    
        private void bindReviewDetails()
        {
           
        }

        protected void GridViewReview_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "View")
            {
                // Retrieve the CourseID from the CommandArgument
                int courseID = Convert.ToInt32(e.CommandArgument);

                // Query the Course table to fetch course details based on the courseID
                string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT CourseCode, CourseName, CourseCategory, CourseLevel, CourseLanguage, ImageURL, CreationDate, Status, InstructorID, RequiredHours FROM Course WHERE CourseID = @CourseID";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@CourseID", courseID);
                        try
                        {
                            connection.Open();
                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                // Retrieve course details from the SqlDataReader
                                string courseCode = reader["CourseCode"].ToString();
                                string courseName = reader["CourseName"].ToString();
                                string courseCategory = reader["CourseCategory"].ToString();
                                string courseLevel = reader["CourseLevel"].ToString();
                                string courseLanguage = reader["CourseLanguage"].ToString();
                                string imageURL = reader["ImageURL"].ToString();
                                DateTime creationDate = Convert.ToDateTime(reader["CreationDate"]);
                                string status = reader["Status"].ToString();
                                int instructorID = Convert.ToInt32(reader["InstructorID"]);
                                int requiredHours = Convert.ToInt32(reader["RequiredHours"]);

                                // Set the course details in JavaScript variables
                                string script = $@"
                            $('#courseCode').text('{courseCode}');
                            $('#courseName').text('{courseName}');
                            $('#courseCategory').text('{courseCategory}');
                            $('#courseLevel').text('{courseLevel}');
                            $('#courseLanguage').text('{courseLanguage}');
                            $('#imageURL').attr('src', '{imageURL}');
                            $('#creationDate').text('{creationDate.ToString("yyyy-MM-dd")}');
                            $('#status').text('{status}');
                            $('#instructorID').text('{instructorID}');
                            $('#requiredHours').text('{requiredHours}');
                            $('#courseDetailsModal').modal('show');
                        ";

                                ClientScript.RegisterStartupScript(this.GetType(), "ShowCourseDetails", script, true);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Handle any exceptions (e.g., display an error message)
                            ShowSweetAlert("Error", "An error occurred while fetching course details: " + ex.Message, "error");
                        }
                    }
                }
            }


            if (e.CommandName == "Select" && !string.IsNullOrEmpty(e.CommandArgument.ToString()))
            {
                int courseID = Convert.ToInt32(e.CommandArgument);
                int userID = Convert.ToInt32(Session["user_id"]); // Replace with the actual user ID

                // Check if the user has already submitted an evaluation for this course
                bool evaluationExists = CheckIfEvaluationExists(userID, courseID);

                // Check the status for the selected course from the database
                string courseName = GetCourseNameFromDatabase(courseID); // Replace with your database query

                if (!string.IsNullOrEmpty(courseName))
                {
                    // Course is completed, display course details in the left info box
                    courseIDtxt.Text = courseID.ToString();
                    course_completed.Text = courseName; // Set the course name in the TextBox
                    course_review.ReadOnly = false;

                    if (evaluationExists)
                    {
                        // User has already submitted an evaluation for this course, hide the review button
                        btn_SubmitReview.Visible = false;
                        ShowSweetAlert("Review Already Submitted", "You have already reviewed this course.", "error");
                    }
                    else
                    {
                        // User has not submitted an evaluation, show the review button
                        btn_SubmitReview.Visible = true;
                    }
                }
                else
                {
                    // Course is not found or an error occurred
                    ShowSweetAlert("Course Not Found", "The course information could not be retrieved.", "error");
                }
            }



        }
        private bool CheckIfEvaluationExists(int userID, int courseCompletedID)
        {
            bool evaluationExists = false;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Query to check if an evaluation exists for the specified user and course completion
                    string query = "SELECT COUNT(*) FROM Course_Evaluation WHERE user_id = @UserID AND course_completed = @CourseCompletedID";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.Parameters.AddWithValue("@CourseCompletedID", courseCompletedID);

                    int evaluationCount = Convert.ToInt32(cmd.ExecuteScalar());

                    if (evaluationCount > 0)
                    {
                        // An evaluation exists for the user and course completion
                        evaluationExists = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur while connecting to the database
                // You can log the exception or handle it as needed
                Console.WriteLine("Error checking evaluation existence: " + ex.Message);
            }

            return evaluationExists;
        }

        private string GetCourseNameFromDatabase(int courseID)
        {
            string courseName = string.Empty;

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT CourseName FROM Course WHERE CourseID = @CourseID";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@CourseID", courseID);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        courseName = result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur while connecting to the database
                // You can log the exception or handle it as needed
                Console.WriteLine("Error retrieving course name: " + ex.Message);
            }

            return courseName;
        }




        protected string GetCourseStatusFromDatabase(int courseID)
        {
            string status = string.Empty;

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT Status FROM Course_Completed WHERE CourseID = @CourseID";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@CourseID", courseID);

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        status = result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur while connecting to the database
                // You can log the exception or handle it as needed
                Console.WriteLine("Error retrieving course status: " + ex.Message);
            }

            return status;
        }

        protected string GetStatusCssClass(object status)
        {
            if (status != null)
            {
                string statusValue = status.ToString();
                switch (statusValue)
                {
                    case "Completed":
                        return "status-completed"; // CSS class for green text
                    case "Uncompleted":
                        return "status-uncompleted"; // CSS class for orange text
                    case "Failed":
                        return "status-failed"; // CSS class for red text
                    default:
                        return ""; // Default CSS class
                }
            }
            return "";
        }


        protected void GridViewReview_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if (GridViewReview.SelectedIndex >= 0)
            {
           
                GridViewRow selectedRow = GridViewReview.SelectedRow;

            
                string courseCompleted = selectedRow.Cells[1].Text; 
                string dateSubmitted = selectedRow.Cells[2].Text; 
                string courseRatings = selectedRow.Cells[3].Text; 

        
                GridViewReview.SelectedIndex = -1;
            }
        }




        protected void GridViewReview_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
          
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
         
        }

        
        private void BindReviewGridView(int userID, string searchText = "")
        {
           
        }

        protected void btn_SubmitReview_Click(object sender, EventArgs e)
        {

            string review = course_review.Text; // Review text
            string starRatingStr = ratingSelect.Text.ToString();  // Access the selected star rating as a string

           
                int courseCompletedID = Convert.ToInt32(courseIDtxt.Text);
            int userID = Convert.ToInt32(Session["user_id"]);

                // Insert the review into the database with starRating, review, courseCompletedID, and userID
                InsertReviewIntoDatabase(courseCompletedID, userID, DateTime.Now, int.Parse(starRatingStr), review);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "SuccessAlert", "showSuccessAlert();", true);

        }

        private void InsertReviewIntoDatabase(int courseCompletedID, int userID, DateTime dateSubmitted, int starRating, string review)
        {
           
                string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Course_Evaluation (course_completed, user_id, date_submitted, course_ratings, course_review) VALUES (@CourseCompletedID, @UserID, @DateSubmitted, @StarRating, @Review)";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@CourseCompletedID", Session["CompletionID"]);
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.Parameters.AddWithValue("@DateSubmitted", dateSubmitted);
                    cmd.Parameters.AddWithValue("@StarRating", starRating);
                    cmd.Parameters.AddWithValue("@Review", review);

                    cmd.ExecuteNonQuery();
                }
            
        }


        //check if review is made
        private bool IsReviewExists(int courseCompleted, int userID)
        {
            return false;
        }


        private void ShowSweetAlert(string title, string message, string type)
        {
            string script = $"Swal.fire({{ title: '{title}', text: '{message}', icon: '{type}', confirmButtonText: 'Ok' }});";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "sweetAlert", script, true);
        }

    }
}
