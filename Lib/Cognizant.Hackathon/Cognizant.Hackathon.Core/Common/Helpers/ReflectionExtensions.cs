using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
//using Cognizant.Hackathon.Core.Model.Attributes;
using Cognizant.Hackathon.Core.Model.Internal;

namespace Cognizant.Hackathon.Core.Common.Helpers
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<Type> GetTypes(this Assembly assembly)
        {
            return assembly.DefinedTypes.Select(t => t.AsType());
        }

        public static EventInfo GetEvent(this Type type, string name)
        {
            return type.GetRuntimeEvent(name);
        }

        public static IEnumerable<Type> GetInterfaces(this Type type)
        {
            return type.GetTypeInfo().ImplementedInterfaces;
        }

        public static IEnumerable<ConstructorInfo> GetConstructors(this Type type)
        {
            return type.GetTypeInfo().DeclaredConstructors.Where(c => c.IsPublic);
        }

        public static bool IsInstanceOfType(this Type type, object obj)
        {
            return type.IsAssignableFrom(obj.GetType());
        }

        public static MethodInfo GetAddMethod(this EventInfo eventInfo, bool nonPublic = false)
        {
            if (eventInfo.AddMethod == null || (!nonPublic && !eventInfo.AddMethod.IsPublic))
            {
                return null;
            }

            return eventInfo.AddMethod;
        }

        public static MethodInfo GetRemoveMethod(this EventInfo eventInfo, bool nonPublic = false)
        {
            if (eventInfo.RemoveMethod == null || (!nonPublic && !eventInfo.RemoveMethod.IsPublic))
            {
                return null;
            }

            return eventInfo.RemoveMethod;
        }

        public static MethodInfo GetGetMethod(this PropertyInfo property, bool nonPublic = false)
        {
            if (property.GetMethod == null || (!nonPublic && !property.GetMethod.IsPublic))
            {
                return null;
            }

            return property.GetMethod;
        }

        public static MethodInfo GetSetMethod(this PropertyInfo property, bool nonPublic = false)
        {
            if (property.SetMethod == null || (!nonPublic && !property.SetMethod.IsPublic))
            {
                return null;
            }

            return property.SetMethod;
        }

        public static IEnumerable<PropertyInfo> GetProperties(this Type type)
        {
            return GetProperties(type, BindingFlags.Public); //BindingFlags.FlattenHierarchy |
        }

        public static IEnumerable<PropertyInfo> GetProperties(this Type type, BindingFlags flags)
        {
            var properties = type.GetTypeInfo().DeclaredProperties;
            if ((flags & BindingFlags.FlattenHierarchy) == BindingFlags.FlattenHierarchy)
            {
                properties = type.GetRuntimeProperties();
            }

            return from property in properties
                   let getMethod = property.GetMethod
                   where getMethod != null
                   where (flags & BindingFlags.Public) != BindingFlags.Public || getMethod.IsPublic
                   where (flags & BindingFlags.Instance) != BindingFlags.Instance || !getMethod.IsStatic
                   where (flags & BindingFlags.Static) != BindingFlags.Static || getMethod.IsStatic
                   select property;
        }

        public static PropertyInfo GetProperty(this Type type, string name, BindingFlags flags)
        {
            return GetProperties(type, flags).FirstOrDefault(p => p.Name == name);
        }
        
        public static IEnumerable<MethodInfo> GetMethods(this Type type)
        {
            return GetMethods(type, BindingFlags.FlattenHierarchy | BindingFlags.Public);
        }

        public static IEnumerable<MethodInfo> GetMethods(this Type type, BindingFlags flags)
        {
            var properties = type.GetTypeInfo().DeclaredMethods;
            if ((flags & BindingFlags.FlattenHierarchy) == BindingFlags.FlattenHierarchy)
            {
                properties = type.GetRuntimeMethods();
            }

            return properties
                .Where(m => (flags & BindingFlags.Public) != BindingFlags.Public || m.IsPublic)
                .Where(m => (flags & BindingFlags.Instance) != BindingFlags.Instance || !m.IsStatic)
                .Where(m => (flags & BindingFlags.Static) != BindingFlags.Static || m.IsStatic);
        }

        public static MethodInfo GetMethod(this Type type, string name, BindingFlags flags)
        {
            return GetMethods(type, flags).FirstOrDefault(m => m.Name == name);
        }

        public static MethodInfo GetMethod(this Type type, string name)
        {
            return GetMethods(type, BindingFlags.Public | BindingFlags.FlattenHierarchy)
                   .FirstOrDefault(m => m.Name == name);
        }

        public static IEnumerable<ConstructorInfo> GetConstructors(this Type type, BindingFlags flags)
        {
            return type.GetConstructors()
                .Where(m => (flags & BindingFlags.Public) != BindingFlags.Public || m.IsPublic)
                .Where(m => (flags & BindingFlags.Instance) != BindingFlags.Instance || !m.IsStatic)
                .Where(m => (flags & BindingFlags.Static) != BindingFlags.Static || m.IsStatic);
        }

        public static IEnumerable<FieldInfo> GetFields(this Type type)
        {
            return GetFields(type, BindingFlags.Public | BindingFlags.FlattenHierarchy);
        }

        public static IEnumerable<FieldInfo> GetFields(this Type type, BindingFlags flags)
        {
            var fields = type.GetTypeInfo().DeclaredFields;
            if ((flags & BindingFlags.FlattenHierarchy) == BindingFlags.FlattenHierarchy)
            {
                fields = type.GetRuntimeFields();
            }

            return fields
                .Where(f => (flags & BindingFlags.Public) != BindingFlags.Public || f.IsPublic)
                .Where(f => (flags & BindingFlags.Instance) != BindingFlags.Instance || !f.IsStatic)
                .Where(f => (flags & BindingFlags.Static) != BindingFlags.Static || f.IsStatic);
        }

        public static FieldInfo GetField(this Type type, string name, BindingFlags flags)
        {
            return GetFields(type, flags).FirstOrDefault(p => p.Name == name);
        }

        public static FieldInfo GetField(this Type type, string name)
        {
            return GetFields(type, BindingFlags.Public | BindingFlags.FlattenHierarchy).FirstOrDefault(p => p.Name == name);
        }

        public static Type[] GetGenericArguments(this Type type)
        {
            return type.GenericTypeArguments;
        }

        //public static T RemoveChildObjects<T>(this T coreObject) where T : CoreObject
        //{
        //    var fields = coreObject.GetType().GetTypeInfo().DeclaredFields;
        //    foreach (var field in fields)
        //    {
        //        if (field.FieldType.GetTypeInfo().IsSubclassOf(typeof(CoreObject)) || field.FieldType.IsEnumerable())
        //            field.SetValue(coreObject, null);
        //    }

        //    return coreObject;
        //}

        public static object GetNestedPropertyValue<T>(this T parentObject, string propertyName) where T : class
        {
            var properties = propertyName.Split('.').ToList();

            for (int i = 0; i < properties.Count; i++)
            {
                var propertyValue = parentObject.GetType().GetProperty(properties[i])?.GetValue(parentObject);

                if (propertyValue == null)
                    return null;

                properties.RemoveAt(0);

                if (i < properties.Count)
                    return propertyValue.GetNestedPropertyValue(string.Join(".", properties));

                return propertyValue;
            }

            return parentObject;
        }

        //public static IEnumerable<PropertyInfo> GetAllRelativeProperties(this object entityObj)
        //{
        //    var props = entityObj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).
        //                        Where(x => (x.PropertyType.IsConstructedGenericType && x.CanWrite && x.GetGetMethod().IsVirtual && x.CustomAttributes.Any(a => a.AttributeType == typeof(ManyToManyCollectionAttribute))));
        //    return props;
        //}

        public static object GetEntityFieldValue(this object entityObj, string propertyName)
        {
            var pro = entityObj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).First(x => x.Name == propertyName);
            return pro.GetValue(entityObj, null);

        }

        //public static Dictionary<PropertyInfo, IList<CoreObject>> LoadedManyToManyCollections(this object entityObj)
        //{
        //    var result = new Dictionary<PropertyInfo, IList<CoreObject>>();
        //    var navigationProperties = entityObj.GetAllRelativeProperties();


        //    foreach (var item in navigationProperties)
        //    {
        //        var propertyValue = GetEntityFieldValue(entityObj, item.Name);
        //        if (propertyValue != null)
        //        {
        //            if (propertyValue is IEnumerable<CoreObject>)
        //            {
        //                var col = propertyValue as IEnumerable<CoreObject>;
        //                if (col.GetEnumerator().MoveNext())
        //                {
        //                    result.Add(item, col.ToList());
        //                }

        //            }
        //        }
        //    }
        //    return result.Count > 0 ? result : null;
        //}

        [Flags]
        public enum BindingFlags
        {
            //None = 0,
            //Instance = 1,
            //Public = 2,
            //Static = 4,
            //FlattenHierarchy = 8,
            //SetProperty = 8192

            Default = 0,
            IgnoreCase = 1,
            DeclaredOnly = 2,
            Instance = 4,
            Static = 8,
            Public = 16,
            NonPublic = 32,
            FlattenHierarchy = 64,
            InvokeMethod = 256,
            CreateInstance = 512,
            GetField = 1024,
            SetField = 2048,
            GetProperty = 4096,
            SetProperty = 8192,
            PutDispProperty = 16384,
            PutRefDispProperty = 32768,
            ExactBinding = 65536,
            SuppressChangeType = 131072,
            OptionalParamBinding = 262144,
            IgnoreReturn = 16777216,
        }
    }
}