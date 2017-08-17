using Assets.Project.src.model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Traitorstown.src.http.representation
{
    [Serializable]
    public class MessageRepresentation
    {
        public long from;
        public string content;

        public MessageRepresentation(string content)
        {
            this.content = content;
        }

        public Message toMessage()
        {
            return new Message(from, content);
        }
    }
}

