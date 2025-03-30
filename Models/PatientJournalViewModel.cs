namespace PatientJournalMVC.Models
{
    public class PatientJournalViewModel
    {
        public Patient? Patient { get; set; }
        public List<Journal> Journals { get; set; } = new List<Journal>();
        public string? NewAnteckning { get; set; }
    }
}