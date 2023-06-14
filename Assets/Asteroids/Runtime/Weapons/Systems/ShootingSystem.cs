using Asteroids.Runtime.CellLists.Components;
using Asteroids.Runtime.Utils;
using Asteroids.Runtime.Weapons.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Asteroids.Runtime.Weapons.Systems
{
    public class ShootingSystem : IEcsRunSystem
    {
        private readonly EcsWorldInject _eventsWorld = Constants.EventsWorldName;
        private readonly EcsFilterInject<Inc<Weapon, WeaponInput, Transform>, Exc<Reload, Delay>> _filter = default;
        private readonly EcsPoolInject<Weapon> _weapons = default;
        private readonly EcsPoolInject<WeaponInput> _inputs = default;
        private readonly EcsPoolInject<Delay> _delays = default;
        private readonly EcsPoolInject<Reload> _reloads = default;
        private readonly EcsPoolInject<SpawnProjectile> _spawnProjectileCommands = Constants.EventsWorldName;
        private readonly EcsPoolInject<Transform> _transforms = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var weapon = ref _weapons.Value.Get(entity);
                ref var input = ref _inputs.Value.Get(entity);
                ref var transform = ref _transforms.Value.Get(entity);

                if (input.Shooting)
                {
                    var commandEntity = _eventsWorld.Value.NewEntity();

                    ref var command = ref _spawnProjectileCommands.Value.Add(commandEntity);
                    command.Damage = weapon.Damage;
                    command.Position = transform.Position + input.LookDirection.normalized * weapon.TransformOffset;
                    command.Direction = input.LookDirection;
                    command.Targets = weapon.Targets;
                    command.Prefab = weapon.ProjectilePrefab;
                    command.Speed = weapon.ProjectileSpeed;
                    command.Distance = weapon.ShootRange;
                    command.Owner = entity;
                    command.Size = weapon.ProjectileSize;

                    weapon.RoundsLeft--;

                    if (weapon.RoundsLeft == 0)
                    {
                        ref var reloading = ref _reloads.Value.Add(entity);
                        reloading.TimeLeft = weapon.ReloadTime;
                    }
                    else
                    {
                        ref var delay = ref _delays.Value.Add(entity);
                        delay.TimeLeft = weapon.ShootDelay;
                    }
                } 
                else if (input.Reloading)
                {
                    ref var reloading = ref _reloads.Value.Add(entity);
                    reloading.TimeLeft = weapon.ReloadTime;
                }
            }
        }
    }
}