using UnityEngine;
using UnityEngine.UI;

namespace MyGame 
{
    public sealed class SuperPuzzleProgress : MonoBehaviour
    {
        [SerializeField] private Image[] _steps;
        [SerializeField] private Text _progressText;
        [SerializeField] private Sprite _activeSprite;
        [SerializeField] private Sprite _passiveSprite;
        [SerializeField] private GameObject _openAllText;

        public void Init(int progress)
        {
            progress = SuperpuzzleData.GetExperience();
            progress %= 10;

            for (int i = 0; i < progress; i++)
                _steps[i].sprite = _activeSprite;

            for (int i = progress; i < 10; i++)
                _steps[i].sprite = _passiveSprite;

            _progressText.text = $"{progress}/10";

            if(SuperpuzzleData.GetExperience() == 100)
            {
                gameObject.SetActive(false);
                _openAllText.SetActive(true);
            }
        }
    }
}