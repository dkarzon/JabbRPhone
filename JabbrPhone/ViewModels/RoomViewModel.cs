using System.Collections.Generic;
using JabbrPhone.Models;
using System.Collections.ObjectModel;
using JabbrPhone.Extensions;

namespace JabbrPhone.ViewModels
{
    public class RoomViewModel : BaseViewModel
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value.ToLower();
                NotifyPropertyChanged("Name");
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

        public RoomViewModel(string name)
        {
            Name = name;
        }

        internal void LoadRoom(RoomModel room)
        {
            Name = room.Name;

            Messages = room.RecentMessages.ToObservableCollection();
            Users = room.Users.ToObservableCollection();
        }
    }
}
