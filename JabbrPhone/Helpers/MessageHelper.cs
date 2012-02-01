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
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JabbrPhone.Helpers
{
    public static class MessageHelper
    {

        public static List<object> TokenizeMessage(string message)
        {
            var tokens = new List<object>();

            try
            {
                //TODO - better Regex
                var r = new Regex("<(.|\n)*?>", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.ExplicitCapture);
                var split = r.Split(message);

                foreach (var t in split)
                {
                    if (!string.IsNullOrWhiteSpace(t))
                    {
                        Uri uri = null;
                        if (Uri.TryCreate(t, UriKind.Absolute, out uri))
                        {
                            tokens.Add(uri);
                        }
                        else
                        {
                            tokens.Add(t);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                tokens.Clear();
                tokens.Add(message);
            }

            return tokens;
        }

    }
}
