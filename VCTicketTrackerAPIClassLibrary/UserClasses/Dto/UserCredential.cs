using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VCTicketTrackerAPIClassLibrary.UserClasses.Dto
{
    /// <summary>
    /// Stores a usrname and password associated with a user
    /// <example>
    /// <code>
    /// {
    ///     "username":"Email@vctickettracker.com",
    ///     "password":"Password1!"
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public class UserCredential
    {
        private string password = string.Empty;
        private string username = string.Empty;

        /// <value>
        /// A users username represented by their email
        /// </value>
        [Required]
        [EmailAddress]
        [JsonPropertyName("username")]
        public string Username
        {
            get => username; set
            {
                username = value;//.Split('@')[0];
            }
        }

        /// <value>
        /// Password Unhashed or using HMAC SHA256, with Username salt
        /// </value>
        [Required]
        [DataType(DataType.Password)]
        [JsonPropertyName("password")]
        public string Password { get => password; set => password = value; }



        public UserCredential(string username, string password)
        {
            Username = username;
            Password = password;
        }


        public override string ToString()
        {
            return $"Credentials:\n" +
                   $"\tUsername: {Username}\n" +
                   $"\tPassword: {Password}\n";
        }
    }
}
