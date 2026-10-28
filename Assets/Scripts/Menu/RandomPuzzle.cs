using UnityEngine;
using UnityEngine.UI;

namespace MyGame.Menu
{
    public sealed class RandomPuzzle : MonoBehaviour
    {
        [SerializeField] private GameObject _allPuzzlesOpened;
        [SerializeField] private LocalizationText _getText;
        [SerializeField] private GameObject _noCapacity;

        private MenuController _menuController;
        private PuzzlePanel _puzzlePanel;
        private Button _getButton;

        public void Init(MenuController menuController, PuzzlePanel puzzlePanel)
        {
            _menuController = menuController;
            _puzzlePanel = puzzlePanel;
            _getButton = GetComponent<Button>();
            _getButton.onClick.AddListener(Get);
            _getButton.onClick.AddListener(SoundsController.Instance.PlayButtonClick);
            CheckPuzzles();
        }

        public void Get()
        {
            int idGroup = GetRandomPuzzleGroup();
            int id = GetRandomPuzzle(idGroup);

            PuzzleItem puzzleItem = _menuController.GetPuzzleItem();
            puzzleItem.SetSprite(idGroup, id);

            _puzzlePanel.Show(puzzleItem, true);

            CheckPuzzles();
        }

        private void CheckPuzzles()
        {
            bool isHavePuzzles = false;
            int openedPuzzles = 0;
            for (int i = 0; i < PuzzleData.Groups.Length; i++)
            {
                openedPuzzles += PuzzleData.Groups[i].openedIds.Count;
                if (PuzzleData.Groups[i].notOpenedIds.Count > 0)
                    isHavePuzzles = true;
            }
            if(isHavePuzzles && openedPuzzles < 5)
            {
                gameObject.SetActive(true);
                _allPuzzlesOpened.SetActive(false);
                _noCapacity.SetActive(false);
            }
            else if (!isHavePuzzles)
            {
                gameObject.SetActive(false);
                _allPuzzlesOpened.SetActive(true);
                _noCapacity.SetActive(false);
            }
            else
            {
                gameObject.SetActive(false);
                _allPuzzlesOpened.SetActive(false);
                _noCapacity.SetActive(true);
            }
            _getText.SetReplaced($"{5 - openedPuzzles}/5");
        }

        private int GetRandomPuzzleGroup()
        {
            int maxGroups = PuzzleData.Count.Length - 1;
            if (PuzzleData.ReceivedPuzzles <= 1) maxGroups = 0;
            else if (PuzzleData.ReceivedPuzzles <= 2) maxGroups = 1;
            else if (PuzzleData.ReceivedPuzzles <= 3) maxGroups = 2;

            int totalWeight = 0;
            int multiplier = 1;
            for (int i = maxGroups; i >= 0; i--)
            {
                totalWeight += multiplier * PuzzleData.Groups[i].notOpenedIds.Count;
                multiplier *= 2;
            }

            int random = Random.Range(0, totalWeight);
            int currentWeight = 0;
            multiplier = 1;
            for (int i = maxGroups; i >= 0; i--)
            {
                currentWeight += multiplier * PuzzleData.Groups[i].notOpenedIds.Count;

                if (random < currentWeight) 
                    return i;

                multiplier *= 2;
            }

            return 0;
        }

        private int GetRandomPuzzle(int idGroup)
        {
            int random = Random.Range(0, PuzzleData.Groups[idGroup].notOpenedIds.Count);
            int id = PuzzleData.Groups[idGroup].notOpenedIds[random];
            PuzzleData.SetStatus(idGroup, id, 1);
            PuzzleData.AddReceivedPuzzle();
            return id;
        }
    }
}