using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace Todo_Project
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        private string _itemtext;
        private ICommand _AddItemCommand;
        private ICommand _RemoveItemCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        public Item SelectedItem { get; set; } = new Item();

        public ObservableCollection<Item> items { get; set; } = new ObservableCollection<Item>();

        public string itemtext
        {
            get
            {
                return _itemtext;
            }
            set
            {
                _itemtext = value;
                NotifyPropertyChanged("itemtext");
            }
        }
        public ICommand AddItemCommand
        {
            get
            {
                return _AddItemCommand ?? (_AddItemCommand = new RelayCommand(() =>
                {
                    items.Add(new Item() { Name = itemtext });

                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        string queryString = "INSERT INTO TodoItems VALUES ('" + itemtext + "')";
                        SqlCommand command = new SqlCommand(queryString, connection);
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                    }
                }, () => true));
            }
        }
        public ICommand RemoveItemCommand
        {
            get
            {
                return _RemoveItemCommand ?? (_RemoveItemCommand = new RelayCommand(() =>
                {
                    if (SelectedItem != null)
                    {
                        items.Remove(SelectedItem as Item);
                        using (SqlConnection connection = new SqlConnection(_connectionString))
                        {
                            string queryString = "DELETE FROM TodoItems WHERE TodoItems='" + itemtext + "'";
                            SqlCommand command = new SqlCommand(queryString, connection);
                            command.Connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }
                }, () => true));
            }
        }

        public MainWindow()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                List<string> databaseitems = new List<string>();
                string queryString = "SELECT * FROM TodoItems";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    databaseitems.Add(String.Format("{0}", reader[0]));
                }
                foreach (string item in databaseitems)
                {
                    items.Add(new Item() { Name = item });
                }
            }

            InitializeComponent();

            DataContext = this;
        }

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
