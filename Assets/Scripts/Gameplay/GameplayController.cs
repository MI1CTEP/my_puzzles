using UnityEngine;
using UnityEngine.UI;

namespace MyGame.Gameplay
{
    public sealed class GameplayController : MonoBehaviour
    {
        [SerializeField] private Button _buttonSettings;
        [SerializeField] private PuzzleController _puzzleController;
        [SerializeField] private ProgressPanel _progressPanel;
        [SerializeField] private UpPanel _upPanel;
        [SerializeField] private EndPanel _endPanel;
        [SerializeField] private ShowGreyButton _showGreyButton;
        [SerializeField] private HintButton _hintButton;
        [SerializeField] private Transform _background;

        private void Start()
        {
            _background.gameObject.SetActive(true);

            if (SuperpuzzleData.CurrentId != -1)
                AsyncContent.LoadSuperPuzzleSprite(SuperpuzzleData.CurrentId, (Sprite sprite) => Init(sprite));
            else
                AsyncContent.LoadPuzzleSprite(PuzzleData.CurrentGroupId, PuzzleData.CurrentId, (Sprite sprite) => Init(sprite));
        }

        private void Init(Sprite sprite)
        {
            _buttonSettings.onClick.AddListener(Settings.Instance.Show);
            _buttonSettings.onClick.AddListener(SoundsController.Instance.PlayButtonClick);
            _puzzleController.Init(_progressPanel, sprite);
            _upPanel.Init();
            _progressPanel.Init();
            _endPanel.Init(_progressPanel, _upPanel);
            _puzzleController.CheckTrueParts();
            _showGreyButton.Init();
            _hintButton.Init(_puzzleController);
            _background.SetAsFirstSibling();
        }
    }
}