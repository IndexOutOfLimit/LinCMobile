using System;
using System.Collections.Generic;

namespace Cognizant.Hackathon.Mobile.Core.Infrastructure
{
    public class ErrorAlert
    {
        public ErrorAlert()
        {
        }

        #region Public Properties

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the error items. ErrorItemMessage, PropertyName (optional)
        /// when doing validation, add error/property Pair
        /// As to allow highlkighting form elemets that are invalid
        /// </summary>
        public Dictionary<string, string> ErrorItems { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the view model type.
        /// </summary>
        public Type ViewModelType { get; set; }

        /// <summary>
        /// altert style
        /// </summary>
        public ViewModelError.ErrorSeverity Severity { get; set; }

        #endregion
    }
}
