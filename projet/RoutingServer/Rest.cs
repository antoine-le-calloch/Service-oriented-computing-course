using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace RoutingServer
{
    public class Rest : IRest
    {
        static readonly HttpClient client = new HttpClient();
        public Stations stations = new Stations();

        
        /// <summary>
        /// Returns the non-empty station closest respectively to the departure
        /// and the arrival. If the walk to these stations is longer than 
        /// the journey between departure and arrival, the method returns null.
        /// </summary>
        public string FindNearestStation(string coord1, string coord2)
        {
            Position nearCoord1 = FindTheNearestStation(Position.setWithTab(coord1.Split(' '))).position;
            Position nearCoord2 = FindTheNearestStation(Position.setWithTab(coord2.Split(' '))).position;
            return coord1.Replace(' ', '+') + "/" + 
                    nearCoord1.longitude + "+" + nearCoord1.latitude + "/" +
                    nearCoord2.longitude + "+" + nearCoord2.latitude + "/" +
                    coord2.Replace(' ', '+');
        }

        /// <summary>
        /// Returns the non-empty station closest respectively to the departure
        /// and the arrival. If the walk to these stations is longer than 
        /// the journey between departure and arrival, the method returns null.
        /// </summary>
        public Station FindTheNearestStation(Position coord)
        {
            double dist;
            double minDist = GetDistanceFrom2GpsCoordinates(coord, stations.getStations()[0].position);
            Station nearestStation = stations.getStations()[0];
            foreach (Station station in stations.getStations())
            {
                if((dist = GetDistanceFrom2GpsCoordinates(coord, station.position)) < minDist)
                {
                    minDist = dist;
                    nearestStation = station;
                }
            }
            return nearestStation;
        }

        /// <summary>
        /// Returns the latitude and longitude of an address as a string
        /// </summary>
        public string GetCoord(string address)
        {
            string api_key = "5b3ce3597851110001cf6248d80ebbb9aefa4fd08b3cac7ddd1b10b5";
            string coords = "";
            foreach (string eachAdress in address.Split('/'))
            {
                HttpResponseMessage response = client.GetAsync($"https://api.openrouteservice.org/geocode/search?api_key={api_key}&text={eachAdress}").Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                Geocoding geocoding = JsonConvert.DeserializeObject<Geocoding>(responseBody);
                coords += geocoding.features[0].geometry.coordinates[0] + "+" + geocoding.features[0].geometry.coordinates[1] + "/";
            }
            return coords;
        }

        /// <summary>
        /// Returns the distance between 2 coordinates passed as a parameter.
        /// </summary>
        public double GetDistanceFrom2GpsCoordinates(Position coord1, Position coord2)
        {
            // Radius of the earth in km
            double earthRadius = 6371;
            double lat1 = Convert.ToDouble(coord1.latitude.Replace(".", ","));
            double long1 = Convert.ToDouble(coord1.longitude.Replace(".", ","));
            double lat2 = Convert.ToDouble(coord2.latitude.Replace(".", ","));
            double long2 = Convert.ToDouble(coord2.longitude.Replace(".", ","));
            double dLat = deg2rad(lat2 - lat1);
            double dLon = deg2rad(long2 - long1);
            double a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
            ;
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = earthRadius * c; // Distance in km
            return d;
        }

        public double deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
        }
    }
}