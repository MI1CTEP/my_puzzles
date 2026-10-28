using UnityEngine;
using UnityEngine.UI;

namespace MyGame.Menu
{
    public sealed class SuperPuzzleController : MonoBehaviour
    {
        [SerializeField] private SuperPuzzleProgress _progress;
        [SerializeField] private Image _image;
        [SerializeField] private Material _grayMaterial;
        [SerializeField] private Button _playButton;
        [SerializeField] private Text _playText1;
        [SerializeField] private Text _playText2;
        [SerializeField] private Button _previousButton;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Text _currentIdText;
        [SerializeField] private Sprite _activePlayImage;
        [SerializeField] private Sprite _passivePlayImage;

        private Image _playImage;
        private Sprite _currentSprite;
        private int _openedValue;
        private int _currentId;

        public void Init()
        {
            _openedValue = SuperpuzzleData.GetExperience();
            int progress = _openedValue % 10;
            _openedValue /= 10;
            _openedValue = Mathf.Clamp(_openedValue, 0, 9);

            _currentId = _openedValue;
            if (progress == 0 && _openedValue > 0) _currentId = _openedValue - 1;

            _previousButton.gameObject.SetActive(_openedValue > 0);
            _previousButton.onClick.AddListener(() => Switch(-1));
            _previousButton.onClick.AddListener(SoundsController.Instance.PlayButtonClick);
            _nextButton.gameObject.SetActive(_openedValue > 0);
            _nextButton.onClick.AddListener(() => Switch(1));
            _nextButton.onClick.AddListener(SoundsController.Instance.PlayButtonClick);

            _playImage = _playButton.GetComponent<Image>();
            _playButton.onClick.AddListener(PlaySuperPuzzle);
            _playButton.onClick.AddListener(SoundsController.Instance.PlayButtonClick);

            _progress.Init(progress);

            SetImage();
        }

        private void Switch(int id)
        {
            _currentId += id;
            if (_currentId > _openedValue) _currentId = 0;
            else if (_currentId < 0) _currentId = _openedValue;
            SetImage();
        }

        private void SetImage()
        {
            _previousButton.interactable = false;
            _nextButton.interactable = false;

            SetActivePlayButton();

            if (_currentSprite != null)
                AsyncContent.ReleaseSprite(_currentSprite);
            _image.color = Color.clear;
            AsyncContent.LoadSuperPuzzleSprite(_currentId, (sprite) =>
            {
                _previousButton.interactable = true;
                _nextButton.interactable = true;
                _currentSprite = sprite;
                _image.color = Color.white;
                _image.sprite = sprite;
                if (SuperpuzzleData.IsComplete(_currentId)) _image.material = null;
                else _image.material = _grayMaterial;
            });

            _currentIdText.text = $"{_currentId + 1}/{_openedValue + 1}";
        }

        private void SetActivePlayButton()
        {
            bool isActive = _currentId < _openedValue || SuperpuzzleData.GetExperience() == 100;
            _playButton.interactable = isActive;
            if (isActive)
            {
                _playImage.sprite = _activePlayImage;
                _playText1.color = Color.white;
                _playText2.color = Color.white;
            }
            else
            {
                _playImage.sprite = _passivePlayImage;
                _playText1.color = Color.grey;
                _playText2.color = Color.grey;
            }
        }

        private void PlaySuperPuzzle()
        {
            SuperpuzzleData.CurrentId = _currentId;
            SceneLoader.LoadGameplay();
        }

        private void OnDestroy()
        {
            if (_currentSprite != null)
                AsyncContent.ReleaseSprite(_currentSprite);
        }
    }
}