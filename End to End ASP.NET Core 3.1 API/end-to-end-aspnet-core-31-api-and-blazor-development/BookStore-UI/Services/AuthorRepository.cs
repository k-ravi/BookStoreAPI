using Blazored.LocalStorage;
using BookStore_UI.Contracts;
using BookStore_UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookStore_UI.Services
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        readonly IHttpClientFactory _httprClient;
        readonly ILocalStorageService _localStorage;
        public AuthorRepository(IHttpClientFactory httpClient, ILocalStorageService localStorage) :base(httpClient, localStorage)
        {
            _httprClient = httpClient;
        }
    }
}
