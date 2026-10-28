using System;
using UnityEngine;
using UnityEngine.UI;
#if TMP_YG2
using TMPro;
#endif

namespace YG
{
    public class LBPlayerDataYG : MonoBehaviour
    {
        public ImageLoadYG imageLoad;

        [Serializable]
        public struct TextLegasy
        {
            public Text rank, name, score;
        }
        public TextLegasy textLegasy;

#if TMP_YG2
        [Serializable]
        public struct TextMP
        {
            public TextMeshProUGUI rank, name, score;
        }
        public TextMP textMP;
#endif
        [Space(10)]
        public MonoBehaviour[] topPlayerActivityComponents = new MonoBehaviour[0];
        public MonoBehaviour[] currentPlayerActivityComponents = new MonoBehaviour[0];
        
        [SerializeField] private Sprite _currentBackground;
        [SerializeField] private Color _currentColor;
        [SerializeField] private Color _topColor;

        public class Data
        {
            public string rank;
            public string name;
            public string score;
            public string photoUrl;
            public bool inTop;
            public bool currentPlayer;
            public Sprite photoSprite;
        }

        [HideInInspector]
        public Data data = new Data();

        public void UpdateEntries()
        {
            if (textLegasy.rank && data.rank != null) textLegasy.rank.text = data.rank.ToString();
            if (textLegasy.name && data.name != null) textLegasy.name.text = data.name;
            if (textLegasy.score && data.score != null) textLegasy.score.text = data.score.ToString();

#if TMP_YG2
            if (textMP.rank && data.rank != null) textMP.rank.text = data.rank.ToString();
            if (textMP.name && data.name != null) textMP.name.text = data.name;
            if (textMP.score && data.score != null) textMP.score.text = data.score.ToString();
#endif
            if (imageLoad)
            {
                if (data.photoSprite)
                {
                    imageLoad.SetTexture(data.photoSprite.texture);
                }
                else if (data.photoUrl == null)
                {
                    imageLoad.ClearTexture();
                }
                else
                {
                    imageLoad.Load(data.photoUrl);
                }
            }

            MyLogic();

            //if (topPlayerActivityComponents.Length > 0)
            //{
            //    if (data.inTop)
            //    {
            //        ActivityMomoObjects(topPlayerActivityComponents, true);
            //    }
            //    else
            //    {
            //        ActivityMomoObjects(topPlayerActivityComponents, false);
            //    }
            //}

            //if (currentPlayerActivityComponents.Length > 0)
            //{
            //    if (data.currentPlayer)
            //    {
            //        ActivityMomoObjects(currentPlayerActivityComponents, true);
            //    }
            //    else
            //    {
            //        ActivityMomoObjects(currentPlayerActivityComponents, false);
            //    }
            //}

            //void ActivityMomoObjects(MonoBehaviour[] objects, bool activity)
            //{
            //    for (int i = 0; i < objects.Length; i++)
            //    {
            //        objects[i].enabled = activity;
            //    }
            //}
        }

        private void MyLogic()
        {
            if (data.currentPlayer)
            {
                GetComponent<RectTransform>().sizeDelta += new Vector2(20, 0);
                GetComponent<Image>().sprite = _currentBackground;

                textLegasy.rank.color = _currentColor;
                textLegasy.rank.fontStyle = FontStyle.Bold;

                textLegasy.name.color = _currentColor;
                textLegasy.name.fontStyle = FontStyle.Bold;

                textLegasy.score.color = _currentColor;
                textLegasy.score.fontStyle = FontStyle.Bold;
            }
            else if (data.inTop)
            {
                textLegasy.rank.color = _topColor;
                textLegasy.name.color = _topColor;
                textLegasy.score.color = _topColor;
            }
        }
    }
}