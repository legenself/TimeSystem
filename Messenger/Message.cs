using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger
{
    public class Message
    {
        public string rule;
        public string describe;
        public Message(string msg) {
           var _temp = msg.Split(';');
            rule = _temp[0];
            describe = _temp[1];

        }
        public static List<Message> Convert(string[] content) {
            return content.Select(p => new Message(p)).ToList();
        }
    }
}
