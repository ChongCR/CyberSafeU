using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net;

namespace WebApplication1
{
    public partial class forgetpassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            string userEmail = email.Text; // Assuming 'email' is the ID of your input field

            if (!string.IsNullOrEmpty(userEmail))
            {
                if (IsUserEmailInDatabase(userEmail))
                {
                    // Generate a unique token for the password reset link (you may use a library or your own logic)
                    string resetToken = GenerateResetToken();

                    // Update the database with the reset token and expiration time
                    UpdateUserResetToken(userEmail, resetToken);

                    // Send a password reset email
                    SendPasswordResetEmail(userEmail, resetToken);

                    // Replace the following line with your actual SweetAlert code for a successful scenario
                    ScriptManager.RegisterStartupScript(this, GetType(), "successAlert", "swal('Success', 'Password reset email has been sent. Check your inbox.', 'success');", true);
                }
                else
                {
                    // The email does not exist in the database
                    // Replace the following line with your actual SweetAlert code for an unsuccessful scenario
                    ScriptManager.RegisterStartupScript(this, GetType(), "errorAlert", "swal('Error', 'Email is not registered as user OR not found in the database.', 'error');", true);
                }
            }
            else
            {
                // Handle the case where the user did not enter an email
                // Replace the following line with your actual SweetAlert code for handling empty email
                ScriptManager.RegisterStartupScript(this, GetType(), "warningAlert", "swal('Warning', 'Please enter an email address.', 'warning');", true);
            }
        }

        private string GenerateResetToken()
        {
            // Your logic to generate a unique reset token
            // You may use a library like Guid.NewGuid().ToString() or your own logic
            return Guid.NewGuid().ToString();
        }

        private void UpdateUserResetToken(string userEmail, string resetToken)
        {
            // Your logic to update the database with the reset token and expiration time
            // This will depend on your database schema and design
            // Example: UPDATE Users SET ResetToken = @ResetToken, ResetTokenExpiration = @Expiration WHERE Email = @Email
        }

        private void SendPasswordResetEmail(string userEmail, string resetToken)
        {
            // Your logic to send a password reset email using SmtpClient

            // Replace the following with your SMTP server details
            string smtpHost = "your-smtp-host";
            int smtpPort = 587;
            string smtpUsername = "your-smtp-username";
            string smtpPassword = "your-smtp-password";

            using (SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort))
            {
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress("your-email@example.com"); // Replace with your email
                    mailMessage.To.Add(userEmail);
                    mailMessage.Subject = "Password Reset";
                    mailMessage.Body = $"Click the following link to reset your password: http://yourwebsite.com/reset?token={resetToken}";

                    // You can customize the email body and subject based on your requirements

                    smtpClient.Send(mailMessage);
                }
            }
        }

        private bool IsUserEmailInDatabase(string email)
        {
            string query = "SELECT COUNT(*) FROM users WHERE email = @Email";

            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    connection.Close();

                    return count > 0; // If count is greater than 0, the email exists
                }
            }
        }
    }
}