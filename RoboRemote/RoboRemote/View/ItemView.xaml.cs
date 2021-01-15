using RoboRemote.Interfaces;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RoboRemote.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemView : ContentPage
    {
        public ItemView(IItemViewModel bind)
        {
            BindingContext = bind;

            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            ((IItemViewModel)BindingContext).IsRunning = false;
        }

        //public async void ShowEditPopup(IItem item)
        //{
        //    string res = null;
        //    if (item.Type == "Int16")
        //        res =  await DisplayPromptAsync($"{item.Name}", "New Value", initialValue: $"{item.Value}", keyboard: Keyboard.Numeric);
        //    if (item.Type == "Int32")
        //        res = await DisplayPromptAsync($"{item.Name}", "New Value", initialValue: $"{item.Value}", keyboard: Keyboard.Numeric);
        //}
    }
}