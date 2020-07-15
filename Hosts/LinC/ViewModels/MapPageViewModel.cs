using System.Collections;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Cognizant.Hackathon.Shared.Mobile.Core.Interfaces;
using LinC.ViewModels.Infrastructure;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LinC.ViewModels
{
    public class MapPageViewModel : ViewModelBase
    {
        private readonly ILinCApiServices _services;

        public Xamarin.Forms.Maps.Map Map { get; set; }
        public Xamarin.Essentials.Location MyLocation { get; set; }
        //private List<Position> _stopCoordinates;

        int _pinCreatedCount = 0;
        readonly ObservableCollection<LinC.ViewModels.Infrastructure.Location> _locations;

        public IEnumerable Locations => _locations;

        public MapPageViewModel(ILinCApiServices services)
        {
            _services = services;
        }

        protected override async Task OnShellNavigatingIn(string sender, ShellNavigatingEventArgs args)
        {
            await base.OnShellNavigatingIn(sender, args);
            await LoadMap();

        }

        /// <summary>
        /// Loads the map.
        /// </summary>
        /// <returns>The map.</returns>
        private async Task LoadMap()
        {
            AppSpinner.ShowLoading();
            await CheckLocationPermission();
            await InitializeGoogleMap();
            AppSpinner.HideLoading();
        }

        private async Task InitializeGoogleMap()
        {
            Map = new Xamarin.Forms.Maps.Map();
            Map.HasZoomEnabled = true;
            Map.IsShowingUser = true;

            var currentLoc = await GetUserLocation();
            if (currentLoc != null)
            {
                var pos = new Xamarin.Forms.Maps.Position(currentLoc.Latitude, currentLoc .Longitude);

                Map.Pins.Add(new Xamarin.Forms.Maps.Pin
                {
                    Label = $"Here I am",
                    Position = pos,
                });
            }

            //Defaults to current
            //Map.
            Map.MoveToRegion(Xamarin.Forms.Maps.MapSpan.FromCenterAndRadius(
                new Xamarin.Forms.Maps.Position(28.644800, 77.216721), Xamarin.Forms.Maps.Distance.FromKilometers(5000)));
        }

    }
}
