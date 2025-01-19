using System.Data;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using VCTicketTrackerAPIClassLibrary.Auth;
using VCTicketTrackerAPIClassLibrary.UserClasses;
using VCTicketTrackerAPIClassLibrary.UserClasses.Poco;



namespace VCTicketTrackerAPIClassLibrary.StorageDB
{
    public partial class DatabaseAccesUtil
    {
        public static ApplicationUser SingleUserReader(IDataRecord reader)
        {
            var fetching = new ApplicationUser();

            fetching.Id                 = reader["UserId"] != DBNull.Value ? (long)reader["UserId"] : 0;
            fetching.FirstName          = reader["FirstName"] != DBNull.Value ? (string)reader["FirstName"] : string.Empty;
            fetching.LastName           = reader["LastName"] != DBNull.Value ? (string)reader["LastName"] : string.Empty;
            fetching.UserName           = reader["Username"] != DBNull.Value ? (string)reader["Username"] : string.Empty;
            fetching.NormalizedUserName = reader["NormalizedUserName"] != DBNull.Value ? (string)reader["NormalizedUserName"] : string.Empty;
            fetching.Email              = reader["Email"] != DBNull.Value ? (string)reader["Email"] : string.Empty;
            fetching.NormalizedEmail    = reader["NormalizedEmail"] != DBNull.Value ? (string)reader["NormalizedEmail"] : string.Empty;
            fetching.EmailConfirmed     = reader["EmailConfirmed"] != DBNull.Value && (bool)reader["EmailConfirmed"];
            fetching.PasswordHash       = reader["PasswordHash"] != DBNull.Value ? (string)reader["PasswordHash"] : string.Empty;
            fetching.SecurityStamp      = reader["SecurityStamp"] != DBNull.Value ? (string)reader["SecurityStamp"] : string.Empty;
            fetching.ConcurrencyStamp   = reader["ConcurrencyStamp"] != DBNull.Value ? (string)reader["ConcurrencyStamp"] : string.Empty;
            fetching.fcmToken           = reader["FcmToken"] != DBNull.Value ? (string)reader["FcmToken"] : string.Empty;


            // Parse UserType safely
            if(reader["UserType"] != DBNull.Value && Enum.TryParse<UserType>(reader["UserType"].ToString(), out var userType))
            {
                fetching.UsersTypes = userType;
            }
            else
            {
                fetching.UsersTypes = UserType.Guest; // Default or fallback value
            }

            fetching.AuthenticationType = reader["AuthenticationType"] != DBNull.Value ? (string)reader["AuthenticationType"] : string.Empty;
            fetching.IsAuthenticated = reader["IsAuthenticated"] != DBNull.Value && (bool)reader["IsAuthenticated"];

            return fetching;
        }
        public static StaffPoco SingleStaffReader(IDataRecord reader)
        {
            var staff = new StaffPoco
            {
                StaffID         = (long)reader["StaffID"],
                UserID          = (long)reader["UserID"],
                DepartmentID    = reader["DepartmentID"] != DBNull.Value ? (int?)reader["DepartmentID"] : null,
                Campus          = (string)reader["Campus"],
                StaffType       = (string)reader["StaffType"],
                ServerSystem    = (string)reader["SerSys"],
                CourseID        = reader["CourseId"] != DBNull.Value ? (int?)reader["CourseId"] : null,
                NumberOfTickets = reader["NumberOfTickets"] != DBNull.Value ? (int?)reader["NumberOfTickets"] : null
            };

            return staff;
        }
        public static StudentPoco SingleStudentReader(IDataRecord reader)
        {
            var student = new StudentPoco
    {
                STNumber         = (string)reader["STNumber"],
                UserID           = (long)reader["UserID"],
                CourseID         = (int)reader["CourseId"],
                ModeOfDelivery   = (string)reader["ModeOfDelivery"],
                Campus           = (string)reader["Campus"],
                RegistrationType = (string)reader["RegistrationType"],
                Qualification    = (string)reader["Qualification"],
                ModGroup         = (int)reader["ModGroup"]
            };

            return student;
        }

        public static List<SqlParameter> UserParameters(ApplicationUser user)
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@FirstName", (object)user.FirstName ?? DBNull.Value),
                new SqlParameter("@LastName", (object)user.LastName ?? DBNull.Value),
                new SqlParameter("@Username", user.UserName),
                new SqlParameter("@NormalizedUserName", (object)user.NormalizedUserName ?? DBNull.Value),
                new SqlParameter("@Email", (object)user.Email ?? DBNull.Value),
                new SqlParameter("@NormalizedEmail", (object)user.NormalizedEmail ?? DBNull.Value),
                new SqlParameter("@EmailConfirmed", user.EmailConfirmed),
                new SqlParameter("@PasswordHash", (object)user.PasswordHash ?? DBNull.Value),
                new SqlParameter("@SecurityStamp", (object)user.SecurityStamp ?? DBNull.Value), // Assuming SecurityStamp is string
                new SqlParameter("@UserType", (string)user.UsersTypes.ToString()), // Assuming UserType is an Enum
                new SqlParameter("@AuthenticationType", (object)user.AuthenticationType ?? DBNull.Value),
                new SqlParameter("@IsAuthenticated", user.IsAuthenticated),
                new SqlParameter("@FcmToken", user.fcmToken)
            };
        }

        public static bool SetNormUsername(long Id, string normName)
        {
            Debug.WriteLine($"Set Norm Username", "Database-UPDATE");
            Debug.IndentLevel++;
            Debug.WriteLine($"ID:{Id}");
            Debug.WriteLine($"NormName:{normName}");
            Debug.IndentLevel--;

            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                string strUpdate = "UPDATE Users SET " +
                    "NormalizedUserName = @NormalizedUserName  " +
                    "WHERE UserID = @UserID";
                conn.Open();
                SqlCommand cmdUpdate = new SqlCommand(strUpdate);

                cmdUpdate.Connection = conn;
                cmdUpdate.Parameters.AddWithValue("@UserID", Id);
                cmdUpdate.Parameters.AddWithValue("@NormalizedUserName", normName);
                try
                {
                    if(cmdUpdate.ExecuteNonQuery() >= 1)
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
        public static bool SetUsername(long Id, string username)
        {
            Debug.WriteLine($"Set Username", "Database-UPDATE");
            Debug.IndentLevel++;
            Debug.WriteLine($"ID:{Id}");
            Debug.WriteLine($"userName:{username}");
            Debug.IndentLevel--;
            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                string strUpdate = "UPDATE Users SET " +
                    "Username = @Username  " +
                    "WHERE UserID = @UserID";
                conn.Open();
                SqlCommand cmdUpdate = new SqlCommand(strUpdate);


                cmdUpdate.Connection = conn;
                cmdUpdate.Parameters.AddWithValue("@UserID", Id);
                cmdUpdate.Parameters.AddWithValue("@Username", username);
                try
                {
                    if(cmdUpdate.ExecuteNonQuery() >= 1)
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

        public static bool SetPassword(long Id, string password)
        {
            Debug.WriteLine($"Set Username", "Database-UPDATE");
            Debug.IndentLevel++;
            Debug.WriteLine($"ID:{Id}");
            Debug.WriteLine($"password:{password}");
            Debug.IndentLevel--;
            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                string strUpdate = "UPDATE Users SET " +
                    "PasswordHash = @PasswordHash " +
                    "WHERE UserID = @UserID";
                conn.Open();
                SqlCommand cmdUpdate = new SqlCommand(strUpdate);


                cmdUpdate.Connection = conn;
                cmdUpdate.Parameters.AddWithValue("@UserID", Id);
                cmdUpdate.Parameters.AddWithValue("@PasswordHash", password);
                try
                {
                    if(cmdUpdate.ExecuteNonQuery() >= 1)
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
        public static bool SetFcm(long Id, string fcmToken)
        {
            Debug.WriteLine($"Set Fcm I", "Database-UPDATE");
            Debug.IndentLevel++;
            Debug.WriteLine($"ID:{Id}");
            Debug.WriteLine($"password:{fcmToken}");
            Debug.IndentLevel--;
            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                string strUpdate = "UPDATE Users SET " +
                    "FcmToken = @FcmToken " +
                    "WHERE UserID = @UserID";
                conn.Open();
                SqlCommand cmdUpdate = new SqlCommand(strUpdate);


                cmdUpdate.Connection = conn;
                cmdUpdate.Parameters.AddWithValue("@UserID", Id);
                cmdUpdate.Parameters.AddWithValue("@FcmToken", fcmToken);
                try
                {
                    if(cmdUpdate.ExecuteNonQuery() >= 1)
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
        public static bool SetFcm(string email, string fcmToken)
        {
            Debug.WriteLine($"Set Fcm U", "Database-UPDATE");
            Debug.IndentLevel++;
            Debug.WriteLine($"ID:{email}");
            Debug.WriteLine($"password:{fcmToken}");
            Debug.IndentLevel--;
            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                string strUpdate = "UPDATE Users SET " +
                    "FcmToken = @FcmToken " +
                    "WHERE Email = @Email";
                conn.Open();
                SqlCommand cmdUpdate = new SqlCommand(strUpdate);


                cmdUpdate.Connection = conn;
                cmdUpdate.Parameters.AddWithValue("@Email", email);
                cmdUpdate.Parameters.AddWithValue("@FcmToken", fcmToken);
                try
                {
                    if(cmdUpdate.ExecuteNonQuery() >= 1)
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

        public static bool UpdateUsers(ApplicationUser newUsers)
        {
            Debug.WriteLine($"Update Users:", "Database-UPDATE");
            Debug.IndentLevel++;
            Debug.WriteLine($"User:\n\t{newUsers}");
            Debug.IndentLevel--;
            string strUserInsert =
                "UPDATE Users SET " +
                "FirstName          = @FirstName, " +
                "LastName           = @LastName, " +
                "Username           = @Username, " +
                "NormalizedUserName = @NormalizedUserName, " +
                "Email              = @Email, " +
                "NormalizedEmail    = @NormalizedEmail, " +
                "EmailConfirmed     = @EmailConfirmed, " +
                "PasswordHash       = @PasswordHash, " +
                "SecurityStamp      = @SecurityStamp, " +
                "UserType       = @UserType, " +
                "AuthenticationType = @AuthenticationType, " +
                "IsAuthenticated    = @IsAuthenticated " +
                "FcmToken           = @FcmToken " +
                "WHERE UserID       = @UserID;";


            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();
                SqlCommand cmdInsert = new SqlCommand(strUserInsert);

                cmdInsert.Connection = conn;
                cmdInsert.Parameters.AddRange(UserParameters(newUsers).ToArray());

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

        public static long? InsertUsers(ApplicationUser newUsers)
        {
            Debug.WriteLine($"Insert Users:", "Database-INSERT");
            Debug.IndentLevel++;
            Debug.WriteLine($"Users:\n\t{newUsers}");
            Debug.IndentLevel--;
            string strUserInsert =
                $"INSERT INTO [dbo].[Users] " +
                $"       ([FirstName],[LastName],[Username],[NormalizedUserName],[Email],[NormalizedEmail],[EmailConfirmed],[PasswordHash],[SecurityStamp],[ConcurrencyStamp],[UserType],[AuthenticationType],[IsAuthenticated],[FcmToken])" +
                $" OUTPUT INSERTED.UserID " +
                $"VALUES (@FirstName, @LastName, @Username, @NormalizedUserName, @Email, @NormalizedEmail, @EmailConfirmed, @PasswordHash, @SecurityStamp, '', @UserType, @AuthenticationType, @IsAuthenticated, @FcmToken);";

            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();
                SqlCommand cmdInsert = new SqlCommand(strUserInsert);

                cmdInsert.Connection = conn;
                //cmdInsert.Parameters.AddRange(UserParameters(newUsers).ToArray());
                cmdInsert.Parameters.AddWithValue("@FirstName", newUsers.FirstName);
                cmdInsert.Parameters.AddWithValue("@LastName", newUsers.LastName);
                cmdInsert.Parameters.AddWithValue("@Username", newUsers.UserName);
                cmdInsert.Parameters.AddWithValue("@NormalizedUserName", newUsers.NormalizedUserName);
                cmdInsert.Parameters.AddWithValue("@Email", newUsers.Email);
                cmdInsert.Parameters.AddWithValue("@NormalizedEmail", newUsers.NormalizedEmail);
                cmdInsert.Parameters.AddWithValue("@EmailConfirmed", newUsers.EmailConfirmed);
                cmdInsert.Parameters.AddWithValue("@PasswordHash", newUsers.PasswordHash);
                cmdInsert.Parameters.AddWithValue("@SecurityStamp", newUsers.SecurityStamp);
                cmdInsert.Parameters.AddWithValue("@ConcurrencyStamp", newUsers.ConcurrencyStamp);
                cmdInsert.Parameters.AddWithValue("@UserType", newUsers.UsersTypes);
                cmdInsert.Parameters.AddWithValue("@AuthenticationType", newUsers.AuthenticationType);
                cmdInsert.Parameters.AddWithValue("@IsAuthenticated", newUsers.IsAuthenticated);
                cmdInsert.Parameters.AddWithValue("@FcmToken", newUsers.fcmToken);

                try
                {
                    return (long?)cmdInsert.ExecuteScalar();
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.Message, "SqlInsertError");
                    return null;
                }
            }
        }


        public static bool InsertStudent(IStudent newStudent)
        {
            Debug.WriteLine($"Insert Stuednt:", "Database-INSERT");
            Debug.IndentLevel++;
            Debug.WriteLine($"Users:\n\t{newStudent}");
            Debug.IndentLevel--;

            string strStudentInsert =
                $"INSERT INTO [dbo].[Students] " +
                $"      ([STNumber],[UserID],[CourseId],[ModeOfDelivery],[Campus],[RegistrationType],[Qualification],[ModGroup]) " +
                $"VALUES( @STNumber, @UserID, @CourseId, @ModeOfDelivery, @Campus, @RegistrationType, @Qualification, @ModGroup);";

            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();
                SqlCommand cmdInsert = new SqlCommand(strStudentInsert);

                cmdInsert.Connection = conn;
                string stId = $"st{newStudent.Id}";
                cmdInsert.Parameters.AddWithValue("@STNumber", stId);
                cmdInsert.Parameters.AddWithValue("@UserID", newStudent.Id);
                cmdInsert.Parameters.AddWithValue("@CourseId", newStudent.CourseId);
                cmdInsert.Parameters.AddWithValue("@ModeOfDelivery", newStudent.ModeOfDelivery);
                cmdInsert.Parameters.AddWithValue("@Campus", newStudent.Campus);
                cmdInsert.Parameters.AddWithValue("@RegistrationType", newStudent.RegistrationType);
                cmdInsert.Parameters.AddWithValue("@Qualification", newStudent.Qualification);
                cmdInsert.Parameters.AddWithValue("@ModGroup", newStudent.ModGroup);

                try
                {
                    if(cmdInsert.ExecuteNonQuery() > 0)
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

        public static bool InsertStaff(StaffPoco staff)
        {
            Debug.WriteLine($"Insert Staff:", "Database-INSERT");
            Debug.IndentLevel++;
            Debug.WriteLine($"Users:\n\t{staff}");
            Debug.IndentLevel--;
            string strStaffInsert =
                $"INSERT INTO [dbo].[Staff] ([StaffID],[UserID],[DepartmentID],[Campus],[StaffType],[SerSys],[CourseID],[NumberOfTickets]) "+
                $"VALUES (@StaffID,@UserID,@DepartmentID,@Campus,@StaffType,@ServerSystem,@CourseID,@NumberOfTickets);";
            //$"INSERT INTO Staff(StaffID, UserID, StaffType, CourseId, Department, NumberOfTickets, ServerSystem) " +
            //$"VALUES(concat('{brev}', trim(str(@UserID))), @UserID, @StaffType, @CourseId, @DepartmentID, @NumberOfTickets, @ServerSystem);";

            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();
                SqlCommand cmdInsert = new SqlCommand(strStaffInsert);

                cmdInsert.Connection = conn;
                cmdInsert.Parameters.AddWithValue("@StaffID", staff.UserID);
                cmdInsert.Parameters.AddWithValue("@UserID", staff.UserID);
                cmdInsert.Parameters.AddWithValue("@DepartmentID", staff.DepartmentID);
                cmdInsert.Parameters.AddWithValue("@Campus", staff.Campus);
                cmdInsert.Parameters.AddWithValue("@StaffType", staff.StaffType);
                cmdInsert.Parameters.AddWithValue("@ServerSystem", staff.ServerSystem);
                cmdInsert.Parameters.AddWithValue("@CourseID", staff.CourseID);
                cmdInsert.Parameters.AddWithValue("@NumberOfTickets", staff.NumberOfTickets);


                try
                {
                    if(cmdInsert.ExecuteNonQuery() > 0)
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

        public static List<ApplicationUser> GetUser()
        {
            Debug.WriteLine($"Get All Users:", "Database-SELECT");
            Debug.IndentLevel++;
            Debug.IndentLevel--;
            string strSelect = $"SELECT * FROM Users;";

            SqlCommand cmdSelect = new SqlCommand();
            cmdSelect.CommandText = strSelect;
            List<ApplicationUser> allUsers = [];

            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();
                cmdSelect.Connection = conn;
                using(SqlDataReader userReader = cmdSelect.ExecuteReader())
                {
                    while(userReader.Read())
                    {
                        allUsers.Add(SingleUserReader((IDataRecord)userReader));
                    }
                }
            }
            return allUsers;
        }

        public static IApplicationUser GetUser(long userId)
        {
            Debug.WriteLine($"Get  User By userId:", "Database-SELECT");
            Debug.IndentLevel++;
            Debug.WriteLine($"UesrId:{userId}");
            Debug.IndentLevel--;
            string strSelect = $"SELECT * FROM Users WHERE UserID = @UserID;";

            SqlCommand cmdSelect = new SqlCommand();
            cmdSelect.CommandText = strSelect;
            cmdSelect.Parameters.Add($"@UserID", SqlDbType.BigInt).Value = userId;
            ApplicationUser result = null;

            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();
                cmdSelect.Connection = conn;
                using(SqlDataReader userReader = cmdSelect.ExecuteReader())
                {
                    while(userReader.Read())
                    {
                        result = SingleUserReader((IDataRecord)userReader);
                    }
                }
            }
            return result;
        }

        public static ApplicationUser GetUser(string username)
        {
            Debug.WriteLine($"Get  User By Username:", "Database-SELECT");
            Debug.IndentLevel++;
            Debug.WriteLine($"User name:{username}");
            Debug.IndentLevel--;

            string strSelect = $"SELECT * FROM Users ";
            strSelect += $"WHERE Email = @Username;";

            SqlCommand cmdSelect = new SqlCommand();
            cmdSelect.CommandText = strSelect;
            cmdSelect.Parameters.Add($"@Username", SqlDbType.VarChar).Value = username;
            ApplicationUser result = null;
            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();
                cmdSelect.Connection = conn;
                using(SqlDataReader userReader = cmdSelect.ExecuteReader())
                {
                    while(userReader.Read())
                    {
                        Debug.WriteLine(userReader.GetColumnSchema());
                        result = SingleUserReader((IDataRecord)userReader);
                    }
                }
            }
            return result;
        }

        public static StaffPoco GetStaffDetails(long userId)
        {
            Debug.WriteLine($"Get  User By userId:", "Database-SELECT");
            Debug.IndentLevel++;
            Debug.WriteLine($"UesrId:{userId}");
            Debug.IndentLevel--;
            string strSelect = $"SELECT * FROM Staff WHERE UserID = @UserID;";

            SqlCommand cmdSelect = new SqlCommand();
            cmdSelect.CommandText = strSelect;
            cmdSelect.Parameters.Add($"@UserID", SqlDbType.BigInt).Value = userId;
            StaffPoco result = null;

            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();
                cmdSelect.Connection = conn;
                using(SqlDataReader userReader = cmdSelect.ExecuteReader())
                {
                    while(userReader.Read())
                    {
                        result = SingleStaffReader((IDataRecord)userReader);
                    }
                }
            }
            return result;
        }
        public static StudentPoco GetStudentDetails(long userId)
        {
            Debug.WriteLine($"Get  User By userId:", "Database-SELECT");
            Debug.IndentLevel++;
            Debug.WriteLine($"UesrId:{userId}");
            Debug.IndentLevel--;
            string strSelect = $"SELECT * FROM Student WHERE UserID = @UserID;";

            SqlCommand cmdSelect = new SqlCommand();
            cmdSelect.CommandText = strSelect;
            cmdSelect.Parameters.Add($"@UserID", SqlDbType.BigInt).Value = userId;
            StudentPoco result = null;

            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();
                cmdSelect.Connection = conn;
                using(SqlDataReader userReader = cmdSelect.ExecuteReader())
                {
                    while(userReader.Read())
                    {
                        result = SingleStudentReader((IDataRecord)userReader);
                    }
                }
            }
            return result;
        }
        public static List<ApplicationUser>? GetAvalibleSupport(int DepartmentId)
        {
            Debug.WriteLine($"Get  Users By DepartmentId:", "QueyrHandler-Select");
            Debug.IndentLevel++;
            Debug.WriteLine($"DepartmentId:{DepartmentId}");
            Debug.IndentLevel--;
            int numTic = 5;

            string strSelect = $"    SELECT * FROM Users " +
                $"Join Staff ON Users.UserID = Staff.UserID   " +
                $"WHERE Staff.DepartmentID = {DepartmentId};";
            List<ApplicationUser> list = new List<ApplicationUser>();
            SqlCommand cmdSelect = new SqlCommand();
            cmdSelect.CommandText = strSelect;
            //cmdSelect.Parameters.Add($"@Username", SqlDbType.VarChar).Value = use   rname;

            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();
                cmdSelect.Connection = conn;
                using(SqlDataReader userReader = cmdSelect.ExecuteReader())
                {
                    if(userReader.HasRows)
                    {

                        while(userReader.Read())
                        {
                            var user = SingleUserReader((IDataRecord)userReader);
                            list.Add(user);
                        }
                    }
                }
            }
            return list;
        }

        public static bool InsertStaffType(ApplicationRole staffType)
        {
            Debug.WriteLine($"Insert Staff Type:", "Database-INSERT");
            Debug.IndentLevel++;
            Debug.WriteLine($"StaffType:\n\t{staffType}");
            Debug.IndentLevel--;

            string strStaffTypeInsert =
                "INSERT INTO [dbo].[StaffTypes] " +
                "([Name], [NormalizedName], [LimitPriv], [ConcurrencyStamp]) " +
                "VALUES (@Name, @NormalizedName, @LimitPriv, @ConcurrencyStamp);";

            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();

                using(SqlCommand cmdInsert = new SqlCommand(strStaffTypeInsert, conn))
                {
                    cmdInsert.Parameters.AddWithValue("@Name", staffType.Name);
                    cmdInsert.Parameters.AddWithValue("@NormalizedName", staffType.NormalizedName);
                    cmdInsert.Parameters.Add(new SqlParameter("@LimitPriv",
                        (object)staffType.LimitPriv ?? DBNull.Value));
                    cmdInsert.Parameters.Add(new SqlParameter("@ConcurrencyStamp",
                        (object)staffType.ConcurrencyStamp ?? DBNull.Value));

                    try
                    {
                        return cmdInsert.ExecuteNonQuery() > 0;
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine(ex.Message, "SqlInsertError");
                        return false;
                    }
                }
            }
        }
        public static ApplicationRole GetRole(string roleName)
        {
            Debug.WriteLine($"Get Users By RoleName:", "Database-SELECT");
            Debug.IndentLevel++;
            Debug.WriteLine($"RoleName: {roleName}");
            Debug.IndentLevel--;

            string strSelect = $"SELECT [Id], [Name], [NormalizedName], [ConcurrencyStamp] FROM [dbo].[StaffTypes] WHERE [NormalizedName] = @NormalizedName";
            ApplicationRole role = new ApplicationRole();

            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();
                using(SqlCommand cmdSelect = new SqlCommand(strSelect, conn))
                {
                    cmdSelect.Parameters.AddWithValue("@NormalizedName", roleName.ToUpper());

                    using(SqlDataReader userReader = cmdSelect.ExecuteReader())
                    {
                        while(userReader.Read())
                        {
                            role.Id = (long)userReader["Id"];
                            role.Name = userReader["Name"].ToString();
                            role.NormalizedName = userReader["NormalizedName"].ToString();
                            role.ConcurrencyStamp = userReader["ConcurrencyStamp"] == DBNull.Value ? null : userReader["ConcurrencyStamp"].ToString();
                        }
                    }
                }
            }
            return role;
        }

        public static ApplicationRole GetRole(long roleId)
        {
            Debug.WriteLine($"Get Role By RoleId:", "Database-SELECT");
            Debug.IndentLevel++;
            Debug.WriteLine($"RoleId: {roleId}");
            Debug.IndentLevel--;

            string strSelect = $"SELECT [Id], [Name], [NormalizedName], [ConcurrencyStamp] FROM [dbo].[StaffTypes] WHERE [Id] = @Id";
            ApplicationRole role = new ApplicationRole();

            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();
                using(SqlCommand cmdSelect = new SqlCommand(strSelect, conn))
                {
                    cmdSelect.Parameters.AddWithValue("@Id", roleId);

                    using(SqlDataReader userReader = cmdSelect.ExecuteReader())
                    {
                        while(userReader.Read())
                        {
                            role.Id = (long)userReader["Id"];
                            role.Name = userReader["Name"].ToString();
                            role.NormalizedName = userReader["NormalizedName"].ToString();
                            role.ConcurrencyStamp = userReader["ConcurrencyStamp"] == DBNull.Value ? null : userReader["ConcurrencyStamp"].ToString();
                        }
                    }
                }
            }
            return role;
        }



        public static bool UpdateRole(ApplicationRole userType)
        {
            Debug.WriteLine($"Update Role:", "Database-UPDATE");
            Debug.IndentLevel++;
            Debug.WriteLine($"UserType: {userType}");
            Debug.IndentLevel--;

            string strUpdate = $"UPDATE [dbo].[StaffTypes] SET [Name] = @Name, [NormalizedName] = @NormalizedName, [ConcurrencyStamp] = @ConcurrencyStamp WHERE [Id] = @Id";

            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();
                using(SqlCommand cmdUpdate = new SqlCommand(strUpdate, conn))
                {
                    cmdUpdate.Parameters.AddWithValue("@Id", userType.Id);
                    cmdUpdate.Parameters.AddWithValue("@Name", userType.Name);
                    cmdUpdate.Parameters.AddWithValue("@NormalizedName", userType.NormalizedName.ToUpper());
                    cmdUpdate.Parameters.AddWithValue("@ConcurrencyStamp", userType.ConcurrencyStamp ?? (object)DBNull.Value);

                    try
                    {
                        return cmdUpdate.ExecuteNonQuery() > 0;
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine(ex.Message, "SqlUpdateError");
                        return false;
                    }
                }
            }
        }
        public static bool DeleteRole(long roleId)
        {
            Debug.WriteLine($"Delete Role:", "Database-DELETE");
            Debug.IndentLevel++;
            Debug.WriteLine($"Role ID: {roleId}");
            Debug.IndentLevel--;

            string strDelete = "DELETE FROM [dbo].[StaffTypes] WHERE [Id] = @Id";

            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();

                using(SqlCommand cmdDelete = new SqlCommand(strDelete, conn))
                {
                    cmdDelete.Parameters.AddWithValue("@Id", roleId);

                    try
                    {
                        return cmdDelete.ExecuteNonQuery() > 0;
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine(ex.Message, "SqlDeleteError");
                        return false;
                    }
                }
            }
        }

        public static bool AddUserToRole(long userId, string roleId)
        {
            Debug.WriteLine($"Assgine User Role:", "Database-INSERT");
            Debug.IndentLevel++;
            Debug.WriteLine($"User ID: {userId}");
            Debug.WriteLine($"Role ID: {roleId}");
            Debug.IndentLevel--;

            string strInsert = "INSERT INTO [dbo].[UserRoles] ([UserId], [RoleId]) " +
                           "SELECT @UserId, [Id] FROM [dbo].[StaffTypes] WHERE [RoleId] = @RoleId";

            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();
                using(SqlCommand cmdInsert = new SqlCommand(strInsert, conn))
                {
                    cmdInsert.Parameters.AddWithValue("@UserId", userId);
                    cmdInsert.Parameters.AddWithValue("@RoleId", roleId);

                    try
                    {
                        return cmdInsert.ExecuteNonQuery() > 0;
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine(ex.Message, "AddUserToRoleError");
                        throw;
                    }
                }
            }
        }

        public static bool RemoveUserFromRole(long userId, string roleName)
        {
            Debug.WriteLine($"Remove User Role:", "Database-DELETE");
            Debug.IndentLevel++;
            Debug.WriteLine($"User ID: {userId}");
            Debug.WriteLine($"Role Name: {roleName}");
            Debug.IndentLevel--;

            string strDelete = "DELETE FROM [dbo].[UserRoles] " +
                           "WHERE [UserId] = @UserId AND [RoleId] = (SELECT [Id] FROM [dbo].[StaffTypes] WHERE [NormalizedName] = @NormalizedRoleName)";

            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();
                using(SqlCommand cmdDelete = new SqlCommand(strDelete, conn))
                {
                    cmdDelete.Parameters.AddWithValue("@UserId", userId);
                    cmdDelete.Parameters.AddWithValue("@NormalizedRoleName", roleName.ToUpper());

                    try
                    {
                        return cmdDelete.ExecuteNonQuery() > 0;
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine(ex.Message, "RemoveUserFromRoleError");
                        throw;
                    }
                }
            }
        }

        // Method to get roles of a user
        public static IList<string> GetRolesForUser(long userId)
        {
            Debug.WriteLine($"Remove User Role:", "Database-SELECT");
            Debug.IndentLevel++;
            Debug.WriteLine($"User ID: {userId}");
            Debug.IndentLevel--;
            IList<string> roles = new List<string>();
            string strSelect = "SELECT [NormalizedName] FROM [dbo].[StaffTypes] " +
                           "JOIN UserRoles on UserRoles.UserId = StaffTypes.Id " +
                           " WHERE [UserId] = @UserId;";

            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();
                using(SqlCommand cmdSelect = new SqlCommand(strSelect, conn))
                {
                    cmdSelect.Parameters.AddWithValue("@UserId", userId);

                    using(SqlDataReader reader = cmdSelect.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            roles.Add(reader["NormalizedName"].ToString());
                        }
                    }
                }
            }
            return roles;
        }

        // Method to check if a user is in a role
        public static bool IsUserInRole(long userId, string roleName)
        {
            Debug.WriteLine($"Remove User Role:", "Database-SELECT");
            Debug.IndentLevel++;
            Debug.WriteLine($"User ID: {userId}");
            Debug.WriteLine($"Role Name: {roleName}");
            Debug.IndentLevel--;
            string strSelect = "SELECT COUNT(1) FROM [dbo].[UserRoles] " +
                           "WHERE [UserId] = @UserId AND [RoleId] = (SELECT [Id] FROM [dbo].[StaffTypes] WHERE [NormalizedName] = @NormalizedRoleName)";

            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();
                using(SqlCommand cmdSelect = new SqlCommand(strSelect, conn))
                {
                    cmdSelect.Parameters.AddWithValue("@UserId", userId);
                    cmdSelect.Parameters.AddWithValue("@NormalizedRoleName", roleName.ToUpper());

                    return (int)cmdSelect.ExecuteScalar() > 0;
                }
            }
        }

        // Method to get users in a role
        public static IList<ApplicationUser> GetUsersInRole(string roleName)
        {
            Debug.WriteLine($"Get all Users in Role:", "Database-SELECT");
            Debug.IndentLevel++;
            Debug.WriteLine($"Role Name: {roleName}");
            Debug.IndentLevel--;
            IList<ApplicationUser> users = new List<ApplicationUser>();

            string strSelect = "SELECT u.[Id], u.[Username], u.[Email] FROM [dbo].[Users] u " +
                           "JOIN [dbo].[UserRoles] ur ON u.[Id] = ur.[UserId] " +
                           "JOIN [dbo].[StaffTypes] r ON ur.[RoleId] = r.[Id] " +
                           "WHERE r.[NormalizedName] = @NormalizedRoleName";

            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();
                using(SqlCommand cmdSelect = new SqlCommand(strSelect, conn))
                {
                    cmdSelect.Parameters.AddWithValue("@NormalizedRoleName", roleName.ToUpper());

                    using(SqlDataReader reader = cmdSelect.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            ApplicationUser user = new ApplicationUser
                        {
                                Id = (long)reader["Id"],
                                UserName = reader["Username"].ToString(),
                                Email = reader["Email"].ToString()
                            };
                            users.Add(user);
                        }
                    }
                }
            }
            return users;
        }

        public static StaffPoco ReadStaffPoco(IDataReader reader)
        {
            Debug.WriteLine($"StaffUserReader:", "Database-READ");
            Debug.IndentLevel++;
            Debug.IndentLevel--;

            StaffPoco staffPoco = new StaffPoco
            {
                StaffID         = Convert.ToInt64(reader["StaffID"]),
                UserID          = Convert.ToInt64(reader["UserID"]),
                DepartmentID    = Convert.ToInt32(reader["DepartmentID"]),
                Campus          = reader["Campus"].ToString(),
                StaffType       = reader["StaffType"].ToString(),
                ServerSystem          = reader["SerSys"].ToString(),
                CourseID        = Convert.ToInt32(reader["CourseId"]),
                NumberOfTickets = Convert.ToInt32(reader["NumberOfTickets"])
            };
            return staffPoco;
        }

    }
}
