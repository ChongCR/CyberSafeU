using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Drawing2D;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text.RegularExpressions;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace WebApplication1
{
    public partial class contentapproval : System.Web.UI.Page
    {

        protected void btnBackDashboard_Click(object sender, EventArgs e)
        {
            // Redirect back to adminhomepage.aspx
            Response.Redirect("adminhomepage.aspx");

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if the user has super admin access during page load
            if (Session["SAdminAccess"] == null)
            {
                // Redirect to the login page for users without admin access
                Response.Redirect("userlogin.aspx");
            }

            BindGridViewRefMaterial();
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


        protected void btnUploadImage_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModalScript", "$('#imageListModal').modal('show');", true);
        }

        protected void btnUploadVideo_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowVideoUploadModalScript", "ShowVideoUploadModal();", true);
        }

        protected void btnRefresh_ServerClick(object sender, EventArgs e)
        {
          
            BindGridViewRefMaterial();
            ShowSweetAlert("Refreshed", "The data has been refreshed.", "success");
        }

        protected void btnShowCreateModal_Click(object sender, EventArgs e)
        {
         
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowCreateMaterialModal", "$('#createMaterialModal').modal('show');", true);
        }

        private void BindGridViewRefMaterialWithSearch(string searchText)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
      
            string query = "SELECT content_id, content_title, language, author_name, content_cat, content_type, content_desc, date_posted, status FROM Ref_Material WHERE content_title LIKE @SearchText OR author_name LIKE @SearchText";

            DataTable dataTable = new DataTable();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
               
                    cmd.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            con.Open();
                            adapter.Fill(dataTable);
                        }
                        catch (Exception ex)
                        {
                         
                            Debug.WriteLine("An error occurred while searching: " + ex.Message);
                        
                        }
                        finally
                        {
                            con.Close();
                        }
                    }
                }
            }

          
            GridViewRefMaterial.DataSource = dataTable;
            GridViewRefMaterial.DataBind();
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
         
            string searchText = txtSearch.Text.Trim();

            
            BindGridViewRefMaterialWithSearch(searchText);
        }

        private void BindGridViewRefMaterial()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            string query = "SELECT content_id, content_title, language, author_name, content_cat, content_type, content_desc, date_posted, status FROM Ref_Material";

            DataTable dataTable = new DataTable();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
                catch (Exception ex)
                {
              
                    Debug.WriteLine("An error occurred while binding the GridView: " + ex.Message);
                    ShowSweetAlert("Error", "An error occurred while refreshing the data.", "error");
                }
                finally
                {
                    con.Close();
                }
            }
            GridViewRefMaterial.DataSource = dataTable;
            GridViewRefMaterial.DataBind();
        }

        protected void GridViewRefMaterial_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                int statusColumnIndex = e.Row.Cells.Count - 2;

                string status = e.Row.Cells[statusColumnIndex].Text;


                if (string.Equals(status, "Pending", StringComparison.OrdinalIgnoreCase))
                {
                    e.Row.Cells[statusColumnIndex].ForeColor = System.Drawing.Color.Orange;
                }
                else if (string.Equals(status, "Approved", StringComparison.OrdinalIgnoreCase))
                {
                    e.Row.Cells[statusColumnIndex].ForeColor = System.Drawing.Color.Green;
                }
                else if (string.Equals(status, "Rejected", StringComparison.OrdinalIgnoreCase))
                {
                    e.Row.Cells[statusColumnIndex].ForeColor = System.Drawing.Color.Red;
                }
            }
        }


        protected void btnApprove_Click(object sender, EventArgs e)
        {
            int contentId = Convert.ToInt32(ViewState["ContentIDViewstate"]);
            ApproveContent(contentId);
            BindGridViewRefMaterial();
            spanStatus.InnerText = "Approved";
            ShowSweetAlert("Approved!", "Content Approved!", "success");
            spanStatus.Style.Add("color", "green");
         

        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            int contentId = Convert.ToInt32(ViewState["ContentIDViewstate"]);
            RejectContent(contentId);
            BindGridViewRefMaterial();
            spanStatus.InnerText = "Rejected";
            ShowSweetAlert("Rejected!", "Content rejected!", "success");
            spanStatus.Style.Add("color", "red");
        }

        protected void btnPending_Click(object sender, EventArgs e)
        {
            int contentId = Convert.ToInt32(ViewState["ContentIDViewstate"]);
            SetContentPending(contentId);
            BindGridViewRefMaterial();
            spanStatus.InnerText = "Pending";
            ShowSweetAlert("Success", "Content status updated!", "success");
            spanStatus.Style.Add("color", "orange");
        }


        protected void ApproveContent(int contentId)
        {
            UpdateContentStatus(contentId, "Approved");
        }

        protected void RejectContent(int contentId)
        {
            UpdateContentStatus(contentId, "Rejected");
        }

        protected void SetContentPending(int contentId)
        {
            UpdateContentStatus(contentId, "Pending");
        }

        private void UpdateContentStatus(int contentId, string newStatus)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "UPDATE Ref_Material SET status = @Status WHERE content_id = @ContentID";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Status", newStatus);
                    cmd.Parameters.AddWithValue("@ContentID", contentId);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                      
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }



        protected void GridViewRefMaterial_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Fetch the current data to be sorted.
            DataTable dataTable = GetData();

            if (dataTable != null)
            {
                // Determine the sort direction.
                string sortDirection = GetSortDirection(e.SortExpression);

                // Use a DataView to sort the DataTable.
                dataTable.DefaultView.Sort = e.SortExpression + " " + sortDirection;

                // Bind the sorted data to the GridView.
                GridViewRefMaterial.DataSource = dataTable;
                GridViewRefMaterial.DataBind();
            }
        }

        private string GetSortDirection(string column)
        {
            
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
               
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

          
            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }


        protected void GridViewRefMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
 

            int index = GridViewRefMaterial.SelectedIndex;

            int contentId = Convert.ToInt32(GridViewRefMaterial.DataKeys[index].Value);


            ViewState["ContentIDViewstate"] = contentId;
            DataTable dt = GetDataById(contentId);


            LoadImages(contentId);
            BindGridViewVideos(contentId);



            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];


                spanContentId.InnerText = row["content_id"].ToString();
                spanContentTitle.InnerText = row["content_title"].ToString();
                spanLanguage.InnerText = row["language"].ToString();
                spanAuthorName.InnerText = row["author_name"].ToString();
                spanCategory.InnerText = row["content_cat"].ToString();
                spanType.InnerText = row["content_type"].ToString();
                spanDescription.InnerText = row["content_desc"].ToString();
                spanStatus.InnerText = row["status"].ToString();

                switch (spanStatus.InnerText)
                {
                    case "Approved":
                        spanStatus.Style.Add("color", "green");
                        break;
                    case "Pending":
                        spanStatus.Style.Add("color", "orange");
                        break;
                    case "Rejected":
                        spanStatus.Style.Add("color", "red");
                        break;
                    default:
                        spanStatus.Style.Add("color", "black"); // Default color
                        break;
                }


                Button1.Enabled = true;
                btnUploadVideo.Enabled = true;
                ShowSweetAlert("Success", "Material retrieved successfully.", "success");

            }
        }


        private void BindGridViewVideos(int contentId)
        {


            DataTable dtVideo = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT video_id, VideoURL, UploadedDate FROM Video where content_id = @contentId";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@contentId", contentId);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtVideo);
                    }
                }
            }


           GridViewVideos.DataSource = dtVideo;
           GridViewVideos.DataBind();
        }


        private bool DeleteVideoFromDatabase(int videoId)
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();


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

                return false;
            }
        }
        protected void GridViewImages_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewImage")
            {
                string imageUrl = (string)e.CommandArgument;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowImageModalScript", $"showImageModal('{imageUrl}');", true);
            }

            if (e.CommandName == "DeleteImage")
            {


                int imageId = Convert.ToInt32(e.CommandArgument);

                DeleteImageById(imageId);

                ShowSweetAlert("Success", "Deleted successfully.", "success");

                LoadImages(Convert.ToInt32(ViewState["ContentIDViewstate"]));

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModalScript", "$('#imageListModal').modal('show');", true);

            }
        }


        private void DeleteImageById(int imageId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "DELETE FROM Image WHERE Image_id = @ImageId";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ImageId", imageId);
                    conn.Open();

                    // Start a local transaction.
                    SqlTransaction sqlTran = conn.BeginTransaction();
                    cmd.Transaction = sqlTran;

                    try
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Commit the transaction.
                        sqlTran.Commit();

                        if (rowsAffected == 0)
                        {
                            // Log or handle the situation where no record was deleted.
                            Console.WriteLine("No record with the Image ID was found.");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Attempt to roll back the transaction.
                        try
                        {
                            sqlTran.Rollback();
                        }
                        catch (Exception exRollback)
                        {
                            // Handle any errors that may have occurred on the server that would cause the rollback to fail.
                            Console.WriteLine("Rollback Exception Type: {0}", exRollback.GetType());
                            Console.WriteLine("  Message: {0}", exRollback.Message);
                        }

                        // Log the exception
                        Console.WriteLine("Exception Type: {0}", ex.GetType());
                        Console.WriteLine("  Message: {0}", ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        protected void btnUploadImageModal_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {


                // Validate the file type (make sure it's an image)
                string[] validFileTypes = { "jpeg", "png", "jpg" };
                string ext = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName).ToLower();

                bool isValidFile = validFileTypes.Any(item => ext.EndsWith(item));

                if (!isValidFile)
                {
                    ShowSweetAlert("Invalid", "Please select an Image file!", "error");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModalScript", "$('#imageListModal').modal('show');", true);
                }
                else
                {
                    try
                    {
                        string fileName = Path.GetFileName(FileUpload1.FileName);
                        string imagePath = Path.Combine(Server.MapPath("~/images/"), fileName);


                        FileUpload1.SaveAs(imagePath);


                        string imgurUrl = UploadToImgur(imagePath);


                        int contentId = Convert.ToInt32(ViewState["ContentIDViewstate"]);


                        InsertImageIntoDatabase(imgurUrl, DateTime.Now, contentId);


                        FileUpload1.Dispose();


                        LoadImages(contentId);

                        ShowSweetAlert("Success", "Image Uploaded Suscessfully.", "success");


                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModalScript", "$('#imageListModal').modal('show');", true);

                    }
                    catch (Exception ex)
                    {

                        Debug.WriteLine("An error occurred: " + ex.Message);

                    }
                }

            }
            else
            {
                ShowSweetAlert("Where the image?", "No image detected..", "question");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModalScript", "$('#imageListModal').modal('show');", true);
            }
        }


        private void InsertImageIntoDatabase(string imageUrl, DateTime uploadedDate, int contentId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO Image (ImageURL, UploadedDate, content_id) VALUES (@ImageURL, @UploadedDate, @content_id)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ImageURL", imageUrl);
                    cmd.Parameters.AddWithValue("@UploadedDate", uploadedDate);
                    cmd.Parameters.AddWithValue("@content_id", contentId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        protected void btnAddVideoLink_Click(object sender, EventArgs e)
        {

            int selectedContentID = Convert.ToInt32( ViewState["ContentIDViewstate"]);


            string videoLink = txtVideoLink.Value.ToString();


            if (selectedContentID > 0 && !string.IsNullOrEmpty(videoLink))
            {

                string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();


                    string insertVideoQuery = "INSERT INTO Video (VideoURL, UploadedDate, content_id) VALUES (@VideoURL, @UploadedDate, @ContentID)";

                    using (SqlCommand cmd = new SqlCommand(insertVideoQuery, connection))
                    {

                        cmd.Parameters.AddWithValue("@VideoURL", videoLink);
                        cmd.Parameters.AddWithValue("@UploadedDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@ContentID", selectedContentID);


                        cmd.ExecuteNonQuery();
                    }


                    connection.Close();


                    ShowSweetAlert("Success", "Video link has been added.", "success");

                    BindGridViewVideos(selectedContentID);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowVideoUploadModalScript", "ShowVideoUploadModal();", true);



                }
            }
            else
            {

                ShowSweetAlert("HUH?", "Where is the video link?", "question");
            }


        }

        protected void GridViewVideos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteVideo")
            {
                int videoIdToDelete = Convert.ToInt32(e.CommandArgument);


                bool isDeleted = DeleteVideoFromDatabase(videoIdToDelete);

                if (isDeleted)
                {
                    ShowSweetAlert("Success", "Video link has been deleted.", "success");
                }
                else
                {
                    ShowSweetAlert("Opps?", "Error..? Cant delete it.", "error");
                }


                int contentId = Convert.ToInt32(spanContentId.InnerText);
                BindGridViewVideos(contentId);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowVideoUploadModalScript", "ShowVideoUploadModal();", true);
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


           GridViewImages.DataSource = dtImages;
           GridViewImages.DataBind();
        }



        private bool ContainsInteger(string input)
        {
            foreach (char c in input)
            {
                if (char.IsDigit(c))
                {
                    return true;
                }
            }
            return false;
        }


        protected void btnCreateMaterial_Click(object sender, EventArgs e)
        {
            try
            {
                // Clear previous error messages
                lblErrorContentTitle.Text = "";
                lblErrorAuthorName.Text = "";

                // Get values from the form controls
                string contentTitle = txtNewContentTitle2.Text.Trim(); // Trim to remove leading/trailing spaces
                string language = ddlNewLanguage2.SelectedValue;
                string category = ddlNewCategory2.SelectedValue;
                string itemType = ddlNewItemtype2.SelectedValue;
                string authorName = txtNewAuthorName2.Text.Trim(); // Trim to remove leading/trailing spaces
                string contentDescription = txtNewContentDescription2.Text;

                string specialCharacterPattern = "[!@#$%^&*()_+}{\":?><]";


                if (Regex.IsMatch(contentTitle, specialCharacterPattern))
                {
                    lblErrorContentTitle.Text = "Content title cannot contain special characters like !*@&#*(!@.";
                    return;
                }
                // Perform data validation
                if (string.IsNullOrEmpty(contentTitle))
                {
                    lblErrorContentTitle.Text = "Content Title cannot be empty.";
                    return;
                }

                if (string.IsNullOrEmpty(authorName))
                {
                    lblErrorAuthorName.Text = "Author Name cannot be empty.";
                    return;
                }

                if (ContainsInteger(authorName) || Regex.IsMatch(authorName, specialCharacterPattern))
                {
                    lblErrorAuthorName.Text = "Author Name cannot contain special characters.";
                    return;
                }

                // Define your connection string (replace with your actual database connection string)
                string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Define the SQL query for inserting data
                    string insertQuery = "INSERT INTO Ref_Material (content_title, language, content_cat, content_type, author_name, content_desc, date_posted, status) " +
                                         "VALUES (@ContentTitle, @Language, @Category, @ItemType, @AuthorName, @ContentDescription, GETDATE(), 'Pending')";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                    {
                        // Add parameters to the SQL command
                        cmd.Parameters.AddWithValue("@ContentTitle", contentTitle);
                        cmd.Parameters.AddWithValue("@Language", language);
                        cmd.Parameters.AddWithValue("@Category", category);
                        cmd.Parameters.AddWithValue("@ItemType", itemType);
                        cmd.Parameters.AddWithValue("@AuthorName", authorName);
                        cmd.Parameters.AddWithValue("@ContentDescription", contentDescription);


                        connection.Open();


                        cmd.ExecuteNonQuery();


                        connection.Close();
                    }
                }


                txtNewContentTitle2.Text = "";
                ddlNewLanguage2.SelectedIndex = 0;
                ddlNewCategory2.SelectedIndex = 0;
                ddlNewItemtype2.SelectedIndex = 0;
                txtNewAuthorName2.Text = "";
                txtNewContentDescription2.Text = "";
                BindGridViewRefMaterial();

                ShowSweetAlert("Success", "Material created successfully.", "success");


            }
            catch (Exception ex)
            {

                ShowSweetAlert("Error", "An error occurred while creating material.", "error");
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


        private DataTable GetData()
        {
         
            DataTable dataTable = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            string query = "SELECT content_id, content_title, language, author_name, content_cat, content_type, content_desc, date_posted, status FROM Ref_Material";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            con.Open();
                            adapter.Fill(dataTable);
                        }
                        catch (Exception ex)
                        {
                          
                            Debug.WriteLine("An error occurred: " + ex.Message);
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

    

        private void ShowSweetAlert(string title, string message, string type)
        {
            string script = $"Swal.fire({{ title: '{title}', text: '{message}', icon: '{type}', confirmButtonText: 'Ok' }});";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "sweetAlert", script, true);
        }

        protected void GridViewRefMaterial_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
           
            GridViewRefMaterial.PageIndex = e.NewPageIndex;

         
            BindGridViewRefMaterial();
        }


    }
}