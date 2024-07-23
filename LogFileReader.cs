using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace WebApplication1
{
    public class LogFileReader
    {
        private string logFilePath;

        // Constructor that takes the log file path as an argument
        public LogFileReader(string filePath)
        {
            logFilePath = filePath;
        }

        public DataTable ReadLogData()
        {
            DataTable logDataTable = new DataTable();
            logDataTable.Columns.Add("Date", typeof(string));
            logDataTable.Columns.Add("userID", typeof(string));
            logDataTable.Columns.Add("Role", typeof(string));
            logDataTable.Columns.Add("Activity", typeof(string));

            if (File.Exists(logFilePath))
            {
                using (FileStream fileStream = File.Open(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader streamReader = new StreamReader(fileStream))
                    {
                        string line;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            string pattern = @"^(?<date>\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2},\d{3}) \[(?<thread>.+?)\] (?<level>\w+) .+? - User ID: (?<userID>.*?), Role: (?<role>.*?), Activity: (?<activity>.+)$";


                            Match match = Regex.Match(line, pattern);

                            if (match.Success && match.Groups.Count >= 4)
                            {
                                string date = match.Groups["date"].Value.Substring(0, 19);
                                string userID = match.Groups["userID"].Value;
                                string role = match.Groups["role"].Value;
                                string activity = match.Groups["activity"].Value;

                                logDataTable.Rows.Add(date, userID, role, activity);
                            }

                        }
                    }
                }
            }
            else
            {
                throw new FileNotFoundException("Log file not found.");
            }

            return logDataTable;
        }
    }
}
