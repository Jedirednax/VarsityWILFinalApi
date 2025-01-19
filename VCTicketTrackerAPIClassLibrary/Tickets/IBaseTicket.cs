using VCTicketTrackerAPIClassLibrary.Auth;
using VCTicketTrackerAPIClassLibrary.UserClasses;

namespace VCTicketTrackerAPIClassLibrary.Tickets
{
    /// <summary>
    /// <example>
    /// <code>
    /// {
    ///  "metadata": {
    ///    "ticketID": 0,
    ///    "assignedTo": 0,
    ///    "ticketCreatedBy": 0,
    ///    "ticketCreationDate": "2024-11-20T11:30:17.835Z",
    ///    "ticketResolvedDate": "2024-11-20T11:30:17.835Z"
    ///  },
    ///  "data": {
    ///    "ticketTitle": "string",
    ///    "ticketDescription": "string",
    ///    "ticketDocumentURLS": "string",
    ///    "category": {
    ///        "id": 0,
    ///      "name": "string",
    ///      "description": "string",
    ///      "depart": 0,
    ///      "priority": 0,
    ///      "parentType": "string",
    ///      "fields": {
    ///            "additionalProp1": "string",
    ///        "additionalProp2": "string",
    ///        "additionalProp3": "string"
    ///      }
    ///    }
    ///},
    ///  "userInfo": {
    ///    "userName": "string",
    ///    "normalizedUserName": "string",
    ///    "email": "string",
    ///    "normalizedEmail": "string",
    ///    "passwordHash": "string",
    ///    "phoneNumber": "string",
    ///    "id": 0,
    ///    "firstName": "string",
    ///    "lastName": "string",
    ///    "userType": "ServerAdmin",
    ///    "authenticationType": "string",
    ///    "isAuthenticated": true,
    ///    "emailConfirmed": true,
    ///    "phoneNumberConfirmed": true,
    ///    "twoFactorEnabled": true,
    ///    "lockoutEnd": "2024-11-20T11:30:17.835Z",
    ///    "lockoutEnabled": true,
    ///    "accessFailedCount": 0,
    ///    "securityStamp": "string",
    ///    "concurrencyStamp": "string",
    ///    "fcmToken": "string"
    ///  },
    ///  "ticketUpdates": [
    ///    {
    ///      "eventId": 0,
    ///      "ticketId": 0,
    ///      "date": "2024-11-20T11:30:17.835Z",
    ///      "updtedBy": 0,
    ///      "statusFrom": "Created",
    ///      "statusTo": "Created",
    ///      "type": "string",
    ///      "comment": "string"
    ///    }
    ///  ]
    ///}
    /// </code>
    /// </example>
    /// </summary>
    public class BaseTicket
    {

        public TicketMetaData Metadata { get; set; }
        public TicketData Data { get; set; }
        public ApplicationUser UserInfo { get; set; }
        public List<TicketEvent> TicketUpdates { get; set; } = new List<TicketEvent>();

        public List<TicketEvent> GetEvents()
        {
            return new List<TicketEvent>();
        }

        public ApplicationUser GetUserData()
        {
            return new ServerAdmin();
        }
    }
}

