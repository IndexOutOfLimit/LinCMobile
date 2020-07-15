using System;
using System.ComponentModel;
using System.Reflection;

namespace Cognizant.Hackathon.Mobile.Core.Helpers.MVVM
{
    /// <summary>
    /// Represents each node of nested properties expression and takes care of
    /// subscribing/unsubscribing INotifyPropertyChanged.PropertyChanged listeners on it.
    /// </summary>
    internal class PropertyObserverNode
    {
        private readonly Action _action;
        private INotifyPropertyChanged _inpcObject;

        public string PropertyName { get; }

        public PropertyObserverNode Next { get; set; }

        public PropertyObserverNode(string propertyName, Action action)
        {
            PropertyObserverNode propertyObserverNode = this;
            this.PropertyName = propertyName;

            this._action = (Action)(() =>
           {
               Action action1 = action;
               if (action1 != null)
                   action1();

               if (propertyObserverNode.Next == null)
                   return;

               propertyObserverNode.Next.UnsubscribeListener();
               propertyObserverNode.GenerateNextNode();
           });
        }

        public void SubscribeListenerFor(INotifyPropertyChanged inpcObject)
        {
            this._inpcObject = inpcObject;
            this._inpcObject.PropertyChanged += new PropertyChangedEventHandler(this.OnPropertyChanged);
            if (this.Next == null)
                return;
            this.GenerateNextNode();
        }

        private void GenerateNextNode()
        {
            switch (this._inpcObject.GetType().GetRuntimeProperty(this.PropertyName).GetValue((object)this._inpcObject))
            {
                case INotifyPropertyChanged inpcObject:
                    this.Next.SubscribeListenerFor(inpcObject);
                    break;
                case null:
                    break;
                default:
                    throw new InvalidOperationException("Trying to subscribe PropertyChanged listener in object that " + string.Format("owns '{0}' property, but the object does not implements INotifyPropertyChanged.", (object)this.Next.PropertyName));
            }
        }

        private void UnsubscribeListener()
        {
            if (this._inpcObject != null)
                this._inpcObject.PropertyChanged -= new PropertyChangedEventHandler(this.OnPropertyChanged);
            this.Next?.UnsubscribeListener();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(e?.PropertyName == this.PropertyName) && e != null && e.PropertyName != null)
                return;
            Action action = this._action;
            if (action == null)
                return;
            action();
        }
    }
}
