using UnityEngine;

namespace MyGame.Gameplay
{
    public sealed class PuzzleCreator
    {
        private PuzzlePart _puzzlePartPrefab;
        private Transform _puzzlePatent;
        private Vector2 _offset;

        public PuzzleCreator(PuzzlePart puzzlePartPrefab, Transform puzzlePatent)
        {
            _puzzlePartPrefab = puzzlePartPrefab;
            _puzzlePatent = puzzlePatent;
        }

        public PuzzlePart[,] Create(Texture2D texture2D, Vector2Int partsQuantuty, int partsSize)
        {
            _offset.x = (texture2D.width - partsQuantuty.x * partsSize) / 2;
            _offset.y = (texture2D.height - partsQuantuty.y * partsSize) / 2;

            PuzzlePart[,] puzzleParts = new PuzzlePart[partsQuantuty.x, partsQuantuty.y];
            for (int y = 0; y < partsQuantuty.y; y++)
            {
                for (int x = 0; x < partsQuantuty.x; x++)
                {
                    Sprite sprite = GetPartSprite(texture2D, x, y, partsSize);
                    puzzleParts[x, y] = GameObject.Instantiate(_puzzlePartPrefab, _puzzlePatent);
                    Vector2Int id = new(x, y);
                    puzzleParts[x, y].Id = id;
                    puzzleParts[x, y].IdPosition = id;
                    puzzleParts[x, y].Init(puzzleParts, sprite, partsQuantuty, partsSize);
                }
            }

            return puzzleParts;
        }

        private Sprite GetPartSprite(Texture2D sourceTexture, int x, int y, int partsSize)
        {
            Rect rect = new Rect(x * partsSize + _offset.x, y * partsSize + _offset.y, partsSize, partsSize);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            return Sprite.Create(sourceTexture, rect, pivot);
        }
    }
}