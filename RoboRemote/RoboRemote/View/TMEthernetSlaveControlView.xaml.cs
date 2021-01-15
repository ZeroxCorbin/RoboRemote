using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RoboRemote.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TMEthernetSlaveControlView : ContentPage
    {
        public TMEthernetSlaveControlView(ITMEthernetSlaveControlView bind)
        {
            BindingContext = bind;

            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            ((ITMEthernetSlaveControlView)BindingContext).IsRunning = false;
        }
    }
}