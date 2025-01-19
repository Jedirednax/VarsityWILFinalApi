using System.Data;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using VCTicketTrackerAPIClassLibrary.Tickets;



namespace VCTicketTrackerAPIClassLibrary.StorageDB
{
    public partial class DatabaseAccesUtil
    {
        public static Ticket singleTickerReader(IDataRecord userReader)
        {
            Ticket t = new Ticket()
            {
                TicketID           = Convert.ToInt64(userReader["TicketID"]),
                TicketTitle        = (string)userReader["TicketTitle"],
                TicketDescription  = (string)userReader["TicketDescription"],
                TicketStatus       = (Status)userReader["TicketStatus"],
                TicketCategoryId   = (Categories)userReader["TicketCategoryID"],
                TicketCategory     = new Category((string)userReader["CategoryInformation"]),
                TicketCreatedBy    = Convert.ToInt64(userReader["TicketCreatedBy"]),
                TicketCreationDate = Convert.ToDateTime(userReader["TicketCreationDate"]),
                TicketDocumentURLS =  (string)userReader["TicketDocumentURLS"],

            };
            try
            {
                if(userReader["TicketResolvedDate"] is not DBNull)
                {

                    t.TicketResolvedDate = Convert.ToDateTime(userReader["TicketResolvedDate"]);
                }
                else
                {
                    t.TicketResolvedDate = null;
                }
            }
            catch(Exception ew)
            {
                Debug.WriteLine(ew);
                t.TicketResolvedDate= null;
            }
            try
            {
                t.AssignedTo= Convert.ToInt64(userReader["AssignedTo"]);
            }
            catch(Exception ew)
            {
                Debug.WriteLine(ew);
                t.AssignedTo = 0;
            }
            return t;
        }
        public static List<Category> GetCategory()
        {
            Debug.WriteLine($"Get All Categories", "Database-SELECT");
            Debug.IndentLevel++;
            Debug.IndentLevel--;
            List<Category> cat =new List<Category>();
            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();
                string strSelect = $"SELECT * FROM Category;";
                SqlCommand cmdSelect = new SqlCommand(strSelect, conn);

                //cmdSelect.Parameters.Add($"@TicketID", SqlDbType.Int).Value = Id;

                using(SqlDataReader userReader = cmdSelect.ExecuteReader())
                {
                    while(userReader.Read())
                    {
                        var cati = new Category(
                            iD: (int)userReader["CategoryID"],
                            name: (string)userReader["CategoryName"],
                            description: (string)userReader["Description"],
                            depart: (int)userReader["CategoryDepartment"],
                            priority: (int)userReader["CategoryPriority"],
                            parentType: (string)userReader["SubType"]
                        );
                        try
                        {
                            cati.Fields = JsonSerializer.Deserialize<Dictionary<string, string>>((string)userReader["CategoryFields"]);
                        }
                        catch
                        {
                            Debug.WriteLine("no feilds");
                        }
                        cat.Add(cati);
                    }
                }
            }
            return cat;
        }
        public static Ticket GetTicketID(long Id)
        {
            Debug.WriteLine($"Get Ticket By Id", "Database-SELECT");
            Debug.IndentLevel++;
            Debug.WriteLine($"TicketID:{Id}");
            Debug.IndentLevel--;
            Ticket ticket = null;
            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();
                string strSelect = $"SELECT * FROM Ticket WHERE TicketID = @TicketID;";
                SqlCommand cmdSelect = new SqlCommand(strSelect, conn);

                cmdSelect.Parameters.Add($"@TicketID", SqlDbType.BigInt).Value = Id;

                using(SqlDataReader userReader = cmdSelect.ExecuteReader())
                {
                    while(userReader.Read())
                    {
                        return singleTickerReader((IDataReader)userReader);
                    }

                }
            }
            return ticket;
        }
        public static List<Ticket> GetAllTickets()
        {
            Debug.WriteLine($"Get All Tickets", "Database-SELECT");
            Debug.IndentLevel++;
            Debug.IndentLevel--;

            List<Ticket> ticket =new List<Ticket>();
            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();
                string strSelect = $"SELECT * FROM Ticket;";
                SqlCommand cmdSelect = new SqlCommand(strSelect, conn);

                using(SqlDataReader userReader = cmdSelect.ExecuteReader())
                {
                    while(userReader.Read())
                    {
                        ticket.Add(singleTickerReader((IDataReader)userReader));
                    }

                }
            }
            return ticket;
        }

        public static List<Ticket> GetUserTickets(long Id)
        {
            Debug.WriteLine($"Get Tickets Owned by", "Database-SELECT");
            Debug.IndentLevel++;
            Debug.WriteLine($"UserID:{Id}");
            Debug.IndentLevel--;
            List<Ticket> tickets = new List<Ticket>();
            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();

                string strSelect = $"SELECT * FROM Ticket WHERE TicketCreatedBy = @UserID;";
                SqlCommand cmdSelect = new SqlCommand(strSelect, conn);

                cmdSelect.Parameters.Add($"@UserID", SqlDbType.BigInt).Value = Id;

                using(SqlDataReader userReader = cmdSelect.ExecuteReader())
                {
                    while(userReader.Read())
                    {
                        tickets.Add(singleTickerReader((IDataReader)userReader));
                    }

                }
            }
            return tickets;
        }


        public static List<Ticket> GetAssginedTickets(long AssgeindTo)
        {
            Debug.WriteLine($"Get Tickets AssginedTO", "Database-SELECT");
            Debug.IndentLevel++;
            Debug.WriteLine($"UserID:{AssgeindTo}");
            Debug.IndentLevel--;
            List<Ticket> tickets = new List<Ticket>();
            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();

                string strSelect = $"SELECT * FROM TICKET WHERE AssignedTo = @AssignedTo;";
                SqlCommand cmdSelect = new SqlCommand(strSelect, conn);

                cmdSelect.Parameters.Add($"@AssignedTo", SqlDbType.BigInt).Value = AssgeindTo;

                using(SqlDataReader userReader = cmdSelect.ExecuteReader())
                {
                    while(userReader.Read())
                    {
                        tickets.Add(singleTickerReader((IDataReader)userReader));
                    }

                }
            }
            return tickets;
        }

        public static List<Ticket> GetCreatedTickets(long createdBy)
        {
            Debug.WriteLine($"Get Tickets CreatedBy", "Database-SELECT");
            Debug.IndentLevel++;
            Debug.WriteLine($"UserID:{createdBy}");
            Debug.IndentLevel--;
            List<Ticket> tickets = new List<Ticket>();
            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();

                string strSelect = $"SELECT * FROM Ticket WHERE CreatedBY = @CreatedBy;";
                SqlCommand cmdSelect = new SqlCommand(strSelect, conn);

                cmdSelect.Parameters.Add($"@CreatedBy", SqlDbType.BigInt).Value = createdBy;

                using(SqlDataReader userReader = cmdSelect.ExecuteReader())
                {
                    while(userReader.Read())
                    {
                        tickets.Add(singleTickerReader((IDataReader)userReader));
                    }

                }
            }
            return tickets;
        }

        public static List<Ticket> GetTicketCreatedBtw(DateTime start, DateTime end)
        {
            Debug.WriteLine($"Get Tickets Created Between DateRange", "Database-SELECT");
            Debug.IndentLevel++;
            Debug.WriteLine($"Start:{start}");
            Debug.WriteLine($"  End:{end}");
            Debug.IndentLevel--;

            List<Ticket> tickets = new List<Ticket>();
            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();

                string strSelect = $"SELECT * FROM Ticket WHERE TicketCreationDate BETWEEN @start AND @end;";
                SqlCommand cmdSelect = new SqlCommand(strSelect, conn);

                cmdSelect.Parameters.Add($"@start", SqlDbType.DateTime2).Value = start;
                cmdSelect.Parameters.Add($"@end", SqlDbType.DateTime2).Value = end;
                //               cmdSelect.Parameters.Add($"@TicketCreationDate", SqlDbType.DateTime2).Value = end;

                using(SqlDataReader userReader = cmdSelect.ExecuteReader())
                {
                    while(userReader.Read())
                    {
                        tickets.Add(singleTickerReader((IDataReader)userReader));
                    }

                }
            }
            return tickets;
        }

        public static List<Ticket> GetTicketResolvedBtw(DateTime start, DateTime end)
        {
            Debug.WriteLine($"Get Tickets Resolved Between DateRange", "Database-SELECT");
            Debug.IndentLevel++;
            Debug.WriteLine($"Start:{start}");
            Debug.WriteLine($"  End:{end}");
            Debug.IndentLevel--;

            List<Ticket> tickets = new List<Ticket>();
            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();

                string strSelect = $"SELECT * FROM Ticket WHERE TicketResolvedDate BETWEEN @start AND @end;";
                SqlCommand cmdSelect = new SqlCommand(strSelect, conn);

                cmdSelect.Parameters.Add($"@start", SqlDbType.DateTime2).Value = start;
                cmdSelect.Parameters.Add($"@end", SqlDbType.DateTime2).Value = end;
                //               cmdSelect.Parameters.Add($"@TicketCreationDate", SqlDbType.DateTime2).Value = end;

                using(SqlDataReader userReader = cmdSelect.ExecuteReader())
                {
                    while(userReader.Read())
                    {
                        tickets.Add(singleTickerReader((IDataReader)userReader));
                    }

                }
            }
            return tickets;
        }


        public static List<Ticket> GetStatusTickets(Status status)
        {
            Debug.WriteLine($"GetTickets by Status", "Database-SELECT");
            Debug.IndentLevel++;
            Debug.WriteLine($"Status:{status}");
            Debug.IndentLevel--;

            List<Ticket> tickets = new List<Ticket>();
            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();

                string strSelect = $"SELECT * FROM Ticket WHERE TicketStatus = @TicketStatus;";
                SqlCommand cmdSelect = new SqlCommand(strSelect, conn);

                cmdSelect.Parameters.Add($"@TicketStatus", SqlDbType.Int).Value = status;

                using(SqlDataReader userReader = cmdSelect.ExecuteReader())
                {
                    while(userReader.Read())
                    {
                        tickets.Add(singleTickerReader((IDataReader)userReader));
                    }

                }
            }
            return tickets;
        }

        public static List<Ticket> GetTicketCategory(int category)
        {
            Debug.WriteLine($"GetTickets by Category", "Database-SELECT");
            Debug.IndentLevel++;
            Debug.WriteLine($"Category:{category}");
            Debug.IndentLevel--;

            List<Ticket> tickets = new List<Ticket>();
            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();

                string strSelect = $"SELECT * FROM Ticket WHERE TicketCategoryID = @TicketCategoryID;";
                SqlCommand cmdSelect = new SqlCommand(strSelect, conn);

                cmdSelect.Parameters.Add($"@TicketCategoryID", SqlDbType.Int).Value = category;

                using(SqlDataReader userReader = cmdSelect.ExecuteReader())
                {
                    while(userReader.Read())
                    {
                        tickets.Add(singleTickerReader((IDataReader)userReader));
                    }

                }
            }
            return tickets;
        }
        public static long InsertTicket(Ticket ticket, TicketEvent ticketEvent)
        {
            Debug.WriteLine($"Insert Ticket and Event", "Database-INSERT");
            Debug.IndentLevel++;
            Debug.WriteLine($"NewTicket:\n\t{ticket}");
            Debug.WriteLine($"NewTicketEvent:\n\t{ticketEvent}");
            Debug.IndentLevel--;

            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();

                string strInsert = $"INSERT INTO Ticket (TicketTitle, TicketDescription, TicketStatus, TicketCategoryID, CategoryInformation, TicketCreatedBy, TicketCreationDate, TicketResolvedDate, TicketDocumentURLS)" +

                    "Values(@TicketTitle, @TicketDescription, @TicketStatus, @TicketCategoryID, @CategoryInformation, @TicketCreatedBy, @TicketCreationDate, null, @TicketDocumentURLS); ";

                string strEventInsert =$"INSERT INTO TicketEvents (TicketID, DateOfEvent, UpdatedBy, StatusSetFrom, StatusSetTo  ,EventType, EventComments) " +
                                        " OUTPUT INSERTED.TicketID "+
                    "Values(@@Identity, @DateOfEvent, @UpdatedBy, @StatusSetFrom, @StatusSetTo, @EventType, @EventComments); ";
                string transaction =
                $"" +
                $"BEGIN " +
                $"{strInsert} {strEventInsert} " +
                $"END";

                SqlCommand cmdInsert = new SqlCommand(transaction, conn);

                //cmdInsert.Parameters.Add($"@TicketID",          SqlDbType.VarChar).Value  =ticket.TicketID           ;
                cmdInsert.Parameters.Add($"@TicketTitle", SqlDbType.VarChar).Value          =ticket.TicketTitle;
                cmdInsert.Parameters.Add($"@TicketDescription", SqlDbType.VarChar).Value    =ticket.TicketDescription;
                cmdInsert.Parameters.Add($"@TicketStatus", SqlDbType.Int).Value             =(int)ticket.TicketStatus;

                cmdInsert.Parameters.Add($"@TicketCategoryID", SqlDbType.Int).Value         = ticket.TicketCategory.ID;
                cmdInsert.Parameters.Add($"@CategoryInformation", SqlDbType.VarChar).Value  =(string)ticket.TicketCategory.ToString();

                cmdInsert.Parameters.Add($"@TicketCreatedBy", SqlDbType.BigInt).Value          =ticket.TicketCreatedBy;
                cmdInsert.Parameters.Add($"@TicketCreationDate", SqlDbType.DateTime2).Value =ticket.TicketCreationDate;
                //cmdInsert.Parameters.Add($"@TicketResolvedDate", SqlDbType.DateTime2).Value =ticket.TicketResolvedDate;
                cmdInsert.Parameters.Add($"@TicketDocumentURLS", SqlDbType.VarChar).Value   =ticket.TicketDocumentURLS;

                //cmdInsert.Parameters.Add($"@TicketID", SqlDbType.Int).Value              = ticketEvent.TicketID;
                cmdInsert.Parameters.Add($"@DateOfEvent", SqlDbType.DateTime2).Value       = ticketEvent.DateOfEvent;
                cmdInsert.Parameters.Add($"@UpdatedBy", SqlDbType.BigInt).Value               = ticketEvent.UpdatedBy;
                cmdInsert.Parameters.Add($"@StatusSetFrom", SqlDbType.Int).Value           = (int)ticketEvent.StatusSetFrom;
                cmdInsert.Parameters.Add($"@StatusSetTo", SqlDbType.Int).Value             = (int)ticketEvent.StatusSetTo;
                cmdInsert.Parameters.Add($"@EventType", SqlDbType.VarChar).Value           = ticketEvent.EventType;
                cmdInsert.Parameters.Add($"@EventComments", SqlDbType.VarChar).Value       = ticketEvent.EventComments;

                try
                {

                    return (long)cmdInsert.ExecuteScalar();
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    Debug.WriteLine(ex.Message, "SqlInsertError");
                    return -1;
                }
            }
        }


        public static bool InsertTicket(Ticket ticket)
        {
            Debug.WriteLine($"Insert only Ticket", "Database-INSERT");
            Debug.IndentLevel++;
            Debug.WriteLine($"NewTicket:\n\t{ticket}");
            Debug.IndentLevel--;

            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();

                string strInsert = $"INSERT INTO Ticket (TicketTitle, TicketDescription, TicketStatus, TicketCategoryID,CategoryInformation, TicketCreatedBy, TicketCreationDate, TicketResolvedDate, TicketDocumentURLS)" +
                        "Values(@TicketTitle, @TicketDescription, @TicketStatus, @TicketCategoryID,@CategoryInformation, @TicketCreatedBy, @TicketCreationDate, @TicketResolvedDate, @TicketDocumentURLS)";
                SqlCommand cmdInsert = new SqlCommand(strInsert, conn);

                //cmdInsert.Parameters.Add($"@TicketID",          SqlDbType.VarChar).Value =ticket.TicketID           ;
                cmdInsert.Parameters.Add($"@TicketTitle", SqlDbType.VarChar).Value =ticket.TicketTitle;
                cmdInsert.Parameters.Add($"@TicketDescription", SqlDbType.VarChar).Value =ticket.TicketDescription;
                cmdInsert.Parameters.Add($"@TicketStatus", SqlDbType.Int).Value =(int)ticket.TicketStatus;

                cmdInsert.Parameters.Add($"@TicketCategoryID", SqlDbType.Int).Value = (int)(Categories)ticket.TicketCategoryId;
                cmdInsert.Parameters.Add($"@CategoryInformation", SqlDbType.VarChar).Value         =(string)ticket.TicketCategory.ToString();

                cmdInsert.Parameters.Add($"@TicketCreatedBy", SqlDbType.BigInt).Value =ticket.TicketCreatedBy;
                cmdInsert.Parameters.Add($"@TicketCreationDate", SqlDbType.DateTime2).Value =ticket.TicketCreationDate;
                cmdInsert.Parameters.Add($"@TicketResolvedDate", SqlDbType.DateTime2).Value =ticket.TicketResolvedDate;
                cmdInsert.Parameters.Add($"@TicketDocumentURLS", SqlDbType.VarChar).Value =ticket.TicketDocumentURLS;

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

        public static bool TicketStatusUP(long ID, Status stat)
        {
            Debug.WriteLine($"Ticket Status Update", "Database-UPDATE");
            Debug.IndentLevel++;
            Debug.WriteLine($"Ticket ID:{ID}");
            Debug.WriteLine($"Ticket new Status:{stat}");
            Debug.IndentLevel--;

            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();

                string strInsert = $"UPDATE Ticket SET TicketStatus = @TicketStatus WHERE TicketID = @TicketID";
                SqlCommand cmdInsert = new SqlCommand(strInsert, conn);

                cmdInsert.Parameters.Add($"@TicketStatus", SqlDbType.Int).Value = (int)stat;
                cmdInsert.Parameters.Add($"@TicketID", SqlDbType.BigInt).Value = ID;

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

        public static bool TicketUrlUP(long ID, string urls)
        {
            Debug.WriteLine($"Ticket Urls", "Database-UPDATE");
            Debug.IndentLevel++;
            Debug.WriteLine($"Ticket ID:{ID}");
            Debug.WriteLine($"Ticket Urls:{urls}");
            Debug.IndentLevel--;

            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();

                string strInsert = $"UPDATE Ticket SET TicketDocumentURLS = @TicketDocumentURLS  WHERE TicketID = @TicketID";
                SqlCommand cmdInsert = new SqlCommand(strInsert, conn);

                cmdInsert.Parameters.Add($"@TicketDocumentURLS", SqlDbType.VarChar).Value = urls;
                cmdInsert.Parameters.Add($"@TicketID", SqlDbType.BigInt).Value = ID;

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

        public static bool TicketAssginedUP(long ID, long UserID, Status status)
        {
            Debug.WriteLine($"Assing Ticket", "Database-UPDATE");
            Debug.IndentLevel++;
            Debug.WriteLine($"Ticket ID:{ID}");
            Debug.WriteLine($"UserId:{UserID}");
            Debug.IndentLevel--;

            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();

                string strInsert = $"UPDATE Ticket SET AssignedTo = @AssignedTo, TicketStatus = @TicketStatus WHERE TicketID = @TicketID";
                SqlCommand cmdInsert = new SqlCommand(strInsert, conn);

                cmdInsert.Parameters.Add($"@AssignedTo", SqlDbType.BigInt).Value = UserID;
                cmdInsert.Parameters.Add($"@TicketID", SqlDbType.BigInt).Value = ID;
                cmdInsert.Parameters.Add($"@TicketStatus", SqlDbType.Int).Value = (int)status;
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

        public static bool TicketResolved(long ID)
        {
            Debug.WriteLine($"Resolved Ticket", "Database-UPDATE");
            Debug.IndentLevel++;
            Debug.WriteLine($"Ticket ID:{ID}");
            Debug.IndentLevel--;

            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();

                string strInsert = $"UPDATE Ticket SET TicketResolvedDate = @TicketResolvedDate, TicketStatus = @TicketStatus WHERE TicketID = @TicketID";
                SqlCommand cmdInsert = new SqlCommand(strInsert, conn);

                cmdInsert.Parameters.Add($"@TicketStatus", SqlDbType.Int).Value = Status.Resolved;

                cmdInsert.Parameters.Add($"@TicketResolvedDate", SqlDbType.DateTime2).Value = DateTime.Now;
                cmdInsert.Parameters.Add($"@TicketID", SqlDbType.BigInt).Value = ID;

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

        public static bool TicketDelete(long ID)
        {
            Debug.WriteLine($"Delete Ticket", "Database-DELETE");
            Debug.IndentLevel++;
            Debug.WriteLine($"Ticket ID:{ID}");
            Debug.IndentLevel--;

            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();

                string strDelete = $"DELETE FROM TicketEvents WHERE TicketID = @TicketID; DELETE FROM Ticket WHERE TicketID = @TicketID;";
                SqlCommand cmdDelete = new SqlCommand(strDelete, conn);

                cmdDelete.Parameters.Add($"@TicketID", SqlDbType.BigInt).Value = ID;

                try
                {
                    if(cmdDelete.ExecuteNonQuery() >= 1)
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
                    Debug.WriteLine(ex.Message, "SqlDeleteError");
                    return false;
                }
            }
        }

        public static List<TicketEvent> GetTicketEvent(long Id)
        {
            Debug.WriteLine($"Get ALl ticketEvents", "Database-SELECT");
            Debug.IndentLevel++;
            Debug.WriteLine($"Ticket ID:{Id}");

            Debug.IndentLevel--;

            List<TicketEvent> tickets = new List<TicketEvent>();
            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                conn.Open();
                string strSelect = $"SELECT * FROM TicketEvents WHERE TicketID = @TicketID;";
                SqlCommand cmdSelect = new SqlCommand(strSelect, conn);

                cmdSelect.Parameters.Add($"@TicketID", SqlDbType.BigInt).Value = Id;

                using(SqlDataReader userReader = cmdSelect.ExecuteReader())
                {
                    while(userReader.Read())
                    {
                        TicketEvent t = new TicketEvent()
                            {
                            TicketEventID = Convert.ToInt64(userReader["TicketEventID"]),
                            TicketID      = Convert.ToInt64(userReader["TicketID"]),
                            DateOfEvent   = (DateTime)userReader["DateOfEvent"],
                            UpdatedBy     =Convert.ToInt64(userReader["UpdatedBy"]),
                            StatusSetFrom = (Status)userReader["StatusSetFrom"],
                            StatusSetTo   = (Status)userReader["StatusSetTo"],
                            EventType     = (string)userReader["EventType"],
                            EventComments = (string)userReader["EventComments"],
                        };
                        tickets.Add(t);
                    }
                }
            }
            return tickets;
        }

        public static bool InsertTicketEvent(TicketEvent ticket)
        {
            Debug.WriteLine($"Insert TIcket Event", "Database-INSERT");
            Debug.IndentLevel++;
            Debug.WriteLine($"Ticket Event:\n\t{ticket}");
            Debug.IndentLevel--;

            using(var connA = new SqlConnection(DatabaseConnection.Connection))
            {
                connA.Open();
                string strInsert = $"INSERT INTO TicketEvents (TicketID, DateOfEvent, UpdatedBy, StatusSetFrom, StatusSetTo, EventType, EventComments) " +
                    "Values(@TicketID, @DateOfEvent, @UpdatedBy, @StatusSetFrom, @StatusSetTo, @EventType, @EventComments);";

                SqlCommand cmdInsert = new SqlCommand(strInsert,connA);

                cmdInsert.Parameters.Add($"@TicketID", SqlDbType.BigInt).Value            = ticket.TicketID;
                cmdInsert.Parameters.Add($"@DateOfEvent", SqlDbType.DateTime2).Value   = ticket.DateOfEvent;
                cmdInsert.Parameters.Add($"@UpdatedBy", SqlDbType.BigInt).Value           = ticket.UpdatedBy;
                cmdInsert.Parameters.Add($"@StatusSetFrom", SqlDbType.Int).Value       = (int)(Status)ticket.StatusSetFrom;
                cmdInsert.Parameters.Add($"@StatusSetTo", SqlDbType.Int).Value         = (int)(Status)ticket.StatusSetTo;
                cmdInsert.Parameters.Add($"@EventType", SqlDbType.VarChar).Value       = ticket.EventType;
                cmdInsert.Parameters.Add($"@EventComments", SqlDbType.VarChar).Value   = ticket.EventComments;

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
