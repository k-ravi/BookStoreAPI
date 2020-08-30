using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_UI.Static
{
    public static class EndPoints
    {
        public static string BaseUrl = "https://localhost:44307";
        public static string AuthorsEndPoint = $"{BaseUrl}/api/authors";
        public static string BooksEndPoint = $"{BaseUrl}/api/books";
        public static string RegisterEndPoint = $"{BaseUrl}/api/Users/register";
        public static string LoginEndPoint = $"{BaseUrl}/api/Users/login";
    }
}
