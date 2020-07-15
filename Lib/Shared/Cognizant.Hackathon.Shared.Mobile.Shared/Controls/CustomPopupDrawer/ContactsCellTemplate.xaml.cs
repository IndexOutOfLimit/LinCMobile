using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cognizant.Hackathon.Shared.Mobile.Shared.Controls.CustomPopupDrawer
{
    public partial class ContactsCellTemplate : CustomShadowFrame
    {
        public ContactsCellTemplate()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ParentContextProperty =
          BindableProperty.Create("ParentContext", typeof(object), typeof(ContactsCellTemplate), null, propertyChanged: OnParentContextPropertyChanged);

        public object ParentContext
        {
            get { return GetValue(ParentContextProperty); }
            set { SetValue(ParentContextProperty, value); }
        }

        private static void OnParentContextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue && newValue != null)
            {
                (bindable as ContactsCellTemplate).ParentContext = newValue;
            }
        }
    }
}
