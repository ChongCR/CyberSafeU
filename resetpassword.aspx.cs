using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class resetpassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if(Request.QueryString["token"] ==null)
            {
                Response.Redirect("userlogin.aspx");
            }
        }

        private bool IsTokenValid(string token)
        {
            bool isValid = false;
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT COUNT(*)
            FROM PasswordResetTokens
            WHERE Token = @Token AND ExpiryDate > GETDATE()";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Token", token);
                    con.Open();
                    int count = (int)cmd.ExecuteScalar();
                    if (count == 1)
                    {
                        isValid = true;
                    }
                }
            }
            return isValid;
        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            string token = Request.QueryString["token"];
            if (IsTokenValid(token))
            {
               
                UpdateUserPassword(txtNewPassword.Text, token);
                string script = @"Swal.fire({
                                title: 'Success!',
                                text: 'Your password has been reset successfully.',
                                icon: 'success',
                                timer: 3000,
                                timerProgressBar: true,
                                didClose: () => window.location.href='userlogin.aspx'
                              });";
                ClientScript.RegisterStartupScript(this.GetType(), "resetSuccess", script, true);
            }
            else
            {
                string script = @"Swal.fire({
                                title: 'Error',
                                text: 'An error occurred while resetting your password. Please try again later.',
                                icon: 'error',
                                confirmButtonText: 'OK'
                              });";
                ClientScript.RegisterStartupScript(this.GetType(), "resetFailed", script, true);
            }
        }


      


        private void UpdateUserPassword(string newPassword, string token)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Get the user_id from the token
                string getUserIdQuery = "SELECT UserID FROM PasswordResetTokens WHERE Token = @Token";
                SqlCommand getUserIdCmd = new SqlCommand(getUserIdQuery, con);
                getUserIdCmd.Parameters.AddWithValue("@Token", token);
                con.Open();
                object result = getUserIdCmd.ExecuteScalar();
                if (result != null)
                {
                    int userId = (int)result;

                    // Update the user's password
                    string updatePasswordQuery = @"
                UPDATE users
                SET password = @NewPassword
                WHERE user_id = @UserID";

                    SqlCommand updatePasswordCmd = new SqlCommand(updatePasswordQuery, con);
                    // Hash the new password before storing
                    updatePasswordCmd.Parameters.AddWithValue("@NewPassword", HashPassword(newPassword));
                    updatePasswordCmd.Parameters.AddWithValue("@UserID", userId);
                    updatePasswordCmd.ExecuteNonQuery();

                    // Optionally, invalidate the token after use
                    string invalidateTokenQuery = "DELETE FROM PasswordResetTokens WHERE Token = @Token";
                    SqlCommand invalidateTokenCmd = new SqlCommand(invalidateTokenQuery, con);
                    invalidateTokenCmd.Parameters.AddWithValue("@Token", token);
                    invalidateTokenCmd.ExecuteNonQuery();
                }
                con.Close();
            }
        }

        protected string HashPassword(string password)
        {


            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
        }
    }
}