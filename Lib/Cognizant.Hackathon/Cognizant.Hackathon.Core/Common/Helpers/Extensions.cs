//using System;
//using System.Collections;
using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Reflection;
//using Cognizant.Hackathon.Core.Model.Internal;

namespace Cognizant.Hackathon.Core.Common.Helpers
{  
    public static class Extensions
    {
        public static Dictionary<string, T> AddReplace<T>(this Dictionary<string, T> dictionary, string key, T value)
        {
            if (dictionary == null)
            {
                return null;
            }

            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }

            return dictionary;
        }

        public static IList<T> AddReplace<T>(this IList<T> list, T value)
        {
            if (list == null)
            {
                return null;
            }

            if (!list.Contains(value))
            {
                list.Add(value);
            }

            return list;
        }

        /*
        private static readonly Func<MethodInfo, IEnumerable<Type>> ParameterTypeProjection =
            method => method.GetParameters().Select(p => p.ParameterType.GetGenericTypeDefinition());
              
        public static IEnumerable<T> SelectDeep<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
        {
            foreach (T item in source)
            {
                yield return item;
                foreach (T subItem in SelectDeep(selector(item), selector))
                {
                    yield return subItem;
                }
            }
        }

        public static IEnumerable<T> SelectDeep<T>(this T source, Func<T, T> selector)
        {
            return new[] { source }.SelectDeep(_ => new[] { selector(_) }.Where(r => !r.Equals(default(T))));
        }  
                
        public static TEntity CopyTo<TEntity>(this TEntity OriginalEntity, TEntity EntityToMergeOn)
        {
            IEnumerable<PropertyInfo> oProperties = OriginalEntity.GetType().GetProperties();

            foreach (PropertyInfo CurrentProperty in oProperties.Where(p => p.CanWrite))
            {
                object originalValue = CurrentProperty.GetValue(EntityToMergeOn);

                if (originalValue != null)
                {
                    IListLogic(OriginalEntity, CurrentProperty, originalValue);
                }
                else
                {
                    object value = CurrentProperty.GetValue(OriginalEntity, null);
                    CurrentProperty.SetValue(EntityToMergeOn, value, null);
                }
            }

            return OriginalEntity;
        }
       
        public static IList<T> GetEnumValues<T>()
        {
            return (T[])System.Enum.GetValues(typeof(T));
        }
        
        public static MethodInfo GetGenericMethod(this Type type, string name, params Type[] parameterTypes)
        {
            return (from method in type.GetRuntimeMethods()
                    where method.Name == name
                    where parameterTypes.SequenceEqual(ParameterTypeProjection(method))
                    select method).SingleOrDefault();
        }     
        
        public static MethodInfo GetMethodExt(this Type thisType, string name, params Type[] parameterTypes)
        {
            MethodInfo matchingMethod = null;

            GetMethodExt(ref matchingMethod, thisType, name, parameterTypes);

            if (matchingMethod == null && thisType.GetTypeInfo().IsInterface)
            {
                foreach (Type interfaceType in thisType.GetTypeInfo().ImplementedInterfaces)
                {
                    GetMethodExt(ref matchingMethod, interfaceType, name, parameterTypes);
                }
            }

            return matchingMethod;
        }

        public static Type GetUnderlyingType(this Type type)
        {
            return type.GetTypeInfo().IsGenericType ? type.GenericTypeArguments[0].GetUnderlyingType() : type;
        }
        
        public static bool IsNullable<T>(this T obj)
        {
            if (obj == null)
            {
                return true; 
            }

            Type type = typeof(T);
            if (type.IsByRef)
            {
                return true; 
            }

            if (Nullable.GetUnderlyingType(type) != null)
            {
                return true; 
            }

            return false;
        }
       
        public static TEntity MergeWith<TEntity>(this TEntity sourceEntity, TEntity updatedEntity, bool writeNull = false, string[] propertiesToNullify = null)
        {
            var properties = typeof(TEntity).GetRuntimeProperties();

            foreach (PropertyInfo prop in properties)
            {
                if (!prop.CanRead || !prop.CanWrite) continue;

				MethodInfo castMethod = typeof(ObjectHelpers).GetMethod("ToType").MakeGenericMethod(prop.PropertyType);

				var updatedEntityValue = castMethod.Invoke(null, new [] { prop.GetValue(updatedEntity, null) }); ;
                var sourceEntityValue = castMethod.Invoke(null, new [] { prop.GetValue(sourceEntity, null) });
				
				if (!ObjectHelpers.ValueEquality(updatedEntityValue, sourceEntityValue) 
					&& !(updatedEntityValue is Guid && ObjectHelpers.ValueEquality((Guid)updatedEntityValue, default(Guid))))
				{
					if (updatedEntityValue != null || (writeNull) || (propertiesToNullify != null && propertiesToNullify.Contains(prop.Name)))
                        prop.SetValue(sourceEntity, updatedEntityValue, null);
                }
            }

            return sourceEntity;
        }		
		
		public static string RemoveLastSeparatedElement(this string separatedString, char separator)
        {
            string[] array = separatedString.Split(separator);

            if (array.Count() <= 1)
            {
                return string.Empty;
            }

            array[array.Count() - 1] = string.Empty;
            return string.Join(separator.ToString(), array).TrimEnd(new[] { '.' });
        }
        
        public static string ToSizeString(this double bytes)
        {
            CultureInfo culture = CultureInfo.CurrentUICulture;
            const string format = "#,0.0";

            if (bytes < 1024)
            {
                return bytes.ToString("#,0", culture);
            }

            bytes /= 1024;
            if (bytes < 1024)
            {
                return bytes.ToString(format, culture) + " KB";
            }

            bytes /= 1024;
            if (bytes < 1024)
            {
                return bytes.ToString(format, culture) + " MB";
            }

            bytes /= 1024;
            if (bytes < 1024)
            {
                return bytes.ToString(format, culture) + " GB";
            }

            bytes /= 1024;
            return bytes.ToString(format, culture) + " TB";
        }
                
        private static void GetMethodExt(
            ref MethodInfo matchingMethod, 
            Type type, 
            string name, 
            params Type[] parameterTypes)
        {
            foreach (MethodInfo methodInfo in type.GetRuntimeMethods().Where(x => x.Name == name))
            {
                ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                if (parameterInfos.Length == parameterTypes.Length)
                {
                    int i = 0;
                    for (; i < parameterInfos.Length; ++i)
                    {
                        if (!parameterInfos[i].ParameterType.IsSimilarType(parameterTypes[i]))
                        {
                            break;
                        }
                    }

                    if (i == parameterInfos.Length)
                    {
                        if (matchingMethod == null)
                        {
                            matchingMethod = methodInfo;
                        }
                        else
                        {
                            throw new AmbiguousMatchException("More than one matching method found!");
                        }
                    }
                }
            }
        }
        
        private static void IListLogic<TEntity>(
            TEntity OriginalEntity, 
            PropertyInfo CurrentProperty, 
            object originalValue)
        {
            if (originalValue is IList)
            {
                var tempList = originalValue as IList;
                var existingList = CurrentProperty.GetValue(OriginalEntity) as IList;

                foreach (object item in tempList)
                {
                    existingList.Add(item);
                }
            }
        }
        
        private static bool IsSimilarType(this Type thisType, Type type)
        {
            if (thisType.IsByRef)
            {
                thisType = thisType.GetElementType();
            }

            if (type.IsByRef)
            {
                type = type.GetElementType();
            }

            if (thisType.IsArray && type.IsArray)
            {
                return thisType.GetElementType().IsSimilarType(type.GetElementType());
            }

            if (thisType.Name == "System.RuntimeType" && type.Name == "System.Type")
            {
                return true;
            }

            if (thisType == type
                || ((thisType.IsGenericParameter || thisType == typeof(T))
                    && (type.IsGenericParameter || type == typeof(T))))
            {
                return true;
            }

            
            if (thisType.GetTypeInfo().IsGenericType && type.GetTypeInfo().IsGenericType)
            {
                Type[] thisArguments = thisType.GenericTypeArguments;
                Type[] arguments = type.GenericTypeArguments;
                if (thisArguments.Length == arguments.Length)
                {
                    for (int i = 0; i < thisArguments.Length; ++i)
                    {
                        if (!thisArguments[i].IsSimilarType(arguments[i]))
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }

            return false;
        }

       
        public class T
        {

        }

        public static bool IsEnumerable(this Type type)
        {
            return type.GetTypeInfo().ImplementedInterfaces.Any(type1 => type1.Name == "IEnumerable");
        }

        public static string ToID(this string value)
        {
            return $"{value}ID";
        }

        public static IEnumerable<T> ApplyExplicitProperties<T>(this IEnumerable<T> data, IEnumerable<string> explicitProperties) where T : CoreObject
        {
            if (explicitProperties != null)
            {
                explicitProperties=explicitProperties.Concat(new[] { "Id", "Created" });
                var query = data.AsQueryable();
                query = query.Select(ReflectionHelpers.DynamicSelectQuery<T>(explicitProperties)).AsQueryable();
                return query.ToList().AsEnumerable();
            }

            return data;
        }

        public static IQueryable<T> ApplyExplicitProperties<T>(this IQueryable<T> data, IQueryable<string> explicitProperties) where T : CoreObject
        {
            if (explicitProperties != null)
            {
                explicitProperties=explicitProperties.Concat(new[] { "Id", "Created" });
                var query = data.AsQueryable();
                return query.Select(ReflectionHelpers.DynamicSelectQuery<T>(explicitProperties)).AsQueryable();
            }

            return data;
        }

        */
    }
}