using Asteroids.Runtime.AI.Components;
using Asteroids.Runtime.Application;
using Asteroids.Runtime.CellLists.Components;
using Asteroids.Runtime.Enemies.Components;
using Asteroids.Runtime.Ships.Components;
using Asteroids.Runtime.Weapons.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Asteroids.Runtime.AI.Systems
{
    public class FollowSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<Config> _config = default;
        private readonly EcsFilterInject<Inc<Enemy, Ship, ShipInput, Transform, Follow>> _filter = default;
        private readonly EcsPoolInject<Transform> _transforms = default;
        private readonly EcsPoolInject<Follow> _follow = default;
        private readonly EcsPoolInject<ShipInput> _inputs = default;
        private readonly EcsPoolInject<Enemy> _enemies = default;
        private readonly EcsPoolInject<Patrol> _patrols = default;
        private readonly EcsPoolInject<Attack> _attacks = default;
        private readonly EcsPoolInject<Weapon> _weapons = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var follow = ref _follow.Value.Get(entity);
                ref var transform = ref _transforms.Value.Get(entity);
                ref var targetTransform = ref _transforms.Value.Get(follow.TargetEntity);
                ref var input = ref _inputs.Value.Get(entity);
                ref var enemy = ref _enemies.Value.Get(entity);
                ref var weapon = ref _weapons.Value.Get(entity);
                var direction = targetTransform.Position - transform.Position;
                var normalizedDirection = direction.normalized;
                
                if(direction.sqrMagnitude >= enemy.FollowRange * enemy.FollowRange)
                {
                    _follow.Value.Del(entity);
                    ref var patrol = ref _patrols.Value.Add(entity);
                    patrol.Point = _config.Value.CellListsConfig.RandomPointInside();
                    continue;
                } 
                
                if (direction.sqrMagnitude <= weapon.ShootRange * weapon.ShootRange)
                {
                    ref var attack = ref _attacks.Value.Add(entity);
                    attack.TargetEntity = follow.TargetEntity;
                    _follow.Value.Del(entity);
                    continue;
                }

                input.MovementDirection = normalizedDirection;
                input.LookDirection = direction.normalized;
                
            }
        }
    }
}