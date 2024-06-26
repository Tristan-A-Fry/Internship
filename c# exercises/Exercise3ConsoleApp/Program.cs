using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Exercise3ConsoleApp
{
    public class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static bool isAdmin = false;

        public static async Task Main(string[] args)
        {
            Console.WriteLine("Starting the application...");

            // Authentication process
            if (await AuthenticateUser())
            {
                if (isAdmin)
                {
                    // Run exercise code
                    ProgramProductData.ProcessProductData();
                    ProgramOutputFile.OutputFile();
                    // Add any additional method calls here
                }
                else
                {
                    Console.WriteLine("You do not have the necessary permissions to run this program.");
                }
            }
            else
            {
                Console.WriteLine("Authentication failed. Exiting application...");
            }
        }

        private static async Task<bool> AuthenticateUser()
        {
            Console.Write("Enter username: ");
            var username = Console.ReadLine();

            Console.Write("Enter password: ");
            var password = Console.ReadLine();

            var loginData = new { Username = username, Password = password };
            var json = JsonConvert.SerializeObject(loginData);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("http://localhost:5028/api/auth/login", data); // Change the URL to your API endpoint
                var result = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Response: {result}"); // Debugging: Print the response

                if (response.IsSuccessStatusCode)
                {
                    var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(result);
                    Console.WriteLine("Authentication successful.");
                    Console.WriteLine($"Token: {tokenResponse.Token}");
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResponse.Token);

                    // Decode the JWT token to extract roles
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(tokenResponse.Token) as JwtSecurityToken;

                    var roleClaim = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "role")?.Value;
                    Console.WriteLine($"Roles: {roleClaim}");

                    // Check if the user is an admin
                    if (roleClaim == "Admin")
                    {
                        isAdmin = true;
                    }
                    return true;
                }
                else
                {
                    Console.WriteLine("Invalid username or password.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }
    }
}




