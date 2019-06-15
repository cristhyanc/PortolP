using BingMapsRESTToolkit;
using BingMapsRESTToolkit.Extensions;
using Portol.Common.DTO;
using Portol.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portol.Calculator.Map
{
    public class MapService : IMapService
    {
        private string _mapApikey;
        public MapService(string mapApikey)
        {
            _mapApikey = mapApikey;
        }

        public async Task<double> CalculateDistance(AddressDto pickup, AddressDto dropoff)
        {

            //var distanceRequest = new DistanceMatrixRequest()
            //{
            //    BingMapsKey = _mapApikey,
            //    Origins = new List<SimpleWaypoint>() { new SimpleWaypoint(pickup.FullAddress) },
            //     Destinations  = new List<SimpleWaypoint>() { new SimpleWaypoint(dropoff.FullAddress) },
            //      DistanceUnits=DistanceUnitType.Kilometers,
            //      TravelMode=TravelModeType.Driving
            //};

            //var result =await  distanceRequest.Execute();

            var routeRequest = new RouteRequest()
            {

                WaypointOptimization = TspOptimizationType.TravelDistance,
                RouteOptions = new RouteOptions()
                {
                    TravelMode = TravelModeType.Driving,
                    Optimize = RouteOptimizationType.Distance,
                    RouteAttributes = new List<RouteAttributeType>()
                        {
                            RouteAttributeType.RoutePath
                        }
                },

                //When straight line distances are used, the distance matrix API is not used, so a session key can be used.
                BingMapsKey = _mapApikey,

            };

            routeRequest.Waypoints = new List<SimpleWaypoint>();
            SimpleWaypoint waypoint = new SimpleWaypoint(pickup.FullAddress);
            routeRequest.Waypoints.Add(waypoint);

            waypoint = new SimpleWaypoint(dropoff.FullAddress);
            routeRequest.Waypoints.Add(waypoint);

            var response = await routeRequest.Execute();

            if (response != null && response.ResourceSets != null && response.ResourceSets.Length > 0 &&
               response.ResourceSets[0].Resources != null && response.ResourceSets[0].Resources.Length > 0
               && response.ResourceSets[0].Resources[0] is Route)
            {
                var route = response.ResourceSets[0].Resources[0] as Route;
                if (route.RouteLegs?.Length > 0)
                {
                    return route.RouteLegs.Average(x => x.TravelDistance);
                }
                else
                {
                    return route.TravelDistance;
                }

            }


            return 0;
        }
    }
}
