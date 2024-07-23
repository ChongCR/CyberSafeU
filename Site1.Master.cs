using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UpdateNavigationVisibility();
            }
        }

        private void UpdateNavigationVisibility()
        {
            if (IsUserLoggedIn())
            {
                LinkButton1.Visible = false; // Login
                LinkButton2.Visible = false; // Sign Up
                LinkButton3.Visible = true;  // Logout
                LinkButton4.Visible = true;  // Profile
            }
            else
            {
                LinkButton1.Visible = true;  // Login
                LinkButton2.Visible = true;  // Sign Up
                LinkButton3.Visible = false; // Logout
                LinkButton4.Visible = false; // Profile
            }
        }

        protected bool IsUserLoggedIn()
        {
            return Session["user_id"] != null;
        }


        protected void Logout_Click(object sender, EventArgs e)
        {
            Session.Clear();  // Clear all session variables
            Session.Abandon();  // Abandon the session
            Response.Redirect("userlogin.aspx");  // Redirect to the login page after logout
        }






        private void ShowSweetAlert(string title, string message, string type)
        {
            string script = $"Swal.fire({{ title: '{title}', text: '{message}', icon: '{type}', confirmButtonText: 'Ok' }});";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "sweetAlert", script, true);
        }
    }

}