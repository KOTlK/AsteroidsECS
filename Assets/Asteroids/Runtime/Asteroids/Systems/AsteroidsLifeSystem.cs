using Asteroids.Runtime.Asteroids.Components;
using Asteroids.Runtime.GameTime.Services;
using Asteroids.Runtime.HP.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Asteroids.Runtime.Asteroids.Systems
{
    public class AsteroidsLifeSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<Time> _time = default;
        private readonly EcsFilterInject<Inc<Asteroid>, Exc<Destroy>> _filter = default;
        private readonly EcsPoolInject<Asteroid> _asteroids = default;
        private readonly EcsPoolInject<Destroy> _destroyPool = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var asteroid = ref _asteroids.Value.Get(entity);

                asteroid.LifeTime -= _time.Value.DeltaTime;

                if (asteroid.LifeTime <= 0)
                {
                    _destroyPool.Value.Add(entity);
                }
            }
        }
    }
}