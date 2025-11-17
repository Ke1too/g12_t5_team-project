using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class StartButton : MonoBehaviour
{
    public TextMeshProUGUI startText;
    public float scaleSpeed = 1.5f;
    public float scaleAmount = 0.05f;

    Vector3 baseScale;

    void Start()
    {
        startText.text = "Press <b><color=#FFD700>ENTER</color></b> to Start";
        baseScale = startText.rectTransform.localScale;
    }

    void Update()
    {
        // 拡大縮小アニメーション
        float scale = 1 + Mathf.Sin(Time.time * scaleSpeed) * scaleAmount;
        startText.rectTransform.localScale = baseScale * scale;

        if (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("StageSelectScene");
        }
    }
}
