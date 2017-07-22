using System;
using System.Collections;
using System.Collections.Generic;

namespace Traitorstown.src.http.request
{
    [Serializable]
    public class LoginRequest
    {
        public String password;
        public String email;

        public LoginRequest(string password, string email)
        {
            this.password = password;
            this.email = email;
        }
    }
}

