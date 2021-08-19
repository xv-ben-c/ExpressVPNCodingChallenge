using CommonServiceLocator;
using ExpressVPNClientModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ExpressVPNClientViewModel
{
    public class VPNServersViewModel : ViewModelBase
    {

        public const string ViewName = "VPNServersPage";

       

        public ICommand RefreshCommand { get; private set; }
        public ICommand BestServerCommand { get; private set; }

        public RelayCommand<Window> CloseCommand { get; private set; }

        private MainViewModel MainVM;

        public VPNServersViewModel(MainViewModel mainVM)
        {
            MainVM = mainVM;
            RefreshCommand = new RelayCommand(async () => await Refresh());
            BestServerCommand = new RelayCommand(() => BestServer());
            CloseCommand = new RelayCommand<Window>(w => Quit(w));

            if (string.IsNullOrEmpty(MainVM.ServerLocationsURI))
            {
                //MessageBox.Show("Missing setting for Server Locator URL","ExpressVPN Client - Configuration Error");
            }

            ServerModel.Init(ServiceLocator.Current, MainVM.ServerLocationsURI, PingUpdate );

            //Initial population async
            Refresh();
        }

        private void PingUpdate()
        {
            RaisePropertyChanged("Servers");
        }

        #region Dependency Props

        /// <summary>
        /// 
        /// </summary>
        public List<ServerLocation> Servers
        {
            get { return ServerModel.Instance.LocationMgr.PresentationList(); }
        }

        public string RefreshButtonText
        {
            get { return ServerModel.Instance.RefreshButtonText; }
        }

        #endregion

        private async Task Refresh()
        {
            await ServerModel.Instance.RefreshAsync();
            RaisePropertyChanged("Servers");
            //TODO  -alternatively use observable collection
        }

        private void BestServer()
        {
            if (ServerModel.Instance.LocationMgr.AvailableLocations == 0)
            {
                MessageBox.Show("There are no locations with IP addresses at this time", "ExpressVPN Client");
                return;
            }

            ServerLocation sl = ServerModel.Instance.LocationMgr.BestServerLocation();
            if (sl==null)
            {
                MessageBox.Show("We don't have any data on best location yet. Please try again in a few moments",  "ExpressVPN Client");
                return;
            }

            MessageBox.Show( $"According to ping tests I’ve been running in the background, the best location for you appears to be {sl.Location}", "ExpressVPN Client");

        }

        private void Quit(Window w)
        {
            if (w != null)
            {
                //Close background services cleanly
                MainVM.MainWndClosingCommand.Execute(null);
               
                w.Close();
            }
        }

    }
}
