using UnityEngine;

namespace Asteroids.Runtime.Collisions.Components
{
    public struct AABBCollider
    {
        public Vector2 Size;
        public PhysicsLayer Layer;
        public PhysicsLayer TargetLayers;
        public Vector2 HalfExtents => Size * 0.5f;
    }
}