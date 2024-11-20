using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace WPF_DB_Iwanov_Egor_Pract_3
{

    public partial class MainWindow : Window
    {
        private string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=yagorchick_;User Id=yagorchick_;Password=23042003;TrustServerCertificate=true;";
        private List<User> users;

        public MainWindow()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            users = new List<User>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT UserID, FirstName, Username, Email, CreatedAt FROM Users", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new User
                    {
                        UserID = (int)reader["UserID"],
                        FirstName = reader["FirstName"].ToString(),
                        Username = reader["Username"].ToString(),
                        Email = reader["Email"].ToString(),
                        CreatedAt = (DateTime)reader["CreatedAt"]
                    });
                }
            }
            UsersDataGrid.ItemsSource = users;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var newUser = new User
            {
                FirstName = FirstNameTextBox.Text,
                Username = UsernameTextBox.Text,
                Email = EmailTextBox.Text,
                PasswordHash = PasswordBox.Password // Здесь вы должны использовать хеширование пароля
            };

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Users (FirstName, Username, Email, PasswordHash) VALUES (@FirstName, @Username, @Email, @PasswordHash)", connection);
                command.Parameters.AddWithValue("@FirstName", newUser.FirstName);
                command.Parameters.AddWithValue("@Username", newUser.Username);
                command.Parameters.AddWithValue("@Email", newUser.Email);
                command.Parameters.AddWithValue("@PasswordHash", newUser.PasswordHash);
                command.ExecuteNonQuery();
            }

            LoadUsers();
            FirstNameTextBox.Clear();
            UsernameTextBox.Clear();
            EmailTextBox.Clear();
            PasswordBox.Clear();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is User selectedUser)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("DELETE FROM Users WHERE UserID = @UserID", connection);
                    command.Parameters.AddWithValue("@UserID", selectedUser.UserID);
                    command.ExecuteNonQuery();
                }

                LoadUsers();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите пользователя для удаления.");
            }
        }

        private void UsersDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is User selectedUser)
            {
                FirstNameTextBox.Text = selectedUser.FirstName;
                UsernameTextBox.Text = selectedUser.Username;
                EmailTextBox.Text = selectedUser.Email;
            }
        }
    }
}
