using System.Collections;
using System.Collections.Generic;
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
    public GameObject instructionUI; // 指令UI（flame）
    public TextMeshProUGUI countdownText; // ★追加：カウントダウン表示用のテキスト

    [Header("Game Settings")]
    public int waitTime = 3; // ★ floatからintに変更（3, 2, 1と数えるため）

    private void Start()
    {
        gameClearText.SetActive(false);
        gameOverText.SetActive(false);
        instructionUI.SetActive(true);

        // カウントダウンテキストの初期化（空にするか、最初の数字を入れる）
        if (countdownText != null) countdownText.text = waitTime.ToString();

        isGameActive = false;
        StartCoroutine(GameStartRoutine());
    }

    IEnumerator GameStartRoutine()
    {
        int currentCount = waitTime;

        while (currentCount > 0)
        {
            if (countdownText != null)
            {
                countdownText.text = currentCount.ToString(); // 数字を更新
            }

            yield return new WaitForSeconds(1.0f); // 1秒待つ
            currentCount--;
        }

        // 最後に「スタート！」と出す場合はここに追加（任意）
        if (countdownText != null) countdownText.text = "始め!!";
        yield return new WaitForSeconds(0.5f); // START!を少しだけ見せる

        if (countdownText != null) countdownText.gameObject.SetActive(false); // テキストを消す
        instructionUI.SetActive(false); // 指令UIを消す
        isGameActive = true;
    }

    private void Update()
    {
        if (!isGameActive) return;

        if (gameClear)
        {
            gameClearText.SetActive(true);
            isGameActive = false;
        }

        if (gameOver)
        {
            gameOverText.SetActive(true);
            isGameActive = false;
        }
    }
}