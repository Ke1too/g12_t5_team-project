using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance;

    public int clears = 0;       // クリア回数
    public int fails = 0;        // 失敗回数

    public int maxClears = 10;   // クリアでステージセレクトに戻る回数
    public int maxFails = 3;     // 失敗でステージセレクトに戻る回数

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン跨ぎで保持
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ReportClear()
    {
        clears++;
        CheckReturnToStageSelect();
    }

    public void ReportFail()
    {
        fails++;
        CheckReturnToStageSelect();
    }

    private void CheckReturnToStageSelect()
    {
        if (clears >= maxClears || fails >= maxFails)
        {
            // ステージセレクトに戻す
            clears = 0;
            fails = 0;
            SceneManager.LoadScene("StageSelectScene");
        }
        else
        {
            // ReadyScene に戻す
            SceneManager.LoadScene("ReadyScene");
        }
    }
}