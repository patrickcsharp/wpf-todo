using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Todo_Project
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
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
                return _AddItemCommand ?? (_AddItemCommand = new RelayCommand(() => items.Add(new Item() { Name = itemtext }), () => true));
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
            InitializeComponent();
            this.DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
