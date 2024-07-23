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



    public partial class adminmanagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if the user has super admin access during page load
            if (Session["SAdminAccess"] == null)
            {
                // Redirect to the login page for users without admin access
                Response.Redirect("userlogin.aspx");
            }

            if (!IsPostBack)
            {
                BindAdminGridView();
            }
        }

        protected void btnRefresh_ServerClick(object sender, EventArgs e)
        {
            BindAdminGridView();


            string script = "showSuccessMessage();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccessScript", script, true);

        }

        private void BindAdminGridView()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT user_id, full_name, contact_no, email, status, role FROM users WHERE role = 'Admin'", con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        GridViewAdmin.DataSource = dataTable;
                        GridViewAdmin.DataBind();
                    }
                }
            }
        }

        protected void GridViewAdmin_Sorting(object sender, GridViewSortEventArgs e)
        {

            DataTable dataTable = GetGVDataSource();

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


                GridViewAdmin.DataSource = dataView;
                GridViewAdmin.DataBind();
            }
        }

        private DataTable GetGVDataSource()
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

        protected void bindAdminDetails()
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
                            

                            contact_no.Text = reader["contact_no"].ToString();
                            email.Text = reader["email"].ToString();
                            

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

                            ShowSweetAlert("Success", "Admin retrieved successfully.", "success");
                        }
                    }
                }
            }

        }

        protected void GridViewAdmin_SelectedIndexChanged(object sender, EventArgs e)
        {

            GridViewRow row = GridViewAdmin.SelectedRow;
            int selectedUserId = Convert.ToInt32(GridViewAdmin.DataKeys[row.RowIndex].Value);

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
                            contact_no.Text = reader["contact_no"].ToString();
                            email.Text = reader["email"].ToString();

                            accountStatuslbl.Text = reader["status"].ToString();
                            accountStatuslbl.Text = reader["status"].ToString();
                            string status = accountStatuslbl.Text;

                            //verify if acc is active, else privelege not granted
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

                            ShowSweetAlert("Success", "Admin retrieved successfully.", "success");
                        }
                    }
                }
            }



        }

        protected void GridViewAdmin_RowDataBound(object sender, GridViewRowEventArgs e)
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



        protected void GridViewAdmin_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewAdmin.PageIndex = e.NewPageIndex;
            BindAdminGridView();
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            BindAdminGridView(txtSearch.Text);
        }

        private void BindAdminGridView(string searchText = "")
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
                        GridViewAdmin.DataSource = dataTable;
                        GridViewAdmin.DataBind();
                    }
                }
            }
        }


        protected void btnUpdateAdmin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(user_id.Text))
            {
                ShowSweetAlert("No admin selected.", "Please select an admin to perform the action", "error");
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
            string emailValidationError = ValidateEmail(email.Text);
            if (!string.IsNullOrEmpty(emailValidationError))
            {
                ShowSweetAlert("Validation Error", emailValidationError, "error");
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            int userId = int.Parse(user_id.Text);
            string fullNameValue = full_name.Text;
            string contactNoValue = contact_no.Text;
            string emailValue = email.Text;

            //UPDATE ADMIN DETAILS
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // SQL statement to update admin details
                string sql = "UPDATE users SET full_name = @FullName, contact_no = @ContactNo, email = @Email, " +
                             "role = @Role WHERE user_id = @UserId";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    // Add parameters to avoid SQL injection
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@FullName", fullNameValue);
                    cmd.Parameters.AddWithValue("@ContactNo", contactNoValue);
                    cmd.Parameters.AddWithValue("@Email", emailValue);
                    cmd.Parameters.AddWithValue("@Role", "Admin");

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        ShowSweetAlert("Success", "Admin details updated successfully.", "success");
                    }
                    else
                    {
                        ShowSweetAlert("Error", "Details failed to be updated.", "error");
                    }
                    con.Close();
                }
            }

            BindAdminGridView();
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

                // Check if the email address ends with "tarc.edu.my"
                if (mail.Address == email && email.EndsWith("tarc.edu.my", StringComparison.OrdinalIgnoreCase))
                {
                    return "";
                }
                else
                {
                    return "Email is invalid or does not end with 'tarc.edu.my'.";
                }
            }
            catch
            {
                return "Email is invalid.";
            }
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
                ShowSweetAlert("No admin selected.", "Please select a admin to perform action", "error");
                return;
            }

            int userIdToUpdate = int.Parse(user_id.Text);

            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // SQL statement to update admin status based on current status
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
                            // Admin status was updated successfully
                            ShowSweetAlert("Success", "Admin status updated successfully.", "success");
                            bindAdminDetails();
                        }
                        else
                        {
                            // No admin was updated, possibly because the ID wasn't found
                            ShowSweetAlert("Info", "No admin found with the provided ID.", "info");
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

            // Rebind the admin grid view to show changes
            BindAdminGridView();
        }



        protected void btnCreateAdmin_Click(object sender, EventArgs e)
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

            if (string.IsNullOrWhiteSpace(contactNo) || contactNo.Any(c => !char.IsDigit(c)))
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
                        cmd.Parameters.AddWithValue("@Role", "Admin");
                        cmd.Parameters.AddWithValue("@Status", "Active");

                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                            //refresh gridview after successful creation of admin
                            BindAdminGridView();
                            ShowSweetAlert("Success", "Admin created successfully.", "success");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseModal", "$('#createAdminModal').modal('hide');", true);


                        }
                        catch (SqlException ex)
                        {
                            ShowSweetAlert("Error", "There is a database error.", "error");
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

            return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
        }
    }




}