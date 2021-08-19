using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ExpressVPNClientModel;

namespace ExpressVPNClientViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        internal string ServerLocationsURI { get; private set; }
        public ICommand MainWndClosingCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            ///

            MainWndClosingCommand = new RelayCommand(() => MainWindowClosing());
            ServerLocationsURI = ConfigurationManager.AppSettings["ServerLocatorURI"];

            bool configureForWeb = true;
            if (Environment.GetCommandLineArgs().Length == 2)
            {
                var url = Environment.GetCommandLineArgs()[1];
                if (url.ToLower().StartsWith("http://") || url.ToLower().StartsWith("https://"))
                {
                    //Override the URI in app.config
                    ServerLocationsURI = url;
                }
                else if (url.ToLower().StartsWith("file://"))
                {
                    configureForWeb = false;
                    //Override the URI in app.config
                    ServerLocationsURI = url;
                }
            }

            if (configureForWeb)
            {
                //We are processing server locations from web
                (Application.Current.Resources["Locator"] as DIContainer).ConfigureWebApiProcessor();
            }
            else
            {
                //We are processing server locations from local file (.xml)
                (Application.Current.Resources["Locator"] as DIContainer).ConfigurFileApiProcessor();
            }



        }


        private void MainWindowClosing()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            ServerModel.Instance?.Shutdown();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

    }

}
