using System.Collections.Generic;

namespace YG
{
    [System.Serializable]
    public partial class SavesYG
    {
        public int idSave;

        public int score;
        public int hints = 3;

        public List<int> puzzleStatuses1 = new();
        public List<int> puzzleStatuses2 = new();
        public List<int> puzzleStatuses3 = new();
        public List<int> puzzleStatuses4 = new();
        public List<int> puzzleStatuses5 = new();
        public int receivedPuzzles;

        public List<bool> superpuzzlesComplete = new();
        public int superpuzzlesExperiense;
    }
}
