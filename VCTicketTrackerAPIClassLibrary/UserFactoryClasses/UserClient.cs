using System.Diagnostics;
using VCTicketTrackerAPIClassLibrary.Auth;
using VCTicketTrackerAPIClassLibrary.UserClasses;
using VCTicketTrackerAPIClassLibrary.UserClasses.Dto;

namespace VCTicketTrackerAPIClassLibrary.UserFactoryClasses
{
    /// <summary>
    /// Intended to help create Users of each intended type for Class based actions
    /// </summary>
    public class UserClient
    {
        private ApplicationUser? _client;
        /// <summary>
        /// Determins Which Factory to use to create the user
        /// </summary>
        /// <param name="UT"></param>
        public UserClient(UserType UT)
        {
            switch(UT)
            {
                case UserType.Student:
                    _client = new StudentFactory().CreateUser();
                    break;
                case UserType.SupportMember:
                    _client = new SupportMemberFactory().CreateUser();
                    break;
                case UserType.Lecturer:
                    _client = new LecturerFactory().CreateUser();
                    break;
                case UserType.Administrator:
                    _client = new AdministratorFactory().CreateUser();
                    break;
                case UserType.ServerAdmin:
                    _client = new ServerAdminFactory().CreateUser();
                    break;
                case UserType.Guest:
                    _client = new GuestFactory().CreateUser();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(UT),
                        $"No factory available for user type: {UT}");

            }
        }
        public UserClient(ApplicationUser appUser)
        {
            try
            {
                if(appUser != null)
                {
                    switch(appUser.UsersTypes)
                    {
                        case UserType.Student:
                            _client = new StudentFactory().CreateUser(appUser);
                            break;
                        case UserType.SupportMember:
                            _client = new SupportMemberFactory().CreateUser(appUser);
                            break;
                        case UserType.Lecturer:
                            _client = new LecturerFactory().CreateUser(appUser);
                            break;
                        case UserType.Administrator:
                            _client = new AdministratorFactory().CreateUser(appUser);
                            break;
                        case UserType.ServerAdmin:
                            _client = new ServerAdminFactory().CreateUser(appUser);
                            break;
                        case UserType.Guest:
                            _client = new GuestFactory().CreateUser(appUser);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(appUser),
                                $"No factory available for user type: {appUser.GetType()}");
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
        public UserClient(RegisterDto appUser)
        {
            switch(Enum.Parse<UserType>(appUser.UserType))
            {
                case UserType.Student:
                    _client = new StudentFactory().CreateUser(appUser);
                    break;
                case UserType.SupportMember:
                    _client = new SupportMemberFactory().CreateUser(appUser);
                    break;
                case UserType.Lecturer:
                    _client = new LecturerFactory().CreateUser(appUser);
                    break;
                case UserType.Administrator:
                    _client = new AdministratorFactory().CreateUser(appUser);
                    break;
                case UserType.ServerAdmin:
                    _client = new ServerAdminFactory().CreateUser(appUser);
                    break;
                case UserType.Guest:
                    _client = new GuestFactory().CreateUser(appUser);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(appUser),
                        $"No factory available for user type: {appUser.GetType()}");

            }
        }
        /// <summary>
        /// Returns the created User
        /// </summary>
        /// <returns> User of Type<T></returns>
        public ApplicationUser? Get()
        {
            return _client;
        }
    }
}