using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace scheduler
{
    public class Assessment
    {
        public string assessmentCode { get; set; }
        public string assessmentName { get; set; }
        public string assessmentType { get; set; }
        string session;
        string area;
        string moduleCode;
        string moduleName;
        string moduleManager;
        string moduleChecker;
        string crse_stts;
        int level;
        int n_Students;
        public int gradePercent { get; set; }
        bool compulsaryPass;
        bool reassessmentSame;
        bool scrutinySheetSubmitted;
        bool programLeaderChecked;
        public DateTime releaseDate { get; set; }
        public DateTime submissionDate { get; set; }
        DateTime extensionDate;
        DateTime feedbackDate;
        string comment;

        public string GetAssCode()
        {
            return assessmentCode;
        }
        public Assessment(DataRow row)
        {
            assessmentCode = row[0].ToString();
            assessmentName = row[2].ToString();
            assessmentType = row[3].ToString();
            session = row[1].ToString();
            area = row[12].ToString();
            moduleCode = row[7].ToString();
            moduleName = row[8].ToString();
            moduleManager = row[11].ToString();
            moduleChecker = row[13].ToString();
            crse_stts = row[5].ToString();
            level = (int)row[10];
            n_Students = (int)row[9];
            gradePercent = (int)row[4];
            compulsaryPass = (bool)row[6];
            reassessmentSame = (bool)row[14];
            scrutinySheetSubmitted = (bool)row[17];
            programLeaderChecked = (bool)row[18];
            if (row[15].ToString() != "" && row[16].ToString() != "")
            {
                releaseDate = (DateTime)row[15];
                submissionDate = (DateTime)row[16];
                scheduled = true;
            }
            else { scheduled = false; }
            if (row[20].ToString() != "" && row[21].ToString() != "")
            {
                extensionDate = (DateTime)row[20];
                feedbackDate = (DateTime)row[21];
            }
            
            comment = row[19].ToString();
        }

        public bool ScheduleAssessment(DateTime pReleaseDate, DateTime pSubmissionDate, DateTime pExtensionDate, DateTime pFeedbackDate)
        {
            string sql = "UPDATE dbo.assessments2 SET" +
                         " Release_Date = @Release_Date, " +
                         "Submission_Date = @Submission_Date, " +
                         "Extension_Date = @Extension_Date, " +
                         "Feedback_Date = @Feedback_Date " +
                         "WHERE Assessment_Code = @AssessmentCode";
            string connstr = "Data Source=SHINOBI;Initial Catalog=agileDB;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connstr))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Release_Date", pReleaseDate);
                cmd.Parameters.AddWithValue("@Submission_Date", pSubmissionDate);
                cmd.Parameters.AddWithValue("@Extension_Date", pExtensionDate);
                cmd.Parameters.AddWithValue("@Feedback_Date", pFeedbackDate);
                cmd.Parameters.AddWithValue("@AssessmentCode", assessmentCode);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return true;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }

            }
        }
    }
}