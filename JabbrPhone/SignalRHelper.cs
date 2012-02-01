using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JabbrPhone.Models;
using JabbrPhone.Events;

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
                //For some reason When comes through localized here.
                message.When = message.When.ToUniversalTime();

                MessageAdded(App.ChatHub, new MessageAddedEventArgs
                {
                    Message = message,
                    RoomName = roomName
                });
            }
        }


        public delegate void PrivateMessageEventHandler(object sender, PrivateMessageEventArgs e);
        public event PrivateMessageEventHandler PrivateMessage;
        public void OnPrivateMessage(string from, string to, string message)
        {
            if (PrivateMessage != null)
            {
                PrivateMessage(App.ChatHub, new PrivateMessageEventArgs
                {
                    From = from,
                    To = to,
                    Message = message
                });
            }
        }
    }
}
