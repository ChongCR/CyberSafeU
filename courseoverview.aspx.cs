using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class courseoverview : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // Define the connection string
                string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

                // Define the SQL query
                string query = "SELECT CourseID, ImageURL, CourseName, CourseLevel, CourseLanguage, CourseCategory FROM Course WHERE status = 'Approved'";


                // Create a new SqlConnection object with the connection string
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the database connection
                    connection.Open();

                    // Create a new SqlCommand object with the SQL query and the SqlConnection object
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Execute the SQL query and store the results in a SqlDataReader object
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Check if the data reader has any rows
                            if (reader.HasRows)
                            {
                                // Bind data to Repeater control
                                rptCourses.DataSource = reader;
                                rptCourses.DataBind();
                            }

                            reader.Close(); // close the data reader
                        }
                    }

                    connection.Close();
                }
            }
        }

    }
}