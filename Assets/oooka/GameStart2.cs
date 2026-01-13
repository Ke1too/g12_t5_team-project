using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart2 : MonoBehaviour
{
    public GameObject instructionUI;
    public TextMeshProUGUI countdownText;
    public string nextSceneName = "QuizScene";

    void Start()
    {
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        int count = 3;

        while (count > 0)
        {
            countdownText.text = count.ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }

        countdownText.text = "START!";
        yield return new WaitForSeconds(0.5f);

        // UIを消す
        countdownText.gameObject.SetActive(false);
        instructionUI.SetActive(false);

        // クイズシーンへ
        
    }
}
