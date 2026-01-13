using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStart_reverse : MonoBehaviour
{
    public GameObject instructionUI;
    public Timer_reverse timer;
    public TextMeshProUGUI countdownText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(UpdateGameStartRoutine());
    }

    // Update is called once per frame
    IEnumerator UpdateGameStartRoutine()
    {
        int currentCount = 3;

        while (currentCount > 0)
        {
            if (countdownText != null)
            {
                countdownText.text = currentCount.ToString(); // 数字を更新
            }

            yield return new WaitForSeconds(1.0f); // 1秒待つ
            currentCount--;
        }

        if (countdownText != null) countdownText.text = "始め!!";
        yield return new WaitForSeconds(0.5f); // START!を少しだけ見せる

        if (countdownText != null) countdownText.gameObject.SetActive(false); // テキストを消す
        instructionUI.SetActive(false); // 指令UIを消す
        timer.GameStart();
    }
}

