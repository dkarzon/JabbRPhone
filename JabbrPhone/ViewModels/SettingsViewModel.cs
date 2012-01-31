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
using JabbrPhone.Helpers;

namespace JabbrPhone.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private string _serverUrl;
        public string ServerUrl
        {
            get { return _serverUrl; }
            set
            {
                _serverUrl = value;
                NotifyPropertyChanged("ServerUrl");
            }
        }

        private string _userFlag;
        public string UserFlag
        {
            get { return _userFlag; }
            set
            {
                _userFlag = value;
                NotifyPropertyChanged("UserFlag");
            }
        }

        private string _userGravEmail;
        public string UserGravEmail
        {
            get { return _userGravEmail; }
            set
            {
                _userGravEmail = value;
                NotifyPropertyChanged("UserGravEmail");
            }
        }

        private string _userNote;
        public string UserNote
        {
            get { return _userNote; }
            set
            {
                _userNote = value;
                NotifyPropertyChanged("UserNote");
            }
        }

        public SettingsViewModel()
        {
            ServerUrl = SettingsHelper.GetServerUrl();
        }

    }
}
