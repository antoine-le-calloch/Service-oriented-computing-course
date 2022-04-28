using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace RoutingServer
{
    public class Soap : ISoap
    {
        static readonly HttpClient client = new HttpClient();
        public Stations stations = new Stations();

        /// <summary>
        /// Returns the non-empty station closest respectively to the departure
        /// and the arrival. If the walk to these stations is longer than 
        /// the journey between departure and arrival, the method returns null.
        /// </summary>
        public string FindPath(string startLat, string startLong, string endLat, string endLong)
        {
            /*Position startCoord = new Position(startLat, startLong);
            Position endCoord = new Position(endLat, endLong);
            Position startStationPosition = FindTheNearestStation(startCoord).position;
            Position endStationPosition = FindTheNearestStation(endCoord).position;
            return startLong + "+" + startLat + "-" +
                startStationPosition.longitude + "+" + startStationPosition.latitude + "-" +
                endStationPosition.longitude + "+" + endStationPosition.latitude + "-" +
                endLong + "+" + endLat;*/
            return null;
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

                if ((dist = GetDistanceFrom2GpsCoordinates(coord, station.position)) < minDist)
                {
                    minDist = dist;
                    nearestStation = station;
                }
            }
            return nearestStation;
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

        /// <summary>
        /// This class makes it possible to recover then to store
        /// the list of the stations existing in the city at the initialization.
        /// </summary>
        public class Stations
        {
            private List<Station> stations;

            /// <summary>
            /// The constructor recovers, once and for all,
            /// all the stations present in the city
            /// </summary>
            public Stations()
            {
                string api_key = "cfe9a0679020b361f73a04113fae576b56ae6ed8";
                HttpResponseMessage response = client.GetAsync($"https://api.jcdecaux.com/vls/v3/stations?contract=toulouse&apiKey={api_key}").Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                stations = JsonConvert.DeserializeObject<List<Station>>(responseBody);
            }

            /// <summary>
            /// This method allows you to retrieve the list of stations
            /// which is stored in the Stations class
            /// </summary>
            public List<Station> getStations()
            {
                return stations;
            }
        }
    }
}