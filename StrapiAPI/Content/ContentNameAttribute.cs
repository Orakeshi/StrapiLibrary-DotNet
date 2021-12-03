using System;
using Newtonsoft.Json.Serialization;

namespace Solarflare.StrapiAPI
{
    /// <summary>
    /// States the properties of the content paramter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ContentNameAttribute : Attribute
    {
        public readonly string PropertyName;
        
        /// <summary>
        /// Creates a new ContentNameAttribute
        /// </summary>
        /// <param name="propertyName">Name of the parameter</param>
        public ContentNameAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}