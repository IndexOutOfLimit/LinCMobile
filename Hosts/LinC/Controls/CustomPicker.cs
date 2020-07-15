using System.Windows.Input;
using Xamarin.Forms;

namespace LinC.Controls
{
    public class CustomPicker : Picker
    {
        public CustomPicker()
        {
            Command = new Command(PickerTappedAction);
        }

        private void PickerTappedAction(object obj)
        {
            //SelectedDateValue = (string)obj;
        }

        public static readonly BindableProperty CommandProperty =
          BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(CustomPicker), null);

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly BindableProperty CommandParameterProperty =
           BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(CustomPicker), null);

        public static readonly BindableProperty FinishTitleProperty =
            BindableProperty.Create(nameof(FinishTitle), typeof(string), typeof(CustomPicker), "Save");

        public static readonly BindableProperty CancelTitleProperty =
            BindableProperty.Create(nameof(CancelTitle), typeof(string), typeof(CustomPicker), "Cancel");

        public static readonly BindableProperty CenterTitleProperty =
            BindableProperty.Create(nameof(CenterTitle), typeof(string), typeof(CustomPicker), string.Empty);


        public ICommand Command
        {
            get { return (ICommand)this.GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
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
