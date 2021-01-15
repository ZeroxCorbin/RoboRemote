using RoboRemote.View;
using SocketManagerNS;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RoboRemote
{
    public partial class App : Application
    {
        private static string SettingsFilePath => Path.Combine(FileSystem.CacheDirectory, "app_settings.xml");
        public static SocketManager EthernetSlaveSocket { get; private set; } 
        public static SocketManager ModbusSocket { get; private set; } 
        public static ApplicationSettingsNS.ApplicationSettings_Serializer.ApplicationSettings Settings { get; private set; }

        static void SetupStatic()
        {
            EthernetSlaveSocket = new SocketManager();
            ModbusSocket= new SocketManager();
            Settings = ApplicationSettingsNS.ApplicationSettings_Serializer.Load(SettingsFilePath);
        }

        public App()
        {
            InitializeComponent();

            if (DesignMode.IsDesignModeEnabled)
                return;

            SetupStatic();

            EthernetSlaveSocket.ConnectionString = $"{Settings.EthernetSlaveRobotIP}:5891";
            ModbusSocket.ConnectionString = $"{Settings.ModbusRobotIP}:502";

            MainPage = new NavigationPage(new MainPage());

        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
            if (DesignMode.IsDesignModeEnabled)
                return;

            if (EthernetSlaveSocket.IsConnected)
                EthernetSlaveSocket.Close();

            if (ModbusSocket.IsConnected)
                ModbusSocket.Close();

            Settings.EthernetSlaveRobotIP = EthernetSlaveSocket.IPAddressString;
            Settings.ModbusRobotIP = ModbusSocket.IPAddressString;

            ApplicationSettingsNS.ApplicationSettings_Serializer.Save(SettingsFilePath, Settings);
        }

        protected override void OnResume()
        {

        }
    }
}
