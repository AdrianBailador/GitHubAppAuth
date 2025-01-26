using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace GitHubAppAuth.Services
{
    public class GitHubAuthService
    {
        private readonly string _jwt;

        public GitHubAuthService(string jwt)
        {
            _jwt = jwt;
        }

        public async Task<string> GetInstallationTokenAsync(long installationId)
        {
            using var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwt);
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MyGitHubApp", "1.0"));

            var url = $"https://api.github.com/app/installations/{installationId}/access_tokens";

            var response = await httpClient.PostAsync(url, null);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to get installation token: {error}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(responseContent);

            return json.RootElement.GetProperty("token").GetString();
        }
    }
}
