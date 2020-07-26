using LinC.Infrastructure;
using Xamarin.Forms.Xaml;

namespace LinC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SupplierCataloguePage : BaseContentPage
    {
        public SupplierCataloguePage()
        {
            InitializeComponent();
        }

        void listView_ItemSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
        }
    }
}
