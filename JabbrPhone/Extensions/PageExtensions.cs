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
using Microsoft.Phone.Shell;
using Microsoft.Phone.Controls;

namespace JabbrPhone.Extensions
{
    public static class PageExtensions
    {
        public static ProgressIndicator GetProgressIndicator(this PhoneApplicationPage page)
        {
            var progressIndicator = SystemTray.ProgressIndicator;
            if (progressIndicator == null)
            {
                progressIndicator = new ProgressIndicator();
                SystemTray.SetProgressIndicator(page, progressIndicator);
            }
            return progressIndicator;
        }
    }
}
