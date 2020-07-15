using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Cognizant.Hackathon.Core.Common.Helpers;

namespace Cognizant.Hackathon.Mobile.Core.Infrastructure
{
    public class ViewModelError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelError"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="errorAction">The error action.</param>
        /// <param name="errorSeverity">The error severity.</param>
        /// <param name="errorItems">The error items.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="viewModelType"></param>
        /// <param name="errors"></param>
        public ViewModelError(string title, string description, ErrorAction errorAction,
           ErrorSeverity errorSeverity, List<string> errorItems = null, Exception exception = null, string viewModelType = "", ReadOnlyDictionary<string, ReadOnlyCollection<string>> errors = null)
        {
            if (exception != null && exception.HasInnerExceptions())
                description = exception.Message == description ? exception.GetInnermostExceptionMessage() : description;

            Title = title;
            Description = description;
            Action = errorAction;
            Severity = errorSeverity;
            ErrorItems = errorItems;
            Exception = exception;
            ViewModelType = viewModelType;
            Errors = errors;
        }

        #region Enums
        /// <summary>
        /// Error Action Enum (Display, Log, DisplayAndLog, None)
        /// </summary>
        public enum ErrorAction
        {
            Display,
            Log,
            DisplayAndLog,
            None
        }

        /// <summary>
        /// Error Severity Enum (Info, Warning, Error, UnhandledError)
        /// </summary>
        public enum ErrorSeverity
        {
            Info,
            Warning,
            Error
        } 
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the Error title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the Error description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the error items.
        /// </summary>
        public List<string> ErrorItems { get; set; }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets the error action.
        /// </summary>
        public ErrorAction Action { get; set; }

        /// <summary>
        /// Gets or sets the error severity.
        /// </summary>
        public ErrorSeverity Severity { get; set; }

        /// <summary>
        /// Gets or sets the ViewModelType
        /// </summary>
        public string ViewModelType { get; set; }

        public ReadOnlyDictionary<string, ReadOnlyCollection<string>> Errors { get; set; }

        #endregion

    }
}
