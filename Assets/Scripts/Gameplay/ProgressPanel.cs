using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace MyGame.Gameplay
{
    public sealed class ProgressPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform _valueRect;
        [SerializeField] private Text _valueText;

        private Vector2 _valueRectAnchoredPosition = Vector2.zero;
        private float _maxSizeX;
        private int _maxValue;
        private int _value = -1;

        public UnityAction OnFullProgress { get; set; }

        public void Init()
        {
            Vector2Int partsQuantuty;
            if (SuperpuzzleData.CurrentId == -1)
                partsQuantuty = PuzzleData.PartsQuantutys[PuzzleData.CurrentGroupId];
            else
                partsQuantuty = SuperpuzzleData.GetPartsQuantutys(SuperpuzzleData.CurrentId);
            _maxValue = partsQuantuty.x * partsQuantuty.y;
            _maxSizeX = _valueRect.sizeDelta.x;
            _valueRectAnchoredPosition.x = -_maxSizeX;
            _valueRect.anchoredPosition = _valueRectAnchoredPosition;

            UpdateValue(null);
            PuzzlePart.OnSetTruePosition += UpdateValue;
        }

        private void UpdateValue(PuzzlePart puzzle)
        {
            _value++;
            _valueText.text = $"{_value} / {_maxValue}";
            _valueRectAnchoredPosition.x = _maxSizeX * ((float)_value / _maxValue - 1);
            _valueRect.anchoredPosition = _valueRectAnchoredPosition;

            if (_value == _maxValue)
                OnFullProgress?.Invoke();
        }

        private void OnDestroy()
        {
            PuzzlePart.OnSetTruePosition -= UpdateValue;
        }
    }
}