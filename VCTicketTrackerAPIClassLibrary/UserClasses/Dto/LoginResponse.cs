using System.Text.Json.Serialization;
using VCTicketTrackerAPIClassLibrary.Auth;

namespace VCTicketTrackerAPIClassLibrary.UserClasses.Dto
{
    /// <summary>
    /// <example>
    /// <code>
    /// {
    ///     "token":"JWT Token",
    ///     "user":
    ///     "
    ///     <see cref="ApplicationUser"/>
    ///     <see cref="ServerAdmin"/>
    ///     <see cref="Administrator"/>
    ///     <see cref="SupportMember"/>
    ///     <see cref="Lecturer"/>
    ///     <see cref="Student"/>
    ///     <see cref="Guest"/>
    ///     "
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public class LoginResponse
    {
        [JsonPropertyName("token")]
        public string? Token { get; set; }
        [JsonPropertyName("user")]
        public ApplicationUser? User { get; set; }
    }
}
