using Blazored.LocalStorage;
using BookStore_UI.WASM.Contracts;
using BookStore_UI.WASM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookStore_UI.WASM.Services
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        readonly HttpClient _httprClient;
        readonly ILocalStorageService _localStorage;
        public AuthorRepository(HttpClient httpClient, ILocalStorageService localStorage) :base(httpClient, localStorage)
        {
            _httprClient = httpClient;
            _localStorage = localStorage;
        }
    }
}
