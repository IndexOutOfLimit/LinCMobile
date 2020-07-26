using System.Windows.Input;
using Xamarin.Forms;

namespace LinC.Behaviors
{
    public class ItemTappedAttached
    {
        public static readonly BindableProperty CommandProperty =
        BindableProperty.CreateAttached(
            propertyName: "Command",
            returnType: typeof(ICommand),
            declaringType: typeof(ListView),
            defaultValue: null,
            defaultBindingMode: BindingMode.OneWay,
            validateValue: null,
            propertyChanged: OnItemTappedChanged);

        public static ICommand Command
        {
            get;
            set;
        }

        public static ICommand GetItemTapped(BindableObject bindable)
        {
            return (ICommand)bindable.GetValue(CommandProperty);
        }

        public static void SetItemTapped(BindableObject bindable, ICommand value)
        {
            bindable.SetValue(CommandProperty, value);
        }

        public static void OnItemTappedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as ListView;
            if (control != null)
                control.ItemTapped += OnItemTapped;
        }

        private static void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var control = sender as ListView;
            Command = GetItemTapped(control);

            if (Command != null && Command.CanExecute(e.Item))
                Command.Execute(e.Item);
        }
    }
}
