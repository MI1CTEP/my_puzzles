using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace MyGame.Menu
{
    public sealed class PuzzlePanel : MonoBehaviour
    {
        [SerializeField] private Transform _puzzleItemParent;
        [SerializeField] private Transform _upPanel;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _playPuzzleButton;
        [SerializeField] private Text _sizeText;
        [SerializeField] private Text _quantityText;
        [SerializeField] private GameObject[] _difficultyTextes;

        private AllPuzzles _allPuzzles;
        private PuzzleItem _puzzleItem;
        private Sequence _seq;
        private readonly float _timeAnim = 0.5f;
        private Image _background;
        private Color _backgroundColor;

        public void Init(AllPuzzles allPuzzles)
        {
            _allPuzzles = allPuzzles;

            _background = GetComponent<Image>();
            _backgroundColor = _background.color;

            _backButton.onClick.AddListener(Hide);
            _backButton.onClick.AddListener(SoundsController.Instance.PlayButtonClick);
            _playPuzzleButton.onClick.AddListener(PlayPuzzle);
            _playPuzzleButton.onClick.AddListener(SoundsController.Instance.PlayButtonClick);

            gameObject.SetActive(false);
        }

        public void Show(PuzzleItem puzzleItem, bool isNew)
        {
            _puzzleItem = puzzleItem;

            Vector2Int parts = PuzzleData.PartsQuantutys[puzzleItem.IdGroup];
            _sizeText.text = $"{parts.x}X{parts.y}";
            _quantityText.text = $"{parts.x * parts.y}";
            for (int i = 0; i < _difficultyTextes.Length; i++)
                _difficultyTextes[i].SetActive(i == puzzleItem.IdGroup);

            gameObject.SetActive(true);
            _upPanel.localScale = Vector3.zero;
            _playPuzzleButton.transform.localScale = Vector3.zero;
            if (isNew)
                puzzleItem.SetParent(_puzzleItemParent);
            puzzleItem.Content.SetParent(_puzzleItemParent);
            _backgroundColor.a = 0;

            TryStopAnim();
            _seq = DOTween.Sequence();
            _seq.Insert(0, _background.DOFade(0.85f, _timeAnim));
            _seq.Insert(0, puzzleItem.Content.DOMove(Vector3.zero, _timeAnim * 0.8f));
            _seq.Insert(0, puzzleItem.Content.DOScale(Vector3.one, _timeAnim * 0.8f));
            _seq.InsertCallback(_timeAnim * 0.8f, () =>
            {
                SoundsController.Instance.PlayShow();
                if (isNew)
                    _allPuzzles.SetNewItem(_puzzleItem);
            });
            _seq.Insert(_timeAnim * 0.8f, _upPanel.DOScale(1, _timeAnim * 0.2f));
            _seq.Insert(_timeAnim * 0.8f, _playPuzzleButton.transform.DOScale(1, _timeAnim * 0.2f));
        }

        public void Hide()
        {
            TryStopAnim();
            _seq = DOTween.Sequence();
            _seq.Insert(0, _upPanel.DOScale(0, _timeAnim * 0.2f));
            _seq.Insert(0, _playPuzzleButton.transform.DOScale(0, _timeAnim * 0.2f));
            _seq.Insert(_timeAnim * 0.2f, _background.DOFade(0, _timeAnim * 0.8f));
            _seq.Insert(_timeAnim * 0.2f, _puzzleItem.Content.DOMove(_puzzleItem.transform.position, _timeAnim * 0.8f));
            _seq.Insert(_timeAnim * 0.2f, _puzzleItem.Content.DOScale(_puzzleItem.StartScale, _timeAnim * 0.8f));
            _seq.InsertCallback(_timeAnim, () =>
            {
                _puzzleItem.Content.SetParent(_puzzleItem.transform);
                gameObject.SetActive(false);
            });
        }

        private void PlayPuzzle()
        {
            SuperpuzzleData.CurrentId = -1;
            PuzzleData.CurrentGroupId = _puzzleItem.IdGroup;
            PuzzleData.CurrentId = _puzzleItem.Id;
            SceneLoader.LoadGameplay();
        }

        private void TryStopAnim()
        {
            if(_seq != null)
            {
                _seq.Kill();
                _seq = null;
            }
        }

        private void OnDestroy()
        {
            TryStopAnim();
        }
    }
}