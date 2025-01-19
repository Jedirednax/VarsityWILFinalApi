namespace VCTicketTrackerAPIClassLibrary.Tickets
{
    /// <summary>
    /// Ticket Data transfer object
    /// <example>
    /// <code>
    /// {
    ///  "ticketTitle": "string",
    ///  "ticketDescription": "string",
    ///  "ticketCategory": {
    ///    "id": 0,
    ///    "name": "string",
    ///    "description": "string",
    ///    "depart": 0,
    ///    "priority": 0,
    ///    "parentType": "string",
    ///    "fields": {
    ///      "additionalProp1": "string",
    ///      "additionalProp2": "string",
    ///      "additionalProp3": "string"
    ///    }
    ///  },
    ///  "ticketCreatedBy": 0,
    ///  "ticketDocumentURLS": "string"
    ///}
    /// </code>
    /// </example>
    /// </summary>
    public class TicketCreateDto
    {
        /// <value>
        /// Property <c> TicketTitle </c> Title of the 
        /// </value>
        public string? TicketTitle { get; set; }

        /// <value>
        /// Property <c> TicketDescription </c> Description for the ticket
        /// </value>
        public string? TicketDescription { get; set; }

        /// <value>
        /// Property <c> TicketCategory </c> <see cref="Category"/>
        /// </value>
        public Category TicketCategory { get; set; }

        /// <value>
        /// Property <c> TickerCreatedBy </c> UserID of user who created the ticket
        /// </value>
        public long TicketCreatedBy { get; set; }

        /// <value>
        /// Property <c> DocuemntURLS </c> Path of stored location of documnets relavent to the ticket
        /// </value>
        public string? TicketDocumentURLS { get; set; }

        public TicketCreateDto() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticketTitle">  </param>
        /// <param name="ticketDescription"></param>
        /// <param name="ticketCategory"></param>
        /// <param name="ticketCreatedBy"></param>
        /// <param name="ticketDocumentURLS"></param>
        public TicketCreateDto(string? ticketTitle, string? ticketDescription, Category ticketCategory, long ticketCreatedBy, string? ticketDocumentURLS)
        {
            TicketTitle=ticketTitle;
            TicketDescription=ticketDescription;
            TicketCategory=ticketCategory;
            TicketCreatedBy=ticketCreatedBy;
            TicketDocumentURLS=ticketDocumentURLS;
        }
    }
}
