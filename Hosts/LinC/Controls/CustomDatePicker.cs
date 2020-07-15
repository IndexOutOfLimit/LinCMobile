using System.Windows.Input;
using Xamarin.Forms;

namespace LinC.Controls
{
    public class CustomDatePicker : DatePicker
    {
        public CustomDatePicker()
        {
            DateSelectedCommand = new Command(DtTapped);
        }

        private void DtTapped(object obj)
        {
            SelectedDateValue = (string)obj;
        }

        public string SelectedDateValue
        {
            get => (string)GetValue(SelectedDateValueProperty);
            set => SetValue(SelectedDateValueProperty, value);
        }

        public static readonly BindableProperty SelectedDateValueProperty = BindableProperty.Create(
            nameof(SelectedDateValue),
            typeof(string),
            typeof(CustomDatePicker),
            default(string));


        public static readonly BindableProperty DateSelectedCommandProperty =
            BindableProperty.Create("DateSelectedCommand", typeof(ICommand), typeof(CustomDatePicker), null);
        public static readonly BindableProperty FinishTitleProperty =
          BindableProperty.Create(nameof(FinishTitle), typeof(string), typeof(CustomPicker), "Save");

        public static readonly BindableProperty CancelTitleProperty =
            BindableProperty.Create(nameof(CancelTitle), typeof(string), typeof(CustomPicker), "Cancel");

        public static readonly BindableProperty CenterTitleProperty =
            BindableProperty.Create(nameof(CenterTitle), typeof(string), typeof(CustomPicker), "Date");

        public ICommand DateSelectedCommand
        {
            get { return (ICommand)GetValue(DateSelectedCommandProperty); }
            set { SetValue(DateSelectedCommandProperty, value); }
        }
        public string FinishTitle
        {
            get { return (string)GetValue(FinishTitleProperty); }
            set { SetValue(FinishTitleProperty, value); }
        }

        public string CancelTitle
        {
            get { return (string)GetValue(CancelTitleProperty); }
            set { SetValue(CancelTitleProperty, value); }
        }

        public string CenterTitle
        {
            get { return (string)GetValue(CenterTitleProperty); }
            set { SetValue(CenterTitleProperty, value); }
        }
    }
}
