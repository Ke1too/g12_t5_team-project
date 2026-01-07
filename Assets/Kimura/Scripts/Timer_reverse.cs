using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer_reverse : MonoBehaviour
{
    [SerializeField]
    public TMP_Text TimerText;
    public TMP_Text GameClearText;
    public TMP_Text GameOverText;
    float limitTime = 3; // êßå¿éûä‘
    bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        GameClearText.gameObject.SetActive(false);
        GameOverText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver) return;
        limitTime -= Time.deltaTime;

        if (limitTime < 0)
        {
            limitTime = 0;
            GameClear();
        }

        TimerText.text = limitTime.ToString("F0");
    }

    public void GameOver()
    {
        isGameOver = true;
        GameOverText.gameObject.SetActive(true);
    }

    public void GameClear()
    {
        isGameOver = true;
        GameClearText.gameObject.SetActive(true);
    }
}