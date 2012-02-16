using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace JabbRPhone.ViewModels
{
    public class MessageViewModel
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTimeOffset When { get; set; }
        public JabbR.Client.Models.User User { get; set; }

        public bool IsMentioned
        {
            get
            {
                if (Content == null) return false;

                if (App.Username != null)
                {
                    return Content.Contains("@" + App.Username);
                }
                return false;
            }
        }

        public List<object> MessageTokens
        {
            get { return Helpers.MessageHelper.TokenizeMessage(Content); }
        }

        public MessageViewModel(JabbR.Client.Models.Message message)
        {
            Id = message.Id;
            Content = message.Content;
            When = message.When;
            User = message.User;
        }
    }
}
