using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text TimerText;
    public TMP_Text GameClearText;
    public TMP_Text GameOverText;

    [Header("Timer")]
    float limitTime = 3f;

    bool isGameOver = false;
    bool isRunning = false;
    bool isFinished = false; // ★ 追加：多重防止

    void Start()
    {
        GameClearText.gameObject.SetActive(false);
        GameOverText.gameObject.SetActive(false);
        TimerText.text = limitTime.ToString("F0");
    }

    void Update()
    {
        if (isGameOver || !isRunning) return;

        limitTime -= Time.deltaTime;

        if (limitTime <= 0f)
        {
            limitTime = 0f;
            GameOver();
        }

        TimerText.text = limitTime.ToString("F0");
    }

    public void GameStart()
    {
        isRunning = true;
    }

    // -------- 失敗 --------
    public void GameOver()
    {
        if (isFinished) return;
        isFinished = true;

        isGameOver = true;
        GameOverText.gameObject.SetActive(true);

        // ★ 共通フローへ報告
        GameFlowState.hasLastResult = true;
        GameFlowState.lastWin = false;
        GameFlowManager.Instance.ReportFail();
    }

    // -------- クリア --------
    public void GameClear()
    {
        if (isFinished) return;
        isFinished = true;

        isGameOver = true;
        GameClearText.gameObject.SetActive(true);

        // ★ 共通フローへ報告
        GameFlowState.hasLastResult = true;
        GameFlowState.lastWin = true;
        GameFlowManager.Instance.ReportClear();
    }
}
