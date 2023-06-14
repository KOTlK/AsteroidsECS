using System.Linq;
using Asteroids.Runtime.AI.Components;
using Asteroids.Runtime.Application;
using Asteroids.Runtime.CellLists.Components;
using Asteroids.Runtime.Enemies.Components;
using Asteroids.Runtime.HP.Components;
using Asteroids.Runtime.Ships.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Transform = Asteroids.Runtime.CellLists.Components.Transform;

namespace Asteroids.Runtime.AI.Systems
{
    public class PatrolSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<Config> _config = default;
        private readonly EcsFilterInject<Inc<Enemy, Ship, ShipInput, Patrol, Transform>> _filter = default;
        private readonly EcsFilterInject<Inc<Ship, Transform, Health>, Exc<HealthOver, Destroy>> _targets = default;
        private readonly EcsPoolInject<Enemy> _enemies = default;
        private readonly EcsPoolInject<Patrol> _patrols = default;
        private readonly EcsPoolInject<Transform> _transforms = default;
        private readonly EcsPoolInject<ShipInput> _shipInputs = default;
        private readonly EcsPoolInject<CellNeighbours> _neighbours = default;
        private readonly EcsPoolInject<Follow> _follow = default;
        private readonly EcsPoolInject<Health> _healths = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var transform = ref _transforms.Value.Get(entity);
                ref var patrol = ref _patrols.Value.Get(entity);
                ref var shipInput = ref _shipInputs.Value.Get(entity);
                
                //move to patrol point
                var direction = patrol.Point - transform.Position;

                if (direction.sqrMagnitude <= 2f)
                {
                    var cellLists = _config.Value.CellListsConfig;

                    patrol.Point = cellLists.RandomPointInside();

                    direction = patrol.Point - transform.Position;
                }

                direction = direction.normalized;
                shipInput.MovementDirection = direction;
                shipInput.LookDirection = direction;
                
                //scan for target around
                if(transform.Cell == int.MinValue)
                    continue;
                
                ref var enemy = ref _enemies.Value.Get(entity);
                ref var neighbours = ref _neighbours.Value.Get(transform.Cell);
                var allTargets = _targets.Value.GetRawEntities();
                var targetFound = false;

                foreach (var neighbourTransformEntity in neighbours.ContainingTransforms)
                {
                    if(allTargets.Contains(neighbourTransformEntity) == false)
                        continue;
                    
                    if(neighbourTransformEntity == entity)
                        continue;

                    ref var health = ref _healths.Value.Get(neighbourTransformEntity);
                    
                    if(health.Current <= health.Min)
                        continue;
                    
                    ref var targetTransform = ref _transforms.Value.Get(neighbourTransformEntity);

                    var directionToTarget = targetTransform.Position - transform.Position;

                    if (directionToTarget.sqrMagnitude < enemy.FollowRange * enemy.FollowRange)
                    {
                        _patrols.Value.Del(entity);
                        ref var follow = ref _follow.Value.Add(entity);
                        follow.TargetEntity = neighbourTransformEntity;
                        targetFound = true;
                        break;
                    }
                }
                
                if(targetFound)
                    continue;

                foreach (var neighbourEntity in neighbours.NeighboursEntities)
                {
                    ref var neighbour = ref _neighbours.Value.Get(neighbourEntity);
                    
                    foreach (var neighbourTransformEntity in neighbour.ContainingTransforms)
                    {
                        if(allTargets.Contains(neighbourTransformEntity) == false)
                            continue;
                        
                        if(neighbourTransformEntity == entity)
                            continue;
                        
                        ref var health = ref _healths.Value.Get(neighbourTransformEntity);
                    
                        if(health.Current <= health.Min)
                            continue;

                        ref var targetTransform = ref _transforms.Value.Get(neighbourTransformEntity);

                        var directionToTarget = targetTransform.Position - transform.Position;

                        if (directionToTarget.sqrMagnitude < enemy.FollowRange * enemy.FollowRange)
                        {
                            _patrols.Value.Del(entity);
                            ref var follow = ref _follow.Value.Add(entity);
                            follow.TargetEntity = neighbourTransformEntity;
                            targetFound = true;
                            break;
                        }
                    }

                    if (targetFound)
                        break;
                }
            }
        }
    }
}