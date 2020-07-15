using System;
using System.Collections.Generic;
using System.Linq;
using Cognizant.Hackathon.Mobile.Core.Exceptions;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cognizant.Hackathon.Shared.Mobile.Shared.Controls.CustomPopupDrawer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MultipleInputSectionPopup : ContentView
    {

        public List<Item> SelectionDataList { get; set; }

        public MultipleInputSectionPopup(List<object> selectionDataList, string headerText)
        {
            try
            {
                InitializeComponent();
                this.BindingContext = this;
                SelectionDataList = selectionDataList.Cast<Item>().ToList();

                this.LblHeader.Text = headerText;

            }
            catch (XamlParseException xp)
            {
                if (!xp.Message.Contains(ExceptionLiteral.StaticResNotFound))
                    throw;
            }

            this.InputSelectionListView.ItemsSource = SelectionDataList;
            this.InputSelectionListView.ItemSelected += InputSelectionListViewOnItemSelected;
            this.InputSelectionListView.ItemTapped += InputSelectionListViewTapped;
            this.InputSelectionListView.HorizontalScrollBarVisibility = ScrollBarVisibility.Never;
            this.InputSelectionListView.VerticalScrollBarVisibility = ScrollBarVisibility.Never;

            var deviceType = Device.Idiom == TargetIdiom.Phone ? "iphone" : "ipad";
            if (selectionDataList.Count <= 7)
            {
                if (deviceType.Equals("ipad"))
                {
                    this.InputSelectionListView.HeightRequest =
                            (this.InputSelectionListView.RowHeight * selectionDataList.Count) + 30;
                }
                else
                {
                    this.InputSelectionListView.HeightRequest =
                            (this.InputSelectionListView.RowHeight * selectionDataList.Count) + 20;
                }
            }
            else
            {
                if (deviceType.Equals("ipad"))
                {
                    this.InputSelectionListView.HeightRequest = this.InputSelectionListView.RowHeight * 7 + 30;
                }
                else
                {
                    this.InputSelectionListView.HeightRequest = this.InputSelectionListView.RowHeight * 7 + 20;
                }
            }
        }
    

        public static readonly BindableProperty InputSelectionStringResultProperty =
            BindableProperty.Create(
                nameof(InputSelectionStringResult),
                typeof(object),
                typeof(MultipleInputSectionPopup),
                string.Empty);
        
        public object InputSelectionStringResult
        {
            get
            {
                return (object)GetValue(InputSelectionStringResultProperty);
            }
            set
            {
                SetValue(InputSelectionStringResultProperty, value);
            }
        }

        public EventHandler InputSelectionEventHandler { get; set; }

        public EventHandler CancelButtonEventHandler { get; set; }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            CancelButtonEventHandler?.Invoke(this, e);
        }

        private void InputSelectionListViewOnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            InputSelectionStringResult = e.SelectedItem as object;

            InputSelectionEventHandler?.Invoke(this, e);
        }

        private void InputSelectionListViewTapped(object sender, ItemTappedEventArgs args)
        {
            if (args.Item != null)
            {
                this.InputSelectionListView.SelectedItem = null;
            }
        }
    }
}
