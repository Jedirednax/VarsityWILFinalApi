namespace VCTicketTrackerAPIClassLibrary.Auth
{
    /// <summary>
    /// <example>
    /// <code>
    /// {
    ///  "tokenId": 0,
    ///  "userId": 0,
    ///  "tokenType": "string",
    ///  "token": "string",
    ///  "tokenActive": true
    ///}
    /// </code>
    /// </example>
    /// </summary>
    public class UserToken
    {
#nullable enable
        public long TokenId { get; set; }
        public long UserId { get; set; }
        public string TokenType { get; set; }
        public string Token { get; set; }
        public bool TokenActive { get; set; }
#nullable disable

        // Parameterless constructor for deserialization or manual instantiation
        public UserToken() { }
        // Constructor with parameters for easy instantiation
        public UserToken(long tokenId, long userId, string tokenType, string token, bool tokenActive)
        {
            TokenId = tokenId;
            UserId = userId;
            TokenType = tokenType;
            Token = token;
            TokenActive = tokenActive;
        }
    }

}