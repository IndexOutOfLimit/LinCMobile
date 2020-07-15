using System;
using System.Reflection;
using Cognizant.Hackathon.Mobile.Core.Interfaces;

namespace Cognizant.Hackathon.Mobile.Core.Services
{
    public class InitialiserService : IInitialiserService, IDisposable
    {
        private object _initialiser;

        public void SetInitialiser<TViewModel>(Action<TViewModel> initialiser)
        {
            _initialiser = initialiser;
        }

        public void Initialise<TViewModel>(TViewModel viewModel)
        {
            if (_initialiser == null || _initialiser.GetType().GenericTypeArguments[0] != viewModel.GetType()) return;

            Type generic = typeof(Action<>);
            Type[] typeArgs = { viewModel.GetType() };
            Type constructed = generic.MakeGenericType(typeArgs);
            MethodInfo methodInfo = constructed.GetMethod("Invoke");
            methodInfo.Invoke(_initialiser, new object[] { viewModel });

            _initialiser = null;
        }

        public void Dispose() { }
    }
}
