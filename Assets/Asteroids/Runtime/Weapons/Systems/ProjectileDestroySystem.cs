using Asteroids.Runtime.CellLists.Components;
using Asteroids.Runtime.HP.Components;
using Asteroids.Runtime.Utils;
using Asteroids.Runtime.Weapons.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Asteroids.Runtime.Weapons.Systems
{
    public class ProjectileDestroySystem : IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsWorldInject _eventsWorld = Constants.EventsWorldName;
        private readonly EcsFilterInject<Inc<Projectile, ProjectileReference, Destroy>> _filter = default;
        private readonly EcsPoolInject<ProjectileReference> _references = default;
        private readonly EcsPoolInject<RemoveFromCellLists> _removeCommands = Constants.EventsWorldName;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var reference = ref _references.Value.Get(entity);
                reference.Dispose();

                var removeEntity = _eventsWorld.Value.NewEntity();
                ref var removeCommand = ref _removeCommands.Value.Add(removeEntity);
                removeCommand.TransformEntity = entity;
                _world.Value.DelEntity(entity);
            }
        }
    }
}