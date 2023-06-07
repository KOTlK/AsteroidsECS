using Asteroids.Runtime.CellLists.Components;
using Asteroids.Runtime.Enemies.Components;
using Asteroids.Runtime.HP.Components;
using Asteroids.Runtime.Utils;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Asteroids.Runtime.Enemies.Systems
{
    public class EnemyDestroySystem : IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsWorldInject _eventsWorld = Constants.EventsWorldName;
        private readonly EcsFilterInject<Inc<Enemy, EnemyReference, Destroy>> _filter = default;
        private readonly EcsPoolInject<EnemyReference> _references = default;
        private readonly EcsPoolInject<RemoveFromCellLists> _removeCommands = Constants.EventsWorldName;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var reference = ref _references.Value.Get(entity);
                reference.Dispose();

                var commandEntity = _eventsWorld.Value.NewEntity();
                ref var command = ref _removeCommands.Value.Add(commandEntity);
                command.TransformEntity = entity;

                _world.Value.DelEntity(entity);
            }
        }
    }
}