using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.OffScreen;

namespace czogovuaWraper
{
    public class CzoGovUaWraper : ICzoGovUaWraper
    {
        private bool browserLoading = false;
        private Param param;
        private static BrowseState browseState;
        private static bool cefInit = false;
        private static object objSyn = new object();
        private static ChromiumWebBrowser browser;
        private string siteUrl = "https://id.gov.ua/sign-widget/v20200813/?address=https://czo.gov.ua&formType=3&debug=true";
        public int Init(int time_out_ms,string server_name = "")
        {
            if (server_name != "")
            {
                siteUrl = server_name;
            }
            lock (objSyn)
            {
                browseState = BrowseState.Init;
            }
            var settings = new CefSettings()
            {
                CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache")
            };
            if (!cefInit || !Cef.IsInitialized)
            {
                Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
                cefInit = true;
            }
            browserLoading = true;
            browser = new ChromiumWebBrowser(siteUrl) { 
            DialogHandler = new TempFileDialogHandler(param.SingPatch)
            };
            browser.LoadingStateChanged += BrowserLoadingStateChanged;
            int attempt = time_out_ms/100;
            while (browserLoading)
            {
                Thread.Sleep(100);
                attempt--;
                if (attempt == 0)
                    return -1;
            }
            return 0;
        }
        public IEnumerable<ISingInfo> CheakFileSing(string file_patch)
        {
            StartComand(BrowseState.CheakFile);
            return null;
        }
        public bool SingFile(string file_patch, string sing_patch, string sing_password,string sing_name="")
        {
            param = new Param();
            param.FilePatch = file_patch;
            param.SingPatch = sing_patch;
            param.SingPassword = sing_password;
            param.SingName = sing_name;
            StartComand(BrowseState.SingFile);
            return true;
        }
        private void StartComand(BrowseState comand)
        {
            lock (objSyn)
            {
                browseState = comand;
            }
            browser.Load(siteUrl);
        }
        public void ShutDown()
        {
            Cef.Shutdown();
        }
        private void BrowserLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
            {
                browserLoading = false;
                //browser.LoadingStateChanged -= BrowserLoadingStateChanged;
                if (browser.CanExecuteJavascriptInMainFrame)
                {
                    lock (objSyn)
                    {
                        switch (browseState)
                        {
                            case BrowseState.Init:
                                break;
                            case BrowseState.SingFile:
                                AddSingToFileAsync(browser);
                                break;
                            case BrowseState.CheakFile:
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    }
                }
            }
        }
        private async Task<string> AddSingToFileAsync(ChromiumWebBrowser browser_page)
        {
            browser_page.DialogHandler = new TempFileDialogHandler(param.SingPatch);
            browser_page.JsDialogHandler = new JsDialogHandler();
            await ButtonClickAsync(browser_page, "pkReadFileInput");
            await SetInputValueAsync(browser_page, "pkReadFilePasswordTextField", param.SingPassword);
            await ButtonClickAsync(browser_page, "pkReadFileButton");
            await ButtonClickAsync(browser_page, "pkInfoNextButton");
            browser_page.DialogHandler = new TempFileDialogHandler(param.FilePatch);
            await ButtonClickAsync(browser_page, "signInFileInput");
            await ButtonClickAsync(browser_page, "signButton");
            await ButtonClickAsync(browser_page, "resultLeftButton");
            var script_add_jquery = @"
            function getRezalt(){
            var element = document.getElementsByClassName('SignResult').item(0);
            var html = element.outerHTML;
            var data = { html: html }; 
            var json = JSON.stringify(data);
            return json;}
            getRezalt();";
            var rez = await browser_page.EvaluateScriptAsync(script_add_jquery);
            return rez.Result.ToString();
        }
        private async Task ButtonClickAsync(ChromiumWebBrowser browser_page,string button_id)
        {
            var script_add_jquery = "document.getElementById('"+ button_id + "').click();";
            await browser_page.EvaluateScriptAsync(script_add_jquery);
        }
        private async Task SetInputValueAsync(ChromiumWebBrowser browser_page, string input_id,string input_value)
        {
            var script_add_jquery = "document.getElementById('"+input_id+"').value=\"" + input_value + "\"";
            await browser_page.EvaluateScriptAsync(script_add_jquery);
        }
    }
}