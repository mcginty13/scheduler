using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace scheduler
{
    /// <summary>
    /// Interaction logic for TaskSelectPage.xaml
    /// </summary>
    public partial class TaskSelectPage : Page
    {
        public static List<Assessment> assessments = new List<Assessment>();
        string res = "";
        public bool scheduling = false;
        public TaskSelectPage()
        {
            InitializeComponent();
        }

        private void Schedule_Button_Click(object sender, RoutedEventArgs e)
        {
            scheduling = true;
            button_stackpanel.Visibility = Visibility.Collapsed;
            pickAssessment_stackpanel.Visibility = Visibility.Visible;
            string connstr = "Data Source=SHINOBI;Initial Catalog=agileDB;Integrated Security=True";
            string sql = "SELECT * FROM assessments2 WHERE Module_Manager = @User_ID";


            using (SqlConnection conn = new SqlConnection(connstr))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@User_ID", MainWindow.user_ID);

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Assessment assess = new Assessment(dt.Rows[i]);
                        assessments.Add(assess);


                    }


                    Assignments_DataGrid.ItemsSource = dt.DefaultView;

                }
            }
        }

        private void Next_Buttton_Click(object sender, RoutedEventArgs e)
        {
            var item = Assignments_DataGrid.SelectedItem as DataRowView;
            if (null == item) return;
            try
            {
                res = item.Row[0].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            if (scheduling)
            {

                pickAssessment_stackpanel.Visibility = Visibility.Collapsed;
                schedule_Stackpanel.Visibility = Visibility.Visible;
                try
                {
                    ReleaseDate_DatePicker.SelectedDate = (DateTime)item.Row["Release_Date"];
                    SubmissionDate_DatePicker.SelectedDate = (DateTime)item.Row["Submission_Date"];
                    ExtensionDate_DatePicker.SelectedDate = (DateTime)item.Row["Extension_Date"];
                    FeedbackDate_DatePicker.SelectedDate = (DateTime)item.Row["Feedback_Date"];

                }
                catch
                {

                }


            }
            else
            {
                if (item.Row[17].ToString() == "True")
                {
                    MessageBox.Show("Already checked that");
                }
                else
                {

                    button_stackpanel.Visibility = Visibility.Collapsed;
                    pickAssessment_stackpanel.Visibility = Visibility.Collapsed;
                    Check_StackPanel.Visibility = Visibility.Visible;
                    Grid grid = questionGrid;
                    //use same picker thing to set current acw. use hasChecked column to set colour
                    for (int i = 1; i < questionGrid.RowDefinitions.Count - 1; i++)
                    {
                        RadioButton yes = new RadioButton();
                        grid.Children.Add(yes);
                        yes.SetValue(Grid.RowProperty, i);
                        yes.SetValue(Grid.ColumnProperty, 1);
                        yes.HorizontalAlignment = HorizontalAlignment.Center;
                        yes.VerticalAlignment = VerticalAlignment.Center;
                        yes.GroupName = i.ToString();
                        yes.Tag = i.ToString() + "_y";

                        RadioButton no = new RadioButton();
                        grid.Children.Add(no);
                        no.SetValue(Grid.RowProperty, i);
                        no.SetValue(Grid.ColumnProperty, 2);
                        no.HorizontalAlignment = HorizontalAlignment.Center;
                        no.VerticalAlignment = VerticalAlignment.Center;
                        no.GroupName = i.ToString();
                        no.Tag = i.ToString() + "_n";

                        RadioButton na = new RadioButton();
                        grid.Children.Add(na);
                        na.SetValue(Grid.RowProperty, i);
                        na.SetValue(Grid.ColumnProperty, 3);
                        na.HorizontalAlignment = HorizontalAlignment.Center;
                        na.VerticalAlignment = VerticalAlignment.Center;
                        na.GroupName = i.ToString();
                        na.Tag = i.ToString() + "_na";
                    }
                    questionGrid = grid;
                    ReferenceWindow refWin = new ReferenceWindow(res);
                    refWin.Show();
                }
            }

        }

        private void SubmissionDate_DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime submissionDate = (DateTime)SubmissionDate_DatePicker.SelectedDate;
            ExtensionDate_DatePicker.SelectedDate = submissionDate.AddDays(10);
            FeedbackDate_DatePicker.SelectedDate = submissionDate.AddDays(28);
        }

        private void Assignments_DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            int i;
            if (scheduling) { i = 15; }
            else { i = 17; }
            try
            {
                if (((System.Data.DataRowView)(e.Row.DataContext)).Row.ItemArray[i].ToString() == "False" || ((System.Data.DataRowView)(e.Row.DataContext)).Row.ItemArray[i].ToString() == "" )
                {
                    e.Row.Background = new SolidColorBrush(Colors.PaleVioletRed);
                }
                else
                {
                    e.Row.Background = new SolidColorBrush(Colors.LightGreen);
                }
            }
            catch
            {

            }

        }

        private void Next2_Buttton_Click(object sender, RoutedEventArgs e)
        {
            Assessment assess = assessments.FirstOrDefault(o => o.assessmentCode == res);
            try
            {
                DateTime releaseDate = (DateTime)ReleaseDate_DatePicker.SelectedDate;
                DateTime submissionDate = (DateTime)SubmissionDate_DatePicker.SelectedDate;
                DateTime extensionDate = (DateTime)ExtensionDate_DatePicker.SelectedDate;
                DateTime feedbackDate = (DateTime)FeedbackDate_DatePicker.SelectedDate;
                assess.ScheduleAssessment(releaseDate, submissionDate, extensionDate, feedbackDate);
                MessageBox.Show("Assessment successfully scheduled");
                schedule_Stackpanel.Visibility = Visibility.Collapsed;
                res = "";
                button_stackpanel.Visibility = Visibility.Visible;
                scheduling = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void Check_Button_Click(object sender, RoutedEventArgs e)
        {
            button_stackpanel.Visibility = Visibility.Collapsed;
            pickAssessment_stackpanel.Visibility = Visibility.Visible;
            string connstr = "Data Source=SHINOBI;Initial Catalog=agileDB;Integrated Security=True";
            string sql = "SELECT * FROM assessments2 WHERE Module_Manager = @User_ID";


            using (SqlConnection conn = new SqlConnection(connstr))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@User_ID", MainWindow.user_ID);

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Assessment assess = new Assessment(dt.Rows[i]);
                        assessments.Add(assess);

                    }

                    Assignments_DataGrid.ItemsSource = dt.DefaultView;

                }
            }
        }

        private void SubmitChecks_Button_Click(object sender, RoutedEventArgs e)
        {
            List<string> answers = new List<string>();
            foreach (UIElement element in questionGrid.Children)
            {

                if (element is RadioButton)
                {
                    RadioButton but = (RadioButton)element;
                    if ((bool)but.IsChecked)
                    {
                        answers.Add(but.Tag.ToString());
                    }
                }
            }
            bool?[] boolAnswers = new bool?[answers.Count];
            for (int i = 0; i < answers.Count; i++)
            {
                if (answers[i].StartsWith((i + 1).ToString()))
                {
                    string[] result = answers[i].Split('_');
                    switch (result[1])
                    {
                        case "y":
                            boolAnswers[i] = true;
                            break;
                        case "n":
                            boolAnswers[i] = false;
                            break;
                        case "na":
                            boolAnswers[i] = null;
                            break;
                    }
                }
            }
            string connstr = "Data Source=SHINOBI;Initial Catalog=agileDB;Integrated Security=True";
            string sql = "INSERT INTO checks (Assessment_Code, q1, q2, q3, q4, q5, q6, q7, q8, q9, q10, q11, q12, comments) VALUES (@Assessment_Code, @q1, @q2, @q3, @q4, @q5, @q6, @q7, @q8, @q9, @q10, @q11, @q12, @comments)";
            string sql2 = "UPDATE dbo.assessments2 SET Scrutiny_Sheet_Submitted = @submitted WHERE Assessment_Code = @Assessment_Code";
            try
            {
                using (SqlConnection conn = new SqlConnection(connstr))
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Assessment_Code", res);
                    for (int i = 0; i < boolAnswers.Length; i++)
                    {
                        cmd.Parameters.AddWithValue("@q" + (i + 1), boolAnswers[i]);
                    }
                    cmd.Parameters.AddWithValue("@comments", CommentBox_TextBox.Text);

                    SqlCommand cmd2 = new SqlCommand(sql2, conn);
                    cmd2.Parameters.AddWithValue("@Assessment_Code", res);
                    cmd2.Parameters.AddWithValue("@submitted", true);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    cmd2.ExecuteNonQuery();
                    conn.Close();
                }
                MessageBox.Show("Successfully submitted scrutiny sheet");
                Check_StackPanel.Visibility = Visibility.Collapsed;
                button_stackpanel.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }



        }

        private void Back1_Buttton_Click(object sender, RoutedEventArgs e)
        {
            pickAssessment_stackpanel.Visibility = Visibility.Collapsed;
            button_stackpanel.Visibility = Visibility.Visible;
            res = "";
            scheduling = false;
        }

        private void Back2_Buttton_Click(object sender, RoutedEventArgs e)
        {
            schedule_Stackpanel.Visibility = Visibility.Collapsed;
            pickAssessment_stackpanel.Visibility = Visibility.Visible;
            res = "";
        }

        private void Back3_Buttton_Click(object sender, RoutedEventArgs e)
        {
            Check_StackPanel.Visibility = Visibility.Collapsed;
            pickAssessment_stackpanel.Visibility = Visibility.Visible;
            res = "";
        }
    }
}