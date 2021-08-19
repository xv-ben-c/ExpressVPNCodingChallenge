
using CommonServiceLocator;
using ExpressVPNClientModel.LocationServer;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:ExpressVPN"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

namespace ExpressVPNClientViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class DIContainer
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public DIContainer()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<VPNServersViewModel>();



        }

        public void ConfigureWebApiProcessor()
        {
            //Replace the implementation class with altenative if format of server location (API response) document changes
            //E.g.

            //SimpleIoc.Default.Register<IWebRequestProcessor, JSONWebRequestProcessor>();
            SimpleIoc.Default.Register<IRequestProcessor, XMLWebRequestProcessor>();
        }


        public void ConfigurFileApiProcessor()
        {
            //Use the XMLFileRequestProcessor when testing with local XML files rather than web response
            SimpleIoc.Default.Register<IRequestProcessor, XMLFileRequestProcessor>();
        }


        public MainViewModel MainVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public VPNServersViewModel VPNServersVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<VPNServersViewModel>();
            }
        }



        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}
