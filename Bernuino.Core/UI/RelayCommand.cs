using System;
using System.Windows.Input;

namespace Bernuino.Core.UI
{
   public class RelayCommand<T> : ICommand
   {
      #region Fields

      private Action<T> _execute;
      private Func<T, bool> _canExecute;

      #endregion

      #region Constructors

      public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
      {
         if (execute is null)
            throw new ArgumentNullException("execute");

         _execute = execute;
         _canExecute = canExecute;
      }

      #endregion

      #region Events

      public event EventHandler CanExecuteChanged
      {
         add { }
         remove { }
      }

      #endregion

      #region Properties

      #endregion

      #region Methods

      public bool CanExecute(object parameter)
      {
         return _canExecute == null
                || _canExecute((T)parameter);
      }

      public void Execute(object parameter)
      {
         _execute(parameter is T ? (T)parameter : default);
      }

      #endregion
   }

   public class RelayCommand : ICommand
   {
      #region Fields

      private Action _execute;
      private Action<object> _executeParam;
      private Func<bool> _canExecute;
      private Func<object, bool> _canExecuteParam;

      #endregion

      #region Constructors

      public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
      {
         if (execute is null)
            throw new ArgumentNullException("execute");

         _executeParam = execute;
         _canExecuteParam = canExecute;
      }

      public RelayCommand(Action execute, Func<bool> canExecute = null)
      {
         if (execute is null)
            throw new ArgumentNullException("execute");

         _execute = execute;
         _canExecute = canExecute;
      }

      #endregion

      #region Events

      public event EventHandler CanExecuteChanged
      {
         add { }
         remove { }
      }

      #endregion

      #region Properties

      #endregion

      #region Methods

      public bool CanExecute(object parameter)
      {
         if (_canExecute != null)
            return _canExecute();
         if (_canExecuteParam != null)
            return _canExecuteParam(this);

         return true;
      }

      public void Execute(object parameter)
      {
         _execute?.Invoke();
         _executeParam?.Invoke(parameter);
      }

      #endregion
   }
}
