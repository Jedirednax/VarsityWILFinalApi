using System.Text.Json.Serialization;

namespace VCTicketTrackerAPIClassLibrary.Tickets
{
    public class TicketAssgin
    {
        [JsonPropertyName("ticketId")]
        public long TicketID { get; set; }
        [JsonPropertyName("updatedBy")]
        public long UpdatedBy { get; set; }
        [JsonPropertyName("assigendtTo")]
        public long AssignedTo { get; set; }
        [JsonPropertyName("eventType")]
        public string EventType { get; set; }
        [JsonPropertyName("eventComments")]
        public string? EventComments { get; set; }

        public TicketAssgin(long ticketId, long updatedBy, long assignedTo, string eventType, string eventComments)
        {
            this.TicketID = ticketId;
            this.UpdatedBy=updatedBy;
            this.AssignedTo= assignedTo;
            this.EventType=eventType;
            this.EventComments=eventComments;
        }
        /*
        public override string? ToString()
        {
            return $"{ticketID}:{updatedBy}:{AssignedTo}:{eventType}:{eventComments}\n";
        }*/
    }
}

