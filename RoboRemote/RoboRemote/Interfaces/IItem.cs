using System.ComponentModel;
using Xamarin.Forms;

namespace RoboRemote.Interfaces
{
    public interface IItem : INotifyPropertyChanged
    {
        IItem Instance { get;}
        string Name { get; }
        string Value { get; set; }
        string Type { get; }
        string Addr { get; }
        string Access { get; }

        bool IsWritable { get; }

        Command WriteItem { get; }
        Command EditItem { get; }

    }
}
