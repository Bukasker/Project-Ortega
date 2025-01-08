namespace Mapbox.Unity.Location
{
	using System;
	using Mapbox.Unity.Utilities;
	using Mapbox.Utils;
	using UnityEngine;

	public class LocationArrayEditorLocationProvider : AbstractEditorLocationProvider
	{
		[SerializeField]
		[Geocode]
		string[] _latitudeLongitude;

		[SerializeField]
		[Range(0, 359)]
		float _heading;

		private int idx = -1;

		Vector2d LatitudeLongitude
		{
			get
			{
				idx++;
				if (idx >= _latitudeLongitude.Length) { idx = 0; }
				return Conversions.StringToLatLon(_latitudeLongitude[idx]);
			}
		}

		protected override void SetLocation()
		{
			_currentLocation.UserHeading = _heading;
			_currentLocation.LatitudeLongitude = LatitudeLongitude;
			_currentLocation.Accuracy = _accuracy;
			_currentLocation.Timestamp = UnixTimestampUtils.To(DateTime.UtcNow);
			_currentLocation.IsLocationUpdated = true;
			_currentLocation.IsUserHeadingUpdated = true;
		}

		// Nowa metoda do aktualizacji współrzędnych
		public void UpdateLocation(string latitude, string longitude)
		{
			// Aktualizujemy lokalizację w tablicy
			_latitudeLongitude = new string[] { latitude + "," + longitude };
			// Resetujemy indeks, by odczytać nową lokalizację przy następnym wywołaniu
			idx = -1;
			SetLocation();
		}
	}
}
