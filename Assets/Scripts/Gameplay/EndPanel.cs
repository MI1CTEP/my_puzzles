using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using YG;

namespace MyGame.Gameplay
{
    public sealed class EndPanel : MonoBehaviour
    {
        [SerializeField] private Transform _scorePanel;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _backToMenuButton;
        [SerializeField] private Text _firstScoreText;
        [SerializeField] private GameObject _firstNameText;
        [SerializeField] private GameObject _secondNameText;
        [SerializeField] private Text _noHintScoreText;
        [SerializeField] private GameObject _noHintsText;
        [SerializeField] private GameObject _useHintsText;
        [SerializeField] private LocalizationText _seriesNameText;
        [SerializeField] private Text _seriesScoreText;
        [SerializeField] private Text _scoreText;
        [SerializeField] private Leaderboard _leaderboard;
        [SerializeField] private HintProgress _hintProgress;

        private Image _background;
        private ProgressPanel _progressPanel;
        private UpPanel _upPanel;
        private Sequence _seq;
        private readonly float _timeAnim = 0.5f;
        private int _score;

        public void Init(ProgressPanel progressPanel, UpPanel upPanel)
        {
            _background = GetComponent<Image>();
            _backButton.onClick.AddListener(SceneLoader.LoadMenu);
            _backButton.onClick.AddListener(SoundsController.Instance.PlayButtonClick);
            _backToMenuButton.onClick.AddListener(SceneLoader.LoadMenu);
            _backToMenuButton.onClick.AddListener(SoundsController.Instance.PlayButtonClick);

            _progressPanel = progressPanel;
            _progressPanel.OnFullProgress += End;

            _upPanel = upPanel;

            gameObject.SetActive(false);

            _leaderboard.Init();

            _hintProgress.Init();
        }

        private void End()
        {
            SoundsController.Instance.PlayWin();

            int parts = PuzzleData.PartsQuantutys[PuzzleData.CurrentGroupId].x * PuzzleData.PartsQuantutys[PuzzleData.CurrentGroupId].y;
            bool isCompleted = false;

            if (SuperpuzzleData.CurrentId == -1)
            {
                if (PuzzleData.GetStatus(PuzzleData.CurrentGroupId, PuzzleData.CurrentId) == 2)
                {
                    isCompleted = true;
                }
                else
                {
                    PuzzleData.SetStatus(PuzzleData.CurrentGroupId, PuzzleData.CurrentId, 2);
                    SuperpuzzleData.AddExperience();
                }
            }
            else
            {
                if(SuperpuzzleData.IsComplete(SuperpuzzleData.CurrentId)) isCompleted = true;
                else SuperpuzzleData.SetComplete(SuperpuzzleData.CurrentId);
            }

            if (isCompleted)
            {
                _firstNameText.SetActive(false);
                _secondNameText.SetActive(true);
                parts /= 2;
            }
            else
            {
                _firstNameText.SetActive(true);
                _secondNameText.SetActive(false);
            }

            _score = 0;

            _firstScoreText.text = $"{(parts * 10):N0}";
            _score += parts * 10;

            if (HintButton.IsUseHint)
            {
                _noHintScoreText.text = "0";
                _noHintsText.SetActive(false);
                _useHintsText.SetActive(true);
            }
            else
            {
                _noHintScoreText.text = $"{(parts * 2):N0}";
                _noHintsText.SetActive(true);
                _useHintsText.SetActive(false);
                _score += parts * 2;
            }

            int seriesScore = _upPanel.MaxSeries * 5;
            _seriesNameText.SetReplaced($"X{_upPanel.MaxSeries}");
            _seriesScoreText.text = $"{seriesScore:N0}";
            _score += seriesScore;

            _scoreText.text = $"+{_score:N0}";

            ScoreData.AddScore(_score);

            int currentScore = ScoreData.GetScore();
            YG2.SetLeaderboard("TheBestPlayers", currentScore);

            ShowAnim();

            _hintProgress.Show(2, _score);
        }

        private void ShowAnim()
        {
            gameObject.SetActive(true);
            _backToMenuButton.transform.localScale = Vector3.zero;
            _scorePanel.localScale = Vector3.zero;
            _background.color = Color.clear;
            _seq = DOTween.Sequence();
            _seq.Insert(1, _scorePanel.DOScale(1, _timeAnim));
            _seq.InsertCallback(1, SoundsController.Instance.PlayShow);
            _seq.Insert(1, _background.DOFade(0.9f, _timeAnim));
            _seq.InsertCallback(_timeAnim + 1, _leaderboard.Show);
            _seq.Insert(2.5f, _backToMenuButton.transform.DOScale(1, 0.3f));
            _seq.InsertCallback(2.5f, SoundsController.Instance.PlayShow);
        }

        private void OnDestroy()
        {
            _seq.Kill();
            _progressPanel.OnFullProgress += End;
        }
    }
}