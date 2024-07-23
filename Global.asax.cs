using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace WebApplication1
{
    public class Global : System.Web.HttpApplication
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(Global));
        string connectionString = ConfigurationManager.ConnectionStrings["CyberSafeUDatabase"].ConnectionString;

        protected void Application_Start(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure(); // Configure log4net
                                                        
        }
        

       

        // Method to log user activity with username, activity, and time
        public static void LogUserActivity(string user_id, string role, string activity)
        {
            GlobalContext.Properties["user_id"] = user_id;
            GlobalContext.Properties["role"] = role;

            string logMessage = activity;
            log.Info(logMessage);
        }
    }
}