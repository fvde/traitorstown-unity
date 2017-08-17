using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Project.src.model
{
    public class Message
    {
        public long From { get; }
        public string Content { get; }

        public Message(long from, string content)
        {
            From = from;
            Content = content;
        }
    }
}
