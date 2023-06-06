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
    }
}