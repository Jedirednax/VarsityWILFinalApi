using System.Diagnostics;
using Azure;
using Azure.Communication.Email;
using FirebaseAdmin.Messaging;
using VCTicketTrackerAPIClassLibrary.UserClasses;

namespace VCTicketTrackerAPIClassLibrary.Handlers
{
    public class NotificationHandler
    {
        // TODO add delegate and event TO ticket 
        // Thsi will update and send notic to assgeind and studnet


        public static void SendNotification(string content, Status status, IApplicationUser user)
        {
            SendEmail(content, status, user.Email);
            SendPushNot(content, status, user.fcmToken);
        }


        //public static void SendEmail(string content, Status status, string target)
        //{
        //
        //    var mail = new MailMessage();
        //    mail.From = new MailAddress("st10021906@vcconnect.edu.za");
        //    //DoNotReply@07d4e607-79c4-467a-bab7-d4d70079d2ad.azurecomm.net
        //    mail.To.Add(new MailAddress(target));
        //    mail.Subject = status.ToString();
        //    mail.Body = content;
        //
        //    var smtp = new MailKit.Net.Smtp.SmtpClient();
        //    //smtp.Connect("smtp.gmail.com");
        //    //smtp.Authenticate("atcollen@gmail.com", "jolemjuwvwglbbsv");
        //    //smtp.Send((MimeMessage)mail);
        //    Console.WriteLine("Email Sent");
        //}
        public static void SendPushNot(string content, Status status, string target)
        {
            try
            {

                var message = new Message
                {
                    Token = target,
                    Notification = new Notification
                    {
                        Title = "Ticket Update",
                        Body = content
                    }
                };

                // Send the message asynchronously
                var response = FirebaseMessaging.DefaultInstance.SendAsync(message).Result;

                //Console.WriteLine($"Push notification sent successfully: {response}");
            }
            catch { }
        }



        public static void SendEmail(string content, Status status, string target)
        {
            //string connectionString = Environment.GetEnvironmentVariable("COMMUNICATION_SERVICES_CONNECTION_STRING");
            string connectionString = "endpoint=https://vctickettrackercomm.africa.communication.azure.com/;accesskey=5cpd7aMUms6jDlP4wXQLKWO1zKNtO12UaD8o4iAhJ6u1e8jqfWj4JQQJ99AKACULyCpKydBzAAAAAZCS8bn8";
            //;
            var emailClient = new EmailClient(connectionString);

            try
            {

                var emailMessage = new EmailMessage(
senderAddress: "DoNotReply@07d4e607-79c4-467a-bab7-d4d70079d2ad.azurecomm.net",
    content: new EmailContent("Ticket Status Update")
    {
        PlainText = content,
        Html = $""+
               $"<html>"+
               $"<body>"+
               $"   <h4>{status}</h4>"+
               $"   <br/>"+
               $"{content}"+
               $"   <br/>"+
               $"  </body>"+
               $"</html>"
    },

    recipients: new EmailRecipients(new List<EmailAddress> { new EmailAddress(target) }));
                //recipients: new EmailRecipients(new List<EmailAddress> { new EmailAddress("St10021906@vcconnect.edu.za") }));


                //EmailSendOperation emailSendOperation = emailClient.Send(W,emailMessage);
                emailClient.Send(WaitUntil.Started, emailMessage);
            }
            catch
            {
                Debug.WriteLine("Failds to send");
            }
        }
    }
}