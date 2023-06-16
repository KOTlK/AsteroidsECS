using System;

namespace Asteroids.Runtime.Ships.Components
{
    public struct ShipReference : IDisposable
    {
        public Mono.ShipView View;

        public void Dispose()
        {
            View.Dispose();
        }
    }
}