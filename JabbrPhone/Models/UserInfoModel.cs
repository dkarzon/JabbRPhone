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

namespace JabbRPhone.Models
{
    /// <summary>
    /// Used for GetUserInfo responses (/who [username])
    /// </summary>
    public class UserInfoModel
    {
        public string Name { get; set; }
        public List<string> OwnedRooms { get; set; }
        public string Status { get; set; }
        public DateTime LastActivity { get; set; }
        public List<string> Rooms { get; set; }
    }
}
