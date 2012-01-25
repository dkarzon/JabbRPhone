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
using System.IO.IsolatedStorage;

namespace JabbrPhone.Storage
{
    public class SettingsStorage
    {
        public static string GetSetting(string key)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
            {
                return IsolatedStorageSettings.ApplicationSettings[key].ToString();
            }
            else
            {
                return "";
            }
        }

        public static void SaveSetting(string key, string value)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
            {
                IsolatedStorageSettings.ApplicationSettings[key] = value;
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings.Add(key, value);
            }
        }
    }
}