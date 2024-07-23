using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mime;
using System.Web.DynamicData;

namespace WebApplication1
{
    public partial class referencematerials : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

           

            if (!IsPostBack)
            {
                BindReferenceGridView();
            }
        }

        private void BindReferenceGridView()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT content_id, content_title, language, author_name, content_cat, content_type FROM Ref_Material WHERE status = 'Approved'", con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        GridViewReference.DataSource = dataTable;
                        GridViewReference.DataBind();
                    }
                    con.Close();
                }
            }
        }


        protected void GridViewReference_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetails")
            {
               


                if (!string.IsNullOrEmpty(e.CommandArgument?.ToString()))
                {
                    if (int.TryParse(e.CommandArgument.ToString(), out int contentId))
                    {

                        
                        // Call the JavaScript function to show the modal with details
                        ScriptManager.RegisterStartupScript(this, GetType(), "showModal", "$('#myModal').modal('show');", true);

                        // Call a method to load modal details using the ViewState["viewContentId"]
                        LoadModalDetails(contentId);
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



        private void LoadModalDetails(int contentId)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = @"SELECT rm.content_title, rm.content_desc, im.ImageURL, v.VideoURL
                            FROM Ref_Material rm 
                            LEFT JOIN Image im ON rm.content_id = im.content_id
                            LEFT JOIN Video v ON rm.content_id = v.content_id
                            WHERE rm.content_id = @contentId";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@contentId", contentId);
                        con.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Extract values from the reader
                                string contentTitle = reader["content_title"].ToString();
                                string contentDesc = reader["content_desc"].ToString();
                                string imageURL = reader["ImageURL"].ToString();
                                string videoURL = reader["VideoURL"] != DBNull.Value ? reader["VideoURL"].ToString() : string.Empty;

                                // Set the text of labels
                                lblContentTitle.Text = contentTitle;
                                lblContentDesc.Text = contentDesc;


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

                string query = "SELECT content_id, content_title, language, author_name,content_cat, content_type, status FROM Ref_Material WHERE content_title LIKE @searchText OR author_name LIKE @searchText";


                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    cmd.Parameters.AddWithValue("@searchText", "%" + txtSearch.Text.Trim() + "%");


                    con.Open();


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        GridViewReference.DataSource = dataTable;
                        GridViewReference.DataBind();
                    }


                    con.Close();
                }
            }
        }

        protected void LoadImages(int contentId)
        {

            DataTable dtImages = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT Image_id, ImageURL, UploadedDate FROM Image WHERE content_id = @contentId";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@contentId", contentId);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtImages);
                    }
                }
            }



        }

        protected void GridViewVideos_RowCommand(object sender, GridViewCommandEventArgs e)
        {


            int contentId = Convert.ToInt32(ViewState["ContentIDViewstate"]);
            BindGridViewVideos(contentId);


        }

        private void BindGridViewVideos(int contentId)
        {


            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string query = "SELECT VideoURL FROM Video where content_id = @content_id";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable videoData = new DataTable();
                    cmd.Parameters.AddWithValue("@contentId", contentId);
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

        private DataTable GetDataById(int contentId)
        {
            DataTable dataTable = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Ref_Material WHERE content_id = @contentId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@contentId", contentId);
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

        protected void GridViewRefMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {


            int index = GridViewReference.SelectedIndex;

            int contentId = Convert.ToInt32(GridViewReference.DataKeys[index].Value);

            ViewState["ContentIDViewstate"] = contentId;

            DataTable dt = GetDataById(contentId);


            LoadImages(contentId);
            BindGridViewVideos(contentId);

        }

        protected void GridViewRefMaterial_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewReference.PageIndex = e.NewPageIndex;
            BindReferenceGridView();
        }

        private DataTable GetData()
        {
            DataTable dataTable = new DataTable();

            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                string query = "SELECT content_id, content_cat, content_type, content_title, language, author_name, status FROM Ref_Material";

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

        protected void GridViewRefMaterial_Sorting(object sender, GridViewSortEventArgs e)
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

                GridViewReference.DataSource = SortDataTable(dataTable, e.SortExpression, sortDirection);
                GridViewReference.DataBind();
            }
        }
    }
}

