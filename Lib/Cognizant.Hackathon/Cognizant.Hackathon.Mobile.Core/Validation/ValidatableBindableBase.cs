using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Cognizant.Hackathon.Mobile.Core.Validation
{
    public class ValidatableBindableBase : BindableBase, IValidatableBindableBase, INotifyPropertyChanged, INotifyDataErrorInfo
    {
        protected readonly RecursiveBindableValidator _bindableValidator;

        [JsonIgnore]
        [NotMapped]
        public bool IsValidationEnabled
        {
            get
            {
                return this._bindableValidator.IsValidationEnabled;
            }
            set
            {
                this._bindableValidator.IsValidationEnabled = value;
            }
        }

        [JsonIgnore]
        [NotMapped]
        public RecursiveBindableValidator Errors => this._bindableValidator;

        [JsonIgnore]
        [NotMapped]
        public bool HasErrors => !this.ValidateProperties();
        
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged
        {
            add
            {
                this._bindableValidator.ErrorsChanged += value;
            }
            remove
            {
                this._bindableValidator.ErrorsChanged -= value;
            }
        }

        public ValidatableBindableBase()
        {
            this._bindableValidator = new RecursiveBindableValidator((INotifyPropertyChanged)this);
        }

        public ReadOnlyDictionary<string, ReadOnlyCollection<string>> GetAllErrors()
        {
            return this._bindableValidator.GetAllErrors();
        }

        public bool ValidateProperties(object entityToValidate = null, List<Tuple<string, bool, string>> validators = null)
        {
            return this._bindableValidator.ValidateProperties(entityToValidate, validators);
        }

        public bool ValidatePropertyList(Dictionary<string, object> properties)
        {
            return ValidatePropertyList(properties, null, null);
        }

        public bool ValidatePropertyList(Dictionary<string, object> properties, Dictionary<string, string> propertiesDisplayName, List<Tuple<string, bool, string>> validators = null)
        {
            return this._bindableValidator.ValidatePropertyList(properties, validators);
        }

        public bool ValidateProperty(string propertyName, object entityToValidate = null)
        {
            if (this._bindableValidator.IsValidationEnabled)
                return this._bindableValidator.ValidateProperty(propertyName, entityToValidate);
            return true;
        }

        public void SetAllErrors(IDictionary<string, ReadOnlyCollection<string>> entityErrors)
        {
            this._bindableValidator.SetAllErrors(entityErrors);
        }

        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            int num = base.SetProperty<T>(ref storage, value, propertyName) ? 1 : 0;
            if (num == 0)
                return num != 0;
            if (string.IsNullOrEmpty(propertyName))
                return num != 0;
            if (!this._bindableValidator.IsValidationEnabled)
                return num != 0;
            this._bindableValidator.ValidateProperty(propertyName);
            return num != 0;
        }

        protected bool SetPropertySafe<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            int num = SetPropertySafeInternal<T>(ref storage, value, propertyName) ? 1 : 0;
            if (num == 0)
                return num != 0;
            if (string.IsNullOrEmpty(propertyName))
                return num != 0;
            if (!this._bindableValidator.IsValidationEnabled)
                return num != 0;
            this._bindableValidator.ValidateProperty(propertyName);
            return num != 0;
        }

        private bool SetPropertySafeInternal<T>(ref T storage, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return false;
            storage = value;
            RaisePropertyChangedSafeInternal(propertyName);
            return true;
        }

        /// <summary>Raises this object's PropertyChanged event.</summary>
        /// <param name="propertyName">Name of the property used to notify listeners. This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="T:System.Runtime.CompilerServices.CallerMemberNameAttribute" />.</param>
        protected void RaisePropertyChangedSafeInternal(string propertyName)
        {
            try
            {
                this.RaisePropertyChanged(propertyName);
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("FFImageLoading"))
                    throw;
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (!this.HasErrors)
                return (IEnumerable)Enumerable.Empty<string>();
            return (IEnumerable)this._bindableValidator[propertyName];
        }
    }
}