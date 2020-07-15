using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace LinC.Behaviors
{
    public class TelephoneNumberFormatter : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += this.HandleTextChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= this.HandleTextChanged;
            base.OnDetachingFrom(bindable);
        }

        private void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            string tele = Regex.Replace(e.NewTextValue, @"[^\d]", string.Empty);

            if (!string.IsNullOrEmpty(tele) && tele.Length < 10)
            {
                if (tele.Length == 4 && e.NewTextValue.Length != 6)
                {
                    ((Entry)sender).Text = "(" + tele.Substring(0, 3) + ")" + " " + tele[3];
                }
                else if (!string.IsNullOrEmpty(e.OldTextValue) && Regex.Replace(e.OldTextValue, @"[^\d]", string.Empty).Length == 6 && tele.Length == 7)
                {
                    ((Entry)sender).Text = "(" + tele.Substring(0, 3) + ")" + " " + tele.Substring(3, 3) + "-" + tele[6];
                }

                if (!string.IsNullOrEmpty(e.OldTextValue) && e.OldTextValue.Length == 11 && e.NewTextValue.Length == 10)
                {
                    ((Entry)sender).Text = "(" + tele.Substring(0, 3) + ")" + " " + tele.Substring(3, 3);
                }
                else if (e.NewTextValue.Length == 6)
                {
                    ((Entry)sender).Text = Regex.Replace(e.NewTextValue, @"[^\d]", string.Empty);
                }
            }
            else if (string.IsNullOrEmpty(Regex.Replace(tele, @"[^\d]", string.Empty)))
            {
                ((Entry)sender).Text = string.Empty;
            }
        }
    }
}
