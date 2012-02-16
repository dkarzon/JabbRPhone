using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coding4Fun.Phone.Controls;
using JabbRPhone.Extensions;
using JabbRPhone.ViewModels;
using Microsoft.Phone.Controls;
using SignalR.Client.Hubs;
using SignalR.Client.Transports;
using System.Windows.Input;

namespace JabbRPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        HomeViewModel _model;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            _model = new HomeViewModel();
            _model.ShowCreateRoom = false;
            this.DataContext = _model;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Join();

            if (_model != null && _model.Prog == null)
            {
                _model.Prog = this.GetProgressIndicator();
            }

            //upon return deselect the room we were just in
            lsbRooms.SelectedIndex = -1;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (_model.ShowCreateRoom)
            {
                _model.ShowCreateRoom = false;
                e.Cancel = true;
            }
            else
            {
                base.OnBackKeyPress(e);
            }
        }

        private void Join()
        {
            var id = Storage.SettingsStorage.GetSetting<string>("id");

            if (!string.IsNullOrEmpty(id))
            {
                App.Client.Connect(id)
                    .ContinueWith(task => JoinContinue(task));
            }
            else
            {
                _model.ShowLogin = true;
                Dispatcher.BeginInvoke(() =>
                {
                    ApplicationBar.IsVisible = false;
                });
                _model.ClearStatus();
            }
        }

        private void JoinContinue(Task<JabbR.Client.Models.LogOnInfo> task)
        {
            if (task.IsFaulted)
            {
                _model.ShowLogin = true;
                Dispatcher.BeginInvoke(() =>
                {
                    ApplicationBar.IsVisible = false;
                });
                _model.ClearStatus();
            }
            else
            {
                //GREAT SUCCESS!
                _model.IsLoggedIn = true;
                _model.ShowLogin = false;
                _model.ShowRooms = true;
                //Load rooms
                LoadRooms();

                Dispatcher.BeginInvoke(() =>
                {
                    ApplicationBar.IsVisible = true;
                });
            }
        }

        private void LoadRooms()
        {
            _model.SetStatus("Loading rooms...", true);
            App.Client.GetRooms()
                .ContinueWith(task =>
                    {
                        _model.Rooms = task.Result.ToObservableCollection();
                        _model.ClearStatus();
                    });
        }

        private void btn_Login_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_model.LoginPassword) || string.IsNullOrEmpty(_model.LoginUsername))
            {
                _model.LoginMessage = "Please enter a username and password.";
                return;
            }

            _model.SetStatus("Logging in...", true);

            App.Client.Connect(_model.LoginUsername, _model.LoginPassword)
                .ContinueWith(task =>
                    {
                        if (App.Client.UserId != null)
                        {
                            _model.IsLoggedIn = true;
                            _model.ShowLogin = false;
                            _model.ShowRooms = true;
                            Storage.SettingsStorage.SaveSetting("id", App.Client.UserId);
                            //Now try and load rooms
                            LoadRooms();
                            Dispatcher.BeginInvoke(() =>
                                {
                                    ApplicationBar.IsVisible = true;
                                });
                        }
                        else
                        {
                            _model.LoginMessage = "Login failed...";
                            _model.ClearStatus();
                        }
                    });
        }

        private void lsbRooms_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0) return;

            var room = e.AddedItems[0] as JabbR.Client.Models.Room;

            if (room == null) return;

            NavigationService.Navigate(new Uri(string.Format("/Pages/RoomPage.xaml?name={0}", room.Name), UriKind.RelativeOrAbsolute));
        }

        private void btnRefresh_Click(object sender, System.EventArgs e)
        {
        	LoadRooms();
        }

        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            _model.ShowSearch = !_model.ShowSearch;
            if (!_model.ShowSearch)
            {
                _model.Search = string.Empty;
            }
            else
            {
                txtSearch.Focus();
            }
        }

        private void menuAbout_Click(object sender, System.EventArgs e)
        {
        	// TODO - About...?
        }

        private void menuLogout_Click(object sender, System.EventArgs e)
        {
            _model.SetStatus("Logging out...", true);

            App.Client.LogOut()
                .ContinueWith(task =>
                    {
                        Storage.SettingsStorage.SaveSetting<string>("id", null);

                        _model.IsLoggedIn = false;
                        _model.ShowLogin = true;
                        _model.ShowRooms = false;

                        _model.ClearStatus();
                    });
        }

        private void btnCreateRoom_Click(object sender, System.EventArgs e)
        {
            _model.ShowCreateRoom = true;
        }

        private void btn_Create_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var newroom = _model.CreateRoomName;
            if (string.IsNullOrEmpty(newroom))
            {
                _model.CreateRoomMessage = "Please enter a name to create a room.";
                return;
            }

            _model.SetStatus("Creating room...", true);

            App.Client.CreateRoom(newroom)
                .ContinueWith((task) =>
                {
                    //done...?
                    _model.ClearStatus();
                    _model.ShowCreateRoom = false;
                    _model.CreateRoomName = string.Empty;

                    NavigationService.Navigate(new Uri(string.Format("/Pages/RoomPage.xaml?name={0}", newroom), UriKind.RelativeOrAbsolute));
                });
        }

        private void btnSettings_Click(object sender, System.EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/SettingsPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btnJoinPrivateRoom_Click(object sender, EventArgs e)
        {
            _model.ShowJoinPrivateRoom = true;
        }

        private void btn_JoinPrivateRoom_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_model.PrivateRoomName))
            {
                _model.JoinPrivateRoomMessage = "Please enter a room name.";
                return;
            }

            if (string.IsNullOrEmpty(_model.PrivateRoomInviteCode))
            {
                _model.JoinPrivateRoomMessage = "Please enter an invite code.";
                return;
            }

            _model.ShowJoinPrivateRoom = false;

            NavigationService.Navigate(new Uri(string.Format("/Pages/RoomPage.xaml?name={0}&inviteCode={1}", _model.PrivateRoomName, _model.PrivateRoomInviteCode), UriKind.RelativeOrAbsolute));
        }
    }
}