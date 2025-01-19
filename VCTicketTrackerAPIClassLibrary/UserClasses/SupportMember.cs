using VCTicketTrackerAPIClassLibrary.Auth;
using VCTicketTrackerAPIClassLibrary.Orginisation;
using VCTicketTrackerAPIClassLibrary.StorageDB;
using VCTicketTrackerAPIClassLibrary.Tickets;
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
    ///     "lockoutEnd": "2024-11-20T11:09:22.674Z",
    ///     "lockoutEnabled": true,
    ///     "accessFailedCount": 0,
    ///     "securityStamp": "string",
    ///     "concurrencyStamp": "string",
    ///     "fcmToken": "string",
    ///     "staffID": 0,
    ///     "campus": "string",
    ///     "departmentId": 0,
    ///     "numberOfTickets": 0,
    ///     "department": 
    ///     {
    ///         "departmentID": 0,
    ///         "departmentName": "string"
    ///     },
    ///     "assigendTickets": 
    ///     [
    ///         {
    ///             "ticketID": 0,
    ///             "ticketTitle": "string",
    ///             "ticketDescription": "string",
    ///             "ticketStatus": "Created",
    ///             "ticketCategory": 
    ///             {
    ///                 "id": 0,
    ///                 "name": "string",
    ///                 "description": "string",
    ///                 "depart": 0,
    ///                 "priority": 0,
    ///                 "parentType": "string",
    ///                 "fields": 
    ///                 {
    ///                     "additionalProp1": "string",
    ///                     "additionalProp2": "string",
    ///                     "additionalProp3": "string"
    ///                 }
    ///             },
    ///             "ticketCategoryId": "AcademicInternalCreditQuery",
    ///             "assignedTo": 0,
    ///             "ticketCreatedBy": 0,
    ///             "ticketCreationDate": "2024-11-20T11:09:22.674Z",
    ///             "ticketResolvedDate": "2024-11-20T11:09:22.674Z",
    ///             "ticketDocumentURLS": "string",
    ///             "ticketUpdates": 
    ///             [
    ///                 {
    ///                     "eventId": 0,
    ///                     "ticketId": 0,
    ///                     "date": "2024-11-20T11:09:22.674Z",
    ///                     "updtedBy": 0,
    ///                     "statusFrom": "Created",
    ///                     "statusTo": "Created",
    ///                     "type": "string",
    ///                     "comment": "string"
    ///                 }
    ///             ]
    ///         }
    ///     ]
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public class SupportMember : ApplicationUser, ISupportMember
    {
        #region Props
        public long StaffID { get; set; }
        public string Campus { get; set; }
        public int DepartmentId { get; set; }
        public int? NumberOfTickets { get; set; }
        public Department Department { get; set; }
        public List<Ticket> AssigendTickets { get; set; }



        #endregion
        #region Constructors
        public SupportMember() : base()
        {
        }
        public SupportMember(long id, string firstName, string lastName, string username, string password, UserType userType)
    : base(id, firstName, lastName, username, password, userType)
        {
        }


        public SupportMember(int departmentId, int? numberOfTickets) : base()
        {
            DepartmentId    = departmentId;
            NumberOfTickets = numberOfTickets;
        }

        public SupportMember(ApplicationUser user) : base(user)
        {
        }

        public SupportMember(string firstName, string lastName, string userName, string email, string userType, string passwordHash, string phoneNumber) : base(firstName, lastName, userName, email, userType, passwordHash, phoneNumber)
        {
        }

        public SupportMember(RegisterDto registerDto) : base(registerDto)
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
            //NumberOfTickets = AssigendTickets.Count;
            var t = DatabaseAccesUtil.GetStaffDetails(Id);
            ConvertFromPoco(t);
            AssigendTickets = DatabaseAccesUtil.GetAssginedTickets(Id);

        }
        public override bool Update() => throw new NotImplementedException();
        #endregion
        #region UnquieMethods

        #endregion
        public override string? ToString()
        {
            return base.ToString() +
            $"|NumTicket: {NumberOfTickets,-8}" +
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
                StaffType       = UserType.SupportMember.ToString(),
                ServerSystem          = "null",
                CourseID        = 0,
                NumberOfTickets = this.NumberOfTickets
            };
        }
        public IStaff ConvertFromPoco(StaffPoco staff)
        {

            StaffID         = staff.StaffID;
            Campus          = staff.Campus;
            DepartmentId    = (int)staff.DepartmentID;
            NumberOfTickets = staff.NumberOfTickets;
            return this;
        }
    }
}
