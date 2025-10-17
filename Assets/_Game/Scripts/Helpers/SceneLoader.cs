using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Scripts.Helpers
{
    public class SceneLoader
    {
        private Scene _loadedScene;
    
        public async UniTask LoadScene(int index)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
            asyncOperation.allowSceneActivation = false;
        
            _loadedScene = SceneManager.GetSceneByBuildIndex(index);

            while (asyncOperation.isDone == false)
            {
                if (asyncOperation.progress >= 0.9f)
                {
                    asyncOperation.allowSceneActivation = true;
                }

                await UniTask.Yield();
            }
        
            SceneManager.SetActiveScene(_loadedScene);
        }

        public async UniTask UnloadScene()
        {
            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(_loadedScene);

            while (asyncOperation.isDone == false)
                await UniTask.Yield();

            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(0));
            DOTween.KillAll();
        }
    }
}