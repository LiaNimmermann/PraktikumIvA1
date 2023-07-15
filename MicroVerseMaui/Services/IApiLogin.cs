using MicroVerseMaui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroVerseMaui.Services
{

    public interface IApiLogin
    {
        // Authenticates the user with the provided LoginInfo and returns a LoginResponse.
        Task<LoginResponse> Authenticate(LoginInfo loginRequest); 
    }
}
