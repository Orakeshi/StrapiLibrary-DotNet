using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Solarflare.StrapiAPI
{
    [JsonObject]
    public abstract class Content
    {
        public readonly string Name;

        protected readonly Dictionary<ContentNameAttribute, PropertyInfo> ContentParameterPropertyInfos = new ();
        
        protected Content()
        {
            Type type = GetType();
            ContentCollectionAttribute contentCollectionAttribute = type.GetCustomAttribute<ContentCollectionAttribute>();
            if (contentCollectionAttribute == null)
                throw new Exception($"{nameof(Content)} class must have a {nameof(ContentCollectionAttribute)} attribute.");

            Name = contentCollectionAttribute.CollectionName;

            IEnumerable<PropertyInfo> contentParameterProperties = type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(pi => pi.GetCustomAttribute<ContentNameAttribute>() != null);
            
            foreach (PropertyInfo contentParameterPropertyInfo in contentParameterProperties)
            {
                ContentNameAttribute contentParameterAttribute = contentParameterPropertyInfo.GetCustomAttribute<ContentNameAttribute>();
                ContentParameterPropertyInfos[contentParameterAttribute] = contentParameterPropertyInfo;
            }
        }

        public override string ToString()
        {
            // Create a string readable by Strapi and send. 
            string contentRequestString = "{";

            foreach (KeyValuePair<ContentNameAttribute, PropertyInfo> contentParameter in ContentParameterPropertyInfos)
            {
                ContentNameAttribute contentParameterAttribute = contentParameter.Key;
                object contentParameterValue = contentParameter.Value.GetValue(this);
                contentRequestString += $@"'{contentParameterAttribute.PropertyName}': ";
                contentRequestString += $@"'{contentParameterValue}',";
            }
            // Remove last comma and space
            contentRequestString = contentRequestString.Remove(contentRequestString.Length - 2, 2);
            
            contentRequestString += @"'}";
            
            return contentRequestString;
        }

    }
}