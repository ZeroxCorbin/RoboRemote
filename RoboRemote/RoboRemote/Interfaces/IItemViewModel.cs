using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;

namespace RoboRemote.Interfaces
{
    public interface IItemViewModel : INotifyPropertyChanged
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
        bool IsMessage { get; }

        bool IsRunning { get; set; }
        bool Heartbeat { get; set; }

        Command ConnectCommand { get; }

        ObservableCollection<IItem> Items { get; }
    }
}
