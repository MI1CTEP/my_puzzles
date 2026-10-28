using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MyGame.Menu
{
    public sealed class PuzzleItem : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Transform _content;
        [SerializeField] private Image _pictureImage;
        [SerializeField] private Text _idText;
        [SerializeField] private Material _graiMaterial;

        private PuzzlePanel _puzzlePanel;
        private Sprite _lastSprite;
        private readonly float _startScale = 0.3515625f;

        public Transform Content => _content;
        public float StartScale => _startScale;
        public int IdGroup { get; private set; }
        public int Id { get; private set; }

        public void Init(PuzzlePanel puzzlePanel)
        {
            _puzzlePanel = puzzlePanel;
        }

        public void SetSprite(int idGroup, int id)
        {
            IdGroup = idGroup;
            Id = id;
            _idText.text = $"{id + 1}";
            if (PuzzleData.GetStatus(IdGroup, Id) != 2)
                _pictureImage.material = _graiMaterial;
            else
                _pictureImage.material = null;
            RemoveLastSprite();
            AsyncContent.LoadPuzzleSprite(idGroup, id, (Sprite sprite) =>
            {
                _lastSprite = sprite;
                _pictureImage.sprite = sprite;
            });
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
            _content.localScale = Vector3.one * _startScale;
            transform.localPosition = Vector3.zero;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SoundsController.Instance.PlayLevelClick();
            _puzzlePanel.Show(this, false);
        }

        private void RemoveLastSprite()
        {
            if(_lastSprite != null)
            {
                AsyncContent.ReleaseSprite(_lastSprite);
                _lastSprite = null;
            }
        }
    }
}