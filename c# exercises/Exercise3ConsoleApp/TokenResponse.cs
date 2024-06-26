namespace Exercise3ConsoleApp
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public string[] Roles { get; set; } // Ensure this matches the structure of your JWT response
    }
}
