using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BCrypt.Net;




namespace WebApplication1
{


  
    public partial class usermanagement : System.Web.UI.Page
    {
        protected void btnBackDashboard_Click(object sender, EventArgs e)
        {
            // Redirect back to adminhomepage.aspx
            Response.Redirect("adminhomepage.aspx");
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            // Check if the user has admin access during page load
            if (Session["AdminAccess"] == null && Session["SAdminAccess"] == null)
            {           
                Response.Redirect("userlogin.aspx");
            }

            if (!IsPostBack)
            {
                BindUsersGridView();
            }
        }

        protected void btnRefresh_ServerClick(object sender, EventArgs e)
        {
            BindUsersGridView();


            string script = "showSuccessMessage();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccessScript", script, true);

        }
        private void BindUsersGridView()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT user_id, full_name, contact_no, email, role, status FROM users", con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        GridViewUsers.DataSource = dataTable;
                        GridViewUsers.DataBind();
                    }
                }
            }
        }

        protected void GridViewUsers_Sorting(object sender, GridViewSortEventArgs e)
        {
          
            DataTable dataTable = GetGridViewDataSource();

            if (dataTable != null)
            {
               
                DataView dataView = new DataView(dataTable);
                string sortExpression = e.SortExpression;

              
                string currentSortDirection = ViewState["SortDirection"] as string;
                if (currentSortDirection == null || currentSortDirection.Equals("ASC"))
                {
                    dataView.Sort = sortExpression + " DESC";
                    ViewState["SortDirection"] = "DESC";
                }
                else
                {
                    dataView.Sort = sortExpression + " ASC";
                    ViewState["SortDirection"] = "ASC";
                }

               
                GridViewUsers.DataSource = dataView;
                GridViewUsers.DataBind();
            }
        }

        private DataTable GetGridViewDataSource()
        {
            
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT user_id, full_name, contact_no, email, role, status FROM users", con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        return dataTable;
                    }
                }
            }
        }

        protected void bindUserDetails()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM users WHERE user_id = @UserId", con))
                {
                    cmd.Parameters.AddWithValue("@UserId", user_id.Text.ToString());

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user_id.Text = reader["user_id"].ToString();
                            full_name.Text = reader["full_name"].ToString();
                            ddlRole.Text = reader["role"].ToString();
                            if (reader["dob"] != DBNull.Value)
                            {
                                dobDetails.Text = Convert.ToDateTime(reader["dob"]).ToString("yyyy-MM-dd");
                            }
                            else
                            {
                               
                                dobDetails.Text = string.Empty; 
                            }
                      
                            contact_no.Text = reader["contact_no"].ToString();
                            emailDetails.Text = reader["email"].ToString();
                            ddlFaculty.Text = reader["faculty"].ToString();
                            courseDetails.Text = reader["course"].ToString();
                            year_of_study.Text = reader["year_of_study"].ToString();

                            accountStatuslbl.Text = reader["status"].ToString();
                            accountStatuslbl.Text = reader["status"].ToString();
                            string status = accountStatuslbl.Text;

                            if (status == "Active")
                            {
                                accountStatuslbl.ForeColor = System.Drawing.Color.Green;
                            }
                            else if (status == "Inactive")
                            {
                                accountStatuslbl.ForeColor = System.Drawing.Color.Red;
                            }
                            else
                            {                              
                                accountStatuslbl.ForeColor = System.Drawing.Color.Black; 
                            }

                            ShowSweetAlert("Success", "User retrieved successfully.", "success");
                        }
                    }
                }
            }
          
        }

        protected void GridViewUsers_SelectedIndexChanged(object sender, EventArgs e)
        {

            GridViewRow row = GridViewUsers.SelectedRow;
            int selectedUserId = Convert.ToInt32(GridViewUsers.DataKeys[row.RowIndex].Value);

            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM users WHERE user_id = @UserId", con))
                {
                    cmd.Parameters.AddWithValue("@UserId", selectedUserId);

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user_id.Text = reader["user_id"].ToString();
                            full_name.Text = reader["full_name"].ToString();
                            ddlRole.Text = reader["role"].ToString();
                            if (reader["dob"] != DBNull.Value)
                            {
                                dobDetails.Text = Convert.ToDateTime(reader["dob"]).ToString("yyyy-MM-dd");
                            }
                            else
                            {

                                dobDetails.Text = string.Empty;
                            }

                            contact_no.Text = reader["contact_no"].ToString();
                            emailDetails.Text = reader["email"].ToString();
                            ddlFaculty.Text = reader["faculty"].ToString();
                            courseDetails.Text = reader["course"].ToString();
                            year_of_study.Text = reader["year_of_study"].ToString();

                            accountStatuslbl.Text = reader["status"].ToString();
                            accountStatuslbl.Text = reader["status"].ToString();
                            string status = accountStatuslbl.Text;

                            if (status == "Active")
                            {
                                accountStatuslbl.ForeColor = System.Drawing.Color.Green;
                            }
                            else if (status == "Inactive")
                            {
                                accountStatuslbl.ForeColor = System.Drawing.Color.Red;
                            }
                            else
                            {
                                accountStatuslbl.ForeColor = System.Drawing.Color.Black;
                            }

                            ShowSweetAlert("Success", "User retrieved successfully.", "success");
                        }
                    }
                }
            }



        }

        protected void GridViewUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
               
                TableCell statusCell = e.Row.Cells[5]; 

              
                string status = statusCell.Text;

              
                if (status.Equals("Active", StringComparison.OrdinalIgnoreCase))
                {
                    statusCell.ForeColor = System.Drawing.Color.Green;
                }
                else if (status.Equals("Inactive", StringComparison.OrdinalIgnoreCase))
                {
                    statusCell.ForeColor = System.Drawing.Color.Red;
                }
            }
        }



        protected void GridViewUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewUsers.PageIndex = e.NewPageIndex;
            BindUsersGridView(); 
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            BindUsersGridView(txtSearch.Text);
        }

        private void BindUsersGridView(string searchText = "")
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT user_id, full_name, contact_no, email, role, status FROM users WHERE full_name LIKE @SearchText OR email LIKE @SearchText";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@SearchText", $"%{searchText}%");
                    con.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        GridViewUsers.DataSource = dataTable;
                        GridViewUsers.DataBind();
                    }
                }
            }
        }


        protected void btnUpdateUser_Click(object sender, EventArgs e)
        {

        

            if (string.IsNullOrEmpty(user_id.Text))
            {
                ShowSweetAlert("No user selected.", "Please select a user to perform action", "error");
                return;
            }


            // Assume validation functions return a string with an error message or an empty string if validation passes
            string fullNameValidationError = ValidateFullName(full_name.Text);
            if (!string.IsNullOrEmpty(fullNameValidationError))
            {
                ShowSweetAlert("Validation Error", fullNameValidationError, "error");
                return;
            }
            string contactNoValidationError = ValidateContactNo(contact_no.Text);
            if (!string.IsNullOrEmpty(contactNoValidationError))
            {
                ShowSweetAlert("Validation Error", contactNoValidationError, "error");
                return;
            }
            string emailValidationError = ValidateEmail(emailDetails.Text);
            if (!string.IsNullOrEmpty(emailValidationError))
            {
                ShowSweetAlert("Validation Error", emailValidationError, "error");
                return;
            }
            string yearOfStudyValidationError = ValidateYearOfStudy(year_of_study.Text);
            if (!string.IsNullOrEmpty(yearOfStudyValidationError))
            {
                ShowSweetAlert("Validation Error", yearOfStudyValidationError, "error");
                return;
            }

         


            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            int userId = int.Parse(user_id.Text);
            string fullName = full_name.Text;
            string contactNo = contact_no.Text;
            string email = emailDetails.Text;
            string role = ddlRole.SelectedValue; 
            DateTime dob = DateTime.Parse(dobDetails.Text);
            string faculty = ddlFaculty.SelectedValue;
            string course = courseDetails.Text;
            int yearOfStudy = int.Parse(year_of_study.Text);



            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // SQL statement to update user details
                string sql = "UPDATE users SET full_name = @FullName, contact_no = @ContactNo, email = @Email, " +
                             "role = @Role, dob = @DOB, faculty = @Faculty, course = @Course, " +
                             "year_of_study = @YearOfStudy WHERE user_id = @UserId";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    // Add parameters to avoid SQL injection
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@FullName", fullName);
                    cmd.Parameters.AddWithValue("@ContactNo", contactNo);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Role", role);
                    cmd.Parameters.AddWithValue("@DOB", dob);
                    cmd.Parameters.AddWithValue("@Faculty", faculty);
                    cmd.Parameters.AddWithValue("@Course", course);
                    cmd.Parameters.AddWithValue("@YearOfStudy", yearOfStudy);

                  
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                   
                    if (rowsAffected > 0)
                    {
                        ShowSweetAlert("Success", "User details updated successfully.", "success");
                    }
                    else
                    {
                        ShowSweetAlert("Error", "No details were updated.", "error");
                    }
                    con.Close();
                }
            }

            BindUsersGridView();

        }

        private string ValidateFullName(string fullName)
        {
            if (string.IsNullOrEmpty(fullName) || fullName.Any(char.IsDigit))
            {
                return "Full Name cannot contain numbers.";
            }
            return "";
        }

        private string ValidateContactNo(string contactNo)
        {
            if (string.IsNullOrEmpty(contactNo) || !contactNo.All(c => char.IsDigit(c) || c == '+'))
            {
                return "Contact Number is invalid.";
            }
            return "";
        }

        private string ValidateEmail(string email)
        {
            try
            {
                var mail = new System.Net.Mail.MailAddress(email);
                return mail.Address == email ? "" : "Email is invalid.";
            }
            catch
            {
                return "Email is invalid.";
            }
        }

        private string ValidateYearOfStudy(string yearOfStudy)
        {
            if (int.TryParse(yearOfStudy, out int year) && year > 0 && year <= DateTime.Now.Year)
            {
                return "";
            }
            return "Year of Study is invalid.";
        }

        private void ShowSweetAlert(string title, string message, string type)
        {
            string script = $"Swal.fire({{ title: '{title}', text: '{message}', icon: '{type}', confirmButtonText: 'Ok' }});";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "sweetAlert", script, true);
        }

        protected void switchStatusButton_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(user_id.Text))
            {
                ShowSweetAlert("No user selected.", "Please select a user to perform action", "error");
                return;
            }

            int userIdToUpdate = int.Parse(user_id.Text);

            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // SQL statement to update user status based on current status
                string sql = "UPDATE users SET status = CASE WHEN status = 'Active' THEN 'Inactive' ELSE 'Active' END WHERE user_id = @UserId";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", userIdToUpdate);
                    try
                    {
                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // User status was updated successfully
                            ShowSweetAlert("Success", "User status updated successfully.", "success");
                            bindUserDetails();
                        }
                        else
                        {
                            // No user was updated, possibly because the ID wasn't found
                            ShowSweetAlert("Info", "No user found with the provided ID.", "info");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log exception details (ex.Message) and show error message to the user
                        ShowSweetAlert("Error", "An error occurred while updating the user status.", "error");
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }

            // Rebind the users grid view to show changes
            BindUsersGridView();
        }



        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            modalFullNameError.Text = "";
            modalContactNoError.Text = "";
            modalEmailError.Text = "";
            modalEmailError.Text = "";

            // Gather data from input fields
            string fullName = modalFullName.Value;
            string contactNo = modalContactNo.Value;
            string email = modalEmail.Value;
            string hashedPassword = HashPassword(modalPassword.Value);
            string role = modalRole.Value;

            // Perform validation

            bool isValid = true;
            if (string.IsNullOrWhiteSpace(fullName) || fullName.Any(char.IsDigit))
            {

                modalFullNameError.Text = "Full Name cannot contain numbers or be empty.";

                isValid = false;

            }

            if (EmailExists(email))
            {
                modalEmailError.Text = "Email already exists.";
                isValid = false;
            }

            if ( string.IsNullOrWhiteSpace(contactNo) || contactNo.Any(c => !char.IsDigit(c)))
            {
             
                modalContactNoError.Text = "Contact Number is invalid.";
                isValid = false;
            }

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                if (addr.Address != email)
                {
                    modalEmailError.Text = "Email is invalid.";
                    isValid = false;
                }
            }
            catch
            {

                modalEmailError.Text = "Email is invalid.";
                isValid = false;
            }


            if (string.IsNullOrWhiteSpace(modalPassword.Value))
            {
                modalPasswordError.Text = "Password cannot be empty.";
                isValid = false;
            }
            else if (modalPassword.Value.Length < 8)
            {
                modalPasswordError.Text = "Password must be at least 8 characters long.";
                isValid = false;
            }
            else if (!modalPassword.Value.Any(char.IsDigit) || !modalPassword.Value.Any(char.IsLetter))
            {
                modalPasswordError.Text = "Password must contain both letters and numbers.";
                isValid = false;
            }

            if (isValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string sql = "INSERT INTO users (full_name, contact_no, email, password, role, status) VALUES (@FullName, @ContactNo, @Email, @Password, @Role , @Status)";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.Parameters.AddWithValue("@ContactNo", contactNo);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", hashedPassword);
                        cmd.Parameters.AddWithValue("@Role", role);
                        cmd.Parameters.AddWithValue("@Status", "Active");

                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                            ShowSweetAlert("Success", "User created successfully.", "success");                         
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseModal", "$('#createUserModal').modal('hide');", true);
                            BindUsersGridView();

                        }
                        catch (SqlException ex)
                        {
                            ShowSweetAlert("Error", "There was a database error.", "error");
                        }
                    }
                }
            }
        }

        private bool EmailExists(string email)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT COUNT(*) FROM users WHERE email = @Email";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    con.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        protected string HashPassword(string password)
        {

            
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
        }

        protected bool VerifyPassword(string password, string hash)
        {

            return BCrypt.Net.BCrypt.EnhancedVerify(password,hash);        
        }
    }

   


}