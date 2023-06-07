using Asteroids.Runtime.GameTime.Services;
using Asteroids.Runtime.Weapons.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Asteroids.Runtime.Weapons.Systems
{
    public class ReloadingSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<Time> _time = default;
        private readonly EcsFilterInject<Inc<Weapon, Reload>> _filter = default;
        private readonly EcsPoolInject<Reload> _reloads = default;
        private readonly EcsPoolInject<Weapon> _weapons = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var reload = ref _reloads.Value.Get(entity);
                reload.TimeLeft -= _time.Value.DeltaTime;

                if (reload.TimeLeft <= 0)
                {
                    _reloads.Value.Del(entity);
                    ref var weapon = ref _weapons.Value.Get(entity);
                    weapon.RoundsLeft = weapon.RoundsMax;
                }
            }
        }
    }
}