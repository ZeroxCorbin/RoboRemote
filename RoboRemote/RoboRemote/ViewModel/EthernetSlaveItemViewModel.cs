using RoboRemote.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace RoboRemote.ViewModel
{
    public class EthernetSlaveItemViewModel : INotifyPropertyChanged, IItem
    {
        public event PropertyChangedEventHandler PropertyChanged;
        //protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //private bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        //{
        //    if (Object.Equals(storage, value))
        //        return false;

        //    storage = value;
        //    OnPropertyChanged(propertyName);
        //    return true;
        //}

        private SocketManagerNS.SocketManager Socket { get; }

        private string _value;
        public string Name { get; }
        public string Value { get => _value; set { _value = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value")); } }
        public string Type { get; }
        public string Addr { get; } = string.Empty;
        public string Access { get; }

        public bool IsWritable { get; }

        public Command WriteItem { get; }
        public Command EditItem { get; }

        public IItem Instance => this;

        public EthernetSlaveItemViewModel(string name, string value, string type, string access, SocketManagerNS.SocketManager socket)
        {
            Name = name;
            _value = value;
            Type = type;
            Access = access;
            Socket = socket;

            WriteItem = new Command(() =>
            {
                Write("1");
            });

            EditItem = new Command(() =>
            {
                Edit();
            });
        }

        public async void Edit()
        {
            string res = await Application.Current.MainPage.DisplayPromptAsync($"{Name}", "New Value", initialValue: $"{Value}", keyboard: Keyboard.Numeric);
            
            Write(res);
        }

        public void Write(string value)
        {
            if (!string.IsNullOrEmpty(value))
                Socket.Write(new TM_Comms.EthernetSlave($"{Name}={value}").Message);
        }
    }
}
