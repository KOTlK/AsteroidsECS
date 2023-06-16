using Asteroids.Runtime.Asteroids.Components;
using Asteroids.Runtime.CellLists.Components;
using Asteroids.Runtime.HP.Components;
using Asteroids.Runtime.Utils;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Asteroids.Runtime.Asteroids.Systems
{
    public class AsteroidsDestroySystem : IEcsRunSystem, IEcsDestroySystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsWorldInject _eventsWorld = Constants.EventsWorldName;
        private readonly EcsFilterInject<Inc<Asteroid, AsteroidReference, Destroy, Transform>> _filter = default;
        private readonly EcsPoolInject<AsteroidReference> _references = default;
        private readonly EcsPoolInject<Transform> _transforms = default;
        private readonly EcsPoolInject<RemoveFromCellLists> _removeCommands = Constants.EventsWorldName;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var reference = ref _references.Value.Get(entity);
                ref var transform = ref _transforms.Value.Get(entity);
                reference.Dispose();

                var removeEntity = _eventsWorld.Value.NewEntity();
                ref var removeCommand = ref _removeCommands.Value.Add(removeEntity);
                removeCommand.TransformEntity = entity;
                removeCommand.CellEntity = transform.Cell;
                _world.Value.DelEntity(entity);
            }
        }

        private readonly EcsFilterInject<Inc<AsteroidReference>> _destroyFilter = default;
        public void Destroy(IEcsSystems systems)
        {
            foreach (var entity in _destroyFilter.Value)
            {
                ref var reference = ref _references.Value.Get(entity);
                reference.Dispose();
            }
        }
    }
}