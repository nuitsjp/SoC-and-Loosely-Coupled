using System;
using System.Device.Location;
using System.Threading;

namespace HatPepper.Integrations
{
    public class GeoCoordinator : IGeoCoordinator
    {
        public Location GetCurrent(TimeSpan timeout)
        {
            Location current;
            using (var watcher = new GeoCoordinateWatcher())
            {
                // PositionChangedイベントを監視し、イベント発生時に待機中のスレッドを再開する
                watcher.PositionChanged += (sender, eventArgs) =>
                {
                    Monitor.Enter(this);
                    try
                    {
                        Monitor.PulseAll(this);
                    }
                    finally
                    {
                        Monitor.Exit(this);
                    }
                };

                Monitor.Enter(this);
                try
                {
                    // GeoCoordinateWatcherを起動し、PositionChangedイベントが発生するか
                    // タイムアウトまで待機する
                    watcher.Start();
                    Monitor.Wait(this, timeout);
                }
                finally
                {
                    Monitor.Exit(this);
                }

                if (watcher.Position?.Location != null
                    && !double.IsNaN(watcher.Position.Location.Latitude)
                    && !double.IsNaN(watcher.Position.Location.Longitude))
                {
                    current = new Location
                    {
                        Latitude = watcher.Position.Location.Latitude,
                        Longitude = watcher.Position.Location.Longitude
                    };
                }
                else
                {
                    current = null;
                }
                watcher.Stop();
            }
            return current;
        }
    }
}
