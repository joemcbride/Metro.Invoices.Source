namespace Metro.Framework
{
	using System;
	using System.Windows.Input;

	public class DelegateCommand : ICommand
	{
		Func<object, bool> canExecute;
		Action<object> executeAction;
		bool canExecuteCache;

		public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecute = null)
		{
			this.executeAction = executeAction;
			this.canExecute = canExecute;
		}

		public event EventHandler CanExecuteChanged = delegate { };

		public bool CanExecute(object parameter)
		{
			bool temp = true;

			if (canExecute != null)
			{
				temp = canExecute(parameter);
			}

			if (canExecuteCache != temp)
			{
				canExecuteCache = temp;
				if (CanExecuteChanged != null)
				{
					CanExecuteChanged(this, new EventArgs());
				}
			}

			return canExecuteCache;
		}

		public void Execute(object parameter)
		{
			executeAction(parameter);
		}
	}

	public class DelegateCommand<T> : ICommand
	{
		Func<T, bool> canExecute;
		Action<T> executeAction;
		bool canExecuteCache;

		public DelegateCommand(Action<T> executeAction, Func<T, bool> canExecute = null)
		{
			this.executeAction = executeAction;
			this.canExecute = canExecute;
		}

		public event EventHandler CanExecuteChanged = delegate { };

		public bool CanExecute(object parameter)
		{
			bool temp = true;

			if (canExecute != null)
			{
				temp = canExecute((T)parameter);
			}

			if (canExecuteCache != temp)
			{
				canExecuteCache = temp;
				if (CanExecuteChanged != null)
				{
					CanExecuteChanged(this, new EventArgs());
				}
			}

			return canExecuteCache;
		}

		public void Execute(object parameter)
		{
			executeAction((T)parameter);
		}
	}
}