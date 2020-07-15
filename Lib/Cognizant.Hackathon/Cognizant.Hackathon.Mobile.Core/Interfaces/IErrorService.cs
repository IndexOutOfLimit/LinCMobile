using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Cognizant.Hackathon.Mobile.Core.Infrastructure;
using Cognizant.Hackathon.RestClient.Models;

namespace Cognizant.Hackathon.Mobile.Core.Interfaces
{
    public interface IErrorService
    {
        List<ViewModelError> AddError(string description, ViewModelError.ErrorAction action, ViewModelError.ErrorSeverity severity, string title = null, List<string> errorItems = null, Exception ex = null, string viewModelType = null, ReadOnlyDictionary<string, ReadOnlyCollection<string>> errors = null);
        List<ViewModelError> AddError<T>(ServiceResponse<T> serviceResponse);
        List<ViewModelError> GetErrors();
        void ProcessErrors();
        List<ViewModelError> TrashCan { get; }
        void ClearInlineValidators();
    }
}