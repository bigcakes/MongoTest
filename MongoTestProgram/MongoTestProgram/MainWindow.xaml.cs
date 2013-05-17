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
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;
using MongoTestProgram.Extensions;

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

        //I know this code is not fantastic, just messing around with Mongo

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            var user = new User();

            user.username = txtUsername.Text;
            user.firstName = txtFirstName.Text;
            user.lastName = txtLastName.Text;

            var returnedList = Database.InsertUsers(new List<User>() { user });
        }

        private void btnGetUser_Click(object sender, RoutedEventArgs e)
        {
            var users = Database.SearchUsers(txtGetUserUsername.Text, txtGetUserFirstName.Text, txtGetUserLastName.Text);

            lstUserResults.Items.Clear();

            foreach (var user in users)
            {
                var listItem = new ListBoxItem()
                {
                    Display = user.username,
                    Value = user.Id
                };

                lstUserResults.Items.Add(listItem);
            }

            btnUpdateUser.IsEnabled = false;
        }

        private void btnUpdateUser_Click(object sender, RoutedEventArgs e)
        {
            if (lstUserResults.SelectedIndex > -1)
            {
                var listItem = (ListBoxItem)lstUserResults.Items[lstUserResults.SelectedIndex];

                var updatedUser = new User()
                {
                    username = txtUpdateUserUsername.Text,
                    firstName = txtUpdateUserFirstName.Text,
                    lastName = txtUpdateUserLastName.Text,
                    Id = (ObjectId)listItem.Value
                };

                var updatedUsers = Database.UpdateUsers(new List<User>() { updatedUser });
            }
        }

        private void btnAddMillionUsers_Click(object sender, RoutedEventArgs e)
        {
            var userList = new List<User>();

            for (var i = 0; i < 1000000; i++)
            {
                var newUser = new User()
                {
                    username = "toughGuy" + i.ToString(),
                    firstName = "Frank" + i.ToString(),
                    lastName = "Bob" + i.ToString()
                };

                userList.Add(newUser);
            }

            var returnedList = Database.InsertUsers(userList);
        }

        private void btnDeleteAllUsers_Click(object sender, RoutedEventArgs e)
        {
            var returnedList = Database.SearchUsers().Select(u => u.Id).ToList();

            var success = Database.RemoveUsers(returnedList);
        }

        private void lstUserResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstUserResults.SelectedIndex > -1)
            {
                var listItem = (ListBoxItem)lstUserResults.Items[lstUserResults.SelectedIndex];

                var user = Database.GetUsers(new List<ObjectId>() { (ObjectId)listItem.Value }).FirstOrDefault();

                if (user != null)
                {
                    txtUpdateUserUsername.Text = user.username;
                    txtUpdateUserFirstName.Text = user.firstName;
                    txtUpdateUserLastName.Text = user.lastName;                    
                }

                btnUpdateUser.IsEnabled = true;
            }
            else
            {
                btnUpdateUser.IsEnabled = false;
            }
        }


        public class ListBoxItem
        {
            public Object Display { get; set; }
            public Object Value { get; set; }

            public ListBoxItem()
            {
                
            }

            public override string ToString()
            {
                return Display.ToString();
            }

            public Object ToValue()
            {
                return Value;
            }
        }
    }
}
