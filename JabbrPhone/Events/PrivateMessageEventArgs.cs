using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JabbRPhone.Models;

namespace JabbRPhone.Events
{
    public class PrivateMessageEventArgs : EventArgs
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Message { get; set; }
    }
}
