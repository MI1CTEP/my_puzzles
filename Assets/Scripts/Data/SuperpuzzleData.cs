using UnityEngine;
using YG;

namespace MyGame
{
    public static class SuperpuzzleData
    {
        public static Vector2Int GetPartsQuantutys(int id) => _partsQuantutys[_difficulties[id]];
        private static readonly Vector2Int[] _partsQuantutys = { new(7, 4), new(10, 5), new(12, 6), new(15, 8) };

        public static int GetSize(int id) => _partsSizes[_difficulties[id]];
        private static readonly int[] _partsSizes =  { 260, 200, 160, 130 };

        public static int GetHorizontalVisible(int id) => _partsHorizontalVisible[_difficulties[id]];
        private static readonly int[] _partsHorizontalVisible = { 3, 4, 5, 6 };

        private static readonly int[] _difficulties = { 0, 0, 1, 1, 1, 2, 2, 2, 3, 3 };

        public static int CurrentId { get; set; }

        public static void SetComplete(int id)
        {
            if (YG2.saves.superpuzzlesComplete.Count <= id)
                for (int i = YG2.saves.superpuzzlesComplete.Count; i <= id; i++)
                    YG2.saves.superpuzzlesComplete.Add(false);

            YG2.saves.superpuzzlesComplete[id] = true;
            YG2.SaveProgress();
        } 

        public static bool IsComplete(int id)
        {
            if (YG2.saves.superpuzzlesComplete.Count <= id)
                return false;

            return YG2.saves.superpuzzlesComplete[id];
        }

        public static void AddExperience()
        {
            YG2.saves.superpuzzlesExperiense++;
            YG2.SaveProgress();
        }

        public static int GetExperience()
        {
            return YG2.saves.superpuzzlesExperiense;
        }
    }
}