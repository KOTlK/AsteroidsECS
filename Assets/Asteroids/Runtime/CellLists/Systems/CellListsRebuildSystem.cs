using Asteroids.Runtime.CellLists.Components;
using Asteroids.Runtime.Collisions.Utils;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Asteroids.Runtime.CellLists.Systems
{
    public class CellListsRebuildSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<Cell, CellNeighbours>> _cellsFilter = default;
        private readonly EcsPoolInject<Cell> _cells = default;
        private readonly EcsPoolInject<Transform> _transforms = default;
        private readonly EcsPoolInject<CellNeighbours> _neighbours = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _cellsFilter.Value)
            {
                ref var cell = ref _cells.Value.Get(entity);
                ref var neighbours = ref _neighbours.Value.Get(entity);

                for(var i = 0; i < neighbours.ContainingTransforms.Count; i++)
                {
                    var transformEntity = neighbours.ContainingTransforms[i];
                    ref var transform = ref _transforms.Value.Get(transformEntity);
                    if (CollisionDetection.AABBContainsPoint(cell.Position, cell.AABB, transform.Position))
                        continue;

                    neighbours.ContainingTransforms.Remove(transformEntity);

                    foreach (var neighbourEntity in neighbours.NeighboursEntities)
                    {
                        ref var neighbourCell = ref _cells.Value.Get(neighbourEntity);

                        if (CollisionDetection.AABBContainsPoint(neighbourCell.Position, neighbourCell.AABB,
                                transform.Position))
                        {
                            ref var neighbourContainer = ref _neighbours.Value.Get(neighbourEntity);
                            neighbourContainer.ContainingTransforms.Add(transformEntity);
                        }
                    }
                }
            }
            
        }
    }
}