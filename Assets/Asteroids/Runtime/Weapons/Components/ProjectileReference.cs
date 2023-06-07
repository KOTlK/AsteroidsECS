using System;
using Asteroids.Runtime.Weapons.Mono;

namespace Asteroids.Runtime.Weapons.Components
{
    public struct ProjectileReference : IDisposable
    {
        public ProjectileView View;

        public void Dispose()
        {
            View.Dispose();
        }
    }
}