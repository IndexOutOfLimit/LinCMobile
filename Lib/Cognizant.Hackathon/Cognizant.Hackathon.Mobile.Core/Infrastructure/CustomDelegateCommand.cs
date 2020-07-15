using Cognizant.Hackathon.Mobile.Core.Helpers.MVVM;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Cognizant.Hackathon.Mobile.Core.Infrastructure
{
    public class CustomDelegateCommand<TParam> : DelegateCommand<TParam>
    {
        private readonly Func<TParam, bool> _validateMethod;
        public EventWaitHandle BackgroundTaskWaitHandle { get; set; }

        public CustomDelegateCommand(Action<TParam> executeMethod, Func<TParam, bool> validateMethod) : base(executeMethod)
        {
            BackgroundTaskWaitHandle = new EventWaitHandle(true, EventResetMode.ManualReset);
            _validateMethod = validateMethod;
        }

        public CustomDelegateCommand(Action<TParam> executeMethod, Func<TParam, bool> canExecuteMethod, Func<TParam, bool> validateMethod) : base(executeMethod, canExecuteMethod)
        {
            BackgroundTaskWaitHandle = new EventWaitHandle(true, EventResetMode.ManualReset);
            _validateMethod = validateMethod;
        }

        public new void Execute(object parameter)
        {
            if (Validate((TParam) parameter))
            {
                BackgroundTaskWaitHandle.Reset();

                try
                {
                    base.Execute(parameter);
                }
                finally
                {
                    BackgroundTaskWaitHandle.Set();
                }
            }
        }

        protected void Execute()
        {
            if (Validate(default(TParam)))
            {
                BackgroundTaskWaitHandle.Reset();

                try
                {
                    base.Execute(null);
                }
                finally
                {
                    BackgroundTaskWaitHandle.Set();
                }
            }
        }

        private bool Validate(TParam parameter)
        {
            if(_validateMethod != null)
                return _validateMethod.Invoke(parameter);

            return false;
        }
    }

    public class CustomDelegateCommand : DelegateCommand
    {
        private readonly Func<bool> _validateMethod;
        public EventWaitHandle BackgroundTaskWaitHandle { get; set; }

        public CustomDelegateCommand(Action executeMethod, Func<bool> validateMethod) : base(executeMethod)
        {
            BackgroundTaskWaitHandle = new EventWaitHandle(true, EventResetMode.ManualReset);
            _validateMethod = validateMethod;
        }

        public CustomDelegateCommand(Action executeMethod, Func<bool> canExecuteMethod, Func<bool> validateMethod) : base(executeMethod, canExecuteMethod)
        {
            BackgroundTaskWaitHandle = new EventWaitHandle(true, EventResetMode.ManualReset);
            _validateMethod = validateMethod;
        }

        protected new void Execute(object parameter)
        {
            if (Validate(parameter))
            {
                BackgroundTaskWaitHandle.Reset();

                try
                {
                    base.Execute(parameter);
                }
                finally
                {
                    BackgroundTaskWaitHandle.Set();
                }
            }
        }

        public new void Execute()
        {
            if (Validate(null))
            {
                BackgroundTaskWaitHandle.Reset();

                try
                {
                    base.Execute();
                }
                finally
                {
                    BackgroundTaskWaitHandle.Set();
                }
            }
        }

        private bool Validate(object parameter)
        {
            if (_validateMethod != null)
                return _validateMethod.Invoke();

            return false;
        }

        private static bool IsAsyncMethod(Type classType, string methodName)
        {
            // Obtain the method with the specified name.
            MethodInfo method = classType.GetMethod(methodName);

            Type attType = typeof(AsyncStateMachineAttribute);

            // Obtain the custom attribute for the method. 
            // The value returned contains the StateMachineType property. 
            // Null is returned if the attribute isn't present for the method. 
            var attrib = (AsyncStateMachineAttribute)method.GetCustomAttribute(attType);

            return (attrib != null);
        }
    }
}