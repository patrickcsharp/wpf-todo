using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;

namespace Todo_Project
{
    class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private Func<Task> _doWork;
        private Func<bool> _canDoWork;

        public RelayCommand(Func<Task> doWork, Func<bool> canDoWork)
        {
            _doWork = doWork;
            _canDoWork = canDoWork;
        }

        public bool CanExecute(object parameter)
        {
            return _canDoWork();
        }

        public async void Execute(object parameter)
        {        
            await _doWork();
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
