using Asteroids.Runtime.Enemies.Components;
using Asteroids.Runtime.HP.Components;
using Asteroids.Runtime.Score;
using Asteroids.Runtime.Ships.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Asteroids.Runtime.Enemies.Systems
{
    public class EnemyDeathSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<GameScore> _score = default;
        private readonly EcsFilterInject<Inc<Enemy, Health, HealthOver>, Exc<Destroy>> _filter = default;
        private readonly EcsPoolInject<Destroy> _destroy = default;
        private readonly EcsPoolInject<Enemy> _enemies = default;
        private readonly EcsPoolInject<Player> _player = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var enemy = ref _enemies.Value.Get(entity);

                if (_player.Value.Has(enemy.LastTakenDamageFrom))
                {
                    _score.Value.Add(enemy.RewardForKill);
                }
                
                _destroy.Value.Add(entity);
            }
        }
    }
}