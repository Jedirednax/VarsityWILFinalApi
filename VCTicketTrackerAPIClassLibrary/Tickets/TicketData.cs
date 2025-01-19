namespace VCTicketTrackerAPIClassLibrary.Tickets
{
    /// <summary>
    /// <example>
    /// <code>
    /// {
    ///  "ticketTitle": "string",
    ///  "ticketDescription": "string",
    ///  "ticketDocumentURLS": "string",
    ///  "category": {
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
    ///  }
    ///}
    /// </code>
    /// </example>
    /// </summary>
    public class TicketData
    {
        public string? TicketTitle { get; set; }
        public string? TicketDescription { get; set; }
        // public Categories TicketCategory { get; set; }

        public string TicketDocumentURLS { get; set; }

        public Category Category { get; set; }


        public TicketData() { }

        public TicketData(string? ticketDescription, string ticketDocumentURLS)
        {
            TicketDescription=ticketDescription;
            TicketDocumentURLS=ticketDocumentURLS;
        }

        public TicketData(string? ticketTitle, string? ticketDescription, string ticketDocumentURLS)
        {
            TicketTitle=ticketTitle;
            TicketDescription=ticketDescription;
            TicketDocumentURLS=ticketDocumentURLS;
        }
    }
}

