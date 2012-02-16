using System.Collections.Generic;
using System.Collections.ObjectModel;
using JabbRPhone.Extensions;
using System.Linq;

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

        private ObservableCollection<MessageViewModel> _messages;
        public ObservableCollection<MessageViewModel> Messages
        {
            get { return _messages; }
            set
            {
                _messages = value;
                NotifyPropertyChanged("Messages");
            }
        }

        private ObservableCollection<RoomUserViewModel> _users;
        public ObservableCollection<RoomUserViewModel> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                NotifyPropertyChanged("Users");
            }
        }

        internal void LoadRoom(JabbR.Client.Models.Room room)
        {
            Name = room.Name;

            Messages = room.RecentMessages.Select(m => new MessageViewModel(m)).ToObservableCollection();
            Users = room.Users.Select(u => new RoomUserViewModel(u, room.Owners.Contains(u.Name))).OrderBy(u => u.IsOwner).ToObservableCollection();
        }
    }
}
