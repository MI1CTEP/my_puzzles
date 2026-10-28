using UnityEngine;
using DG.Tweening;

namespace MyGame.Gameplay
{
    public sealed class PuzzleController : MonoBehaviour
    {
        [SerializeField] private PuzzlePart _puzzlePartsPrefab;
        [SerializeField] private Transform _partsParent;
        [SerializeField] private PuzzleBackground _puzzleBackground;
        [SerializeField] private PuzzlePartsOutline _puzzlePartsOutline;

        private Sprite _sprite;
        private PuzzleCreator _puzzleCreator;
        private PuzzleMixer _puzzleMixer;
        private PuzzlePart[,] _puzzleParts;
        private SuperPuzzleController _superPuzzleController;
        private Vector2Int _partsQuantuty;
        private int _partsSize;
        private RectTransform _rect;
        private Sequence _seq;
        private readonly float _timeAnim = 0.3f;
        private int _neededLines;
        private int _completeLines;

        public PuzzlePart[,] PuzzleParts => _puzzleParts;

        public void Init(ProgressPanel progressPanel, Sprite sprite)
        {
            _sprite = sprite;

            if(SuperpuzzleData.CurrentId != -1)
            {
                _rect = GetComponent<RectTransform>();
                _rect.sizeDelta = new Vector2(2048, 1152);
                _rect.anchoredPosition = new Vector2(576, 0);
                _partsQuantuty = SuperpuzzleData.GetPartsQuantutys(SuperpuzzleData.CurrentId);
                _partsSize = SuperpuzzleData.GetSize(SuperpuzzleData.CurrentId);
            }
            else
            {
                _partsQuantuty = PuzzleData.PartsQuantutys[PuzzleData.CurrentGroupId];
                _partsSize = PuzzleData.PartsSizes[PuzzleData.CurrentGroupId];
            }
            _puzzleBackground.Init(progressPanel, _sprite);

            _puzzleCreator = new(_puzzlePartsPrefab, _partsParent);
            _puzzleParts = _puzzleCreator.Create(sprite.texture, _partsQuantuty, _partsSize);

            _puzzleMixer = new();
            if (SuperpuzzleData.CurrentId != -1)
                _puzzleMixer.MixSuperParts(_puzzleParts, _partsQuantuty, SuperpuzzleData.GetHorizontalVisible(SuperpuzzleData.CurrentId));
            else
                _puzzleMixer.MixParts(_puzzleParts, _partsQuantuty);

            SetPartsPosition();

            _puzzlePartsOutline.Init(_partsQuantuty, _partsSize);

            if (SuperpuzzleData.CurrentId != -1)
            {
                _superPuzzleController = new(_puzzleParts, _partsQuantuty, SuperpuzzleData.GetHorizontalVisible(SuperpuzzleData.CurrentId));
                _neededLines = _partsQuantuty.x - SuperpuzzleData.GetHorizontalVisible(SuperpuzzleData.CurrentId);
                PuzzlePart.OnSetTruePosition += CheckLine;
            }
        }

        public void CheckTrueParts()
        {
            foreach (var item in _puzzleParts)
            {
                if (item.Id == item.IdPosition)
                    item.SetTruePosition();
            }
        }

        public void ShowHint()
        {
            foreach (PuzzlePart puzzlePart in _puzzleParts)
                puzzlePart.SetActiveHint(true, false);

            Vector2Int id = Vector2Int.zero;
            for (int x = 0; x < _partsQuantuty.x; x++)
            {
                for (int y = 0; y < _partsQuantuty.y; y++)
                {
                    if(_puzzleParts[x, y].IsTruePosition == false)
                    {
                        id.x = x;
                        id.y = y;
                        _puzzleParts[x, y].SetActiveHint(true, true);
                        x = _partsQuantuty.x;
                        break;
                    }
                }
            }

            for (int x = 0; x < _partsQuantuty.x; x++)
            {
                for (int y = 0; y < _partsQuantuty.y; y++)
                {
                    if (_puzzleParts[x, y].IdPosition == id)
                    {
                        _puzzleParts[x, y].SetActiveHint(true, true);
                        x = _partsQuantuty.x;
                        break;
                    }
                }
            }

            PuzzlePart.IsHint = true;
        }

        private void CheckLine(PuzzlePart puzzlePart)
        {
            if (_completeLines >= _neededLines) return;

            if (_superPuzzleController.CheckLine())
            {
                _completeLines++;
                _seq.Kill();
                _seq = DOTween.Sequence();
                _seq.Insert(0, _rect.DOAnchorPosX(576 - 1152f * _completeLines / _neededLines, _timeAnim));
                _seq.InsertCallback(_timeAnim, () => CheckLine(null));
            }
        }

        private void SetPartsPosition()
        {
            for (int y = 0; y < _partsQuantuty.y; y++)
            {
                for (int x = 0; x < _partsQuantuty.x; x++)
                {
                    Vector3 position = Vector3.zero;
                    position.x = -_partsSize * (_partsQuantuty.x - 1) / 2f + _partsSize * _puzzleParts[x, y].IdPosition.x;
                    position.y = -_partsSize * (_partsQuantuty.y - 1) / 2f + _partsSize * _puzzleParts[x, y].IdPosition.y;
                    _puzzleParts[x, y].transform.localPosition = position;
                }
            }
        }

        private void OnDestroy()
        {
            PuzzlePart.IsHint = false;
            AsyncContent.ReleaseSprite(_sprite);
            if (SuperpuzzleData.CurrentId != -1)
            {
                _seq.Kill();
                PuzzlePart.OnSetTruePosition -= CheckLine;
            }
        }
    }
}