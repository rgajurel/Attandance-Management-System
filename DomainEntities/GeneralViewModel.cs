using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
   public class GeneralViewModel<T>
    {
        public T Model { get; set; }
        public LoginDetails LoginInfo { get; set; }
        public string LatLong { get; set; }
        public GeoAddress LocationDetails { get; set; }
    }

 
    public class GeoAddress
    {
        public string Label { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
    public class LoginDetails
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string ID { get; set; }
        public string EmployeeID { get; set; }
        public string OrganisationID { get; set; }
        public string UserDeviceID { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public DeviceType devicetype { get; set; }

    }

    public class MetaInfo
    {
        public DateTime Timestamp { get; set; }
        public string NextPageInformation { get; set; }
    }

    public class MatchQuality
    {
        public double Country { get; set; }
        public double State { get; set; }
        public double County { get; set; }
        public double City { get; set; }
        public double District { get; set; }
        public List<double?> Street { get; set; }
    }

    public class DisplayPosition
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class TopLeft
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class BottomRight
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class MapView
    {
        public TopLeft TopLeft { get; set; }
        public BottomRight BottomRight { get; set; }
    }

    public class AdditionalData
    {
        public string value { get; set; }
        public string key { get; set; }
    }

    public class Address
    {
        public string Label { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string County { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public List<AdditionalData> AdditionalData { get; set; }
        public string Street { get; set; }
    }

    public class MapReference
    {
        public string ReferenceId { get; set; }
        public double Spot { get; set; }
        public string SideOfStreet { get; set; }
        public string CountryId { get; set; }
        public string StateId { get; set; }
        public string CountyId { get; set; }
        public string CityId { get; set; }
        public string DistrictId { get; set; }
    }

    public class Location
    {
        public string LocationId { get; set; }
        public string LocationType { get; set; }
        public DisplayPosition DisplayPosition { get; set; }
        public MapView MapView { get; set; }
        public Address Address { get; set; }
        public MapReference MapReference { get; set; }
    }

    public class Result
    {
        public double Relevance { get; set; }
        public double Distance { get; set; }
        public string MatchLevel { get; set; }
        public MatchQuality MatchQuality { get; set; }
        public Location Location { get; set; }
    }

    public class View
    {
        public string _type { get; set; }
        public int ViewId { get; set; }
        public List<Result> Result { get; set; }
    }

    public class Response
    {
        public MetaInfo MetaInfo { get; set; }
        public List<View> View { get; set; }
    }


}
