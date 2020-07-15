using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Cognizant.Hackathon.Mobile.Core.Helpers.MVVM
{
    internal class PropertyObserver
    {
        private readonly Action _action;

        private PropertyObserver(Expression propertyExpression, Action action)
        {
            this._action = action;
            this.SubscribeListeners(propertyExpression);
        }

        private void SubscribeListeners(Expression propertyExpression)
        {
            Stack<string> stringStack = new Stack<string>();

            while (propertyExpression is MemberExpression memberExpression)
            {
                propertyExpression = memberExpression.Expression;
                stringStack.Push(memberExpression.Member.Name);
            }

            if (!(propertyExpression is ConstantExpression constantExpression))
                throw new NotSupportedException("Operation not supported for the given expression type. Only MemberExpression and ConstanteExpression are currently supported.");

            PropertyObserverNode propertyObserverNode1 = new PropertyObserverNode(stringStack.Pop(), this._action);
            PropertyObserverNode propertyObserverNode2 = propertyObserverNode1;

            foreach (string propertyName in stringStack)
            {
                PropertyObserverNode propertyObserverNode3 = new PropertyObserverNode(propertyName, this._action);
                propertyObserverNode2.Next = propertyObserverNode3;
                propertyObserverNode2 = propertyObserverNode3;
            }

            if (!(constantExpression.Value is INotifyPropertyChanged inpcObject))
                throw new InvalidOperationException("Trying to subscribe PropertyChanged listener in object that " + string.Format("owns '{0}' property, but the object does not implements INotifyPropertyChanged.", (object)propertyObserverNode1.PropertyName));

            propertyObserverNode1.SubscribeListenerFor(inpcObject);
        }
        
        internal static PropertyObserver Observes<T>(Expression<Func<T>> propertyExpression,
          Action action)
        {
            return new PropertyObserver(propertyExpression.Body, action);
        }
    }
}
