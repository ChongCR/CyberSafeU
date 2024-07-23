using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class course : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ViewState["SelectedContentId"] != null)
            {
                int moduleContentId = (int)ViewState["SelectedContentId"];
                DisplayContent(moduleContentId); // Re-display the content
            }


            if (Request.QueryString["courseID"] != null)
            {
                Session["courseID"] = Request.QueryString["courseID"];
                rptModules.ItemCommand += rptModuleContent_ItemCommand;
                string courseID = Request.QueryString["courseID"];
                string courseTitle = GetCourseTitle(Convert.ToInt32(courseID)); 
                litCourseTitle.Text = courseTitle;
                Page.Title = courseTitle;

                // Fetch all module data for the course at once
                DataTable allModuleData = GetModuleData(Convert.ToInt32(courseID));

                // Create a list to hold grouped module data
                List<ModuleGroup> groupedModuleData = new List<ModuleGroup>();

                // LINQ query to group module data by module number
                var groupedData = from row in allModuleData.AsEnumerable()
                                  group row by row.Field<int>("CourseModuleNo") into moduleGroup
                                  select new ModuleGroup
                                  {
                                      ModuleNo = moduleGroup.Key,
                                      ModuleContents = moduleGroup.CopyToDataTable()
                                  };

                // Populate the groupedModuleData list with the grouped data
                groupedModuleData = groupedData.ToList();

                // Now bind the groupedModuleData to the rptModules
                rptModules.DataSource = groupedModuleData;
                rptModules.DataBind();
                multiView.SetActiveView(view1);
            }
            else
            {
                Response.Redirect("courseoverview.aspx");
            }
        }


        public class ModuleGroup
        {
            public int ModuleNo { get; set; }
            public DataTable ModuleContents { get; set; }
        }

        protected void ContentItem_Command(object sender, CommandEventArgs e)
        {
            int moduleContentId = Convert.ToInt32(e.CommandArgument);
            ViewState["SelectedContentId"] = moduleContentId; // Storing the selected content ID in ViewState
            DisplayContent(moduleContentId); // Call a method to display content
        }

        private void DisplayContent(int moduleContentId)
        {
            DataTable dt = GetModuleContentById(moduleContentId);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                lblModuleContentTitle.InnerText = row["ModuleContentTitle"].ToString();
                ltModuleContentText.Text = HttpUtility.HtmlDecode(row["ModuleContentText"].ToString());

                if (!string.IsNullOrEmpty(row["ModuleContentImage"].ToString()))
                {
                    imgModuleContentImage.ImageUrl = row["ModuleContentImage"].ToString();
                    imgModuleContentImage.Visible = true;
                }
                else
                {
                    imgModuleContentImage.Visible = false;
                }

                string youtubeLink = row["ModuleContentVideo"].ToString();
                if (!string.IsNullOrEmpty(youtubeLink))
                {
                    string embedUrl = GetEmbedUrlFromYoutubeLink(youtubeLink);
                    iframeVideo.Attributes["src"] = embedUrl;
                    iframeVideo.Visible = true;
                }
                else
                {
                    iframeVideo.Visible = false;
                }
            }
        }


        private string GetEmbedUrlFromYoutubeLink(string youtubeLink)
        {
            var uri = new Uri(youtubeLink);
            var query = HttpUtility.ParseQueryString(uri.Query);
            var videoId = query["v"];
            return $"https://www.youtube.com/embed/{videoId}";
        }


        private DataTable GetModuleContentDetailsById(int moduleContentId)
        {
            DataTable dt = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["YourConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM ModuleContent WHERE ModuleContentID = @ModuleContentID", connection))
                {
                    command.Parameters.AddWithValue("@ModuleContentID", moduleContentId);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dt);
                }
            }

            return dt;


        }
        protected void rptModuleContent_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SelectContent")
            {
                int moduleContentId = Convert.ToInt32(e.CommandArgument);
                DisplayContent(moduleContentId);
            }
        }

        private DataTable GetModuleContentById(int moduleContentId)
        {
            DataTable dt = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM ModuleContent WHERE ModuleContentID = @ModuleContentID", connection))
                {
                    command.Parameters.AddWithValue("@ModuleContentID", moduleContentId);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dt);
                }
            }

            return dt.Rows.Count > 0 ? dt : null;
        }

        protected void rptModules_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater rptModuleContent = e.Item.FindControl("rptModuleContent") as Repeater;
                rptModuleContent.ItemCommand += rptModuleContent_ItemCommand;
            }
        }

        private string GetCourseTitle(int courseID)
        {
            string courseTitle = string.Empty;

            // Get the connection string from Web.config
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            // Define the SQL query to fetch the course title
            string query = "SELECT CourseName FROM Course WHERE CourseID = @CourseID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CourseID", courseID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Retrieve the course title from the query result
                                courseTitle = reader["CourseName"].ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle any database connection or query errors here
                        // You can log the error or display an error message
                        // For simplicity, this example does not include detailed error handling
                    }
                }
            }

            return courseTitle;
        }

    


        private DataTable GetModuleData(int courseID)
        {
            // Define the connection string
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            // Define the SQL query to fetch all module data for the course
            string query = "SELECT ModuleContentID, ModuleContentTitle, CourseModuleNo, CourseCode " +
                           "FROM ModuleContent " +
                           "WHERE CourseCode = (SELECT CourseCode FROM Course WHERE CourseID = @CourseID) " +
                           "ORDER BY CourseModuleNo, ModuleContentTitle";

            DataTable moduleData = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CourseID", courseID);

                    try
                    {
                        connection.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(moduleData);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                return moduleData;
            }





        }

        protected void lnkAssessment_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "ChangeView")
            {
                // Convert the command argument to an integer and change the view state.
                int viewState = Convert.ToInt32(e.CommandArgument);
                ViewState["CurrentView"] = viewState;

                // Now you can check ViewState["CurrentView"] wherever needed to change the view of the page
                // For example, show/hide controls or change content based on this ViewState
            }
        }

        protected void btnAssessment_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "ChangeView")
            {

                multiView.SetActiveView(view2);

                // Your logic to switch the view, for example, showing the GridView
                gvAssessmentList.Visible = true;

                // Populate the GridView with assessment data
                PopulateAssessmentGridView();
            }
        }

        private void PopulateAssessmentGridView()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            int courseId;

            if (int.TryParse(Request.QueryString["courseID"], out courseId))
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        // Get the CourseCode for the given CourseID
                        string courseCodeQuery = "SELECT CourseCode FROM Course WHERE CourseID = @CourseID";
                        string courseCode = null;

                        using (SqlCommand courseCmd = new SqlCommand(courseCodeQuery, con))
                        {
                            courseCmd.Parameters.AddWithValue("@CourseID", courseId);
                            con.Open();
                            object result = courseCmd.ExecuteScalar();
                            con.Close();
                            if (result != null)
                            {
                                courseCode = result.ToString();
                            }
                        }

                        // If the CourseCode is retrieved successfully, fetch the assessment details with first and second try scores
                        if (!string.IsNullOrEmpty(courseCode))
                        {
                            string query = @"WITH FirstAndSecondTries AS (
                                            SELECT 
                                                AR.CourseAssessmentID,
                                                AR.Score,
                                                AR.user_id,
                                                ROW_NUMBER() OVER (PARTITION BY AR.CourseAssessmentID, AR.user_id ORDER BY AR.ResultID) AS AttemptRank
                                            FROM 
                                                AssessmentResult AR
                                            WHERE
                                                AR.user_id = @UserID
                                        )
                                        SELECT 
                                            CA.CourseAssessmentID as 'AssessmentID',
                                            ISNULL(MC.ModuleContentTitle, 'Final Assessment') as 'AssessmentTitle',
                                            CA.AssessmentType,
                                            ISNULL(CM.CourseModuleNo, 0) as 'ModuleNo',
                                            FST1.Score as 'FirstTryScore',
                                            FST2.Score as 'SecondTryScore'
                                        FROM 
                                            CourseAssessment CA
                                        LEFT JOIN CourseModule CM ON CA.CourseModuleID = CM.CourseModuleID
                                        LEFT JOIN ModuleContent MC ON CM.AssociationCriteria = MC.AssociationCriteria AND CM.CourseCode = MC.CourseCode
                                        LEFT JOIN FirstAndSecondTries FST1 ON CA.CourseAssessmentID = FST1.CourseAssessmentID AND FST1.AttemptRank = 1
                                        LEFT JOIN FirstAndSecondTries FST2 ON CA.CourseAssessmentID = FST2.CourseAssessmentID AND FST2.AttemptRank = 2
                                        WHERE
                                            CA.CourseCode = @CourseCode
                                        ORDER BY 
                                            CASE WHEN CM.CourseModuleNo IS NULL THEN 1 ELSE 0 END, 
                                            CM.CourseModuleNo;

                                    ";

                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.Parameters.AddWithValue("@CourseCode", courseCode);
                                cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(Session["user_id"]));
                                con.Open();
                                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                                {
                                    DataTable dt = new DataTable();
                                    sda.Fill(dt);
                                    con.Close();
                                    if (dt.Rows.Count > 0)
                                    {
                                        gvAssessmentList.DataSource = dt;
                                        gvAssessmentList.DataBind();
                                        gvAssessmentList.Visible = true;
                                    }
                                    else
                                    {
                                        gvAssessmentList.Visible = false;
                                        // Handle the case when no data is returned
                                        // Maybe display a message indicating no assessments are available
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Handle the case when course code is not found
                            // Maybe display an error message or redirect the user
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception and/or inform the user
                    // Maybe display an error message about the exception
                    // Example: lblError.Text = "An error occurred: " + ex.Message;
                    // lblError.Visible = true;
                }
            }
            else
            {
                // Handle the case when course ID is not valid or not provided
                // Maybe redirect the user to another page or display an error message
            }
        }

        protected string GetCourseStatus(object firstTryScore, object secondTryScore)
        {
            double firstScore = firstTryScore != DBNull.Value ? Convert.ToDouble(firstTryScore) : 0;
            double secondScore = secondTryScore != DBNull.Value ? Convert.ToDouble(secondTryScore) : 0;

            if (firstScore >= 70 || secondScore >= 70)
            {
                return "<span style='color:green;'>Passed</span>";
            }
            else
            {
                return "<span style='color:red;'>Failed</span>";
            }
        }



        protected void gvAssessmentList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewAssessment")
            {
                // Retrieve the AssessmentID from the CommandArgument
                int assessmentId = Convert.ToInt32(e.CommandArgument);


                // Redirect to assessment.aspx with the AssessmentID as a query string parameter
                Response.Redirect($"assessment.aspx?AssessmentID={assessmentId}");
            }
        }






    }
}
