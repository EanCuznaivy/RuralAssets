using System.Collections.Generic;

namespace RuralAssets.WebApplication
{
    public class ApiAuthorizeOptions
    {
        public Dictionary<string, string> AppAccount { get; set; }
        public int AllowMinutes { get; set; } = 10;
        public int TolerantMilliseconds { get; set; } = 30000;
    }
}