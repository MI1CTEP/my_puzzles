using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace MyGame.Gameplay
{
    public sealed class PuzzlePart : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Image _hint;

        private PuzzlePart[,] _puzzleParts;
        private PuzzlePartNeighbours _neighbours;
        private int _partSize;
        private Sequence _seq;
        private readonly float _timeAnim = 0.2f;
        private Image _image;
        private Camera _camera;
        private Vector3 _startPosition;
        private Vector3 _clickPosition;
        private bool _isMove;
        private static bool _block;
        private bool _isCloset;
        
        public Vector2Int Id { get; set; }
        public Vector2Int IdPosition { get; set; }
        public bool IsTruePosition { get; private set; }
        public static UnityAction<PuzzlePart> OnSetTruePosition { get; set; }
        public static UnityAction OnFail { get; set; }
        public static bool IsHint { get; set; }

        public void Init(PuzzlePart[,] puzzleParts, Sprite sprite, Vector2Int partsQuantuty, int size)
        {
            _puzzleParts = puzzleParts;
            _neighbours = new(puzzleParts, this, partsQuantuty);
            _partSize = size;
            _camera = Camera.main;
            _image = GetComponent<Image>();
            _image.sprite = sprite;
            GetComponent<RectTransform>().sizeDelta = Vector2.one * size;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_block || IsTruePosition || _isCloset) return;

            SoundsController.Instance.PlayPuzzleDown();

            _isMove = true;
            _startPosition = transform.localPosition;
            _clickPosition = transform.position - _camera.ScreenToWorldPoint(Input.mousePosition);
            transform.SetAsLastSibling();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_isMove || _isCloset) return;

            SoundsController.Instance.PlayPuzzleUp();

            _isMove = false;
            _block = true;

            PuzzlePart puzzlePart = TryGetNearestPart();

            _seq.Kill();
            _seq = DOTween.Sequence();
            if(puzzlePart == null || puzzlePart.IsTruePosition)
            {
                _seq.Insert(0, transform.DOLocalMove(_startPosition, _timeAnim));
                _seq.InsertCallback(_timeAnim, () => 
                {
                    _block = false;
                });
            }
            else
            {
                SoundsController.Instance.PlayPuzzleMove();

                bool isFail = false;
                puzzlePart.transform.SetAsLastSibling();
                (IdPosition, puzzlePart.IdPosition) = (puzzlePart.IdPosition, IdPosition);
                _seq.Insert(0, transform.DOLocalMove(puzzlePart.transform.localPosition, _timeAnim));
                _seq.InsertCallback(_timeAnim, () => 
                {
                    isFail = !TrySetTruePosition();
                });
                _seq.Insert(0, puzzlePart.transform.DOLocalMove(_startPosition, _timeAnim * 2f));
                _seq.InsertCallback(_timeAnim * 2f, () => 
                {
                    if (!puzzlePart.TrySetTruePosition() && isFail)
                        OnFail?.Invoke();
                    _block = false;
                });
            }

            if (IsHint)
            {
                foreach (PuzzlePart part in _puzzleParts)
                    part.SetActiveHint(false, false);
                IsHint = false;
            }
        }

        public void SetActivePuzzle(bool value)
        {
            if (value) _image.color = Color.white;
            else _image.color = Color.gray;

            _isCloset = !value;
        }

        public void SetActiveHint(bool value, bool isTrue)
        {
            _hint.gameObject.SetActive(value);

            if (isTrue) _hint.color = Color.green;
            else _hint.color = Color.black;
        }

        private PuzzlePart TryGetNearestPart()
        {
            foreach(PuzzlePart puzzlePart in _puzzleParts)
            {
                if (this == puzzlePart) continue;

                if (Vector3.Distance(transform.localPosition, puzzlePart.transform.localPosition) < _partSize / 2.01f)
                {
                    if (puzzlePart._isCloset) return null;
                    else return puzzlePart;
                }
            }
            return null;
        }

        private bool TrySetTruePosition()
        {
            if (_neighbours.IsHaveTrueNeighbours() && Id == IdPosition)
            {
                SetTruePosition();
                return true;
            }

            return false;
        }

        public void SetTruePosition()
        {
            if (IsTruePosition) return;

            SoundsController.Instance.PlayPuzzleTrue();

            gameObject.SetActive(false);
            IsTruePosition = true;
            OnSetTruePosition?.Invoke(this);
            _neighbours.TrySetTruePositionNeighbours();
        }

        private void Update()
        {
            if (!_isMove) return;

            transform.position = _camera.ScreenToWorldPoint(Input.mousePosition) + _clickPosition;
        }
    }
}