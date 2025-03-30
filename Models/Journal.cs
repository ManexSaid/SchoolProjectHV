using System.Text.Json.Serialization;

namespace PatientJournalMVC.Models
{
    public class Journal
    {
        [JsonPropertyName("journalId")]
        public int JournalId { get; set; }

        [JsonPropertyName("personnummer")]
        public string Personnummer { get; set; } = string.Empty;

        [JsonPropertyName("anteckning")]
        public string Anteckning { get; set; } = string.Empty;
    }
}