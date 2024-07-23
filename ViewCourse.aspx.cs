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
    public partial class ViewCourse : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string courseCode = Request.QueryString["CourseCode"];
                BindCourseDetails(courseCode);
                BindModuleList();
                BindAssessmentGridView();
            }
        }

        protected void GridViewAssessment_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "View")
            {
                // Assuming your CommandArgument is the CourseAssessmentID
                int courseAssessmentId = Convert.ToInt32(e.CommandArgument);

                // Store the CourseAssessmentID in ViewState to use later, for example, when populating the questions and answers
                ViewState["viewCourseAssessmentId"] = courseAssessmentId;

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#detailsModal').modal();", true);

                // Call a method to load the questions and answers for the selected assessment
                LoadQuestionsAndAnswers(courseAssessmentId); // Replace with your actual method to load data
            }
        }

        protected void GridViewQuestions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Repeater RepeaterAnswers = e.Row.FindControl("RepeaterAnswers") as Repeater;
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                int assessmentQuestionId = Convert.ToInt32(rowView["AssessmentQuestionID"]);

                string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT AnswerName, IsCorrect FROM AssessmentAnswer WHERE AssessmentQuestionID = @AssessmentQuestionID", con))
                    {
                        cmd.Parameters.AddWithValue("@AssessmentQuestionID", assessmentQuestionId);
                        con.Open();
                        DataTable dtAnswers = new DataTable();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dtAnswers);
                        }
                        RepeaterAnswers.DataSource = dtAnswers;
                        RepeaterAnswers.DataBind();
                    }
                }
            }
        }

        private void LoadQuestionsAndAnswers(int courseAssessmentId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT DISTINCT q.AssessmentQuestionID, q.Question
            FROM AssessmentQuestion q
            WHERE q.CourseAssessmentID = @CourseAssessmentID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CourseAssessmentID", courseAssessmentId);
                    ViewState["courseAssessmentIDQuestionAnswer"] = courseAssessmentId;
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        GridViewQuestions.DataSource = dt;
                        GridViewQuestions.DataBind();
                    }
                }
            }
        }


        private void BindCourseDetails(string courseCode)
        {
      

            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Course WHERE CourseCode = @CourseCode";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CourseCode", courseCode);

                    try
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                lblCourseCode.InnerText = reader["CourseCode"].ToString();
                                lblCourseName.InnerText = reader["CourseName"].ToString();
                                lblCourseCategory.InnerText = reader["CourseCategory"].ToString();
                                lblCourseLevel.InnerText = reader["CourseLevel"].ToString();
                                lblCourseLanguage.InnerText = reader["CourseLanguage"].ToString();
                                string imageUrl = reader["ImageURL"].ToString();
                              
                                imageUrl = imageUrl + ".png";
                            
                                imgCourseImage.ImageUrl = imageUrl;
                                lblCreationDate.InnerText = Convert.ToDateTime(reader["CreationDate"]).ToString("yyyy-MM-dd");
                                lblStatus.InnerText = reader["Status"].ToString();
                                lblInstructorID.InnerText = reader["InstructorID"].ToString();
                                lblRequiredHours.InnerText = reader["RequiredHours"].ToString();
                            }
                           
                        }
                    }
                    catch (Exception ex)
                    {
                      
                    }
                }
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "View")
            {
                // Get the associationCriteria from the CommandArgument
                string associationCriteria = e.CommandArgument.ToString();

                // Fetch the details from the database based on AssociationCriteria
                BindModuleContentDetails(associationCriteria);

                // Show the modal
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#moduleContentModal').modal();", true);
            }
        }


        private string GetYouTubeEmbeddedPlayer(string videoUrl)
        {
            string videoId = GetYouTubeVideoId(videoUrl);
            return $"<iframe width='560' height='315' src='https://www.youtube.com/embed/{videoId}' frameborder='0' allowfullscreen></iframe>";
        }

        private string GetYouTubeVideoId(string videoUrl)
        {
            var uri = new Uri(videoUrl);
            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
            return query["v"];
        }


        private void BindModuleContentDetails(string associationCriteria)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
SELECT 
    ModuleContentTitle,
    ModuleContentType,
    ModuleContentImage,
    ModuleContentVideo,
    ModuleContentText,
    CourseModuleNo,
    CourseCode
FROM 
    ModuleContent
WHERE 
    AssociationCriteria = @AssociationCriteria";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@AssociationCriteria", associationCriteria);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Set the text of labels
                        lblModuleContentTitle.Text = reader["ModuleContentTitle"].ToString();
                        lblModuleContentType.Text = reader["ModuleContentType"].ToString();
                        lblCourseModuleNo.Text = reader["CourseModuleNo"].ToString();
                        Label1.Text = reader["CourseCode"].ToString();

                        // Handle image content
                        string imageUrl = reader["ModuleContentImage"].ToString();
                        imgModuleContentImage.Visible = !string.IsNullOrEmpty(imageUrl);
                        imgModuleContentImage.ImageUrl = imageUrl;

                        // Handle video content
                        string videoUrl = reader["ModuleContentVideo"].ToString();
                        divModuleContentVideo.Visible = !string.IsNullOrEmpty(videoUrl);
                        if (divModuleContentVideo.Visible)
                        {
                            if (videoUrl.Contains("youtube.com") || videoUrl.Contains("youtu.be"))
                            {
                                divModuleContentVideo.InnerHtml = GetYouTubeEmbeddedPlayer(videoUrl);
                            }
                            else
                            {
                                divModuleContentVideo.InnerHtml = $"<video width='100%' controls><source src='{videoUrl}' type='video/mp4'>Your browser does not support the video tag.</video>";
                            }
                        }

                        // Handle text content
                        string textContent = HttpUtility.HtmlDecode(reader["ModuleContentText"].ToString());
                        divModuleContentText.Visible = !string.IsNullOrEmpty(textContent);
                        divModuleContentText.InnerHtml = textContent;
                    }
                    else
                    {
                        // Hide the controls if no data is found
                        lblModuleContentTitle.Visible = false;
                        lblModuleContentType.Visible = false;
                        imgModuleContentImage.Visible = false;
                        divModuleContentVideo.Visible = false;
                        divModuleContentText.Visible = false;
                        lblCourseModuleNo.Visible = false;
                        Label1.Visible = false;
                    }
                }
                con.Close();
            }
        }

        private void BindAssessmentGridView()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
                string query = @"
            SELECT 
                    a.CourseAssessmentID, 
                    a.CourseCode, 
                    mc.ModuleContentTitle, 
                    a.AssessmentType
                FROM 
                    CourseAssessment a
                    LEFT JOIN CourseModule cm ON a.CourseModuleID = cm.CourseModuleID
                    LEFT JOIN ModuleContent mc ON cm.AssociationCriteria = mc.AssociationCriteria
                WHERE 
                    a.CourseCode = @CourseCode;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        string courseCode = Request.QueryString["CourseCode"];

                        command.Parameters.AddWithValue("@CourseCode", courseCode);
                        connection.Open();

                        DataTable dt = new DataTable();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dt);
                        }

                        GridViewAssessment.DataSource = dt;
                        GridViewAssessment.DataBind();

                        if (dt.Rows.Count == 0)
                        {
                            GridViewAssessment.EmptyDataText = "No assessments created.";
                            GridViewAssessment.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error binding assessment data: " + ex.Message);
            }
        }



        private void BindModuleList()
        {

            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            string query = @"
                SELECT
                    CM.CourseModuleID,
                    CM.CourseModuleNo,
                    CM.CourseCode,
                    MC.ModuleContentTitle,
                    MC.ModuleContentType,
                    MC.AssociationCriteria
                FROM
                    CourseModule CM
                INNER JOIN
                    ModuleContent MC ON CM.CourseModuleNo = MC.CourseModuleNo
                WHERE
                    CM.CourseCode = @CourseCode
                    AND MC.CourseCode = @CourseCode
                    AND CM.AssociationCriteria = MC.AssociationCriteria;";





            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    string courseCode = Request.QueryString["CourseCode"];
                    command.Parameters.AddWithValue("@CourseCode", courseCode);
                    connection.Open();
                    DataTable dt = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                    GridView1.DataSource = dt;
                    GridView1.DataBind();

                    if (dt.Rows.Count == 0)
                    {
                        GridView1.EmptyDataText = "No modules created.";
                        GridView1.DataBind();
                    }
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("coursemanagement.aspx");
        }

    }

   


}