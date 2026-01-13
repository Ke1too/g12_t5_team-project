using UnityEngine;
using UnityEngine.SceneManagement;

public class FusumaSceneLoader : MonoBehaviour
{
    [Header("遷移するミニゲームシーン")]
    [Tooltip("Build Settings に登録済みのシーン名を入れてください")]
    public string[] miniGameSceneNames;

    // AnimationEvent から呼ばれる
    public void LoadNextScene()
    {
        if (miniGameSceneNames == null || miniGameSceneNames.Length == 0)
        {
            Debug.LogError("ミニゲームシーンが設定されていません！");
            return;
        }

        int index = Random.Range(0, miniGameSceneNames.Length);
        string sceneName = miniGameSceneNames[index];

        Debug.Log($"次のシーンへ遷移: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
}
