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
using MongoTestProgram.Models;
using MongoTestProgram.Services;

namespace MongoTestProgram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            var user = new User();

            user.username = txtUsername.Text;
            user.firstName = txtFirstName.Text;
            user.lastName = txtLastName.Text;

            var returnedList = Database.InsertUsers(new List<User>() { user });

            var fancy = "";

        }

        private void btnGetUser_Click(object sender, RoutedEventArgs e)
        {
            var searchUserBy = "username";
            var searchUsing = "";

            if (!string.IsNullOrEmpty(txtGetUserUsername.Text))
            {
                searchUserBy = "username";
                searchUsing = txtGetUserUsername.Text;
            }
            else if(!string.IsNullOrEmpty(txtGetUserFirstName.Text))
            {
                searchUserBy = "firstName";
                searchUsing = txtGetUserFirstName.Text;
            }
            else if (!string.IsNullOrEmpty(txtGetUserLastName.Text))
            {
                searchUserBy = "lastName";
                searchUsing = txtGetUserLastName.Text;
            }

            var users = Database.SearchUsers(searchUserBy, searchUsing);

            lstUserResults.Items.Clear();

            foreach (var user in users)
            {
                lstUserResults.Items.Add(user.username);
            }

        }
    }
}
