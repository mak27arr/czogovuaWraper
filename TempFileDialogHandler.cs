using CefSharp;
using System.Collections.Generic;
using System.IO;

namespace czogovuaWraper
{
    public class TempFileDialogHandler : IDialogHandler
    {
        private string file_patch;
        public TempFileDialogHandler(string FilePatch)
        {
            file_patch = FilePatch;
        }
        public bool OnFileDialog(IWebBrowser browserControl, IBrowser browser, CefFileDialogMode mode, string title, string defaultFilePath, List<string> acceptFilters, int selectedAcceptFilter, IFileDialogCallback callback)
        {
            callback.Continue(selectedAcceptFilter, new List<string> { file_patch });
            return true;
        }

        public bool OnFileDialog(IWebBrowser chromiumWebBrowser, IBrowser browser, CefFileDialogMode mode, CefFileDialogFlags flags, string title, string defaultFilePath, List<string> acceptFilters, int selectedAcceptFilter, IFileDialogCallback callback)
        {
            callback.Continue(selectedAcceptFilter, new List<string> { file_patch });
            return true;
        }
    }
}
