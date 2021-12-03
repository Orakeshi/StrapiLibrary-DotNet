namespace Solarflare.StrapiAPI.StrapiRequests
{
    /// <summary>
    /// Handles all attributes that can be assigned from strapi requests
    /// </summary>
    public class RequestData
    {
        /// <summary>
        /// Authorization token to be sent with the requests
        /// </summary>
        public string Authorization { get; set; }
        
        /// <summary>
        /// The Username field to be sent to login to strapi
        /// </summary>
        public string identifier { get; set; }
        
        /// <summary>
        /// Password field to be sent to login to strapi
        /// </summary>
        public string password { get; set; }
        
        /// <summary>
        /// JWT token used to authorize requests to strapi
        /// </summary>
        public string jwt { get; set; }
        
        public int id { get; set; }
    }
}