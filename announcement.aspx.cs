using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class announcement : System.Web.UI.Page
    {

        protected void btnBackDashboard_Click(object sender, EventArgs e)
        {
            // Redirect back to adminhomepage.aspx
            Response.Redirect("adminhomepage.aspx");
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                // Check if the user has admin access during page load
                if (Session["AdminAccess"] == null && Session["SAdminAccess"] == null)
                {
                    Response.Redirect("userlogin.aspx");
                }


                BindGridViewAnnouncements();
                PopulateAnnouncerDropDownList();
            }
        }


        protected void btnCreateAnnouncement_Click(object sender, EventArgs e)
        {
          
            string announcementTitle = txtNewAnnouncementTitle.Text.Trim();
            string announcementContent = txtNewAnnouncementContent.Text.Trim();
            int announcerId = Convert.ToInt32(ddlAnnouncer.SelectedValue);
         

         
            if (string.IsNullOrEmpty(announcementTitle) )
            {
                ShowSweetAlert("Empty?", "Where annoucement title?..", "question");
                return;
            }else if (string.IsNullOrEmpty(announcementContent))
            {
                ShowSweetAlert("Empty?", "Where annoucement content?..", "question");
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            INSERT INTO Announcement (ann_title, ann_content, announcer, ann_status) 
            VALUES (@Title, @Content, @Announcer, 'Unpublished')"; 

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", announcementTitle);
                    cmd.Parameters.AddWithValue("@Content", announcementContent);
                    cmd.Parameters.AddWithValue("@Announcer", announcerId);
                

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        BindGridViewAnnouncements();
                        ShowSweetAlert("Success", "Announcement created successfully.", "success");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "createAnnouncementModal", "$('#createAnnouncementModal').modal('hide');", true);
                        txtNewAnnouncementTitle.Text = "";
                        txtNewAnnouncementContent.Text = "";
                      
                      

                    }
                    else
                    {
                       
                    }

                    conn.Close();
                }
            }
        }


        protected void btnShowCreateModal_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "createAnnouncementModal", "$('#createAnnouncementModal').modal('show');", true);
        }



        protected void btnRefresh_ServerClick(object sender, EventArgs e)
        {
         
            BindGridViewAnnouncements();
            string script = "showSuccessMessage();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccessScript", script, true);


        }
        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Get the search text from the TextBox
            string searchText = txtSearch.Text.Trim();

            // Initialize a new DataTable or use a method to get the data
            DataTable dtAnnouncements = new DataTable();

            // Define your connection string (you'll find this in your Web.config)
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            // Define the query with a WHERE clause to filter the results
            string query = @"
        SELECT * 
        FROM Announcement 
        WHERE ann_title LIKE @SearchText OR ann_content LIKE @SearchText";

            // Use using statement for better resource management
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Create a SqlCommand
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add parameters to prevent SQL injection
                    cmd.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");

                    // Open the connection
                    conn.Open();

                    // Execute the command
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataTable with the result of the SQL query
                        adapter.Fill(dtAnnouncements);
                    }
                }
            }

            // Bind the GridView to the DataTable
            GridViewAnnouncements.DataSource = dtAnnouncements;
            GridViewAnnouncements.DataBind();
        }

        protected void GridViewAnnouncements_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
          
            GridViewAnnouncements.PageIndex = e.NewPageIndex;

            BindGridViewAnnouncements();

        }

        protected void GridViewAnnouncements_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
               
                string status = e.Row.Cells[4].Text;

                switch (status)
                {
                    case "Published":
                        e.Row.Cells[4].ForeColor = System.Drawing.Color.Green;
                        break;
                    case "Unpublished":
                        e.Row.Cells[4].ForeColor = System.Drawing.Color.Orange;
                        break;
                }
            }
        }


        protected void GridViewAnnouncements_Sorting(object sender, GridViewSortEventArgs e)
        {
          
            DataTable dataTable = GetAnnouncementsData(); 

            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);

              
                if (ViewState["SortDirection"] as string == "ASC" && ViewState["SortExpression"] as string == e.SortExpression)
                {
                   
                    ViewState["SortDirection"] = "DESC";
                }
                else
                {
                    ViewState["SortDirection"] = "ASC";
                }

                ViewState["SortExpression"] = e.SortExpression;

            
                dataView.Sort = e.SortExpression + " " + ViewState["SortDirection"];

              
                GridViewAnnouncements.DataSource = dataView;
                GridViewAnnouncements.DataBind();
            }
        }

       
        private DataTable GetAnnouncementsData()
        {
           
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Announcement", conn))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }

        private DataTable GetAnnouncementDetails(int annId)
        {
            DataTable dataTable = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Announcement WHERE ann_id = @AnnId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@AnnId", annId);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            con.Open();
                            adapter.Fill(dataTable);
                        }
                        catch (Exception ex)
                        {
                            // Handle the exception
                            // Log the error or display a message to the user
                        }
                        finally
                        {
                            con.Close();
                        }
                    }
                }
            }

            return dataTable;
        }



        protected void GridViewAnnouncements_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = GridViewAnnouncements.SelectedIndex;
            int annId = Convert.ToInt32(GridViewAnnouncements.DataKeys[index].Value);

            ViewState["AnnouncementIDViewState"] = annId;

            DataTable dt = GetAnnouncementDetails(annId);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                spanAnnId.InnerText = row["ann_id"].ToString();
                ann_title.Text = row["ann_title"].ToString();
                ann_content.Text = row["ann_content"].ToString();         
                spanAnnStatus.InnerText = row["ann_status"].ToString();
                if (spanAnnStatus.InnerText.Equals("Published", StringComparison.OrdinalIgnoreCase))
                {
                    spanAnnStatus.Style["color"] = "green"; 
                }
                else
                {
                    spanAnnStatus.Style["color"] = "orange";
                }

                LoadAnnouncementImages(annId); 
                LoadAnnouncementVideos(annId);

                announcer.Enabled = true;
                ann_content.Enabled = true;
                ann_title.Enabled = true;
                btnUpdate.Enabled = true; 
                btnDelete.Enabled = true;
                Button5.Enabled = true;
                btnUploadVideo.Enabled = true;
                btnToggleStatus.Enabled = true;

                ShowSweetAlert("Success", "Announcement retrieved successfully.", "success");
            }
        }


        protected void LoadAnnouncementImages(int annId)
        {
            DataTable dtImages = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT Image_id, ImageURL, UploadedDate FROM Image WHERE ann_id = @AnnId"; 
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@AnnId", annId);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtImages);
                    }
                }
            }

            GridViewAnnouncementImages.DataSource = dtImages; 
            GridViewAnnouncementImages.DataBind();
        }

        private void LoadAnnouncementVideos(int annId)
        {
            DataTable dtVideo = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT video_id, VideoURL, UploadedDate FROM Video WHERE ann_id = @AnnId"; // Modify table and field names as needed
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@AnnId", annId);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtVideo);
                    }
                }
            }

            GridViewAnnouncementVideos.DataSource = dtVideo; // Replace with your actual GridView for announcement videos
            GridViewAnnouncementVideos.DataBind();
        }


        protected void GridViewImages_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteImage")
            {
                int imageId = Convert.ToInt32(e.CommandArgument);
                DeleteImage(imageId);
            }

            if (e.CommandName == "ViewImage")
            {
                if (e.CommandName == "ViewImage")
                {
                    string imageUrl = (string)e.CommandArgument;

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowImageModalScript", $"showImageModal('{imageUrl}');", true);
                }

            }
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


        private void InsertAnnouncementImageIntoDatabase(string imageUrl, DateTime uploadedDate, int annId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Adjust the table and column names as per your announcement image table
                string sql = "INSERT INTO Image (ImageURL, UploadedDate, ann_id) VALUES (@ImageURL, @UploadedDate, @AnnId)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ImageURL", imageUrl);
                    cmd.Parameters.AddWithValue("@UploadedDate", uploadedDate);
                    cmd.Parameters.AddWithValue("@AnnId", annId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        protected void GridViewVideos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteVideo")
            {
                int videoIdToDelete = Convert.ToInt32(e.CommandArgument);
                bool isDeleted = DeleteAnnouncementVideoFromDatabase(videoIdToDelete);

                if (isDeleted)
                {
                    ShowSweetAlert("Success", "Video link has been deleted.", "success");
                }
                else
                {
                    ShowSweetAlert("Error", "Could not delete the video.", "error");
                }

                // Assuming you're storing the announcement ID in ViewState when an announcement is selected
                int annId = Convert.ToInt32(ViewState["AnnouncementIDViewState"]);
                LoadAnnouncementVideos(annId);

                // Update the modal display if needed
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowVideoUploadModalScript", "ShowVideoUploadModal();", true);
            }
        }

        private bool DeleteAnnouncementVideoFromDatabase(int videoId)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Adjust the table name if needed
                    string deleteQuery = "DELETE FROM Video WHERE video_id = @VideoId";

                    using (SqlCommand cmd = new SqlCommand(deleteQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@VideoId", videoId);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Optionally log the exception
                return false;
            }
        }

        protected void btnAddVideoLink_Click(object sender, EventArgs e)
        {
            // Assuming you're storing the announcement ID in ViewState when an announcement is selected
            int selectedAnnouncementID = Convert.ToInt32(ViewState["AnnouncementIDViewState"]);

            string videoLink = txtVideoLink.Value.ToString();

            if (selectedAnnouncementID > 0 && !string.IsNullOrEmpty(videoLink))
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Adjust the table and column names as per your announcement video table
                    string insertVideoQuery = "INSERT INTO Video (VideoURL, UploadedDate, ann_id) VALUES (@VideoURL, @UploadedDate, @AnnId)";

                    using (SqlCommand cmd = new SqlCommand(insertVideoQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@VideoURL", videoLink);
                        cmd.Parameters.AddWithValue("@UploadedDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@AnnId", selectedAnnouncementID);

                        cmd.ExecuteNonQuery();
                    }

                    connection.Close();

                    // Refresh the video list
                    LoadAnnouncementVideos(selectedAnnouncementID);

                    ShowSweetAlert("Success", "Video link has been added.", "success");
                }
            }
            else
            {
                ShowSweetAlert("Missing Data", "Please provide a video link.", "warning");
            }
        }



        protected void btnUploadImageModal_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                string[] validFileTypes = { "jpeg", "png", "jpg" };
                string ext = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName).ToLower();

                if (!validFileTypes.Any(item => ext.EndsWith(item)))
                {
                    ShowSweetAlert("Invalid", "Please select an Image file!", "error");
                }
                else
                {
                    try
                    {
                        string fileName = Path.GetFileName(FileUpload1.FileName);
                        string imagePath = Path.Combine(Server.MapPath("~/images/"), fileName);
                        FileUpload1.SaveAs(imagePath);

                        string imgurUrl = UploadToImgur(imagePath);

                        // Assuming you're storing the announcement ID in ViewState when an announcement is selected
                        int annId = Convert.ToInt32(ViewState["AnnouncementIDViewState"]);

                        InsertAnnouncementImageIntoDatabase(imgurUrl, DateTime.Now, annId);

                        LoadAnnouncementImages(annId);

                        ShowSweetAlert("Success", "Image Uploaded Successfully.", "success");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("An error occurred: " + ex.Message);
                        ShowSweetAlert("Error", "An error occurred while uploading the image.", "error");
                    }
                }
            }
            else
            {
                ShowSweetAlert("Missing Image", "No image detected. Please select an image to upload.", "warning");
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModalScript", "$('#imageListModal').modal('show');", true);
        }




        private void DeleteImage(int imageId)
        {
           
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Image WHERE Image_id = @ImageId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ImageId", imageId);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                      
                        LoadAnnouncementImages(Convert.ToInt32(ViewState["AnnouncementIDViewState"]));
                    }
                    catch (Exception ex)
                    {
                       
                    }
                }
            }
        }


        private void ShowSweetAlert(string title, string message, string type)
        {
            string script = $"Swal.fire({{ title: '{title}', text: '{message}', icon: '{type}', confirmButtonText: 'Ok' }});";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "sweetAlert", script, true);
        }

   

        private DataRow GetAnnouncementById(int announcementId)
        {
            // Replace this with your actual data access logic.
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Announcement WHERE ann_id = @AnnId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AnnId", announcementId);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        if (dataTable.Rows.Count > 0)
                        {
                            return dataTable.Rows[0];
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }


        private void PopulateAnnouncerDropDownList()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Corrected the column name to full_name as per the CREATE TABLE statement.
                string query = "SELECT user_id, full_name FROM users WHERE role = 'Admin'"; // The role should be in single quotes for SQL string literals

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dtInstructors = new DataTable();

                    try
                    {
                        conn.Open();
                        adapter.Fill(dtInstructors);

                        ddlAnnouncer.Items.Clear();

                        ListItem defaultItem = new ListItem("-- Please select --", "")
                        {
                            Enabled = true, // You might want to enable this if it doesn't show up when disabled
                            Selected = true
                        };
                        ddlAnnouncer.Items.Add(defaultItem);

                        foreach (DataRow row in dtInstructors.Rows)
                        {
                            ListItem listItem = new ListItem
                            {
                                Text = row["full_name"].ToString(), // Corrected to full_name
                                Value = row["user_id"].ToString()
                            };
                            ddlAnnouncer.Items.Add(listItem);
                            // Ensure this dropdown exists or remove this line if not needed
                            // announcer.Items.Add(listItem);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the exception or handle it
                        throw ex; // Re-throwing the exception for simplicity, consider logging it instead
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }


        private void BindGridViewAnnouncements()
        {
          
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            string query = "SELECT * FROM Announcement";

            DataTable dtAnnouncements = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        conn.Open();
                        adapter.Fill(dtAnnouncements);
                    }
                }
            }

            GridViewAnnouncements.DataSource = dtAnnouncements;
            GridViewAnnouncements.DataBind();
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
              
                int annId = Convert.ToInt32(ViewState["AnnouncementIDViewState"]);
                string updatedTitle = ann_title.Text;
                string updatedContent = ann_content.Text;
               

               
                bool isUpdated = UpdateAnnouncement(annId, updatedTitle, updatedContent);

                if (isUpdated)
                {
                    // Successful update
                    ShowSweetAlert("Success", "Announcement updated successfully.", "success");
                }
                else
                {
                    // Failed to update
                    ShowSweetAlert("Error", "Failed to update announcement.", "error");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the update process
                ShowSweetAlert("Error", "An error occurred: " + ex.Message, "error");
            }
        }


        public bool UpdateAnnouncement(int annId, string updatedTitle, string updatedContent)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string updateQuery = "UPDATE Announcement SET ann_title = @UpdatedTitle, ann_content = @UpdatedContent WHERE ann_id = @AnnouncementId";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@AnnouncementId", annId);
                        cmd.Parameters.AddWithValue("@UpdatedTitle", updatedTitle);
                        cmd.Parameters.AddWithValue("@UpdatedContent", updatedContent);
             

                        int rowsAffected = cmd.ExecuteNonQuery();

                        return rowsAffected > 0; 
                    }
                }
                catch (Exception ex)
                {
                
                    return false;
                }
            }
        }

        protected void ToggleAnnouncementStatus(object sender, EventArgs e)
        {
            string currentStatus = spanAnnStatus.InnerText.Trim();

            string newStatus = (currentStatus == "Published") ? "Unpublished" : "Published";

           
            bool statusUpdated = UpdateAnnouncementStatus(Convert.ToInt32(ViewState["AnnouncementIDViewState"]),newStatus);

            if (statusUpdated)
            {
                
                spanAnnStatus.InnerText = newStatus;
                BindGridViewAnnouncements();
                ShowSweetAlert("Status", "Status Updated.", "success");

                if (spanAnnStatus.InnerText.Equals("Published", StringComparison.OrdinalIgnoreCase))
                {
                    spanAnnStatus.Style["color"] = "green";
                }
                else
                {
                    spanAnnStatus.Style["color"] = "orange";
                }

            }
            else
            {
                ShowSweetAlert("Error", "Failed to update status.", "error");
            }

        }


        private bool UpdateAnnouncementStatus(int annId, string newStatus)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                 
                    string updateQuery = "UPDATE Announcement SET ann_status = @NewStatus, date_posted = GETDATE() WHERE ann_id = @AnnouncementId";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@AnnouncementId", annId);
                        cmd.Parameters.AddWithValue("@NewStatus", newStatus);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
               
                return false;
            }
        }



        protected void btnDelete_Click(object sender, EventArgs e)
        {


            // Assuming annId is the announcement ID you want to delete
            int annIdToDelete = Convert.ToInt32(ViewState["AnnouncementIDViewState"]);


            spanAnnId.InnerText = "";
            ann_title.Text = "";
            ann_content.Text = "";
            btnToggleStatus.Enabled = false;
            announcer.Enabled = false;
            ann_content.Enabled = false;
            ann_title.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            Button5.Enabled = false;
            btnUploadVideo.Enabled = false;


            if (annIdToDelete > 0)
            {
                bool isDeleted = DeleteAnnouncement(annIdToDelete);

                if (isDeleted)
                {
                    // Successfully deleted the announcement, show success message or perform any other actions
                    ShowSweetAlert("Success", "Announcement has been deleted.", "success");

                 
                    BindGridViewAnnouncements();
                }
                else
                {
                    // Failed to delete the announcement, show error message or perform any other actions
                    ShowSweetAlert("Error", "Failed to delete the announcement.", "error");
                }
            }
            else
            {
                // Handle the case where annIdToDelete is not valid (e.g., not a positive integer)
                ShowSweetAlert("Invalid Input", "Invalid announcement ID.", "error");
            }

          
        }

        private bool DeleteAnnouncement(int annId)
        {
            try
            {
                // Delete images associated with ann_id
                DeleteImagesByAnnouncementId(annId);

                // Delete videos associated with ann_id
                DeleteVideosByAnnouncementId(annId);

                // Delete the announcement itself
                string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string deleteQuery = "DELETE FROM Announcement WHERE ann_id = @AnnouncementId";

                    using (SqlCommand cmd = new SqlCommand(deleteQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@AnnouncementId", annId);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        return rowsAffected > 0; 
                    }
                }
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }

        private void DeleteImagesByAnnouncementId(int annId)
        {
            try
            {
                // Your connection string
                string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Define a SQL query to delete images by announcement ID
                    string deleteImagesQuery = "DELETE FROM Image WHERE ann_id = @AnnouncementId";

                    using (SqlCommand cmd = new SqlCommand(deleteImagesQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@AnnouncementId", annId);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
               
            }
        }

        private void DeleteVideosByAnnouncementId(int annId)
        {
            try
            {
                // Your connection string
                string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Define a SQL query to delete videos by announcement ID
                    string deleteVideosQuery = "DELETE FROM Video WHERE ann_id = @AnnouncementId";

                    using (SqlCommand cmd = new SqlCommand(deleteVideosQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@AnnouncementId", annId);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
               
            }
        }
        protected void btnUploadVideo_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowVideoUploadModalScript", "ShowVideoUploadModal();", true);
        }

        protected void btnUploadImage_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModalScript", "$('#imageListModal').modal('show');", true);
        }


    }
}