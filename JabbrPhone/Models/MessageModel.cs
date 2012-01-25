using System;

namespace JabbrPhone.Models
{
    public class MessageModel
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTimeOffset When { get; set; }
        public UserModel User { get; set; }
    }
}
