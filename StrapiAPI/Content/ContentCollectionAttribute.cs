using System;
using Newtonsoft.Json;

namespace Solarflare.StrapiAPI
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ContentCollectionAttribute : Attribute
    {
        /// <summary>
        /// Name of this Collection
        /// </summary>
        public readonly string CollectionName;
        
        /// <summary>
        /// New CollectionNameAttribute is created
        /// </summary>
        /// <param name="collectionName"></param>
        public ContentCollectionAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }
        
    }
}