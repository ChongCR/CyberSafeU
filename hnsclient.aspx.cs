using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


namespace WebApplication1
{
    public partial class hnsclient : System.Web.UI.Page
    {

        string strcon = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
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
                BindGridViewHNS();
            }
        }

        protected void btnSubmit_HNS(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(strcon))
            {
                connection.Open();
                string query = "INSERT INTO HelpAndSupport (user_id, inquiry_title, inquiry_content, date_submitted) VALUES (@user_id, @inquiry_title, @inquiry_content, @date_submitted)";

                SqlCommand command = new SqlCommand(query, connection);

                // Add parameters to prevent SQL injection
                command.Parameters.AddWithValue("@inquiry_title", inquiry_title.Text.Trim());
                command.Parameters.AddWithValue("@inquiry_content", inquiry_content.Text.Trim());
                command.Parameters.AddWithValue("@user_id", Session["user_id"]);
                command.Parameters.AddWithValue("@date_submitted", DateTime.Now); // Assuming you want to use the current date and time

                // Perform the insertion
                int result = command.ExecuteNonQuery();

                if (result == 1)
                {
                    // Show success message 
                    ShowSweetAlert("Success", "Inquiry submitted successfully.", "success");
                }
                else
                {
                    // Show error message
                    ShowSweetAlert("Error", "Unable to submit inquiry.", "error");
                }
            }
        }

        private void BindGridViewHNS()
        {
            using (SqlConnection connection = new SqlConnection(strcon))
            {
                connection.Open();
                string query = "SELECT inquiry_id, inquiry_title, response FROM HelpAndSupport";
                SqlCommand command = new SqlCommand(query, connection);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);

                GridViewHNS.DataSource = dataSet.Tables[0];
                GridViewHNS.DataBind();
            }
        }

        private void ShowSweetAlert(string title, string message, string type)
        {
            string script = $"Swal.fire({{ title: '{title}', text: '{message}', icon: '{type}', confirmButtonText: 'Ok' }});";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "sweetAlert", script, true);
        }
    }
}