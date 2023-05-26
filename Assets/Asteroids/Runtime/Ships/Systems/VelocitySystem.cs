using Asteroids.Runtime.GameTime.Services;
using Asteroids.Runtime.Ships.Components;
using Asteroids.Runtime.Transforms.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Asteroids.Runtime.Ships.Systems
{
    public class VelocitySystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<Ship, Transform>> _filter = default;
        private readonly EcsPoolInject<Ship> _ships = default;
        private readonly EcsPoolInject<Transform> _transforms = default;
        private readonly EcsCustomInject<Time> _time = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var ship = ref _ships.Value.Get(entity);
                ref var transform = ref _transforms.Value.Get(entity);

                transform.Position += ship.Velocity * _time.Value.DeltaTime;
            }
        }
    }
}