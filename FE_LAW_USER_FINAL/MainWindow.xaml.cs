using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Data.SQLite;

namespace FE_LAW_USER_FINAL {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window {
        public const string dbcon = @"Data Source=C:\Users\20010844\source\repos\FE_LAW_FINAL\FE_LAW_FINAL\testDB.db";
        SQLiteConnection conn = new SQLiteConnection(dbcon);
        SQLiteCommand cmd = new SQLiteCommand();
        SQLiteCommand cmd1 = new SQLiteCommand();
        SQLiteDataAdapter adapter = new SQLiteDataAdapter();
        private string clauseSearch, articleSearch;
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            initArticle();
            initClause();
        }
        public void initArticle()
        {
            string query = "SELECT article FROM Law";
            conn.Open();
            cmd.CommandText = query;
            cmd.Connection = conn;
            adapter.SelectCommand = cmd;
            SQLiteDataReader reader = cmd.ExecuteReader();
            articleCombobox.Items.Clear();
            while (reader.Read())
            {
                TextBlock newT = new TextBlock();
                ComboBoxItem item = new ComboBoxItem();
                item.Content = String.Format("{0}", reader[0]);
                articleCombobox.Items.Add(item);
            }
            conn.Close();
        }

        public void initClause()
        {
            string query = "SELECT clause FROM Law";
            conn.Open();
            cmd1.CommandText = query;
            cmd1.Connection = conn;
            adapter.SelectCommand = cmd1;
            SQLiteDataReader reader = cmd1.ExecuteReader();
            clauseCombobox.Items.Clear();
            while (reader.Read())
            {
                TextBlock newT = new TextBlock();
                ComboBoxItem item = new ComboBoxItem();
                item.Content = String.Format("{0}", reader[0]);
                clauseCombobox.Items.Add(item);
            }
            conn.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String _searchText = searchBox.Text;
            if(_searchText.Trim(' ') == "")
            {
                MessageBox.Show("Vui lòng nhập từ khóa!!");
                return;
            }

            string query = "SELECT * FROM Law";
            conn.Open();
            cmd.CommandText = query;
            cmd.Connection = conn;
            adapter.SelectCommand = cmd;
            SQLiteDataReader reader = cmd.ExecuteReader();
            listBox.Items.Clear();
            while (reader.Read())
            {
                TextBlock newT = new TextBlock();
                Border border = new Border();
                border.BorderThickness = new Thickness(1);
                newT.Text = String.Format("Điều: {0} \nNội dung điều: {1} \nKhoản: {2} \nNội dung khoản: {3} \nMức phạt trên: {4} \nMức phạt dưới: {5}", reader[1], reader[2], reader[3], reader[4], reader[5], reader[6]);
                listBox.Items.Add(newT);
                listBox.Items.Add(border);
            }
            conn.Close();
        }

        private void articleCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem typeItem = (ComboBoxItem)articleCombobox.SelectedItem;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            string articleSearch = typeItem.Content.ToString();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            string query = "SELECT * FROM Law WHERE article=" + articleSearch;
            conn.Open();
            cmd.CommandText = query;
            cmd.Connection = conn;
            adapter.SelectCommand = cmd;
            SQLiteDataReader reader = cmd.ExecuteReader();
            listBox.Items.Clear();
            while (reader.Read())
            {
                TextBlock newT = new TextBlock();
                Border border = new Border();
                border.BorderThickness = new Thickness(1);
                newT.Text = String.Format("Điều: {0} \nNội dung điều: {1} \nKhoản: {2} \nNội dung khoản: {3} \nMức phạt trên: {4} \nMức phạt dưới: {5}", reader[1], reader[2], reader[3], reader[4], reader[5], reader[6]);
                listBox.Items.Add(newT);
                listBox.Items.Add(border);
            }
            conn.Close();
        }

        private void clauseCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem typeItem = (ComboBoxItem)clauseCombobox.SelectedItem;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            string clauseSearch = typeItem.Content.ToString();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            if (articleSearch == null)
            {
                MessageBox.Show("Vui lòng chọn điều");
                return;
            }
            string query = "SELECT * FROM Law WHERE article=" + articleSearch + "AND clause=" + clauseSearch;
            conn.Open();
            cmd.CommandText = query;
            cmd.Connection = conn;
            adapter.SelectCommand = cmd;
            SQLiteDataReader reader = cmd.ExecuteReader();
            listBox.Items.Clear();
            while (reader.Read())
            {
                TextBlock newT = new TextBlock();
                Border border = new Border();
                border.BorderThickness = new Thickness(1);
                newT.Text = String.Format("Điều: {0} \nNội dung điều: {1} \nKhoản: {2} \nNội dung khoản: {3} \nMức phạt trên: {4} \nMức phạt dưới: {5}", reader[1], reader[2], reader[3], reader[4], reader[5], reader[6]);
                listBox.Items.Add(newT);
                listBox.Items.Add(border);
            }
            conn.Close();
        }
    }
}
