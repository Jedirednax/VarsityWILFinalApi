using System.Text.Json.Serialization;
using VCTicketTrackerAPIClassLibrary.Auth;
using VCTicketTrackerAPIClassLibrary.StorageDB;
using VCTicketTrackerAPIClassLibrary.UserClasses.Dto;
using VCTicketTrackerAPIClassLibrary.UserClasses.Poco;

namespace VCTicketTrackerAPIClassLibrary.UserClasses
{

    /// <summary>
    /// <example>
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
    ///     "lockoutEnd": "2024-11-20T11:12:37.454Z",
    ///     "lockoutEnabled": true,
    ///     "accessFailedCount": 0,
    ///     "securityStamp": "string",
    ///     "concurrencyStamp": "string",
    ///     "fcmToken": "string",
    ///     "staffID": 0,
    ///     "campus": "string",
    ///     "serverSystem": "string"
    /// }
    /// </code>
    /// </example>
    /// 
    /// </summary>
    public class ServerAdmin : ApplicationUser, IServerAdmin, IApplicationUser
    {


        #region Props
        /// <value>
        /// a Long to allow for seperate table in DB
        /// </value>
        [JsonPropertyName("staffID")]
        public long StaffID { get; set; }

        /// <value>
        /// The campus teh Server admin is based at
        /// </value>
        [JsonPropertyName("campus")]
        public string Campus { get; set; }

        /// <value>
        /// Is used to differentiate which system the User over sees.
        /// </value>
        [JsonPropertyName("serverSystem")]
        public string ServerSystem { get; set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Paramateless constructor For Application User Base
        /// </summary>
        public ServerAdmin() : base() { }

        /// <summary>
        /// Implimnet of Base constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="userType"></param>
        public ServerAdmin(long id, string firstName, string lastName, string username, string password, UserType userType)
    : base(id, firstName, lastName, username, password, userType)
        {
            StaffID = id;
        }


        public ServerAdmin(ApplicationUser user) : base(user)
        {
            StaffID = user.Id;

        }

        public ServerAdmin(string firstName, string lastName, string userName, string email, string userType, string passwordHash, string phoneNumber) : base(firstName, lastName, userName, email, userType, passwordHash, phoneNumber)
        {
        }

        public ServerAdmin(RegisterDto registerDto) : base(registerDto)
        {

            ConvertFromPoco(((StaffDto)registerDto).ConvertToPoco());

        }



        #endregion
        #region Overrides
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Insert()
        {

            long? userId = DatabaseAccesUtil.InsertUsers(this);

            // Check Worked
            if(userId.HasValue)
            {
                Id = userId.Value;
                // Insert into user TypeTable Second!
                var staffPoco = ConvertToPoco();
                return DatabaseAccesUtil.InsertStaff(staffPoco);
            }

            return false;
        }

        /// <summary>
        /// Gets details For the specific user type, Fomr (Staff)TypeTable
        /// </summary>
        public override void GetDetails()
        {
            var t = DatabaseAccesUtil.GetStaffDetails(Id);
            ConvertFromPoco(t);
        }

        /// <summary>
        /// Gets details For the specific user type, Fomr (Staff)TypeTable
        /// </summary>
        public override void GetUserInfo()
        {
            //DatabaseAccesUtil.GetStaffDetails();
        }


        public override bool Update() => throw new NotImplementedException();

        #endregion
        #region UnquieMethods
        #endregion
        public override string? ToString()
        {
            return base.ToString() +
            $"|System: {ServerSystem,-9}" +
            $"\n";
        }

        public IStaff ConvertFromPoco(StaffPoco staff)
        {
            StaffID = staff.StaffID;
            Campus =  staff.Campus;
            ServerSystem =  staff.ServerSystem;
            return this;

        }
        public StaffPoco ConvertToPoco()
        {
            return new StaffPoco {
                StaffID = this.StaffID,
                UserID = this.Id,
                DepartmentID = 1, // Set if applicable
                Campus = this.Campus,
                StaffType = this.GetType().Name,
                ServerSystem = this.ServerSystem,
                CourseID = 0,
                NumberOfTickets = 0
            };
        }

    }
}
