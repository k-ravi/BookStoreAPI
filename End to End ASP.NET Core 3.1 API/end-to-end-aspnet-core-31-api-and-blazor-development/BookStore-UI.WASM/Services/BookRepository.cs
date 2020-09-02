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
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        readonly ILocalStorageService _localStorage;
        readonly HttpClient _httpClient;
        public BookRepository(ILocalStorageService localStorage, HttpClient httpClient) : base(httpClient, localStorage)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
        }
    }
}
