using System;
using UnityEngine;

namespace Asteroids.Runtime.CellLists.Components
{
    [Serializable]
    public struct Transform
    {
        public Vector2 Position;
        public Quaternion Rotation;
    }
}