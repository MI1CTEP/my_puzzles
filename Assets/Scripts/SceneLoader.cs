using UnityEngine.SceneManagement;
using YG;

namespace MyGame
{
    public static class SceneLoader
    {
        public static void LoadMenu() => Load("MenuScene");

        public static void LoadGameplay() => Load("GameplayScene");

        private static void Load(string sceneName)
        {
            YG2.InterstitialAdvShow();
            SceneManager.LoadScene(sceneName);
        }
    }
}