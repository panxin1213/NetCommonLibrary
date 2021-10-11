using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System.Windows.Forms
{
    public static class WebBrowserEx
    {

        public static void ClearDocumentCompleted(this WebBrowser webobj)
        {
            var type = webobj.GetType();
            var documentCompleted = type.GetField("DocumentCompleted", BindingFlags.Instance | BindingFlags.NonPublic);
            var eventHandler = (WebBrowserDocumentCompletedEventHandler)documentCompleted.GetValue(webobj);
            var eventInfo = type.GetEvent("DocumentCompleted");
            if (eventHandler == null)
            {
                return;
            }
            foreach (var d in eventHandler.GetInvocationList())
            {
                eventInfo.RemoveEventHandler(webobj, d);
            }
        }
    }
}
