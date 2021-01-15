using RoboRemote.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RoboRemote.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TMConnectionView : ContentPage
    {
        public TMConnectionView(ITMConnectionView bind)
        {
            BindingContext = bind;

            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            ((ITMConnectionView)BindingContext).IsRunning = false;
        }
    }
}