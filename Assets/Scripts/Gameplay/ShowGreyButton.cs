using UnityEngine;
using UnityEngine.UI;

namespace MyGame.Gameplay
{
    public sealed class ShowGreyButton : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _showImagetext;
        [SerializeField] private Text _showPuzzletext;
        [SerializeField] private Image _puzzleBackground;
        [SerializeField] private Material _greyMaterial;
        [SerializeField] private GameObject _puzzleParts;
        [SerializeField] private GameObject _puzzleOutline;
        [SerializeField] private Button _hintButton;

        private Button _button;
        private Color _activeColor = new Color(0.86f, 1, 0.68f);
        private bool _isShowing;

        public void Init()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(Switch);
            _button.onClick.AddListener(SoundsController.Instance.PlayButtonClick);
        }

        public void Switch()
        {
            _isShowing = !_isShowing;

            _puzzleParts.SetActive(!_isShowing);
            _puzzleOutline.SetActive(!_isShowing);

            _hintButton.interactable = !_isShowing;

            if (_isShowing)
            {
                _puzzleBackground.material = _greyMaterial;
                _icon.color = _activeColor;
                _showPuzzletext.color = _activeColor;
                _showImagetext.gameObject.SetActive(false);
                _showPuzzletext.gameObject.SetActive(true);
            }
            else
            {
                _puzzleBackground.material = null;
                _icon.color = Color.white;
                _showImagetext.color = Color.white;
                _showImagetext.gameObject.SetActive(true);
                _showPuzzletext.gameObject.SetActive(false);
            }
        }
    }
}