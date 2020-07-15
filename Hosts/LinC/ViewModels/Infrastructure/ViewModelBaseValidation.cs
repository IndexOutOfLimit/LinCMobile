using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cognizant.Hackathon.Core.Common.Helpers;
using Cognizant.Hackathon.Core.Interface;
using Cognizant.Hackathon.Mobile.Core.Exceptions;
using Cognizant.Hackathon.Mobile.Core.Helpers.MVVM;
using Cognizant.Hackathon.Mobile.Core.Infrastructure;
using Cognizant.Hackathon.Mobile.Core.Interfaces;
using Cognizant.Hackathon.Mobile.Core.Validation;
using Cognizant.Hackathon.Shared.Mobile.Bootstrap;
using Unity;

namespace LinC.ViewModels
{
    public abstract partial class ViewModelBase : ValidatableBindableBase
    {
        protected IPopupInputService AppPopupInputService => BootStrapper.Container.Resolve<IPopupInputService>();

        public IErrorService AppErrorService => BootStrapper.Container.Resolve<IErrorService>();

        protected ILogger AppLogger => BootStrapper.Container.Resolve<ILogger>();

        public bool IsValid { get; set; }

        /// <summary>
        /// Gets the on error command.
        /// </summary>
        public DelegateCommand<object> OnErrorCommand => new DelegateCommand<object>(OnError);

        protected void OnError(object param)
        {
            var error = (Exception)param;
            AppErrorService.AddError(error.Message, ViewModelError.ErrorAction.DisplayAndLog, ViewModelError.ErrorSeverity.Error);
        }

        protected virtual bool Validate(Dictionary<string, object> properties = null, Dictionary<string, string> propertiesDisplayName = null, List<Tuple<string, bool, string>> validators = null)
        {
            // clean any inline val property 
            AppErrorService.ClearInlineValidators();

            if (properties?.Count > 0)
            {
                if (ValidatePropertyList(properties, propertiesDisplayName, validators))
                    return true;
            }
            else
            {
                var isValid = ValidateProperties(null, validators);

                if (isValid)
                    return true;
            }

            string formattedErrors = "";
            var errors = GetAllErrors();

            // invalid but no errors: move on
            if (!errors.Any())
                return true;

            foreach (var key in errors.Keys)
            {
                // TODO we would need to replace the CamelCase filed key with a globalized name
                // in the meantime, we de-camel case
                //formattedErrors += $"\n\u2022 {key.SplitCamelCase()} : {string.Join("\n", errors[key])}";
                if (errors.Keys.Count == 1)
                {
                    formattedErrors = string.Join("\n", errors[errors.Keys.First()]);
                }
                else
                {
                    formattedErrors += $"\u2022 {string.Join("\n", errors[key])}\n";
                }

                // replace property name with display name
                if (propertiesDisplayName != null)
                    formattedErrors = propertiesDisplayName.Aggregate(formattedErrors, (current, propertyDisplayName) => current.Replace(propertyDisplayName.Key, propertyDisplayName.Value));

                // split camel case
                formattedErrors = formattedErrors.SplitCamelCase();
            }
            
            formattedErrors = formattedErrors.ReplaceLast("\n", "");

            AppErrorService.AddError(formattedErrors, ViewModelError.ErrorAction.Display, ViewModelError.ErrorSeverity.Warning, errors: errors);

            AppErrorService.ProcessErrors();

            return false;
        }

        protected void HandleUIError(Exception ex, bool hideSpinner = true, ViewModelError.ErrorAction action = ViewModelError.ErrorAction.DisplayAndLog)
        {
            string title = null;

            if (hideSpinner)
                AppSpinner.HideLoading();

            if (ex is HandledException exception && !exception.IsLog)
                action = ViewModelError.ErrorAction.Display;

            if (ex is HandledException exception2)
                title = exception2.Title;

            // hack: we just use this exception to surface messages to ui from any lib without including it
            if (ex is RankException)
                action = ViewModelError.ErrorAction.Display;

            AppErrorService.AddError(ex.Message, action, ViewModelError.ErrorSeverity.Error, title, ex: ex);
            AppErrorService.ProcessErrors();

        }
    }
}