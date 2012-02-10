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
using System.Windows.Navigation;

namespace JabbRPhone.Extensions
{
    public static class NavigationServiceExtensions
    {
        public static void SafeGoBack(this NavigationService service)
        {
            if (service.CanGoBack)
            {
                try
                {
                    service.GoBack();
                }
                catch (Exception) { }
            }
        }
    }
}