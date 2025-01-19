using System.Diagnostics;
using VCTicketTrackerAPIClassLibrary.Handlers;
using VCTicketTrackerAPIClassLibrary.StorageDB;
using VCTicketTrackerAPIClassLibrary.UserClasses;
namespace VCTicketTrackerAPIClassLibrary.Tickets
{

    /// <summary>
    /// Main Ticket object class, for ticket metadata
    /// </summary>
    /// <remarks>
    /// <example>
    /// <code>
    /// {
    ///     "ticketID": 0,
    ///     "ticketTitle": "string",
    ///     "ticketDescription": "string",
    ///     "ticketStatus": "Created",
    ///     "ticketCategory": 
    ///     {
    ///         "id": 0,
    ///         "name": "string",
    ///         "description": "string",
    ///         "depart": 0,
    ///         "priority": 0,
    ///         "parentType": "string",
    ///         "fields": 
    ///         {
    ///             "additionalProp1": "string",
    ///             "additionalProp2": "string",
    ///             "additionalProp3": "string"
    ///         }
    ///     },
    ///     "ticketCategoryId": "AcademicInternalCreditQuery",
    ///     "assignedTo": 0,
    ///     "ticketCreatedBy": 0,
    ///     "ticketCreationDate": "2024-11-20T11:26:51.234Z",
    ///     "ticketResolvedDate": "2024-11-20T11:26:51.234Z",
    ///     "ticketDocumentURLS": "string",
    ///     "ticketUpdates": 
    ///     [
    ///         {
    ///           "eventId": 0,
    ///           "ticketId": 0,
    ///           "date": "2024-11-20T11:26:51.234Z",
    ///           "updtedBy": 0,
    ///           "statusFrom": "Created",
    ///           "statusTo": "Created",
    ///           "type": "string",
    ///           "comment": "string"
    ///         }
    ///     ]
    /// }
    /// </code>
    /// </example>
    /// </remarks>

    public class Ticket
    {
        /// <value>
        /// Property <c> TicketID </c> The ticket ID in the database
        /// </value>
        public long TicketID { get; set; }

        /// <value>
        /// Property <c> TicketTitle </c> Title of the 
        /// </value>
        public string? TicketTitle { get; set; }

        /// <value>
        /// Property <c> TicketDescription </c> Description for the ticket
        /// </value>
        public string? TicketDescription { get; set; }

        /// <value>
        /// Property <c> TicketID </c> The ticket ID in the database
        /// </value>
        public Status? TicketStatus { get; set; }

        /// <value>
        /// Property <c> TicketCategory </c> <see cref="Category"/>
        public Category TicketCategory { get; set; }
        /// </value>
        /// <value>
        /// Property <c> TicketCategory </c> <see cref="Category"/>
        /// </value>
        public Categories TicketCategoryId { get; set; } = (Categories)1;

        /// <value>
        /// Property <c> AssgiendTo </c> Represents the User who is assgined to the ticket
        /// </value>
        public long AssignedTo { get; set; } = default(long);

        /// <value>
        /// Property <c> TicketCreatedBy </c> Represents the User who created the ticket
        /// </value>
        public long TicketCreatedBy { get; set; }

        /// <value>
        /// Property <c> TicketCreationDate </c> The date when ticket was created
        /// </value>
        public DateTime? TicketCreationDate { get; set; } = DateTime.Now;

        /// <value>
        /// Property <c> TicketResolvedDate </c> The date when ticket was set to Resolved
        /// </value>
        public DateTime? TicketResolvedDate { get; set; } = DateTime.Now;

        /// <value>
        /// Property <c> DocuemntURLS </c> Path of stored location of documnets relavent to the ticket
        /// </value>
        public string TicketDocumentURLS { get; set; }

        // Needs
        /// <value>
        /// Property <c> TicketUpdate </c> List of Ticket events associated with the ticket
        /// </value>
        public List<TicketEvent> TicketUpdates { get; set; } = new List<TicketEvent>();

        /// <summary>
        /// Generic empty constructor To Create a Ticket object
        /// </summary>
        public Ticket()
        {

        }


        /// <summary>
        /// Full Ticket Constructor
        /// </summary>
        /// <param name="ticketID"> the Ticket ID</param>
        /// <param name="ticketTitle"> The Ticket Title </param>
        /// <param name="ticketDescription"> The Ticket Description </param>
        /// <param name="ticketStatus"> The current Ticket Status <seealso cref="Status"/></param>
        /// <param name="ticketCategory"> The ticket Category <seealso cref="Categories"/></param>
        /// <param name="ticketCreatedBy"> THe userId of who created the ticket </param>
        /// <param name="ticketCreationDate"> The Createiong Date of the ticket</param>
        /// <param name="ticketResolvedDate"> The Resolution Date of the ticket</param>
        /// <param name="ticketDocumentURLS"> The list of Ticket URls delimited by ','</param>
        public Ticket(long ticketID, string? ticketTitle, string? ticketDescription, Status? ticketStatus, Category ticketCategory, long ticketCreatedBy, DateTime ticketCreationDate, DateTime ticketResolvedDate, string ticketDocumentURLS)
        {
            TicketID           = ticketID;
            TicketTitle        = ticketTitle;
            TicketDescription  = ticketDescription;
            TicketStatus       = ticketStatus;
            TicketCategory     = ticketCategory;
            TicketCreatedBy    = ticketCreatedBy;
            TicketCreationDate = ticketCreationDate;
            TicketResolvedDate = ticketResolvedDate;
            TicketDocumentURLS = ticketDocumentURLS;
        }

        /// <summary>
        /// Ticket constructor with out ID
        /// </summary>
        /// <param name="ticketTitle"> The Ticket Title </param>
        /// <param name="ticketDescription"> The Ticket Description </param>
        /// <param name="ticketStatus"> The current Ticket Status <seealso cref="Status"/></param>
        /// <param name="ticketCategory"> The ticket Category <seealso cref="Categories"/></param>
        /// <param name="ticketCreatedBy"> THe userId of who created the ticket </param>
        /// <param name="ticketCreationDate"> The Createiong Date of the ticket</param>
        /// <param name="ticketResolvedDate"> The Resolution Date of the ticket</param>
        /// <param name="ticketDocumentURLS"> The list of Ticket URls delimited by ','</param>
        public Ticket(string? ticketTitle, string? ticketDescription, Status? ticketStatus, Category ticketCategory, long ticketCreatedBy, DateTime ticketCreationDate, DateTime ticketResolvedDate, string ticketDocumentURLS)
        {
            TicketID          =default;
            TicketTitle       =ticketTitle;
            TicketDescription =ticketDescription;
            TicketStatus      =ticketStatus;
            TicketCategory    =ticketCategory;
            TicketCreatedBy   =ticketCreatedBy;
            TicketCreationDate=ticketCreationDate;
            TicketResolvedDate=ticketResolvedDate;
            TicketDocumentURLS=ticketDocumentURLS;
        }

        /// <summary>
        /// Ticket Constructor that creates an autopopulated Ticket with data from a DTO <seealso cref="TicketCreateDto"/>
        /// </summary>
        /// <param name="dto"> Limited Ticket object for creationg a ticktes</param>
        public Ticket(TicketCreateDto dto)
        {
            Debug.WriteLine("DTO convert");

            TicketID           = default;
            TicketTitle        = dto.TicketTitle;
            TicketDescription  = dto.TicketDescription;
            TicketStatus       = Status.Created;
            TicketCategory     = dto.TicketCategory;
            TicketCreatedBy    = dto.TicketCreatedBy;
            TicketCreationDate = DateTime.Now;
            TicketResolvedDate = null;
            TicketDocumentURLS = dto.TicketDocumentURLS;
        }
        //Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJmaXJlYmFzZS1hZG1pbnNkay04NXh3c0B2Y3RpY2tldHRyYWNrZXIuaWFtLmdzZXJ2aWNlYWNjb3VudC5jb20iLCJqdGkiOiI1ZmFlOGVlOS02NmM2LTRkZDQtODEwNS1lODk2YTU1YWE5YTciLCJpYXQiOiIxNzMwMzkxMDkxIiwicm9sZSI6IlNlcnZlckFkbWluIiwidWlkIjoiU1QxMDAyMTkwNkB2Y2Nvbm5lY3QuZWR1LnphIiwiZXhwIjoxNzMwNDc3NDkxLCJpc3MiOiJmaXJlYmFzZS1hZG1pbnNkay04NXh3c0B2Y3RpY2tldHRyYWNrZXIuaWFtLmdzZXJ2aWNlYWNjb3VudC5jb20iLCJhdWQiOiJodHRwczovL2lkZW50aXR5dG9vbGtpdC5nb29nbGVhcGlzLmNvbS9nb29nbGUuaWRlbnRpdHkuaWRlbnRpdHl0b29sa2l0LnYxLklkZW50aXR5VG9vbGtpdCJ9.mqxrsaZ2LoFOXCqII81FjwmqIRaQQE_G73YetkIUK4Q
        /// <summary>
        /// Gets a specific ticket from teh database
        /// </summary>
        /// <param name="Id"> ticket Id to be search for</param>
        /// <returns> Returns a Fully populated ticket</returns>
        public Ticket? GetSelectedTicket(long Id)
        {

            var tic = DatabaseAccesUtil.GetTicketID(Id);
            //tic.GetDetails();
            return tic;
        }

        /// <summary>
        /// Gets Tickets for the specified user di
        /// </summary>
        /// <param name="UId"> USer Id to be search for</param>
        /// <returns> Returns a list of Fully populated tickets</returns>
        public List<Ticket> GetSelectedTicketList(long UId)
        {
            var tic =DatabaseAccesUtil.GetUserTickets(UId);
            foreach(var item in tic)
            {
                item.GetDetails();
            }
            return tic;
        }

        /// <summary>
        /// Gets the details stored in seperate tables from the database,
        /// and populate the current object
        /// </summary>
        public void GetDetails()
        {
            TicketUpdates = DatabaseAccesUtil.GetTicketEvent(TicketID);
        }

        /// <summary>
        /// Attempts to insert the current Object in to teh Database
        /// </summary>
        /// <returns> Bool indicating if the action was belived to be success ful or not </returns>
        public long Insert()
        {
            Debug.WriteLine("Insert");
            var created = DatabaseAccesUtil.InsertTicket(this, new TicketEvent(TicketID, DateTime.Now, TicketCreatedBy, Status.Created, Status.Unassgined, "Creation", $"{TicketTitle}:{TicketDescription}"));
            TicketID = created;
            this.Assgin();

            return created;
        }

        /// <summary>
        /// Method attempst to Automaticly assgin its self to any avalible user who meets the,
        /// specified contions.
        /// Creates an Ticket update Event.<seealso cref="Update(TicketUpdate)"/>
        /// WIP
        /// </summary>
        /// <returns> Bool indicating if the action was belived to be success ful or not </returns>
        public bool Assgin()
        {
            var ticDep = TicketCategory.Depart;
            Debug.WriteLine(ticDep);
            var users = DatabaseAccesUtil.GetAvalibleSupport(ticDep);
            if(users.Count > 0)
            {

                AssignedTo = users.First().Id;
                AssginUpdate(users.First());
            }
            return true;
        }

        /// <summary>
        /// Attempts to assgin the ticket to a specified user.
        /// WIP
        /// </summary>
        /// <returns> Bool indicating if the action was belived to be success ful or not </returns>
        public bool Assgin(long userId)
        {
            var users = DatabaseAccesUtil.GetUser(userId);
            if(users != null)
            {

                AssignedTo = userId;
                AssginUpdate(users);
            }
            return true;
        }
        public bool Assgin(long userId, long by)
        {
            var users = DatabaseAccesUtil.GetUser(userId);
            if(users != null)
            {

                AssignedTo = userId;
                AssginUpdate(users, by);
            }

            return true;
        }

        public bool AssginUpdate(IApplicationUser user, long By)
        {
            Status status = Status.Unassgined;
            if(user.UsersTypes == UserType.Lecturer)
            {
                status = Status.WithLecturer;
            }
            else if(user.UsersTypes == UserType.SupportMember)
            {
                status = Status.WithSupport;
            }
            else if(user.UsersTypes == UserType.Administrator)
            {
                status = Status.WithExternal;
            }

            var ticketUpdate = new TicketUpdate(TicketID,By,status,"Assgin", $"Ticket is Being Assgined To:{user.Email}");

            var newEvent = new TicketEvent(TicketID, DateTime.Now, ticketUpdate.updatedBy, (Status)TicketStatus, ticketUpdate.statusSetTo, ticketUpdate.eventType, ticketUpdate.eventComments);
            AssignedTo = user.Id;
            NotificationHandler.SendNotification(ticketUpdate.eventComments, ticketUpdate.statusSetTo, user);
            DatabaseAccesUtil.TicketAssginedUP(TicketID, user.Id, status);
            return DatabaseAccesUtil.InsertTicketEvent(newEvent);
        }

        public bool AssginUpdate(IApplicationUser user)
        {
            Status status = Status.Unassgined;
            if(user.UsersTypes == UserType.Lecturer)
            {
                status = Status.WithLecturer;
            }
            else if(user.UsersTypes == UserType.SupportMember)
            {
                status = Status.WithSupport;
            }
            else if(user.UsersTypes == UserType.Administrator)
            {
                status = Status.WithExternal;
            }
            var ticketUpdate = new TicketUpdate(TicketID,1,Status.WithSupport,"Assgin", $"Ticket is Being Assgined To:{user.Email}");

            var newEvent = new TicketEvent(TicketID, DateTime.Now, ticketUpdate.updatedBy, (Status)TicketStatus, ticketUpdate.statusSetTo, ticketUpdate.eventType, ticketUpdate.eventComments);
            AssignedTo = user.Id;
            NotificationHandler.SendNotification(ticketUpdate.eventComments, ticketUpdate.statusSetTo, user);
            DatabaseAccesUtil.TicketAssginedUP(TicketID, user.Id, status);
            return DatabaseAccesUtil.InsertTicketEvent(newEvent);
        }
        /// <summary>
        /// Creates a Ticket Event, and adds it to the database. And triggers notifications if required.
        /// </summary>
        /// <param name="ticketUpdate"> Details to be updated </param>
        /// <returns> Bool indicating if the action was belived to be success ful or not </returns>
        public bool Update(TicketUpdate ticketUpdate)
        {
            if(ticketUpdate.statusSetTo == Status.Resolved)
            {
                var newResolvedEvent = new TicketEvent(TicketID, DateTime.Now, ticketUpdate.updatedBy, (Status)TicketStatus, ticketUpdate.statusSetTo, "ticketUpdate", $"{TicketTitle}:{ticketUpdate.eventComments}");
                var updateDet =DatabaseAccesUtil.GetUser(TicketCreatedBy);
                NotificationHandler.SendNotification(ticketUpdate.eventComments, ticketUpdate.statusSetTo, updateDet);
                DatabaseAccesUtil.InsertTicketEvent(newResolvedEvent);
                return DatabaseAccesUtil.TicketResolved(TicketID);
            }
            else
            {

                var newEvent = new TicketEvent(TicketID, DateTime.Now, ticketUpdate.updatedBy, (Status)TicketStatus, ticketUpdate.statusSetTo, "ticketUpdate", $"{TicketTitle}:{ticketUpdate.eventComments}");
                var updateDet =DatabaseAccesUtil.GetUser(TicketCreatedBy);
                NotificationHandler.SendNotification(ticketUpdate.eventComments, ticketUpdate.statusSetTo, updateDet);
                DatabaseAccesUtil.TicketStatusUP(TicketID, ticketUpdate.statusSetTo);
                return DatabaseAccesUtil.InsertTicketEvent(newEvent);
            }
        }

        /// <summary>
        /// Sets the Document Urls to beassociated with a ticket as a seperate Ticket Event
        /// </summary>
        /// <param name="ticketUpdate"> <seealso cref="TicketUpdate"/></param>
        /// <param name="Urls"> string delimted by ',' of sorage path</param>
        /// <returns> Bool indicating if the action was belived to be success ful or not </returns>
        public bool AddDocument(TicketUpdate ticketUpdate, string Urls)
        {
            //DatabaseAccesUtil.UpdateTicket();
            TicketDocumentURLS +=$"{Urls}, ";
            //var p = new TicketEvent(TicketID, DateTime.Now, up.updatedBy, (Status)TicketStatus, up.statusSetTo, "Document Upload", $"{TicketTitle}:{up.eventComments}");
            Update(ticketUpdate);
            return DatabaseAccesUtil.TicketUrlUP(TicketID, TicketDocumentURLS);

        }

        /// <summary>
        /// Delets a ticket and its events from the database
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Delete()
        {
            //DatabaseAccesUtil.Unasgin;
            return DatabaseAccesUtil.TicketDelete(TicketID);
            //DatabaseAccesUtil.DeleteTicket;
            //return true;
        }

        /// <summary>
        /// Retrives a primary list of ticket based on  a criteria And sorts.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void sort()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets tickets with a particular status
        /// </summary>
        /// <param name="stat"> The Status to filter on <seealso cref="Status"/> </param>
        /// <returns> Returns a list of Ticketc with teh current status specified </returns>
        public List<Ticket> GetStatus(Status stat)
        {
            return DatabaseAccesUtil.GetStatusTickets(stat);
        }

        /// <summary>
        /// Gets tickets created by a specific user
        /// </summary>
        /// <param name="userID"> The UserId to filter on </param>
        /// <returns> Returns a list of Tickets assgned to the Specifc user </returns>
        public List<Ticket> GetCreated(long userID)
        {
            return DatabaseAccesUtil.GetCreatedTickets(userID);
        }

        /// <summary>
        /// Gets tickets assgined to a specific user
        /// </summary>
        /// <param name="userID"> The UserId to filter on </param>
        /// <returns> Returns a list of Tickets assgned to the Specifc user </returns>
        public List<Ticket> GetAssgined(long userID)
        {
            return DatabaseAccesUtil.GetAssginedTickets(userID);
        }

        // REVIEW
        public override string ToString()
        {
            string res = "";
            foreach(var item in TicketUpdates)
            {
                res += item.ToString()+"|";
            }
            return $"    TicketID: {TicketID,-8} " +
                   $"   CreatedBy: {TicketCreatedBy,-3}, " +
                   $"      Status: {TicketStatus,-8}\n " +
                   $"       Title: {TicketTitle,-32}, " +
                   //$"Description: {TicketDescription}, " +
                   $"    Category: {TicketCategory,-24}, " +
                   $"CreationDate: {TicketCreationDate?.ToString(),-10}, " +
                   $"ResolvedDate: {TicketResolvedDate?.ToString(),-10}, \n" +
                   //$"Documents: {TicketDocumentURLS}" +
                   $"-------TicketEvents-------\n" +
                   $"{res}";
        }

    }
}
