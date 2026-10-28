using UnityEngine;
using UnityEngine.UI;

public class MyImage : MonoBehaviour
{
    [SerializeField] private Color[] _colors;

    private Material _material;

    private void Start()
    {
        Image img = GetComponent<Image>();

        _material = new Material(img.material);
        img.material = _material;
        ChangeImage();
    }

    public void ChangeImage()
    {
        Vector4[] paletteVec = new Vector4[10];

        for (int i = 0; i < paletteVec.Length; i++)
        {
            int colorId = i;
            if (i >= _colors.Length) colorId = _colors.Length - 1;
            paletteVec[i] = _colors[colorId];
        }

        _material.SetVectorArray("_Palette", paletteVec);
    }
}