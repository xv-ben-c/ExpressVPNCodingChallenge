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


        //collection view source

        public List<VPNServer> Servers
        {
            get
            {
                return ServerList.PrimaryServerList.Servers;
            }
        }
        

        private void Refresh()
        {
            ServerList.PrimaryServerList.Refresh();
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
