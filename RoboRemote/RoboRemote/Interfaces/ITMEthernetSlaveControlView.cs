using RoboRemote.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace RoboRemote.View
{



    public interface ITMEthernetSlaveControlView : INotifyPropertyChanged
    {
        string Title { get; }
        Color PageTextColor { get; }
        Color PageBackgroundColor { get; }
        Color ButtonTextColor { get; }
        Color ButtonBackgroundColor { get; }
        Color EntryTextColor { get; }
        Color EntryBackgroundColor { get; }
        Color EntryPlaceholderColor { get; }

        string ConnectionString { get; set; }

        string ConnectButtonText { get; set; }
        bool ConnectionState { get; set; }
        string Message { get; set; }

        bool IsRunning { get; set; }
        bool Heartbeat { get; set; }

        Command ConnectCommand { get; }

        ObservableCollection<EthernetSlaveWriteItem> WriteableItems { get; }
    }
}
