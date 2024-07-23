using MimeKit;
using MimeKit.Utils;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace WebApplication1
{

    public partial class usersignup : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //sign up button click event
        protected void signup_Click(object sender, EventArgs e)
        {

            using (SqlConnection connection = new SqlConnection(strcon))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM users WHERE email = @email";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@email", email.Text);
                int count = (int)command.ExecuteScalar();

                if (count > 0)
                {
                    emailerror.Text = "The Email already exists";// Show error message that the username or email already exists
                    emailerror.ForeColor = Color.Red;
                }
                else if (string.IsNullOrEmpty(fullname.Text) || string.IsNullOrEmpty(email.Text) || string.IsNullOrEmpty(password.Text) || string.IsNullOrEmpty(confirmpassword.Text))
                {
                    // Show error message that all fields are required
                    passworderror.Text = "All fields are required";
                    passworderror.ForeColor = Color.Red;
                    return;
                }
                else if (password.Text != confirmpassword.Text)
                {
                    confirmpassworderror.Text = "Passwords don't match";// Show error message that the passwords don't match
                    confirmpassworderror.ForeColor = Color.Red;
                }
                else if (!Regex.IsMatch(password.Text, @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$"))
                {
                    passworderror.Text = "Password is too short or doesn't meet the requirements"; // Combine error messages
                    passworderror.ForeColor = Color.Red;

                }
                else if (!Regex.IsMatch(email.Text, @"^[^@\s]+@[^@\s]+\.(tarc\.edu\.my)$"))
                {
                    // Show error message that username, email, and full name cannot contain special characters and email must be valid
                    emailerror.Text = "Email cannot contain special characters and must be valid （Use tarc email to sign up)";
                    emailerror.ForeColor = System.Drawing.Color.Red;


                }
                else if (!Regex.IsMatch(contact.Text.ToString(), @"^(\+?60|0)(1|3|4|5|6|7|9)(\d{7,8})$"))
                {

                    contacterror.Text = $"Invalid phone number: [{contact.Text.Trim()}]";
                    contacterror.ForeColor = System.Drawing.Color.Red;
                }
                else if (!Regex.IsMatch(fullname.Text, "^[a-zA-Z ]*$"))
                {
                    nameerror.Text = "Full name cannot contain special characters ";
                }
                else
                {

                    query = "INSERT INTO users(full_name, dob, contact_no, email, faculty, course, year_of_study, password, role, status) " +
                    "values(@full_name, @dob, @contact_no, @email, @faculty, @course, @year_of_study, @password, @role, @status)";
                    command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@full_name", fullname.Text.Trim());
                    command.Parameters.AddWithValue("@dob", dob.Text.Trim());
                    command.Parameters.AddWithValue("@contact_no", contact.Text.Trim());
                    command.Parameters.AddWithValue("@email", email.Text.Trim());
                    command.Parameters.AddWithValue("@faculty", faculty.SelectedItem.Value);
                    command.Parameters.AddWithValue("@course", course.Text.Trim());
                    command.Parameters.AddWithValue("@year_of_study", yearofstudy.Text.Trim());
                    command.Parameters.AddWithValue("@password", HashPassword(password.Text.Trim()));
                    command.Parameters.AddWithValue("@role", "user");
                    command.Parameters.AddWithValue("@status", "Active");

                    // new parameter for phone number
                    int result = command.ExecuteNonQuery();
                    if (result == 1)
                    {
                        // Show success message and redirect to login page
                        Response.Redirect("userlogin.aspx");
                    }
                    else
                    {
                        signuperror.Text = "unable to sign up"; // Show error message
                        signuperror.ForeColor = Color.Red;
                    }
                }

            }

        }



        //hash pwd
        protected string HashPassword(string password)
        {


            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
        }

        //verify during login
        protected bool VerifyPassword(string password, string hash)
        {

            return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
        }





    }
}