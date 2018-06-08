using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Data.SqlClient;

namespace Todo_Project
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        string connectionString = "Data Source=localhost;Initial Catalog=TodoDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public Item SelectedItem { get; set; } = new Item();
        public ObservableCollection<Item> items { get; set; } = new ObservableCollection<Item>();
        private string _itemtext;
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
        private ICommand _AddItemCommand;
        public ICommand AddItemCommand
        {
            get
            {
                return _AddItemCommand ?? (_AddItemCommand = new RelayCommand(() =>
                {
                    items.Add(new Item() { Name = itemtext });

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string queryString = "INSERT INTO TodoItems VALUES ('" + itemtext + "')";
                        SqlCommand command = new SqlCommand(queryString, connection);
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                    }
                }, () => true));
            }
        }
        private ICommand _RemoveItemCommand;
        public ICommand RemoveItemCommand
        {
            get
            {
                return _RemoveItemCommand ?? (_RemoveItemCommand = new RelayCommand(() =>
                {
                    if (SelectedItem != null)
                    {
                        items.Remove(SelectedItem as Item);
                        using (SqlConnection connection = new SqlConnection(connectionString))
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

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public MainWindow()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
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

            this.DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
