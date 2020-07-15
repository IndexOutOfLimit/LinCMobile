using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using Cognizant.Hackathon.Mobile.Core.Helpers.MVVM;

namespace Cognizant.Hackathon.Mobile.Core.Infrastructure
{
    public class CustomDelegateTimerCommand<TParam> : DelegateCommand<TParam>
    {
        private readonly Func<TParam, bool> _validateMethod;

        private Timer _timer;
        private int _timerRetry = 0;
        private bool _stillProcessing = false;
        private bool _stillWaiting = false;
        private bool _isBusy;
        private Action _onBusy;
        public EventWaitHandle BackgroundTaskWaitHandle { get; set; }

        private Action<object> _showSpinner;
        private TimeSpan _elapsed;

        private const int INTERVAL = 250;
        private const int SPINNER_DUETIME = INTERVAL * 2;

        public CustomDelegateTimerCommand(Action<TParam> executeMethod, Func<TParam, bool> validateMethod, Action OnBusy = default(Action), Action<object> showSpinner = default(Action<object>)) : base(executeMethod)
        {
            BackgroundTaskWaitHandle = new EventWaitHandle(true, EventResetMode.ManualReset);
            _validateMethod = validateMethod;
            _onBusy = OnBusy;
            _showSpinner = showSpinner;
        }

        public CustomDelegateTimerCommand(Action<TParam> executeMethod, Func<TParam, bool> canExecuteMethod, Func<TParam, bool> validateMethod, Action OnBusy = default(Action), Action<object> showSpinner = default(Action<object>)) : base(executeMethod, canExecuteMethod)
        {
            BackgroundTaskWaitHandle = new EventWaitHandle(true, EventResetMode.ManualReset);
            _validateMethod = validateMethod;
            _onBusy = OnBusy;
            _showSpinner = showSpinner;
        }

        public new void Execute(object parameter)
        {
            if (Validate((TParam)parameter))
            {
                ResetTimer();
                _elapsed = new TimeSpan();
                _timer = new Timer(OnTick, parameter, 0, INTERVAL);
            }
        }

        public void Execute()
        {
            if (Validate(default(TParam)))
            {
                ResetTimer();
                _elapsed = new TimeSpan();
                _timer = new Timer(OnTick, null, 0, INTERVAL);
            }
        }

        private bool Validate(TParam parameter)
        {
            if (_validateMethod != null)
                return _validateMethod.Invoke(parameter);

            return false;
        }

        public void ResetTimer()
        {
            _timer?.Dispose();
            _timerRetry = 0;
            _stillProcessing = false;
            _stillWaiting = false;
            _isBusy = false;
            BackgroundTaskWaitHandle.Reset();
        }

        private void OnTick(object parameter)
        {
            _elapsed = _elapsed.Add(TimeSpan.FromMilliseconds(INTERVAL));

            if (_elapsed.Equals(TimeSpan.FromMilliseconds(SPINNER_DUETIME)))
            {
                Debug.WriteLine("SPINNER_DUETIME");
                _showSpinner?.Invoke(parameter);
            }

            if (_timerRetry > 0 && !_stillWaiting)
            {
                _stillWaiting = true;

                if (!_isBusy)
                    _onBusy?.Invoke();

                return;
            }

            _timerRetry++;

            if (_stillProcessing) return;
            _stillProcessing = true;

            base.Execute(parameter);
            BackgroundTaskWaitHandle.Set();
        }
    }

    public class CustomDelegateTimerCommand : DelegateCommand
    {
        private readonly Func<bool> _validateMethod;
        private Timer _timer;
        private int _timerRetry = 0;
        private bool _stillProcessing = false;
        private bool _stillWaiting = false;
        private bool _isBusy;
        private Action _onBusy;

        private Action<object> _showSpinner;
        private TimeSpan _elapsed;

        public EventWaitHandle BackgroundTaskWaitHandle { get; set; }

        private const int INTERVAL = 250;
        private const int SPINNER_DUETIME = INTERVAL * 2;

        public CustomDelegateTimerCommand(Action executeMethod, Func<bool> validateMethod, Action onBusy = default(Action), Action<object> showSpinner = default(Action<object>)) : base(executeMethod)
        {
            BackgroundTaskWaitHandle = new EventWaitHandle(true, EventResetMode.ManualReset);
            _validateMethod = validateMethod;
            _onBusy = onBusy;
            _showSpinner = showSpinner;
        }

        public CustomDelegateTimerCommand(Action executeMethod, Func<bool> canExecuteMethod, Func<bool> validateMethod, Action onBusy = default(Action), Action<object> showSpinner = default(Action<object>)) : base(executeMethod, canExecuteMethod)
        {
            BackgroundTaskWaitHandle = new EventWaitHandle(true, EventResetMode.ManualReset);
            _validateMethod = validateMethod;
            _onBusy = onBusy;
            _showSpinner = showSpinner;
        }

        protected override void Execute(object parameter)
        {
            if (Validate(parameter))
            {
                ResetTimer();
                _elapsed = new TimeSpan();
                _timer = new Timer(OnTick, parameter, 0, INTERVAL);
            }
        }

        public new void Execute()
        {
            if (Validate(null))
            {
                ResetTimer();
                _elapsed = new TimeSpan();
                _timer = new Timer(OnTick, null, 0, INTERVAL);
            }
        }

        private bool Validate(object parameter)
        {
            if (_validateMethod != null)
                return _validateMethod.Invoke();

            return false;
        }

        private void OnTick(object parameter)
        {
            _elapsed = _elapsed.Add(TimeSpan.FromMilliseconds(INTERVAL));

            if (_elapsed.Equals(TimeSpan.FromMilliseconds(SPINNER_DUETIME)))
            {
                _showSpinner?.Invoke(parameter);
            }

            if (_timerRetry > 0 && !_stillWaiting)
            {
                _stillWaiting = true;

                if (!_isBusy)
                    _onBusy?.Invoke();

                return;
            }

            _timerRetry++;

            if (_stillProcessing) return;
            _stillProcessing = true;

            base.Execute(parameter);
            BackgroundTaskWaitHandle.Set();
        }

        public void ResetTimer()
        {
            _timer?.Dispose();
            _timerRetry = 0;
            _stillProcessing = false;
            _stillWaiting = false;
            _isBusy = false;
            BackgroundTaskWaitHandle.Reset();
        }

        private static bool IsAsyncMethod(Type classType, string methodName)
        {
            // Obtain the method with the specified name.
            MethodInfo method = classType.GetMethod(methodName);

            Type attType = typeof(AsyncStateMachineAttribute);
                        
            var attrib = (AsyncStateMachineAttribute)method.GetCustomAttribute(attType);

            return (attrib != null);
        }
    }
}