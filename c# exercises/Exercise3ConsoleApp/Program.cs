using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Exercise3ConsoleApp
{
    public class Program
    {
        private static string _token;

        public static async Task Main(string[] args)
        {
            await Authenticate();

            if (string.IsNullOrEmpty(_token))
            {
                Console.WriteLine("Authentication failed. Exiting application.");
                return;
            }

            // Now you can call API methods with the token
            var success = await CallProtectedApiMethods();

            if (!success)
            {
                Console.WriteLine("Unauthorized access detected. Exiting application.");
                return;
            }

            // Original functionalities
            ProgramProductData.ProcessProductData();
            ProgramOutputFile.OutputFile();
            // ProgramSearchBOEM.searchBoem().Wait();
        }

        private static async Task Authenticate()
        {
            Console.WriteLine("Enter Username:");
            var username = Console.ReadLine();
            Console.WriteLine("Enter Password:");
            var password = Console.ReadLine();

            var client = new HttpClient();
            var loginModel = new { Username = username, Password = password };
            var response = await client.PostAsJsonAsync("http://localhost:5263/api/auth/login", loginModel);

            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response: {responseContent}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Invalid credentials.");
                return;
            }

            try
            {
                var result = await response.Content.ReadFromJsonAsync<AuthenticationResult>();
                _token = result?.Token;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing JSON response: {ex.Message}");
            }
        }

        private static async Task<bool> CallProtectedApiMethods()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = await client.GetAsync("http://localhost:5263/api/example/products");

            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response: {responseContent}");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("Unauthorized access.");
                return false;
            }

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to fetch products. Status Code: {response.StatusCode}");
                return false;
            }

            try
            {
                var products = await response.Content.ReadFromJsonAsync<List<Product>>();
                if (products != null)
                {
                    // Handle products (for example, display them)
                    foreach (var product in products)
                    {
                        Console.WriteLine($"{product.Name} - {product.Category} - {product.Price}");
                    }
                }
                else
                {
                    Console.WriteLine("No products returned.");
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing JSON response: {ex.Message}");
                return false;
            }
        }
    }

    public class AuthenticationResult
    {
        public string Token { get; set; }
    }

    public class Product
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}


