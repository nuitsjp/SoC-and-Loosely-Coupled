using System;

namespace Nuits.System.Device.Location
{
    public interface IGeoCoordinator
    {
        Location GetCurrent(TimeSpan timeout);
    }
}