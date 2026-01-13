using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager2 : MonoBehaviour
{
    public bool gameClear;
    public bool gameOver;
    public bool isGameActive = false;

    [Header("UI Settings")]
    public GameObject gameClearText;
    public GameObject gameOverText;
    public GameObject instructionUI;
    public TextMeshProUGUI countdownText;

    [Header("Game Settings")]
    public int waitTime = 3;

    private bool isFinished = false; // ★ 追加：多重呼び出し防止

    void Start()
    {
        gameClearText.SetActive(false);
        gameOverText.SetActive(false);
        instructionUI.SetActive(true);

        if (countdownText != null)
            countdownText.text = waitTime.ToString();

        isGameActive = false;
        StartCoroutine(GameStartRoutine());
    }

    IEnumerator GameStartRoutine()
    {
        int currentCount = waitTime;

        while (currentCount > 0)
        {
            if (countdownText != null)
                countdownText.text = currentCount.ToString();

            yield return new WaitForSeconds(1.0f);
            currentCount--;
        }

        if (countdownText != null)
            countdownText.text = "開始!!";

        yield return new WaitForSeconds(0.5f);

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);

        instructionUI.SetActive(false);
        isGameActive = true;
    }

    void Update()
    {
        if (!isGameActive || isFinished) return;

        // -------- クリア --------
        if (gameClear)
        {
            isFinished = true;
            isGameActive = false;

            gameClearText.SetActive(true);

            // ★ 共通フローへ報告
            GameFlowState.hasLastResult = true;
            GameFlowState.lastWin = true;
            GameFlowManager.Instance.ReportClear();
        }

        // -------- ゲームオーバー --------
        if (gameOver)
        {
            isFinished = true;
            isGameActive = false;

            gameOverText.SetActive(true);

            // ★ 共通フローへ報告
            GameFlowState.hasLastResult = true;
            GameFlowState.lastWin = false;
            GameFlowManager.Instance.ReportFail();
        }
    }
}
