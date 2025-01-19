using System.Text.Json.Serialization;

namespace VCTicketTrackerAPIClassLibrary.UserClasses.Dto
{
    /// <summary>
    /// {
    ///   "email": "string",
    ///   "message": "string",
    ///   "newPassword": "string"
    /// }
    /// </summary>
    public class ForgotPassword
    {
        /// <value>
        /// 
        /// </value>
        [JsonPropertyName("email")]
        public string Email { get; set; }
        /// <value>
        /// 
        /// </value>        
        [JsonPropertyName("message")]
        public string Message { get; set; }
        /// <value>
        /// 
        /// </value>
        [JsonPropertyName("newPassword")]
        public string NewPassword { get; set; }
        public ForgotPassword()
        {
        }

        public ForgotPassword(string email, string message, string newPassword)
        {
            Email = email;
            Message = message;
            NewPassword = newPassword;
        }

        public string GenPassword()
        {
            var chars = new[] {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
                'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
                'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '=' };
            Random random = new Random();

            var password = new char[12]; // Array for the password

            for (int i = 0; i < password.Length; i++)
            {
                password[i] = chars[random.Next(chars.Length)];
            }
            return password.ToString();
        }
    }
}
