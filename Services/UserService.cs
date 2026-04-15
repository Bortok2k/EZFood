using EZFood.Models;
using System.Net.Http.Json;

namespace EZFood.Services
{
    public class UserService
    {
        private readonly HttpClient _http;

        public UserService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<User>> GetUsers()
        {
            return await _http.GetFromJsonAsync<List<User>>("users");
        }

        public async Task CreateUser(User user)
        {
            await _http.PostAsJsonAsync("users", user);
        }
    }
}
