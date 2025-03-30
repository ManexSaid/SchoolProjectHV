using System.Text;
using System.Text.Json;

namespace PatientJournalMVC.Models
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string PatientApiBase = "https://informatik12.ei.hv.se/PatientAPI/api/Patient/";
        private const string JournalApiBase = "https://informatik12.ei.hv.se/JournalAPI/api/Journal/";

        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Patient>?> GetPatientsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(PatientApiBase);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Patient>>(content, _options);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error fetching patients: {e.Message}");
                return null; 
            }
        }

        public async Task<Patient?> GetPatientByPersonnummerAsync(string personnummer)
        {
            var patients = await GetPatientsAsync();
            return patients?.FirstOrDefault(p => p.Personnummer == personnummer);
        }


        public async Task<List<Journal>?> GetJournalsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(JournalApiBase);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Journal>>(content, _options);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error fetching journals: {e.Message}");
                return null;
            }
        }

        public async Task<List<Journal>?> GetJournalsByPersonnummerAsync(string personnummer)
        {
            var journals = await GetJournalsAsync();
            return journals?.Where(j => j.Personnummer == personnummer).ToList();
        }


        public async Task<Journal?> CreateJournalAsync(Journal journal)
        {
            try
            {
                var json = JsonSerializer.Serialize(journal);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(JournalApiBase, data);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Journal>(content, _options);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error creating journal: {e.Message}");
                return null;
            }
        }
    }
}