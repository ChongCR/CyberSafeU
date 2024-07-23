using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class evaluation : System.Web.UI.Page
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
                BindLogDataToGridView();
            }
        }
        private void BindLogDataToGridView()
        {
            
                string logFilePath = @"C:\Users\Chun Rock\Downloads\FYPTest2\FYPTest2\WebApplication1\App_Data\Log.xml";
                LogFileReader logFileReader = new LogFileReader(logFilePath); // Pass logFilePath as an argument
                DataTable logData = logFileReader.ReadLogData();

                logGridView.DataSource = logData;
                logGridView.DataBind();
           
        }



    }
}