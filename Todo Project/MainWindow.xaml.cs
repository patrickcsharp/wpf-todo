using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Todo_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public User SelectedItem { get; set; } = new User();
        public ObservableCollection<User> users { get; set; } = new ObservableCollection<User>();
        public string textbox { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (textbox != null)
            {
                users.Add(new User() { Name = textbox });
            }           
        }
        private void TodoRemove(object sender, RoutedEventArgs e)
        {
            if (SelectedItem != null)
                users.Remove(SelectedItem as User);
        }
    }
    public class User : INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get { return this.name; }
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    this.NotifyPropertyChanged("Name");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
