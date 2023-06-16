using Asteroids.Runtime.Application;
using Asteroids.Runtime.Ships.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Asteroids.Runtime.View.Systems
{
    public class BackgroundMovementSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<Config> _config = default;
        private readonly EcsFilterInject<Inc<Ship, Player>> _filter = default;
        private readonly EcsPoolInject<Ship> _ships = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var ship = ref _ships.Value.Get(entity);

                _config.Value.Background.Move(ship.Velocity.normalized, Mathf.Lerp(0, _config.Value.MaxBackgroundSpeed, ship.Velocity.magnitude / ship.MaxSpeed));
            }
        }
    }
}