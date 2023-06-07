using Asteroids.Runtime.Asteroids.Components;
using Asteroids.Runtime.CellLists.Components;
using Asteroids.Runtime.HP.Components;
using Asteroids.Runtime.Utils;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Asteroids.Runtime.HP.Systems
{
    public class AsteroidsDestroySystem : IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsWorldInject _eventsWorld = Constants.EventsWorldName;
        private readonly EcsFilterInject<Inc<Asteroid, AsteroidReference, Destroy>> _filter = default;
        private readonly EcsPoolInject<AsteroidReference> _asteroidReferences = default;
        private readonly EcsPoolInject<RemoveFromCellLists> _removePool = Constants.EventsWorldName;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var reference = ref _asteroidReferences.Value.Get(entity);
                reference.Dispose();

                var removeEntity = _eventsWorld.Value.NewEntity();
                ref var removeCommand = ref _removePool.Value.Add(removeEntity);
                removeCommand.TransformEntity = entity;
                
                _world.Value.DelEntity(entity);
            }
        }
    }
}