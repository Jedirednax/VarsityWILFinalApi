namespace VCTicketTrackerAPIClassLibrary.Tickets
{
    /// <summary>
    /// <example>
    /// <code>
    /// {
    ///  "ticketID": 0,
    ///  "updatedBy": 0,
    ///  "statusSetTo": "Created",
    ///  "eventType": "string",
    ///  "eventComments": "string"
    ///}
    /// </code>
    /// </example>
    /// </summary>
    public class TicketUpdate
    {
        public long ticketID { get; set; }
        public long updatedBy { get; set; }
        public Status statusSetTo { get; set; }
        public string eventType { get; set; }
        public string? eventComments { get; set; }

        public TicketUpdate(long ticketId, long updatedBy, Status statusSetTo, string eventType, string eventComments)
        {
            this.ticketID = ticketId;
            this.updatedBy=updatedBy;
            this.statusSetTo=statusSetTo;
            this.eventType=eventType;
            this.eventComments=eventComments;
        }

        public override string? ToString()
        {
            return $"{ticketID}:{updatedBy}:{statusSetTo}:{eventType}:{eventComments}\n";
        }
    }
}

