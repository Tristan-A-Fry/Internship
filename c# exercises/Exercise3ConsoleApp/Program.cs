using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace Exercise2LINQ{

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
        await CallProtectedApiMethods();

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
        var response = await client.PostAsJsonAsync("https://yourapi.com/api/auth/login", loginModel);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("Invalid credentials.");
            return;
        }

        var result = await response.Content.ReadFromJsonAsync<AuthenticationResult>();
        _token = result?.Token;
    }

    private static async Task CallProtectedApiMethods()
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

        var response = await client.GetAsync("https://yourapi.com/api/example/products");

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            Console.WriteLine("Unauthorized access.");
            return;
        }

        var products = await response.Content.ReadFromJsonAsync<List<Product>>();
        // Handle products (for example, display them)
        foreach (var product in products)
        {
            Console.WriteLine($"{product.Name} - {product.Category} - {product.Price}");
        }
    }
}

public class AuthenticationResult
{
    public string Token { get; set; }
}
}