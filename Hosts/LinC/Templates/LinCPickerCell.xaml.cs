using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LinC.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LinCPickerCell : ContentView
    {
        public LinCPickerCell()
        {
            InitializeComponent();
        }
        public static readonly BindableProperty ParentContextProperty =
            BindableProperty.Create("ParentContext", typeof(object), typeof(PickerCell), null, propertyChanged: OnParentContextPropertyChanged);

        public object ParentContext
        {
            get { return GetValue(ParentContextProperty); }
            set { SetValue(ParentContextProperty, value); }
        }

        private static void OnParentContextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue && newValue != null)
            {
                (bindable as PickerCell).ParentContext = newValue;
            }
        }
    }
}
