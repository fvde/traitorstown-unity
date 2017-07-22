using System;
using System.Collections;
using System.Collections.Generic;

namespace Traitorstown.src.http.request
{
    [Serializable]
    public class RegistrationRequest
    {
        public String password;
        public String email;

        public RegistrationRequest(string password, string email)
        {
            this.password = password;
            this.email = email;
        }
    }
}

