using System;
using Asteroids.Runtime.Enemies.Mono;

namespace Asteroids.Runtime.Enemies.Components
{
    public struct EnemyReference : IDisposable
    {
        public EnemyView View;

        public void Dispose()
        {
            View.Dispose();
        }
    }
}