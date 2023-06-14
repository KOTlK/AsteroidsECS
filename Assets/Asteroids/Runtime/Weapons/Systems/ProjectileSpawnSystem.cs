using Asteroids.Runtime.CellLists.Components;
using Asteroids.Runtime.Collisions.Components;
using Asteroids.Runtime.Transforms.Components;
using Asteroids.Runtime.Utils;
using Asteroids.Runtime.Weapons.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Transform = Asteroids.Runtime.CellLists.Components.Transform;

namespace Asteroids.Runtime.Weapons.Systems
{
    public class ProjectileSpawnSystem : IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsWorldInject _eventsWorld = Constants.EventsWorldName;
        private readonly EcsFilterInject<Inc<SpawnProjectile>> _filter = Constants.EventsWorldName;
        private readonly EcsPoolInject<Projectile> _projectiles = default;
        private readonly EcsPoolInject<AABBCollider> _colliders = default;
        private readonly EcsPoolInject<Transform> _transforms = default;
        private readonly EcsPoolInject<InsertInCellLists> _cellLists = default;
        private readonly EcsPoolInject<SpawnProjectile> _commands = Constants.EventsWorldName;
        private readonly EcsPoolInject<ProjectileReference> _references = default;
        private readonly EcsPoolInject<TransformReference> _transformReferences = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var command = ref _commands.Value.Get(entity);
                var projectileEntity = _world.Value.NewEntity();
                ref var projectile = ref _projectiles.Value.Add(projectileEntity);
                ref var collider = ref _colliders.Value.Add(projectileEntity);
                ref var transform = ref _transforms.Value.Add(projectileEntity);
                ref var reference = ref _references.Value.Add(projectileEntity);
                ref var transformReference = ref _transformReferences.Value.Add(projectileEntity);
                var instance = Object.Instantiate(command.Prefab, command.Position, Quaternion.identity);
                var direction = command.Direction;
                var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
                _cellLists.Value.Add(projectileEntity);

                projectile.Direction = command.Direction;
                projectile.Owner = command.Owner;
                projectile.Damage = command.Damage;
                projectile.Speed = command.Speed;
                projectile.MaxDistance = command.Distance;
                projectile.DistancePassed = 0;
                collider.Layer = PhysicsLayer.Projectile;
                collider.TargetLayers = command.Targets;
                collider.Size = command.Size;
                transform.Position = command.Position;
                transform.Rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.Cell = int.MinValue;
                reference.View = instance;
                transformReference.Transform = instance.transform;

                _eventsWorld.Value.DelEntity(entity);
            }
        }
    }
}