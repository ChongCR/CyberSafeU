using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace WebApplication1
{
    public partial class EditCourse : System.Web.UI.Page
    {
        private const string ClientId = "1ab0d690241254e";

        public class ImgurApiResponse
        {
            public ImgurData data { get; set; }
            public bool success { get; set; }
            public int status { get; set; }
        }

        public class ImgurData
        {
            public string link { get; set; }
        }
        protected void btnBack_Click2(object sender, EventArgs e)
        {
            // Perform the redirect to coursemanagement.aspx
            Response.Redirect("coursemanagement.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if the user has admin access during page load
            if (Session["AdminAccess"] == null && Session["SAdminAccess"] == null)
            {
                Response.Redirect("userlogin.aspx");
            }

            if (!IsPostBack)
            {
                PopulateInstructorDropDownView5(editInstructorDropDownList);

                string courseCode = Request.QueryString["CourseCode"];
                if (!string.IsNullOrEmpty(courseCode))
                {
                    ViewState["CourseCode"] = courseCode;
                    BindCourseDetails(courseCode);
                }

                if (multiView.GetActiveView() == view4)
                {
                    string moduleContentVideo = ViewState["ModuleContentVideo"]?.ToString();
                    if (!string.IsNullOrEmpty(moduleContentVideo))
                    {
                        divModuleContentVideo.InnerHtml = GetYouTubeEmbeddedPlayer(moduleContentVideo);
                        divModuleContentVideo.Visible = true;
                    }
                }

                if (ViewState["courseTitle"] != null)
                    courseTitle.Text = ViewState["courseTitle"].ToString();

                if (ViewState["selectedLanguage"] != null)
                    languageDropdown.SelectedValue = ViewState["selectedLanguage"].ToString();

                if (ViewState["selectedInstructor"] != null)
                    instructorDropdown.SelectedValue = ViewState["selectedInstructor"].ToString();

                if (ViewState["selectedCategory"] != null)
                    category.SelectedValue = ViewState["selectedCategory"].ToString();

                if (ViewState["selectedLevel"] != null)
                    level.SelectedValue = ViewState["selectedLevel"].ToString();

                if (ViewState["imagePath"] != null)
                {
                    string imagePath = ViewState["imagePath"].ToString();
                    imagePreview.Src = ResolveUrl(imagePath);
                    imagePreview.Style.Add("display", "block");
                }
                PopulateCourseEditFields(courseCode);
                multiView.SetActiveView(view5);


            }



        }


        private void PopulateCourseEditFields(string courseCode)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
        SELECT 
            CourseCode,
            CourseName,
            CourseCategory,
            CourseLevel,
            CourseLanguage,
            ImageURL,
            InstructorID,
            RequiredHours
        FROM 
            Course
        WHERE 
            CourseCode = @CourseCode";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CourseCode", courseCode);

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Assuming you have text boxes and other controls to display the data
                            editCourseID.Text = courseCode;
                            editCourseTitle.Text = reader["CourseName"].ToString();
                            editLanguageDropDownList.SelectedValue = reader["CourseLanguage"].ToString();
                            editCategoryDropdownList.SelectedValue = reader["CourseCategory"].ToString();
                            editLevelDropdownList.SelectedValue = reader["CourseLevel"].ToString();
                            editHoursRequired.Text = reader["RequiredHours"].ToString();

                            // Populate the image if exists
                            string imageUrl = reader["ImageURL"].ToString();
                            if (!string.IsNullOrEmpty(imageUrl))
                            {
                             
                                Img2.Src = imageUrl; // For the edit image.
                            }

                            // Populate the instructor if exists
                            if (reader["InstructorID"] != DBNull.Value)
                            {
                                editInstructorDropDownList.SelectedValue = reader["InstructorID"].ToString();
                                // Ensure that the instructor dropdown list is bound before this step.
                            }
                            // Otherwise, you will need to fetch the instructor's name based on the ID
                            // and possibly bind the dropdown list here.
                        }
                        else
                        {
                            Label1.Text = "Course details not found.";
                            // Handle the case when no data is found.
                        }
                    }
                    con.Close();
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

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Use "CourseName" instead of "CourseTitle"
                            courseTitle.Text = reader["CourseName"].ToString();
                            languageDropdown.SelectedValue = reader["CourseLanguage"].ToString();
                            if (instructorDropdown.Items.FindByValue(reader["InstructorID"].ToString()) != null)
                            {
                                instructorDropdown.SelectedValue = reader["InstructorID"].ToString();
                            }
                            category.SelectedValue = reader["CourseCategory"].ToString();
                            level.SelectedValue = reader["CourseLevel"].ToString();
                            hoursRequired.Text = reader["RequiredHours"].ToString();

                            // Use "ImageURL" instead of "ImagePath"
                            if (reader["ImageURL"] != DBNull.Value)
                            {
                                string imageUrl = reader["ImageURL"].ToString();
                                if (Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
                                {
                                    // Assuming imagePreview is an HTML img tag with runat="server"
                                    imagePreview.Src = imageUrl;
                                }
                            }
                        }
                        else
                        {
                            errorLabel.Text = "Course details not found.";
                        }
                    }
                    con.Close();
                }
            }
        }

        protected void btnAddAssessment_Click(object sender, EventArgs e)
        {
            string courseCode = ViewState["CourseCode"].ToString();
            InsertCourseAssessment(courseCode);
            BindModuleList();
            BindAssessmentGridView();
            PopulateModules(ViewState["CourseCode"].ToString());
        }
        private void InsertCourseAssessment(string courseCode)
        {

            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            string query = "INSERT INTO CourseAssessment (CourseCode) VALUES (@CourseCode)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CourseCode", courseCode);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
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
                        command.Parameters.AddWithValue("@CourseCode", ViewState["CourseCode"]);
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

        protected void btnSubmitQuestions_Click(object sender, GridViewRowEventArgs e)
        {

        }

        private string UploadToImgur(string imagePath)
        {
            try
            {
                string bearerToken = "d8a5d069d975f2084bbf21fb2222c829bb119592";

                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("Authorization", $"Bearer {bearerToken}");

                    // Read the image file into a byte array
                    byte[] imageData = File.ReadAllBytes(imagePath);

                    // Upload the image data to Imgur
                    byte[] response = client.UploadData("https://api.imgur.com/3/image", "POST", imageData);

                    // Parse the Imgur API response
                    ImgurApiResponse imgurResponse = JsonConvert.DeserializeObject<ImgurApiResponse>(Encoding.UTF8.GetString(response));

                    if (imgurResponse.success)
                    {
                        // Save the Imgur URL in ViewState
                        ViewState["imgurUrl"] = imgurResponse.data.link;

                        // Return the Imgur URL
                        return imgurResponse.data.link;
                    }
                    else
                    {
                        // Handle Imgur API error
                        return $"Error: Imgur upload failed. {imgurResponse.status}";
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                return $"Error: {ex.Message}";
            }
        }

        // VIEW STATE 2
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {

            GridView1.EditIndex = -1;
            int rowIndex = e.NewEditIndex;
            if (rowIndex >= 0 && rowIndex < GridView1.Rows.Count)
            {
                string associationCriteria = GridView1.DataKeys[rowIndex]["AssociationCriteria"].ToString();
                ViewState["AssociationCriteria"] = associationCriteria;
                multiView.SetActiveView(view3);
                PopulateModuleContentData(associationCriteria);
            }
        }

        private void PopulateModuleContentData(string associationCriteria)
        {
            string query = @"
                            SELECT 
                                ModuleContentTitle, 
                                ModuleContentType, 
                                ModuleContentImage, 
                                ModuleContentVideo, 
                                ModuleContentText
                            FROM 
                                ModuleContent 
                            WHERE 
                                AssociationCriteria = @AssociationCriteria";

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AssociationCriteria", associationCriteria);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            moduleContentTitleTextBox.Text = reader["ModuleContentTitle"].ToString();
                            moduleContentTypeTextBox.Text = reader["ModuleContentType"].ToString();
                            fileUploaderVideo.Text = reader["ModuleContentVideo"].ToString();


                            string moduleContentText = reader["ModuleContentText"].ToString();
                            if (!string.IsNullOrEmpty(moduleContentText))
                            {
                                textAreaInput.Text = HttpUtility.HtmlDecode(moduleContentText);

                            }


                            string imageUrl = reader["ModuleContentImage"].ToString();
                            if (!string.IsNullOrEmpty(imageUrl))
                            {
                                Img3.ImageUrl = imageUrl;
                                Img3.Visible = true;
                                Img3.Attributes.CssStyle.Add("display", "block");
                            }
                            else
                            {
                                Img3.Visible = false;
                            }

                            string videoUrl = reader["ModuleContentVideo"].ToString();
                            if (!string.IsNullOrEmpty(videoUrl))
                            {
                                string embedUrl = ConvertToEmbedUrl(videoUrl);
                                youtubeVideo.Attributes["src"] = embedUrl;
                                youtubeVideo.Visible = true;
                            }
                            else
                            {
                                youtubeVideo.Visible = false;
                            }
                        }
                    }
                }
            }
        }


        private string ConvertToEmbedUrl(string videoUrl)
        {
            var uri = new Uri(videoUrl);
            var query = HttpUtility.ParseQueryString(uri.Query);
            var videoId = string.Empty;

            if (query.AllKeys.Contains("v"))
            {
                videoId = query["v"];
                return "https://www.youtube.com/embed/" + videoId;
            }
            else if (uri.Segments.Contains("embed/"))
            {
                videoId = uri.Segments.Last();
                return videoUrl;
            }
            return videoUrl;
        }


        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "View")
            {
                // Get the row index from the CommandArgument
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                // Check if a row is selected
                if (rowIndex >= 0 && rowIndex < GridView1.Rows.Count)
                {
                    // Retrieve AssociationCriteria using DataKeys
                    string associationCriteria = GridView1.DataKeys[rowIndex]["AssociationCriteria"].ToString();

                    // Store the AssociationCriteria in ViewState
                    ViewState["AssociationCriteria"] = associationCriteria;

                    // Switch to view4
                    multiView.SetActiveView(view4);

                    // Retrieve and populate data for ModuleContent
                    PopulateModuleContentDataDetails(associationCriteria);
                }
            }

        }

        private void PopulateModuleContentDataDetails(string associationCriteria)
        {
            try
            {

                string query = @"
            SELECT 
                ModuleContentTitle, 
                ModuleContentType, 
                ModuleContentImage, 
                ModuleContentVideo, 
                ModuleContentText,
                CourseModuleNo,
                CourseCode,
                AssociationCriteria
            FROM 
                ModuleContent 
            WHERE 
                AssociationCriteria = @AssociationCriteria";

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AssociationCriteria", associationCriteria);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string moduleContentImage = reader["ModuleContentImage"].ToString();
                                if (!string.IsNullOrEmpty(moduleContentImage))
                                {
                                    imgModuleContentImage.ImageUrl = moduleContentImage;
                                    rowModuleContentImage.Visible = true;
                                }
                                else
                                {
                                    rowModuleContentImage.Visible = false;
                                }


                                string moduleContentVideo = reader["ModuleContentVideo"].ToString();
                                if (!string.IsNullOrEmpty(moduleContentVideo))
                                {

                                    divModuleContentVideo.InnerHtml = GetYouTubeEmbeddedPlayer(moduleContentVideo);
                                    divModuleContentVideo.Visible = true;


                                    ViewState["ModuleContentVideo"] = moduleContentVideo;
                                }
                                else
                                {
                                    divModuleContentVideo.Visible = false;
                                }


                                string moduleContentText = reader["ModuleContentText"].ToString();
                                if (!string.IsNullOrEmpty(moduleContentText))
                                {


                                    divModuleContentText.InnerHtml = HttpUtility.HtmlDecode(reader["ModuleContentText"].ToString());


                                    rowModuleContentText.Visible = true;
                                }
                                else
                                {
                                    rowModuleContentText.Visible = false;
                                }



                                lblCourseModuleNo.Text = reader["CourseModuleNo"].ToString();
                                lblCourseCode.Text = reader["CourseCode"].ToString();
                                lblModuleContentTitle.Text = reader["ModuleContentTitle"].ToString();
                                lblModuleContentType.Text = reader["ModuleContentType"].ToString();
                                imgModuleContentImage.ImageUrl = reader["ModuleContentImage"].ToString();

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error retrieving module content details: " + ex.Message);
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
                    command.Parameters.AddWithValue("@CourseCode", Request.QueryString["CourseCode"]);
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


        private void PopulateModules(string courseCode)
        {
            DataTable dt = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT cm.CourseModuleID, mc.ModuleContentTitle
            FROM CourseModule cm
            JOIN ModuleContent mc ON cm.AssociationCriteria = mc.AssociationCriteria
            WHERE cm.CourseCode = @CourseCode";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CourseCode", courseCode);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            moduleDropdown.DataSource = dt;
            moduleDropdown.DataTextField = "ModuleContentTitle";
            moduleDropdown.DataValueField = "CourseModuleID";
            moduleDropdown.DataBind();

            moduleDropdown.Items.Insert(0, new ListItem("-- Select Module --", "0"));
        }



        protected void btnAddModule_Click(object sender, EventArgs e)
        {
            string moduleNumberStr = txtModuleNumber.Value;
            int moduleNumber;

            if (int.TryParse(moduleNumberStr, out moduleNumber) && moduleNumber > 0)
            {
                string insertQuery = @"
            BEGIN TRANSACTION;
            DECLARE @CourseModuleID INT;
            DECLARE @AssociationCriteria NVARCHAR(255);

            -- Generate AssociationCriteria
            SET @AssociationCriteria = CONVERT(NVARCHAR(255), NEWID());

            -- Insert into CourseModule
            INSERT INTO CourseModule (CourseModuleNo, CourseCode, AssociationCriteria) 
            VALUES (@ModuleNumber, @CourseCode, @AssociationCriteria);
            SET @CourseModuleID = SCOPE_IDENTITY();

            -- Insert into ModuleContent
            INSERT INTO ModuleContent (ModuleContentTitle, CourseCode, CourseModuleNo, AssociationCriteria) 
            VALUES (@ModuleContentTitle, @CourseCode, @ModuleNumber, @AssociationCriteria);

            -- Insert into ModuleTopic
            INSERT INTO ModuleTopic (CourseModuleID, ModuleContentID)
            VALUES (@CourseModuleID, SCOPE_IDENTITY());

            COMMIT;";

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        connection.Open();

                        command.Parameters.AddWithValue("@ModuleNumber", moduleNumber);
                        command.Parameters.AddWithValue("@CourseCode", ViewState["CourseCode"]);
                        command.Parameters.AddWithValue("@ModuleContentTitle", txtModuleContentName.Value);

                        command.ExecuteNonQuery();
                    }
                }
                multiView.SetActiveView(view2);
                BindModuleList();
                BindAssessmentGridView();
                PopulateModules(ViewState["CourseCode"].ToString());
                txtModuleNumber.Value = "";
                txtModuleContentName.Value = "";

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "InvalidModuleNumber", "alert('Please enter a valid positive integer for Module Number.');", true);
            }
        }



        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (fileUploaderImage.HasFile)
            {

                string fileName = Path.GetFileName(fileUploaderImage.FileName);
                string filePath = Server.MapPath("~/UploadedImages/") + fileName;
                fileUploaderImage.SaveAs(filePath);
                Img3.ImageUrl = "~/UploadedImages/" + fileName;
                Img3.Visible = true;
            }
        }


        protected void fileUploaderImage_Uploaded(object sender, EventArgs e)
        {
            if (fileUploaderImage.HasFile)
            {
                string fileName = Path.GetFileName(fileUploaderImage.PostedFile.FileName);
                string folderPath = Server.MapPath("~/UploadedImages/");
                string filePath = folderPath + fileName;
                fileUploaderImage.SaveAs(filePath);
                ViewState["UploadedImagePath"] = ResolveUrl("~/UploadedImages/") + fileName;
                Img3.ImageUrl = ViewState["UploadedImagePath"].ToString();
                Img3.Visible = true;
            }
        }

        protected void fileUploaderVideo_TextChanged(object sender, EventArgs e)
        {
            string videoUrl = fileUploaderVideo.Text;
            string videoId = ExtractYouTubeVideoId(videoUrl);

            if (!string.IsNullOrEmpty(videoId))
            {
                youtubeVideo.Attributes["src"] = "https://www.youtube.com/embed/" + videoId;
                string script = $@"
            var loadingBar = document.getElementById('{loadingBar.ClientID}');
            loadingBar.style.width = '100%';
            setTimeout(function() {{
                loadingBar.style.width = '0%';
                var youtubeVideo = document.getElementById('{youtubeVideo.ClientID}');
                youtubeVideo.style.display = 'block';
            }}, 2500);";

                ScriptManager.RegisterStartupScript(this, GetType(), "ShowVideo", script, true);
            }
            else
            {
                youtubeVideo.Attributes["src"] = "";
                youtubeVideo.Style["display"] = "none";
            }
        }


        private string ExtractYouTubeVideoId(string url)
        {
            var videoId = string.Empty;
            var uri = new Uri(url);
            if (uri.Host.Contains("youtube.com"))
            {
                var query = HttpUtility.ParseQueryString(uri.Query);
                videoId = query["v"];
            }
            else if (uri.Host.Contains("youtu.be"))
            {
                videoId = uri.Segments.Last();
            }

            return videoId;
        }

        protected void view3backBtn_Click(object sender, EventArgs e)
        {
            multiView.SetActiveView(view2);
            GridView1.EditIndex = -1;
            BindModuleList();
            BindAssessmentGridView();
            PopulateModules(ViewState["CourseCode"].ToString());
        }

        protected void btnSubmitContent_Click(object sender, EventArgs e)
        {
            string associationCriteria = ViewState["AssociationCriteria"]?.ToString();

            if (!string.IsNullOrEmpty(associationCriteria))
            {

                string moduleContentTitle = moduleContentTitleTextBox.Text;
                string moduleContentType = moduleContentTypeTextBox.Text;
                string moduleContentVideo = fileUploaderVideo.Text;
                string moduleContentText = HttpUtility.HtmlEncode(textAreaInput.Text);

                string imgurUrl = string.Empty;


                if (fileUploaderImage.HasFile)
                {

                    string moduleContentImage = GetImageFilePath();
                    imgurUrl = UploadToImgur(moduleContentImage);
                }
                else
                {

                    imgurUrl = GetExistingImageUrl(associationCriteria);
                }


                UpdateModuleContent(associationCriteria, moduleContentTitle, moduleContentType, imgurUrl, moduleContentVideo, moduleContentText);

                textAreaInput.Text = "";
                ViewState["UploadedImgurUrl"] = null;


            }
            else
            {

            }

            multiView.SetActiveView(view2);
            GridView1.EditIndex = -1;
            BindModuleList();
            BindAssessmentGridView();
            PopulateModules(ViewState["CourseCode"].ToString());
        }

        private string GetExistingImageUrl(string associationCriteria)
        {
            string existingImageUrl = string.Empty;
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            string query = @"
        SELECT ModuleContentImage 
        FROM ModuleContent 
        WHERE AssociationCriteria = @AssociationCriteria";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AssociationCriteria", associationCriteria);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                existingImageUrl = reader["ModuleContentImage"] as string;
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            return existingImageUrl ?? string.Empty;
        }

        private void UpdateModuleContent(string associationCriteria, string moduleContentTitle, string moduleContentType, string moduleContentImage, string moduleContentVideo, string moduleContentText)
        {



            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();


                string updateCommand = "UPDATE ModuleContent " +
                                       "SET ModuleContentTitle = @ModuleContentTitle, " +
                                       "    ModuleContentType = @ModuleContentType, " +
                                       "    ModuleContentImage = @ModuleContentImage, " +
                                       "    ModuleContentVideo = @ModuleContentVideo, " +
                                       "    ModuleContentText = @ModuleContentText " +
                                       "WHERE AssociationCriteria = @AssociationCriteria";

                using (SqlCommand command = new SqlCommand(updateCommand, connection))
                {

                    command.Parameters.AddWithValue("@ModuleContentTitle", moduleContentTitle);
                    command.Parameters.AddWithValue("@ModuleContentType", moduleContentType);
                    command.Parameters.AddWithValue("@ModuleContentImage", moduleContentImage);
                    command.Parameters.AddWithValue("@ModuleContentVideo", moduleContentVideo);
                    command.Parameters.AddWithValue("@ModuleContentText", moduleContentText);
                    command.Parameters.AddWithValue("@AssociationCriteria", associationCriteria);


                    command.ExecuteNonQuery();
                }
            }
        }



        private string GetImageFilePath()
        {
            if (fileUploaderImage.HasFile)
            {
                string fileName = Path.GetFileName(fileUploaderImage.FileName);
                string imagePath = Path.Combine(Server.MapPath("~/images/"), fileName);


                fileUploaderImage.SaveAs(imagePath);

                return imagePath;
            }

            return string.Empty;
        }




        protected void btnUpdate_Click2(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            // Get the CourseCode from the query string
            string courseCode = Request.QueryString["CourseCode"];

            // Check if the CourseCode is valid (not null or empty)
            if (!string.IsNullOrEmpty(courseCode))
            {
                // Assuming you have a SqlConnection object named "sqlConnection" defined elsewhere
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Retrieve the current image URL from the database
                    string currentImageURL = GetCourseImageURL(connection, courseCode);

                    // Check if a new image was uploaded
                    string newImageURL = currentImageURL; // Default to the current URL

                    if (FileUpload1.HasFile)
                    {
                        string fileName = Path.GetFileName(FileUpload1.FileName);
                        string imagePath = Path.Combine(Server.MapPath("~/images/"), fileName);


                        FileUpload1.SaveAs(imagePath);


                        newImageURL = UploadToImgur(imagePath);
                    }

                    else
                    {
                        newImageURL = GetCourseImageURL(connection, courseCode);
                    }

                    // Continue with the other updates to course information (excluding the image)
                    string updateQuery = @"UPDATE Course
                               SET CourseName = @CourseName,
                                   CourseCategory = @CourseCategory,
                                   CourseLevel = @CourseLevel,
                                   CourseLanguage = @CourseLanguage,
                                   ImageURL = @ImageURL,
                                   RequiredHours = @RequiredHours
                               WHERE CourseCode = @CourseCode";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@CourseName", editCourseTitle.Text);
                        command.Parameters.AddWithValue("@CourseCategory", editCategoryDropdownList.SelectedValue);
                        command.Parameters.AddWithValue("@CourseLevel", editLevelDropdownList.SelectedValue);
                        command.Parameters.AddWithValue("@CourseLanguage", editLanguageDropDownList.SelectedValue);
                        command.Parameters.AddWithValue("@ImageURL", newImageURL); // Use the new image URL
                        command.Parameters.AddWithValue("@RequiredHours", int.Parse(editHoursRequired.Text));
                        command.Parameters.AddWithValue("@CourseCode", courseCode);


                        int rowsAffected = command.ExecuteNonQuery();

                        // Check if the update was successful
                        if (rowsAffected > 0)
                        {
                            multiView.SetActiveView(view2);
                            BindModuleList();
                            BindAssessmentGridView();
                            PopulateModules(ViewState["CourseCode"].ToString());

                        }
                        else
                        {
                            // Update failed
                            // Display an error message
                        }
                    }
                }
            }
            else
            {
                // Handle the case when CourseCode is not valid or missing in the query string
            }
        }


        // Function to retrieve the current image URL from the database
        private string GetCourseImageURL(SqlConnection connection, string courseCode)
        {
            string imageURL = null;

            string query = "SELECT ImageURL FROM Course WHERE CourseCode = @CourseCode";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CourseCode", courseCode);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        imageURL = reader["ImageURL"] as string;
                    }
                }
            }

            return imageURL;
        }



        private string GetLastCourseCode(string prefix)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            string lastCode = null;


            string query = "SELECT TOP 1 CourseCode FROM Course WHERE CourseCode LIKE @Prefix ORDER BY CourseCode DESC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@Prefix", prefix + "%");

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lastCode = reader["CourseCode"].ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine("Error in GetLastCourseCode: " + ex.Message);

                    }
                }
            }

            return lastCode;
        }

        private string GenerateCourseCode(string prefix)
        {
            string lastCode = GetLastCourseCode(prefix);
            int nextNumber;

            if (!string.IsNullOrEmpty(lastCode))
            {
                string numericPart = new String(lastCode.Substring(prefix.Length).Where(Char.IsDigit).ToArray());
                if (int.TryParse(numericPart, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
                else
                {

                    throw new FormatException("The last course code's numeric part is not in the correct format.");
                }
            }
            else
            {
                nextNumber = 1;
            }

            return $"{prefix}{nextNumber:000}";
        }



        private string GetPrefixForCategory(string category)
        {
            switch (category)
            {
                case "Guided Project":
                    return "G";
                case "Course":
                    return "C";
                case "Certification":
                    return "CTF";
                default:
                    return "";
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {

            int number;
            bool isValidNumber = int.TryParse(hoursRequired.Text, out number) && number >= 1 && number <= 50;

            string prefix = GetPrefixForCategory(category.SelectedValue);
            ViewState["CourseCode"] = GenerateCourseCode(prefix);
            ViewState["courseTitle"] = courseTitle.Text;
            ViewState["languageDropdown"] = languageDropdown.SelectedValue;
            ViewState["instructorDropdown"] = instructorDropdown.SelectedValue;
            ViewState["category"] = category.SelectedValue;
            ViewState["level"] = level.SelectedValue;
            ViewState["hoursRequired"] = hoursRequired.Text;
            imagePreview.Src = ViewState["imgurUrl"]?.ToString();

            string existingCourseCode = ViewState["CourseCode"].ToString();

            if (!string.IsNullOrEmpty(imagePreview.Src))
            {
                ViewState["imagePath"] = imagePreview.Src;
            }


            if (string.IsNullOrWhiteSpace(courseTitle.Text) ||
                string.IsNullOrWhiteSpace(languageDropdown.SelectedValue) ||
                string.IsNullOrWhiteSpace(instructorDropdown.SelectedValue) ||
                string.IsNullOrWhiteSpace(category.SelectedValue) ||
                string.IsNullOrWhiteSpace(level.SelectedValue) ||
                string.IsNullOrWhiteSpace(hoursRequired.Text) ||
                !isValidNumber)
            {

                errorLabel.Text = "<i class=\"fas fa-exclamation-triangle\"></i> Please fill in all the fields.";
                LabelError.Text = "Please enter a number between 1 and 50.";

            }
            else
            {

                if (!string.IsNullOrEmpty(existingCourseCode))
                {
                    CheckAndStoreCourseCode(existingCourseCode);
                }
                else
                {

                }
            }
        }



        private void CheckAndStoreCourseCode(string courseCode)
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;


                string query = "SELECT c.CourseCode, c.CourseName, c.CreationDate, c.Status, c.InstructorID, i.InstructorName " +
                               "FROM Course c " +
                               "JOIN CourseInstructor i ON c.InstructorID = i.InstructorID " +
                               "WHERE c.CourseCode = @CourseCode";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@CourseCode", courseCode);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                string retrievedCourseCode = reader["CourseCode"]?.ToString();
                                string retrievedCourseName = reader["CourseName"]?.ToString();
                                string retrievedCreationDate = reader["CreationDate"]?.ToString();
                                string retrievedStatus = reader["Status"]?.ToString();
                                string retrievedInstructorID = reader["InstructorID"]?.ToString();
                                string retrievedInstructorName = reader["InstructorName"]?.ToString();


                                ViewState["RetrievedCourseCode"] = retrievedCourseCode;
                                ViewState["RetrievedCourseName"] = retrievedCourseName;
                                ViewState["RetrievedCreationDate"] = retrievedCreationDate;
                                ViewState["RetrievedStatus"] = retrievedStatus;
                                ViewState["RetrievedInstructorID"] = retrievedInstructorID;
                                ViewState["RetrievedInstructorName"] = retrievedInstructorName;


                                ScriptManager.RegisterStartupScript(this, GetType(), "openCourseDetailsModal",
                                $"OpenCourseDetailsModal('{retrievedCourseCode}', '{retrievedCourseName}', '{retrievedCreationDate}', '{retrievedStatus}', '{retrievedInstructorID}', '{retrievedInstructorName}');", true);
                            }
                            else
                            {

                                InsertCourseData();
                                multiView.SetActiveView(view2);
                                BindModuleList();
                                BindAssessmentGridView();
                                PopulateModules(ViewState["CourseCode"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving course information: " + ex.Message);

            }
        }


     

        protected void SubmitModuleAssessment_Click(object sender, EventArgs e)
        {
            string courseCode = ViewState["CourseCode"] as string;
            string assessmentType = "Module";
            int courseModuleId;


            if (int.TryParse(hiddenFieldForCourseModuleId.Value, out courseModuleId))
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    string checkQuery = @"
                SELECT COUNT(1)
                FROM CourseModule
                WHERE CourseModuleID = @CourseModuleID";

                    conn.Open();
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@CourseModuleID", courseModuleId);
                        int moduleExists = (int)checkCmd.ExecuteScalar();

                        if (moduleExists == 0)
                        {
                            conn.Close();
                            return;
                        }
                    }


                    string insertQuery = @"
                INSERT INTO CourseAssessment (CourseCode, AssessmentType, CourseModuleID)
                VALUES (@CourseCode, @AssessmentType, @CourseModuleID)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CourseCode", courseCode);
                        cmd.Parameters.AddWithValue("@AssessmentType", assessmentType);
                        cmd.Parameters.AddWithValue("@CourseModuleID", courseModuleId);

                        cmd.ExecuteNonQuery();

                        BindAssessmentGridView();
                    }
                    conn.Close();
                }
            }
            else
            {

            }
        }

        protected void AddFinalAssessment_Click(object sender, EventArgs e)
        {
            string courseCode = ViewState["CourseCode"] as string;
            string assessmentType = "Final";


            if (FinalAssessmentExists(courseCode))
            {
                AssessmentError.Text = "A final assessment already exists for this course.";
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string insertQuery = @"
            INSERT INTO CourseAssessment (CourseCode, AssessmentType)
            VALUES (@CourseCode, @AssessmentType)";

                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@CourseCode", courseCode);
                    cmd.Parameters.AddWithValue("@AssessmentType", assessmentType);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    AssessmentError.Text = "";

                    BindAssessmentGridView();
                }
            }
        }

        private bool FinalAssessmentExists(string courseCode)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            string query = "SELECT COUNT(*) FROM CourseAssessment WHERE CourseCode = @CourseCode AND AssessmentType = 'Final'";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CourseCode", courseCode);

                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }


        protected string GetImageUrlFromDatabase(string courseCode)
        {
            string imageUrl = string.Empty;

            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;


                string query = "SELECT ImageURL FROM Course WHERE CourseCode = @CourseCode";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@CourseCode", courseCode);


                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            imageUrl = result.ToString();
                        }

                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error retrieving ImageURL from database: " + ex.Message);
            }

            return imageUrl;
        }


        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            string courseCode = ViewState["CourseCode"].ToString();
            string imageUrl = GetImageUrlFromDatabase(courseCode);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SetImageUrl", "setTimeout(function() { SetImageUrl('" + imageUrl + "'); }, 100);", true);

            multiView.SetActiveView(view5);
            BindModuleList();
            BindAssessmentGridView();
            PopulateModules(ViewState["CourseCode"].ToString());

            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;


                string query = "SELECT c.*, i.InstructorID, i.InstructorName " +
                               "FROM Course c " +
                               "JOIN CourseInstructor i ON c.InstructorID = i.InstructorID " +
                               "WHERE c.CourseCode = @CourseCode";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@CourseCode", courseCode);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                editCourseID.Text = reader["CourseCode"].ToString();
                                editCourseTitle.Text = reader["CourseName"].ToString();

                                PopulateInstructorDropDownView5(editInstructorDropDownList);
                                PopulateInstructorDropDownView5(editInstructorDropDownList);

                                foreach (ListItem item in editInstructorDropDownList.Items)
                                {
                                    if (item.Text == reader["InstructorName"].ToString())
                                    {
                                        item.Selected = true;
                                        break;
                                    }
                                }

                                lblEditingCourseCode.Text = reader["CourseCode"].ToString();
                                editCategoryDropdownList.Text = reader["CourseCategory"].ToString();
                                editHoursRequired.Text = reader["RequiredHours"].ToString();
                                editLevelDropdownList.Text = reader["CourseLevel"].ToString();
                                editLanguageDropDownList.Text = reader["CourseLanguage"].ToString();

                                reader.Close();
                            }
                            else
                            {
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error retrieving course details: " + ex.Message);
            }
        }
        private bool IsValidNumber(int number)
        {
            return number >= 1 && number <= 7;
        }
        private void InsertCourseData()
        {
            string imgurUrl = string.Empty;

            if (imageUploader.HasFile)
            {
                string fileName = Path.GetFileName(imageUploader.FileName);
                string imagePath = Path.Combine(Server.MapPath("~/images/"), fileName);


                imageUploader.SaveAs(imagePath);


                imgurUrl = UploadToImgur(imagePath);
            }

            string category = ViewState["category"].ToString();
            string prefix = GetPrefixForCategory(category);
            string courseCode = GenerateCourseCode(prefix);
            ViewState["CourseCode"] = GenerateCourseCode(prefix);
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            string insertQuery = "INSERT INTO Course (CourseCode, CourseName, CourseCategory, CourseLevel, CourseLanguage, ImageURL, CreationDate, Status, InstructorID, RequiredHours) " +
                                 "VALUES (@CourseCode, @CourseName, @CourseCategory, @CourseLevel, @CourseLanguage, @ImageURL, @CreationDate, @Status, @InstructorID, @RequiredHours)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@CourseCode", courseCode);
                    command.Parameters.AddWithValue("@CourseName", ViewState["courseTitle"].ToString());
                    command.Parameters.AddWithValue("@CourseCategory", category);
                    command.Parameters.AddWithValue("@CourseLevel", ViewState["level"].ToString());
                    command.Parameters.AddWithValue("@CourseLanguage", ViewState["languageDropdown"].ToString());
                    command.Parameters.AddWithValue("@ImageURL", string.IsNullOrEmpty(imgurUrl) ? string.Empty : imgurUrl);
                    command.Parameters.AddWithValue("@CreationDate", DateTime.Now);
                    command.Parameters.AddWithValue("@Status", "Uncompleted Course");
                    command.Parameters.AddWithValue("@InstructorID", Convert.ToInt32(ViewState["instructorDropdown"]));
                    command.Parameters.AddWithValue("@RequiredHours", Convert.ToInt32(ViewState["hoursRequired"]));

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string courseCode = Request.QueryString["CourseCode"];
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                string query = "UPDATE Course SET Status = @Status WHERE CourseCode = @CourseCode";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Status", "Pending");
                    cmd.Parameters.AddWithValue("@CourseCode", courseCode);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }


            Response.Redirect("coursemanagement.aspx?status=success");


        }

        private void ShowSweetAlert(string title, string message, string type)
        {
            string script = $"Swal.fire({{ title: '{title}', text: '{message}', icon: '{type}', confirmButtonText: 'Ok' }});";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "sweetAlert", script, true);
        }


        protected void btnBackToGridView_Click(object sender, EventArgs e)
        {
            multiView.SetActiveView(view2);
            GridView1.EditIndex = -1;
            BindModuleList();
            BindAssessmentGridView();
            PopulateModules(ViewState["CourseCode"].ToString());

        }

        protected void GridViewCourseModule_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                int rowNumber = e.Row.RowIndex + 1;
                e.Row.Cells[0].Text = rowNumber.ToString();
            }
        }

        protected void GridViewAssessmentList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                int rowNumber = e.Row.RowIndex + 1;
                e.Row.Cells[0].Text = rowNumber.ToString();
            }
        }

        protected void btnNext2_Click(object sender, EventArgs e)
        {
        }


        protected void btnNextStep2_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;


            multiView.SetActiveView(view9);


            string courseCode = Request.QueryString["CourseCode"];

            int totalModules = 0;
            int totalAssessments = 0;


            string courseQuery = "SELECT * FROM Course WHERE CourseCode = @CourseCode";
            string courseModuleQuery = @"SELECT 
                ROW_NUMBER() OVER(ORDER BY cm.CourseModuleID) AS 'No',
                cm.CourseModuleID, 
                cm.CourseModuleNo, 
                cm.CourseCode, 
                mc.ModuleContentTitle AS ModuleName
            FROM CourseModule cm
            LEFT JOIN ModuleContent mc ON cm.AssociationCriteria = mc.AssociationCriteria
            WHERE cm.CourseCode = @CourseCode
            GROUP BY cm.CourseModuleID, cm.CourseModuleNo, cm.CourseCode, mc.ModuleContentTitle;";


            string assessmentQuery = "SELECT * FROM CourseAssessment WHERE CourseCode = @CourseCode";

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                using (SqlCommand cmd = new SqlCommand(courseQuery, con))
                {
                    cmd.Parameters.AddWithValue("@CourseCode", courseCode);

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cCode.Text = reader["CourseCode"].ToString();
                            lblCourseName.Text = reader["CourseName"].ToString();
                            lblCourseCategory.Text = reader["CourseCategory"].ToString();
                            lblCourseLevel.Text = reader["CourseLevel"].ToString();
                            lblCourseLanguage.Text = reader["CourseLanguage"].ToString();
                            lblRequiredHours.Text = reader["RequiredHours"].ToString();
                            lblCreationDate.Text = Convert.ToDateTime(reader["CreationDate"]).ToString("yyyy-MM-dd");
                            lblStatus.Text = reader["Status"].ToString();
                            lblInstructor.Text = GetInstructorName(Convert.ToInt32(reader["InstructorID"]));
                            imgCourseImage.ImageUrl = reader["ImageURL"].ToString();
                        }
                        reader.Close();
                    }
                    con.Close();
                }


                using (SqlCommand cmd = new SqlCommand(courseModuleQuery, con))
                {
                    cmd.Parameters.AddWithValue("@CourseCode", courseCode);

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        GridViewCourseModule.DataSource = reader;
                        GridViewCourseModule.DataBind();
                        totalModules = GridViewCourseModule.Rows.Count;
                    }
                    con.Close();
                }


                using (SqlCommand cmd = new SqlCommand(assessmentQuery, con))
                {
                    cmd.Parameters.AddWithValue("@CourseCode", courseCode);

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        GridViewAssessmentList.DataSource = reader;
                        GridViewAssessmentList.DataBind();
                        totalAssessments = GridViewAssessmentList.Rows.Count;
                    }
                    con.Close();
                }
            }


            lblTotalModules.Text = totalModules.ToString();
            lblTotalAssessments.Text = totalAssessments.ToString();
        }



        private string GetInstructorName(int instructorId)
        {
            string instructorName = "";
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            string query = "SELECT InstructorName FROM CourseInstructor WHERE InstructorID = @InstructorID";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.Add(new SqlParameter("@InstructorID", SqlDbType.Int)).Value = instructorId;

                try
                {
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        instructorName = result.ToString();
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return instructorName;
        }


        protected void view9btnBack_Click(object sender, EventArgs e)
        {
            multiView.SetActiveView(view2);
            BindModuleList();
            BindAssessmentGridView();
            PopulateModules(Request.QueryString["CourseCode"]);
            GridView1.EditIndex = -1;
        }

        protected void PopulateInstructorDropDownView5(DropDownList dropDownList)
        {
            dropDownList.Items.Clear();
            dropDownList.Items.Add(new ListItem("-- Please select --", ""));


            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string query = "SELECT InstructorID, InstructorName FROM CourseInstructor";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string instructorID = reader["InstructorID"].ToString();
                        string instructorName = reader["InstructorName"].ToString();
                        dropDownList.Items.Add(new ListItem(instructorName, instructorID));
                    }

                    connection.Close();
                }
            }
        }





        protected void GridViewAssessment_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AddQuestion")
            {
                if (int.TryParse(Convert.ToString(e.CommandArgument), out int assessmentId))
                {
                    ViewState["SelectedAssessmentId"] = assessmentId;
                    multiView.SetActiveView(view6);

                    InitializeView6(assessmentId);
                    BindGridView();
                    GridView1.EditIndex = -1;
                }
            }


            if (e.CommandName == "View")
            {


                int courseAssessmentId = Convert.ToInt32(e.CommandArgument);
                ViewState["viewCourseAssessmentId"] = courseAssessmentId;
                multiView.SetActiveView(view7);
                LoadQuestionsAndAnswers(courseAssessmentId);


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


        protected void InitializeView6(int assessmentId)
        {

            Label lblAssessmentId = view6.FindControl("lblAssessmentId") as Label;
            if (lblAssessmentId != null)
            {
                lblAssessmentId.Text = assessmentId.ToString();
            }


            GridView gvQuestions = view6.FindControl("gvQuestions") as GridView;
            if (gvQuestions != null)
            {

                DataTable dtQuestions = GetQuestionsForAssessment(assessmentId);
                gvQuestions.DataSource = dtQuestions;
                gvQuestions.DataBind();
            }


            ViewState["CurrentAssessmentId"] = assessmentId;
        }


        private DataTable GetQuestionsForAssessment(int assessmentId)
        {

            DataTable dtQuestions = new DataTable();


            string connectionString = ConfigurationManager.ConnectionStrings["YourConnectionStringName"].ConnectionString;

            string query = @"
            SELECT QuestionID, QuestionText, AnswerOptions, CorrectAnswer
            FROM AssessmentQuestions
            WHERE CourseAssessmentID = @AssessmentID
            ORDER BY QuestionID";


            using (SqlConnection con = new SqlConnection(connectionString))
            {

                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    cmd.Parameters.AddWithValue("@AssessmentID", assessmentId);


                    con.Open();


                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dtQuestions);
                    }
                }
            }


            return dtQuestions;
        }




        protected void GridViewAssessment_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (GridViewAssessment.DataKeys.Count > e.RowIndex)
            {
                int assessmentId = Convert.ToInt32(GridViewAssessment.DataKeys[e.RowIndex].Value);
                DeleteAssessment(assessmentId);
                BindAssessmentGridView();
            }
        }


        protected void GridViewAssessment_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        private void PopulateInstructorDropdown()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT InstructorID, InstructorName FROM CourseInstructor";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        instructorDropdown.Items.Clear();
                        instructorDropdown.Items.Add(new ListItem("-- Please select --", ""));

                        while (reader.Read())
                        {
                            string id = reader["InstructorID"].ToString();
                            string name = reader["InstructorName"].ToString();
                            instructorDropdown.Items.Add(new ListItem(name, id));
                        }
                    }
                    con.Close();
                }
            }
        }


       





        protected void AddQuestionRedirect_click(object sender, EventArgs e)
        {
            multiView.SetActiveView(view6);
            BindGridView();
        }

        protected void btnSubmitQuestion_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;


            int courseAssessmentId = Convert.ToInt32(ViewState["SelectedAssessmentId"]);


            string question = questionInput.Text.Trim();
            int questionId = InsertQuestionAndGetId(question, courseAssessmentId, connectionString);


            var correctAnswers = new HashSet<string>(HiddenCorrectAnswer.Value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));


            InsertAnswer(answer1.Text.Trim(), correctAnswers.Contains("answer1"), questionId, connectionString);
            InsertAnswer(answer2.Text.Trim(), correctAnswers.Contains("answer2"), questionId, connectionString);
            InsertAnswer(answer3.Text.Trim(), correctAnswers.Contains("answer3"), questionId, connectionString);
            InsertAnswer(answer4.Text.Trim(), correctAnswers.Contains("answer4"), questionId, connectionString);

            ResetForm();
            BindGridView();


        }

        protected void AddQuestionToAssessmentBackBtn_Click(object sender, EventArgs e)
        {
            multiView.SetActiveView(view2);
            BindModuleList();
            BindAssessmentGridView();
            PopulateModules(ViewState["CourseCode"].ToString());
        }


        private int InsertQuestionAndGetId(string question, int courseAssessmentId, string connectionString)
        {
            string insertQuestionQuery = "INSERT INTO AssessmentQuestion (Question, CourseAssessmentID) OUTPUT INSERTED.AssessmentQuestionID VALUES (@Question, @CourseAssessmentID);";
            int questionId = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(insertQuestionQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@Question", question);
                        cmd.Parameters.AddWithValue("@CourseAssessmentID", courseAssessmentId);

                        con.Open();
                        questionId = (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (SqlException ex)
            {

                throw new InvalidOperationException("Error inserting question: " + ex.Message, ex);
            }
            return questionId;
        }

        private void InsertAnswer(string answerText, bool isCorrect, int questionId, string connectionString)
        {
            string insertAnswerQuery = "INSERT INTO AssessmentAnswer (AnswerName, IsCorrect, AssessmentQuestionID) VALUES (@AnswerName, @IsCorrect, @AssessmentQuestionID);";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(insertAnswerQuery, con))
                {
                    cmd.Parameters.AddWithValue("@AnswerName", answerText);
                    cmd.Parameters.AddWithValue("@IsCorrect", isCorrect);
                    cmd.Parameters.AddWithValue("@AssessmentQuestionID", questionId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void ResetForm()
        {
            questionInput.Text = string.Empty;
            answer1.Text = string.Empty;
            answer2.Text = string.Empty;
            answer3.Text = string.Empty;
            answer4.Text = string.Empty;
            HiddenCorrectAnswer.Value = string.Empty;


            btnSetCorrect1.CssClass = "btn btn-outline-secondary answer-button";
            btnSetCorrect2.CssClass = "btn btn-outline-secondary answer-button";
            btnSetCorrect3.CssClass = "btn btn-outline-secondary answer-button";
            btnSetCorrect4.CssClass = "btn btn-outline-secondary answer-button";
        }

        private void BindGridView()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                int courseAssessmentId = Convert.ToInt32(ViewState["SelectedAssessmentId"]);

                string query = @"
        SELECT aq.AssessmentQuestionID, aq.Question, ca.CourseCode
        FROM AssessmentQuestion aq
        INNER JOIN CourseAssessment ca ON aq.CourseAssessmentID = ca.CourseAssessmentID
        WHERE aq.CourseAssessmentID = @CourseAssessmentID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CourseAssessmentID", courseAssessmentId);

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }

            gvRelatedQuestions.DataSource = dt;
            gvRelatedQuestions.DataKeyNames = new string[] { "AssessmentQuestionID" };
            gvRelatedQuestions.DataBind();
        }


        protected void gvRelatedQuestions_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            int questionId = Convert.ToInt32(gvRelatedQuestions.DataKeys[e.RowIndex].Value);

            DeleteQuestion(questionId);

            BindGridView();
        }







        private void DeleteModuleAndContent(string associationCriteria)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {

                    string deleteModuleTopicQuery = @"
                DELETE FROM ModuleTopic 
                WHERE ModuleContentID IN (
                    SELECT ModuleContentID 
                    FROM ModuleContent 
                    WHERE AssociationCriteria = @AssociationCriteria
                )";
                    using (SqlCommand cmd = new SqlCommand(deleteModuleTopicQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@AssociationCriteria", associationCriteria);
                        cmd.ExecuteNonQuery();
                    }


                    string deleteModuleContentQuery = "DELETE FROM ModuleContent WHERE AssociationCriteria = @AssociationCriteria";
                    using (SqlCommand cmd = new SqlCommand(deleteModuleContentQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@AssociationCriteria", associationCriteria);
                        cmd.ExecuteNonQuery();
                    }


                    string deleteCourseModuleQuery = "DELETE FROM CourseModule WHERE AssociationCriteria = @AssociationCriteria";
                    using (SqlCommand cmd = new SqlCommand(deleteCourseModuleQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@AssociationCriteria", associationCriteria);
                        cmd.ExecuteNonQuery();
                    }


                    transaction.Commit();
                }
                catch (SqlException sqlEx)
                {
                    transaction.Rollback();

                    LogError(sqlEx);

                    lblErrorMessage.Text = $"A SQL error occurred: {sqlEx.Message}";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    LogError(ex);
                    lblErrorMessage.Text = "An error occurred while deleting the content. Please try again.";
                }
                finally
                {
                    con.Close();
                }
            }
        }



        private void LogError(Exception ex)
        {

            Debug.WriteLine(ex.ToString());
        }



        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            int index = Convert.ToInt32(e.RowIndex);

            string associationCriteria = GridView1.DataKeys[index].Values["AssociationCriteria"].ToString();

            DeleteModuleAndContent(associationCriteria);


            BindModuleList();
        }




        private void DeleteAssessment(int courseAssessmentId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {

                    string deleteAnswersQuery = "DELETE FROM AssessmentAnswer WHERE AssessmentQuestionID IN (SELECT AssessmentQuestionID FROM AssessmentQuestion WHERE CourseAssessmentID = @CourseAssessmentID)";
                    using (SqlCommand cmd = new SqlCommand(deleteAnswersQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@CourseAssessmentID", courseAssessmentId);
                        cmd.ExecuteNonQuery();
                    }


                    string deleteQuestionsQuery = "DELETE FROM AssessmentQuestion WHERE CourseAssessmentID = @CourseAssessmentID";
                    using (SqlCommand cmd = new SqlCommand(deleteQuestionsQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@CourseAssessmentID", courseAssessmentId);
                        cmd.ExecuteNonQuery();
                    }


                    string deleteAssessmentQuery = "DELETE FROM CourseAssessment WHERE CourseAssessmentID = @CourseAssessmentID";
                    using (SqlCommand cmd = new SqlCommand(deleteAssessmentQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@CourseAssessmentID", courseAssessmentId);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {

                    transaction.Rollback();

                    lblErrorMessage.Text = "Error deleting Assessment with ID: " + courseAssessmentId + "<br />Error: " + ex.Message;
                }
                finally
                {
                    con.Close();
                }
            }
        }







        private void DeleteQuestion(int questionId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                string deleteAnswersQuery = "DELETE FROM AssessmentAnswer WHERE AssessmentQuestionID = @QuestionID";
                using (SqlCommand cmd = new SqlCommand(deleteAnswersQuery, con))
                {
                    cmd.Parameters.AddWithValue("@QuestionID", questionId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }


                string deleteQuestionQuery = "DELETE FROM AssessmentQuestion WHERE AssessmentQuestionID = @QuestionID";
                using (SqlCommand cmd = new SqlCommand(deleteQuestionQuery, con))
                {

                    cmd.Parameters.AddWithValue("@QuestionID", questionId);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        protected void view7btnBack_Click(object sender, EventArgs e)
        {
            multiView.SetActiveView(view2);
            BindModuleList();
            BindAssessmentGridView();
            PopulateModules(ViewState["CourseCode"].ToString());
        }


        protected void view7btnEdit_Click(object sender, EventArgs e)
        {

            int courseAssessmentId = Convert.ToInt32(ViewState["viewCourseAssessmentId"]);


            LoadAssessmentDataForEdit(courseAssessmentId);


            multiView.SetActiveView(view8);

        }

        protected void ButtonSetCorrect_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string assessmentAnswerID = button.CommandArgument;

            // Retrieve the current list of IDs from the hidden field, splitting by the delimiter
            List<string> currentIDs = HiddenFieldCorrectAnswer.Value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            if (button.CssClass.Contains("btn-success"))
            {
                // If the button is already marked as correct, remove the ID from the list
                currentIDs.Remove(assessmentAnswerID);
                button.CssClass = "btn btn-outline-secondary";
            }
            else
            {
                // If the button is not marked as correct, add the ID to the list
                currentIDs.Add(assessmentAnswerID);
                button.CssClass = "btn btn-success";
            }

            // Update the hidden field with the new list of IDs
            HiddenFieldCorrectAnswer.Value = String.Join(";", currentIDs);

            // Assume we want to prevent a postback for immediate client-side changes
            ScriptManager.RegisterStartupScript(this, GetType(), "UpdateButtons", "updateButtons();", true);
        }






        private void LoadAssessmentDataForEdit(int courseAssessmentId)
        {
            ClearPreviousAnswers(); // Clear any existing data

            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            string query = @"
        SELECT q.AssessmentQuestionID, q.Question, a.AnswerName, a.IsCorrect, a.AssessmentAnswerID
        FROM AssessmentQuestion q
        LEFT JOIN AssessmentAnswer a ON q.AssessmentQuestionID = a.AssessmentQuestionID
        WHERE q.CourseAssessmentID = @CourseAssessmentID";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CourseAssessmentID", courseAssessmentId);

                try
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        int answerIndex = 1;

                        while (reader.Read())
                        {
                            if (answerIndex == 1)
                            {
                                // Set the question
                                TextBox txtQuestion = (TextBox)multiView.FindControl("TextBoxQuestion");
                                txtQuestion.Text = reader["Question"].ToString();
                                ViewState["EditingQuestionID"] = Convert.ToInt32(reader["AssessmentQuestionID"]);
                            }

                            // Set the answer text
                            TextBox txtAnswer = (TextBox)multiView.FindControl("TextBoxAnswer" + answerIndex);
                            if (txtAnswer != null)
                            {
                                txtAnswer.Text = reader["AnswerName"].ToString();
                            }

                            // Set the CommandArgument for the button with the AssessmentAnswerID
                            Button btnSetCorrect = (Button)multiView.FindControl("ButtonSetCorrect" + answerIndex);
                            if (btnSetCorrect != null)
                            {
                                btnSetCorrect.CommandArgument = reader["AssessmentAnswerID"].ToString();
                            }

                            // Set the button's CSS class
                            if (btnSetCorrect != null)
                            {
                                btnSetCorrect.CssClass = Convert.ToBoolean(reader["IsCorrect"]) ? "btn btn-success" : "btn btn-outline-secondary";
                            }

                            answerIndex++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                }
                finally
                {
                    con.Close();
                }
            }
        }




        private void ClearPreviousAnswers()
        {

            TextBox txtQuestion = (TextBox)multiView.FindControl("TextBoxQuestion");
            if (txtQuestion != null)
            {
                txtQuestion.Text = string.Empty;
            }


            for (int i = 1; i <= 4; i++)
            {
                TextBox txtAnswer = (TextBox)multiView.FindControl("TextBoxAnswer" + i);
                Button btnSetCorrect = (Button)multiView.FindControl("ButtonSetCorrect" + i);

                if (txtAnswer != null)
                {
                    txtAnswer.Text = string.Empty;
                }

                if (btnSetCorrect != null)
                {
                    btnSetCorrect.CssClass = "btn btn-outline-secondary";
                    btnSetCorrect.Text = "Set Correct Answer";
                    btnSetCorrect.CommandArgument = string.Empty;
                }
            }
        }


        protected void view8ButtonSaveChanges(object sender, EventArgs e)
        {
            int questionId = Convert.ToInt32(ViewState["EditingQuestionID"]);
            string updatedQuestion = ((TextBox)multiView.FindControl("TextBoxQuestion")).Text;
            UpdateQuestion(questionId, updatedQuestion);

            // Assuming the hidden field contains a semicolon-separated list of correct answer IDs
            System.Diagnostics.Debug.WriteLine("HiddenFieldCorrectAnswer: " + HiddenFieldCorrectAnswer.Value);

            string[] correctAnswerIds = HiddenFieldCorrectAnswer.Value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            HashSet<int> correctAnswerIdSet = new HashSet<int>(correctAnswerIds.Select(id => int.Parse(id)));

            // Debugging: Check the content of the HashSet
            System.Diagnostics.Debug.WriteLine("Correct IDs: " + string.Join(", ", correctAnswerIdSet));

            // ... rest of your code ...

            // Ensure that you are using try-catch to handle any exceptions during database operations
            try
            {
                for (int i = 1; i <= 4; i++)
                {
                    TextBox txtAnswer = (TextBox)multiView.FindControl($"TextBoxAnswer{i}");
                    Button btnSetCorrect = (Button)multiView.FindControl($"ButtonSetCorrect{i}");

                    if (txtAnswer != null && btnSetCorrect != null)
                    {
                        string answerText = txtAnswer.Text;
                        int answerId = int.Parse(btnSetCorrect.CommandArgument);
                        bool isAnswerCorrect = correctAnswerIdSet.Contains(answerId);

                        UpdateAnswer(answerId, answerText, isAnswerCorrect);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                System.Diagnostics.Debug.WriteLine("Error in view8ButtonSaveChanges: " + ex.Message);
            }

            LoadQuestionIntoView8(questionId);
            multiView.SetActiveView(view8);
            HiddenFieldCorrectAnswer.Value = "";
        }



        private void UpdateAnswer(int answerId, string answerText, bool isCorrect)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            string query = "UPDATE AssessmentAnswer SET AnswerName = @AnswerName, IsCorrect = @IsCorrect WHERE AssessmentAnswerID = @AnswerID";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.Add(new SqlParameter("@AnswerID", SqlDbType.Int)).Value = answerId;
                cmd.Parameters.Add(new SqlParameter("@AnswerName", SqlDbType.NVarChar)).Value = answerText;
                cmd.Parameters.Add(new SqlParameter("@IsCorrect", SqlDbType.Bit)).Value = isCorrect;

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
            }
        }



        private void UpdateQuestion(int questionId, string questionText)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            string query = "UPDATE AssessmentQuestion SET Question = @Question WHERE AssessmentQuestionID = @QuestionID";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.Add(new SqlParameter("@QuestionID", SqlDbType.Int)).Value = questionId;
                cmd.Parameters.Add(new SqlParameter("@Question", SqlDbType.NVarChar)).Value = questionText;

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }
            }
        }





        protected void gvRelatedQuestions_RowEditing2(object sender, GridViewEditEventArgs e)
        {

            GridView gv = sender as GridView;
            int questionId = Convert.ToInt32(gv.DataKeys[e.NewEditIndex].Value);
            ViewState["EditingQuestionID"] = questionId;

            // Load the question details
            LoadQuestionIntoView8(questionId);

            // Load answers for the question
            LoadAnswersForQuestion(questionId);

            // Switch to the appropriate view for editing
            multiView.SetActiveView(view8);
            e.Cancel = true;
        }

        public class Answer
        {
            public int AssessmentAnswerID { get; set; }
            public string AnswerName { get; set; }
            public bool IsCorrect { get; set; }
        }

        private void LoadAnswersForQuestion(int questionId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            string answersQuery = "SELECT AssessmentAnswerID, AnswerName, IsCorrect FROM AssessmentAnswer WHERE AssessmentQuestionID = @QuestionID";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand answersCmd = new SqlCommand(answersQuery, con);
                answersCmd.Parameters.AddWithValue("@QuestionID", questionId);

                try
                {
                    con.Open();
                    List<string> correctAnswerIDs = new List<string>();

                    using (SqlDataReader answersReader = answersCmd.ExecuteReader())
                    {
                        int answerIndex = 1;
                        while (answersReader.Read() && answerIndex <= 4) // Assuming you have a maximum of 4 answers
                        {
                            TextBox txtAnswer = (TextBox)multiView.FindControl("TextBoxAnswer" + answerIndex);
                            Button btnSetCorrect = (Button)multiView.FindControl("ButtonSetCorrect" + answerIndex);

                            if (txtAnswer != null && btnSetCorrect != null)
                            {
                                txtAnswer.Text = answersReader["AnswerName"].ToString();
                                btnSetCorrect.CommandArgument = answersReader["AssessmentAnswerID"].ToString();

                                bool isCorrect = (bool)answersReader["IsCorrect"];
                                btnSetCorrect.CssClass = isCorrect ? "btn btn-success" : "btn btn-outline-secondary";

                                // If the answer is marked as correct in the database, add its ID to the list
                                if (isCorrect)
                                {
                                    correctAnswerIDs.Add(answersReader["AssessmentAnswerID"].ToString());
                                }
                            }
                            answerIndex++;
                        }
                    }

                    // Set the hidden field value to the list of correct answer IDs, joined by a semicolon
                    HiddenFieldCorrectAnswer.Value = String.Join(";", correctAnswerIDs);
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    // Log the error and/or inform the user as necessary
                }
                finally
                {
                    con.Close();
                }
            }
        }




        private void LoadQuestionIntoView8(int questionId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;


            ClearPreviousAnswers();

            string questionQuery = "SELECT Question FROM AssessmentQuestion WHERE AssessmentQuestionID = @QuestionID";
            string answersQuery = "SELECT AssessmentAnswerID, AnswerName, IsCorrect FROM AssessmentAnswer WHERE AssessmentQuestionID = @QuestionID";

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                SqlCommand questionCmd = new SqlCommand(questionQuery, con);
                questionCmd.Parameters.AddWithValue("@QuestionID", questionId);


                SqlCommand answersCmd = new SqlCommand(answersQuery, con);
                answersCmd.Parameters.AddWithValue("@QuestionID", questionId);

                try
                {
                    con.Open();


                    using (SqlDataReader questionReader = questionCmd.ExecuteReader())
                    {
                        if (questionReader.Read())
                        {
                            TextBox txtQuestion = (TextBox)multiView.FindControl("TextBoxQuestion");
                            if (txtQuestion != null)
                            {
                                txtQuestion.Text = questionReader["Question"].ToString();
                            }
                        }
                    }


                    using (SqlDataReader answersReader = answersCmd.ExecuteReader())
                    {
                        int answerIndex = 1;
                        while (answersReader.Read() && answerIndex <= 4)
                        {
                            TextBox txtAnswer = (TextBox)multiView.FindControl("TextBoxAnswer" + answerIndex);
                            Button btnSetCorrect = (Button)multiView.FindControl("ButtonSetCorrect" + answerIndex);

                            if (txtAnswer != null && btnSetCorrect != null)
                            {
                                txtAnswer.Text = answersReader["AnswerName"].ToString();
                                btnSetCorrect.CssClass = (bool)answersReader["IsCorrect"] ? "btn btn-success" : "btn btn-outline-secondary";
                                btnSetCorrect.CommandArgument = answersReader["AssessmentAnswerID"].ToString();
                            }
                            answerIndex++;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {

                    con.Close();
                }
            }
        }







        protected void view8ButtonBack(object sender, EventArgs e)
        {
            int assessmentId = Convert.ToInt32(ViewState["SelectedAssessmentId"]);
            multiView.SetActiveView(view6);


            InitializeView6(assessmentId);
            BindGridView();
            GridView1.EditIndex = -1;
        }


    }
}