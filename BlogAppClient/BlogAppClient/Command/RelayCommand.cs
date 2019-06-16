using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BlogApp.Command
{
    class RelayCommand : ICommand
    {
        private Predicate<object> _canExecute; // amit ennek átadunk, az dönti el, hogy lefuthat-e az adott action. általános esetben mindig true-t adunk át, de ha speciális eset kell, hogy ne lehessen futtatni a metódust, ez esetben ide logikai kiértékelést adunk át
        private Action<object> _execute; //ebbe kerül a metódushívás

        public RelayCommand(Predicate<object> canExecute, Action<object> execute)
        {
            this._canExecute = canExecute;
            this._execute = execute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
