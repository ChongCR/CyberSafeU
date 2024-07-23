using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using MimeKit;
using MimeKit.Utils;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class userlogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                Session["LockedOut"] = false;
                multiView.SetActiveView(view1);
            }

        }


        private void SendVerificationCodeToEmail(string recipientEmail, string verificationCode)
        {

            Session["VerificationCode"] = verificationCode;
            // SMTP Credential
            string smtpServer = "smtp-relay.brevo.com";
            int smtpPort = 587;
            string smtpUsername = "chongcr128@gmail.com";
            string smtpPassword = "9g1d0UOFWb8rxvmN";

            // CyberSafeU's logo
            string companyLogoPath = Server.MapPath("~/imgs/cybersafeulogo.png");



            // Create an email message
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("CyberSafeU", smtpUsername));
            message.To.Add(new MailboxAddress("Recipient Name", recipientEmail));
            message.Subject = "[CyberSafeU] Two-Factor Authentication Verification Code";

            var builder = new BodyBuilder();

            // Insert Logo Images to the mail
            var logoImage = builder.LinkedResources.Add(companyLogoPath);
            logoImage.ContentId = MimeUtils.GenerateMessageId();
            builder.HtmlBody = $@"<html>
                <head>
                    <style>
                        body {{
                            text-align: center;
                            background-color: #f8f9fa;
                        }}
                        .container {{
                            display: inline-block;
                            padding: 20px;
                        }}
                        img {{
                            max-width: 200px;
                        }}
                    </style>
                </head>
                <body>
                    <div class=""container"">
                        <img src=""cid:{logoImage.ContentId}"" alt=""Company Logo"">
                        <p>Dear user,</p>
                        <p>Thank you for using CyberSafeU.</p>
                        <p>Your two-factor authentication verification code is:</p>
                        <p><b>{verificationCode}</b></p>
                        <p>If this request does not come from you, please change your account password immediately to prevent others from logging in without permission.</p>
                    </div>
                </body>
            </html>";

            message.Body = builder.ToMessageBody();

            try
            {
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    // Connect to the SMTP server
                    client.ServerCertificateValidationCallback = (s, c, h, ex) => true; // Accept any certificate (for testing purposes)
                    client.Connect(smtpServer, smtpPort, false);

                    // Authenticate 
                    client.Authenticate(smtpUsername, smtpPassword);

                    // Send the email message
                    client.Send(message);

                    // Disconnect from the server
                    client.Disconnect(true);
                }

                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }
        }


        private void SendPasswordResetLink(string recipientEmail, string token)
        {

            string smtpServer = "smtp-relay.brevo.com";
            int smtpPort = 587;
            string smtpUsername = "chongcr128@gmail.com";
            string smtpPassword = "9g1d0UOFWb8rxvmN";

            //LINK UPDATE
            string resetLink = "https://localhost:44343/resetpassword.aspx?token=" + HttpUtility.UrlEncode(token);
            string companyLogoPath = Server.MapPath("~/imgs/cybersafeulogo.png");

            // Create an email message
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("CyberSafeU", smtpUsername));
            message.To.Add(new MailboxAddress("Recipient Name", recipientEmail));
            message.Subject = "CyberSafeU Password Reset Request";

            var builder = new BodyBuilder();

            // Insert Logo Images to the mail
            var logoImage = builder.LinkedResources.Add(companyLogoPath);
            logoImage.ContentId = MimeUtils.GenerateMessageId();
            builder.HtmlBody = $@"<html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    text-align: center;
                    background-color: #f8f9fa;
                    color: #333;
                }}
                .container {{
                    display: inline-block;
                    padding: 20px;
                    background-color: #fff;
                    border-radius: 10px;
                    box-shadow: 0 0 10px rgba(0,0,0,0.1);
                }}
                img {{
                    max-width: 200px;
                }}
                .button {{
                    display: inline-block;
                    padding: 10px 20px;
                    margin-top: 20px;
                    background-color: #007bff;
                    color: #fff;
                    text-decoration: none;
                    border-radius: 5px;
                }}
                .button:hover {{
                    background-color: #0056b3;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <img src='cid:{logoImage.ContentId}' alt='Company Logo'>
                <h2>Password Reset Request</h2>
                <p>Hello,</p>
                <p>We received a request to reset the password for your CyberSafeU account.</p>
                <p>Please click the button below to set a new password:</p>
                <a href='{resetLink}' class='button'>Reset Password</a>
                <p>If you did not request a password reset, please ignore this email.</p>
                <p>Thank you,<br>CyberSafeU Team</p>
            </div>
        </body>
    </html>";

            message.Body = builder.ToMessageBody();
            try
            {
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    // Connect to the SMTP server
                    client.ServerCertificateValidationCallback = (s, c, h, ex) => true; // Accept any certificate (for testing purposes)
                    client.Connect(smtpServer, smtpPort, false);

                    // Authenticate 
                    client.Authenticate(smtpUsername, smtpPassword);

                    // Send the email message
                    client.Send(message);

                    // Disconnect from the server
                    client.Disconnect(true);
                }

                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }
        }


        private bool SaveToken(string email, string token)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // First, get the user_id corresponding to the email
                string getUserIdQuery = "SELECT user_id FROM users WHERE email = @Email";
                SqlCommand getUserIdCmd = new SqlCommand(getUserIdQuery, con);
                getUserIdCmd.Parameters.AddWithValue("@Email", email);

                con.Open();
                object result = getUserIdCmd.ExecuteScalar();
                if (result == null)
                {
                    // Email not found in the users table
                    return false;
                }

                int userId = Convert.ToInt32(result);

                // Now, save the token with the user_id and an expiry date (e.g., 24 hours from now)
                string saveTokenQuery = @"
            INSERT INTO PasswordResetTokens (UserID, Token, ExpiryDate)
            VALUES (@UserID, @Token, @ExpiryDate)";

                SqlCommand saveTokenCmd = new SqlCommand(saveTokenQuery, con);
                saveTokenCmd.Parameters.AddWithValue("@UserID", userId);
                saveTokenCmd.Parameters.AddWithValue("@Token", token);
                saveTokenCmd.Parameters.AddWithValue("@ExpiryDate", DateTime.UtcNow.AddHours(24)); // Setting expiry to 24 hours

                int rowsAffected = saveTokenCmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }


        protected void btnSendResetLink_Click(object sender, EventArgs e)
        {
            string recipientEmail = txtEmailForReset.Text;

            if (IsEmailExists(recipientEmail))
            {
                string token = GenerateToken();
                if (SaveToken(recipientEmail, token))
                {
                    SendPasswordResetLink(recipientEmail, token);

                    // Display SweetAlert message and redirect
                    string script = @"
                Swal.fire({
                    title: 'Email Sent!',
                    text: 'A password reset email has been sent to your address. Please check your inbox.',
                    icon: 'success',
                    timer: 5000,
                    timerProgressBar: true,
                    willClose: () => {
                        window.location.href = 'userlogin.aspx';
                    }
                });
            ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ResetEmailSent", script, true);
                }
                else
                {
                    // Handle the case where token saving fails
                }
            }
            else
            {
                // Even if the email does not exist, show a generic message for security reasons
                string script = @"
            Swal.fire({
                title: 'Email Sent!',
                text: 'If your email address exists in our system, a password reset email will be sent.',
                icon: 'info',
                timer: 5000,
                timerProgressBar: true
            });
        ";
                ScriptManager.RegisterStartupScript(this, GetType(), "ResetEmailSentGeneric", script, true);
            }
        }


        private string GenerateToken()
        {
            return Guid.NewGuid().ToString();
        }

        private bool IsEmailExists(string email)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Query to check if the email exists
                string query = "SELECT COUNT(*) FROM users WHERE email = @Email";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // Replace @Email with the actual email address
                    cmd.Parameters.AddWithValue("@Email", email);

                    con.Open();

                    // Execute the query and convert the result to an integer
                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    // If count is more than 0, the email exists
                    return count > 0;
                }
            }
        }


        private static string GenerateVerificationCode(int length)
        {
            const string validChars = "0123456789";
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[length];
                crypto.GetBytes(data);
                StringBuilder result = new StringBuilder(length);
                foreach (byte b in data)
                {
                    result.Append(validChars[b % validChars.Length]);
                }
                return result.ToString();
            }
        }

        protected void lnkForgotPassword_Click(object sender, EventArgs e)
        {
            multiView.SetActiveView(view3);
        }







        protected void btnBackFromForgot_Click(object sender, EventArgs e)
        {
            multiView.SetActiveView(view1);
        }



        protected void btnVerify_Click(object sender, EventArgs e)
        {
            string script;
            if (VerifyCode(txtVerificationCode.Text))
            {
                // The verification code is correct
                script = "Swal.fire({" +
                         "title: 'Success'," +
                         "text: 'Verification successful! Redirecting to homepage...'," +
                         "icon: 'success'," +
                         "showConfirmButton: false," +
                         "timer: 3000," +
                         "timerProgressBar: true," +
                         "didClose: () => window.location.href='homepage.aspx'" +
                         "});";
            }
            else
            {
                // The verification code is incorrect
                script = "Swal.fire({" +
                         "title: 'Error'," +
                         "text: 'Incorrect verification code. Please try again.'," +
                         "icon: 'error'," +
                         "showConfirmButton: false," +
                         // Removed the "Resend" button logic since you no longer want the redirect
                         "timer: 2000," + // Keep the alert for 5 seconds so the user can read it
                         "});";
            }

            // Register the script to run on the client side
            ScriptManager.RegisterStartupScript(this, GetType(), "VerificationAlert", script, true);
        }

        protected void btnResendCode_Click(object sender, EventArgs e)
        {
            if (Session["VerificationCode"] == null)
            {
                Response.Redirect("userlogin.aspx");
            }

            // Logic to generate and send a new verification code
            string newVerificationCode = GenerateVerificationCode(6); // Implement this method based on your requirements
            Session["VerificationCode"] = newVerificationCode; // Store the new code in the session


            SendVerificationCodeToEmail(email.Text, newVerificationCode);

            string script = $@"
        Swal.fire({{
            title: 'Sent!',
            text: 'A new verification code has been sent.',
            icon: 'success',
            timer: 2000
        }});
        var btnResend = document.getElementById('{btnResendCode.ClientID}');
        btnResend.disabled = true;
        setTimeout(function() {{
            btnResend.disabled = false;
        }}, 60000); // Disable for 60 seconds
    ";

            ClientScript.RegisterStartupScript(this.GetType(), "ResendCode", script, true);

        }


        private bool VerifyCode(string userInputCode)
        {
            // Retrieve the verification code from the session
            string sessionCode = Session["VerificationCode"] as string;

            // Check if the session still contains the verification code
            if (sessionCode == null)
            {
                // Session expired or verification code not found
                return false;
            }

            // Compare the user input against the session code
            if (userInputCode.Equals(sessionCode))
            {
                // Optionally clear the verification code from the session
                Session.Remove("VerificationCode");
                return true; // Verification succeeded
            }
            else
            {
                return false; // Verification failed
            }
        }



        protected void btnBackToLogin_Click(object sender, EventArgs e)
        {
            // Switch back to the login view..
            Session.Remove("VerificationCode");
            multiView.SetActiveView(view1);
        }


        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (Session["LockedOut"] != null && (bool)Session["LockedOut"])
            {
                // Show error message indicating that the user is locked out

                ShowSweetAlert("Login Error", "Too many failed attempts. Please try again after 30 seconds.", "error");
                return;
            }
            else
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    //check db column names later
                    string secretpassword = password.Text.ToString();



                    string query = "SELECT user_id, role, password, status FROM users WHERE email = @email";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@email", email.Text);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string hashedpassword = reader["password"].ToString();
                                string status = reader["status"].ToString();
                                Session["user_id"] = reader["user_id"];
                                if (BCrypt.Net.BCrypt.EnhancedVerify(secretpassword, hashedpassword))
                                {
                                    if (status == "Active")
                                    {
                                        string role = reader["role"].ToString();
                                        Session["user_id"] = reader["user_id"].ToString(); // store the user ID in a session variable

                                        // Check roles and send them to respective homepages
                                        Session["LoginAttempts"] = 0;

                                        if (role == "Admin")
                                        {
                                            Session["AdminAccess"] = email.Text; // Set specific session variable for Admin

                                            Global.LogUserActivity(Session["user_id"].ToString(), role, "Logged in.");

                                            Response.Redirect("adminhomepage.aspx");
                                        }
                                        else if (role == "sadmin")
                                        {
                                            Session["SAdminAccess"] = email.Text; // Set specific session variable for Super Admin

                                            Global.LogUserActivity(Session["user_id"].ToString(), role, "Logged in.");

                                            Response.Redirect("sadminhomepage.aspx");
                                        }
                                        else if (role == "user" || role == "Staff" || role == "Student")
                                        {
                                            Session["UserAccess"] = email.Text; // Set specific session variable for other roles

                                            Global.LogUserActivity(Session["user_id"].ToString(), role, "Logged in.");   
                                            Response.Redirect("homepage.aspx");

                                        }
                                    }
                                    else if (status == "Inactive")
                                    {
                                        string role = reader["role"].ToString();
                                        // Show SweetAlert for inactive account
                                        Global.LogUserActivity(Session["user_id"].ToString(), role, $"Inactive Account Log in attempt. Status: {status}, Role: {role}");

                                        ShowSweetAlert("Login Error", "Your account is inactive. Please contact support.", "error");
                                    }

                                    if (Session["VerificationCode"] == null)
                                    {
                                        // Generate and send a verification code
                                        string verificationCode = GenerateVerificationCode(6);
                                        Session["emailAddressCode"] = email.Text;
                                        SendVerificationCodeToEmail(email.Text, verificationCode);
                                        multiView.SetActiveView(view2);
                                    }
                                }
                                else
                                {
                                    // Show error message if the login failed
                                    ShowSweetAlert("Login Error", "Invalid username or password", "error");
                                    // Auth fail
                                    if (Session["LoginAttempts"] != null)
                                    {
                                        int loginAttempts = (int)Session["LoginAttempts"] + 1;
                                        Session["LoginAttempts"] = loginAttempts;

                                        if (loginAttempts >= 3)
                                        {
                                            // Prevent further attempt from the user for 30 seconds

                                            string role = reader["role"].ToString();
                                            Global.LogUserActivity(Session["user_id"].ToString(), role, "Too many login attempts.");

                                            // Show error message if the login failed
                                            ShowSweetAlert("Login Locked", "Too many attempts, please try again after 30 seconds", "error");
                                            Session["LockedOut"] = true;
                                            Timer1.Enabled = true; // enable the timer to unlock the user after 30 seconds
                                        }
                                    }
                                    else
                                    {
                                        Session["LoginAttempts"] = 1;
                                    }
                                }
                            }
                        }
                    }










                }
            }

        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            // Unlock the user after 30 seconds
            Session["LockedOut"] = false;
            Timer1.Enabled = false;
        }

        private void ShowSweetAlert(string title, string message, string type)
        {
            string script = $"Swal.fire({{ title: '{title}', text: '{message}', icon: '{type}', confirmButtonText: 'Ok' }});";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "sweetAlert", script, true);
        }

        protected bool VerifyPassword(string password, string hash)
        {

            return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
        }
    }
}