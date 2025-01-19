using System.Text.Json.Serialization;

namespace VCTicketTrackerAPIClassLibrary.Tickets
{
    /// <summary>
    /// <example>
    /// <code>
    /// {
    ///  "eventId": 0,
    ///  "ticketId": 0,
    ///  "date": "2024-11-20T11:27:54.568Z",
    ///  "updtedBy": 0,
    ///  "statusFrom": "Created",
    ///  "statusTo": "Created",
    ///  "type": "string",
    ///  "comment": "string"
    ///}
    /// </code>
    /// </example>
    /// </summary>
    public class TicketEvent
    {
        [JsonInclude]
        [JsonPropertyName("eventId")]
        public long? TicketEventID { get; set; }

        [JsonInclude]
        [JsonPropertyName("ticketId")]
        public long? TicketID { get; set; }

        [JsonInclude]
        [JsonPropertyName("date")]
        public DateTime DateOfEvent { get; set; }

        [JsonInclude]
        [JsonPropertyName("updtedBy")]
        public long? UpdatedBy { get; set; }

        [JsonInclude]
        [JsonPropertyName("statusFrom")]
        public Status? StatusSetFrom { get; set; }
        [JsonInclude]
        [JsonPropertyName("statusTo")]
        public Status? StatusSetTo { get; set; }
        [JsonInclude]
        [JsonPropertyName("type")]
        public string? EventType { get; set; }
        [JsonInclude]
        [JsonPropertyName("comment")]
        public string? EventComments { get; set; }

        public TicketEvent(long? ticketEventID, long? ticketID, DateTime dateOfEvent, long? updatedBy, Status? statusSetFrom, Status? statusSetTo, string? eventType, string? eventComments)
        {
            TicketEventID=ticketEventID;
            TicketID     =ticketID;
            DateOfEvent  =dateOfEvent;
            UpdatedBy    =updatedBy;
            StatusSetFrom=statusSetFrom;
            StatusSetTo  =statusSetTo;
            EventType    =eventType;
            EventComments=eventComments;
        }
        public TicketEvent(long? ticketID, DateTime dateOfEvent, long? updatedBy, Status statusSetFrom, Status statusSetTo, string? eventType, string? eventComments)
        {
            TicketID=ticketID;
            DateOfEvent=dateOfEvent;
            UpdatedBy=updatedBy;
            StatusSetFrom=statusSetFrom;
            StatusSetTo=statusSetTo;
            EventType=eventType;
            EventComments=eventComments;
        }

        public TicketEvent()
        {
        }


        public override string? ToString()
        {
            return
                $"\n" +
                $"||TicketEventID:{TicketEventID,-10}||     TicketID:{TicketID,-16}||  DateOfEvent:{DateOfEvent,-10}||    UpdatedBy:{UpdatedBy,-10}\n"+
                $"||StatusSetFrom:{StatusSetFrom,-10}||  StatusSetTo:{StatusSetTo,-16}||    EventType:{EventType,-10}||\n"+
                $"||EventComments:{EventComments}\n";

        }
    }
}

