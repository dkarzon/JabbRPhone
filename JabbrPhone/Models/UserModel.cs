using System;
using System.Windows.Media;

namespace JabbrPhone.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Hash { get; set; }
        public bool Active { get; set; }
        public bool IsAfk { get; set; }
        public string Flag { get; set; }
        public string Country { get; set; }

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
    }
}
