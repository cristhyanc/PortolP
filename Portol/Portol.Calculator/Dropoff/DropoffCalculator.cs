using Portol.Common;
using Portol.Common.DTO;
using Portol.Common.Helper;
using Portol.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portol.Calculator.Dropoff
{
    public class DropoffCalculator : IDropoffCalculator
    {
        IMapService _mapService;
        public DropoffCalculator(IMapService mapService)
        {
            _mapService = mapService;
        }

        public async Task<decimal> EstimatePrice(MeasurementDto measurement, AddressDto pickup, AddressDto dropoff, List<VehiculeTypeDto> vehiculeTypes)
        {
            if (vehiculeTypes == null || vehiculeTypes.Count == 0)
            {
                throw new AppException(StringResources.VehiculeTypeNoAvailable);
            }

            double distance = await _mapService.CalculateDistance(pickup, dropoff);

            if (distance < 0.2)
            {
                throw new AppException(StringResources.NexToDestination);
            }

            var availableVehiculeTypes = vehiculeTypes.Where(x => x.MaximumDistance >= distance && x.MaximumHeight >= measurement.Height && x.MaximumLength >= measurement.Length &
                                                             x.MaximumWeight >= measurement.Weight & x.MaximumWidth >= measurement.Weight).ToList();

            if (availableVehiculeTypes?.Count > 0)
            {
                decimal result = 0;
                foreach (var item in availableVehiculeTypes)
                {
                    decimal totalCharge = 0;
                    decimal extraCharge = 0;
                    var volumenRange = item.Ranges.Where(x => x.RangeType == VehiculeRangeType.Volumen && x.MinimumValue <= measurement.Volume && x.MaximumValue >= measurement.Volume).FirstOrDefault();
                    var weightRange = item.Ranges.Where(x => x.RangeType == VehiculeRangeType.Weight && x.MinimumValue <= measurement.Weight && x.MaximumValue >= measurement.Weight).FirstOrDefault();

                    if (volumenRange?.CostPerExtraUnit > 0)
                    {
                        var volumenTobeCharged = volumenRange.MaximumValue - measurement.Volume;
                        var totalUnits = volumenTobeCharged / volumenRange.Unit;
                        extraCharge += totalUnits * volumenRange.CostPerExtraUnit;
                    }

                    if (weightRange?.CostPerExtraUnit > 0)
                    {
                        var weightTobeCharged = weightRange.MaximumValue - measurement.Weight;
                        var totalUnits = weightTobeCharged / weightRange.Unit;
                        extraCharge += totalUnits * weightRange.CostPerExtraUnit;
                    }

                    totalCharge = item.StartingFee + (item.CostPerkilometre * (decimal)distance) + extraCharge;
                    result += totalCharge;
                }

                return Math.Round((result / availableVehiculeTypes.Count), 2);
            }
            else
            {
                throw new AppException(StringResources.NoVehiculeTypeForParcel);
            }


        }
    }
}
