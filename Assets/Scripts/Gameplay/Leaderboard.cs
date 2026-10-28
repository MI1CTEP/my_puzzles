using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using YG;

namespace MyGame.Gameplay
{
    public sealed class Leaderboard : MonoBehaviour
    {
        [SerializeField] private Text _scoreText;
        [SerializeField] private LeaderboardYG _leaderboard;

        private Sequence _seq;
        private readonly float _timeAnim = 0.5f;

        public void Init()
        {
            transform.localScale = Vector3.zero;
        }

        public void Show()
        {
            SoundsController.Instance.PlayShow();

            int score = ScoreData.GetScore();
            _scoreText.text = score.ToString("N0");
            _leaderboard.UpdateLB();
            _seq = DOTween.Sequence();
            _seq.Insert(0, transform.DOScale(1, _timeAnim));
        }

        private void OnDestroy()
        {
            _seq.Kill();
        }
    }
}