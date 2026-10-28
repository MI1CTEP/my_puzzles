using UnityEngine;
using UnityEngine.UI;
using YG;

namespace MyGame.Gameplay
{
    public sealed class HintButton : MonoBehaviour
    {
        [SerializeField] private GameObject _hintIcon;
        [SerializeField] private GameObject _hintText;
        [SerializeField] private GameObject _advIcon;
        [SerializeField] private GameObject _advText;
        [SerializeField] private Text _hintValueText;

        private PuzzleController _puzzleController;
        private Button _button;
        private int _hint;

        public static bool IsUseHint { get; set; }

        public void Init(PuzzleController puzzleController)
        {
            IsUseHint = false;
            _puzzleController = puzzleController;
            _button = GetComponent<Button>();
            _button.onClick.AddListener(Show);
            _button.onClick.AddListener(SoundsController.Instance.PlayButtonClick);
            _hint = HintData.GetHint();
            _hintValueText.text = _hint.ToString();
            TrySwitchView();
        }

        private void Show()
        {
            if (PuzzlePart.IsHint) 
                return;

            if(_hint <= 0)
            {
                YG2.RewardedAdvShow("", GetReward);
            }
            else
            {
                IsUseHint = true;
                HintData.AddHint(-1);
                _hint--;
                _hintValueText.text = _hint.ToString();
                _puzzleController.ShowHint();
            }
            TrySwitchView();
        }

        private void GetReward()
        {
            _hint++;
            _hintValueText.text = _hint.ToString();
            HintData.AddHint(1);
            TrySwitchView();
        }

        private void TrySwitchView()
        {
            _hintIcon.SetActive(_hint > 0);
            _hintText.SetActive(_hint > 0);
            _hintValueText.gameObject.SetActive(_hint > 0);
            _advIcon.SetActive(_hint <= 0);
            _advText.SetActive(_hint <= 0);
        }
    }
}