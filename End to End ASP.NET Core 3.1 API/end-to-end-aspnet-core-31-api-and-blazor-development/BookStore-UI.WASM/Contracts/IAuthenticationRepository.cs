using BookStore_UI.WASM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_UI.WASM.Contracts
{
    public interface IAuthenticationRepository
    {
         Task<bool> Register(RegisterModel user);
        Task<bool> LogIn(LogInModel logInModel);
        Task LogOut();
    }
}
