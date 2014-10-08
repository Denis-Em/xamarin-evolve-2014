﻿using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Maps;
using Android.Locations;
using Android.Util;
using Android.Gms.Maps.Model;
using Android.Graphics;

namespace MapTestApp
{
    [Activity(Label = "MapTestApp", MainLauncher = true)]
    public class MainActivity : Activity, ILocationListener
    {
        private GoogleMap _map;
        private MapFragment _mapFragment;
        private LocationManager _locationManager;
		private Marker _newYorkMarker;
		private GroundOverlay _chicagoOverlay;

		#region Step 3a - define LatLng Constants
        // TODO: Step 3a - add lat/long position constants to use
        private static readonly LatLng Location_NewYork = new LatLng(40.714353, -74.005973);
        private static readonly LatLng Location_Chicago = new LatLng(41.878114, -87.629798);
        private static readonly LatLng Location_Xamarin = new LatLng(37.797679, -122.401816);
        private static readonly LatLng Location_Atlanta = new LatLng(33.755, -84.39);
		#endregion

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
        }

		protected override void OnResume ()
		{
			base.OnResume ();

			// Get a handle on the map element
			_mapFragment = FragmentManager.FindFragmentById(Resource.Id.map) as MapFragment;
			_map = _mapFragment.Map;

			// Set the map type 
			_map.MapType = GoogleMap.MapTypeNormal;

			// show user location
			_map.MyLocationEnabled = true;

			// setup a location manager
			_locationManager = GetSystemService(Context.LocationService) as LocationManager;

			#region Step 3b - Add Markers to the Google Map object
			// TODO: Step 3b - Add points on the map
			MarkerOptions marker1 = new MarkerOptions()
				.SetPosition(Location_Xamarin)
				.SetTitle("Xamarin")
				.InvokeIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueBlue));
			_map.AddMarker(marker1);

			MarkerOptions marker2 = new MarkerOptions()
			    .SetPosition(Location_Atlanta)
			    .SetTitle("Atlanta, GA")
			    .InvokeIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueRed));
			var myMarker = _map.AddMarker(marker2);
			#endregion


			// TODO: Step 3c - Add custom marker images on the map
            AddMonkeyMarkersToMap();

			// TODO: Step 3d - Add custom arrow callout on map
            AddInitialNewYorkBarToMap();

			// TODO: Step 3e - Add custom overlay image on the map
            PositionChicagoGroundOverlay(Location_Chicago);

			// use a generic location provider instead
			Criteria locationCriteria = new Criteria();
			locationCriteria.Accuracy = Accuracy.Coarse;
			locationCriteria.PowerRequirement = Power.Medium;

			var locationProvider = _locationManager.GetBestProvider(locationCriteria, true);
			if (locationProvider != null)
			{
				_locationManager.RequestLocationUpdates(locationProvider, 2000, 1, this);
			} else
			{
				Log.Info("error", "Best provider is not available. Does the device have location services enabled?");
			}
		}            

		#region Step 3c - Add custom Markers to Map
		// TODO: Step 3c - Add custom marker images on the map
		void AddMonkeyMarkersToMap()
		{
		    LatLng[] locationForCustomIconMarkers = new[]
		    {
		        new LatLng(36.741773, -84.004986),
		        new LatLng(37.051696, -83.545667),
		        new LatLng(37.311197, -82.902646),
                new LatLng(49.25, -123.1),
		    };

          BitmapDescriptor icon = BitmapDescriptorFactory.FromResource(Resource.Drawable.monkey);

		    for (int i = 0; i < locationForCustomIconMarkers.Length; i++)
		    {
		        MarkerOptions markerOptions = new MarkerOptions()
		            .SetPosition(locationForCustomIconMarkers[i])
		            .InvokeIcon(icon)
		            .SetSnippet(String.Format("This is marker #{0}.", i + 1))
		            .SetTitle(String.Format("Marker {0}", i + 1));
		        _map.AddMarker(markerOptions);
		    }
		}
		#endregion

		#region Step 3d - Add custom callout
		// TODO: Step 3d - Add custom arrow callout on map
		void AddInitialNewYorkBarToMap()
		{
		    MarkerOptions markerOptions = new MarkerOptions()
		        .SetSnippet("Click me to visit New York.")
		        .SetPosition(Location_NewYork)
		        .SetTitle("Goto New York");
		    _newYorkMarker = _map.AddMarker(markerOptions);
		    _newYorkMarker.ShowInfoWindow();
		}
		#endregion

		#region Step 3e - Add custom overlay

		// TODO: Step 3e - Add custom overlay image on the map
		void PositionChicagoGroundOverlay(LatLng position)
		{
		    if (_chicagoOverlay == null)
		    {
		        BitmapDescriptor image = BitmapDescriptorFactory.FromResource(Resource.Drawable.monkey);
		        
				GroundOverlayOptions groundOverlayOptions = new GroundOverlayOptions()
		            .Position(position, 150000, 200000)
		            .InvokeImage(image);
		        _chicagoOverlay = _map.AddGroundOverlay(groundOverlayOptions);
		    }
		    else
		    {
		        _chicagoOverlay.Position = Location_Chicago;
		    }
		}
		#endregion

        // implement ILocationListener
        public void OnLocationChanged(Location location)
        {
            _map.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(new Android.Gms.Maps.Model.LatLng(location.Latitude, location.Longitude), 5.0f));
        }

        public void OnProviderDisabled(string provider)
        {
        }

        public void OnProviderEnabled(string provider)
        {
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
        }

    }
}


