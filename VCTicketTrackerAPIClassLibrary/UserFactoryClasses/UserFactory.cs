using VCTicketTrackerAPIClassLibrary.Auth;
using VCTicketTrackerAPIClassLibrary.UserClasses;
using VCTicketTrackerAPIClassLibrary.UserClasses.Dto;

namespace VCTicketTrackerAPIClassLibrary.UserFactoryClasses
{
    /// <summary>
    /// Factory for creating Lecturers
    /// </summary>
    public class LecturerFactory : IUsersFactory
    {
        public ApplicationUser CreateUser()
        {
            return new Lecturer();
        }

        public ApplicationUser CreateUser(ApplicationUser user)
        {
            return new Lecturer(user);
        }

        /// <summary>
        /// Create a Lecturer Object from RegisterDto
        /// </summary>
        /// <param name="dto">RegisterDto containing user information</param>
        /// <returns>Newly created Lecturer</returns>
        public ApplicationUser CreateUser(RegisterDto dto)
        {
            return new Lecturer(dto);
        }
    }

    /// <summary>
    /// Factory for creating Students
    /// </summary>
    public class StudentFactory : IUsersFactory
    {
        public ApplicationUser CreateUser()
        {
            return new Student();
        }

        public ApplicationUser CreateUser(ApplicationUser user)
        {
            return new Student(user);
        }

        /// <summary>
        /// Create a Student Object from RegisterDto
        /// </summary>
        /// <param name="dto">RegisterDto containing user information</param>
        /// <returns>Newly created Student</returns>
        public ApplicationUser CreateUser(RegisterDto dto)
        {
            return new Student(dto);
        }
    }

    /// <summary>
    /// Factory for creating Guests
    /// </summary>
    public class GuestFactory : IUsersFactory
    {
        public ApplicationUser CreateUser()
        {
            return new Guest();
        }

        public ApplicationUser CreateUser(ApplicationUser user)
        {
            return new Guest(user);
        }

        /// <summary>
        /// Create a Guest Object from RegisterDto
        /// </summary>
        /// <param name="dto">RegisterDto containing user information</param>
        /// <returns>Newly created Guest</returns>
        public ApplicationUser CreateUser(RegisterDto dto)
        {
            return new Guest(dto);
        }
    }

    /// <summary>
    /// Factory for creating ServerAdmins
    /// </summary>
    public class ServerAdminFactory : IUsersFactory
    {
        public ApplicationUser CreateUser()
        {
            return new ServerAdmin();
        }

        public ApplicationUser CreateUser(ApplicationUser user)
        {
            return new ServerAdmin(user);
        }

        /// <summary>
        /// Create a ServerAdmin Object from RegisterDto
        /// </summary>
        /// <param name="dto">RegisterDto containing user information</param>
        /// <returns>Newly created ServerAdmin</returns>
        public ApplicationUser CreateUser(RegisterDto dto)
        {
            return new ServerAdmin(dto);
        }

    }

    /// <summary>
    /// Factory for creating SupportMembers
    /// </summary>
    public class SupportMemberFactory : IUsersFactory
    {
        public ApplicationUser CreateUser()
        {
            return new SupportMember();
        }

        public ApplicationUser CreateUser(ApplicationUser user)
        {
            return new SupportMember(user);
        }

        /// <summary>
        /// Create a SupportMember Object from RegisterDto
        /// </summary>
        /// <param name="dto">RegisterDto containing user information</param>
        /// <returns>Newly created SupportMember</returns>
        public ApplicationUser CreateUser(RegisterDto dto)
        {
            return new SupportMember(dto);
        }
    }

    /// <summary>
    /// Factory for creating Administrators
    /// </summary>
    public class AdministratorFactory : IUsersFactory
    {
        public ApplicationUser CreateUser()
        {
            return new Administrator();
        }

        public ApplicationUser CreateUser(ApplicationUser user)
        {
            return new Administrator(user);
        }

        /// <summary>
        /// Create an Administrator Object from RegisterDto
        /// </summary>
        /// <param name="dto">RegisterDto containing user information</param>
        /// <returns>Newly created Administrator</returns>
        public ApplicationUser CreateUser(RegisterDto dto)
        {
            return new Administrator(dto);
        }
    }
}
