using UnityEngine;
using UnityEngine.SceneManagement; // ← これが絶対に必要です！

public class SceneController : MonoBehaviour
{
    // ボタンに割り当てる関数（publicにする必要があります）
    public void ChangeScene(string sceneName)
    {
        // 指定した名前のシーンを読み込む
        SceneManager.LoadScene(sceneName);
    }
}