using Asteroids.Runtime.GameTime.Services;
using Asteroids.Runtime.Weapons.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Asteroids.Runtime.Weapons.Systems
{
    public class ShootDelaySystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<Time> _time = default;
        private readonly EcsFilterInject<Inc<Weapon, Delay>> _filter = default;
        private readonly EcsPoolInject<Delay> _delays = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var delay = ref _delays.Value.Get(entity);

                delay.TimeLeft -= _time.Value.DeltaTime;

                if (delay.TimeLeft <= 0)
                {
                    _delays.Value.Del(entity);
                }
            }
        }
    }
}