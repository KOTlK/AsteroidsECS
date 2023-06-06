using System;
using Asteroids.Runtime.Collisions.Components;
using UnityEngine;
using Transform = Asteroids.Runtime.CellLists.Components.Transform;

namespace Asteroids.Runtime.Collisions.Utils
{
    public static class CollisionDetection
    {
        public static bool AABBCircle(AABBCollider aabb, Transform aabbTransform, CircleCollider circle, Transform circleTransform)
        {
            var difference = circleTransform.Position - aabbTransform.Position;
            var halfExtents = aabb.HalfExtents;
            var clamped = Clamp(difference, -halfExtents, halfExtents);
            var closest = aabbTransform.Position + clamped;
            difference = closest - circleTransform.Position;

            return difference.sqrMagnitude <= circle.Radius * circle.Radius;
        }

        public static bool AABBPair(AABBCollider first, Transform firstTransform, AABBCollider second, Transform secondTransform)
        {
            return firstTransform.Position.x + first.Size.x >= secondTransform.Position.x &&
                   secondTransform.Position.x + second.Size.x >= firstTransform.Position.x &&
                   firstTransform.Position.y + first.Size.y >= secondTransform.Position.y &&
                   secondTransform.Position.y + second.Size.y >= firstTransform.Position.y;
        }

        public static bool CirclePair(CircleCollider first, Transform firstTransform, CircleCollider second, Transform secondTransform)
        {
            var dx = firstTransform.Position.x - secondTransform.Position.x;
            var dy = firstTransform.Position.y - secondTransform.Position.y;
            var distance = Math.Sqrt(dx * dx + dy * dy);

            return distance <= first.Radius + second.Radius;
        }
        
        public static bool AABBContainsPoint(Vector2 position, AABBCollider aabb, Vector2 point)
        {
            var halfExtents = aabb.HalfExtents;
            return position.x + halfExtents.x >= point.x && 
                   position.y + halfExtents.y >= point.y &&
                   position.x - halfExtents.x <= point.x &&
                   position.y - halfExtents.y <= point.y;
        }
        
        private static Vector2 Clamp(Vector2 vector, Vector2 min, Vector2 max)
        {
            return new Vector2(Mathf.Clamp(vector.x, min.x, max.x), Mathf.Clamp(vector.y, min.y, max.y));
        }
    }
}