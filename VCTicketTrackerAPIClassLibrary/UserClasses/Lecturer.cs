using VCTicketTrackerAPIClassLibrary.Auth;
using VCTicketTrackerAPIClassLibrary.Orginisation;
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
    ///     "lockoutEnd": "2024-11-20T11:07:50.150Z",
    ///     "lockoutEnabled": true,
    ///     "accessFailedCount": 0,
    ///     "securityStamp": "string",
    ///     "concurrencyStamp": "string",
    ///     "fcmToken": "string",
    ///     "staffID": 0,
    ///     "campus": "string",
    ///     "departmentId": 0,
    ///     "department": 
    ///     {
    ///         "departmentID": 0,
    ///         "departmentName": "string"
    ///     },
    ///     "modules": 
    ///     [
    ///         {
    ///             "moduleCode": "string",
    ///             "modGroup": 0,
    ///             "staffID": 0
    ///         }
    ///     ]
    ///}
    /// </code>
    /// </example>
    /// </summary>
    public class Lecturer : ApplicationUser, ILecturer
    {
        #region Props
        public long StaffID { get; set; }
        public string Campus { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public List<StaffModule> Modules { get; set; }





        #endregion
        #region Constructors
        public Lecturer(long id, string firstName, string lastName, string username, string password, UserType userType)
    : base(id, firstName, lastName, username, password, userType)
        {
        }
        public Lecturer() : base()
        {
        }

        public Lecturer(int departmentId) : base()
        {
            DepartmentId=departmentId;
        }
        public Lecturer(ApplicationUser user) : base(user)
        {
        }

        public Lecturer(string firstName, string lastName, string userName, string email, string userType, string passwordHash, string phoneNumber) : base(firstName, lastName, userName, email, userType, passwordHash, phoneNumber)
        {
        }

        public Lecturer(RegisterDto registerDto) : base(registerDto)
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
            Modules = DatabaseAccesUtil.GetStaffModule(Id);
        }

        public override bool Update() => throw new NotImplementedException();
        #endregion
        #region UnquieMethods
        public bool AddMoudles(StaffModule staff)
        {
            return DatabaseAccesUtil.ModuleInsert(staff);
        }

        public bool AddMoudles(List<StaffModule> mods)
        {
            try
            {

                foreach(var module in mods)
                {
                    DatabaseAccesUtil.ModuleInsert(module);
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public void Chat()
        {
            // TODO Add chat Function
        }

        public void UpdateStudent()
        {

        }

        public void UpdateTicket()
        {

        }
        /// <summary>
        /// Formats modules into a string for display
        /// </summary>
        /// <returns> Module toString in a sting </returns>
        public string PrintMod()
        {
            string res = "\n";
            if(Modules != null)
            {

                foreach(var item in Modules)
                {
                    res += item.ToString()+"\n";
                }
            }
            return res;
        }
        #endregion
        public override string? ToString()
        {
            return base.ToString() +
            //            $"|DepartID: {Department.DepartmentID,-9}\n" +
            //            $"|Modules: {PrintMod()}\n" +
            $"\n";
        }

        public StaffPoco ConvertToPoco()
        {
            return new StaffPoco {
                StaffID         = this.StaffID,
                UserID          = this.Id,
                DepartmentID    = this.DepartmentId,
                Campus          = this.Campus,
                StaffType       = UserType.Lecturer.ToString(),
                ServerSystem          = "null",
                CourseID        = 0,
                NumberOfTickets = 0
            };
        }
        public IStaff ConvertFromPoco(StaffPoco staff)
        {

            StaffID = staff.StaffID;
            Campus = staff.Campus;
            DepartmentId = (int)staff.DepartmentID;
            return this;
        }
    }
}