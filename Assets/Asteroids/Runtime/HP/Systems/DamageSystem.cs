using Asteroids.Runtime.HP.Components;
using Asteroids.Runtime.Utils.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Asteroids.Runtime.HP.Systems
{
    public class DamageSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<Health, Buffer<Damage>>, Exc<HealthOver>> _filter = default;
        private readonly EcsPoolInject<Health> _healths = default;
        private readonly EcsPoolInject<Buffer<Damage>> _buffers = default;
        private readonly EcsPoolInject<HealthOver> _healthOver = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var health = ref _healths.Value.Get(entity);
                ref var damageBuffer = ref _buffers.Value.Get(entity);

                while (damageBuffer.Count > 0)
                {
                    var damage = damageBuffer.Remove();
                    health.Current -= damage.Amount;

                    if (health.Current <= health.Min)
                    {
                        damageBuffer.Clear();
                        _healthOver.Value.Add(entity);
                    }
                }
            }
        }
    }
}