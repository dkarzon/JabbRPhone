using System.Collections.ObjectModel;
using System.Linq;
using JabbRPhone.Extensions;

namespace JabbRPhone.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get
            {
                return _isLoggedIn;
            }
            set
            {
                _isLoggedIn = value;
                NotifyPropertyChanged("IsLoggedIn");
            }
        }
        private bool _showRooms;
        public bool ShowRooms
        {
            get
            {
                return _showRooms;
            }
            set
            {
                _showRooms = value;
                NotifyPropertyChanged("ShowRooms");
            }
        }
        private bool _showLogin;
        public bool ShowLogin
        {
            get
            {
                return _showLogin;
            }
            set
            {
                _showLogin = value;
                NotifyPropertyChanged("ShowLogin");
            }
        }
        private bool _showCreateRoom;
        public bool ShowCreateRoom
        {
            get
            {
                return _showCreateRoom;
            }
            set
            {
                _showCreateRoom = value;
                NotifyPropertyChanged("ShowCreateRoom");
            }
        }

        private string _createroomname;
        public string CreateRoomName
        {
            get { return _createroomname; }
            set
            {
                _createroomname = value;
                NotifyPropertyChanged("CreateRoomName");
            }
        }

        private string _createroommessage;
        public string CreateRoomMessage
        {
            get { return _createroommessage; }
            set
            {
                _createroommessage = value;
                NotifyPropertyChanged("CreateRoomMessage");
            }
        }

        private string _privateRoomName;
        public string PrivateRoomName
        {
            get { return _privateRoomName; }
            set
            {
                _privateRoomName = value;
                NotifyPropertyChanged("PrivateRoomName");
            }
        }

        private string _privateRoomInviteCode;
        public string PrivateRoomInviteCode
        {
            get { return _privateRoomInviteCode; }
            set
            {
                _privateRoomInviteCode = value;
                NotifyPropertyChanged("PrivateRoomInviteCode");
            }
        }

        private bool _showJoinPrivateRoom;
        public bool ShowJoinPrivateRoom
        {
            get { return _showJoinPrivateRoom; }
            set
            {
                _showJoinPrivateRoom = value;
                NotifyPropertyChanged("ShowJoinPrivateRoom");
            }
        }

        private string _joinPrivateRoomMessage;
        public string JoinPrivateRoomMessage
        {
            get { return _joinPrivateRoomMessage; }
            set
            {
                _joinPrivateRoomMessage = value;
                NotifyPropertyChanged("JoinPrivateRoomMessage");
            }
        }


        private string _loginusername;
        public string LoginUsername
        {
            get { return _loginusername; }
            set
            {
                _loginusername = value;
                NotifyPropertyChanged("LoginUsername");
            }
        }
        private string _loginpassword;
        public string LoginPassword
        {
            get { return _loginpassword; }
            set
            {
                _loginpassword = value;
                NotifyPropertyChanged("LoginPassword");
            }
        }
        private string _loginmessage;
        public string LoginMessage
        {
            get { return _loginmessage; }
            set
            {
                _loginmessage = value;
                NotifyPropertyChanged("LoginMessage");
            }
        }

        private bool _showSearch;
        public bool ShowSearch
        {
            get { return _showSearch; }
            set
            {
                _showSearch = value;
                NotifyPropertyChanged("ShowSearch");
            }
        }

        private string _search;
        public string Search
        {
            get { return _search; }
            set
            {
                _search = value;
                NotifyPropertyChanged("Search");
                NotifyPropertyChanged("Rooms");
            }
        }

        private ObservableCollection<JabbR.Client.Models.Room> _allRooms;
        public ObservableCollection<JabbR.Client.Models.Room> Rooms
        {
            get
            {
                if (_allRooms == null) return null;

                if (string.IsNullOrEmpty(_search))
                {
                    return _allRooms.OrderByDescending(r => r.Count).ToObservableCollection();
                }

                return _allRooms.Where(r => r.Name.ToLower().Contains(_search.ToLower())).OrderByDescending(r => r.Count).ToObservableCollection();
            }
            set
            {
                _allRooms = value;
                NotifyPropertyChanged("Rooms");
            }
        }

    }
}
