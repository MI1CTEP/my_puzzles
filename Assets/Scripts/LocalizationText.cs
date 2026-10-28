using UnityEngine;
using UnityEngine.UI;
using YG;

namespace MyGame
{
    public sealed class LocalizationText : MonoBehaviour
    {
        [SerializeField] private string _ru;
        [SerializeField] private string _en;
        [SerializeField] private bool _isReplaceable;

        private Text _text;
        private string _current;
        private string _replaced = "";

        private void Awake()
        {
            _text = GetComponent<Text>();

            YG2.onSwitchLang += SwitchLanguage;
            SwitchLanguage(YG2.lang);
        }

        public void SetReplaced(string replaced)
        {
            _replaced = replaced;
            ReplaceText();
        }

        private void OnDestroy()
        {
            YG2.onSwitchLang -= SwitchLanguage;
        }

        private void SwitchLanguage(string lang)
        {
            _current = lang switch
            {
                "ru" => _ru,
                _ => _en,
            };
            ReplaceText();
        }

        private void ReplaceText()
        {
            if (_isReplaceable)
                _text.text = _current.Replace("$$$", _replaced);
            else
                _text.text = _current;
        }
    }
}