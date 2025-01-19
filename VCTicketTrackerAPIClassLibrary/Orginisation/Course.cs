using System.Diagnostics;
using VCTicketTrackerAPIClassLibrary.StorageDB;

namespace VCTicketTrackerAPIClassLibrary.Orginisation
{
    /// <summary>
    /// <example>
    /// <code>
    /// {
    ///  "courseID": 0,
    ///  "courseCode": "string",
    ///  "courseName": "string",
    ///  "school": "string",
    ///  "years": 0,
    ///  "nqfLevel": 0,
    ///  "credits": 0,
    ///  "modules": [
    ///    {
    ///      "moduleCredits": 0,
    ///      "years": 0,
    ///      "semester": 0,
    ///      "nqfLevel": 0
    ///    }
    ///  ]
    ///}
    /// </code>
    /// </example>
    /// </summary>
    public class Course
    {

        public int CourseID { get; set; }
        public string? CourseCode { get; set; }
        public string? CourseName { get; set; }
        public string? School { get; set; }
        public int? Years { get; set; }
        public int? NQFLevel { get; set; }
        public int? Credits { get; set; }

        // TODO Delacr modules a lazy [Enhancement]
        public List<StudentModule> Modules { get; set; } = new List<StudentModule>();

        public Course()
        {
        }
        public Course(int courseID, string? courseCode, string? courseName, string? school, int? years, int? nQFLevel, int? credits)
        {
            CourseID = courseID;
            CourseCode = courseCode;
            CourseName = courseName;
            School = school;
            Years = years;
            NQFLevel = nQFLevel;
            Credits = credits;
        }

        /// <summary>
        /// Querys the database and get a course's modules from a user ID, for current course
        /// </summary>
        /// <param name="UserID"> The Users id, linked to the course </param>
        public void GetCourseModules(long UserID)
        {
            try
            {
                Modules = DatabaseAccesUtil.GetStudentModule(UserID);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
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

        public override string? ToString()
        {
            return
                $"|Name: {CourseName}\n" +
                $"|  ID: {CourseID,-10}" +
                //$"|Code: {CourseCode }" +
                //$"Schl: {School     }" +
                $"|Year: {Years,-10}" +
                $"|NQFL: {NQFLevel,-10}" +
                $"|Cred: {Credits,-10}"+
            $"Modules:\n{PrintMod()}";
        }
    }
}
