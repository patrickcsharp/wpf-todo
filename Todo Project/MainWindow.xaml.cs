using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using System.Collections.Concurrent;
using System.Threading.Tasks;
namespace Todo_Project
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private bool _isAddingItem = false;
        private bool _isRemovingItem = false;
        private string _connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        private ICommand _AddItemCommand;
        private ICommand _RemoveItemCommand;

        public event PropertyChangedEventHandler PropertyChanged;
        private Item _SelectedItem = new Item();

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
        public ICommand AddItemCommand
        {
            get
            {
                return _AddItemCommand ?? (_AddItemCommand = new RelayCommand(async () =>
                {
                    _isAddingItem = true;
                    items.Add(new Item() { Name = itemtext });

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {                    
                    string queryString = "INSERT INTO TodoItems VALUES ('" + itemtext + "')";
                    SqlCommand command = new SqlCommand(queryString, connection);
                    await command.Connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
                    _isAddingItem = false;
                }, () => !_isAddingItem));
            }
        }
        public ICommand RemoveItemCommand
        {
            get
            {
                return _RemoveItemCommand ?? (_RemoveItemCommand = new RelayCommand(async () =>
                {
                    if (SelectedItem != null)
                    {
                        _isRemovingItem = true;
                        Item _item = new Item();
                        _item = SelectedItem;
                        items.Remove(SelectedItem as Item);
                        using (SqlConnection connection = new SqlConnection(_connectionString))
                        {
                            string queryString = "DELETE FROM TodoItems WHERE TodoItems='" + _item.Name.ToString() + "'";
                            SqlCommand command = new SqlCommand(queryString, connection);
                            await command.Connection.OpenAsync();
                            await command.ExecuteNonQueryAsync();
                        }
                        _isRemovingItem = false;
                    }
                }, () => !_isRemovingItem));
            }
        }

        public MainWindow()
        {

            InitializeComponent();

            DataContext = this;

            ListboxLoad();
        }

        public async void ListboxLoad()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                ConcurrentBag<string> databaseitems = new ConcurrentBag<string>();
                string queryString = "SELECT * FROM TodoItems";
                SqlCommand command = new SqlCommand(queryString, connection);
                await command.Connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();                
                while (await reader.ReadAsync())
                {
                    databaseitems.Add(String.Format("{0}", reader[0]));
                }
                foreach (string item in databaseitems)
                {
                    items.Add(new Item() { Name = item });
                }
            }
        }

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
