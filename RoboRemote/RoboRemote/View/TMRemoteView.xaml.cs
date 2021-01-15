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
    public partial class TMRemoteView : ContentPage
    {
        public TMRemoteView(ITMRemoteView bind)
        {
            if (!DesignMode.IsDesignModeEnabled)
                BindingContext = bind;

            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            ((ITMRemoteView)BindingContext).IsRunning = false;
        }

        private double width;
        private double height;

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (width != this.width || height != this.height)
            {
                this.width = width;
                this.height = height;
                double ratio = width / height;

                ((ITMRemoteView)this.BindingContext).ButtonHeight = 80 * ratio;

                if (width > height)
                {//Landscape
                    //((ITMRemoteView)this.BindingContext).ButtonHeight = 80;
                }
                else
                {//Portrait
                    //((ITMRemoteView)this.BindingContext).ButtonHeight = -1;
                }
            }
        }
    }
}