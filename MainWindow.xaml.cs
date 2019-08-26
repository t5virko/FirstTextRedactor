using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Configuration;
using System.Data.SqlClient;

namespace ExperimentApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CreateNew_Click(object sender, RoutedEventArgs e)
        {
            if(textbox.Text != "")
            {
                methodSaveFile();
            }
            textbox.Text = "";
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            methodSaveFile();
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            bool? res = ofd.ShowDialog();

            if (res != false) {

                Stream myStream;
                if ((myStream = ofd.OpenFile()) != null)
                {
                    string file_name = ofd.FileName;
                    string file_text = File.ReadAllText(file_name);
                    textbox.Text = file_text;
                }
            }
        }

        private void methodSaveFile()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            bool? res = sfd.ShowDialog();

            if (res != false)
            {
                using (Stream s = File.Open(sfd.FileName, FileMode.OpenOrCreate))
                {
                    using (StreamWriter sw = new StreamWriter(s))
                    {
                        sw.Write(textbox.Text);
                    }
                }
            }
        }

        private void TimesNewRomanFont_Click(object sender, RoutedEventArgs e)
        {
            textbox.FontFamily = new FontFamily("Times New Roman");
            verdanaFont.IsChecked = false;
            segoeUIFont.IsChecked = false;

        }

        private void VerdanaFont_Click(object sender, RoutedEventArgs e)
        {
            textbox.FontFamily = new FontFamily("Verdana");
            timesNewRomanFont.IsChecked = false;
            segoeUIFont.IsChecked = false;
        }

        private void SegoeUIFont_Click(object sender, RoutedEventArgs e)
        {
            textbox.FontFamily = new FontFamily("Segoe UI");
            timesNewRomanFont.IsChecked = false;
            verdanaFont.IsChecked = false;
        }

        private void ColorWhite_Click(object sender, RoutedEventArgs e)
        {
            textbox.Background = Brushes.White;
            colorBlack.IsChecked = false;
            colorYellow.IsChecked = false;
        }

        private void ColorBlack_Click(object sender, RoutedEventArgs e)
        {
            textbox.Background = Brushes.Black;
            colorYellow.IsChecked = false;
            colorWhite.IsChecked = false;
        }

        private void ColorYellow_Click(object sender, RoutedEventArgs e)
        {
            textbox.Background = Brushes.Yellow;
            colorBlack.IsChecked = false;
            colorWhite.IsChecked = false;
        }

        private void TextColorBlack_Click(object sender, RoutedEventArgs e)
        {
            textbox.Foreground = Brushes.Black;
            textColorBlue.IsChecked = false;
            textColorGreen.IsChecked = false;
        }

        private void TextColorBlue_Click(object sender, RoutedEventArgs e)
        {
            textbox.Foreground = Brushes.Blue;
            textColorGreen.IsChecked = false;
            textColorBlack.IsChecked = false;
        }

        private void TextColorGreen_Click(object sender, RoutedEventArgs e)
        {
            textbox.Foreground = Brushes.Green;
            textColorBlue.IsChecked = false;
            textColorBlack.IsChecked = false;
        }

        private void SelectFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string fontSize = selectFontSize.SelectedItem.ToString();
            fontSize = fontSize.Substring(fontSize.Length - 2);
            switch(fontSize)
            {
                case "10":
                    textbox.FontSize = 10;
                    break;
                case "12":
                    textbox.FontSize = 12;
                    break;
                case "14":
                    textbox.FontSize = 14;
                    break;
                case "16":
                    textbox.FontSize = 16;
                    break;
                case "18":
                    textbox.FontSize = 18;
                    break;
                case "20":
                    textbox.FontSize = 20;
                    break;

            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = ConfigurationManager.AppSettings["connectionString"];

            SqlConnection sql = new SqlConnection(connectionString);

            try
            {
                if (sql.State == System.Data.ConnectionState.Closed)
                    sql.Open();

                string sel = "SELECT COUNT(1) FROM Users WHERE Login=@log AND Password=@pass";
                SqlCommand sqlCom = new SqlCommand(sel, sql);
                sqlCom.CommandType = System.Data.CommandType.Text;
                sqlCom.Parameters.Add("@log", loginField.Text);
                sqlCom.Parameters.Add("@pass", passwordField.Password);

                int cUser = Convert.ToInt32(sqlCom.ExecuteScalar());
                if(cUser == 0)
                {
                    sel = "INSERT INTO Users(Login, Password) VALUES(@log, @pass)";
                    SqlCommand sqlComUser = new SqlCommand(sel, sql);
                    sqlComUser.CommandType = System.Data.CommandType.Text;
                    sqlComUser.Parameters.Add("@log", loginField.Text);
                    sqlComUser.Parameters.Add("@pass", passwordField.Password);

                    sqlComUser.ExecuteNonQuery();
                    MessageBox.Show("We joined u in data base!");
                } else
                {
                    MessageBox.Show("You autorization success!");
                    AuthWindow auPage = new AuthWindow();
                    auPage.Show();
                }
            }
            catch (Exception pr)
            {
                MessageBox.Show(pr.Message);
            }
            finally
            {
                sql.Close();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = ConfigurationManager.AppSettings["connectionString"];

            SqlConnection sql = new SqlConnection(connectionString);

            try
            {
                if (sql.State == System.Data.ConnectionState.Closed)
                    sql.Open();


                string del = "DELETE FROM Users WHERE Login=@log AND Password=@pass";
                SqlCommand sqlComUser = new SqlCommand(del, sql);
                sqlComUser.CommandType = System.Data.CommandType.Text;
                sqlComUser.Parameters.Add("@log", loginField.Text);
                sqlComUser.Parameters.Add("@pass", passwordField.Password);

                sqlComUser.ExecuteNonQuery();

                    MessageBox.Show("User deleted!");
            }
            catch (Exception pr)
            {
                MessageBox.Show(pr.Message);
            }
            finally
            {
                sql.Close();
            }
        }
    }
}
