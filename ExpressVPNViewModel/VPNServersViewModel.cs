using ExpressVPNClientModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ExpressVPNClientViewModel
{
    public class VPNServersViewModel : ViewModelBase, IView
    {

        public const string ViewName = "VPNServersPage";

        public void Activated(object state)
        {
        }

        public ICommand RefreshCommand { get; private set; }
        public ICommand BestServerCommand { get; private set; }
        //public ICommand CloseCommand { get; private set; }

        public RelayCommand<Window> CloseCommand { get; private set; }

        private MainViewModel MainVM; //FIXME

        public VPNServersViewModel(MainViewModel mainVM)
        {
            MainVM = mainVM;
            RefreshCommand = new RelayCommand(async () => await Refresh());
            BestServerCommand = new RelayCommand(() => BestServer());
            CloseCommand = new RelayCommand<Window>(w => Quit(w));

            //Initial population async
            Refresh();
        }


        //FIXME
        //collection view source

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
            //Move to AppConfig FIXME

            string mockAPIURL = "https://private-16d939-codingchallenge2020.apiary-mock.com/locations";


            await ServerModel.Instance.RefreshAsync(mockAPIURL);
            RaisePropertyChanged("Servers");
            //fixme observable collectioj
        }

        private void BestServer()
        {
            if (ServerModel.Instance.LocationMgr.AvailableLocations == 0)
            {
                MessageBox.Show("There are no locations with IP addresses at this time", "ExpressVPN Client");
                return;
            }

            if (!ServerModel.Instance.LocationMgr.PingComplete())
            {
                MessageBox.Show("Background ping tests are incomplete. Please try again in a few moments",  "ExpressVPN Client");
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

                //
                ServerModel.Instance.Shutdown();
                w.Close();
            }
        }

    }
}
