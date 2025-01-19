using VCTicketTrackerAPIClassLibrary.Auth;
using VCTicketTrackerAPIClassLibrary.Tickets;
using VCTicketTrackerAPIClassLibrary.UserClasses;

namespace VCTicketTrackerAPIClassLibrary.StorageDB
{
    interface IQueryHandler
    {

        List<ApplicationUser> GetAllUsers();
        ApplicationUser GetUser(int userId);
        ApplicationUser GetUser(long userId);
        ApplicationUser GetUser(string username);

        ApplicationUser GetUserByUsername(string username);
        ApplicationUser GetUserById(long userId);

        List<ApplicationUser> GetUserByType(UserType userType);
        List<ApplicationUser> GetUserByType(string userType);
        List<ApplicationUser> GetUserByType(int userType);


        List<Ticket> GetAllTickets();

        Ticket GetTicketById(int ticketId);
        Ticket GetTicketById(long ticketId);
        Ticket GetTicketById(string ticketId);

        Ticket GetTicketByTitle(string ticketId);
        Ticket GetTicketByCategory(string ticketId);
        Ticket GetTicketByUser(string ticketId);


        List<Ticket> GetTicketByAssignedUser<TUser>(TUser user);
        List<Ticket> GetTicketByCreatedUser<TUser>(TUser user);




    }

    public class SQLscripts
    {
        interface ICrud
        {
            //SqlCommand Insert();
            //SqlCommand Select();
            //SqlCommand Select(long Id);
            //SqlCommand Select(string Id);
            //SqlCommand Update(long Id);
            //SqlCommand Update(string Id);
            //SqlCommand Delete(long Id);
            //SqlCommand Delete(string Id);
        }

        public class TBCategory : ICrud
        {
        }
        public class TBCourse : ICrud
        {
        }
        public class TBCourseModules : ICrud
        {
        }
        public class TBDepartment : ICrud
        {
        }
        public class TBEnumList : ICrud
        {
        }
        public class TBFaq : ICrud
        {
        }
        public class TBLogins : ICrud
        {
        }
        public class TBModules : ICrud
        {
        }
        public class TBStaff : ICrud
        {
        }
        public class TBStaffModules : ICrud
        {
        }
        public class TBStaffTypes : ICrud
        {
        }
        public class TBStudentModules : ICrud
        {
        }
        public class TBStudents : ICrud
        {
        }
        public class TBTicket : ICrud
        {
        }
        public class TBTicketEvents : ICrud
        {
        }
        public class TBUsers : ICrud
        {
        }
    }
}
/*
 * public class TBClassName : ICrud
{
    private readonly string _connectionString;

    public TBClassName(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqlCommand Insert()
    {
        string query = "INSERT INTO [dbo].[TableName] ([Column1], [Column2], [Column3], ...) " +
                       "VALUES (@Column1, @Column2, @Column3, ...)";
        
        var command = new SqlCommand(query);
        command.Parameters.AddWithValue("@Column1", "Value1");
        command.Parameters.AddWithValue("@Column2", "Value2");
        command.Parameters.AddWithValue("@Column3", "Value3");
        return command;
    }

    public SqlCommand Select()
    {
        string query = "SELECT * FROM [dbo].[TableName]";
        return new SqlCommand(query);
    }

    public SqlCommand Select(long Id)
    {
        string query = "SELECT * FROM [dbo].[TableName] WHERE [PrimaryKey] = @Id";
        var command = new SqlCommand(query);
        command.Parameters.AddWithValue("@Id", Id);
        return command;
    }

    public SqlCommand Select(string Id)
    {
        string query = "SELECT * FROM [dbo].[TableName] WHERE [PrimaryKey] = @Id";
        var command = new SqlCommand(query);
        command.Parameters.AddWithValue("@Id", Id);
        return command;
    }

    public SqlCommand Update(long Id)
    {
        string query = "UPDATE [dbo].[TableName] SET [Column1] = @Column1 WHERE [PrimaryKey] = @Id";
        var command = new SqlCommand(query);
        command.Parameters.AddWithValue("@Column1", "UpdatedValue");
        command.Parameters.AddWithValue("@Id", Id);
        return command;
    }

    public SqlCommand Update(string Id)
    {
        string query = "UPDATE [dbo].[TableName] SET [Column1] = @Column1 WHERE [PrimaryKey] = @Id";
        var command = new SqlCommand(query);
        command.Parameters.AddWithValue("@Column1", "UpdatedValue");
        command.Parameters.AddWithValue("@Id", Id);
        return command;
    }

    public SqlCommand Delete(long Id)
    {
        string query = "DELETE FROM [dbo].[TableName] WHERE [PrimaryKey] = @Id";
        var command = new SqlCommand(query);
        command.Parameters.AddWithValue("@Id", Id);
        return command;
    }

    public SqlCommand Delete(string Id)
    {
        string query = "DELETE FROM [dbo].[TableName] WHERE [PrimaryKey] = @Id";
        var command = new SqlCommand(query);
        command.Parameters.AddWithValue("@Id", Id);
        return command;
    }
}
*/