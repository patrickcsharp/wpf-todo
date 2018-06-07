using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Todo_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public User SelectedItem { get; set; } = new User();
        public ObservableCollection<User> users { get; set; } = new ObservableCollection<User>();
        private string _textbox;
        public string textbox
        {
            get
            {
                return _textbox;
            }
            set
            {
                _textbox = value;
                NotifyPropertyChanged("textbox");
            }
        }
        private ICommand _addUserCommand;
        public ICommand AddCommand
        {
            get
            {
                return _addUserCommand ?? (_addUserCommand = new RelayCommand(AddUser, CanAddUser));
            }
        }

        private void AddUser()
        {
            users.Add(new User() { Name = textbox });
        }

        private bool CanAddUser()
        {
            return true;
        }


        private ICommand _removeUserCommand;
        public ICommand RemoveCommand
        {
            get
            {
                return _removeUserCommand ?? (_addUserCommand = new RelayCommand(RemoveUser, CanRemoveUser));
            }
        }

        private void RemoveUser()
        {
            if (SelectedItem != null)
            {
                users.Remove(SelectedItem as User);
            }
        }

       private bool CanRemoveUser()
        {
            return true;
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

        private void TodoRemove(object sender, RoutedEventArgs e)
        {
            if (SelectedItem != null)
                users.Remove(SelectedItem as User);
        }
    }
}
