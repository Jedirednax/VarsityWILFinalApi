using System.Text.Json.Serialization;

namespace VCTicketTrackerAPIClassLibrary.Supporting
{
    /// <summary>
    /// <example>
    /// <code>
    /// {
    ///  "questionId": 0,
    ///  "question": "string",
    ///  "answer": "string",
    ///  "linkedDocument": "string",
    ///  "topic": "string",
    ///  "rating": 0
    ///}
    /// </code>
    /// </example>
    /// </summary>
    public class FAQ
    {
        [JsonPropertyName("questionId")]
        public long QuestionID { get; set; } = default(int);
        [JsonPropertyName("question")]
        public string Question { get; set; }
        [JsonPropertyName("answer")]
        public string Answer { get; set; }
        [JsonPropertyName("linkedDocument")]
        public string LinkedDocument { get; set; }
        [JsonPropertyName("topic")]
        public string Topic { get; set; }
        [JsonPropertyName("rating")]
        public byte Rating { get; set; }

        public FAQ()
        { }

        public FAQ(long questionID, string question, string answer, string linkedDocument, string topic, byte rating)
        {
            QuestionID      = questionID;
            Question        = question;
            Answer          = answer;
            LinkedDocument = linkedDocument;
            Topic           = topic;
            Rating          = rating;
        }
        public FAQ(string question, string answer, string linkedDocument, string topic, byte rating)
        {
            Question        = question;
            Answer          = answer;
            LinkedDocument = linkedDocument;
            Topic           = topic;
            Rating          = rating;
        }

        public override string ToString()
        {
            return $"{QuestionID}:{Question}:{LinkedDocument}:{Topic}\n{Answer}";
        }
    }
}



