using System.Text.Json.Serialization;
using VCTicketTrackerAPIClassLibrary.Auth;
using VCTicketTrackerAPIClassLibrary.Orginisation;
using VCTicketTrackerAPIClassLibrary.StorageDB;
using VCTicketTrackerAPIClassLibrary.UserClasses.Dto;
using VCTicketTrackerAPIClassLibrary.UserClasses.Poco;

namespace VCTicketTrackerAPIClassLibrary.UserClasses
{

    /// <summary>
    /// <example>
    /// Full User Returned
    /// <code>
    /// {
    ///     "id":                  0,
    ///     "userName":            "string",
    ///     "normalizedUserName":  "string",
    ///     "email":               "string",
    ///     "normalizedEmail":     "string",
    ///     "phoneNumber":         "string",
    ///     "firstName":           "string",
    ///     "passwordHash":        "string",
    ///     "lastName":            "string",
    ///     "userType":            "ServerAdmin",
    ///     "authenticationType":  "string",
    ///     "isAuthenticated":     true,
    ///     "emailConfirmed":      true,
    ///     "phoneNumberConfirmed": true,
    ///     "twoFactorEnabled":    true,
    ///     "lockoutEnd":          "2024-10-28T05:12:45.911Z",
    ///     "lockoutEnabled":      true,
    ///     "accessFailedCount":   0,
    ///     "securityStamp":       "string",
    ///     "concurrencyStamp":    "string",
    ///     
    ///     "staffID":             0,
    ///     "campus":              "string",
    ///     "departmentId":        0,
    ///     "department":
    ///     {
    ///        "departmentID":     0,
    ///        "departmentName":   "string"
    ///     }
    /// }
    /// </code>
    /// Register DTO
    /// <see cref="StaffDto"/>
    /// <seealso cref="StaffPoco"/>
    /// <code>
    /// </code>
    /// </example>
    /// </summary>
    public class Administrator : ApplicationUser, IAdministrator
    {

        #region Props
        /// <value>
        /// 
        /// </value>
        [JsonPropertyName("staffID")]
        public long StaffID { get; set; }

        /// <value>
        /// 
        /// </value>
        [JsonPropertyName("campusD")]
        public string Campus { get; set; }
        /// <value>
        /// 
        /// </value>
        [JsonPropertyName("departmentId")]
        public int DepartmentId { get; set; }

        /// <value>
        /// 
        /// </value>
        [JsonPropertyName("department")]
        public Department Department { get; set; }


        #endregion
        #region Constructors
        public Administrator(long id, string firstName, string lastName, string username, string password, UserType userType)
    : base(id, firstName, lastName, username, password, userType)
        {
        }

        public Administrator() : base() { }

        public Administrator(int departmentId) : base()
        {
            DepartmentId = departmentId;
        }

        public Administrator(ApplicationUser user) : base(user) { }

        public Administrator(string firstName, string lastName, string userName, string email, string userType, string passwordHash, string phoneNumber) : base(firstName, lastName, userName, email, userType, passwordHash, phoneNumber)
        {
        }

        public Administrator(RegisterDto registerDto) : base(registerDto)
        {
            if(registerDto is StaffDto staffDto)
            {
                ConvertFromPoco(staffDto.ConvertToPoco());
            }
            else
            {
                throw new InvalidCastException("RegisterDto is not of type StaffDto");
            }
        }


        #endregion
        #region Overrides
        public override bool Insert()
        {
            long? userId = DatabaseAccesUtil.InsertUsers(this);

            if(userId.HasValue)
            {
                Id = userId.Value;
                var staffPoco = ConvertToPoco();
                return DatabaseAccesUtil.InsertStaff(staffPoco);
            }

            return false;
        }

        public override void GetDetails()
        {
            var t = DatabaseAccesUtil.GetStaffDetails(Id);
            ConvertFromPoco(t);
        }
        public override bool Update() => throw new NotImplementedException();
        #endregion
        #region UnquieMethods
        #endregion
        public override string? ToString()
        {
            return base.ToString() +
            $"|DepartID: {DepartmentId,-9}" +
            $"\n";
        }

        public StaffPoco ConvertToPoco()
        {
            return new StaffPoco {
                StaffID         = this.StaffID,
                UserID          = this.Id,
                DepartmentID    = this.DepartmentId,
                Campus          = this.Campus,
                StaffType       = UserType.Administrator.ToString(),
                ServerSystem    = "null",
                CourseID        = 0,
                NumberOfTickets = 0
            };
        }
        public IStaff ConvertFromPoco(StaffPoco staff)
        {

            StaffID      = staff.StaffID;
            Campus       = staff.Campus;
            DepartmentId = (int)staff.DepartmentID;
            return this;
        }
    }
}
