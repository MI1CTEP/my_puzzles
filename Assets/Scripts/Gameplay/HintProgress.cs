using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace MyGame
{
    public sealed class HintProgress : MonoBehaviour
    {
        [SerializeField] private Text _scoreText;
        [SerializeField] private GameObject _hint;
        [SerializeField] private Text _hintText;

        private Sequence _seq;
        private int _experience;
        private int _score;
        private int _currentScore;
        private int _currentHint;

        public void Init()
        {
            _currentScore = ScoreData.GetScore();
            _hint.SetActive(false);
            gameObject.SetActive(false);
        }

        public void Show(float timeWait, int score)
        {
            gameObject.SetActive(true);
            transform.localScale = Vector3.zero;

            _score = score;
            _experience = HintData.NecessaryExperience - (_currentScore % HintData.NecessaryExperience);
            int hintValue = (_score - _experience + HintData.NecessaryExperience) / HintData.NecessaryExperience;
            if (hintValue > 0)
                HintData.AddHint(hintValue);

            _scoreText.text = _experience.ToString();

            _seq = DOTween.Sequence();
            _seq.InsertCallback(timeWait, SoundsController.Instance.PlayShow);
            _seq.Insert(timeWait, transform.DOScale(1, 0.5f));
            _seq.Insert(timeWait + 0.5f, DOTween.To(UpdateExperience, 0, 1, 1));
        }

        private void UpdateExperience(float multiplier)
        {
            int experienceView = _experience - (int)(_score * multiplier) + HintData.NecessaryExperience * _currentHint;

            if (experienceView < 0)
            {
                _currentHint++;
                experienceView = _experience - (int)(_score * multiplier) + HintData.NecessaryExperience * _currentHint;
                _hint.SetActive(true);
                _hintText.text = $"+{_currentHint}";
            }

            _scoreText.text = experienceView.ToString();
        }

        private void OnDestroy()
        {
            _seq.Kill();
        }
    }
}
