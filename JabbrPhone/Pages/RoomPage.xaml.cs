﻿using System;
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
using JabbRPhone.Models;
using JabbRPhone.ViewModels;
using JabbRPhone.Extensions;
using JabbRPhone.Events;
using JabbRPhone.Helpers;

namespace JabbRPhone.Pages
{
    public partial class RoomPage : PhoneApplicationPage
    {
        RoomViewModel _model;

        public RoomPage()
        {
            InitializeComponent();
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

            ((App)App.Current).EventManager.MessageAdded += NewMessageAdded;

            var roomName = NavigationContext.QueryString["name"];
            string inviteCode = null;
            NavigationContext.QueryString.TryGetValue("inviteCode", out inviteCode);

            _model = new RoomViewModel(roomName, inviteCode);
            _model.Prog = this.GetProgressIndicator();
            this.DataContext = _model;

            _model.SetStatus("Loading room...", true);
            txtMessage.IsEnabled = false;

            App.ChatHub.Invoke<RoomModel>("GetRoomInfo", roomName)
                .ContinueWith((roomTask) =>
                {
                    JoinRoom();
                    roomTask.Result.CheckOwners();
                    _model.LoadRoom(roomTask.Result);
                    _model.SetStatus("Joining room...", true);
                    ScrollToLastMessage();
                });
        }

        private void JoinRoom()
        {
            //Set this to the active room
            App.ChatHub["activeRoom"] = _model.Name;

            App.ChatHub.Invoke("Send", string.Format("/join {0} {1}", _model.Name, _model.InviteCode))
                .ContinueWith((task) =>
                {
                    //done...?
                    if (task.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                    {
                        Dispatcher.BeginInvoke(() =>
                            {
                                txtMessage.IsEnabled = true;
                            });
                        _model.ClearStatus();
                    }
                    else if (task.Status == System.Threading.Tasks.TaskStatus.Faulted)
                    {
                        // Couldn't join room. What up?
                        DispatcherHelper.SafeDispatch(() => 
                            {
                                MessageBox.Show("Couldn't join the room.\n\nAre you sure the details are correct?", "Oops!", MessageBoxButton.OK);
                                NavigationService.SafeGoBack();
                            });
                    }
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
            if (string.IsNullOrWhiteSpace(content)) return;

            //Clear it
            txtMessage.Text = string.Empty;

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
                            Dispatcher.BeginInvoke(() =>
                            {
                                txtMessage.Text = content;
                            });
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
            if (_model == null) return;

            if (e.RoomName.Equals(_model.Name, StringComparison.OrdinalIgnoreCase))
            {
                Dispatcher.BeginInvoke(() => _model.Messages.Add(e.Message));
                ScrollToLastMessage();
            }
        }
    }
}