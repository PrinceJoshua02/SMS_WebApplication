using SMS_WebApplication.Models;
using SMS_WebApplication.ViewModel;
using Newtonsoft.Json;
using SMS_WebApplication.Repositories;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;

namespace SMS_WebApplication.Repositories.Api
{
    public class StudentRestRepository : IStudentRepository
    {
        

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configs;
        private readonly string _baseURL;

        public StudentRestRepository(IConfiguration configs)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(httpClientHandler);
            _configs = configs;
            _baseURL = "https://localhost:7076/api"; // Corrected base URL
        }

        public StudentViewModel AddStudents(StudentViewModel newStudent, string token)
        {
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _configs.GetValue<string>("ApiKey"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); // Assuming 'token' is defined

            string data = JsonConvert.SerializeObject(newStudent);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var queryParameters = new Dictionary<string, string>
            {
                { "First_Name", newStudent.First_Name },
                { "Last_Name", newStudent.Last_Name },
                { "Middle_Name", newStudent.Middle_Name },
                { "Address", newStudent.Address },
                { "DOB", newStudent.DOB }
            };

            string queryString = string.Join("&", queryParameters.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}")); // Fixed encoding of query parameters
            string fullURL = $"{_baseURL}/Student?{queryString}";

            var response = _httpClient.PostAsync(fullURL, content).Result;
            if (response.IsSuccessStatusCode)
            {
                // Handle successful response here
            }
            return null;
        }

        public async Task DeleteStudent(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _configs.GetValue<string>("ApiKey"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync(_baseURL + "/Student/?Id=" + id);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to delete selected student. Error: " + response.StatusCode);
            }
        }

        public Student GetStudentById(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _configs.GetValue<string>("ApiKey"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); // Assuming 'token' is defined

            string fullURL = $"{_baseURL}/Student/{id}";

            var response = _httpClient.GetAsync(fullURL).Result;
            if (response.IsSuccessStatusCode)
            {
                var responseData = response.Content.ReadAsStringAsync().Result;
                var student = JsonConvert.DeserializeObject<Student>(responseData);
                return student;
            }
            else
            {
                return null;
            }
        }

        public List<Student> GetStudents(string token)
        {
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _configs.GetValue<string>("ApiKey"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); // Assuming 'token' is defined

            string fullURL = $"{_baseURL}/Student";

            var response = _httpClient.GetAsync(fullURL).Result;
            if (response.IsSuccessStatusCode)
            {
                var responseData = response.Content.ReadAsStringAsync().Result;
                var student = JsonConvert.DeserializeObject<List<Student>>(responseData);
                return student;
            }
            else
            {
                return null;
            }
        }

        public async Task<Student> UpdateStudent(int id, Student newStudent, string token)
        {
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _configs.GetValue<string>("ApiKey"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string data = JsonConvert.SerializeObject(newStudent);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            
            string fullURL = _baseURL + "/Student/?Id=" + newStudent.Id + "&First_Name=" + newStudent.First_Name + "&Last_Name=" + newStudent.Last_Name + "&Middle_Name=" + newStudent.Middle_Name + "&Address=" + newStudent.Address + "&DOB=" + newStudent.DOB;
            var response = await _httpClient.PutAsync(fullURL, content);

            if (response.IsSuccessStatusCode)
            {
                return newStudent;
            }
            else
            {
                return null;
            }
        }

    }
}
