using System;
using System.ComponentModel;
using JabbrPhone.Helpers;
using Microsoft.Phone.Shell;
using System.Threading;

namespace JabbrPhone.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public ProgressIndicator Prog;

        public void SetStatus(string message, bool isProgress, int? clearAfterMilliSeconds = null)
        {
            if (Prog == null) return;

            DispatcherHelper.SafeDispatch(() =>
            {
                Prog.IsIndeterminate = isProgress;
                Prog.IsVisible = true;
                Prog.Text = message;
            });

            if (clearAfterMilliSeconds.HasValue)
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    Thread.Sleep(clearAfterMilliSeconds.Value);
                    ClearStatus();
                });
            }
        }

        public void ClearStatus()
        {
            if (Prog == null) return;

            DispatcherHelper.SafeDispatch(() =>
            {
                Prog.IsIndeterminate = false;
                Prog.IsVisible = false;
                Prog.Text = string.Empty;
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                DispatcherHelper.SafeDispatch(() =>
                  PropertyChanged(this, new PropertyChangedEventArgs(propertyName)));
            }
        }
    }
}
