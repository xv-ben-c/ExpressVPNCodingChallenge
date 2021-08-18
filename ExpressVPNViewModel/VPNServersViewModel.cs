using ExpressVPNClientModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExpressVPNClientViewModel
{
    public class VPNServersViewModel : ViewModelBase, IView
    {

        public const string ViewName = "VPNServersPage";

        public void Activated(object state)
        {
            //throw new NotImplementedException();
        }

        public ICommand RefreshCommand { get; private set; }
        public ICommand BestServerCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }

        private MainViewModel MainVM;

        public VPNServersViewModel(MainViewModel mainVM)
        {
            MainVM = mainVM;
            RefreshCommand = new RelayCommand(() => Refresh());
            BestServerCommand = new RelayCommand(() => BestServer());
            CloseCommand = new RelayCommand(() => Quit());
        }


        //FIXME
        //collection view source

        public List<ServerLocation> Servers
        {
            get
            {
                return ServerModel.Instance.LocationMgr.PresentationList();
            }
        }
        

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
            //Clicking it pops up a dialog with text “According to ping tests I’ve been running in the background, the best location for you appears to be [location name]”.

        }

        private void Quit()
        {


        }

    }
}
