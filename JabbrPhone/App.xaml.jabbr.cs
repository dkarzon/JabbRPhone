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
using SignalR.Client.Hubs;
using JabbRPhone.Models;
using System.Threading.Tasks;
using SignalR.Client.Transports;
using JabbRPhone.Helpers;

namespace JabbRPhone
{
    public partial class App
    {
        public static string Username
        {
            get
            {
                if (ChatHub == null) return null;
                var name = ChatHub["name"] as string;
                if (name == null) return null;
                return name;
            }
        }

        public static HubConnection JabbrConnection { get; set; }
        public static IHubProxy ChatHub { get; set; }

        /// <summary>
        /// A helper class that managers server events across the app
        /// </summary>
        public SignalRHelper EventManager { get; set; }

        /// <summary>
        /// This method should be run before the signalR connection is started
        /// to subscribe to the events
        /// </summary>
        internal static void SetupEvents()
        {
            App.ChatHub.On<MessageModel, string>("AddMessage",
                (message, roomName) =>
                {
                    ((App)App.Current).EventManager.OnNewMessage(message, roomName);
                });

            App.ChatHub.On<string, string, string>("sendPrivateMessage",
                (from, to, message) =>
                {
                    ((App)App.Current).EventManager.OnPrivateMessage(from, to, message);
                });

            //Other events we need to watch?
        }

        /// <summary>
        /// This method initialises the signalR connection to the server and Starts it
        /// </summary>
        /// <param name="startContinue"></param>
        internal static void InitializeJabbr(Action<Task> startContinue)
        {
            App.JabbrConnection = new HubConnection(SettingsHelper.GetServerUrl());
            App.ChatHub = App.JabbrConnection.CreateProxy("JabbR.Chat");

            var id = Storage.SettingsStorage.GetSetting("id");

            if (!string.IsNullOrEmpty(id))
            {
                App.ChatHub["id"] = id;
            }

            //Setup the signalR events!
            SetupEvents();

            //Server sent events dont work at the moment...
            App.JabbrConnection.Start(Transport.LongPolling)
                .ContinueWith(startContinue);
        }

    }
}
