using UnityEngine;

namespace MyGame
{
    public sealed class SoundsController : MonoBehaviour
    {
        [SerializeField] private AudioClip _backgroundMusic;
        [SerializeField] private AudioClip _buttonClick;
        [SerializeField] private AudioClip _levelClick;
        [SerializeField] private AudioClip _show;
        [SerializeField] private AudioClip _puzzleDown;
        [SerializeField] private AudioClip _puzzleMove;
        [SerializeField] private AudioClip _puzzleTrue;
        [SerializeField] private AudioClip _win;

        private AudioSource _oneShotSource;
        private AudioSource _musicSourse;

        public static SoundsController Instance { get; set; }

        private void Awake()
        {
            if (Instance != null) return;

            DontDestroyOnLoad(gameObject);

            _oneShotSource = gameObject.AddComponent<AudioSource>();
            _oneShotSource.playOnAwake = false;
            _oneShotSource.loop = false;

            _musicSourse = gameObject.AddComponent<AudioSource>();
            _musicSourse.playOnAwake = false;
            _musicSourse.loop = true;
            _musicSourse.clip = _backgroundMusic;
            _musicSourse.Play();

            Instance = this;
        }

        public void SetSoundVolume(float value) => _oneShotSource.volume = value;

        public void SetMusicVolume(float value) => _musicSourse.volume = 0.5f * value;

        public void PlayButtonClick()
        {
            _oneShotSource.pitch = 1f;
            PlayOneShot(_buttonClick);
        }

        public void PlayLevelClick()
        {
            _oneShotSource.pitch = 1f;
            PlayOneShot(_levelClick);
        }

        public void PlayShow()
        {
            _oneShotSource.pitch = 1f;
            PlayOneShot(_show);
        }

        public void PlayPuzzleDown()
        {
            _oneShotSource.pitch = 1f;
            PlayOneShot(_puzzleDown);
        }

        public void PlayPuzzleUp()
        {
            _oneShotSource.pitch = 0.85f;
            PlayOneShot(_puzzleDown);
        }

        public void PlayPuzzleMove()
        {
            _oneShotSource.pitch = Random.Range(0.9f, 1.1f);
            PlayOneShot(_puzzleMove);
        }

        public void PlayPuzzleTrue()
        {
            _oneShotSource.pitch = 1;
            PlayOneShot(_puzzleTrue);
        }

        public void PlayWin()
        {
            _oneShotSource.pitch = 1;
            PlayOneShot(_win);
        }

        private void PlayOneShot(AudioClip audioClip)
        {
            if (Instance == null) return;
            _oneShotSource.PlayOneShot(audioClip);
        }
    }
}