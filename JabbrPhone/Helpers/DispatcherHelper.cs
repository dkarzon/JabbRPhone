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
using System.Windows.Threading;

namespace JabbRPhone.Helpers
{
    /// <summary>
    /// Helper class for dispatcher operations on the UI thread.
    /// </summary>
    //// [ClassInfo(typeof(DispatcherHelper),
    ////  VersionString = "1.0.0.0",
    ////  DateString = "201003041420",
    ////  Description = "Helper class for dispatcher operations on the UI thread.",
    ////  UrlContacts = "http://www.galasoft.ch/contact_en.html",
    ////  Email = "laurent@galasoft.ch")]
    public static class DispatcherHelper
    {
        /// <summary>
        /// Gets a reference to the UI thread's dispatcher, after the
        /// <see cref="Initialize" /> method has been called on the UI thread.
        /// </summary>
        public static Dispatcher UIDispatcher
        {
            get;
            private set;
        }

        /// <summary>
        /// Executes an action on the UI thread. If this method is called
        /// from the UI thread, the action is executed immediately. If the
        /// method is called from another thread, the action will be enqueued
        /// on the UI thread's dispatcher and executed asynchronously.
        /// <para>For additional operations on the UI thread, you can get a
        /// reference to the UI thread's dispatcher thanks to the property
        /// <see cref="UIDispatcher" /></para>.
        /// </summary>
        /// <param name="action">The action that will be executed on the UI
        /// thread.</param>
        public static void SafeDispatch(Action action)
        {
            if (UIDispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                UIDispatcher.BeginInvoke(action);
            }
        }

        /// <summary>
        /// This method should be called once on the UI thread to ensure that
        /// the <see cref="UIDispatcher" /> property is initialized.
        /// <para>In a Silverlight application, call this method in the
        /// Application_Startup event handler, after the MainPage is constructed.</para>
        /// <para>In WPF, call this method on the static App() constructor.</para>
        /// </summary>
        public static void Initialize()
        {
            if (UIDispatcher != null)
            {
                return;
            }

            UIDispatcher = Deployment.Current.Dispatcher;
        }
    }
}
