using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace Solarflare.StrapiAPI
{
    /// <summary>
    /// Represents a content type to access
    /// </summary>
    [ContentCollection("competition-entries")]
    public class CompetitionEntry : Content
    {
        /// <summary>
        /// Fullname from data from strapi
        /// </summary>
        [ContentName("fullName")]
        public string FullName { get; set; }
        
        /// <summary>
        /// Email data from strapi
        /// </summary>
        [ContentName("email")]
        public string Email { get; set; }
        
        /// <summary>
        /// Address data from strapi
        /// </summary>
        [ContentName("address")]
        public string Address { get; set; }
        
        /// <summary>
        /// Phone data from strapi
        /// </summary>
        [ContentName("phone")]
        public string PhoneNumber { get; set; }
    }
}