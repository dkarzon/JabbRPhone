using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JabbRPhone.Models;

namespace JabbRPhone.Events
{
    public class MessageAddedEventArgs : EventArgs
    {
        public MessageModel Message { get; set; }
        public string RoomName { get; set; }
    }
}
