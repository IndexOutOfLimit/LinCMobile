using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Windows.Input;

namespace Cognizant.Hackathon.Mobile.Core.Helpers.MVVM
{
    public abstract class DelegateCommandBase : ICommand
    {
        private readonly HashSet<string> _observedPropertiesExpressions = new HashSet<string>();
        private bool _isActive;
        private SynchronizationContext _synchronizationContext;

        protected DelegateCommandBase()
        {
            this._synchronizationContext = SynchronizationContext.Current;
        }

        public virtual event EventHandler CanExecuteChanged;

        protected virtual void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChanged;
            if (handler == null)
                return;

            if (this._synchronizationContext != null && this._synchronizationContext != SynchronizationContext.Current)
                this._synchronizationContext.Post((SendOrPostCallback)(o => handler((object)this, EventArgs.Empty)), (object)null);
            else
                handler((object)this, EventArgs.Empty);
        }

        public void RaiseCanExecuteChanged()
        {
            this.OnCanExecuteChanged();
        }

        void ICommand.Execute(object parameter)
        {
            this.Execute(parameter);
        }

        bool ICommand.CanExecute(object parameter)
        {
            return this.CanExecute(parameter);
        }

        protected abstract void Execute(object parameter);

        protected abstract bool CanExecute(object parameter);

        protected internal void ObservesPropertyInternal<T>(Expression<Func<T>> propertyExpression)
        {
            if (this._observedPropertiesExpressions.Contains(propertyExpression.ToString()))
                throw new ArgumentException(string.Format("{0} is already being observed.", (object)propertyExpression.ToString()), nameof(propertyExpression));

            this._observedPropertiesExpressions.Add(propertyExpression.ToString());
            PropertyObserver.Observes<T>(propertyExpression, new Action(this.RaiseCanExecuteChanged));
        }

       public bool IsActive
        {
            get
            {
                return this._isActive;
            }
            set
            {
                if (this._isActive == value)
                    return;

                this._isActive = value;
                this.OnIsActiveChanged();
            }
        }

        public virtual event EventHandler IsActiveChanged;

       protected virtual void OnIsActiveChanged()
        {

            EventHandler isActiveChanged = this.IsActiveChanged;
            if (isActiveChanged == null)
                return;

            isActiveChanged((object)this, EventArgs.Empty);
        }
    }
}
