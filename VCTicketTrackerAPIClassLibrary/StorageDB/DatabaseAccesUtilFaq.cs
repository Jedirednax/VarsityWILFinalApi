using System.Data;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using VCTicketTrackerAPIClassLibrary.Supporting;
namespace VCTicketTrackerAPIClassLibrary.StorageDB
{
    public partial class DatabaseAccesUtil
    {

        public static List<FAQ>? GetFAQ(int page, int rows = 10)
        {
            Debug.WriteLine($"Get Faqs:", "Database-SELECT");
            Debug.IndentLevel++;
            Debug.WriteLine($"Number of ROws:{rows}");
            Debug.WriteLine($"Page Number:   {page}");
            Debug.IndentLevel--;

            List<FAQ> fetching = new List<FAQ>();
            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                string strSelect = $"SELECT TOP {rows} * FROM FAQ WHERE QuestionID >= {(rows * page)} ORDER BY QuestionID;";

                SqlCommand cmdSelect = new SqlCommand();
                cmdSelect.CommandText = strSelect;

                cmdSelect.Connection = conn;
                conn.Open();
                using(SqlDataReader userReader = cmdSelect.ExecuteReader())
                {
                    while(userReader.Read())
                    {
                        FAQ faq = new FAQ(

                    questionID     :(long)userReader["QuestionID"],
                    answer: (string)userReader["Answer"],
                    question: (string)userReader["Question"],
                    linkedDocument: (string)userReader["LinkedDocument"],
                    topic: (string)userReader["Topic"],
                    rating: (byte)userReader["Rating"]
                        );

                        fetching.Add(faq);
                    }
                }
                conn.Close();
            }
            return fetching;
        }

        public static List<FAQ>? GetFAQAnswer(string question = "", string answer = "", string topic = "", byte rating = 0)
        {
            Debug.WriteLine($"Search Faqs:", "Database-SELECT");
            Debug.IndentLevel++;
            Debug.WriteLine($"Question:{question}");
            Debug.WriteLine($"Answer:  {answer}");
            Debug.WriteLine($"Topic:   {topic}");
            Debug.WriteLine($"Rating:  {rating}");
            Debug.IndentLevel--;

            List<FAQ> fetching = new List<FAQ>();
            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                string strSelect = $"SELECT * FROM FAQ  WHERE " +
$"    (RTRIM(Question) LIKE '%' + @Question + '%' OR @Question IS NULL OR @Question = '') "+
$" AND (RTRIM(Answer) LIKE '%' + @Answer + '%' OR @Answer IS NULL OR @Answer = '') "+
$" AND (RTRIM(Topic) LIKE '%' + @Topic + '%' OR @Topic IS NULL OR @Topic = '') "+
$" AND (Rating >= @Rating OR @Rating IS NULL);";

                SqlCommand cmdSelect = new SqlCommand();
                cmdSelect.CommandText = strSelect;
                cmdSelect.Parameters.Add($"@Question", SqlDbType.VarChar).Value  = question;
                cmdSelect.Parameters.Add($"@Answer  ", SqlDbType.VarChar).Value  = answer;
                cmdSelect.Parameters.Add($"@Topic   ", SqlDbType.VarChar).Value  = topic;
                cmdSelect.Parameters.Add($"@Rating   ", SqlDbType.TinyInt).Value = rating;
                //cmdSelect.Parameters.Add($"@LinkedDocument", SqlDbType.VarChar).Value  = faq.LinkedDocument;
                cmdSelect.Connection = conn;
                conn.Open();
                using(SqlDataReader userReader = cmdSelect.ExecuteReader())
                {
                    while(userReader.Read())
                    {

                        FAQ faq = new FAQ(

                    questionID     :(long)userReader["QuestionID"],
                    answer: (string)userReader["Answer"],
                    question: (string)userReader["Question"],
                    linkedDocument: (string)userReader["LinkedDocument"],
                    topic: (string)userReader["Topic"],
                    //rating:0
                    rating:Convert.ToByte(userReader["Rating"])
                        );

                        fetching.Add(faq);
                    }
                }
                conn.Close();
            }
            return fetching;
        }

        public static long InsertFAQ(FAQ faq)
        {
            Debug.WriteLine($"Insert New Faq:", "Database-INSERT");
            Debug.IndentLevel++;
            Debug.WriteLine($"Number of ROws:\n\t{faq}");
            Debug.IndentLevel--;

            List<FAQ> fetching = new List<FAQ>();
            using(SqlConnection conn = new SqlConnection(DatabaseConnection.Connection))
            {
                string strInsert = $"INSERT INTO FAQ(Question, Answer, LinkedDocument, Topic, Rating) " +
                    $" OUTPUT INSERTED.QuestionID "+
                    "Values(@Question, @Answer, @LinkedDocument, @Topic, @Rating);";

                SqlCommand cmdInsert = new SqlCommand(strInsert,conn);

                cmdInsert.Parameters.Add($"@Question", SqlDbType.VarChar).Value        = faq.Question;
                cmdInsert.Parameters.Add($"@Answer", SqlDbType.VarChar).Value          = faq.Answer;
                cmdInsert.Parameters.Add($"@LinkedDocument", SqlDbType.VarChar).Value  = faq.LinkedDocument;
                cmdInsert.Parameters.Add($"@Topic", SqlDbType.VarChar).Value           = faq.Topic;
                cmdInsert.Parameters.Add($"@Rating", SqlDbType.TinyInt).Value          = faq.Rating;

                conn.Open();
                try
                {
                    return (long)cmdInsert.ExecuteScalar();

                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.Message, "SqlInsertError");
                    return -1;
                }
            }
        }
    }
}
