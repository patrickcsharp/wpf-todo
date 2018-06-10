using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;

namespace Todo_Project
{
    class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action _doWork;
        private Func<bool> _canDoWork;

        public RelayCommand(Action doWork, Func<bool> canDoWork)
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
            _doWork();
            
        }
    }
}
