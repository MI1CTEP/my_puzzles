using YG;

namespace MyGame
{
    public static class HintData
    {
        public static int NecessaryExperience { get; } = 300;

        public static void AddHint(int value)
        {
            YG2.saves.hints += value;
            YG2.SaveProgress();
        }

        public static int GetHint()
        {
            return YG2.saves.hints;
        }
    }
}