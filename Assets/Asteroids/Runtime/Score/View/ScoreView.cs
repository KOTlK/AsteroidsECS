using TMPro;
using UnityEngine;

namespace Asteroids.Runtime.Score.View
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _score;

        public void Display(int score)
        {
            _score.text = score.ToString();
        }
    }
}