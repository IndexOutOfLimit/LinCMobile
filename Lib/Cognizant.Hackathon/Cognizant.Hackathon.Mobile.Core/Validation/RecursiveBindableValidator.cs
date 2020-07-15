using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Cognizant.Hackathon.Core.Common.Helpers;
//using ReflectionExtensions = Cognizant.Hackathon.Core.Common.Helpers.ReflectionExtensions;

namespace Cognizant.Hackathon.Mobile.Core.Validation
{
    public class RecursiveBindableValidator : INotifyPropertyChanged
    {
        public static readonly ReadOnlyCollection<string> EmptyErrorsCollection =
            new ReadOnlyCollection<string>((IList<string>) new List<string>());

        private IDictionary<string, ReadOnlyCollection<string>> _errors =
            (IDictionary<string, ReadOnlyCollection<string>>) new Dictionary<string, ReadOnlyCollection<string>>();

        private readonly INotifyPropertyChanged _entityToValidate;
        private Func<string, string, string> _getResourceDelegate;

        public ReadOnlyCollection<string> this[string propertyName]
        {
            get
            {
                if (!this._errors.ContainsKey(propertyName))
                    return EmptyErrorsCollection;
                return this._errors[propertyName];
            }
        }

        public IDictionary<string, ReadOnlyCollection<string>> Errors => this._errors;

        public bool IsValidationEnabled { get; set; }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public RecursiveBindableValidator(INotifyPropertyChanged entityToValidate,
            Func<string, string, string> getResourceDelegate)
            : this(entityToValidate)
        {
            this._getResourceDelegate = getResourceDelegate;
        }

        public RecursiveBindableValidator(INotifyPropertyChanged entityToValidate)
        {
            if (entityToValidate == null)
                throw new ArgumentNullException(nameof(entityToValidate));
            this._entityToValidate = entityToValidate;
            this.IsValidationEnabled = true;
        }

        public ReadOnlyDictionary<string, ReadOnlyCollection<string>> GetAllErrors()
        {
            return new ReadOnlyDictionary<string, ReadOnlyCollection<string>>(this._errors);
        }

        public void SetAllErrors(IDictionary<string, ReadOnlyCollection<string>> entityErrors)
        {
            if (entityErrors == null)
                throw new ArgumentNullException(nameof(entityErrors));
            this._errors.Clear();
            foreach (
                KeyValuePair<string, ReadOnlyCollection<string>> entityError in
                (IEnumerable<KeyValuePair<string, ReadOnlyCollection<string>>>) entityErrors)
                this.SetPropertyErrors(entityError.Key, (IList<string>) entityError.Value);
            this.OnPropertyChanged("Item[]");
            this.OnErrorsChanged(string.Empty);
        }

        public bool ValidateProperty(string propertyName, object entityToValidate = null)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            if (entityToValidate == null)
                entityToValidate = _entityToValidate;

            PropertyInfo runtimeProperty = entityToValidate.GetType().GetRuntimeProperty(propertyName);

            if (runtimeProperty == null)
                throw new ArgumentException("InvalidPropertyNameException", propertyName);

            List<string> propertyErrors = new List<string>();

            bool flag = this.TryValidateProperty(runtimeProperty, propertyErrors, entityToValidate);

            if (this.SetPropertyErrors(runtimeProperty.Name, (IList<string>) propertyErrors))
            {
                this.OnErrorsChanged(propertyName);
                this.OnPropertyChanged(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Item[{0}]",
                    (object) propertyName));
            }

            return flag;
        }

        public bool ValidateProperties(object entityToValidate = null, List<Tuple<string, bool, string>> validators = null)
        {
            List<string> stringList = new List<string>();

            this._errors.Clear();

            if (entityToValidate == null)
                entityToValidate = _entityToValidate;

            ValidatePropertiesRecursive(entityToValidate, stringList);

            if (validators != null)
            {
                foreach (var validator in validators)
                {
                    if (validator.Item2) continue;

                    List<string> propertyErrors = new List<string>();
                    propertyErrors.Add(validator.Item3);

                    if (SetPropertyErrors(validator.Item1, propertyErrors) && !stringList.Contains(validator.Item1))
                        stringList.Add(validator.Item1);
                }
            }

            foreach (var propertyName in stringList)
            {
                OnErrorsChanged(propertyName);
                OnPropertyChanged(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Item[{0}]", propertyName));
            }

            return this._errors.Values.Count == 0;
        }

        public bool ValidatePropertyList(Dictionary<string, object> properties, List<Tuple<string, bool,string>> validators)
        {
            List<string> stringList = new List<string>();

            this._errors.Clear();

            foreach (var property in properties)
            {
                var fullPropertyName = property.Key;

                var propertyName = fullPropertyName.Split('.').LastOrDefault();

                List<string> propertyErrors = new List<string>();
              
                var propertyInfo = property.Value.GetType().GetProperty(propertyName);

                TryValidateProperty(propertyInfo, propertyErrors, property.Value, fullPropertyName);

                if (SetPropertyErrors(fullPropertyName, propertyErrors) && !stringList.Contains(fullPropertyName))
                    stringList.Add(fullPropertyName);
            }

            foreach (var validator in validators)
            {
                if (validator.Item2) continue;

                List<string> propertyErrors = new List<string>();
                propertyErrors.Add(validator.Item3);

                if (SetPropertyErrors(validator.Item1, propertyErrors) && !stringList.Contains(validator.Item1))
                    stringList.Add(validator.Item1);
            }

            foreach (var propertyName in stringList)
            {
                OnErrorsChanged(propertyName);
                OnPropertyChanged(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "Item[{0}]", propertyName));
            }
            return this._errors.Values.Count == 0;
        }

        private List<string> ValidatePropertiesRecursive(object instance, List<string> stringList, int level = 4)
        {
            if (level == 0)
                return stringList;

            if (instance is null)
                return default(List<string>);

            var props = instance.GetType().GetProperties(ReflectionExtensions.BindingFlags.DeclaredOnly | ReflectionExtensions.BindingFlags.Public | ReflectionExtensions.BindingFlags.Instance)
                .Where<PropertyInfo>((Func<PropertyInfo, bool>)
                        (c => c.GetCustomAttributes().Any<Attribute>((Func<Attribute, bool>)
                                    (a => a.GetType().GetTypeInfo().BaseType.Name == nameof(ValidationAttribute))))).ToList();

            foreach (PropertyInfo propertyInfo in props)
            {
                List<string> propertyErrors = new List<string>();

                object propValue = propertyInfo.GetValue(instance, null);

                if (propertyInfo.PropertyType.GetTypeInfo().IsPrimitive ||
                            new Type[] {typeof(Enum),typeof(String),typeof(Decimal),typeof(DateTime),typeof(DateTimeOffset),typeof(TimeSpan),typeof(Guid)}.Contains(propertyInfo.PropertyType) ||
                            (propertyInfo.PropertyType.GetTypeInfo().IsGenericType && propertyInfo.PropertyType.GetTypeInfo().GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    TryValidateProperty(propertyInfo, propertyErrors, instance);

                    if (SetPropertyErrors(propertyInfo.Name, propertyErrors) && !stringList.Contains(propertyInfo.Name))
                        stringList.Add(propertyInfo.Name);
                }
                else
                {
                   ValidatePropertiesRecursive(propValue, stringList, level - 1);
                }
            }

            return stringList;
        }

        private bool TryValidateProperty(PropertyInfo propertyInfo, List<string> propertyErrors, object entityToValidate = null, string fullPropertyName = null)
        {
            List<ValidationResult> source = new List<ValidationResult>();

            if (entityToValidate == null)
                entityToValidate = _entityToValidate;

            ValidationContext validationContext = new ValidationContext(entityToValidate)
            {
                MemberName = propertyInfo.Name
            };

            int num = Validator.TryValidateProperty(propertyInfo.GetValue(entityToValidate),
                validationContext, source) ? 1 : 0;

            if (!source.Any())
                return num != 0;

            propertyErrors.AddRange(
                source.Select(c => c.ErrorMessage.Replace(propertyInfo.Name, fullPropertyName ?? propertyInfo.Name)));

            return num != 0;
        }

        private bool SetPropertyErrors(string propertyName, IList<string> propertyNewErrors)
        {
            bool flag = false;
            if (!this._errors.ContainsKey(propertyName))
            {
                if (propertyNewErrors.Count > 0)
                {
                    this._errors.Add(propertyName, new ReadOnlyCollection<string>(propertyNewErrors));
                    flag = true;
                }
            }
            else if (propertyNewErrors.Count != this._errors[propertyName].Count ||
                     this._errors[propertyName].Intersect<string>((IEnumerable<string>) propertyNewErrors).Count<string>() !=
                     propertyNewErrors.Count)
            {
                if (propertyNewErrors.Count > 0)
                    this._errors[propertyName] = new ReadOnlyCollection<string>(propertyNewErrors);
                else
                    this._errors.Remove(propertyName);
                flag = true;
            }
            return flag;
        }

        private void OnPropertyChanged(string propertyName)
        {
            // ISSUE: reference to a compiler-generated field
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged == null)
                return;
            PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
            propertyChanged((object) this, e);
        }

        private void OnErrorsChanged(string propertyName)
        {
            if (!this.IsValidationEnabled)
                return;
            // ISSUE: reference to a compiler-generated field
            EventHandler<DataErrorsChangedEventArgs> errorsChanged = this.ErrorsChanged;
            if (errorsChanged == null)
                return;
            DataErrorsChangedEventArgs e = new DataErrorsChangedEventArgs(propertyName);
            errorsChanged((object) this, e);
        }
    }
}
