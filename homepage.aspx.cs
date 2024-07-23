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
    public partial class homepage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            // Check if the user has user access during page load
            if (Session["UserAccess"] == null)
            {
                // Redirect to the login page for users without admin access
                Response.Redirect("userlogin.aspx");
            }

            if (!IsPostBack)
            {
                BindAnnouncementGridView();
            }


        }

        private void BindAnnouncementGridView()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(@"SELECT 
                                            a.ann_id, 
                                            a.ann_title, 
                                            a.ann_content, 
                                            u.full_name as announcer, 
                                            a.ann_status, 
                                            a.date_posted 
                                        FROM 
                                            Announcement a 
                                        JOIN 
                                            users u ON a.announcer = u.user_id", con))

                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        GridViewAnnouncement.DataSource = dataTable;
                        GridViewAnnouncement.DataBind();
                    }
                    con.Close();
                }
            }
        }


        protected void GridViewAnnouncement_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetails")
            {



                if (!string.IsNullOrEmpty(e.CommandArgument?.ToString()))
                {
                    if (int.TryParse(e.CommandArgument.ToString(), out int annId))
                    {


                        // Call the JavaScript function to show the modal with details
                        ScriptManager.RegisterStartupScript(this, GetType(), "showModal", "$('#myModal').modal('show');", true);

                        // Call a method to load modal details using the ViewState["viewAnnouncementId"]
                        LoadModalDetails(annId);


                    }
                    else
                    {
                        // Handle the case where the CommandArgument cannot be converted to an integer.
                    }
                }
                else
                {
                    // Handle the case where CommandArgument is null or empty.
                }
            }
        }


        private bool IsYouTubeVideo(string videoURL)
        {
            return videoURL.Contains("youtube.com") || videoURL.Contains("youtu.be");
        }

        private string GetYouTubeEmbeddedPlayer(string videoURL)
        {
            // Extract YouTube video ID from the URL
            string videoId = GetYouTubeVideoId(videoURL);

            // Generate YouTube embedded player HTML
            return $"<iframe class='embed-responsive-item modal-video' src='https://www.youtube.com/embed/{videoId}' frameborder='0' allowfullscreen></iframe>";
        }

        private string GetYouTubeVideoId(string videoURL)
        {
            // Extract video ID from YouTube URL
            Uri uri = new Uri(videoURL);
            string videoId = uri.Segments.Last(); // Get the last segment of the path, which is the video ID
            videoId = videoId.Trim('/'); // Remove any trailing slashes

            return videoId;
        }



        private void LoadModalDetails(int annId)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = @"SELECT ann_title, ann_content, image.ImageURL, video.VideoURL
                            FROM Announcement 
                            LEFT JOIN Image ON Announcement.ann_id = Image.ann_id
                            LEFT JOIN Video ON Announcement.ann_id = Video.ann_id
                            WHERE Announcement.ann_id = @annId";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@annId", annId);
                        con.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Extract values from the reader
                                string annTitle = reader["ann_title"].ToString();
                                string annContent = reader["ann_content"].ToString();
                                string imageURL = reader["ImageURL"].ToString();
                                string videoURL = reader["VideoURL"] != DBNull.Value ? reader["VideoURL"].ToString() : string.Empty;

                                // Set the text of labels
                                lblAnnouncementTitle.Text = annTitle;
                                lblAnnouncementDesc.Text = annContent;

                                // Set the src attribute of the image element
                                imgContent.ImageUrl = imageURL;

                                if (IsYouTubeVideo(videoURL))
                                {
                                    string embeddedPlayerHtml = GetYouTubeEmbeddedPlayer(videoURL);
                                    litYouTubePlayer.Text = embeddedPlayerHtml;
                                }
                                else
                                {
                                    // Handle non-YouTube videos (e.g., display using HTML5 video tag)
                                    // Modify this part based on your specific needs
                                    litYouTubePlayer.Text = string.Empty;
                                }

                                // Update the modal content using JavaScript
                                ClientScript.RegisterStartupScript(this.GetType(), "UpdateModalContent", "updateModalContent();", true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle exceptions (for debugging)
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
            }
        }














        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                string query = "SELECT ann_id, ann_title, ann_content, announcer, date_posted FROM Announcement WHERE ann_title LIKE @searchText";


                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    cmd.Parameters.AddWithValue("@searchText", "%" + txtSearch.Text.Trim() + "%");


                    con.Open();


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        GridViewAnnouncement.DataSource = dataTable;
                        GridViewAnnouncement.DataBind();
                    }


                    con.Close();
                }
            }
        }

        protected void LoadImages(int annId)
        {

            DataTable dtImages = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT Image_id, ImageURL, UploadedDate FROM Image WHERE ann_id = @annId";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@annId", annId);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtImages);
                    }
                }
            }



        }

        protected void GridViewVideos_RowCommand(object sender, GridViewCommandEventArgs e)
        {


            int annId = Convert.ToInt32(ViewState["AnnouncementIDViewstate"]);
            BindGridViewVideos(annId);


        }

        private void BindGridViewVideos(int annId)
        {


            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string query = "SELECT VideoURL FROM Video where ann_id = @ann_id";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable videoData = new DataTable();
                    cmd.Parameters.AddWithValue("@annId", annId);
                    try
                    {
                        connection.Open();
                        adapter.Fill(videoData);

                        if (videoData.Rows.Count > 0)
                        {

                        }
                        else
                        {

                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        private DataTable GetDataById(int annId)
        {
            DataTable dataTable = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Announcement WHERE ann_id = @annId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@annId", annId);
                    con.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                    con.Close();
                }
            }

            return dataTable;
        }

        protected void GridViewAnnouncement_SelectedIndexChanged(object sender, EventArgs e)
        {


            int index = GridViewAnnouncement.SelectedIndex;

            int annId = Convert.ToInt32(GridViewAnnouncement.DataKeys[index].Value);

            ViewState["AnnouncementViewstate"] = annId;

            DataTable dt = GetDataById(annId);


            LoadImages(annId);
            BindGridViewVideos(annId);

        }

        protected void GridViewAnnouncement_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewAnnouncement.PageIndex = e.NewPageIndex;
            BindAnnouncementGridView();
        }

        private DataTable GetData()
        {
            DataTable dataTable = new DataTable();

            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                string query = "SELECT ann_id, ann_title, ann_content, announcer, date_posted FROM Announcement";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();


                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }

                    con.Close();
                }
            }

            return dataTable;
        }
        private DataView SortDataTable(DataTable dataTable, string sortExpression, string direction)
        {
            dataTable.DefaultView.Sort = sortExpression + " " + direction;
            return dataTable.DefaultView;
        }

        protected void GridViewAnnouncement_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dataTable = GetData();

            if (dataTable != null)
            {

                string sortDirection = "ASC";
                if (ViewState["SortExpression"] != null && ViewState["SortExpression"].ToString() == e.SortExpression)
                {
                    ViewState["SortExpression"] = null;
                    sortDirection = "DESC";
                }
                else
                {
                    ViewState["SortExpression"] = e.SortExpression;
                }

                GridViewAnnouncement.DataSource = SortDataTable(dataTable, e.SortExpression, sortDirection);
                GridViewAnnouncement.DataBind();
            }
        }
    
    }
}