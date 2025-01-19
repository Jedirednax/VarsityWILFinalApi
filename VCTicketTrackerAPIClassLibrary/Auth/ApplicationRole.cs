using Microsoft.AspNetCore.Identity;

namespace VCTicketTrackerAPIClassLibrary.Auth
{
    /// <summary>
    /// Custom implimentation of a Application role For ASP.NET Core Identity
    /// </summary>
    public class ApplicationRole : IdentityRole<long>
    {
        /// <summary>
        /// Description of the role and limits
        /// </summary>
        private string description = string.Empty;

        /// <summary>
        /// A limit on the privileges for the role
        /// </summary>
        private short limitPriv = 0;

        /// <summary>
        /// Gets or sets the description for the role
        /// </summary>
        public string Description { get => description; set => description = value; }

        /// <summary>
        /// Gets or sets the limit on privileges for the role
        /// </summary>
        public short LimitPriv { get => limitPriv; set => limitPriv = value; }
    }
}