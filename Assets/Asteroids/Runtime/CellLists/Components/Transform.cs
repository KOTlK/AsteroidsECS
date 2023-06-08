using System;
using UnityEngine;

namespace Asteroids.Runtime.CellLists.Components
{
    [Serializable]
    public struct Transform
    {
        public Vector2 Position;
        public Quaternion Rotation;

        public Vector2 Up => Rotation * Vector3.up;
        public Vector2 Right => Rotation * Vector3.right;
        public Vector2 Down => Rotation * Vector3.down;
        public Vector2 Left => Rotation * Vector3.left;
    }
}