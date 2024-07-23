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
    public partial class assessment : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.QueryString["AssessmentID"] == null)
            {
                Response.Redirect("courseoverview.aspx");
                return; // Make sure to return here so the rest of the code does not execute
            }

            if (!IsPostBack)
            {
                PopulateQuestions();
            }
        }

        private void PopulateQuestions()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            if (int.TryParse(Request.QueryString["AssessmentID"], out int assessmentId))
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT 
                            AssessmentQuestionID,
                            Question
                        FROM 
                            AssessmentQuestion
                        WHERE
                            CourseAssessmentID = @AssessmentID";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@AssessmentID", assessmentId);
                        con.Open();
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        rptQuestions.DataSource = dt;
                        rptQuestions.DataBind();
                    }
                }
            }
        }

        protected void rptQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CheckBoxList cblAnswers = (CheckBoxList)e.Item.FindControl("cblAnswers");
                int questionId = (int)DataBinder.Eval(e.Item.DataItem, "AssessmentQuestionID");

                string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // This query will retrieve answers in a random order for each question
                    string query = @"
                SELECT 
                    AssessmentAnswerID,
                    AnswerName
                FROM 
                    AssessmentAnswer
                WHERE
                    AssessmentQuestionID = @AssessmentQuestionID
                ORDER BY NEWID()";  // NEWID() generates a random order

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@AssessmentQuestionID", questionId);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            cblAnswers.DataSource = reader;
                            cblAnswers.DataValueField = "AssessmentAnswerID";
                            cblAnswers.DataTextField = "AnswerName";
                            cblAnswers.DataBind();
                        }
                    }
                }
            }
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int totalQuestions = 0;
            int userCorrectQuestions = 0;
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            // Calculate the score based on user's answers for each question
            foreach (RepeaterItem item in rptQuestions.Items)
            {
                CheckBoxList cblAnswers = (CheckBoxList)item.FindControl("cblAnswers");
                HiddenField hdnQuestionId = (HiddenField)item.FindControl("hdnQuestionId");
                int questionId = int.Parse(hdnQuestionId.Value);

                bool isAllCorrect = true; // Flag to check if all selected answers are correct

                foreach (ListItem answer in cblAnswers.Items)
                {
                    bool isAnswerCorrect = IsAnswerCorrect(int.Parse(answer.Value), connectionString);

                    if (answer.Selected != isAnswerCorrect)
                    {
                        // If the selected answer does not match the correct answer, mark the question as incorrect
                        isAllCorrect = false;
                        break; // Break out of the loop as at least one incorrect answer is selected
                    }
                }

                if (isAllCorrect)
                {
                    userCorrectQuestions++;
                }

                totalQuestions++;

                // Mark the correct answers for this question
                MarkCorrectAnswers(cblAnswers, questionId);
            }

            double score = totalQuestions > 0 ? (double)userCorrectQuestions / totalQuestions * 100 : 0;
            lblScore.Text = $"You scored {score:0.##}% correct questions.";
            lblScore.Visible = true;

            // Insert the result into the database
            int userId = Convert.ToInt32(Session["user_id"]);
            int assessmentId = int.Parse(Request.QueryString["AssessmentID"]);
            string assessmentType = GetAssessmentType(assessmentId, connectionString);

            InsertAssessmentResult(assessmentId, score, userId);
            btnSubmit.Visible = false;
            btnBack.Visible = false;
            btnBackDone.Visible = true;

            // Check if the user has enrolled in a course and add to Course_Completed if not
            string courseCode = GetCourseCodeForAssessment(assessmentId, connectionString);
            if (!string.IsNullOrEmpty(courseCode))
            {
                int courseId = GetCourseIdByCode(courseCode, connectionString);
                if (courseId > 0)
                {
                    // Insert into Course_Completed
                    InsertCourseCompleted(userId, courseId, connectionString);

                    if (assessmentType == "Final" && score >= 70)
                    {
                        // Update the status to "Completed"
                        UpdateCourseStatus(userId, courseId, "Completed", connectionString);
                    }
                }
            }
        }




        // GetAssessmentType function to retrieve the assessment type based on assessment ID
        private string GetAssessmentType(int assessmentId, string connectionString)
        {
            string assessmentType = string.Empty;

          
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT AssessmentType FROM CourseAssessment  WHERE CourseAssessmentID  = @AssessmentID";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@AssessmentID", assessmentId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            assessmentType = reader["AssessmentType"].ToString();
                            Console.WriteLine("Assessment Type Retrieved: " + assessmentType); // Log the assessment type
                        }
                    }
                }
            
            
            

            return assessmentType;
        }


        // UpdateCourseStatus function to update the course status in the database
        private void UpdateCourseStatus(int userId, int courseId, string status, string connectionString)
        {
            Console.WriteLine($"UpdateCourseStatus called with UserID: {userId}, CourseID: {courseId}, Status: {status}");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
            UPDATE Course_Completed 
            SET Status = @Status
            WHERE UserID = @UserID AND CourseID = @CourseID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@CourseID", courseId);
                    cmd.Parameters.AddWithValue("@Status", status);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine($"Rows affected: {rowsAffected}");
                }
            }
        }






        // Function to get the course code for the given assessment
        private string GetCourseCodeForAssessment(int assessmentId, string connectionString)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT CA.CourseCode
            FROM CourseAssessment CA
            WHERE CA.CourseAssessmentID  = @AssessmentID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@AssessmentID", assessmentId);
                    con.Open();
                    return (string)cmd.ExecuteScalar();
                }
            }
        }

        // Function to get the course ID by course code
        private int GetCourseIdByCode(string courseCode, string connectionString)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT CourseID
            FROM Course
            WHERE CourseCode = @CourseCode";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CourseCode", courseCode);
                    con.Open();
                    var result = cmd.ExecuteScalar();
                    return result != null ? (int)result : 0;
                }
            }
        }

     
        private void InsertCourseCompleted(int userId, int courseId, string connectionString)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
               
                string checkQuery = @"
            SELECT COUNT(*) 
            FROM Course_Completed 
            WHERE UserID = @UserID AND CourseID = @CourseID";

                using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                {
                    checkCmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(userId));//modify here 
                    checkCmd.Parameters.AddWithValue("@CourseID", courseId);
                    con.Open();

                    int existingRecordCount = (int)checkCmd.ExecuteScalar();

                    if (existingRecordCount == 0)
                    {
                        // No existing record found, insert a new record
                        string insertQuery = @"
                    INSERT INTO Course_Completed (UserID, CourseID, CompletionDate, Status)
                    VALUES (@UserID, @CourseID, NULL, 'Uncompleted')";

                        using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(userId));  //modify here 
                            cmd.Parameters.AddWithValue("@CourseID", courseId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                  
                }
            }
        }



        protected void btnBack_Click(object sender, EventArgs e)
        {
            // Retrieve the courseID from the query string or session
            string courseID = Request.QueryString["courseID"];

            // Redirect to course.aspx with the courseID as a query string parameter
            Response.Redirect($"course.aspx?courseID={Session["courseID"]}");
        }

        protected void btnConfirmBack_Click(object sender, EventArgs e)
        {
            // Retrieve the courseID from the query string or session
            string courseID = Request.QueryString["courseID"];

            // Redirect to course.aspx with the courseID as a query string parameter
            Response.Redirect($"course.aspx?courseID={Session["courseID"]}");
        }




    
        private int GetCorrectAnswerCountForQuestion(int questionId, string connectionString)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT COUNT(*)
            FROM AssessmentAnswer
            WHERE AssessmentQuestionID = @AssessmentQuestionID AND IsCorrect = 1";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@AssessmentQuestionID", questionId);
                    con.Open();
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        private bool IsAnswerCorrect(int answerId, string connectionString)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT IsCorrect
            FROM AssessmentAnswer
            WHERE AssessmentAnswerID = @AssessmentAnswerID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@AssessmentAnswerID", answerId);
                    con.Open();
                    return (bool)cmd.ExecuteScalar();
                }
            }
        }


        private void InsertAssessmentResult(int courseAssessmentId, double score, int userId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
            INSERT INTO AssessmentResult (CourseAssessmentID, Score, user_id)
            VALUES (@CourseAssessmentID, @Score, @UserID)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CourseAssessmentID", courseAssessmentId);
                    cmd.Parameters.AddWithValue("@Score", score);
                    cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(userId));

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


        private void MarkCorrectAnswers(CheckBoxList cblAnswers, int questionId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT AssessmentAnswerID, IsCorrect
            FROM AssessmentAnswer
            WHERE AssessmentQuestionID = @QuestionID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@QuestionID", questionId);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Find the answer in the CheckBoxList and determine if it is correct
                            ListItem answerItem = cblAnswers.Items.FindByValue(reader["AssessmentAnswerID"].ToString());
                            if (answerItem != null && (bool)reader["IsCorrect"])
                            {
                                // Apply a CSS class to highlight the correct answer
                                answerItem.Attributes.CssStyle.Add("background-color", "lightgreen");
                            }
                        }
                    }
                }
            }
        }


    }

}