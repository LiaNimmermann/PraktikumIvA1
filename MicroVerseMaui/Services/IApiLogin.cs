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
        Task<LoginResponse> Authenticate(LoginInfo loginRequest); 
    }
}



