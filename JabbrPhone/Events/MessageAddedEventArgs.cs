using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JabbrPhone.Models;

namespace JabbrPhone.Events
{
    public class MessageAddedEventArgs : EventArgs
    {
        public MessageModel Message { get; set; }
        public string RoomName { get; set; }
    }
}
