using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mariola
{
    class RsMessage
    {
        public string type;
        public string message;
        public string addr = "";
        public RsMessage(string _type, string _message)
        {
            type = _type;
            message = _message;

            if (_message.IndexOf("Addr") >= 0)
            {
                switch (_type)
                {
                    case ("SendUDP"):
                        {
                            if (_message.Length > "Addr:".Length + 39)
                            {
                                addr = _message.Substring(_message.IndexOf("Addr:") + "Addr:".Length, 39);
                            }
                            break;
                        }
                    case ("SendUDP2"):
                        {
                            if (_message.Length > "Addr:".Length + 39)
                            {
                                addr = _message.Substring(_message.IndexOf("Addr:") + "Addr:".Length, 39);
                            }
                            break;
                        }
                    case ("ReceiveUDP"):
                        {
                            if (_message.Length > "Addr:".Length + 39)
                            {
                                addr = _message.Substring(_message.IndexOf("Addr:") + "Addr:".Length, 39);
                            }
                            break;
                        }
                    case ("Connection Status"):
                        {
                            if (_message.Length > "Addr = ".Length + 39)
                            {
                                addr = _message.Substring(_message.IndexOf("Addr = ") + "Addr = ".Length, 39);
                            }
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }
    }
}
