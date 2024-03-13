namespace LongLatTestApp;
using GoogleMaps.LocationServices;

// my original way: not sure why i cant just use this D: I guess you need an interface to do the mock testing??
// public class NuGetWay
// {
//     private GoogleLocationService gls; 

//     public NuGetWay(string apiKey)
//     {
//         gls = new GoogleLocationService(apiKey);
//     }

//     public string GetLongLatNuGetWay(string address)
//     {

//         try
//         {
//             var latlong = gls.GetLatLongFromAddress(address);
//             var latitude = latlong.Latitude;
//             var longitude = latlong.Longitude;
//             return $"Address ({address}) is at {latitude},{longitude}";
//         }

//         catch (System.Net.WebException ex)
//         {
//             return $"Google Maps API Error: {ex.Message}";
//         }
//     }
// }


public class CustomMapPoint // this code below works with the unit test
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public interface IMapLocationService
{
    CustomMapPoint GetLatLongFromAddress(string address);
}

public class GoogleLocationServiceWrapper : IMapLocationService
{
    private readonly GoogleLocationService _gls;

    public GoogleLocationServiceWrapper(string apiKey)
    {
        _gls = new GoogleLocationService(apiKey);
    }

    public CustomMapPoint GetLatLongFromAddress(string address)
    {
        var location = _gls.GetLatLongFromAddress(address);
        return new CustomMapPoint() { Latitude = location.Latitude, Longitude = location.Longitude };
        // MapPoint vs CustomMapPoint ?? 
    }
}

public class NuGetWay
{
    private readonly IMapLocationService _locationService;

    public NuGetWay(IMapLocationService locationService)
    {
        _locationService = locationService;
    }

    public string GetLongLatNuGetWay(string address)
    {
        try
        {
            var latlong = _locationService.GetLatLongFromAddress(address);
            return $"Address ({address}) is at {latlong.Latitude},{latlong.Longitude}";
        }
        catch (Exception ex)
        {
            return $"Error retrieving coordinates: {ex.Message}";
        }
    }
}








