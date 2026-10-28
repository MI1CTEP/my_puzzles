using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

namespace MyGame.Menu
{
    public sealed class AllPuzzles : MonoBehaviour
    {
        [SerializeField] private Button[] _switchButtons;
        [SerializeField] private Sprite _active;
        [SerializeField] private Sprite _passive;
        [SerializeField] private Transform _puzzleItemsParent;

        private MenuController _menuController;
        private Image[] _switchImages;
        private RectTransform[] _switchRect;
        private Vector2 _startSizeDelta;
        private List<PuzzleItem> _puzzleItems = new();
        private int _currentId;

        public void Init(MenuController menuController)
        {
            _menuController = menuController;

            _switchImages = new Image[_switchButtons.Length];
            _switchRect = new RectTransform[_switchButtons.Length];
            for (int i = 0; i < _switchButtons.Length; i++)
            {
                int id = i;
                _switchButtons[i].onClick.AddListener(() => SwitchTo(id));
                _switchButtons[i].onClick.AddListener(SoundsController.Instance.PlayButtonClick);
                _switchImages[i] = _switchButtons[i].GetComponent<Image>();
                _switchRect[i] = _switchButtons[i].GetComponent<RectTransform>();
            }
            _switchImages[0].sprite = _active;
            _startSizeDelta = _switchRect[0].sizeDelta;
            _switchRect[0].sizeDelta = _startSizeDelta * 1.2f;
            UpdatePuzzleItems(null);
        }

        public void SetNewItem(PuzzleItem puzzleItem)
        {
            if(puzzleItem.IdGroup == _currentId)
            {
                int idPosition = 0;
                for (int i = 0; i < _puzzleItems.Count; i++)
                {
                    if (puzzleItem.Id < _puzzleItems[i].Id)
                        break;
                    idPosition++;
                }
                _puzzleItems.Insert(idPosition, puzzleItem);
                puzzleItem.transform.SetParent(_puzzleItemsParent);
                puzzleItem.transform.SetSiblingIndex(idPosition + 2);
            }
            else
            {
                SwitchTo(puzzleItem.IdGroup, puzzleItem);
            }
        }

        private void SwitchTo(int id, PuzzleItem puzzleItem = null)
        {
            if (id == _currentId) return;

            _switchImages[_currentId].sprite = _passive;
            _switchRect[_currentId].sizeDelta = _startSizeDelta;

            _switchImages[id].sprite = _active;
            _switchRect[id].sizeDelta = _startSizeDelta * 1.2f;

            _currentId = id;

            UpdatePuzzleItems(puzzleItem);
        }

        private void UpdatePuzzleItems(PuzzleItem puzzleItem)
        {
            for (int i = 0; i < _puzzleItems.Count; i++)
                _menuController.ReturnPuzzleItem(_puzzleItems[i]);
            _puzzleItems.Clear();

            List<int> puzzleItemIds = PuzzleData.Groups[_currentId].openedIds.OrderBy(x => x).ToList();
            SetItems(puzzleItemIds, puzzleItem);

            puzzleItemIds = PuzzleData.Groups[_currentId].completedIds.OrderBy(x => x).ToList();
            SetItems(puzzleItemIds, puzzleItem);
        }

        private void SetItems(List<int> puzzleItemIds, PuzzleItem puzzleItem)
        {
            PuzzleItem newPuzzleItem;
            for (int i = 0; i < puzzleItemIds.Count; i++)
            {
                if(puzzleItem != null && puzzleItem.Id == puzzleItemIds[i])
                {
                    puzzleItem.transform.SetParent(_puzzleItemsParent);
                    _puzzleItems.Add(puzzleItem);
                }
                else
                {
                    newPuzzleItem = _menuController.GetPuzzleItem();
                    newPuzzleItem.SetParent(_puzzleItemsParent);
                    newPuzzleItem.SetSprite(_currentId, puzzleItemIds[i]);
                    _puzzleItems.Add(newPuzzleItem);
                }
            }
        }
    }
}