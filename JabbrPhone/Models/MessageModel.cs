using System;
using System.Collections.Generic;

namespace JabbrPhone.Models
{
    public class MessageModel
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTimeOffset When { get; set; }
        public UserModel User { get; set; }

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
    }
}
