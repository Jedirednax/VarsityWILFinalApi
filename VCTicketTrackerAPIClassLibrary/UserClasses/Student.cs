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
    /// <see cref="Course"/>
    /// <see cref="StudentModule"/>
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
    ///     "lockoutEnd": "2024-11-20T11:06:16.115Z",
    ///     "lockoutEnabled": true,
    ///     "accessFailedCount": 0,
    ///     "securityStamp": "string",
    ///     "concurrencyStamp": "string",
    ///     "fcmToken": "string",
    ///     
    ///     "stNumber": "string",
    ///     "userID": 0,
    ///     "campus": "string",
    ///     "courseId": 0,
    ///     "modeOfDelivery": "string",
    ///     "registrationType": "string",
    ///     "qualification": "string",
    ///     "moduleGroup": 0,
    ///     "courses": 
    ///     {
    ///         "courseID": 0,
    ///         "courseCode": "string",
    ///         "courseName": "string",
    ///         "school": "string",
    ///         "years": 0,
    ///         "nqfLevel": 0,
    ///         "credits": 0,
    ///         "modules": 
    ///         [
    ///             {
    ///                 "moduleCredits": 0,
    ///                 "years": 0,
    ///                 "semester": 0,
    ///                 "nqfLevel": 0
    ///             }
    ///         ]
    ///     }
    ///}
    /// </code>
    /// <code>
    /// {
    ///    "firstName": 	"Jhon",
    ///    "lastName": 	"Doe",
    ///    "userName": 	"DemoStudent",
    ///    "email": 		"DemoStudent@vctickettracker.com",
    ///    "userType": 	"Student",
    ///    "passwordHash":     "Password1!",
    ///	"courseId": 	97600,
    ///    "modeOfDelivery":   "Contact",
    ///    "campus":           "Sandton",
    ///    "registrationType": "Full",
    ///    "modGroup":         3,
    ///    "qualification":    "Bacd97600"
    ///}
    /// </code>
    /// </example>
    /// </summary>
    public class Student : ApplicationUser, IStudent//, IUser
    {
        #region Props
        public string? STNumber { get; set; }
        public long? UserID { get; set; }
        /// <summary>
        /// The campus where the student is enrolled.
        /// </summary>
        [JsonPropertyName("campus")]
        public string Campus { get; set; }

        /// <summary>
        /// The ID of the course the student is enrolled in.
        /// </summary>
        [JsonPropertyName("courseId")]
        public int CourseId { get; set; }

        /// <summary>
        /// Mode of delivery for the course (e.g., online, in-person).
        /// </summary>
        [JsonPropertyName("modeOfDelivery")]
        public string ModeOfDelivery { get; set; }

        /// <summary>
        /// The type of registration for the student.
        /// </summary>
        [JsonPropertyName("registrationType")]
        public string RegistrationType { get; set; }

        /// <summary>
        /// The qualification level of the student.
        /// </summary>
        [JsonPropertyName("qualification")]
        public string Qualification { get; set; }

        /// <summary>
        /// The module group the student belongs to.
        /// </summary>
        [JsonPropertyName("moduleGroup")]
        public int ModGroup { get; set; }

        public Course? Courses { get; set; }

        #endregion
        #region Constructors
        public Student(long id, string firstName, string lastName, string username, string password, UserType userType)
    : base(id, firstName, lastName, username, password, userType)
        {
        }
        public Student() : base() { }



        public Student(string campus, int courseID, string modeOfDelivery, int modGroup, string registrationType) : base()
        {
            Campus=campus;
            CourseId=courseID;
            ModeOfDelivery=modeOfDelivery;
            ModGroup=modGroup;
            RegistrationType=registrationType;
        }
        public Student(ApplicationUser user) : base(user)
        {
        }

        public Student(string firstName, string lastName, string userName, string email, string userType, string passwordHash, string phoneNumber) : base(firstName, lastName, userName, email, userType, passwordHash, phoneNumber)
        {
        }

        public Student(RegisterDto registerDto) : base(registerDto)
        {
            if(registerDto is StudentDto studentDto)
            {
                var t           = ConvertFromPoco(studentDto.ConvertToPoco());
                Campus          =t.Campus;
                CourseId        =t.CourseId;
                ModeOfDelivery  =t.ModeOfDelivery;
                ModGroup        =t.ModGroup;
                RegistrationType=t.RegistrationType;
            }
            else
            {
                throw new InvalidCastException("StudentDto is not of type RegisterDto");
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

                return DatabaseAccesUtil.InsertStudent(this);
            }

            return false;
        }
        public override void GetDetails()
        {

            Courses = GetMyCourse();
            CourseId = Courses.CourseID;
        }
        public override bool Update()
        {
            throw new NotImplementedException();
        }
        #endregion
        #region UnquieMethods
        private Course? GetMyCourse()
        {
            var myCourses = DatabaseAccesUtil.GetStudentCourse(this.Id);
            myCourses.GetCourseModules(Id);
            return myCourses;

        }
        #endregion
        public override string? ToString()
        {
            return base.ToString()+
            $"|CourseId: {CourseId,-9}\n";
            //$"|{Courses.ToString()}";
        }

        public StudentDTO toDto()
        {
            return new StudentDTO {
                Id =        this.Id, // Assuming Id comes from AppUser
                FirstName = this.FirstName,
                LastName =  this.LastName,
                Username =  this.Email, // Assuming UserName comes from AppUser
                Password =  this.PasswordHash, // Be cautious with this field
                Campus =    this.Campus,
                CourseId =  this.CourseId,
                Course =    this.Courses // Assuming Courses is of the same type as DTO
            };
        }

        public StudentPoco ConvertToPoco()
        {
            return new StudentPoco() {
                STNumber         = this.STNumber,
                UserID           = this.Id,
                CourseID         = this.CourseId,
                ModeOfDelivery   = this.ModeOfDelivery,
                Campus           = this.Campus,
                RegistrationType = this.RegistrationType,
                Qualification    = this.Qualification,
                ModGroup         = this.ModGroup
            };
        }

        public Student ConvertFromPoco(StudentPoco student)
        {

            this.STNumber         = student.STNumber;
            this.CourseId         = (int)student.CourseID;
            this.ModeOfDelivery   = student.ModeOfDelivery;
            this.Campus           = student.Campus;
            this.RegistrationType = student.RegistrationType;
            this.Qualification    = student.Qualification;
            this.ModGroup         = (int)student.ModGroup;
            return this;

        }
    }

}
