using Asteroids.Runtime.Asteroids.Components;
using Asteroids.Runtime.CellLists.Components;
using Asteroids.Runtime.GameTime.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Asteroids.Runtime.Asteroids.Systems
{
    public class AsteroidsMovementSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<Time> _time = default;
        private readonly EcsFilterInject<Inc<Asteroid, Transform>> _filter = default;
        private readonly EcsPoolInject<Asteroid> _asteroids = default;
        private readonly EcsPoolInject<Transform> _transforms = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var asteroid = ref _asteroids.Value.Get(entity);
                ref var transform = ref _transforms.Value.Get(entity);

                transform.Position += asteroid.MovementDirection * asteroid.Speed * _time.Value.DeltaTime;
            }
        }
    }
}