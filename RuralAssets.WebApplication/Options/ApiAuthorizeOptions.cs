using System.Collections.Generic;

namespace RuralAssets.WebApplication
{
    public class ApiAuthorizeOptions
    {
        public Dictionary<string, string> AppAccount { get; set; }
        public int AllowMinutes { get; set; }
    }
}