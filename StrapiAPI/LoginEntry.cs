using Newtonsoft.Json;

namespace Solarflare.StrapiAPI
{
    [ContentCollection("auth/local")]
    public class LoginEntry : Content
    {
        /// <summary>
        /// The Username field to be sent to login to strapi
        /// </summary>
        [ContentName("identifier")]
        public string Identifier { get; set; }
        
        /// <summary>
        /// Password field to be sent to login to strapi
        /// </summary>
        [ContentName("password")]
        public string Password { get; set; }
    }
}