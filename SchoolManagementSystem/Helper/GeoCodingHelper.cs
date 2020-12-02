using DomainEntities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;

namespace SchoolManagementSystem.Helper
{
    //public class GeoCodingHelper
    //{
    //    const string API_REVERSE_GEOCODE = "https://maps.googleapis.com/maps/api/geocode/xml?latlng={0}&sensor=false&key={1}";
    //    public GeoAddress GetAddressDetailsByLatLong(string latLong, string key)
    //    {
    //        WebRequest tRequest = WebRequest.Create(String.Format(API_REVERSE_GEOCODE, latLong, key));
    //        tRequest.Method = "get";
    //        GeoAddress address = new GeoAddress();
    //        try
    //        {

    //            var doc = XDocument.Load(String.Format(API_REVERSE_GEOCODE, latLong, key));
    //            var result = doc.Descendants("result").First();
    //            address.FormattedAddress = result.Descendants("formatted_address").First().Value;
    //            address.City = result.Descendants("address_component").First(x => x.Descendants("type").Any(y => y.Value == "locality")).Descendants("short_name").First()?.Value;
    //            address.State = result.Descendants("address_component").First(x => x.Descendants("type").Any(y => y.Value == "administrative_area_level_1")).Descendants("short_name").First()?.Value;
    //            address.ZipCode = result.Descendants("address_component").First(x => x.Descendants("type").Any(y => y.Value == "postal_code")).Descendants("short_name").First()?.Value;
    //            address.Country = result.Descendants("address_component").First(x => x.Descendants("type").Any(y => y.Value == "country")).Descendants("long_name").First()?.Value;
    //            return address;
    //        }
    //        catch
    //        {
    //            return address;
    //        }
    //    }
    //}

    public class GeoCodingHelper
    {

        const string API_REVERSE_GEOCODE = "https://reverse.geocoder.api.here.com/6.2/reversegeocode.xml?app_id={0}&app_code={1}&mode=retrieveAddresses&prox={2}";
       
        public GeoAddress GetAddressDetailsByLatLong(string latLong, string appid,string appcode)
        {
            
            WebRequest tRequest = WebRequest.Create(String.Format(API_REVERSE_GEOCODE, appid, appcode,latLong));
            tRequest.Method = "get";
          

            GeoAddress address = new GeoAddress();

            try
            {              
                var doc = XDocument.Load(String.Format(API_REVERSE_GEOCODE, appid, appcode, latLong));
                var tt = doc.Descendants("Address");
                address.Address = tt.Descendants("Label").First().Value;               
                address.City = tt.Descendants("City").First().Value;
                address.State = tt.Descendants("State").First().Value;
                address.PostalCode = doc.Descendants("PostalCode").First().Value;
               
                return address;
            }
            catch (Exception ex)
            {
                return address;
            }
           
        }
    }


}