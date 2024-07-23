using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class adminhomepage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if the user has admin access during page load
            if (Session["AdminAccess"] == null)
            {
                // Redirect to the login page for users without admin access
                Response.Redirect("userlogin.aspx");
            }
           
        }

    }
}