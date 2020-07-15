using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cognizant.Hackathon.Core.Common.Helpers
{
    /// <summary>
    ///     The property helper.
    /// </summary>
    public static class PropertyHelper
    {
        #region Static Fields

        /// <summary>
        ///     The _cache lock.
        /// </summary>
        private static readonly object _cacheLock = new object();

        /// <summary>
        ///     The _cached entries.
        /// </summary>
        private static readonly Dictionary<Type, Dictionary<string, PropertyInfo>> _cachedEntries =
            new Dictionary<Type, Dictionary<string, PropertyInfo>>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get property descriptor.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The <see cref="PropertyInfo"/>.
        /// </returns>
        public static PropertyInfo GetPropertyDescriptor(string propertyName, object obj)
        {
            return obj == null ? null : GetPropertyDescriptor(propertyName, obj.GetType());
        }


        public static object ReadField(this object obj, string fieldName)
        {
            FieldInfo field = obj.GetType().GetRuntimeField(fieldName);
            return field.GetValue(obj);
        }

        public static string[] GetFieldNames(Type type)
        {
            var fieldNames = new List<string>();
            var fields = type.GetRuntimeFields();

            foreach (var field in fields)
                fieldNames.Add(field.Name);

            return fieldNames.ToArray();
        }

        /// <summary>
        /// The get property descriptor.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="PropertyInfo"/>.
        /// </returns>
        public static PropertyInfo GetPropertyDescriptor(string propertyName, Type type)
        {
            lock (_cacheLock)
            {
                if (_cachedEntries.ContainsKey(type) == false)
                {
                    IEnumerable<PropertyInfo> descriptors = type.GetProperties();
                    _cachedEntries[type] = new Dictionary<string, PropertyInfo>();
                    foreach (PropertyInfo descriptor in descriptors)
                    {
                        _cachedEntries[type][descriptor.Name] = descriptor;
                    }
                }
            }

            Dictionary<string, PropertyInfo> entry = _cachedEntries[type];
            if (entry.ContainsKey(propertyName) == false)
            {
                return null;
            }

            return entry[propertyName];
        }

        /// <summary>
        /// The get property names.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="string[]"/>.
        /// </returns>
        public static string[] GetPropertyNames(Type type)
        {
            var propertyNames = new List<string>();
            IEnumerable<PropertyInfo> properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                propertyNames.Add(property.Name);
            }

            return propertyNames.ToArray();
        }

        /// <summary>
        /// The read property.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object ReadProperty(this object obj, string propertyName)
        {
            return ReadProperty(propertyName, obj);
        }

        /// <summary>
        /// The read property.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object ReadProperty(string propertyName, object obj)
        {
            PropertyInfo property = GetPropertyDescriptor(propertyName, obj);
            return (property == null) ? null : property.GetValue(obj);
        }

        /// <summary>
        /// The read static property.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object ReadStaticProperty<T>(string propertyName)
        {
            PropertyInfo property = typeof(T).GetRuntimeProperty(propertyName);
            return (property == null) ? null : property.GetValue(null, null);
        }

        /// <summary>
        /// The set property.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public static void SetProperty(string propertyName, object obj, object value)
        {
            Type type = obj.GetType();

            lock (_cacheLock)
            {
                if (_cachedEntries.ContainsKey(type) == false)
                {
                    IEnumerable<PropertyInfo> descriptors = type.GetRuntimeProperties();
                    _cachedEntries[type] = new Dictionary<string, PropertyInfo>();
                    foreach (PropertyInfo descriptor in descriptors)
                    {
                        _cachedEntries[type][descriptor.Name] = descriptor;
                    }
                }
            }

            if (_cachedEntries[type].ContainsKey(propertyName))
            {
                _cachedEntries[type][propertyName].SetValue(obj, value);
            }
        }

        public static void SetProperty(this object obj, string propertyName, object value)
        {
            Type type = obj.GetType();

            lock (_cacheLock)
            {
                if (_cachedEntries.ContainsKey(type) == false)
                {
                    IEnumerable<PropertyInfo> descriptors = type.GetProperties();
                    _cachedEntries[type] = new Dictionary<string, PropertyInfo>();
                    foreach (PropertyInfo descriptor in descriptors)
                    {
                        _cachedEntries[type][descriptor.Name] = descriptor;
                    }
                }
            }

            if (_cachedEntries[type].ContainsKey(propertyName))
            {
                _cachedEntries[type][propertyName].SetValue(obj, value);
            }
        }

        #endregion
    }
}