using System.Text.Json;
using System.Text.Json.Serialization;

namespace VCTicketTrackerAPIClassLibrary.Orginisation
{
    /// <summary>
    /// Department
    /// <example>
    /// <code>
    /// 
    ///{
    ///  "departmentID": 0,
    ///  "departmentName": "string"
    ///}
    /// </code>
    /// </example>
    /// </summary>
    public record Department(int DepartmentID, string DepartmentName);

    // DTO class for Department
    [JsonConverter(typeof(DepartmentDtoConverter))]
    public class DepartmentDto
    {
        public string DepartmentName { get; set; }
        public int DepartmentId { get; set; }
        public object AdditionalInfo { get; set; }
    }

    // Custom converter to handle DepartmentDto in multiple forms (string, number, or object)
    public class DepartmentDtoConverter : JsonConverter<DepartmentDto>
    {
        public override DepartmentDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Handle string input (Department Name)
            if(reader.TokenType == JsonTokenType.String)
            {
                return new DepartmentDto { DepartmentName = reader.GetString() };
            }

            // Handle integer input (Department ID)
            if(reader.TokenType == JsonTokenType.Number)
            {
                return new DepartmentDto { DepartmentId = reader.GetInt32() };
            }

            // Handle object input (complex Department structure)
            if(reader.TokenType == JsonTokenType.StartObject)
            {
                // Read the JSON document
                using(JsonDocument doc = JsonDocument.ParseValue(ref reader))
                {
                    var root = doc.RootElement;

                    var departmentName = root.GetProperty("name").GetString();
                    var departmentId = root.GetProperty("id").GetInt32();

                    // Optional: Check for AdditionalInfo if it exists
                    object additionalInfo = null;
                    if(root.TryGetProperty("additionalInfo", out JsonElement additionalInfoElement))
                    {
                        additionalInfo = additionalInfoElement.GetRawText();
                    }

                    return new DepartmentDto {
                        DepartmentName = departmentName,
                        DepartmentId = departmentId,
                        AdditionalInfo = additionalInfo
                    };
                }
            }

            // Throw an exception if none of the expected Token types were found
            throw new JsonException("Unexpected JSON Token when reading DepartmentDto.");
        }

        public override void Write(Utf8JsonWriter writer, DepartmentDto value, JsonSerializerOptions options)
        {
            // Write as a string if DepartmentName is provided
            if(!string.IsNullOrEmpty(value.DepartmentName))
            {
                writer.WriteStringValue(value.DepartmentName);
            }
            // Write as an integer if DepartmentId is provided
            else if(value.DepartmentId != 0)
            {
                writer.WriteNumberValue(value.DepartmentId);
            }
            // Write as an object if complex data exists
            else
            {
                writer.WriteStartObject();
                writer.WriteString("name", value.DepartmentName);
                writer.WriteNumber("id", value.DepartmentId);

                // If AdditionalInfo exists, write it as a JSON property
                if(value.AdditionalInfo != null)
                {
                    writer.WritePropertyName("additionalInfo");
                    writer.WriteRawValue(value.AdditionalInfo.ToString());
                }

                writer.WriteEndObject();
            }
        }
    }
}