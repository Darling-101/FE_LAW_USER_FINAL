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
        public const string dbcon = @"Data Source=C:\Users\20010844\Desktop\law.db";
        SQLiteConnection conn = new SQLiteConnection(dbcon);
        SQLiteDataAdapter adapter = new SQLiteDataAdapter();
        private int clauseSearch, articleSearch, pointSearch;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public MainWindow()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            initArticle();
            initClause();
            clauseCombobox.IsEnabled = false;
            initShow();
        }

        public void initShow()
        {
            string query = "SELECT DISTINCT article, article_content  FROM Law";
            SQLiteCommand cmd = new SQLiteCommand();
            conn.Open();
            cmd.CommandText = query;
            cmd.Connection = conn;
            adapter.SelectCommand = cmd;
            SQLiteDataReader reader = cmd.ExecuteReader();
            listBox.Items.Clear();
            while (reader.Read())
            {
                if (reader[1].ToString() == "") continue;
                TextBlock newT = new TextBlock();
                newT.TextWrapping = TextWrapping.WrapWithOverflow;
                newT.MaxWidth = 1000;
                newT.Text = String.Format("Điều: {0} \nNội dung điều: {1}", reader[0], reader[1]);
                
                listBox.Items.Add(newT);
            }
            conn.Close();
        }
        public void initArticle()
        {
            string query = "SELECT DISTINCT article FROM Law ORDER BY article ASC";
            SQLiteCommand cmd = new SQLiteCommand();
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
                if (reader[0].ToString() == "0") continue;
                item.Content = String.Format("{0}", reader[0]);
                articleCombobox.Items.Add(item);
            }
            conn.Close();
        }

        public void initClause()
        {
            string query;
            if (articleSearch == null)
            {
                query = "SELECT DISTINCT clause FROM Law ";
            }
            else
            {
                query = "SELECT DISTINCT clause FROM Law WHERE  clause > -1 AND article=" + articleSearch + " ORDER BY clause ASC";
            }
            SQLiteCommand cmd = new SQLiteCommand();
            conn.Open();
            cmd.CommandText = query;
            cmd.Connection = conn;
            adapter.SelectCommand = cmd;
            SQLiteDataReader reader = cmd.ExecuteReader();
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
            int count = 0;
            if(_searchText.Trim(' ') == "")
            {
                MessageBox.Show("Vui lòng nhập từ khóa!!");
                return;
            }
            SQLiteCommand cmd = new SQLiteCommand();
            string query = "SELECT * FROM Law WHERE article_content LIKE '%" + _searchText + "%'" + "OR clause_content LIKE'%" + _searchText + "%'";
                conn.Open();
                cmd.CommandText = query;
                cmd.Connection = conn;
                adapter.SelectCommand = cmd;
                SQLiteDataReader reader = cmd.ExecuteReader();
                listBox.Items.Clear();

                while (reader.Read())
                {
                    TextBlock newT = new TextBlock();
                    count++;
                    newT.TextWrapping = TextWrapping.WrapWithOverflow;
                    newT.MaxWidth = 1450;
                    newT.Text = String.Format("Điều: {0} \nNội dung điều:\n {1} \nKhoản: {2} \nNội dung khoản:\n {3} \nMức phạt trên: {4} \nMức phạt dưới: {5}", reader[1], reader[2], reader[3], reader[4], reader[5], reader[6]);
                    
                    listBox.Items.Add(newT);
                }
                if(count == 0)
                {
                    TextBlock newT = new TextBlock();
                    newT.Text = "Không tìm thấy thông tin này";
                    listBox.Items.Add(newT);
            }
                conn.Close();

        }

        private void articleCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem typeItem = (ComboBoxItem)articleCombobox.SelectedItem;
            articleSearch = Int32.Parse(typeItem.Content.ToString());
            string query = "SELECT clause, clause_content, fine_above, fine_below FROM Law WHERE article=" + articleSearch;
            SQLiteCommand cmd = new SQLiteCommand();
            conn.Open();
            cmd.CommandText = query;
            cmd.Connection = conn;
            adapter.SelectCommand = cmd;
            SQLiteDataReader reader = cmd.ExecuteReader();
            listBox.Items.Clear();
            while (reader.Read())
            {
                if (reader[1].ToString() == "") continue;
                TextBlock newT = new TextBlock();
                newT.TextWrapping = TextWrapping.WrapWithOverflow;
                newT.MaxWidth = 1450;
                newT.Text = String.Format("Khoản: {0} \nNội dung khoản: {1} \nMức phạt dưới: {2} \nMức phạt trên: {3}", reader[0], reader[1], reader[3], reader[2]);
                
                listBox.Items.Add(newT);
            }
            conn.Close();
            initClause();
            clauseCombobox.IsEnabled = true;          
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            initShow();
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = this.listBox.SelectedIndex;
            //MessageBox.Show();
        }

        private void clauseCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem typeItem = (ComboBoxItem)clauseCombobox.SelectedItem;
            
            if(typeItem != null)
            {
                clauseSearch = Int32.Parse(typeItem.Content.ToString());

                string query = "SELECT point, point_content, fine_above, fine_below FROM Law WHERE article=" + articleSearch + " AND clause=" + clauseSearch;
                SQLiteCommand _cmd = new SQLiteCommand();
                conn.Open();
                _cmd.CommandText = query;
                _cmd.Connection = conn;
                adapter.SelectCommand = _cmd;
                SQLiteDataReader reader = _cmd.ExecuteReader();
                listBox.Items.Clear();
                while (reader.Read())
                {
                    if (reader[1].ToString() == "") continue;
                    TextBlock newT = new TextBlock();
                    newT.TextWrapping = TextWrapping.WrapWithOverflow;
                    newT.MaxWidth = 1450;
                    newT.Text = String.Format("Điểm: {0} \nNội dung điểm: {1} \nMức phạt dưới: {2} \nMức phạt trên: {3}", reader[0], reader[1], reader[3], reader[2]);
                    listBox.Items.Add(newT);
                    
                }
                conn.Close();
            }
        }
    }
}
