using Asteroids.Runtime.Score.View;

namespace Asteroids.Runtime.Score
{
    public class GameScore
    {
        private readonly ScoreView _scoreView;
        
        private int _current;

        public GameScore(ScoreView scoreView)
        {
            _scoreView = scoreView;
        }

        public void Add(int amount)
        {
            _current += amount;
            _scoreView.Display(_current);
        }
        
        public void Reset()
        {
            _current = 0;
            _scoreView.Display(_current);
        }
    }
}