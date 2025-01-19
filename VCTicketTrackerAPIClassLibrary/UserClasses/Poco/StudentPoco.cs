namespace VCTicketTrackerAPIClassLibrary.UserClasses.Poco
{
    /// <summary>
    /// <example>
    /// <code>
    /// {
    ///   "stNumber": "string",
    ///   "userID": 0,
    ///   "courseID": 0,
    ///   "modeOfDelivery": "string",
    ///   "campus": "string",
    ///   "registrationType": "string",
    ///   "qualification": "string",
    ///   "modGroup": 0
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public class StudentPoco
    {
        /// <value>
        /// 
        /// </value>
        public string STNumber { get; set; }
        /// <value>
        /// 
        /// </value>
        public long? UserID { get; set; }
        /// <value>
        /// 
        /// </value>
        public int? CourseID { get; set; }
        /// <value>
        /// 
        /// </value>
        public string ModeOfDelivery { get; set; }
        /// <value>
        /// 
        /// </value>
        public string Campus { get; set; }
        /// <value>
        /// 
        /// </value>
        public string RegistrationType { get; set; }
        /// <value>
        /// 
        /// </value>
        public string Qualification { get; set; }
        /// <value>
        /// 
        /// </value>
        public int? ModGroup { get; set; }
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
