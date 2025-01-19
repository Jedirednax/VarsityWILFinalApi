namespace VCTicketTrackerAPIClassLibrary.Tickets
{
    /// <summary>
    /// <example>
    /// <code>
    /// {
    ///  "ticketID": 0,
    ///  "assignedTo": 0,
    ///  "ticketCreatedBy": 0,
    ///  "ticketCreationDate": "2024-11-20T11:28:41.084Z",
    ///  "ticketResolvedDate": "2024-11-20T11:28:41.084Z"
    ///}
    /// </code>
    /// </example>
    /// </summary>
    public class TicketMetaData
    {
        public long TicketID { get; set; }
        public long AssignedTo { get; set; } = default(int);
        public long TicketCreatedBy { get; set; }
        public DateTime? TicketCreationDate { get; set; } = DateTime.Now;
        public DateTime? TicketResolvedDate { get; set; } = DateTime.Now;

        public TicketMetaData(long ticketID, long assignedTo, long ticketCreatedBy, DateTime? ticketCreationDate, DateTime? ticketResolvedDate)
        {
            TicketID=ticketID;
            AssignedTo=assignedTo;
            TicketCreatedBy=ticketCreatedBy;
            TicketCreationDate=ticketCreationDate;
            TicketResolvedDate=ticketResolvedDate;
        }

        public TicketMetaData(long ticketID, long assignedTo, long ticketCreatedBy)
        {
            TicketID=ticketID;
            AssignedTo=assignedTo;
            TicketCreatedBy=ticketCreatedBy;
        }

    }
}

