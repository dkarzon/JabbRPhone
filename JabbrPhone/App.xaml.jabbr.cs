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
using System.Threading.Tasks;
using JabbRPhone.Helpers;
using JabbR.Client;

namespace JabbRPhone
{
    public partial class App
    {
        public static string Username
        {
            get
            {
                return "TEST";
            }
        }

        public static JabbRClient Client { get; set; }

        /// <summary>
        /// This method initialises the JabbR.Client connection to the server and Starts it
        /// </summary>
        /// <param name="connectContinue"></param>
        internal static void InitializeJabbr()
        {
            App.Client = new JabbRClient(SettingsHelper.GetServerUrl());
        }

        internal static void ConnectJabbr(Action<Task> connectContinue)
        {
            var id = Storage.SettingsStorage.GetSetting<string>("id");

            if (!string.IsNullOrEmpty(id))
            {
                App.Client.Connect(id)
                    .ContinueWith(connectContinue);
            }
        }
    }
}
