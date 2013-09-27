using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mariola
{
    class MessagesFromModule
    {
        public string address = "";
        public List<RsMessage> messages = new List<RsMessage>();
        public DateTime lastPackage;
        public int countSend = 0;
        public int countReceive = 0;
        public string rssi="";
        public string hop = "";

        public void AddSend(RsMessage _message)
        {
            messages.Add(_message);
            countSend++;
        }
        public void AddReceive(RsMessage _message)
        {
            messages.Add(_message);
            countReceive++;
            lastPackage = DateTime.Now;
        }
    }
}
