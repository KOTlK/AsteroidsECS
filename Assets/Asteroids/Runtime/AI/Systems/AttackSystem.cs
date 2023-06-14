using Asteroids.Runtime.AI.Components;
using Asteroids.Runtime.Application;
using Asteroids.Runtime.Enemies.Components;
using Asteroids.Runtime.HP.Components;
using Asteroids.Runtime.Ships.Components;
using Asteroids.Runtime.Weapons.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Transform = Asteroids.Runtime.CellLists.Components.Transform;

namespace Asteroids.Runtime.AI.Systems
{
    public class AttackSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<Config> _config = default;
        private readonly EcsFilterInject<Inc<Ship, Enemy, Transform, Attack, ShipInput, Weapon, WeaponInput>> _filter = default;
        private readonly EcsPoolInject<Attack> _attacks = default;
        private readonly EcsPoolInject<Transform> _transforms = default;
        private readonly EcsPoolInject<Enemy> _enemies = default;
        private readonly EcsPoolInject<Ship> _ships = default;
        private readonly EcsPoolInject<ShipInput> _shipInputs = default;
        private readonly EcsPoolInject<Weapon> _weapons = default;
        private readonly EcsPoolInject<WeaponInput> _weaponInputs = default;
        private readonly EcsPoolInject<Health> _healths = default;
        private readonly EcsPoolInject<Patrol> _patrols = default;
        private readonly EcsPoolInject<Follow> _follow = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var transform = ref _transforms.Value.Get(entity);
                ref var attack = ref _attacks.Value.Get(entity);
                ref var weaponInput = ref _weaponInputs.Value.Get(entity);
                
                if (_healths.Value.Has(attack.TargetEntity) == false)
                {
                    ref var patrol = ref _patrols.Value.Add(entity);
                    patrol.Point = _config.Value.CellListsConfig.RandomPointInside();
                    _attacks.Value.Del(entity);
                    weaponInput.Shooting = false;
                    continue;
                }

                ref var targetTransform = ref _transforms.Value.Get(attack.TargetEntity);
                ref var targetHealth = ref _healths.Value.Get(attack.TargetEntity);
                ref var weapon = ref _weapons.Value.Get(entity);

                if (targetHealth.Current <= targetHealth.Min)
                {
                    ref var patrol = ref _patrols.Value.Add(entity);
                    patrol.Point = _config.Value.CellListsConfig.RandomPointInside();
                    _attacks.Value.Del(entity);
                    weaponInput.Shooting = false;
                    continue;
                }

                var directionToTarget = targetTransform.Position - transform.Position;

                if (directionToTarget.sqrMagnitude > weapon.ShootRange * weapon.ShootRange)
                {
                    ref var follow = ref _follow.Value.Add(entity);
                    follow.TargetEntity = attack.TargetEntity;
                    _attacks.Value.Del(entity);
                    weaponInput.Shooting = false;
                    continue;
                }

                ref var enemy = ref _enemies.Value.Get(entity);
                ref var shipInput = ref _shipInputs.Value.Get(entity);

                if (directionToTarget.sqrMagnitude < enemy.SafeDistance * enemy.SafeDistance)
                {
                    shipInput.MovementDirection = -directionToTarget.normalized;
                }
                else
                {
                    shipInput.MovementDirection = directionToTarget.normalized;
                }

                //calculate shoot direction
                ref var targetShip = ref _ships.Value.Get(attack.TargetEntity);

                var distance = Vector3.Distance(targetTransform.Position, transform.Position);
                var targetPosition =
                    targetTransform.Position + targetShip.Velocity * (distance / weapon.ProjectileSpeed);

                var lookDirection = (targetPosition - transform.Position).normalized;

                shipInput.LookDirection = lookDirection;
                weaponInput.Shooting = true;
            }
        }
    }
}