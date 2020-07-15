using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cognizant.Hackathon.Mobile.Core.Infrastructure;
using LinC.Infrastructure;
using LinC.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LinC.Template
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabBarTemplate : ContentView
    {
        public Xamarin.Forms.Color DashBoardSelectedColor { get; set; }
        public Xamarin.Forms.Color MoreSelectedColor { get; set; }

        public TabBarTemplate()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ParentContextProperty =
            BindableProperty.Create("ParentContext", typeof(object), typeof(TabBarTemplate), null, propertyChanged: OnParentContextPropertyChanged);

        public object ParentContext
        {
            get { return GetValue(ParentContextProperty); }
            set { SetValue(ParentContextProperty, value); }
        }
       

        private static void OnParentContextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //if (newValue != oldValue && newValue != null)
            //{
            //    (bindable as TabBarTemplate).ParentContext = newValue;                
            //}
        }
        

        async void OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {           
            var tabCell = (sender as StackLayout);
            var item = (TapGestureRecognizer)tabCell.GestureRecognizers[0];

            Int16 output = 0;

            bool arg = Int16.TryParse(item.CommandParameter.ToString(), out output);

            Label[] iconArrary = { null, null, null};
            Label[] captionArrary = { null, null, null };

            if (tabCell != null)
            {
                iconArrary[output] = tabCell.Children[0] as Label;
                captionArrary[output] = tabCell.Children[1] as Label;
                if (output == 2)
                    MoreSelectedColor = Color.Green;
                if (output == 1)
                    DashBoardSelectedColor = Color.Green;
              captionArrary[output].TextColor = Color.Green;
                iconArrary[output].TextColor = Color.Green;
            }

            var tabCollection = (tabCell.Parent as Grid).Children;

            for(Int16 i = 0; i < 3; i++)
            {
                if (i == output) continue;

                var tabCellOther = tabCollection[i] as StackLayout;

                if (tabCellOther != null)
                {
                    iconArrary[i] = tabCellOther.Children[0] as Label;
                    captionArrary[i] = tabCellOther.Children[1] as Label;

                    captionArrary[i].TextColor = Color.Black;
                    iconArrary[i].TextColor = Color.Black;
                }
            }

            (ParentContext as ViewModelBase).TabCurrentIndex = output;
            (ParentContext as ViewModelBase).TabbarTappedCommand.Execute();

            //await App.Instance.MainPage.Navigation.("//root");
            int index = 0;
            foreach (var page in Navigation.NavigationStack.ToList())
            {
                
                if (page != null && index < Navigation.NavigationStack.Count - 1)
                {
                    Navigation.RemovePage(page);
                }
                index++;
            }
            
        }

        //public List<TabBarUiModel> TabsData => GetTabControlData();

        //public List<TabBarUiModel> GetTabControlData()
        //{
        //    if (TabsData == null)
        //    {

        //        List<TabBarUiModel> tabBars = new List<TabBarUiModel>()
        //        {
        //            new TabBarUiModel{ TabBarIcon = "tab_feed.png" , IsSelected = true, TabBarCaption = "Dashboard", TabBarCaptionColor= "Black"},
        //            new TabBarUiModel{ TabBarIcon = "tab_about.png" , IsSelected = false, TabBarCaption = "Profile", TabBarCaptionColor= "Black"},
        //            new TabBarUiModel{ TabBarIcon = "tab_feed.png" , IsSelected = false, TabBarCaption = "More", TabBarCaptionColor= "Black"}
        //        };

        //        return tabBars;
        //    }
        //    else return TabsData;
        //}
    }
}
