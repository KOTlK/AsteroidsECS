using UnityEngine;
using UnityEngine.UI;

namespace Asteroids.Runtime.View.Mono
{
    public class Background : MonoBehaviour
    {
        [SerializeField] private RawImage _image;
        
        public void Move(Vector2 direction, float speed)
        {
            _image.uvRect =
                new Rect(_image.uvRect.position + direction * (speed * Time.deltaTime),
                    _image.uvRect.size);
        }
    }
}