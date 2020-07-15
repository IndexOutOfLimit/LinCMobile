using Xamarin.Forms;

namespace LinC.Templates
{
    public partial class CustomDatePickerTemplate : ContentView
    {
        public string SelectedDT { get; set; }

        public CustomDatePickerTemplate()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ParentContextProperty =
            BindableProperty.Create("ParentContext", typeof(object), typeof(CustomDatePickerTemplate), null, propertyChanged: OnParentContextPropertyChanged);

        public object ParentContext
        {
            get { return GetValue(ParentContextProperty); }
            set { SetValue(ParentContextProperty, value); }
        }

        private static void OnParentContextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue && newValue != null)
            {
                (bindable as CustomDatePickerTemplate).ParentContext = newValue;
            }
        }

        void DatePickerTapped(System.Object sender, System.EventArgs e)
        {
            this.DtPicker.Focus();
        }
    }
}
