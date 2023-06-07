using Asteroids.Runtime.CellLists.Components;
using Asteroids.Runtime.Utils;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Asteroids.Runtime.CellLists.Systems
{
    public class RemoveFromCellListsSystem : IEcsRunSystem
    {
        private readonly EcsWorldInject _eventsWorld = Constants.EventsWorldName;
        private readonly EcsFilterInject<Inc<CellNeighbours>> _cellsFilter = default;
        private readonly EcsFilterInject<Inc<RemoveFromCellLists>> _filter = Constants.EventsWorldName;
        private readonly EcsPoolInject<RemoveFromCellLists> _commands = Constants.EventsWorldName;
        private readonly EcsPoolInject<CellNeighbours> _neighbours = default;
        
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var command = ref _commands.Value.Get(entity);

                foreach (var neighbourEntity in _cellsFilter.Value)
                {
                    ref var neighbour = ref _neighbours.Value.Get(neighbourEntity);

                    if (neighbour.ContainingTransforms.Contains(command.TransformEntity))
                    {
                        neighbour.ContainingTransforms.Remove(command.TransformEntity);
                        break;
                    }
                }

                _eventsWorld.Value.DelEntity(entity);
            }
        }
    }
}