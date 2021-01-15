using RoboRemote.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using TM_Comms;
using Xamarin.Forms;

namespace RoboRemote.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        private bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }


        private string versionString;

        public bool IsEthernetSlaveConnected { get => App.EthernetSlaveSocket.IsConnected; set { OnPropertyChanged(); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VersionSelectEnable")); } }
        public bool IsModbusConnected { get => App.ModbusSocket.IsConnected; set { OnPropertyChanged(); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VersionSelectEnable")); } }

        public bool VersionSelectEnable { get => (!App.ModbusSocket.IsConnected) & (!App.EthernetSlaveSocket.IsConnected); }

        public string Title { get; } = "RoboRemote";

        public TMflowVersions Version  { get => App.Settings.Version; set { App.Settings.Version = value; } }
        public string VersionString { get => App.Settings.Version.ToString(); set { Version = (TMflowVersions)Enum.Parse(typeof(TMflowVersions), value); SetProperty(ref versionString, value); } }
        public List<string> VersionNames { get => new List<string>(Enum.GetNames(typeof(TMflowVersions))); }

        public Color PageTextColor { get => App.Settings.PageTextColor; }
        public Color PageBackgroundColor { get => App.Settings.PageBackgroundColor; }
        public Color ButtonTextColor { get => App.Settings.ButtonTextColor; }
        public Color ButtonBackgroundColor { get => App.Settings.ButtonBackgroundColor; }
        public Color EntryTextColor { get => App.Settings.EntryTextColor; }
        public Color EntryBackgroundColor { get => App.Settings.EntryBackgroundColor; }
        public Color EntryPlaceholderColor { get => App.Settings.EntryPlaceholderColor; }

        public Command NavigateEthernetSlave { get; }
        public Command NavigateEthernetSlaveControl { get; }
        public Command NavigateModbus { get; }
        public Command NavigateRemote { get; }
        public Command NavigateModbusControl { get; }
        public Command Navigate3D { get; }
        public MainPageViewModel()
        {
            if (DesignMode.IsDesignModeEnabled)
                return;

            App.Settings.PropertyChanged += Settings_PropertyChanged;

        App.EthernetSlaveSocket.ConnectState += EthernetSlaveSocket_ConnectState;
            App.ModbusSocket.ConnectState += ModbusSocket_ConnectState;

            //NavigateEthernetSlave = new Command(() =>
            //{
            //    Application.Current.MainPage.Navigation.PushAsync(new TMConnectionView(new TMEthernetSlaveViewModel()));
            //});

            NavigateEthernetSlaveControl = new Command(() =>
            {
                Application.Current.MainPage.Navigation.PushAsync(new ItemView(new EthernetSlaveViewModel()));
                Application.Current.MainPage.Title = "Ethernet Slave";
            });

            //NavigateModbus = new Command(() =>
            //{
            //    Application.Current.MainPage.Navigation.PushAsync(new TMConnectionView(new TMModbusViewModel()));
            //});

            NavigateModbusControl = new Command(() =>
            {
                Application.Current.MainPage.Navigation.PushAsync(new ItemView(new ModbusViewModel()));
                ((NavigationPage)Application.Current.MainPage).Title = "Modbus";
            });

            //Navigate3D = new Command(() =>
            //{
            //    Application.Current.MainPage.Navigation.PushAsync(new _3DView());
            //    ((NavigationPage)Application.Current.MainPage).Title = "3D";
            //});

            //NavigateRemote = new Command(() =>
            //{
            //    Application.Current.MainPage.Navigation.PushAsync(new TMRemoteView(new TMModbusViewModel()));
            //});
        }

        private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName.Equals("TextColor"))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TextColor"));

            if (e.PropertyName.Equals("BackgroundColor"))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BackgroundColor"));
        }

        private void ModbusSocket_ConnectState(object sender, bool state) => IsModbusConnected = state;

        private void EthernetSlaveSocket_ConnectState(object sender, bool state) => IsEthernetSlaveConnected = state;
    }
}
