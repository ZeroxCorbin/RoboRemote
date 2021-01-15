using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace RoboRemote.View
{
    public interface ITMRemoteView : INotifyPropertyChanged
    {
        string Title { get; }

        string ConnectButtonText { get; set; }
        double ButtonHeight { get; set; }

        bool IsRunning { get; set; }

        bool RemoteError { get; }
        bool RemoteProjectRunning { get; }
        bool RemoteProjectPaused { get; }
        bool RemoteProjectEditing { get; }
        bool RemoteGetControl { get;}
        //bool RemoteLight { get; set; }
        bool RemoteSafePortA { get;  }
        bool RemoteEStop { get;  }
        int RemoteMAMode { get;  }
        bool RemotePlayPause { set; }
        bool RemoteStop { set; }
        bool RemoteStickPlus { set; }
        bool RemoteStickMinus { set; }

        Command ConnectCommand { get; }
        Command StopCommand { get; }
        Command PlayPauseCommand { get; }
        Command MinusCommand { get; }
        Command PlusCommand { get; }
    }
}
