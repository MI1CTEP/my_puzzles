using UnityEngine;

namespace MyGame.Gameplay
{
    public sealed class SuperPuzzleController
    {
        private PuzzlePart[,] _puzzleParts;
        private Vector2Int _partsQuantuty;
        private int _horizontalVisible;
        private int _currentLine;

        public SuperPuzzleController(PuzzlePart[,] puzzleParts, Vector2Int partsQuantuty, int horizontalVisible)
        {
            _puzzleParts = puzzleParts;
            _partsQuantuty = partsQuantuty;
            _horizontalVisible = horizontalVisible;
            UpdatePuzzleParts();
        }

        public bool CheckLine()
        {
            for (int i = 0; i < _partsQuantuty.y; i++)
            {
                if (_puzzleParts[_currentLine, i].IsTruePosition == false)
                    return false;
            }
            _currentLine++;
            UpdatePuzzleParts();
            return true;
        }

        private void UpdatePuzzleParts()
        {
            foreach (PuzzlePart puzzlePart  in _puzzleParts)
                puzzlePart.SetActivePuzzle(puzzlePart.IdPosition.x < _horizontalVisible + _currentLine);
        }
    }
}