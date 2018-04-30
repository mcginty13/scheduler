using System;
using System.Collections.Generic;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum UserType
        {
            manager,
            student,
            admin
        }

        public static UserType userType;
        public static int user_ID;
        public MainWindow()
        {
            LoginWindow login = new LoginWindow();
            bool result = (bool)login.ShowDialog();

            if (result)
            {
                InitializeComponent();
                Content = new TaskSelectPage();
            }
            else
            {
                this.Close();
            }



        }
    }
}
