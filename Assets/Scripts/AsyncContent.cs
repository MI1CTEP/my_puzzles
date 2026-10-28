using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

namespace MyGame
{
    public static class AsyncContent
    {
        public static void LoadPuzzleSprite(int idGroup, int id, UnityAction<Sprite> onEndLoad)
        {
            string key = $"puzzle_{idGroup + 1}_{id + 1}";
            Addressables.LoadAssetAsync<Sprite>(key).Completed += (asyncOperation) =>
            {
                onEndLoad?.Invoke(asyncOperation.Result);
            };
        }

        public static void LoadSuperPuzzleSprite(int id, UnityAction<Sprite> onEndLoad)
        {
            string key = $"superpuzzle_{id + 1}";
            Addressables.LoadAssetAsync<Sprite>(key).Completed += (asyncOperation) =>
            {
                onEndLoad?.Invoke(asyncOperation.Result);
            };
        }

        public static void ReleaseSprite(Sprite sprite)
        {
            Addressables.Release(sprite);
        }
    }
}