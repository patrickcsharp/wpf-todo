using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Todo_Project
{
    public class AddUserCommand : ICommand
    {

        private Collection<User> _users;

        public AddUserCommand(ObservableCollection<User> users)
        {
            _users = users;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _users.Add(new User() { Name = parameter.ToString() });
        }
    }
}
