using System;
using System.Collections;
using System.Collections.Generic;

namespace Traitorstown.src.http.request
{
    [Serializable]
    public class MessageRequest
    {
        public List<int> recipients;
        public string content;

        public MessageRequest(List<int> recipients, string content)
        {
            this.recipients = recipients;
            this.content = content;
        }
    }
}

