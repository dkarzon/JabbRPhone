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
using JabbR.Client.Models;

namespace JabbRPhone.ViewModels
{
    public class RoomUserViewModel
    {
        public string Name { get; set; }
        public string Hash { get; set; }
        public bool Active { get; set; }
        public UserStatus Status { get; set; }
        public string Note { get; set; }
        public string AfkNote { get; set; }
        public bool IsAfk { get; set; }
        public string Flag { get; set; }
        public string Country { get; set; }
        public DateTime LastActivity { get; set; }
        public bool IsOwner { get; set; }

        public Brush Brush
        {
            get
            {
                if (Active)
                {
                    return (SolidColorBrush)System.Windows.Application.Current.Resources["PhoneForegroundBrush"];
                }
                else
                {
                    return (SolidColorBrush)System.Windows.Application.Current.Resources["PhoneDisabledBrush"];
                }
            }
        }

        public RoomUserViewModel(JabbR.Client.Models.User user, bool isOwner)
        {
            Name = user.Name;
            Hash = user.Hash;
            Active = user.Active;
            Status = user.Status;
            Note = user.Note;
            AfkNote = user.AfkNote;
            IsAfk = user.IsAfk;
            Flag = user.Flag;
            Country = user.Country;
            LastActivity = LastActivity;
            IsOwner = isOwner;
        }
    }
}
