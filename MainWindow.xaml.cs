using Login_Page.Constants;
using Npgsql;
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

namespace Login_Page
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

        private async void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            var result = await RegisterAsync();
            if (result)
            {
                Window1 dashboard = new Window1();
                dashboard.Show();
                this.Close();
            }
            else MessageBox.Show("User NotFound");
        }
        public async Task<bool> RegisterAsync()
        {
            await using (var connection = new NpgsqlConnection(DbConstants.DB_CONNECTION_STRING))
            {
                await connection.OpenAsync();

                string query = $"SELECT COUNT(1) FROM tblUser WHERE Username=@Username AND Password=@Password;";

                await using (var command = new NpgsqlCommand(query, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.Parameters.AddWithValue("@Username", txtUsername.Text);
                    command.Parameters.AddWithValue("@Password", txtPassword.Password);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    if (count == 1)
                    {
                        await connection.CloseAsync();
                        return true;
                    }
                    else
                    {
                        await connection.CloseAsync();
                        return false;
                    }
                }
            }
        }
    }
}
