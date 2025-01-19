using System.Diagnostics;
using System.Text.Json;
using VCTicketTrackerAPIClassLibrary.Auth;
using VCTicketTrackerAPIClassLibrary.UserClasses;
using VCTicketTrackerAPIClassLibrary.UserFactoryClasses;

namespace VCTicketTrackerAPIClassLibrary
{
    /// <summary>
    /// Class globaly accesible intended for common methods that are not specific
    /// to a object instance but of type
    /// As well as Regularly Reuired Objects
    /// Recomended to be implimented in evry File
    /// </summary>
    public static class Extensions
    {
        public static JsonSerializerOptions serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented        = true,
            ReadCommentHandling  = JsonCommentHandling.Skip,
            AllowTrailingCommas  = true,
        };

        public static string GetUType(string userType)
        {

            switch(Enum.Parse<UserType>(userType))
            {
                case UserType.Student:
                    return $"st";
                case UserType.SupportMember:
                    return $"sm";
                case UserType.Lecturer:
                    return $"lr";
                case UserType.Administrator:
                    return $"ad";
                case UserType.ServerAdmin:
                    return $"sa";
                case UserType.Guest:
                    return $"gu";
                default:
                    return $"ur";
            }
        }
        public static ApplicationUser? DetectUser(string userType)
        {
            Debug.WriteLine($"Detecting User Type:{userType}", "Extension Action");
            if(userType == "Student")
            {
                return new UserClient(UT: UserType.Student).Get();
            }
            else if(userType == "Administrator")
            {
                return new UserClient(UserType.Administrator).Get();
            }
            else if(userType == "SupportMember")
            {
                return new UserClient(UserType.SupportMember).Get();
            }
            else if(userType == "Lecturer")
            {
                return new UserClient(UserType.Lecturer).Get();
            }
            else if(userType == "ServerAdmin")
            {
                return new UserClient(UserType.ServerAdmin).Get();
            }
            else if(userType == "Guest")
            {
                return new UserClient(UserType.Guest).Get();
            }
            else
            {
                return null;
            }
        }
    }
}
