using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame.Menu
{
    public sealed class MenuController : MonoBehaviour
    {
        [SerializeField] private Settings _settings;
        [SerializeField] private Button _buttonSettings;
        [SerializeField] private SuperPuzzleController _superPuzzleController;
        [SerializeField] private PuzzleItem _puzzleItemPrefab;
        [SerializeField] private PuzzlePanel _puzzlePanel;
        [SerializeField] private RandomPuzzle _randomPuzzle;
        [SerializeField] private AllPuzzles _allPuzzles;

        private readonly Stack<PuzzleItem> _puzzleItems = new();

        private void Start()
        {
            Input.multiTouchEnabled = false;
            Application.targetFrameRate = int.MaxValue;

            PuzzleData.Init();
            _settings.TryInstantiate();
            _buttonSettings.onClick.AddListener(Settings.Instance.Show);
            _buttonSettings.onClick.AddListener(SoundsController.Instance.PlayButtonClick);
            _superPuzzleController.Init();
            _randomPuzzle.Init(this, _puzzlePanel);
            _allPuzzles.Init(this);
            _puzzlePanel.Init(_allPuzzles);
        }

        public PuzzleItem GetPuzzleItem()
        {
            PuzzleItem puzzleItem;
            if(_puzzleItems.Count == 0)
            {
                puzzleItem = Instantiate(_puzzleItemPrefab, transform);
                puzzleItem.Init(_puzzlePanel);
            }
            else
            {
                puzzleItem = _puzzleItems.Pop();
                puzzleItem.gameObject.SetActive(true);
            }
            return puzzleItem;
        }

        public void ReturnPuzzleItem(PuzzleItem puzzleItem)
        {
            puzzleItem.gameObject.SetActive(false);
            puzzleItem.transform.SetParent(transform);
            _puzzleItems.Push(puzzleItem);
        }
    }
}   