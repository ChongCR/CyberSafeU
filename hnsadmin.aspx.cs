using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class helpandsupport : System.Web.UI.Page
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
                BindGridViewHelpSupport();
            }    
        }
        protected void txtSearchHelpSupport_TextChanged(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
             
                string query = @"
            SELECT 
                hs.inquiry_id, 
                hs.inquiry_title, 
                hs.inquiry_content, 
                u.email, 
                hs.date_submitted, 
                hs.inquiry_status, 
                hs.respond_by, 
                hs.response 
            FROM HelpAndSupport hs
            INNER JOIN users u ON hs.user_id = u.user_id
            WHERE hs.inquiry_title LIKE @searchText 
            OR hs.inquiry_content LIKE @searchText
            OR u.email LIKE @searchText";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                  
                    cmd.Parameters.AddWithValue("@searchText", "%" + txtSearchHelpSupport.Text.Trim() + "%");

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        GridViewHelpSupport.DataSource = dt;
                        GridViewHelpSupport.DataBind();
                    }

                    con.Close();
                }
            }
        }



        protected void GridViewHelpSupport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewHelpSupport.PageIndex = e.NewPageIndex;
            BindGridViewHelpSupport();
        }

        private void BindGridViewHelpSupport()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
              
                string query = @"
            SELECT 
                hs.inquiry_id, 
                hs.inquiry_title, 
                hs.inquiry_content, 
                u.email, 
                hs.date_submitted, 
                hs.inquiry_status, 
                hs.respond_by, 
                hs.response 
            FROM HelpAndSupport hs
            INNER JOIN users u ON hs.user_id = u.user_id";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        GridViewHelpSupport.DataSource = dt;
                        GridViewHelpSupport.DataBind();
                    }

                    con.Close();
                }
            }
        }


        protected void GridViewHelpSupport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                int statusColumnIndex = 4; 


                string status = e.Row.Cells[statusColumnIndex].Text;

                // Change the colors based on the status
                if (string.Equals(status, "Pending", StringComparison.OrdinalIgnoreCase))
                {
                    e.Row.Cells[statusColumnIndex].ForeColor = System.Drawing.Color.Orange;
                }
                else if (string.Equals(status, "Resolved", StringComparison.OrdinalIgnoreCase))
                {
                    e.Row.Cells[statusColumnIndex].ForeColor = System.Drawing.Color.Green;
                }
              
            }
        }


        protected void GridViewHelpSupport_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dataTable = GetHelpSupportData(); 

            if (dataTable != null)
            {
                
                string sortDirection = "ASC";
                if (ViewState["SortExpression"] != null && ViewState["SortExpression"].ToString() == e.SortExpression)
                {
                    ViewState["SortExpression"] = null;
                    sortDirection = "DESC";
                }
                else
                {
                    ViewState["SortExpression"] = e.SortExpression;
                }

                GridViewHelpSupport.DataSource = SortDataTable(dataTable, e.SortExpression, sortDirection);
                GridViewHelpSupport.DataBind();
            }
        }

        private DataView SortDataTable(DataTable dataTable, string sortExpression, string direction)
        {
            dataTable.DefaultView.Sort = sortExpression + " " + direction;
            return dataTable.DefaultView;
        }

        private DataTable GetHelpSupportData()
        {
            DataTable dataTable = new DataTable();

            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                
                string query = "SELECT inquiry_id, inquiry_title, inquiry_content, user_id, date_submitted, inquiry_status, respond_by, response FROM HelpAndSupport";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();

                   
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }

                    con.Close();
                }
            }

            return dataTable;
        }

        protected void GridViewHelpSupport_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = GridViewHelpSupport.SelectedIndex;

            // Assuming you have a DataKeyNames set to "inquiry_id" in GridViewHelpSupport
            int inquiryId = Convert.ToInt32(GridViewHelpSupport.DataKeys[index].Value);

            ViewState["InquiryIDViewstate"] = inquiryId;

            // Fetch the data for the selected inquiry
            DataTable dt = GetHelpSupportDataById(inquiryId);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

              
                emailTextBox.Text = row["email"].ToString(); // Make sure you have a TextBox with ID="emailTextBox"
                date_submitted.Text = Convert.ToDateTime(row["date_submitted"]).ToString("yyyy-MM-dd");
                inquiry_status.Text = row["inquiry_status"].ToString();
                inquiry_content.Text = row["inquiry_content"].ToString();
                response.Text = row["response"].ToString();

                response.Enabled = true;
                ShowSweetAlert("Success", "Inquiry retrieved successfully.", "success");
            }
        }


        private void ShowSweetAlert(string title, string message, string type)
        {
            string script = $"Swal.fire({{ title: '{title}', text: '{message}', icon: '{type}', confirmButtonText: 'Ok' }});";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "sweetAlert", script, true);
        }




        private DataTable GetHelpSupportDataById(int inquiryId)
        {
            DataTable dataTable = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
               
                string query = @"
            SELECT 
                hs.inquiry_id, 
                hs.inquiry_title, 
                hs.inquiry_content, 
                u.email, 
                hs.date_submitted, 
                hs.inquiry_status, 
                hs.respond_by, 
                hs.response 
            FROM HelpAndSupport hs
            INNER JOIN users u ON hs.user_id = u.user_id
            WHERE hs.inquiry_id = @inquiryId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@inquiryId", inquiryId);
                    con.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }

                    con.Close();
                }
            }

            return dataTable;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            // Assuming you have a way to get the admin's user ID, for example, from session
            int adminId = GetAdminUserId(); // You need to implement this method
            int inquiryId = Convert.ToInt32(ViewState["InquiryIDViewstate"]);
            string responseText = response.Text; // Assuming 'response' is the ID of the TextBox where you write the response

            string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Update the inquiry with the response and set the status to 'Resolved'
                string query = @"
            UPDATE HelpAndSupport
            SET 
                inquiry_status = 'Resolved',
                respond_by = @adminId,
                response = @responseText
            WHERE inquiry_id = @inquiryId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@adminId", adminId);
                    cmd.Parameters.AddWithValue("@responseText", responseText);
                    cmd.Parameters.AddWithValue("@inquiryId", inquiryId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            // Optionally, rebind the GridView to show the updated status
            inquiry_status.Text = "Resolved";
            BindGridViewHelpSupport();

            // Optionally, show a notification or message that the reply was successful
            ShowSweetAlert("Success", "The inquiry has been updated with your response.", "success");
        }

        // You need to implement this method to get the admin's user ID
        private int GetAdminUserId()
        {
            // Implement logic to retrieve the admin's user ID, for example, from session or a static value
            // return (int)Session["AdminUserId"]; // Example if using session
            return 1; // Example with a static value
        }

    }
}