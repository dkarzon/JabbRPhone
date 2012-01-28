using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using JabbrPhone.Models;
using JabbrPhone.ViewModels;
using JabbrPhone.Extensions;
using JabbrPhone.Events;

namespace JabbrPhone.Pages
{
    public partial class RoomPage : PhoneApplicationPage
    {
        RoomViewModel _model;

        public RoomPage()
        {
            InitializeComponent();

            ((App)App.Current).EventManager.MessageAdded += NewMessageAdded;
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            //Stop listening for the event (Sometimes caused duplicate messages)
            ((App)App.Current).EventManager.MessageAdded -= NewMessageAdded;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var roomName = NavigationContext.QueryString["name"];

            _model = new RoomViewModel(roomName);
            _model.Prog = this.GetProgressIndicator();
            this.DataContext = _model;

            _model.SetStatus("Loading room...", true);
            txtMessage.IsEnabled = false;

            App.ChatHub.Invoke<RoomModel>("GetRoomInfo", roomName)
                .ContinueWith((roomTask) =>
                    {
                        _model.LoadRoom(roomTask.Result);
                        _model.SetStatus("Joining room...", true);
                        JoinRoom();
                        ScrollToLastMessage();
                    });
        }

        private void JoinRoom()
        {
            //Set this to the active room
            App.ChatHub["activeRoom"] = _model.Name;

            App.ChatHub.Invoke("Send", string.Format("/join {0}", _model.Name))
                .ContinueWith((task) =>
                    {
                        //done...?
                        Dispatcher.BeginInvoke(() =>
                            {
                                txtMessage.IsEnabled = true;
                            });
                        _model.ClearStatus();
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

        private void ScrollToLastMessage()
        {
            Dispatcher.BeginInvoke(() =>
                {
                    try
                    {
                        lsbMessages.ScrollIntoView(lsbMessages.Items[lsbMessages.Items.Count - 1]);
                        lsbMessages.UpdateLayout();
                        lsbMessages.ScrollIntoView(lsbMessages.Items[lsbMessages.Items.Count - 1]);
                    }
                    catch (Exception ex) { }
                });
        }


        private void NewMessageAdded(object sender, MessageAddedEventArgs e)
        {
            if (e.RoomName.Equals(_model.Name, StringComparison.OrdinalIgnoreCase))
            {
                Dispatcher.BeginInvoke(() => _model.Messages.Add(e.Message));
                ScrollToLastMessage();
            }
        }
    }
}