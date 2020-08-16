using CefSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace czogovuaWraper
{
    public class JsDialogHandler : IJsDialogHandler
    {
        bool IJsDialogHandler.OnJSDialog(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, CefJsDialogType dialogType, string messageText, string defaultPromptText, IJsDialogCallback callback, ref bool suppressMessage)
        {
            return false;
        }

        bool IJsDialogHandler.OnBeforeUnloadDialog(IWebBrowser chromiumWebBrowser, IBrowser browser, string message, bool isReload, IJsDialogCallback callback)
        {
            //Custom implementation would look something like
            // - Create/Show dialog on UI Thread
            // - execute callback once user has responded
            // - callback.Continue(true);
            // - return true

            //NOTE: Returning false will trigger the default behaviour, no need to execute the callback if you return false.
            return false;
        }

        void IJsDialogHandler.OnResetDialogState(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {

        }

        void IJsDialogHandler.OnDialogClosed(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {

        }
    }
}
