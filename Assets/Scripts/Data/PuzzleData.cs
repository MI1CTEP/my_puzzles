using System.Collections.Generic;
using UnityEngine;
using YG;

namespace MyGame
{
    public static class PuzzleData
    {
        public static PuzzleGroupData[] Groups { get; private set; }

        public static Vector2Int[] PartsQuantutys { get; set; } = new Vector2Int[]
        {
            new(2, 3), new(3, 4), new(4, 5), new(5, 6), new(6, 8)
        };

        public static int[] PartsSizes { get; set; } = new int[]
        {
            340, 260, 200, 160, 130
        };

        public static int[] Count { get; private set; } = new int[] { 10, 15, 20, 25, 30 };

        public static int ReceivedPuzzles { get; private set; }
        public static int CurrentGroupId { get; set; }
        public static int CurrentId { get; set; }

        public static void Init()
        {
            Groups = new PuzzleGroupData[Count.Length];
            int status;

            for (int i = 0; i < Count.Length; i++)
            {
                Groups[i] = new();
                for (int j = 0; j < Count[i]; j++)
                {
                    if (GetPuzzleStatuses(i) == null)
                        GetPuzzleStatuses(i).Add(0);

                    if (GetPuzzleStatuses(i).Count <= j)
                        GetPuzzleStatuses(i).Add(0);

                    status = GetStatus(i, j);
                    if (status == 1) Groups[i].openedIds.Add(j);
                    else if (status == 2) Groups[i].completedIds.Add(j);
                    else Groups[i].notOpenedIds.Add(j);
                }
            }
            ReceivedPuzzles = LoadReceivedPuzzles();
        }

        public static void SetStatus(int idGroup, int id, int status)
        {
            int currentStatus = GetStatus(idGroup, id);

            if (currentStatus == 2) return;

            if(currentStatus == 0) Groups[idGroup].notOpenedIds.Remove(id);
            else if (currentStatus == 1) Groups[idGroup].openedIds.Remove(id);

            if(status == 1) Groups[idGroup].openedIds.Add(id);
            else if (status == 2) Groups[idGroup].completedIds.Add(id);

            GetPuzzleStatuses(idGroup)[id] = status;
            YG2.SaveProgress();
        }

        public static int GetStatus(int idGroup, int id)
        {
            return GetPuzzleStatuses(idGroup)[id];
        }

        public static void AddReceivedPuzzle()
        {
            YG2.saves.receivedPuzzles++;
            YG2.SaveProgress();
        }

        private static int LoadReceivedPuzzles()
        {
            return YG2.saves.receivedPuzzles;
        }

        private static List<int> GetPuzzleStatuses(int idGroup)
        {
            return idGroup switch
            {
                1 => YG2.saves.puzzleStatuses2,
                2 => YG2.saves.puzzleStatuses3,
                3 => YG2.saves.puzzleStatuses4,
                4 => YG2.saves.puzzleStatuses5,
                _ => YG2.saves.puzzleStatuses1,
            };
        }
    }

    public class PuzzleGroupData
    {
        public List<int> notOpenedIds = new();
        public List<int> openedIds = new();
        public List<int> completedIds = new();
    }
}