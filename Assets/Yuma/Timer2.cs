using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer2 : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI TimerText;
    public float limitTime; //êßå¿éûä‘
    public GameManager2 gameManager;

    private void Start()
    {

    }

    private void Update()
    {
        // Åö !gameManager.isGameActive Çí«â¡
        if (!gameManager.isGameActive || gameManager.gameOver || gameManager.gameClear) return;
        {
            limitTime -= Time.deltaTime;

            if (limitTime <= 0)
            {
                gameManager.gameOver = true;
                Debug.Log("é∏îs");
            }

            TimerText.text = limitTime.ToString("F0");
        }
    }
}
