using System;
using System.IO;
using System.Threading.Tasks;
using GitHubAppAuth.Services;

namespace GitHubAppAuth
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Configuration
                var privateKeyPath = "path/to/private-key.pem"; // r .pem file
                var appId = 123456; //  App ID
                var installationId = 987654321; // Installation ID

                // private key
                var privateKey = File.ReadAllText(privateKeyPath);

                // Generate the JWT
                var jwtGenerator = new GitHubJwtGenerator(privateKey, appId);
                var jwt = jwtGenerator.GenerateJwt();

                Console.WriteLine("JWT generated successfully.");

                // Get the installation token
                var authService = new GitHubAuthService(jwt);
                var installationToken = await authService.GetInstallationTokenAsync(installationId);

                Console.WriteLine("Installation token: " + installationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
