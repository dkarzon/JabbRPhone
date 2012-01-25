using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coding4Fun.Phone.Controls;
using JabbrPhone.Extensions;
using JabbrPhone.Models;
using JabbrPhone.ViewModels;
using Microsoft.Phone.Controls;
using SignalR.Client.Hubs;
using SignalR.Client.Transports;
using System.Windows.Input;

namespace JabbrPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        HomeViewModel _model;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _model = new HomeViewModel();
            _model.Prog = this.GetProgressIndicator();
            _model.ShowCreateRoom = false;
            this.DataContext = _model;

            App.JabbrConnection = new HubConnection("http://jabbr.net/");
            App.ChatHub = App.JabbrConnection.CreateProxy("JabbR.Chat");

            var id = Storage.SettingsStorage.GetSetting("id");

            if (!string.IsNullOrEmpty(id))
            {
                App.ChatHub["id"] = id;
            }
            
            _model.SetStatus("Connecting to Server...", true);

            //Setup the signalR events!
            App.ChatHub.On<MessageModel, string>("AddMessage",
                (message, roomName) =>
                {
                    ((App)App.Current).EventManager.OnNewMessage(message, roomName);
                });

            //Server sent events dont work at the moment...
            App.JabbrConnection.Start(Transport.LongPolling).ContinueWith(startTask =>
                {
                    App.ChatHub.Invoke<bool>("Join")
                        .ContinueWith(task =>
                            {
                                Join(task);
                            });
                });
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

        private void Join(Task<bool> task)
        {
            if (task.Result)
            {
                //GREAT SUCCESS!
                _model.IsLoggedIn = true;
                _model.ShowLogin = false;
                _model.ShowRooms = true;
                //Load rooms
                LoadRooms();
            }
            else
            {
                _model.ShowLogin = true;
                _model.ClearStatus();
            }
        }

        private void LoadRooms()
        {
            _model.SetStatus("Loading rooms...", true);
            App.ChatHub.Invoke<List<RoomModel>>("GetRooms")
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

            App.ChatHub.Invoke("Send", string.Format("/nick {0} {1}", _model.LoginUsername, _model.LoginPassword))
                .ContinueWith(task =>
                    {
                        if (App.ChatHub["id"] != null)
                        {
                            _model.IsLoggedIn = true;
                            _model.ShowLogin = false;
                            _model.ShowRooms = true;
                            Storage.SettingsStorage.SaveSetting("id", App.ChatHub["id"].ToString());
                            //Now try and load rooms
                            LoadRooms();
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

            var room = e.AddedItems[0] as RoomModel;

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
                if (pvtMain.SelectedIndex != 1)
                {
                    pvtMain.SelectedIndex = 1;
                }
                txtSearch.Focus();
            }
        }

        private void menuAbout_Click(object sender, System.EventArgs e)
        {
        	// TODO: Add event handler implementation here.
        }

        private void menuLogout_Click(object sender, System.EventArgs e)
        {
            Storage.SettingsStorage.SaveSetting("id", null);

            _model.IsLoggedIn = false;
            _model.ShowLogin = true;
            _model.ShowRooms = false;
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
            App.ChatHub.Invoke("Send", string.Format("/create {0}", newroom))
                .ContinueWith((task) =>
                {
                    //done...?
                    _model.ClearStatus();
                    _model.ShowCreateRoom = false;
                    _model.CreateRoomName = string.Empty;

                    NavigationService.Navigate(new Uri(string.Format("/Pages/RoomPage.xaml?name={0}", newroom), UriKind.RelativeOrAbsolute));
                });
        }

        private void txtMessage_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage(txtMessage.Text);
            }
        }

        private void SendMessage(string content)
        {
            _model.SetStatus("Sending...", true);

            App.ChatHub.Invoke("Send", content)
                .ContinueWith((task) =>
                {
                    //Add this 1 to the list?
                    if (task.IsFaulted)
                    {
                        try
                        {
                            _model.SetStatus(task.Exception.Message, false, 2000);
                        }
                        catch { }
                    }
                    else
                    {
                        _model.SetStatus("Message sent!", false, 2000);
                        Dispatcher.BeginInvoke(() =>
                        {
                            txtMessage.Text = string.Empty;
                        });
                    }
                });
        }

    }

}