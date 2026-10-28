using UnityEngine;

namespace MyGame.Gameplay
{
    public sealed class PuzzleMixer
    {
        public void MixParts(PuzzlePart[,] puzzleParts, Vector2Int partsQuantuty)
        {
            int partsHorizontalVisible = partsQuantuty.x;
            Mix(puzzleParts, partsQuantuty, partsHorizontalVisible);
        }

        public void MixSuperParts(PuzzlePart[,] puzzleParts, Vector2Int partsQuantuty, int partsHorizontalVisible)
        {
            Mix(puzzleParts, partsQuantuty, partsHorizontalVisible);
        }

        private void Mix(PuzzlePart[,] puzzleParts, Vector2Int partsQuantuty, int partsHorizontalVisible)
        {
            Vector2Int[,] idPositions = new Vector2Int[partsQuantuty.x, partsQuantuty.y];
            for (int y = 0; y < partsQuantuty.y; y++)
            {
                for (int x = 0; x < partsQuantuty.x; x++)
                {
                    idPositions[x, y] = new Vector2Int(x, y);
                }
            }

            for (int i = partsQuantuty.x - partsHorizontalVisible; i >= 0; i--)
            {
                for (int y = 0;  y < partsQuantuty.y; y++)
                {
                    for (int x = i; x < i + partsHorizontalVisible; x++)
                    {
                        Vector2Int id = new(x, y);
                        Vector2Int r = id;
                        while (idPositions[x, y] == r || idPositions[r.x, r.y] == id)
                        {
                            r.x = Random.Range(0, partsHorizontalVisible) + i;
                            r.y = Random.Range(0, partsQuantuty.y);
                        }
                        (idPositions[x, y], idPositions[r.x, r.y]) = (idPositions[r.x, r.y], idPositions[x, y]);
                    }
                }
            }

            for (int y = 0; y < partsQuantuty.y; y++)
            {
                for (int x = 0; x < partsQuantuty.x; x++)
                {
                    puzzleParts[idPositions[x, y].x, idPositions[x, y].y].IdPosition = new Vector2Int(x, y);
                }
            }
        }
    }
}