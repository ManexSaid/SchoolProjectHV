using System.Text.Json.Serialization;

namespace PatientJournalMVC.Models
{
    public class Patient
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("personnummer")]
        public string Personnummer { get; set; } = string.Empty;

        [JsonPropertyName("fornamn")]
        public string Fornamn { get; set; } = string.Empty;

        [JsonPropertyName("efternamn")]
        public string Efternamn { get; set; } = string.Empty;
    }
}