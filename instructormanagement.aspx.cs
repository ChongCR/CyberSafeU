using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class instructormanagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["AdminAccess"] == null && Session["SAdminAccess"] == null)
            {
                Response.Redirect("userlogin.aspx");
            }
            if (!IsPostBack)
            {
                BindInstructorGridView();
            }
        }
        protected void btnBackDashboard_Click(object sender, EventArgs e)
        {
            // Redirect back to adminhomepage.aspx
            Response.Redirect("adminhomepage.aspx");
        }

        protected void btnRefresh_ServerClick(object sender, EventArgs e)
        {
           BindInstructorGridView();


            string script = "showSuccessMessage();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccessScript", script, true);
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            BindInstructorGridView(txtSearch.Text);
        }

    

        protected void GridViewInstructors_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Retrieve the row index of the selected row
            int index = GridViewInstructors.SelectedIndex;

            // Retrieve the InstructorID from the selected row
            int instructorId = Convert.ToInt32(GridViewInstructors.DataKeys[index].Value);

            // Assuming you have a method to get data of a single instructor by ID
            DataRow instructorData = GetInstructorData(instructorId);

            if (instructorData != null)
            {
                // Populate the text boxes with the retrieved data
                ins_id.Text = instructorData["InstructorID"].ToString();
                ins_name.Text = instructorData["InstructorName"].ToString();
                ins_email.Text = instructorData["InstructorEmail"].ToString();
                ins_contact.Text = instructorData["InstructorPhone"].ToString();
                ins_info.Text = instructorData["ins_info"].ToString();
                ins_qualiDetails.Text = instructorData["ins_quali"].ToString();
                ins_specDetails.Text = instructorData["ins_spec"].ToString();
                ShowSweetAlert("Success", "Instructor retrieved successfully.", "success");
            }
        }

        private DataRow GetInstructorData(int instructorId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
              
                string sql = "SELECT * FROM CourseInstructor WHERE InstructorID = @InstructorID";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@InstructorID", instructorId);

                
                    con.Open();

                  
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                      
                        if (dataTable.Rows.Count > 0)
                        {
                          
                            return dataTable.Rows[0];
                        }
                    }
                }
            }

          
            return null;
        }


        protected void btnCreateInstructor_Click(object sender, EventArgs e)
        {
            bool isValid = true;
            lblError.Text = "";
            txtInstructorEmailError.Text = "";
            txtInstructorPhoneError.Text = "";
            txtInstructorNameError.Text = "";
            DropDownList1Error.Text = "";
            DropDownList2Error.Text = "";

            // Retrieve the values from the form controls
            string instructorName = txtInstructorName.Text.Trim();
            string instructorEmail = txtInstructorEmail.Text.Trim();
            string instructorPhone = txtInstructorPhone.Text.Trim();
            string qualification = DropDownList1.SelectedValue;
            string specialization = DropDownList2.SelectedValue;
            string additionalInfo = txtInsInfo.Text.Trim();

          

     
            if (string.IsNullOrWhiteSpace(instructorName) || instructorName.Any(char.IsDigit))
            {
                // Handle the error - name contains numbers
                txtInstructorNameError.Text = "Name must not contain numbers.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(instructorEmail))
            {
                txtInstructorEmailError.Text += "Email cannot be empty.<br />";
                isValid = false;
            }
            else
            {
                try
                {
                    var addr = new System.Net.Mail.MailAddress(instructorEmail);
                    if (addr.Address != instructorEmail)
                    {
                        txtInstructorEmailError.Text += "Email is invalid.<br />";
                        isValid = false;
                    }
                }
                catch
                {
                    txtInstructorEmailError.Text += "Email is invalid.<br />";
                    isValid = false;
                }
            }

            if (string.IsNullOrWhiteSpace(instructorPhone) || instructorPhone.Any(c => !char.IsDigit(c)))
            {
               
                txtInstructorPhoneError.Text = "Phone number must contain only digits.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(qualification))
            {
                DropDownList1Error.Text += "Please select a qualification.<br />";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(specialization))
            {
                DropDownList2Error.Text += "Please select a specialization.<br />";
                isValid = false;
            }

            if (isValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // SQL command to insert the new instructor data
                    string sql = "INSERT INTO CourseInstructor (InstructorName, InstructorEmail, InstructorPhone, ins_quali, ins_spec, ins_info) VALUES (@InstructorName, @InstructorEmail, @InstructorPhone, @Qualification, @Specialization, @AdditionalInfo)";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        // Add parameters with the values to insert
                        cmd.Parameters.AddWithValue("@InstructorName", instructorName);
                        cmd.Parameters.AddWithValue("@InstructorEmail", instructorEmail);
                        cmd.Parameters.AddWithValue("@InstructorPhone", instructorPhone);
                        cmd.Parameters.AddWithValue("@Qualification", qualification);
                        cmd.Parameters.AddWithValue("@Specialization", specialization);
                        cmd.Parameters.AddWithValue("@AdditionalInfo", additionalInfo);

                        // Open the connection, execute the command, and close the connection
                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        con.Close();

                        if (rowsAffected > 0)
                        {

                            BindInstructorGridView();
                            ShowSweetAlert("Success", "Instructor created successfully.", "success");                        
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseModal", "$('#createInstructorModal').modal('hide');", true);
                            txtInstructorName.Text = "";
                            txtInstructorEmail.Text = "";
                            txtInstructorPhone.Text = "";
                            txtInsInfo.Text = "";

                        }
                        else
                        {
                            // Insert failed, handle as needed
                            lblError.Text = "An error occurred while creating the instructor. Please try again.";
                        }
                    }
                }
            }
        }

        private void ShowSweetAlert(string title, string message, string type)
        {
            string script = $"Swal.fire({{ title: '{title}', text: '{message}', icon: '{type}', confirmButtonText: 'Ok' }});";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "sweetAlert", script, true);
        }



        protected void GridViewInstructors_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
      
            GridViewInstructors.PageIndex = e.NewPageIndex;
            BindInstructorGridView();
        }

        protected void GridViewInstructors_Sorting(object sender, GridViewSortEventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT InstructorID, InstructorName, InstructorPhone, InstructorEmail, ins_quali, ins_spec FROM CourseInstructor";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        DataView dataView = new DataView(dataTable);
                        dataView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);

                        GridViewInstructors.DataSource = dataView;
                        GridViewInstructors.DataBind();
                    }
                }
            }
        }

        private string GetSortDirection(string column)
        {
            // Retrieve the last sort direction and sort expression
            string sortDirection = "ASC", lastSortExpr = ViewState["SortExpression"] as string;
            if (lastSortExpr != null && lastSortExpr == column)
            {
                var lastDirection = ViewState["SortDirection"] as string;
                if (lastDirection != null && lastDirection == "ASC")
                    sortDirection = "DESC";
            }

            // Update the sort direction and sort expression in ViewState
            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }

        private void BindInstructorGridView(string searchText = "")
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT InstructorID, InstructorName, InstructorPhone, InstructorEmail, ins_quali, ins_spec  FROM CourseInstructor WHERE InstructorName LIKE @SearchText OR InstructorEmail LIKE @SearchText";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@SearchText", $"%{searchText}%");
                    con.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        GridViewInstructors.DataSource = dataTable;
                        GridViewInstructors.DataBind();
                    }
                }
            }
        }


       


        protected void Button1_Click(object sender, EventArgs e)
        { }
        protected void Button2_Click(object sender, EventArgs e)
        { }


        protected void InstuctorDelete_Click(object sender, EventArgs e)
        {
            // Assuming you have a hidden field or label that holds the ID of the instructor to be deleted
            // For example, a Label control with ID 'lblInstructorId'
            string instructorId = ins_id.Text;

            if (string.IsNullOrWhiteSpace(instructorId))
            {
                ShowSweetAlert("Error", "Instructor ID is not specified.", "error");
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Prepare the SQL command to delete the instructor
                string sql = "DELETE FROM CourseInstructor WHERE InstructorID = @InstructorID";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    
                    cmd.Parameters.AddWithValue("@InstructorID", instructorId);                
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    con.Close();

                   
                    if (rowsAffected > 0)
                    {
                       
                        ShowSweetAlert("Success", "Instructor deleted successfully.", "success");
                        ins_id.Text = "";
                        ins_name.Text = "";
                        ins_email.Text = "";
                        ins_contact.Text = "";
                        ins_info.Text = "";
                        ins_qualiDetails.Text = "";
                        ins_specDetails.Text = "";

                        BindInstructorGridView();
                    }
                    else
                    {
                        
                        ShowSweetAlert("Error", "There was an error deleting the instructor. Please try again.", "error");
                    }
                }
            }
        }



        protected void InstructorUpdate_Click(object sender, EventArgs e)
        {
            
            string instructorId = ins_id.Text;
            string instructorName = ins_name.Text.Trim(); 
            string instructorEmail = ins_email.Text.Trim();
            string instructorPhone = ins_contact.Text.Trim(); 
            string additionalInfo = ins_info.Text; 
            string qualification = ins_qualiDetails.SelectedValue; 
            string specialization = ins_specDetails.SelectedValue;

            bool isValid = true;

           
            if (string.IsNullOrEmpty(instructorId) || !int.TryParse(instructorId, out _))
            {
                ShowSweetAlert("Error", "Please select an instructor.", "error");
                isValid = false;
            }
            if (string.IsNullOrEmpty(instructorName) || instructorName.Any(char.IsDigit))
            {
                ShowSweetAlert("Error", "Instructor Name cannot contain numbers or be empty.", "error");
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(instructorEmail))
            {
                ShowSweetAlert("Error", "Email cannot be empty.", "error");
                isValid = false;
            }
            else if (!new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(instructorEmail))
            {
                ShowSweetAlert("Error", "Email is not in a valid format.", "error");
                isValid = false;
            }

         
            if (string.IsNullOrWhiteSpace(instructorPhone))
            {
                ShowSweetAlert("Error", "Phone Number cannot be empty.", "error");
                isValid = false;
            }
            else if (instructorPhone.Any(c => !char.IsDigit(c)))
            {
                ShowSweetAlert("Error", "Phone Number must contain only digits.", "error");
                isValid = false;
            }


            if (isValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                   
                    string sql = "UPDATE CourseInstructor SET InstructorName = @InstructorName, InstructorEmail = @InstructorEmail, InstructorPhone = @InstructorPhone, ins_quali = @Qualification, ins_spec = @Specialization, ins_info = @AdditionalInfo WHERE InstructorID = @InstructorID";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        // Add parameters with the values to update
                        cmd.Parameters.AddWithValue("@InstructorID", instructorId);
                        cmd.Parameters.AddWithValue("@InstructorName", instructorName);
                        cmd.Parameters.AddWithValue("@InstructorEmail", instructorEmail);
                        cmd.Parameters.AddWithValue("@InstructorPhone", instructorPhone);
                        cmd.Parameters.AddWithValue("@Qualification", qualification);
                        cmd.Parameters.AddWithValue("@Specialization", specialization);
                        cmd.Parameters.AddWithValue("@AdditionalInfo", additionalInfo);

                        // Open the connection, execute the command, and close the connection
                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        con.Close();

                        // Check if the update operation was successful
                        if (rowsAffected > 0)
                        {
                            ShowSweetAlert("Success", "Instructor updated successfully.", "success");
                          
                            BindInstructorGridView(); 
                        }
                        else
                        {
                            ShowSweetAlert("Error", "No instructor was updated. Please check the ID and try again.", "error");
                        }
                    }
                }
            }
        }

       
    }
}