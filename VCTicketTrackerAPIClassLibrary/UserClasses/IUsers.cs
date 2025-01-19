using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using VCTicketTrackerAPIClassLibrary.Orginisation;
using VCTicketTrackerAPIClassLibrary.Tickets;
using VCTicketTrackerAPIClassLibrary.UserClasses.Poco;

namespace VCTicketTrackerAPIClassLibrary.UserClasses
{
    // User 'role'
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserType
    {
        // Order is also Priv Level
        ServerAdmin = 0,
        Administrator = 1,
        SupportMember  =2,
        Guest = 3,
        Lecturer = 4,
        Student = 5,
    }

    public interface IApplicationUser
    {
        long Id { get; set; }
        [ProtectedPersonalData]
        string? UserName { get; set; }
        [JsonIgnore]
        string? NormalizedUserName { get; set; }
        [ProtectedPersonalData]
        string? Email { get; set; }
        [JsonIgnore]
        string? NormalizedEmail { get; set; }

        UserType UsersTypes { get; set; }

        [PersonalData]
        [JsonIgnore]
        bool EmailConfirmed { get; set; }
        string? PasswordHash { get; set; }
        [JsonIgnore]
        string? SecurityStamp { get; set; }
        [JsonIgnore]
        string? ConcurrencyStamp { get; set; }
        [ProtectedPersonalData]
        [JsonIgnore]
        string? PhoneNumber { get; set; }
        [PersonalData]
        [JsonIgnore]
        bool PhoneNumberConfirmed { get; set; }
        [PersonalData]
        [JsonIgnore]
        bool TwoFactorEnabled { get; set; }
        [JsonIgnore]
        DateTimeOffset? LockoutEnd { get; set; }
        [JsonIgnore]
        bool LockoutEnabled { get; set; }
        [JsonIgnore]
        int AccessFailedCount { get; set; }

        string fcmToken { get; set; }
        public abstract void GetDetails();
        public abstract bool Insert();
        public abstract bool Update();
        //public abstract bool ConvertToPoco();
        //public abstract bool ConvertFromPoco();

    }


    // Represents Table and is user
    public interface IStudent : IApplicationUser
    {
        string STNumber { get; set; }
        string Campus { get; set; }
        int CourseId { get; set; }
        Course? Courses { get; set; }
        string ModeOfDelivery { get; set; }
        int ModGroup { get; set; }
        string Qualification { get; set; }
        string RegistrationType { get; set; }
        abstract StudentPoco ConvertToPoco();

        abstract Student ConvertFromPoco(StudentPoco student);

    }
    // Represents table is not a user instance
    public interface IStaff
    {
        long StaffID { get; set; }
        string Campus { get; set; }
        abstract StaffPoco ConvertToPoco();
        abstract IStaff ConvertFromPoco(StaffPoco staff);
    }
    // Staff
    public interface ILecturer : IApplicationUser, IStaff
    {
        public int DepartmentId { get; set; }
        Department Department { get; set; }
        List<StaffModule> Modules { get; set; }
    }

    //Staff
    public interface IAdministrator : IApplicationUser, IStaff
    {
        public int DepartmentId { get; set; }
        Department Department { get; set; }
    }

    //Staff
    public interface ISupportMember : IApplicationUser, IStaff
    {
        public int DepartmentId { get; set; }
        List<Ticket> AssigendTickets { get; set; }
        Department Department { get; set; }
        int? NumberOfTickets { get; set; }
    }

    // staff
    public interface IServerAdmin : IApplicationUser, IStaff
    {
        string ServerSystem { get; set; }
    }


    public class StudentDTO
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } = string.Empty;

        [JsonPropertyName("lastName")]
        public string LastName { get; set; } = string.Empty;

        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;

        [JsonPropertyName("campus")]
        public string Campus { get; set; } = string.Empty;

        [JsonPropertyName("courseId")]
        public int CourseId { get; set; }

        [JsonPropertyName("course")]
        public Course? Course { get; set; }
    }

}
/*
        public class StaffDto : RegisterDto
        {
            public string Campus { get; set; }
            public string StaffType { get; set; }
            public string ServerSystem { get; set; }
            public int? CourseID { get; set; }
            public int? NumberOfTickets { get; set; }
            public int? DepartmentId { get; set; } = null;
            public StaffPoco ConvertToPoco()
            {
                return new StaffPoco {
                    StaffID = 0, // Assuming StaffID is generated later
                    UserID = 0, // Set the UserID after user is created
                    DepartmentID = this.DepartmentId,
                    Campus = this.Campus,
                    StaffType = this.StaffType,
                    ServerSystem = this.ServerSystem,
                    CourseID = this.CourseID,
                    NumberOfTickets = this.NumberOfTickets
                };
            }
        }
    }

    public class StudentDTO
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } = string.Empty;

        [JsonPropertyName("lastName")]
        public string LastName { get; set; } = string.Empty;

        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;

        [JsonPropertyName("campus")]
        public string Campus { get; set; } = "Sandton";  // Default campus

        [JsonPropertyName("courseId")]
        public int CourseId { get; set; }

        [JsonPropertyName("course")]
        public Course? Course { get; set; }  // Optionally, the Course details could be included

        [JsonPropertyName("modeOfDelivery")]
        public string ModeOfDelivery { get; set; } = "Contact";  // Default mode

        [JsonPropertyName("qualification")]
        public string Qualification { get; set; } = "N/A";  // Default qualification

        [JsonPropertyName("registrationType")]
        public string RegistrationType { get; set; } = "Full-Time";  // Default registration type

        [JsonPropertyName("modGroup")]
        public int ModGroup { get; set; } = 1;  // Default mod group

        // Convert StudentDTO to StudentPoco
        public StudentPoco ConvertToPoco()
        {
            return new StudentPoco {
                STNumber = this.Username,
                UserID = this.Id,
                CourseID = this.CourseId,
                ModeOfDelivery = this.ModeOfDelivery,
                Campus = this.Campus,
                RegistrationType = this.RegistrationType,
                Qualification = this.Qualification,
                ModGroup = this.ModGroup
            };
        }

        // Convert from StudentPoco to StudentDTO
        public static StudentDTO ConvertFromPoco(StudentPoco poco)
        {
            return new StudentDTO {
                Id = poco.UserID ?? 0,  // Ensure ID is valid
                Username = poco.STNumber ?? string.Empty,
                CourseId = poco.CourseID ?? 0,
                ModeOfDelivery = poco.ModeOfDelivery ?? "Contact",
                Campus = poco.Campus ?? "Sandton",
                RegistrationType = poco.RegistrationType ?? "Full-Time",
                Qualification = poco.Qualification ?? "N/A",
                ModGroup = poco.ModGroup ?? 1
            };
        }*/

/*
public class StudentDTO
{
[JsonPropertyName("id")]
public long Id { get; set; }

[JsonPropertyName("firstName")]
public string FirstName { get; set; } = string.Empty;

[JsonPropertyName("lastName")]
public string LastName { get; set; } = string.Empty;

[JsonPropertyName("username")]
public string Username { get; set; } = string.Empty;

[JsonPropertyName("password")]
public string Password { get; set; } = string.Empty;

[JsonPropertyName("campus")]
public string Campus { get; set; } = string.Empty;

[JsonPropertyName("courseId")]
public int CourseId { get; set; }

[JsonPropertyName("course")]
public Course? Course { get; set; }
}*/
