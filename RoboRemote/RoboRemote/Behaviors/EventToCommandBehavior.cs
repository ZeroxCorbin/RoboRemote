using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RoboRemote.Behaviors
{
    public class EventToCommandBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.Focused += Entry_Focused;
            base.OnAttachedTo(entry);
        }

        private void Entry_Focused(object sender, FocusEventArgs e)
        {

        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.Focused -= Entry_Focused;
            base.OnDetachingFrom(entry);
        }

        //void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        //{
        //    double result;
        //    bool isValid = double.TryParse(args.NewTextValue, out result);
        //    ((Entry)sender).TextColor = isValid ? Color.Default : Color.Red;
        //}
    }

}
