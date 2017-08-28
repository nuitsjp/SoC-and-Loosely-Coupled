using System;

namespace HotPepper.Console.Integrations.GeoCoordinate
{
    public interface IGeoCoordinateService
    {
        Position GetGurrentPosition(TimeSpan timeout);
    }
}