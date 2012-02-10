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

namespace JabbRPhone.Helpers
{
    public static class SettingsHelper
    {
        public static string GetServerUrl()
        {
            var serverurl = Storage.SettingsStorage.GetSetting<string>("serverurl");
            if (string.IsNullOrEmpty(serverurl))
            {
                //default
                return "http://jabbr.net/";
            }
            return serverurl;
        }

        public static void SaveServerUrl(string url)
        {
            Storage.SettingsStorage.SaveSetting("serverurl", url);
        }
    }
}
