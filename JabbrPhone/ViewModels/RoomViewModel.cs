using System.Collections.Generic;
using JabbRPhone.Models;
using System.Collections.ObjectModel;
using JabbRPhone.Extensions;

namespace JabbRPhone.ViewModels
{
    public class RoomViewModel : BaseViewModel
    {
        public RoomViewModel(string name)
            : this(name, null)
        { }

        public RoomViewModel(string name, string inviteCode)
        {
            Name = name;
            InviteCode = inviteCode;
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        private string _inviteCode;
        public string InviteCode
        {
            get { return _inviteCode; }
            set
            {
                _inviteCode = value;
                NotifyPropertyChanged("InviteCode");
            }
        }

        private ObservableCollection<MessageModel> _messages;
        public ObservableCollection<MessageModel> Messages
        {
            get { return _messages; }
            set
            {
                _messages = value;
                NotifyPropertyChanged("Messages");
            }
        }

        private ObservableCollection<UserModel> _users;
        public ObservableCollection<UserModel> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                NotifyPropertyChanged("Users");
            }
        }

        internal void LoadRoom(RoomModel room)
        {
            Name = room.Name;

            Messages = room.RecentMessages.ToObservableCollection();
            Users = room.Users.ToObservableCollection();
        }
    }
}
