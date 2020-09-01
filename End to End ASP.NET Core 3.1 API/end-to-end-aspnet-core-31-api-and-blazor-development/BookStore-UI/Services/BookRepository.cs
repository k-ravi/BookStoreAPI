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
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        readonly ILocalStorageService _localStorage;
        readonly IHttpClientFactory _httpClient;
        public BookRepository(ILocalStorageService localStorage, IHttpClientFactory httpClient) : base(httpClient, localStorage)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
        }
    }
}
