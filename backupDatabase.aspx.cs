using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class backupDatabase : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if the user has superadmin access during page load
            if (Session["SAdminAccess"] == null)
            {
                // Redirect to the login page for superadmin without admin access
                Response.Redirect("userlogin.aspx");
            }

            if (!IsPostBack)
            {
                PopulateDatabasesDropDownList();
                BindGrid();
            }
        }

        private void BindGrid()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM DatabaseBackups", con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        GridViewDatabaseBackups.DataSource = dt;
                        GridViewDatabaseBackups.DataBind();
                    }
                }
            }
        }

        private void PopulateDatabasesDropDownList()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Open the connection
                con.Open();

                // Prepare the SQL query to select database names
                string query = "SELECT name FROM sys.databases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb');";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Clear existing items
                        ddlDatabases.Items.Clear();

                        // Read each item and add to the dropdown list
                        while (reader.Read())
                        {
                            string databaseName = reader["name"].ToString();
                            ddlDatabases.Items.Add(databaseName);
                        }
                    }
                }
            }
        }




        protected void btnBackup_Click(object sender, EventArgs e)
        {
          
                string databaseName = ddlDatabases.SelectedValue;
                string backupFilePath = BackupDatabase(databaseName);
                DownloadBackup(backupFilePath);              
                BindGrid();

        }

        protected void btnRefresh_ServerClick(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void ShowSweetAlert(string title, string message, string type)
        {
            string script = $"Swal.fire({{ title: '{title}', text: '{message}', icon: '{type}', confirmButtonText: 'Ok' }});";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "sweetAlert", script, true);
        }



        private string BackupDatabase(string databaseName)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            string backupFolder = Server.MapPath("~/Backups/");
            string backupFileName = String.Format("{0}-{1}.bak", databaseName, DateTime.Now.ToString("yyyyMMddHHmmss"));
            string backupFilePath = System.IO.Path.Combine(backupFolder, backupFileName);

            // Ensure the backup folder exists
            if (!System.IO.Directory.Exists(backupFolder))
            {
                System.IO.Directory.CreateDirectory(backupFolder);
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Safely create the file path, escaping any single quotes that might be used for SQL injection
                backupFilePath = backupFilePath.Replace("'", "''");

                // Construct the BACKUP DATABASE command as a string with the backup file path included
                string query = $"BACKUP DATABASE [{databaseName}] TO DISK='{backupFilePath}'";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            InsertBackupRecord(databaseName);
            return backupFilePath;
        }




        private void DownloadBackup(string filePath)
        {
            System.IO.FileInfo file = new System.IO.FileInfo(filePath);
            if (file.Exists)
            {
                // Clear the current output content from the buffer
                Response.Clear();

                // Add the file name and attachment, which will force the open/save dialog to show, to the header
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

                // Add the file size into the response header
                Response.AddHeader("Content-Length", file.Length.ToString());

                // Set the ContentType
                Response.ContentType = "application/octet-stream";

                // Write the file into the response
                Response.WriteFile(file.FullName);

                // End the response
                Response.End();
            }
            else
            {
                lblMessage.Text = "Error: File not found.";
            }
        }

        protected void btnBackDashboard_Click(object sender, EventArgs e)
        {
            // Redirect back to adminhomepage.aspx
            Response.Redirect("adminhomepage.aspx");

        }
        private void InsertBackupRecord(string databaseName)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            int userId = Convert.ToInt32(Session["user_id"]);

            string query = "INSERT INTO DatabaseBackups (BackupDate, user_id) VALUES (@BackupDate, @UserId)";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@BackupDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    BindGrid();
                    ShowSweetAlert("Success!", "Backup successfully!", "success");

                }
            }
        }


        

    }
}