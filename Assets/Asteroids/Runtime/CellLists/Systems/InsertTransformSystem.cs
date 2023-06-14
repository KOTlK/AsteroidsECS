using Asteroids.Runtime.CellLists.Components;
using Asteroids.Runtime.Collisions.Utils;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Transform = Asteroids.Runtime.CellLists.Components.Transform;

namespace Asteroids.Runtime.CellLists.Systems
{
    public class InsertTransformSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<InsertInCellLists, Transform>, Exc<InsideCellLists>> _filter = default;
        private readonly EcsFilterInject<Inc<Cell, CellNeighbours>> _cellLists = default;
        private readonly EcsPoolInject<Transform> _transforms = default;
        private readonly EcsPoolInject<InsertInCellLists> _commands = default;
        private readonly EcsPoolInject<Cell> _cells = default;
        private readonly EcsPoolInject<CellNeighbours> _neighbours = default;
        private readonly EcsPoolInject<InsideCellLists> _inside = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var transform = ref _transforms.Value.Get(entity);
                var inserted = false;

                foreach (var cellEntity in _cellLists.Value)
                {
                    ref var cell = ref _cells.Value.Get(cellEntity);

                    if (cell.Contains(transform.Position))
                    {
                        ref var neighbour = ref _neighbours.Value.Get(cellEntity);
                        neighbour.ContainingTransforms.Add(entity);
                        transform.Cell = cellEntity;
                        _inside.Value.Add(entity);
                        inserted = true;
                        break;
                    }
                }

                if (inserted == false)
                {
                    var closestCell = (int.MinValue, float.MaxValue);
                    
                    foreach (var cellEntity in _cellLists.Value)
                    {
                        ref var cell = ref _cells.Value.Get(cellEntity);

                        var distance = Vector2.Distance(cell.Position, transform.Position);

                        if (distance < closestCell.MaxValue)
                        {
                            closestCell = (cellEntity, distance);
                        }
                    }

                    ref var neighbour = ref _neighbours.Value.Get(closestCell.MinValue);
                    neighbour.ContainingTransforms.Add(entity);
                    transform.Cell = closestCell.MinValue;
                    _inside.Value.Add(entity);
                }
                _commands.Value.Del(entity);
            }
        }
    }
}