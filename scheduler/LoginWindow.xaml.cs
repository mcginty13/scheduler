using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace scheduler
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }



        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            CheckLogin();
        }
        private void CheckLogin()
        {
            string connstr = "Data Source=SHINOBI;Initial Catalog=agileDB;Integrated Security=True";
            string sql = "SELECT Count(*) FROM users where User_ID = @username AND password = @password";
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@username", Username_TextBox.Text);
                cmd.Parameters.AddWithValue("@password", Password_PasswordBox.Password);
                object count;
                try
                {
                    conn.Open();
                    count = cmd.ExecuteScalar();
                    conn.Close();
                    if ((int)count == 1)
                    {
                        DialogResult = true;
                        MainWindow.user_ID = int.Parse(Username_TextBox.Text);
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
 
         
            
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
