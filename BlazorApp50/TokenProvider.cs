using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp50
{
    public class TokenProvider
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string XsrfToken { get; set; }
    }

    public class InitialApplicationState
    {
        public string? XsrfToken { get; set; }
    }
}
