using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace MyGame.Gameplay
{
    public sealed class PuzzleBackground : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Image _light;

        private ProgressPanel _progressPanel;
        private Image _image;
        private Sequence _seq;

        public void Init(ProgressPanel progressPanel, Sprite sprite)
        {
            _light.gameObject.SetActive(false);
            _progressPanel = progressPanel;
            _image = GetComponent<Image>();
            _image.sprite = sprite;
            _image.SetNativeSize();

            _progressPanel.OnFullProgress += EndAnim;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _seq.Kill();
            _seq = DOTween.Sequence();
            _seq.Insert(0, _image.DOColor(new Color(0.5f, 0.5f, 0.5f, 1), 0.3f));
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _seq.Kill();
            _seq = DOTween.Sequence();
            _seq.Insert(0, _image.DOColor(new Color(0.9f, 0.9f, 0.9f, 1), 0.3f));
        }

        private void EndAnim()
        {
            _light.gameObject.SetActive(true);
            _light.color = new Color(1, 1, 1, 0);
            _seq.Kill();
            _seq = DOTween.Sequence();
            _seq.Insert(0, _light.DOFade(0.3f, 0.5f));
            _seq.Insert(0.5f, _light.DOFade(0, 0.5f));
            _seq.InsertCallback(1, () => _light.gameObject.SetActive(false));
        }

        private void OnDestroy()
        {
            _progressPanel.OnFullProgress -= EndAnim;
            _seq.Kill();
        }
    }
}