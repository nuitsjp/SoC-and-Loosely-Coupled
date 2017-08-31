using System;

namespace HatPepper.Integrations
{
    public interface IGeoCoordinator
    {
        Location GetCurrent(TimeSpan timeout);
    }
}