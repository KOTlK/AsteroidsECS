using Asteroids.Runtime.CellLists.Components;
using Asteroids.Runtime.Collisions.Utils;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Asteroids.Runtime.CellLists.Systems
{
    public class CellListsRebuildSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<Cell, CellNeighbours>> _cellsFilter = default;
        private readonly EcsFilterInject<Inc<Transform, InsideCellLists>> _transformsFilter = default;
        private readonly EcsPoolInject<Cell> _cells = default;
        private readonly EcsPoolInject<Transform> _transforms = default;
        private readonly EcsPoolInject<CellNeighbours> _neighbours = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _transformsFilter.Value)
            {
                ref var transform = ref _transforms.Value.Get(entity);

                ref var cell = ref _cells.Value.Get(transform.Cell);
                
                if(cell.Contains(transform.Position))
                    continue;

                ref var neighbours = ref _neighbours.Value.Get(transform.Cell);

                neighbours.ContainingTransforms.Remove(entity);

                foreach (var cellEntity in _cellsFilter.Value)
                {
                    ref var targetCell = ref _cells.Value.Get(cellEntity);

                    if (targetCell.Contains(transform.Position))
                    {
                        ref var targetCellNeighbours = ref _neighbours.Value.Get(cellEntity);

                        targetCellNeighbours.ContainingTransforms.Add(entity);
                        transform.Cell = cellEntity;
                    }
                }
            }
            
            /*
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
                    transform.Cell = int.MinValue;

                    foreach (var neighbourEntity in neighbours.NeighboursEntities)
                    {
                        ref var neighbourCell = ref _cells.Value.Get(neighbourEntity);

                        if (CollisionDetection.AABBContainsPoint(neighbourCell.Position, neighbourCell.AABB,
                                transform.Position))
                        {
                            ref var neighbourContainer = ref _neighbours.Value.Get(neighbourEntity);
                            neighbourContainer.ContainingTransforms.Add(transformEntity);
                            transform.Cell = neighbourEntity;
                            break;
                        }
                    }
                }
            }
            */
        }
    }
}