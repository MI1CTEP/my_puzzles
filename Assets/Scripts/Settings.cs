using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using YG;

namespace MyGame
{
    public sealed class Settings : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private Transform _panel;
        [SerializeField] private Button _closeButton;

        [SerializeField] private Button _downSoundButton;
        [SerializeField] private Button _upSoundButton;
        [SerializeField] private Text _currentSoundText;
        private int _soundValue;
        private readonly string _soundKey = "sound_volume";

        [SerializeField] private Button _downMusicButton;
        [SerializeField] private Button _upMusicButton;
        [SerializeField] private Text _currentMusicText;
        private int _musicValue;
        private readonly string _musicKey = "music_volume";

        [SerializeField] private Button _ruButton;
        [SerializeField] private Button _enButton;
        [SerializeField] private Image _ruOutline;
        [SerializeField] private Image _enOutline;

        private Sequence _seq;
        private readonly float _timeAnim = 0.3f;

        public static Settings Instance { get; set; }

        public void TryInstantiate()
        {
            if (Instance != null) return;

            Instance = Instantiate(this);
            Instance.Init();
        }

        private void Init()
        {
            gameObject.SetActive(false);
            _closeButton.onClick.AddListener(Hide);
            _closeButton.onClick.AddListener(SoundsController.Instance.PlayButtonClick);
            DontDestroyOnLoad(gameObject);

            if (PlayerPrefs.HasKey(_soundKey)) ChangeSound(PlayerPrefs.GetInt(_soundKey));
            else ChangeSound(7);
            _downSoundButton.onClick.AddListener(() => ChangeSound(-1));
            _downSoundButton.onClick.AddListener(SoundsController.Instance.PlayButtonClick);
            _upSoundButton.onClick.AddListener(() => ChangeSound(1));
            _upSoundButton.onClick.AddListener(SoundsController.Instance.PlayButtonClick);

            if (PlayerPrefs.HasKey(_musicKey)) ChangeMusic(PlayerPrefs.GetInt(_musicKey));
            else ChangeMusic(7);
            _downMusicButton.onClick.AddListener(() => ChangeMusic(-1));
            _downMusicButton.onClick.AddListener(SoundsController.Instance.PlayButtonClick);
            _upMusicButton.onClick.AddListener(() => ChangeMusic(1));
            _upMusicButton.onClick.AddListener(SoundsController.Instance.PlayButtonClick);

            _ruButton.onClick.AddListener(() => SwitchLanguage("ru"));
            _enButton.onClick.AddListener(() => SwitchLanguage("en"));
            ChangeLanguage(YG2.lang);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            Color color = _background.color;
            color.a = 0;
            _background.color = color;
            _panel.localScale = Vector3.zero;

            _seq.Kill();
            _seq = DOTween.Sequence();
            _seq.Insert(0, _background.DOFade(0.9f, _timeAnim));
            _seq.Insert(0, _panel.DOScale(1, _timeAnim));
        }

        private void ChangeSound(int value)
        {
            _soundValue += value;
            PlayerPrefs.SetInt(_soundKey, _soundValue);
            SoundsController.Instance.SetSoundVolume(_soundValue / 10f);
            _currentSoundText.text = (_soundValue * 10).ToString();
            _downSoundButton.interactable = _soundValue > 0;
            _upSoundButton.interactable = _soundValue < 10;
        }

        private void ChangeMusic(int value)
        {
            _musicValue += value;
            PlayerPrefs.SetInt(_musicKey, _musicValue);
            SoundsController.Instance.SetMusicVolume(_musicValue / 10f);
            _currentMusicText.text = (_musicValue * 10).ToString();
            _downMusicButton.interactable = _musicValue > 0;
            _upMusicButton.interactable = _musicValue < 10;
        }

        private void SwitchLanguage(string lang)
        {
            SoundsController.Instance.PlayButtonClick();
            YG2.SwitchLanguage(lang);
            ChangeLanguage(lang);
        }

        private void ChangeLanguage(string lang)
        {
            switch (lang)
            {
                case "ru":
                    _ruButton.interactable = false;
                    _ruOutline.color = Color.green;
                    _enButton.interactable = true;
                    _enOutline.color = Color.gray;
                    break;
                default:
                    _ruButton.interactable = true;
                    _ruOutline.color = Color.gray;
                    _enButton.interactable = false;
                    _enOutline.color = Color.green;
                    break;
            }
        }

        private void Hide()
        {
            _seq.Kill();
            _seq = DOTween.Sequence();
            _seq.Insert(0, _background.DOFade(0, _timeAnim));
            _seq.Insert(0, _panel.DOScale(0, _timeAnim));
            _seq.InsertCallback(_timeAnim, () => gameObject.SetActive(false));
        }

        private void OnDestroy()
        {
            _seq.Kill();
        }
    }
}