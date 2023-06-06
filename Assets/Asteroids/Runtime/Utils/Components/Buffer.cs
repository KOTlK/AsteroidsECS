using System.Collections.Generic;

namespace Asteroids.Runtime.Utils.Components
{
    public struct Buffer<T>
    {
        public Queue<T> ExplicitValue;
        public int Count => ExplicitValue.Count;
        public void Add(T element) => ExplicitValue.Enqueue(element);
        public T Remove() => ExplicitValue.Dequeue();
        public void Clear() => ExplicitValue.Clear();
    }
}