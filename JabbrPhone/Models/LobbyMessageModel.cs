using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JabbrPhone.Models
{
    public class LobbyMessageModel
    {
        public string Content { get; set; }
        public DateTimeOffset When { get; set; }
    }
}
