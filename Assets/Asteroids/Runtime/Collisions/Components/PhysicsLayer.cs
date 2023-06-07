using System;

namespace Asteroids.Runtime.Collisions.Components
{
    [Flags]
    public enum PhysicsLayer
    {
        None = 1,
        All = 2,
        Player = 4,
        Enemy = 8,
        Asteroid = 16,
        Projectile = 32
    }
}