﻿namespace Asteroids.Runtime.Collisions.Components
{
    public struct Collision
    {
        public int Sender;
        public int Receiver;
        public PhysicsLayer SenderLayer;
        public PhysicsLayer ReceiverLayer;
    }
}