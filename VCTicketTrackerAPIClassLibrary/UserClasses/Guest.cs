using VCTicketTrackerAPIClassLibrary.Auth;
using VCTicketTrackerAPIClassLibrary.StorageDB;
using VCTicketTrackerAPIClassLibrary.UserClasses.Dto;

namespace VCTicketTrackerAPIClassLibrary.UserClasses
{
    public class Guest : ApplicationUser
    {
        #region Props
        #endregion
        #region Constructors
        public Guest(long id, string firstName, string lastName, string username, string password, UserType userType)
    : base(id, firstName, lastName, username, password, userType)
        {
        }
        public Guest() : base()
        {
        }

        public Guest(ApplicationUser user) : base(user)
        {
        }

        public Guest(string firstName, string lastName, string userName, string email, string userType, string passwordHash, string phoneNumber) : base(firstName, lastName, userName, email, userType, passwordHash, phoneNumber)
        {
        }

        public Guest(RegisterDto registerDto) : base(registerDto)
        {
        }

        #endregion
        #region Overrides
        public override void GetDetails()
        {
            throw new NotImplementedException();
        }

        public override bool Insert()
        {
            long? userId = DatabaseAccesUtil.InsertUsers(this);
            return userId.HasValue;
        }

        public override bool Update() => throw new NotImplementedException();
        #endregion
        #region UnquieMethods
        #endregion
        public override string? ToString()
        {
            return base.ToString() +
            $"|\n";
        }
    }
}


