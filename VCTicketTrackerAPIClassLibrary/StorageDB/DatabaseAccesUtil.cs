using System.Data;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using VCTicketTrackerAPIClassLibrary.Orginisation;



namespace VCTicketTrackerAPIClassLibrary.StorageDB
{
    public partial class DatabaseAccesUtil
    {
        public DatabaseAccesUtil()
        {

        }
        public static string strUserInsert =
                $"INSERT INTO Users (FirstName, LastName, Username, Password, UserType) " +
                $"Values (@FirstName, @LastName, @Username, @Password, @UserType); ";
        public static string strStaffInsert =
                $"INSERT INTO Staff(StaffID, UserID, StaffType, CourseId, Deparment, NumberOfTickets, ServerSystem) " +
                $"VALUES(concat('', trim(str(@@IDENTITY))), @@IDENTITY, @StaffType, @CourseId, @DepartmentID, @NumberOfTickets, @ServerSystem);";



        public static List<Course>? GetCourse()
        {
            Debug.WriteLine($"Get all Courses:", "DatabaseUtil-SELECT");
            Debug.IndentLevel++;
            Debug.IndentLevel--;

            string strSelect = $"SELECT * FROM Courses;";

            SqlCommand cmdSelect = new SqlCommand();
            cmdSelect.CommandText = strSelect;

            List<Course> fetching = new List<Course>();
            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                cmdSelect.Connection = conn;
                conn.Open();
                using(SqlDataReader userReader = cmdSelect.ExecuteReader())
                {
                    while(userReader.Read())
                    {
                        var course        = new Course();
                        course.CourseID     = (int)userReader["CourseId"];
                        course.CourseName   = (string)userReader["CourseName"];
                        //course.CourseCode = (string)userReader["CourseCode"];
                        course.School       = (string)userReader["School"];
                        course.Years        = (int)userReader["Years"];
                        course.NQFLevel     = (int)userReader["Nqf"];
                        course.Credits      = (int)userReader["Credits"];
                        fetching.Add(course);
                    }
                }
                conn.Close();
            }
            return fetching;
        }
        public static Course? GetCourse(int ID)
        {
            Debug.WriteLine("Get List Users", "Select");
            string strSelect = $"SELECT * FROM Course WHERE CourseId = @CourseId;";

            SqlCommand cmdSelect = new SqlCommand();
            cmdSelect.CommandText = strSelect;
            cmdSelect.Parameters.Add($"@CourseId", SqlDbType.Int).Value = ID;
            Course fetching = null;
            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                Debug.WriteLine($"Course", "GetCourse");

                cmdSelect.Connection = conn;
                conn.Open();
                using(SqlDataReader userReader = cmdSelect.ExecuteReader())
                {
                    while(userReader.Read())
                    {
                        fetching          = new Course();
                        fetching.CourseID     = (int)userReader["CourseId"];
                        fetching.CourseName   = (string)userReader["CourseName"];
                        //fetchingse.CourseCode = (string)userReader["CourseCode"];
                        fetching.School       = (string)userReader["School"];
                        fetching.Years        = (int)userReader["Years"];
                        fetching.NQFLevel     = (int)userReader["Nqf"];
                        fetching.Credits      = (int)userReader["Credits"];
                        break;
                    }
                }
                conn.Close();
            }
            return fetching;
        }



        public static Course? GetStudentCourse(long UserID)
        {
            Debug.WriteLine($"Get Students Courses:", "Database-SELECT");
            Debug.IndentLevel++;
            Debug.WriteLine($"UserID:{UserID}");
            Debug.IndentLevel--;

            string strSelect =
                    $"SELECT * FROM Students \r" +
                    $"\nJOIN Course ON Students.CourseId = Course.CourseId\r" +
                    $"\nWHERE UserID = @UserID;";

            SqlCommand cmdSelect = new SqlCommand();
            cmdSelect.CommandText = strSelect;
            cmdSelect.Parameters.Add($"@UserID", SqlDbType.Int).Value = UserID;
            Course fetching = null;
            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                Debug.WriteLine($"Course", "GetCourse");

                cmdSelect.Connection = conn;
                conn.Open();
                using(SqlDataReader userReader = cmdSelect.ExecuteReader())
                {
                    while(userReader.Read())
                    {
                        fetching          = new Course();
                        fetching.CourseID     = (int)userReader["CourseId"];
                        fetching.CourseName   = (string)userReader["CourseName"];
                        //fetchingse.CourseCode = (string)userReader["CourseCode"];
                        fetching.School       = (string)userReader["School"];
                        fetching.Years        = (int)userReader["Years"];
                        fetching.NQFLevel     = (int)userReader["Nqf"];
                        fetching.Credits      = (int)userReader["Credits"];
                        break;
                    }
                }
                conn.Close();
            }
            return fetching;
        }


        public static Course? GetLecturerCourse(long UserID)
        {
            Debug.WriteLine($"Get Lecturer Courses:", "Database-SELECT");
            Debug.IndentLevel++;
            Debug.WriteLine($"UserID:{UserID}");
            Debug.IndentLevel--;

            string strSelect =
                    $"SELECT * FROM Staff " +
                    $" JOIN Course ON Staff.CourseId = Course.CourseId " +
                    $" WHERE UserID = @UserID;";

            SqlCommand cmdSelect = new SqlCommand();
            cmdSelect.CommandText = strSelect;
            cmdSelect.Parameters.Add($"@UserID", SqlDbType.BigInt).Value = UserID;
            Course fetching = null;
            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                cmdSelect.Connection = conn;
                conn.Open();
                using(SqlDataReader userReader = cmdSelect.ExecuteReader())
                {
                    while(userReader.Read())
                    {
                        fetching          = new Course();
                        fetching.CourseID     = (int)userReader["CourseId"];
                        fetching.CourseName   = (string)userReader["CourseName"];
                        //fetchingse.CourseCode = (string)userReader["CourseCode"];
                        fetching.School       = (string)userReader["School"];
                        fetching.Years        = (int)userReader["Years"];
                        fetching.NQFLevel     = (int)userReader["Nqf"];
                        fetching.Credits      = (int)userReader["Credits"];
                        break;
                    }
                }
                conn.Close();
            }
            return fetching;
        }
    }
}