using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BCrypt.Net;


namespace WebApplication1
{
    public partial class userprofile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            // Check if the user has user access during page load
            if (Session["UserAccess"] == null)
            {
                // Redirect to the login page for users without user access
                Response.Redirect("userlogin.aspx");
            }
            BindData();

            
        }

        private void BindData()
        {
            // Check if the user_id is stored in the session
            if (Session["user_id"] != null)
            {
                // Retrieve the user_id from the session
                string loggedInUserId = Session["user_id"].ToString();

                using (SqlConnection connection = new SqlConnection(strcon))
                {
                    connection.Open();
                    string query = "SELECT * FROM users WHERE user_id = @user_id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Use the user_id from the session
                        command.Parameters.AddWithValue("@user_id", loggedInUserId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader != null && reader.Read())
                            {
                                full_name.Text = GetValueOrDefault(reader, "full_name");

                                if (reader["dob"] != DBNull.Value)
                                {
                                    dob.Text = Convert.ToDateTime(reader["dob"]).ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    dob.Text = string.Empty;
                                }
                                accountRolelbl.Text = reader["role"].ToString();
                                accountRolelbl.Text = reader["role"].ToString();
                                string status = accountRolelbl.Text;

                                contact_no.Text = GetValueOrDefault(reader, "contact_no");
                                email.Text = GetValueOrDefault(reader, "email");
                                faculty.Text = GetValueOrDefault(reader, "faculty");
                                course.Text = GetValueOrDefault(reader, "course");
                                year_of_study.Text = GetValueOrDefault(reader, "year_of_study");
                            }
                        }
                    }
                }
            }
        }


        //if value not empty then fetch value from db
        private string GetValueOrDefault(SqlDataReader reader, string columnName)
        {
            return reader[columnName] != DBNull.Value ? reader[columnName].ToString() : string.Empty;
        }


        string strcon = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

        protected void btnUpdateProf(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(strcon))
            {
                connection.Open();


                // Update the existing user details
                string updateQuery = "UPDATE users SET full_name = @full_name, dob = @dob, contact_no = @contact_no," +
                " faculty = @faculty, course = @course, year_of_study = @year_of_study, " +
                "password = @password  WHERE email = @email";
                SqlCommand updateCommand = new SqlCommand(updateQuery, connection);

                updateCommand.Parameters.AddWithValue("@full_name", full_name.Text.Trim());
                updateCommand.Parameters.AddWithValue("@dob", DateTime.Parse(dob.Text.Trim()));
                updateCommand.Parameters.AddWithValue("@contact_no", contact_no.Text.Trim());
                updateCommand.Parameters.AddWithValue("@email", email.Text.Trim());
                updateCommand.Parameters.AddWithValue("@faculty", faculty.Text.Trim());
                updateCommand.Parameters.AddWithValue("@course", course.Text.Trim());
                updateCommand.Parameters.AddWithValue("@year_of_study", year_of_study.Text.Trim());
                updateCommand.Parameters.AddWithValue("@password", HashPassword(password.Text.Trim()));

                int result = updateCommand.ExecuteNonQuery();

                if (result == 1)
                {
                    // Show success message
                  
                    ShowSweetAlert("Success", "User details updated successfully.", "success");
                    BindData();
                }
                else
                {
                    // Show error message
                   
                    ShowSweetAlert("Error", "Unable to update user details.", "error");
                }
            }
        }
        //hash pwd
        protected string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
        }

        //verify login password
        protected bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
        }

        private void ShowSweetAlert(string title, string message, string type)
        {
            string script = $"Swal.fire({{ title: '{title}', text: '{message}', icon: '{type}', confirmButtonText: 'Ok' }});";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "sweetAlert", script, true);
        }

    }



}
