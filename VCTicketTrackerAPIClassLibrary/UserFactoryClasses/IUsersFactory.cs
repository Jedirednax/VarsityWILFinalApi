using VCTicketTrackerAPIClassLibrary.Auth;
using VCTicketTrackerAPIClassLibrary.UserClasses.Dto;

namespace VCTicketTrackerAPIClassLibrary.UserFactoryClasses
{
    /// <summary>
    /// Interface used to create Generic users template
    /// </summary>
    public interface IUsersFactory
    {
        /// <summary>
        /// Creation of User type template
        /// </summary>
        /// <returns></returns>
        public abstract ApplicationUser CreateUser();
        public abstract ApplicationUser CreateUser(ApplicationUser user);
        public abstract ApplicationUser CreateUser(RegisterDto dto);
    }
}
