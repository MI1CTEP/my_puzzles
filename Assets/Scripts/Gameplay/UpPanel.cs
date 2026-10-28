using UnityEngine;
using UnityEngine.UI;

namespace MyGame.Gameplay
{
    public sealed class UpPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _firstRun;
        [SerializeField] private GameObject _secondRun;
        [SerializeField] private Text _currentSeriesText;
        [SerializeField] private Text _maxSeriesText;

        private int _currentSeries;
        private int _maxSeries;

        public int MaxSeries => _maxSeries;

        public void Init()
        {
            if(PuzzleData.GetStatus(PuzzleData.CurrentGroupId, PuzzleData.CurrentId) == 2)
            {
                _firstRun.SetActive(false);
                _secondRun.SetActive(true);
            }
            else
            {
                _firstRun.SetActive(true);
                _secondRun.SetActive(false);
            }
            PuzzlePart.OnFail += ResetSeries;
            PuzzlePart.OnSetTruePosition += UpSeries;
        }

        private void ResetSeries()
        {
            _currentSeries = 0;
            _currentSeriesText.text = $"X{_currentSeries}";
        }

        private void UpSeries(PuzzlePart puzzlePart)
        {
            _currentSeries++;
            _currentSeriesText.text = $"X{_currentSeries}";
            if (_maxSeries < _currentSeries)
            {
                _maxSeries = _currentSeries;
                _maxSeriesText.text = $"X{_maxSeries}";
            }
        }

        private void OnDestroy()
        {
            PuzzlePart.OnFail -= ResetSeries;
            PuzzlePart.OnSetTruePosition -= UpSeries;
        }
    }
}