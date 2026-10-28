using UnityEngine;
using UnityEngine.UI;

namespace MyGame.Gameplay
{
    public sealed class PuzzlePartsOutline : MonoBehaviour
    {
        private Image _image;
        private Texture2D _texture;

        public void Init(Vector2Int partsQuantuty, int partsSize)
        {
            _image = GetComponent<Image>();
            _image.GetComponent<RectTransform>().sizeDelta = partsQuantuty * partsSize;
            _texture = new Texture2D(partsQuantuty.x, partsQuantuty.y);
            _texture.filterMode = FilterMode.Point;
            _texture.wrapMode = TextureWrapMode.Clamp;
            for (int y = 0; y < _texture.height; y++)
                for (int x = 0; x < _texture.width; x++)
                    _texture.SetPixel(x, y, Color.black);
            ApplyTexture();

            PuzzlePart.OnSetTruePosition += Rebuild;
        }

        private void Rebuild(PuzzlePart puzzlePart)
        {
            _texture.SetPixel(puzzlePart.Id.x, puzzlePart.Id.y, Color.clear);
            ApplyTexture();
        }

        private void ApplyTexture()
        {
            _texture.Apply();
            _image.sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(0.5f, 0.5f));
        }

        private void OnDestroy()
        {
            PuzzlePart.OnSetTruePosition -= Rebuild;
        }
    }
}