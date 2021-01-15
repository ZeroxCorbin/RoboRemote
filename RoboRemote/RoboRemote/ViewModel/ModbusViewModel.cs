using System;
using System.ComponentModel;
using SocketManagerNS;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using TM_Comms;
using System.Threading;
using System.Collections.ObjectModel;
using RoboRemote.Interfaces;

namespace RoboRemote.ViewModel
{
    public class ModbusViewModel : IItemViewModel
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

        private SocketManager Socket { get; set; }

        private string connectionString;
        private string connectButtonText = "Connect";
        private bool connectionState;
        private string message;
        private bool isRunning;
        private bool heartbeat;

        public string Title { get => "RoboRemote: TM Modbus"; }
        public Color PageTextColor { get => App.Settings.PageTextColor; }
        public Color PageBackgroundColor { get => App.Settings.PageBackgroundColor; }
        public Color ButtonTextColor { get => App.Settings.ButtonTextColor; }
        public Color ButtonBackgroundColor { get => App.Settings.ButtonBackgroundColor; }
        public Color EntryTextColor { get => App.Settings.EntryTextColor; }
        public Color EntryBackgroundColor { get => App.Settings.EntryBackgroundColor; }
        public Color EntryPlaceholderColor { get => App.Settings.EntryPlaceholderColor; }

        public string ConnectionString { get => connectionString; set => SetProperty(ref connectionString, value); }
        public string ConnectButtonText { get => connectButtonText; set => SetProperty(ref connectButtonText, value); }
        public bool ConnectionState { get => connectionState; set => SetProperty(ref connectionState, value); }
        public string Message { get => message; set { SetProperty(ref message, value); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsMessage")); } }
        public bool IsMessage { get => !string.IsNullOrEmpty(message); }

        public bool IsRunning { get => isRunning; set => SetProperty(ref isRunning, value); }
        private bool Cancel { get; set; } = false;
        public bool Heartbeat { get => heartbeat; set { SetProperty(ref heartbeat, value); } }

        public Command ConnectCommand { get => new Command(execute: () => { ConnectAction(); }, canExecute: () => true); }

        public ObservableCollection<IItem> Items { get; } = new ObservableCollection<IItem>();

        public ModbusViewModel()
        {
            Socket = App.ModbusSocket;
            ConnectionString = Socket.IPAddressString;

            if (Socket.IsConnected)
                ConnectButtonText = "Close";

            Socket.ConnectState += Socket_ConnectState;

            GetItems();

            if (Socket.IsConnected && !IsRunning)
                ThreadPool.QueueUserWorkItem(new WaitCallback(AsyncRecieveThread_DoWork));
        }
        private void GetItems()
        {
            foreach (var kv in ModbusDictionary.ModbusData[App.Settings.Version])
                Items.Add(new ModbusItemViewModel(kv.Key, kv.Value, Socket));
        }

        private void ConnectAction()
        {
            if (Socket.IsConnected)
            {
                Cancel = true;
                while (isRunning) Thread.Sleep(1);

                Socket.Close();
            }
            else
            {
                Message = string.Empty;

                Socket.ConnectionString = $"{ConnectionString}:502";
                if (Socket.Connect())
                {
                    if (Socket.IsConnected && !IsRunning)
                        ThreadPool.QueueUserWorkItem(new WaitCallback(AsyncRecieveThread_DoWork));
                }
                else
                    Message = Socket.IsException ? Socket.Exception.Message : "Unable to connect!";
            }
        }

        private void Socket_ConnectState(object sender, bool state)
        {
            ConnectionState = state;
            if (state)
                ConnectButtonText = "Close";
            else
            {
                Cancel = true;
                while (isRunning) Thread.Sleep(1);

                ConnectButtonText = "Connect";
                Heartbeat = false;
            }

        }


        private void AsyncRecieveThread_DoWork(object sender)
        {
            IsRunning = true;
            Cancel = false;

            while (!Cancel)
            {
                try
                {
                    Heartbeat = !Heartbeat;

                    foreach (ModbusItemViewModel mvi in Items)
                    {
                        if (!Socket.IsConnected) break;

                        mvi.Read();
                    }
                }
                catch
                {
                    break;
                }
            }

            IsRunning = false;
            Cancel = false;
        }

        //public class Remote
        //{
        //    public event PropertyChangedEventHandler PropertyChanged;

        //    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //    private bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        //    {
        //        if (Object.Equals(storage, value))
        //            return false;

        //        storage = value;
        //        OnPropertyChanged(propertyName);
        //        return true;
        //    }

        //    private SimpleModbusTCP ModbusTCP { get; }

        //    private bool isRunning;
        //    public bool IsRunning { get => isRunning; set => SetProperty(ref isRunning, value); }

        //    private double buttonHeight;
        //    private bool remoteError;
        //    private bool remoteProjectRunning;
        //    private bool remoteProjectPaused;
        //    private bool remoteProjectEditing;
        //    private bool remoteGetControl;
        //    private bool remoteSafePortA;
        //    private bool remoteEStop;
        //    private int remoteMAMode;

        //    public double ButtonHeight { get => buttonHeight; set => SetProperty(ref buttonHeight, value); }

        //    public bool RemoteError { get => remoteError; set => SetProperty(ref remoteError, value); }
        //    public bool RemoteProjectRunning { get => remoteProjectRunning; set => SetProperty(ref remoteProjectRunning, value); }
        //    public bool RemoteProjectPaused { get => remoteProjectPaused; set => SetProperty(ref remoteProjectPaused, value); }
        //    public bool RemoteProjectEditing { get => remoteProjectEditing; set => SetProperty(ref remoteProjectEditing, value); }
        //    public bool RemoteGetControl { get => remoteGetControl; set => SetProperty(ref remoteGetControl, value); }
        //    public bool RemoteSafePortA { get => remoteSafePortA; set => SetProperty(ref remoteSafePortA, value); }
        //    public bool RemoteEStop { get => remoteEStop; set => SetProperty(ref remoteEStop, value); }
        //    public int RemoteMAMode { get => remoteMAMode; set => SetProperty(ref remoteMAMode, value); }
        //    public bool RemotePlayPause { set => ModbusTCP.SetBool(ModbusDictionary.ModbusData[App.Settings.Version]["Play/Pause"].Addr, value); }
        //    public bool RemoteStop { set => ModbusTCP.SetBool(ModbusDictionary.ModbusData[App.Settings.Version]["Stop"].Addr, value); }
        //    public bool RemoteStickPlus { set => ModbusTCP.SetBool(ModbusDictionary.ModbusData[App.Settings.Version]["Stick+"].Addr, value); }
        //    public bool RemoteStickMinus { set => ModbusTCP.SetBool(ModbusDictionary.ModbusData[App.Settings.Version]["Stick-"].Addr, value); }

        //    public Command StopCommand => new Command(new Action(() => RemoteStop = true));
        //    public Command PlayPauseCommand => new Command(new Action(() => RemotePlayPause = true));
        //    public Command MinusCommand => new Command(new Action(() => RemoteStickMinus = true));
        //    public Command PlusCommand => new Command(new Action(() => RemoteStickPlus = true));

        //    public string Title => throw new NotImplementedException();

        //    public string ConnectButtonText { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        //    public Command ConnectCommand => throw new NotImplementedException();

        //    private void AsyncRecieveThread_DoWork(object sender)
        //    {
        //        IsRunning = true;
        //        while (IsRunning)
        //        {
        //            if (!ModbusTCP.Socket.IsConnected) break;
        //            try
        //            {
        //                RemoteError = ModbusTCP.GetBool(ModbusDictionary.ModbusData[App.Settings.Version]["Error"].Addr);
        //                RemoteProjectRunning = ModbusTCP.GetBool(ModbusDictionary.ModbusData[App.Settings.Version]["Project Running"].Addr);
        //                RemoteProjectPaused = ModbusTCP.GetBool(ModbusDictionary.ModbusData[App.Settings.Version]["Project Paused"].Addr);
        //                RemoteProjectEditing = ModbusTCP.GetBool(ModbusDictionary.ModbusData[App.Settings.Version]["Project Editing"].Addr);
        //                RemoteGetControl = ModbusTCP.GetBool(ModbusDictionary.ModbusData[App.Settings.Version]["Get Control"].Addr);
        //                RemoteSafePortA = ModbusTCP.GetBool(ModbusDictionary.ModbusData[App.Settings.Version]["EStop"].Addr);
        //                RemoteEStop = ModbusTCP.GetBool(ModbusDictionary.ModbusData[App.Settings.Version]["EStop"].Addr);
        //                RemoteMAMode = ModbusTCP.GetInt16(ModbusDictionary.ModbusData[App.Settings.Version]["M/A Mode"].Addr);
        //            }
        //            catch
        //            {
        //                IsRunning = false;
        //            }

        //            Thread.Sleep(1000);
        //        }
        //    }

        //    private string Print()
        //    {
        //        return $"Mode : {(RemoteMAMode == 0 ? "Unknown" : "")}{(RemoteMAMode == 1 ? "Auto" : "")}{(RemoteMAMode == 2 ? "Manual" : "")}\r\n" +
        //                $"Project Status : {(RemoteProjectRunning ? "Running" : "")}{(RemoteProjectPaused ? "Paused" : "")}{(RemoteProjectEditing ? "Editing" : "")}\r\n" +
        //                $"Get Control : {RemoteGetControl}\r\n" +
        //                $"Error : {RemoteError}\r\n" +
        //                $"EStop : {RemoteEStop}\r\n" +
        //                $"Safe Port A : {RemoteSafePortA}\r\n";
        //    }
        //}

    }
}
