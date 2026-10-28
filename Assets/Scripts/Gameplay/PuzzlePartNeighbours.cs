using UnityEngine;

namespace MyGame.Gameplay
{
    public sealed class PuzzlePartNeighbours
    {
        private PuzzlePart[,] _puzzleParts;
        private Vector2Int _partsQuantity;
        private Vector2Int[] _ids;

        public PuzzlePartNeighbours(PuzzlePart[,] puzzleParts, PuzzlePart puzzlePartThis, Vector2Int partsQuantity)
        {
            _puzzleParts = puzzleParts;
            _partsQuantity = partsQuantity;

            int neighboursQuantity = 4;
            if (puzzlePartThis.Id.x == 0) neighboursQuantity--;
            if (puzzlePartThis.Id.x == partsQuantity.x - 1) neighboursQuantity--;
            if (puzzlePartThis.Id.y == 0) neighboursQuantity--;
            if (puzzlePartThis.Id.y == partsQuantity.y - 1) neighboursQuantity--;

            _ids = new Vector2Int[neighboursQuantity];
            int i = 0;
            if(puzzlePartThis.Id.x != 0)
            {
                _ids[i] = puzzlePartThis.Id + Vector2Int.left;
                i++;
            }
            if (puzzlePartThis.Id.x != partsQuantity.x - 1)
            {
                _ids[i] = puzzlePartThis.Id + Vector2Int.right;
                i++;
            }
            if (puzzlePartThis.Id.y != 0)
            {
                _ids[i] = puzzlePartThis.Id + Vector2Int.down;
                i++;
            }
            if (puzzlePartThis.Id.y != partsQuantity.y - 1)
            {
                _ids[i] = puzzlePartThis.Id + Vector2Int.up;
            }
        }

        public bool IsHaveTrueNeighbours()
        {
            if (_ids.Length < 4)
                return true;
            else 
                for (int i = 0; i < _ids.Length; i++)
                    if (_puzzleParts[_ids[i].x, _ids[i].y].IsTruePosition)
                        return true;

            return false;
        }

        public void TrySetTruePositionNeighbours()
        {
            PuzzlePart neighbour;

            for (int i = 0; i < _ids.Length; i++)
            {
                neighbour = _puzzleParts[_ids[i].x, _ids[i].y];
                if (!neighbour.IsTruePosition && neighbour.Id == neighbour.IdPosition)
                    neighbour.SetTruePosition();
            }
        }
    }
}