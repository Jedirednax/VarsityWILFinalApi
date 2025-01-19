using System.Data;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using VCTicketTrackerAPIClassLibrary.Orginisation;

namespace VCTicketTrackerAPIClassLibrary.StorageDB
{
    public partial class DatabaseAccesUtil
    {
        public static List<StudentModule>? GetStudentModule(long UserID)
        {
            Debug.WriteLine($"Get Students Modules:", "Database-SELECT");
            Debug.IndentLevel++;
            Debug.WriteLine($"UserID:{UserID}");
            Debug.IndentLevel--;

            string strSelect =
                    $"SELECT Modules.ModuleCode ,Modules.ModuleName ,CourseModules.ModuleCredits ,CourseModules.Semester ,CourseModules.Years ,CourseModules.NQFLevel " +
                    $"FROM Students " +
                    $" JOIN Course ON Students.CourseId = Course.CourseId " +
                    $" JOIN CourseModules ON Students.CourseId = CourseModules.CourseId " +
                    $" JOIN Modules ON CourseModules.ModuleCode = Modules.ModuleCode " +
                    $" WHERE UserID = @UserID;";

            SqlCommand cmdSelect = new SqlCommand();
            cmdSelect.CommandText = strSelect;
            cmdSelect.Parameters.Add($"@UserID", SqlDbType.BigInt).Value = UserID;

            List<StudentModule> fetching = new List<StudentModule>();
            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                cmdSelect.Connection = conn;
                conn.Open();
                using(SqlDataReader userReader = cmdSelect.ExecuteReader())
                {
                    while(userReader.Read())
                    {
                        var module     = new StudentModule(
                            moduleCode   : (string)userReader["ModuleCode"],
                            moduleName   : (string)userReader["ModuleName"],
                            moduleCredits: (int)userReader["ModuleCredits"],
                            semester     : (int)userReader["Semester"],
                            years        : (int)userReader["Years"],
                            nQFLevel     : (int)userReader["NQFLevel"]
                        );
                        fetching.Add(module);
                    }
                }
                conn.Close();
            }
            return fetching;
        }

        public static List<StaffModule>? GetStaffModule(long UserID)
        {
            Debug.WriteLine($"Get Staff Modules:", "Database-SELECT");
            Debug.IndentLevel++;
            Debug.WriteLine($"UserID:{UserID}");
            Debug.IndentLevel--;

            string strSelect =
                $"SELECT Modules.ModuleCode, ModuleName, ModGroup, StaffID FROM StaffModules " +
                $"JOIN Modules ON Modules.ModuleCode = StaffModules.ModuleCode " +
                $"WHERE StaffModules.StaffID = @StaffID; ";

            SqlCommand cmdSelect = new SqlCommand();
            cmdSelect.CommandText = strSelect;
            cmdSelect.Parameters.Add($"@StaffID", SqlDbType.BigInt).Value = UserID;
            //cmd.Parameters.Add($"@CourseId", SqlDbType.Int).Value = ID;
            List<StaffModule> fetching = new List<StaffModule>();
            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                cmdSelect.Connection = conn;
                conn.Open();
                using(SqlDataReader userReader = cmdSelect.ExecuteReader())
                {
                    while(userReader.Read())
                    {
                        var module     = new StaffModule(
                            moduleCode   : (string)userReader["ModuleCode"],
                            moduleName   : (string)userReader["ModuleName"],
                            modGroup     : (int)userReader["ModGroup"],
                            staffID      : (long)userReader["StaffID"]
                        );
                        fetching.Add(module);
                    }
                }
                conn.Close();
            }
            return fetching;
        }

        public static bool ModuleInsert(StaffModule stm)
        {
            Debug.WriteLine($"Add StaffModules:", "Database-INSERT");
            Debug.IndentLevel++;
            Debug.WriteLine($"StaffModule:\n\t{stm}");
            Debug.IndentLevel--;

            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();
                string strModuleInsert =
                $"INSERT INTO StaffModules(StaffID,ModuleCode,ModGroup) "+
                    $"Values(@StaffID, @ModuleCode, @ModGroup); ";

                SqlCommand cmdInsert = new SqlCommand(strModuleInsert);

                cmdInsert.Parameters.Add($"@StaffID", SqlDbType.BigInt).Value = stm.StaffID;
                cmdInsert.Parameters.Add($"@ModuleCode", SqlDbType.VarChar).Value = stm.ModuleCode;
                cmdInsert.Parameters.Add($"@ModGroup", SqlDbType.Int).Value   = stm.ModGroup;
                cmdInsert.Connection = conn;
                try
                {
                    if(cmdInsert.ExecuteNonQuery() >= 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.Message, "SqlInsertError");
                    return false;
                }
            }
        }
    }
}
