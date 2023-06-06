namespace Asteroids.Runtime.Collisions.Components
{
    public struct CircleCollider
    {
        public float Radius;
        public PhysicsLayer Layer;
        public PhysicsLayer TargetLayers;
    }
}