using System.Diagnostics;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using VCTicketTrackerAPIClassLibrary.StorageDB;
using VCTicketTrackerAPIClassLibrary.UserClasses;
using VCTicketTrackerAPIClassLibrary.UserClasses.Dto;

namespace VCTicketTrackerAPIClassLibrary.Auth
{


    /// <summary>
    /// Abstract Class used for defining the properties and actions used by all users of the system.
    /// Inherits from IdentityUser for ASP.NET Identity integration.
    /// <example>
    /// Json Representation
    /// <code>
    /// {
    ///     "userName": "string",
    ///     "normalizedUserName": "string",
    ///     "email": "string",
    ///     "normalizedEmail": "string",
    ///     "passwordHash": "string",
    ///     "phoneNumber": "string",
    ///     "id": 0,
    ///     "firstName": "string",
    ///     "lastName": "string",
    ///     "userType": "ServerAdmin",
    ///     "authenticationType": "string",
    ///     "isAuthenticated": true,
    ///     "emailConfirmed": true,
    ///     "phoneNumberConfirmed": true,
    ///     "twoFactorEnabled": true,
    ///     "lockoutEnd": "2024-11-20T10:53:10.445Z",
    ///     "lockoutEnabled": true,
    ///     "accessFailedCount": 0,
    ///     "securityStamp": "string",
    ///     "concurrencyStamp": "string",
    ///     "fcmToken": "string"
    /// }
    /// </code>
    /// <code>
    /// <see cref="RegisterDto">
    ///  {
    ///    "firstName": "string",
    ///    "lastName": "string",
    ///    "userName": "string",
    ///    "email": "string",
    ///    "userType": "string",
    ///    "passwordHash": "string"
    ///  }
    /// </code>
    /// </example>
    /// </summary>
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "UsersTypes")]
    [JsonDerivedType(typeof(Student), typeDiscriminator: nameof(UserType.Student))]
    [JsonDerivedType(typeof(ServerAdmin), typeDiscriminator: nameof(UserType.ServerAdmin))]
    [JsonDerivedType(typeof(Administrator), typeDiscriminator: nameof(UserType.Administrator))]
    [JsonDerivedType(typeof(SupportMember), typeDiscriminator: nameof(UserType.SupportMember))]
    [JsonDerivedType(typeof(Lecturer), typeDiscriminator: nameof(UserType.Lecturer))]
    [JsonDerivedType(typeof(Guest), typeDiscriminator: nameof(UserType.Guest))]
    public class ApplicationUser : IdentityUser<long>, IApplicationUser//, IUser
    {
        #region Properties

        /// <value>A long integer representing the unique identifier for the user.</value>
        [JsonPropertyName("id")]
        public override long Id { get; set; }

        /// <value>A string representing the user's first name.</value>
        [JsonPropertyName("firstName")]
        [PersonalData]
        public string FirstName { get; set; } = string.Empty;

        /// <value>A string representing the user's last name.</value>
        [JsonPropertyName("lastName")]
        [PersonalData]
        public string LastName { get; set; } = string.Empty;

        /// <value>An enum value representing the type of user (e.g., Guest, Admin).</value>
        [JsonPropertyName("userType")]
        public UserType UsersTypes { get; set; } = UserType.Guest;

        /// <value>A string representing the type of authentication used for the user. Defaults to "Basic".</value>
        [JsonPropertyName("authenticationType")]
        public string AuthenticationType { get; set; } = "Basic";

        /// <value><c>true</c> if the user is authenticated; otherwise, <c>false</c>.</value>
        [JsonPropertyName("isAuthenticated")]
        public bool IsAuthenticated { get; set; } = false;

        /// <value><c>true</c> if the email is confirmed; otherwise, <c>false</c>.</value>
        [JsonPropertyName("emailConfirmed")]
        public override bool EmailConfirmed { get; set; } = false;

        /// <value><c>true</c> if the phone number is confirmed; otherwise, <c>false</c>.</value>
        [JsonPropertyName("phoneNumberConfirmed")]
        public override bool PhoneNumberConfirmed { get; set; } = false;

        /// <value><c>true</c> if two-factor authentication is enabled; otherwise, <c>false</c>.</value>
        [JsonPropertyName("twoFactorEnabled")]
        public override bool TwoFactorEnabled { get; set; } = false;

        /// <value>A nullable <see cref="DateTimeOffset"/> representing the end time of the lockout.</value>
        [JsonPropertyName("lockoutEnd")]
        public override DateTimeOffset? LockoutEnd { get; set; } = null;

        /// <value><c>true</c> if lockout is enabled; otherwise, <c>false</c>.</value>
        [JsonPropertyName("lockoutEnabled")]
        public override bool LockoutEnabled { get; set; } = false;

        /// <value>An integer representing the count of failed access attempts.</value>
        [JsonPropertyName("accessFailedCount")]
        public override int AccessFailedCount { get; set; } = 0;

        /// <value>A string representing the security stamp for security-related operations.</value>
        [JsonPropertyName("securityStamp")]
        public override string SecurityStamp { get; set; } = Guid.NewGuid().ToString();

        /// <value>A string representing the concurrency stamp for concurrency checks.</value>
        [JsonPropertyName("concurrencyStamp")]
        public override string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        /// <value>A string representing the FCM Token for push notifications. Defaults to "empty".</value>
        [JsonPropertyName("fcmToken")]
        public string fcmToken { get; set; } = "empty";

        #endregion


        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public ApplicationUser()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="userType"></param>
        public ApplicationUser(long id, string firstName, string lastName, string username, string password, UserType userType)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            UserName = username;
            PasswordHash = password;
            UsersTypes = userType;
            NormalizedEmail = Email.Normalize().ToUpper();
            NormalizedUserName = UserName.Normalize().ToUpper();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="userType"></param>
        /// <param name="passwordHash"></param>
        /// <param name="phoneNumber"></param>
        public ApplicationUser(string firstName, string lastName, string userName, string email, string userType, string passwordHash, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            Email = email;
            UsersTypes   = (UserType)Enum.Parse(typeof(UserType), userType);
            PasswordHash = passwordHash;
            NormalizedEmail = Email.ToUpper();
            NormalizedUserName = UserName.ToUpper();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public ApplicationUser(ApplicationUser user)
        {
            Debug.WriteLine(user);
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            UserName = user.UserName;
            PasswordHash = user.PasswordHash;
            UsersTypes = user.UsersTypes;
            AuthenticationType = user.AuthenticationType;
            IsAuthenticated = user.IsAuthenticated;
            UserName = user.UserName;
            if(user.Email == null)
            {
                user.Email = user.UserName;
                UserName = user.UserName;
            }
            Email = user.Email;
            NormalizedEmail =    user.UserName.ToUpper();
            NormalizedUserName = user.UserName.ToUpper();
        }

        /// <summary>
        /// Constuctor Creating a Application User From A <cref>RegisterDTO</cref>
        /// </summary>
        /// <param name="registerDto"> Data transfer object for A user regsetering </param>
        public ApplicationUser(RegisterDto registerDto)
        {
            FirstName          = registerDto.FirstName;
            LastName           = registerDto.LastName;
            UserName           = registerDto.UserName;
            Email              = registerDto.Email;
            PhoneNumber        = "000-000-0000";
            PasswordHash       = registerDto.PasswordHash;
            UsersTypes         = Enum.Parse<UserType>(registerDto.UserType);
            NormalizedEmail    = Email.Normalize().ToUpper();
            NormalizedUserName = UserName.Normalize().ToUpper();
            SecurityStamp      = Guid.NewGuid().ToString();
            ConcurrencyStamp   = Guid.NewGuid().ToString();
        }

        public ApplicationUser(StudentDTO registerDto)
        {
            FirstName          = registerDto.FirstName;
            LastName           = registerDto.LastName;
            UserName           = registerDto.Username;
            Email              = registerDto.Username;
            PhoneNumber        = "000-000-0000";
            PasswordHash       = registerDto.Password;
            UsersTypes         = UserType.Student;
            NormalizedEmail    = Email.Normalize().ToUpper();
            NormalizedUserName = UserName.Normalize().ToUpper();
            SecurityStamp      = Guid.NewGuid().ToString();
            ConcurrencyStamp   = Guid.NewGuid().ToString();
        }
        #endregion

        #region Virtual Methods
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        virtual public void GetDetails() { }
        /// <summary>
        /// Generic implimintstion for child classes
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        virtual public void GetUserInfo() => throw new NotImplementedException();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        virtual public bool Insert()

        {
            long? userId = DatabaseAccesUtil.InsertUsers(this);
            return userId != null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        virtual public bool Update() => throw new NotImplementedException();

        virtual public bool UpdateFcm(string fcmToken)
        {
            return DatabaseAccesUtil.SetFcm(Id, fcmToken);


        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        virtual public bool ConvertToPoco() => throw new NotImplementedException();
        /// <summary>
        /// Parent tempate Implimintation
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        virtual public bool ConvertFromPoco() => throw new NotImplementedException();
        #endregion

        #region Methods
        /// <summary>
        /// Get the created User Credentials
        /// (Note password Will mostlikly be hased)
        /// </summary>
        /// <returns>
        /// <see cref="UserCredential"/>
        /// </returns>
        public UserCredential GetCredential()
        {
            return new UserCredential(UserName, PasswordHash);
        }

        /// <summary>
        /// Provides a string representation of the user object.
        /// </summary>
        public override string ToString()
        {
            return $"UserId: {Id}, " +
                   $"FirstName: {FirstName}, " +
                   $"LastName: {LastName}, " +
                   $"UserName: {UserName}, " +
                   $"NormalizedUserName: {NormalizedUserName}, " +
                   $"Email: {Email}, " +
                   $"NormalizedEmail: {NormalizedEmail}, " +
                   $"EmailConfirmed: {EmailConfirmed}, " +
                   $"UserType: {UsersTypes}, " +
                   $"AuthenticationType: {AuthenticationType}, " +
                   $"IsAuthenticated: {IsAuthenticated}" +
                   $"PAssword: {PasswordHash}";
        }
        #endregion
    }

}