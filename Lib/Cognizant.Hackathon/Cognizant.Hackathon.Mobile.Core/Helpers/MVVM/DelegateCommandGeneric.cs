using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Cognizant.Hackathon.Mobile.Core.Helpers.MVVM
{
    public class DelegateCommand<T> : DelegateCommandBase
    {
        private readonly Action<T> _executeMethod;
        private Func<T, bool> _canExecuteMethod;

        public DelegateCommand(Action<T> executeMethod) : this(executeMethod, (Func<T, bool>)(o => true))
        {
        }

        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException(nameof(executeMethod), "DelegateCommandDelegatesCannotBeNull");

            TypeInfo typeInfo = typeof(T).GetTypeInfo();

            if (typeInfo.IsValueType && (!typeInfo.IsGenericType || !typeof(Nullable<>).GetTypeInfo().IsAssignableFrom(typeInfo.GetGenericTypeDefinition().GetTypeInfo())))
                throw new InvalidCastException("DelegateCommandInvalidGenericPayloadType");

            this._executeMethod = executeMethod;
            this._canExecuteMethod = canExecuteMethod;
        }

        public void Execute(T parameter)
        {
            this._executeMethod(parameter);
        }

        public bool CanExecute(T parameter)
        {
            return this._canExecuteMethod(parameter);
        }

        protected override void Execute(object parameter)
        {
            this.Execute((T)parameter);
        }

        protected override bool CanExecute(object parameter)
        {
            return this.CanExecute((T)parameter);
        }

        public DelegateCommand<T> ObservesProperty<TType>(Expression<Func<TType>> propertyExpression)
        {
            this.ObservesPropertyInternal<TType>(propertyExpression);
            return this;
        }

        public DelegateCommand<T> ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
        {
            this._canExecuteMethod = Expression.Lambda<Func<T, bool>>(canExecuteExpression.Body, Expression.Parameter(typeof(T), "o")).Compile();
            this.ObservesPropertyInternal<bool>(canExecuteExpression);
            return this;
        }
    }
}
