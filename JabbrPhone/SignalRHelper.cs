using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JabbrPhone.Models;

namespace JabbrPhone
{
    public class SignalRHelper
    {
        public delegate void MessageAddedEventHandler(object sender, MessageAddedEventArgs e);
        public event MessageAddedEventHandler MessageAdded;

        public void OnNewMessage(MessageModel message, string roomName)
        {
            if (MessageAdded != null)
            {
                MessageAdded(App.ChatHub, new MessageAddedEventArgs
                {
                    Message = message,
                    RoomName = roomName
                });
            }
        }

    }
}
