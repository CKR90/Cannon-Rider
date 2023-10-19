using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoaderAsync : MonoBehaviour
{
    public static SceneLoaderAsync Instance;

    private string sceneName;
    private string currentSceneName;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
    }
    public void LoadSceneAsync(string sceneName)
    {
        this.sceneName = sceneName;
        currentSceneName = SceneManager.GetActiveScene().name;
        StartCoroutine(DelayedLoadAndUnloadScenesAsyncCoroutine());
    }

    private IEnumerator DelayedLoadAndUnloadScenesAsyncCoroutine()
    {
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(LoadAndUnloadScenesAsyncCoroutine());
    }


    private IEnumerator LoadAndUnloadScenesAsyncCoroutine()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(currentSceneName);

        while (loadOperation != null && !loadOperation.isDone || unloadOperation != null && !unloadOperation.isDone)
        {
            yield return null;
        }
    }
}