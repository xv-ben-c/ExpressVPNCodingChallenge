﻿using GalaSoft.MvvmLight;
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
        internal string ServerLocationsURL { get; private set; }
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
            ServerLocationsURL = ConfigurationManager.AppSettings["ServerLocatorURL"];
        }


        private void MainWindowClosing()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            ServerModel.Instance?.Shutdown();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

    }

}
