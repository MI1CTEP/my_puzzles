using YG;

namespace MyGame
{
    public static class ScoreData
    {
        public static void AddScore(int value)
        {
            YG2.saves.score += value;
            YG2.SaveProgress();
        }

        public static int GetScore()
        {
            return YG2.saves.score;
        }
    }
}