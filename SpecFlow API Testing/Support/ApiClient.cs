using System;
using System.Collections.Generic;
using System.Text;

namespace SpecFlow_API_Testing.Support
{
    public static class ApiClient
    {
        public static readonly HttpClient Client = new()
        {
            BaseAddress = new Uri("https://jsonplaceholder.typicode.com")
        };

    }
}
