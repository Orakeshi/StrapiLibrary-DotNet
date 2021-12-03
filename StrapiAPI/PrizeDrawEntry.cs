using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace Solarflare.StrapiAPI
{
    /// <summary>
    /// Represents a content type to access
    /// </summary>
    [ContentCollection("prize-draw-entries")]
    public class PrizeDrawEntry : Content
    {
        [ContentName("id")]
        public string Id { get; set; }
        
        /// <summary>
        /// Email data from strapi
        /// </summary>
        [ContentName("email")]
        public string Email { get; set; }
        
        /// <summary>
        /// Fullname from data from strapi
        /// </summary>
        [ContentName("fullName")]
        public string FullName { get; set; }
        
        /// <summary>
        /// Fullname from data from strapi
        /// </summary>
        [ContentName("age")]
        public string Age { get; set; }

        /// <summary>
        /// Phone data from strapi
        /// </summary>
        [ContentName("phone")]
        public string PhoneNumber { get; set; }
        
        /// <summary>
        /// Address data from strapi
        /// </summary>
        [ContentName("address")]
        public string Address { get; set; }
    }
}