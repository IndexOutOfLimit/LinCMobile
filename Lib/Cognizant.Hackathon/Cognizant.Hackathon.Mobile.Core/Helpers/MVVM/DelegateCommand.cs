using System;
using System.Linq.Expressions;

namespace Cognizant.Hackathon.Mobile.Core.Helpers.MVVM
{   
    public class DelegateCommand : DelegateCommandBase
    {
        private Action _executeMethod;
        private Func<bool> _canExecuteMethod;

        public DelegateCommand(Action executeMethod)  : this(executeMethod, (Func<bool>)(() => true))
        {
        }

      public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException(nameof(executeMethod), new Exception("DelegateCommandDelegatesCannotBeNull"));

            this._executeMethod = executeMethod;
            this._canExecuteMethod = canExecuteMethod;
        }

        public void Execute()
        {
            this._executeMethod();
        }

        public bool CanExecute()
        {
            return this._canExecuteMethod();
        }

        protected override void Execute(object parameter)
        {
            this.Execute();
        }

        protected override bool CanExecute(object parameter)
        {
            return this.CanExecute();
        }

        public DelegateCommand ObservesProperty<T>(Expression<Func<T>> propertyExpression)
        {
            this.ObservesPropertyInternal<T>(propertyExpression);
            return this;
        }

       public DelegateCommand ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
        {
            this._canExecuteMethod = canExecuteExpression.Compile();
            this.ObservesPropertyInternal<bool>(canExecuteExpression);
            return this;
        }
    }
}
