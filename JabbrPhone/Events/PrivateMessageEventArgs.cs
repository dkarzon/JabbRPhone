using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JabbrPhone.Models;

namespace JabbrPhone.Events
{
    public class PrivateMessageEventArgs : EventArgs
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Message { get; set; }
    }
}
