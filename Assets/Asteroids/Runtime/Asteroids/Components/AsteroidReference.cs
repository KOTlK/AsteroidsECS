using System;
using Asteroids.Runtime.Asteroids.Mono;

namespace Asteroids.Runtime.Asteroids.Components
{
    public struct AsteroidReference : IDisposable
    {
        public AsteroidView View;

        public void Dispose()
        {
            View.Dispose();
        }
    }
}