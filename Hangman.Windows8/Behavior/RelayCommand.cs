using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Hangman.Windows8.Behavior
{
    public delegate void ExecuteCommandDelegate(object obj);
    public delegate bool CanExecuteCommandDelegate(object obj);

    public class RelayCommand : ICommand
    {
        public ExecuteCommandDelegate execute;
        public CanExecuteCommandDelegate canExecute;

        public RelayCommand(ExecuteCommandDelegate execute)
            : this(execute, null)
        {
        }

        public RelayCommand(ExecuteCommandDelegate execute, CanExecuteCommandDelegate canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (this.canExecute != null)
            {
                return this.canExecute(parameter);
            }
            return true;
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}
