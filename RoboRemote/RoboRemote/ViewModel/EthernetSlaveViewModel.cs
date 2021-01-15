using System;
using System.ComponentModel;
using SocketManagerNS;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using TM_Comms;
using RoboRemote.Interfaces;
using System.Linq;

namespace RoboRemote.ViewModel
{
    public class EthernetSlaveViewModel : IItemViewModel
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

        public string Title { get => "RoboRemote: TM Ethernet Slave"; }
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
        public bool Heartbeat { get => heartbeat; set { SetProperty(ref heartbeat, value); } }

        public Command ConnectCommand { get => new Command(execute: () => { ConnectAction(); }, canExecute: () => true); }

        public ObservableCollection<IItem> Items { get; } = new ObservableCollection<IItem>();

        public EthernetSlaveViewModel()
        {
            Socket = App.EthernetSlaveSocket;
            ConnectionString = Socket.IPAddressString;

            if (Socket.IsConnected)
                ConnectButtonText = "Close";

            Socket.ConnectState += Socket_ConnectState;
            Socket.MessageReceived += Socket_MessageReceived;

        }

        private void ConnectAction()
        {
            if (Socket.IsConnected)
            {
                Socket.Close();
            }
            else
            {
                Message = string.Empty;

                Socket.ConnectionString = $"{ConnectionString}:5891";
                if (Socket.Connect())
                    Socket.StartReceiveMessages(@"[$]", @"[*][A-Z0-9][A-Z0-9]");
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
                ConnectButtonText = "Connect";
                Heartbeat = false;
            }
        }

        private int MessageCount { get; set; } = 0;
        private object ItemsProcessing { get; set; } = new object();
        private void Socket_MessageReceived(object sender, string message, string pattern)
        {
            if (MessageCount++ == 0)
            {
                Heartbeat = !Heartbeat;

                if (Regex.IsMatch(message, @"^TMSVR,.*,local")) return;

                string res = Regex.Replace(message, @"^[$]TMSVR,\w*,[0-9],[0-9],", new MatchEvaluator((target) => ""));
                res = Regex.Replace(res, @",[*].*$", new MatchEvaluator((target) => ""));
                ProcessJSON(res);
            }

            if (MessageCount >= 100)
                MessageCount = 0;
        }

        private void ProcessJSON(string message)
        {
            EthernetSlaveXMLData.File data = EthernetSlave.GetXMLCommands(App.Settings.Version);
            List<EthernetSlaveXMLData.FileSetting> lst = new List<EthernetSlaveXMLData.FileSetting>(data.CodeTable);

            JArray arr = Newtonsoft.Json.JsonConvert.DeserializeObject<JArray>(message);
            if (Items.Count == 0)
                foreach (var item in arr)
                {
                    EthernetSlaveXMLData.FileSetting set = lst.FirstOrDefault((e) => e.Item.Equals(item["Item"].ToString()));
                    if (set != null)
                        Application.Current.MainPage.Dispatcher.BeginInvokeOnMainThread(new Action(() =>
                        {
                            lock (ItemsProcessing)
                                Items.Add(new EthernetSlaveItemViewModel(set.Item, item["Value"].ToString().TrimStart('[').TrimEnd(']').Replace("\n", "").Replace("\r", "").Trim(), set.DataType.ToLower(), set.Accessibility.Replace("/", ""), Socket));
                        }
                        ));
                    else
                        Application.Current.MainPage.Dispatcher.BeginInvokeOnMainThread(new Action(() =>
                        {
                            lock (ItemsProcessing)
                                Items.Add(new EthernetSlaveItemViewModel(item["Item"].ToString(), item["Value"].ToString().TrimStart('[').TrimEnd(']').Replace("\n", "").Replace("\r", "").Trim(), "string", "R", Socket));
                        }
                        ));
                }
            else
            {
                lock (ItemsProcessing)
                    foreach (var item in arr)
                    {
                        EthernetSlaveItemViewModel vm = (EthernetSlaveItemViewModel)Items.FirstOrDefault((e) => e.Name.Equals(item["Item"].ToString()));
                        if (vm != null)
                            vm.Value = item["Value"].ToString().TrimStart('[').TrimEnd(']').Replace("\n", "").Replace("\r", "").Trim();
                    }
            }
        }


        //    private void CleanSock()
        //{
        //    if (Socket != null)
        //    {
        //        //Dispatcher.BeginInvoke(DispatcherPriority.Render,
        //        //        (Action)(() =>
        //        //        {
        //        //            ConnectionInActive();
        //        //        }));

        //        Socket.MessageReceived -= Socket_MessageReceived;
        //        Socket.ConnectState -= Socket_ConnectState;

        //        Socket.StopReceiveAsync();
        //        Socket.Close();

        //        Socket = null;
        //    }
        //}
        //private void Socket_ConnectState(object sender, bool data)
        //{
        //if (!data)
        //    CleanSock();
        //else
        //{
        //    Dispatcher.BeginInvoke(DispatcherPriority.Render,
        //            (Action)(() =>
        //            {
        //                ConnectionActive();
        //            }));

        //    Socket.MessageReceived += Socket_MessageReceived;
        //    Socket.StartReceiveMessages(@"[$]", @"[*][A-Z0-9][A-Z0-9]");

        //    DataReceiveStopWatch.Restart();
        //}
        //}

        //private void Socket_MessageReceived(object sender, string message, string pattern)
        //{
        //long time = DataReceiveStopWatch.ElapsedMilliseconds;
        //PackRate.Add(time);
        //DataReceiveStopWatch.Restart();

        //int updateRate;
        //if (!PackRate.IsFull)
        //    updateRate = (int)(SliderValue / time);
        //else
        //{
        //    updateRate = (int)(SliderValue / PackRate.Average);

        //    Dispatcher.BeginInvoke(DispatcherPriority.Normal,
        //    (Action)(() =>
        //    {
        //        TxtAverageMessage.Text = (1.0 / (PackRate.Average / 1000.0)).ToString("# Hz");
        //    }));
        //}

        //if (PackRate.Count < updateRate && Regex.IsMatch(message, @"^[$]TMSVR,\w*,[0-9]"))
        //    return;

        //PackRate.Count = 0;

        //EthernetSlave es = new EthernetSlave();

        //if (!es.ParseMessage(message))
        //{
        //    Dispatcher.BeginInvoke(DispatcherPriority.Render,
        //            (Action)(() =>
        //            {
        //                TxtSocketResponse.Text = message;
        //            }));
        //}
        //else
        //{
        //    Dispatcher.BeginInvoke(DispatcherPriority.Render,
        //            (Action)(() =>
        //            {
        //                if (es.Header == EthernetSlave.Headers.TMSVR && es.TransactionID_Int >= 0 && es.TransactionID_Int <= 9)
        //                    TxtSocketResponse.Text = message;
        //                else
        //                {
        //                    TxtCommandResponse.Text += es.Message;
        //                    TxtCommandResponse.ScrollToEnd();
        //                }
        //            }));
        //}
        //}
    }
}
