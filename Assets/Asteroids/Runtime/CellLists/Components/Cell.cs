using System;
using Asteroids.Runtime.Collisions.Components;
using UnityEngine;

namespace Asteroids.Runtime.CellLists.Components
{
    [Serializable]
    public struct Cell
    {
        public AABBCollider AABB;
        public Vector2 Position;

        public bool Contains(Vector2 point)
        {
            var halfExtents = AABB.HalfExtents;
            return Position.x + halfExtents.x >= point.x && 
                   Position.y + halfExtents.y >= point.y &&
                   Position.x - halfExtents.x <= point.x &&
                   Position.y - halfExtents.y <= point.y;
        }
    }
}