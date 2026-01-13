using UnityEngine;
using TMPro;

public class Timer_reverse : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text gameClearText;
    [SerializeField] TMP_Text gameOverText;

    [Header("Timer")]
    [SerializeField] float limitTime = 3f;

    bool isGameOver = false;
    bool isRunning = false;
    bool isFinished = false; // ★ 多重呼び出し防止

    void Start()
    {
        if (gameClearText != null) gameClearText.gameObject.SetActive(false);
        if (gameOverText != null) gameOverText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isRunning || isFinished) return;

        limitTime -= Time.deltaTime;

        if (limitTime <= 0f)
        {
            limitTime = 0f;
            GameClear();
        }

        if (timerText != null)
            timerText.text = limitTime.ToString("F0");
    }

    // -------- ゲーム開始 --------
    public void GameStart()
    {
        isRunning = true;
    }

    // -------- ゲームオーバー --------
    public void GameOver()
    {
        if (isFinished) return;
        isFinished = true;
        isRunning = false;

        if (gameOverText != null)
            gameOverText.gameObject.SetActive(true);

        // ★ 共通フローへ報告
        GameFlowState.hasLastResult = true;
        GameFlowState.lastWin = false;
        GameFlowManager.Instance.ReportFail();
    }

    // -------- ゲームクリア --------
    void GameClear()
    {
        if (isFinished) return;
        isFinished = true;
        isRunning = false;

        if (gameClearText != null)
            gameClearText.gameObject.SetActive(true);

        // ★ 共通フローへ報告
        GameFlowState.hasLastResult = true;
        GameFlowState.lastWin = true;
        GameFlowManager.Instance.ReportClear();
    }
}
