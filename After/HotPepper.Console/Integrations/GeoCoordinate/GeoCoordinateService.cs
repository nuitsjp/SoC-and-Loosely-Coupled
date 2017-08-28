using System;
using System.Device.Location;
using System.Threading;

namespace HotPepper.Console.Integrations.GeoCoordinate
{
    public class GeoCoordinateService : IGeoCoordinateService
    {
        public Position GetGurrentPosition(TimeSpan timeout)
        {
            GeoPosition<System.Device.Location.GeoCoordinate> position;
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
                position = watcher.Position;
                watcher.Stop();
            }

            if (position == null) return null;

            return new Position
            {
                Latitude = position.Location.Latitude,
                Longitude = position.Location.Longitude
            };
        }
    }
}
